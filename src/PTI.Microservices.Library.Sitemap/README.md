# PTI.Microservices.Library.Sitemap

This is part of PTI.Microservices.Library set of packages

The purpose of this package is to facilitate parsing Sitemaps, while maintaining a consistent usage pattern among the different services in the group

**Examples:**

## Expand Url
    SitemapService sitemapService = new SitemapService(null, new Microservices.Library.Interceptors.CustomHttpClient(
        new Microservices.Library.Interceptors.CustomHttpClientHandler(null)));
    foreach (var singleUrl in TEST_URLS)
    {
        var expandedUrl = await sitemapService.ExpandUrlAsync(singleUrl);
        Assert.AreNotEqual(singleUrl, expandedUrl);
    }

## Get Robots File
    SitemapService sitemapService = new SitemapService(null, new Microservices.Library.Interceptors.CustomHttpClient(
        new Microservices.Library.Interceptors.CustomHttpClientHandler(null)));
    foreach (var singleUrl in TEST_URLS.Except(urlsToExclude))
    {
                
        try
        {
            var expandedUrl = await sitemapService.ExpandUrlAsync(URL_TO_EXPAND);
            var robotsFileData = await sitemapService.GetRobotsFileAsync(new Uri(expandedUrl));
            Assert.AreNotEqual(singleUrl, expandedUrl);
            Assert.IsNotNull(robotsFileData.RootSitemaps);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }