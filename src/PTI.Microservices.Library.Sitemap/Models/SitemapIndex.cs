using System;
using System.Collections.Generic;
using System.Text;

namespace PTI.Microservices.Library.Models.SitemapService
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", IsNullable = false, ElementName ="sitemapindex")]
    public partial class SitemapIndex
    {

        private sitemapindexSitemap[] sitemapField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sitemap")]
        public sitemapindexSitemap[] sitemap
        {
            get
            {
                return this.sitemapField;
            }
            set
            {
                this.sitemapField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public partial class sitemapindexSitemap
    {

        private string locField;

        private System.DateTime lastmodField;

        /// <remarks/>
        public string loc
        {
            get
            {
                return this.locField;
            }
            set
            {
                this.locField = value;
            }
        }

        /// <remarks/>
        public System.DateTime lastmod
        {
            get
            {
                return this.lastmodField;
            }
            set
            {
                this.lastmodField = value;
            }
        }
    }


}
