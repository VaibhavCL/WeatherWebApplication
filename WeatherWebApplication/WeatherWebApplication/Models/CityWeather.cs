using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherWebApplication.Models
{
    public class CityWeather
    {
        [Key]
        [Column(Order = 0)]
        public string city { get; set; }

        [Column(Order = 1)]
        public string country { get; set; }

        [Column(Order = 2)]
        public int temperature { get; set; }

        [Column(Order = 3)]
        public string text { get; set; }

        [Column(Order = 4)]
        public string direction { get; set; }

        [Column(Order = 5)]
        public string speed { get; set; }

        [Column(Order = 6)]
        public string humidity { get; set; }

        [Column(Order = 7)]
        public string pressure { get; set; }

        [Column(Order = 8)]
        public string rising { get; set; }

        [Column(Order = 9)]
        public string visibility { get; set; }

        [Column(Order = 10)]
        public int high { get; set; }

        [Column(Order = 11)]
        public int low { get; set; }

        [Column(Order = 12)]
        public int low1 { get; set; }

        [Column(Order = 13)]
        public int low2 { get; set; }

        [Column(Order = 14)]
        public int low3 { get; set; }

        [Column(Order = 15)]
        public int low4 { get; set; }

        [Column(Order = 16)]
        public int low5 { get; set; }

        [Column(Order = 17)]
        public int low6 { get; set; }

        [Column(Order = 18)]
        public int high1 { get; set; }

        [Column(Order = 19)]
        public int high2 { get; set; }

        [Column(Order = 20)]
        public int high3 { get; set; }

        [Column(Order = 21)]
        public int high4 { get; set; }

        [Column(Order = 22)]
        public int high5 { get; set; }

        [Column(Order = 23)]
        public int high6 { get; set; }

        [Column(Order = 24)]
        public int high7 { get; set; }

        [Column(Order = 25)]
        public int high8{ get; set; }

        [Column(Order = 26)]
        public int high9 { get; set; }

        [Column(Order = 27)]
        public int high10 { get; set; }

        [Column(Order = 28)]
        public int low7 { get; set; }

        [Column(Order = 29)]
        public int low8 { get; set; }

        [Column(Order = 30)]
        public int low9 { get; set; }

        [Column(Order = 31)]
        public int low10 { get; set; }

        [Column(Order = 32)]
        public string city2 { get; set; }

        [Column(Order = 33)]
        public string city3 { get; set; }

        [Column(Order = 34)]
        public string city4 { get; set; }

        [Column(Order = 35)]
        public string precipitation { get; set; }

        [Column(Order =36)]
        public string sunrise { get; set; }

        [Column(Order =37)]
        public string sunset { get; set; }

        [Column(Order =38)]
        public int chill { get; set; }

        [Column(Order =39)]
        public string date { get; set; }

        [Column(Order = 40)]
        public string date1 { get; set; }

        [Column(Order = 41)]
        public string date2 { get; set; }

        [Column(Order = 42)]
        public string date3 { get; set; }

        [Column(Order = 43)]
        public string date4 { get; set; }

        [Column(Order = 44)]
        public string date5 { get; set; }

        [Column(Order = 45)]
        public string date6 { get; set; }

        [Column(Order = 46)]
        public string date7 { get; set; }

        [Column(Order = 47)]
        public string date8 { get; set; }

        [Column(Order = 48)]
        public string date9 { get; set; }

        [Column(Order = 49)]
        public string region { get; set; }

        [Column(Order = 50)]
        public string day { get; set; }

        [Column(Order = 51)]
        public string day1 { get; set; }

        [Column(Order = 52)]
        public string day2 { get; set; }

        [Column(Order = 53)]
        public string day3 { get; set; }

        [Column(Order = 54)]
        public string day4 { get; set; }

        [Column(Order = 55)]
        public string day5 { get; set; }

        [Column(Order = 56)]
        public string day6 { get; set; }

        [Column(Order = 57)]
        public string day7 { get; set; }

        [Column(Order = 58)]
        public string day8 { get; set; }

        [Column(Order = 59)]
        public string day9 { get; set; }

        //[Key]
        //[Column(Order = 60)]
        //public string id { get; set; }

        public query query { get; set; }

        //public CityWeatherMapping cityWeatherMapping { get; set; }

        //public List<CityValues> cityList { get; set; }
    }

}