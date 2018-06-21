using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class CityWeatherMapping
    {
        [Column(Order = 0)]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("CityWeather")]
        public int CityWeatherId { get; set; }

        public query channel { get; set; }

        public CityWeather cityWeather { get; set; }
    }
}