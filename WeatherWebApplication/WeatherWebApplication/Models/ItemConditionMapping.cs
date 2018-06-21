using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class ItemConditionMapping
    {
        
        [Column(Order = 0)]
        [ForeignKey("Condition")]
        public int ConditionId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public Condition condition { get; set; }

        public Item item { get; set; }
    }
}