using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class ImageChannelMapping
    {
        [Column(Order = 0)]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Image")]
        public int ImageId { get; set; }

        public query channel { get; set; }

        public Image image { get; set; }
    }
}