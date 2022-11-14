using System;
using System.Collections.Generic;
using System.Text;

namespace PTI.Microservices.Library.Models.SitemapService
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRobotsFileResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string TextContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> RootSitemaps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SitemapInfo> SitemapsData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SitemapIndex> SitemapsIndexes { get; set; }
    }
}
