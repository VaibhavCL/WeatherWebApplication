using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class ItemChannelMapping
    {
        [Column(Order = 0)]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        [Column(Order = 1)]
        public int ItemId { get; set; }

        public query channel { get; set; }

    }
}