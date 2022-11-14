﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PTI.Microservices.Library.Models.SitemapService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Url
    {
        [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string Loc { get; set; }
        [XmlElement(ElementName = "lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string Lastmod { get; set; }
        [XmlElement(ElementName = "changefreq", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string Changefreq { get; set; }
        [XmlElement(ElementName = "priority", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string Priority { get; set; }
    }

    [XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SitemapInfo
    {
        [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public List<Url> Url { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemalocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Schemalocation { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
