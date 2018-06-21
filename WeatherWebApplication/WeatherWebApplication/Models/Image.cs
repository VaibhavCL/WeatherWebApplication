using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class Image
    {
        [Column(Order = 0)]
        public string title { get; set; }

        [Column(Order = 1)]
        public string width { get; set; }

        [Column(Order = 2)]
        public string height { get; set; }

        [Column(Order = 3)]
        public string link { get; set; }

        [Column(Order = 4)]
        public string url { get; set; }
    }
}