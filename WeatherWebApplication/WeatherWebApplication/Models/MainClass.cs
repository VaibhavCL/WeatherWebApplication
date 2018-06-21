using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WeatherWebApplication.Models
{
    public class MainClass
    {
        [XmlRoot(ElementName = "url")]
        public class Url
        {
            public string Executionstarttime { get; set; }
            public string Executionstoptime { get; set; }
            public string Executiontime { get; set; }
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "javascript")]
        public class Javascript
        {
            public string Executionstarttime { get; set; }
            public string Executionstoptime { get; set; }
            public string Executiontime { get; set; }
            public string Instructionsused { get; set; }
            public string Tablename { get; set; }
        }

        [XmlRoot(ElementName = "diagnostics")]
        public class Diagnostics
        {
            public string PubliclyCallable { get; set; }
            public List<Url> Url { get; set; }
            public Javascript Javascript { get; set; }
            public string Usertime { get; set; }
            public string Servicetime { get; set; }
            public string Buildversion { get; set; }
        }

        [XmlRoot(ElementName = "units", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Units
        {
            public string Yweather { get; set; }
            public string Distance { get; set; }
            public string Pressure { get; set; }
            public string Speed { get; set; }
            public string Temperature { get; set; }
        }

        [XmlRoot(ElementName = "location", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Location
        {
            public string Yweather { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }
        }

        [XmlRoot(ElementName = "wind", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Wind
        {
            public string Yweather { get; set; }
            public string Chill { get; set; }
            public string Direction { get; set; }
            public string Speed { get; set; }
        }

        [XmlRoot(ElementName = "atmosphere", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Atmosphere
        {
            public string Yweather { get; set; }
            public string Humidity { get; set; }
            public string Pressure { get; set; }
            public string Rising { get; set; }
            public string Visibility { get; set; }
        }

        [XmlRoot(ElementName = "astronomy", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Astronomy
        {
            [XmlAttribute(AttributeName = "yweather", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Yweather { get; set; }
            [XmlAttribute(AttributeName = "sunrise")]
            public string Sunrise { get; set; }
            [XmlAttribute(AttributeName = "sunset")]
            public string Sunset { get; set; }
        }

        [XmlRoot(ElementName = "image")]
        public class Image
        {
            public string Title { get; set; }
            public string Width { get; set; }
            public string Height { get; set; }
            public string Link { get; set; }
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "lat", Namespace = "http://www.w3.org/2003/01/geo/wgs84_pos#")]
        public class Lat
        {
            public string Geo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "long", Namespace = "http://www.w3.org/2003/01/geo/wgs84_pos#")]
        public class Long
        {
            public string Geo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "condition", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Condition
        {
            public string Yweather { get; set; }
            public string Code { get; set; }
            public string Date { get; set; }
            public string Temp { get; set; }
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "forecast", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
        public class Forecast
        {
            public string Yweather { get; set; }
            public string Code { get; set; }
            public string Date { get; set; }
            public string Day { get; set; }
            public string High { get; set; }
            public string Low { get; set; }
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "guid")]
        public class Guid
        {
            [XmlAttribute(AttributeName = "isPermaLink")]
            public string IsPermaLink { get; set; }
        }

        [XmlRoot(ElementName = "item")]
        public class Item
        {
            [XmlElement(ElementName = "title")]
            public string Title { get; set; }
            [XmlElement(ElementName = "lat", Namespace = "http://www.w3.org/2003/01/geo/wgs84_pos#")]
            public Lat Lat { get; set; }
            [XmlElement(ElementName = "long", Namespace = "http://www.w3.org/2003/01/geo/wgs84_pos#")]
            public Long Long { get; set; }
            [XmlElement(ElementName = "link")]
            public string Link { get; set; }
            [XmlElement(ElementName = "pubDate")]
            public string PubDate { get; set; }
            [XmlElement(ElementName = "condition", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
            public Condition Condition { get; set; }
            [XmlElement(ElementName = "forecast", Namespace = "http://xml.weather.yahoo.com/ns/rss/1.0")]
            public List<Forecast> Forecast { get; set; }
            [XmlElement(ElementName = "description")]
            public string Description { get; set; }
            [XmlElement(ElementName = "guid")]
            public Guid Guid { get; set; }
        }

        [XmlRoot(ElementName = "channel")]
        public class Channel
        {
            public Units Units { get; set; }
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public string Language { get; set; }
            public string LastBuildDate { get; set; }
            public string Ttl { get; set; }
            public Location Location { get; set; }
            public Wind Wind { get; set; }
            public Atmosphere Atmosphere { get; set; }
            public Astronomy Astronomy { get; set; }
            public Image Image { get; set; }
            public Item Item { get; set; }
        }

        [XmlRoot(ElementName = "results")]
        public class Results
        {
            public Channel Channel { get; set; }
        }

        [XmlRoot(ElementName = "query")]
        public class Query
        {
            public Diagnostics Diagnostics { get; set; }
            public Results Results { get; set; }
            public string Yahoo { get; set; }
            public string Count { get; set; }
            public string Created { get; set; }
            public string Lang { get; set; }
        }

    }
}