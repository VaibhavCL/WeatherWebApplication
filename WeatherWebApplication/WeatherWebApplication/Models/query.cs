using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class query
    {
        [Key]
        [Column(Order =0)]
        public int id { get; set; }

        [Column(Order = 1)]
        public string distance { get; set; }

        [Column(Order = 2)]
        public string pressure { get; set; }

        [Column(Order = 3)]
        public string speed { get; set; }

        [Column(Order = 4)]
        public string temperature { get; set; }

        [Column(Order = 5)]
        public string title { get; set; }

        [Column(Order = 6)]
        public string link { get; set; }

        [Column(Order = 7)]
        public string description { get; set; }

        [Column(Order = 8)]
        public string language { get; set; }

        [Column(Order = 9)]
        public string lastBuildDate { get; set; }

        [Column(Order = 10)]
        public string ttl { get; set; }

        //[Column(Order = 11)]
        //public string queryId { get; set; }

        //[Column(Order = 11)]
        //[ForeignKey("CityWeatherMapping")]
        //public int CityWeatherMappingId { get; set; }

        //[Column(Order = 12)]
        //[ForeignKey("ImageChannelMapping")]
        //public int ImageChannelMappingId { get; set; }

        //[Column(Order = 13)]
        //[ForeignKey("ItemChannelMapping")]
        //public int ItemChannelMappingId { get; set; }

        //public virtual CityWeatherMapping cityWeatherMapping { get; set; }

        //public virtual ImageChannelMapping imageChannelMapping { get; set; }

        //public virtual ItemChannelMapping itemChannelMapping { get; set; }

    }
}