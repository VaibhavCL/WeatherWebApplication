using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class Forecast
    {
        [Column(Order = 0)]
        public string code { get; set; }

        [Column(Order = 1)]
        public string date { get; set; }

        [Column(Order = 2)]
        public string day { get; set; }

        [Column(Order = 3)]
        public string high { get; set; }

        [Column(Order = 4)]
        public string low { get; set; }

        [Column(Order = 5)]
        public string text { get; set; }
    }
}