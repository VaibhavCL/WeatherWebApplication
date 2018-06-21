using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class City
    {
        [Key]
        [Column(Order =0)]
        public int count { get; set; }

        [Column(Order = 1)]
        public DateTime created { get; set; }

        [Column(Order = 2)]
        public string lang { get; set; }

        [Column(Order = 3)]
        [ForeignKey("CityChannelMapping")]
        public int CityChannelMappingId { get; set; }

        [Column(Order =4)]
        public int minValue { get; set; }

        [Column(Order =5)]
        public int maxValue { get; set; }

        public CityChannelMapping cityChannelMapping { get; set; }
    }
}