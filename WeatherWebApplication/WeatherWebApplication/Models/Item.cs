using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class Item
    {
        [Column(Order = 0)]
        public string title { get; set; }

        [Column(Order = 1)]
        public string lat { get; set; }

        [Column(Order = 2)]
        public string longi { get; set; }

        [Column(Order = 3)]
        public string link { get; set; }

        [Column(Order = 4)]
        public string pubDate { get; set; }

        [Column(Order = 5)]
        public int ItemConditionMappingId { get; set; }

        [Column(Order = 6)]
        public int ItemForecastMappingId { get; set; }

        [Column(Order = 7)]
        public string description { get; set; }

        [Column(Order = 8)]
        public string isPermaLink { get; set; }

        public ItemForecastMapping itemForecastMapping { get; set; }

    }
}