using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using DansTonChat.Html;
using System.Runtime.Serialization;

namespace DansTonChat.Rss
{
    [DataContract]
    public class FeedItem
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime PubDate { get; set; }
        [DataMember]
        public string Link { get; set; }

        public FeedItem(XElement item)
        {
            var feedburner = XNamespace.Get("http://rssnamespace.org/feedburner/ext/1.0");

            Title = item.Element("title").Value;
            Description = ParseDescription(item.Element("description").Value);
            PubDate = DateTime.Parse(item.Element("pubDate").Value);
            Link = item.Element(feedburner+"origLink").Value;
        }

        public string ParseDescription(string description)
        {
            var content = new Parser().ParseHtml(description);
            var trimmed = content.Text.Trim();
            if (trimmed.EndsWith("Votez !"))
            {
                return trimmed.Remove(trimmed.Length - "Votez !".Length);
            }
            else 
            {
                return trimmed;
            }

        }

        public XElement GetXML()
        {
            return
                new XElement("item",
                    new XElement("title", Title),
                    new XElement("description", Description),
                    new XElement("pubDate", PubDate.ToLongTimeString()));

        }
    }
}
