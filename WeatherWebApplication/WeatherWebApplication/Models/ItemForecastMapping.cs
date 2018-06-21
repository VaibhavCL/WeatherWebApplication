using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class ItemForecastMapping
    {
        [Column(Order = 0)]
        [ForeignKey("Forecast")]
        public int ForcastId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Item")]
        public int ItemId { get; set; }

        //public Forecast forecast { get; set; }

        public Item item { get; set; }

    }
}