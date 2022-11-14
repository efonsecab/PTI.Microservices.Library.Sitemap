using Microsoft.Extensions.Logging;
using PTI.Microservices.Library.Interceptors;
using PTI.Microservices.Library.Models.SitemapService;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PTI.Microservices.Library.Services
{
    /// <summary>
    /// Service in charge of detecting Sitemap information
    /// </summary>
    public sealed class SitemapService
    {
        private ILogger<SitemapService> Logger { get; }
        private CustomHttpClient CustomHttpClient { get; }

        /// <summary>
        /// Creates a new intance of <see cref="SitemapService"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="customHttpClient"></param>
        public SitemapService(ILogger<SitemapService> logger, CustomHttpClient customHttpClient)
        {
            this.Logger = logger;
            this.CustomHttpClient = customHttpClient;
        }

        /// <summary>
        /// Received a shortened url and returns the expanded url
        /// </summary>
        /// <param name="shortenedUrl"></param>
        /// <exception cref="InvalidProgramException">asdasdad</exception>
        /// <returns></returns>
        public async Task<string> ExpandUrlAsync(string shortenedUrl)
        {
            try
            {
                var response = await this.CustomHttpClient.GetAsync(shortenedUrl);
                var expandedUrl = response.RequestMessage.RequestUri.ToString();
                return expandedUrl;
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.LogError(ex,
                        $"Unable to expand url: {shortenedUrl}. check your logs for more information");
                }
                throw;
            }
        }

        /// <summary>
        /// Finds the robots.txt file from a given url and returns its data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<GetRobotsFileResponse> GetRobotsFileAsync(Uri url)
        {
            string robotsFileUrl = $"{url.Scheme}://{url.Host}/robots.txt";
            var response = await this.CustomHttpClient.GetAsync(robotsFileUrl);
            if (response.IsSuccessStatusCode)
            {
                var robotsFileContent = await response.Content.ReadAsStringAsync();
                if (robotsFileContent.ToLower().Contains("sitemap:"))
                {
                    GetRobotsFileResponse result = await ParseRobotsFileContent(robotsFileContent);
                    return result;
                }
                else
                    throw new Exception("Robots file did not specify a sitemap");
            }
            else
            {
                string reason = response.ReasonPhrase;
                string detailedError = await response.Content.ReadAsStringAsync();
                throw new Exception($"Reason: {reason}. Details: {detailedError}");
            }
        }

        private async Task<GetRobotsFileResponse> ParseRobotsFileContent(string robotsFileContent)
        {
            GetRobotsFileResponse result = new GetRobotsFileResponse();
            result.TextContent = robotsFileContent;
            string currentTextLine = string.Empty;
            List<string> lstSitemapsUrls = new List<string>();
            using (System.IO.StringReader textReader = new System.IO.StringReader(robotsFileContent))
            {
                while ((currentTextLine = textReader.ReadLine()) != null)
                {
                    if (currentTextLine.ToLower().StartsWith("sitemap:"))
                    {
                        //Check robots.txt specifications, spaces are optional
                        //https://developers.google.com/search/reference/robots_txt?hl=en
                        var sitemapUrl = currentTextLine.Substring(8).TrimStart();
                        lstSitemapsUrls.Add(sitemapUrl);
                        byte[] sitemapBytes = null;
                        switch (System.IO.Path.GetExtension(sitemapUrl))
                        {
                            case ".gz":
                                sitemapBytes = await this.CustomHttpClient.GetByteArrayAsync(sitemapUrl);
                                using (MemoryStream sourceStream = new MemoryStream(sitemapBytes))
                                {
                                    using (MemoryStream destStream = new MemoryStream())
                                    {
                                        using (GZipStream gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                                        {
                                            await gZipStream.CopyToAsync(destStream);
                                        }
                                        //Check specification https://developers.google.com/search/reference/robots_txt?hl=en#file-format
                                        string sitemapUtf8String = Encoding.UTF8.GetString(destStream.ToArray());
                                        //lstSitemapsXmls.Add(sitemapUtf8String);
                                    }
                                }
                                break;
                            case ".xml":
                                var r = await this.CustomHttpClient.GetAsync(sitemapUrl);
                                if (!r.IsSuccessStatusCode)
                                {
                                    //urls such as https://www.exame.com/sitemap.xml return not found, but still return the data in the contents
                                    var content = await r.Content.ReadAsStringAsync();
                                }
                                string sitemapXml = await this.CustomHttpClient.GetStringAsync(sitemapUrl);
                                if (sitemapXml.Contains("<sitemapindex"))
                                {
                                    var sitemapIndex = this.GetSitemapIndex(sitemapXml);
                                    if (result.SitemapsIndexes == null)
                                        result.SitemapsIndexes = new List<SitemapIndex>();
                                }
                                else
                                {
                                    var sitemapInfo = this.GetSitemapInfo(sitemapXml);
                                    if (result.SitemapsData == null)
                                        result.SitemapsData = new List<SitemapInfo>();
                                    result.SitemapsData.Add(sitemapInfo);
                                }
                                break;
                        }
                    }
                }
            }
            result.RootSitemaps = lstSitemapsUrls;
            return result;
        }

        private SitemapIndex GetSitemapIndex(string sitemapContents)
        {
            SitemapIndex result = null;
            XmlSerializer serializer = new XmlSerializer(typeof(SitemapIndex));
            using (TextReader reader = new StringReader(sitemapContents))
            {
                result = serializer.Deserialize(reader) as SitemapIndex;
            }
            return result;
        }

        /// <summary>
        /// Retrieves the data from the sitemap in the specified url
        /// </summary>
        /// <param name="sitemapContents">Url of the sitemap xml file.</param>
        /// <returns></returns>
        public SitemapInfo GetSitemapInfo(string sitemapContents)
        {
            SitemapInfo result = null;
            XmlSerializer serializer = new XmlSerializer(typeof(SitemapInfo));
            using (TextReader reader = new StringReader(sitemapContents))
            {
                try
                {
                    result = serializer.Deserialize(reader) as SitemapInfo;
                }
                catch (Exception ex)
                {
                    this.Logger?.LogError(ex, ex.Message);
                    throw;
                }
            }
            return result;
        }
    }
}
