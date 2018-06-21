using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WeatherWebApplication.Models;
using static WeatherWebApplication.Class.AllClasses;
using System.Collections;
using System.Data;
using System.Diagnostics;
using static WeatherWebApplication.Models.MainClass;
using System.Data.Entity;

namespace WeatherWebApplication.Controllers
{
    /// <summary>
    /// It contains all the actions for creating weather values 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CityWeather Temp = new CityWeather();
             
            return View(Temp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[AllowAnonymous]
        //public ActionResult weatherApp(string returnUrl)
        //{
        //    var high = TempData["Message"];
        //    ViewBag.StatusMessage = high;
        //    var low = TempData["Message1"];
        //    ViewBag.Message = low;
        //    var city = TempData["Message2"];
        //    ViewBag.City = city;
        //    var temp = TempData["temperature"];
        //    ViewBag.temp = temp;
        //    var humidity = TempData["Humidity"];
        //    ViewBag.humidit = humidity;
        //    var visibility = TempData["visibility"];
        //    ViewBag.visibilit = visibility;

        //    var listCity = cityList();
        //    return View(listCity);

        //}
        /// <summary>
        /// This action is used to get a details of temperature of required location
        /// </summary>
        /// <param name="objcity"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult weatherApp(string objcity,string returnUrl)
        {
            if (objcity != null)
            {
                var city = new CityWeather();
                city.city = objcity;
                // Initialize XML Serializer Class which hosts the Deserialize() function
                // typeof(query) - query is root node of XML

                XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

                // Initialize Web Client and set its encoding to UTF8
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;

                // Form Actual URL - REST API call
                StringBuilder sbURL = new StringBuilder();
                sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
                // YQL is select * from geo.places where text='sfo'
                sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
            (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + city.city + "')")); // Anti XSS encoder - 
                                                                                                                                  // Prevent cross site scripting
                sbURL.Append(@"&diagnostics=true");

                // Download string (XML data) from REST API response
                // Downloads the requested resource as a string.
                string XMLresult = wc.DownloadString(sbURL.ToString());

                //diagnostics diagnostics = new diagnostics();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(XMLresult);
                XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
                //XmlNode node2 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
                XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
                XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
                XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
                var a = node.Attributes.Item(6).Value;
                CityWeather Temp = new CityWeather();
                Temp.humidity = pnode.Attributes.Item(1).Value;
                Temp.high = int.Parse(node.Attributes.Item(4).Value);
                int thigh = convertCelcius(Temp.high);
                Temp.low = int.Parse(node.Attributes.Item(5).Value);
                int tlow = convertCelcius(Temp.low);
                Temp.city = node3.Attributes.Item(1).Value;
                Temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
                int temperature = convertCelcius(Temp.temperature);
                TempData["Message"] = temperature;
                TempData["Message"] = thigh;
                TempData["Message1"] = tlow;
                TempData["Message2"] = node3.Attributes.Item(1).Value;
                TempData["temperature"] = temperature;
                TempData["Humidity"] = Temp.humidity;
                //Temp.precipitation = node.Attributes.Item(7).Value;
                //TempData["precipitation"] = Temp.precipitation;
                Temp.visibility = pnode.Attributes.Item(4).Value;
                TempData["visibility"] = Temp.visibility;
                AddCity(Temp.city, temperature,Temp.humidity,Temp.visibility);
                var listCity = cityList();
                var high = TempData["Message"];
                ViewBag.StatusMessage = high;
                var low = TempData["Message1"];
                ViewBag.Message = low;
                var cityV = TempData["Message2"];
                ViewBag.City = cityV;
                var temp = TempData["temperature"];
                ViewBag.temp = temp;
                var humidity = TempData["Humidity"];
                ViewBag.humidit = humidity;
                var visibility = TempData["visibility"];
                ViewBag.visibilit = visibility;
                return View(listCity);
            }
            else
            {
                var listCity = cityList();
                return View(listCity);
            }
        }

        /// <summary>
        /// Converting fahrenheit to celsius
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int convertCelcius(int b)
        {
            b = (b - 32) * 5 / 9;
            return b;
        }

        /// <summary>
        /// This action is used to show all the details of particular location 
        /// when we click on the location in weatherApp page
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult weather(string cityValue,string returnUrl)
        {
            var cityName = new CityWeather();
            cityName.city = cityValue;
            // Initialize XML Serializer Class which hosts the Deserialize() function
            // typeof(query) - query is root node of XML

            XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

            // Initialize Web Client and set its encoding to UTF8
            // In this we are initializing the web browser and encoding (which is used to upload 
            // and download strings) to UTF8
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            // Form Actual URL - REST API call
            StringBuilder sbURL = new StringBuilder();
            sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
            // YQL is select * from geo.places where text='sfo'
            sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
        (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + cityName.city + "')")); // Anti XSS encoder - 
                                                                                                                              // Prevent cross site scripting
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response

            string XMLresult = wc.DownloadString(sbURL.ToString());

            //diagnostics diagnostics = new diagnostics();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLresult);
            XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
            CityWeather tempe = new CityWeather();
            tempe.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = convertCelcius(tempe.temperature);
            TempData["Message"] = temperature;

            XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            tempe.city = node3.Attributes.Item(1).Value;
            TempData["Message2"] = node3.Attributes.Item(1).Value;

            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            tempe.text = node.Attributes.Item(6).Value;
            TempData["Message3"] = node.Attributes.Item(6).Value;

            //Tuesday
            XmlNode tuesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            tempe.high7 = int.Parse(tuesday.Attributes.Item(4).Value);
            int tueHigh = convertCelcius(tempe.high7);
            TempData["TuesdayHigh"] = tueHigh;

            tempe.low7 = int.Parse(tuesday.Attributes.Item(5).Value);
            int tueLow = convertCelcius(tempe.low7);
            TempData["TuesdayLow"] = tueLow;

            //Wednesday

            XmlNode node4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            tempe.high = int.Parse(node4.Attributes.Item(4).Value);
            int thigh = convertCelcius(tempe.high);
            TempData["Message4"] = thigh;

            XmlNode node5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            tempe.low = int.Parse(node4.Attributes.Item(5).Value);
            int tlow = convertCelcius(tempe.low);
            TempData["Message5"] = tlow;

            //Thursday

            XmlNode node6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            tempe.high1 = int.Parse(node6.Attributes.Item(4).Value);
            int thigh1 = convertCelcius(tempe.high1);
            TempData["Message6"] = thigh1;

            XmlNode node7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            tempe.low1 = int.Parse(node7.Attributes.Item(5).Value);
            int tlow1 = convertCelcius(tempe.low1);
            TempData["Message7"] = tlow1;

            //Friday
            XmlNode node8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            tempe.high2 = int.Parse(node8.Attributes.Item(4).Value);
            int thigh2 = convertCelcius(tempe.high2);
            TempData["Message8"] = thigh2;

            XmlNode node9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            tempe.low2 = int.Parse(node9.Attributes.Item(5).Value);
            int tlow2 = convertCelcius(tempe.low2);
            TempData["Message9"] = tlow2;

            //Saturday
            XmlNode node10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            tempe.high3 = int.Parse(node10.Attributes.Item(4).Value);
            int thigh3 = convertCelcius(tempe.high3);
            TempData["Message10"] = thigh3;

            XmlNode node11 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            tempe.low3 = int.Parse(node11.Attributes.Item(5).Value);
            int tlow3 = convertCelcius(tempe.low3);
            TempData["Message11"] = tlow3;

            //Sunday
            XmlNode node12 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            tempe.high4 = int.Parse(node12.Attributes.Item(4).Value);
            int thigh4 = convertCelcius(tempe.high4);
            TempData["Message12"] = thigh4;

            XmlNode node13 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            tempe.low4 = int.Parse(node13.Attributes.Item(5).Value);
            int tlow4 = convertCelcius(tempe.low4);
            TempData["Message13"] = tlow4;

            //Monday
            XmlNode node14 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            tempe.high5 = int.Parse(node14.Attributes.Item(4).Value);
            int thigh5 = convertCelcius(tempe.high5);
            TempData["Message14"] = thigh5;

            XmlNode node15 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            tempe.low5 = int.Parse(node15.Attributes.Item(5).Value);
            int tlow5 = convertCelcius(tempe.low5);
            TempData["Message15"] = tlow5;

            //Tuesday
            XmlNode node16 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            tempe.high6 = int.Parse(node16.Attributes.Item(4).Value);
            int thigh6 = convertCelcius(tempe.high6);
            TempData["Message16"] = thigh6;

            XmlNode node17 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            tempe.low6 = int.Parse(node17.Attributes.Item(5).Value);
            int tlow6 = convertCelcius(tempe.low6);
            TempData["Message17"] = tlow6;

            //Wednesday
            XmlNode wednesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            tempe.high8 = int.Parse(wednesday.Attributes.Item(4).Value);
            int thigh8 = convertCelcius(tempe.high8);
            TempData["NewWednesdayValueHigh"] = thigh8;

            tempe.low8 = int.Parse(wednesday.Attributes.Item(5).Value);
            int tlow8 = convertCelcius(tempe.low8);
            TempData["NewWednesdayValueLow"] = tlow8;

            //Thursday
            XmlNode thursday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);
            tempe.high9 = int.Parse(thursday.Attributes.Item(4).Value);
            int thigh9 = convertCelcius(tempe.high9);
            TempData["NewThursdayValueHigh"] = thigh9;

            tempe.low9 = int.Parse(thursday.Attributes.Item(5).Value);
            int tlow9 = convertCelcius(tempe.low9);
            TempData["NewThursdayValueLow"] = tlow9;

            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            tempe.humidity = pnode.Attributes.Item(1).Value;
            TempData["Humidity"] = tempe.humidity;
            tempe.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = tempe.visibility;

            var temp = TempData["Message"];
            ViewBag.temp = temp;

            var city = TempData["Message2"];
            ViewBag.City = city;

            var text = TempData["Message3"];
            ViewBag.condition = text;


            var tuesHigh = TempData["TuesdayHigh"];
            ViewBag.tuesHigh = tuesHigh;

            var tuesLow = TempData["TuesdayLow"];
            ViewBag.tuesLow = tuesLow;

            //Tuesday
            var high = TempData["Message4"];
            ViewBag.high = high;

            var low = TempData["Message5"];
            ViewBag.low = low;

            //Wednesday
            var high1 = TempData["Message6"];
            ViewBag.high1 = high1;

            var low1 = TempData["Message7"];
            ViewBag.low1 = low1;

            //Thursday
            var high2 = TempData["Message8"];
            ViewBag.high2 = high2;

            var low2 = TempData["Message9"];
            ViewBag.low2 = low2;

            //Friday
            var high3 = TempData["Message10"];
            ViewBag.high3 = high3;

            var low3 = TempData["Message11"];
            ViewBag.low3 = low3;

            //Saturday
            var high4 = TempData["Message12"];
            ViewBag.high4 = high4;

            var low4 = TempData["Message13"];
            ViewBag.low4 = low4;

            //Sunday
            var high5 = TempData["Message14"];
            ViewBag.high5 = high5;

            var low5 = TempData["Message15"];
            ViewBag.low5 = low5;

            //Monday
            var high6 = TempData["Message16"];
            ViewBag.high6 = high6;

            var low6 = TempData["Message17"];
            ViewBag.low6 = low6;

            //

            var highWed = TempData["NewWednesdayValueHigh"];
            ViewBag.highWed = highWed;

            var lowWed = TempData["NewWednesdayValueLow"];
            ViewBag.lowWed = lowWed;

            //
            var highThurs = TempData["NewThursdayValueHigh"];
            ViewBag.highThurs = highThurs;

            var lowThurs = TempData["NewThursdayValueLow"];
            ViewBag.lowThurs = lowThurs;

            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;

            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;


            return View("weather");

        }


        /// <summary>
        /// This action is used to add the values to the database
        /// </summary>
        /// <param name="city"></param>
        /// <param name="temp"></param>
        /// <param name="humidity"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        public ActionResult AddCity(string city, int temp ,string humidity , string visibility)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var cityWeather = new CityWeather();
                cityWeather.city = city;
                cityWeather.temperature = temp;
                cityWeather.humidity = humidity;
                cityWeather.visibility = visibility;
                db.cityWeather.Add(cityWeather);
                db.SaveChanges();
                return RedirectToAction("weatherApp", "Home");
            }
        }
        /// <summary>
        /// This action is used to fetch all the values from the database
        /// </summary>
        /// <returns></returns>
        public List<CityWeather> cityList()
        {
             List<CityWeather> tm = new List<CityWeather>();
            
            var cWeather = new List<CityWeather>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dv = db.cityWeather.ToList();
                foreach (var item in dv)
                {
                    tm.Add(new CityWeather() { city = item.city, temperature = item.temperature , humidity = item.humidity , visibility = item.visibility });
                }
            }
            return tm;
        }

        /// <summary>
        /// getting the values from fahreneit values from post method
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult fahrenheit(string returnUrl)
        {
            var temp = TempData["Message"];
            ViewBag.temp = temp;

            var city = TempData["Message2"];
            ViewBag.City = city;

            var text = TempData["Message3"];
            ViewBag.condition = text;

            var tuesHigh = TempData["TuesdayHigh"];
            ViewBag.tuesHigh = tuesHigh;

            var tuesLow = TempData["TuesdayLow"];
            ViewBag.tuesLow = tuesLow;

            //Tuesday
            var high = TempData["Message4"];
            ViewBag.high = high;

            var low = TempData["Message5"];
            ViewBag.low = low;

            //Wednesday
            var high1 = TempData["Message6"];
            ViewBag.high1 = high1;

            var low1 = TempData["Message7"];
            ViewBag.low1 = low1;

            //Thursday
            var high2 = TempData["Message8"];
            ViewBag.high2 = high2;

            var low2 = TempData["Message9"];
            ViewBag.low2 = low2;

            //Friday
            var high3 = TempData["Message10"];
            ViewBag.high3 = high3;

            var low3 = TempData["Message11"];
            ViewBag.low3 = low3;

            //Saturday
            var high4 = TempData["Message12"];
            ViewBag.high4 = high4;

            var low4 = TempData["Message13"];
            ViewBag.low4 = low4;

            //Sunday
            var high5 = TempData["Message14"];
            ViewBag.high5 = high5;

            var low5 = TempData["Message15"];
            ViewBag.low5 = low5;

            //Monday
            var high6 = TempData["Message16"];
            ViewBag.high6 = high6;

            var low6 = TempData["Message17"];
            ViewBag.low6 = low6;

            //
            var highWed = TempData["NewWednesdayValueHigh"];
            ViewBag.highWed = highWed;

            var lowWed = TempData["NewWednesdayValueLow"];
            ViewBag.lowWed = lowWed;

            //
            var highThurs = TempData["NewThursdayValueHigh"];
            ViewBag.highThurs = highThurs;

            var lowThurs = TempData["NewThursdayValueLow"];
            ViewBag.lowThurs = lowThurs;

            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;

            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;

            var day = TempData["day"];
            ViewBag.day = day;
            var day1 = TempData["day1"];
            ViewBag.day1 = day1;
            var day2 = TempData["day2"];
            ViewBag.day2 = day2;
            var day3 = TempData["day3"];
            ViewBag.day3 = day3;
            var day4 = TempData["day4"];
            ViewBag.day4 = day4;
            var day5 = TempData["day5"];
            ViewBag.day5 = day5;
            var day6 = TempData["day6"];
            ViewBag.day6 = day6;
            var day7 = TempData["day7"];
            ViewBag.day7 = day7;
            var day8 = TempData["day8"];
            ViewBag.day8 = day8;
            var day9 = TempData["day9"];
            ViewBag.day9 = day9;


            return View();
        }
        /// <summary>
        /// This action is used to convert from celcius to fahrenheit
        /// </summary>
        /// <param name="cityFahrenValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fahrenheit(string cityFahrenValue , string returnUrl)
        {
            var cityName = new CityWeather();
            cityName.city = cityFahrenValue;
            // Initialize XML Serializer Class which hosts the Deserialize() function
            // typeof(query) - query is root node of XML

            XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

            // Initialize Web Client and set its encoding to UTF8
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            // Form Actual URL - REST API call
            StringBuilder sbURL = new StringBuilder();
            sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
            // YQL is select * from geo.places where text='sfo'
            sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
        (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + cityName.city + "')")); // Anti XSS encoder - 
                                                                                                                                  // Prevent cross site scripting
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response

            string XMLresult = wc.DownloadString(sbURL.ToString());

            //diagnostics diagnostics = new diagnostics();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLresult);
            XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
            CityWeather temp = new CityWeather();
            temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = temp.temperature;
            TempData["Message"] = temperature;

            XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            temp.city = node3.Attributes.Item(1).Value;
            TempData["Message2"] = node3.Attributes.Item(1).Value;

            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            temp.text = node.Attributes.Item(6).Value;
            TempData["Message3"] = node.Attributes.Item(6).Value;

            XmlNode tuesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            temp.high7 = int.Parse(tuesday.Attributes.Item(4).Value);
            TempData["TuesdayHigh"] = temp.high7;

            temp.low7 = int.Parse(tuesday.Attributes.Item(5).Value);
            TempData["TuesdayLow"] = temp.low7;

            //Tuesday

            XmlNode node4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            temp.high = int.Parse(node4.Attributes.Item(4).Value);
            int thigh = temp.high;
            TempData["Message4"] = thigh;

            XmlNode node5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            temp.low = int.Parse(node4.Attributes.Item(5).Value);
            int tlow = temp.low;
            TempData["Message5"] = tlow;

            //Wednesday

            XmlNode node6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            temp.high1 = int.Parse(node6.Attributes.Item(4).Value);
            int thigh1 = temp.high1;
            TempData["Message6"] = thigh1;

            XmlNode node7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            temp.low1 = int.Parse(node7.Attributes.Item(5).Value);
            int tlow1 = temp.low1;
            TempData["Message7"] = tlow1;

            //Thursday
            XmlNode node8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            temp.high2 = int.Parse(node8.Attributes.Item(4).Value);
            int thigh2 = temp.high2;
            TempData["Message8"] = thigh2;

            XmlNode node9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            temp.low2 = int.Parse(node9.Attributes.Item(5).Value);
            int tlow2 = temp.low2;
            TempData["Message9"] = tlow2;

            //Friday
            XmlNode node10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            temp.high3 = int.Parse(node10.Attributes.Item(4).Value);
            int thigh3 = temp.high3;
            TempData["Message10"] = thigh3;

            XmlNode node11 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            temp.low3 = int.Parse(node11.Attributes.Item(5).Value);
            int tlow3 = temp.low3;
            TempData["Message11"] = tlow3;

            //Saturday
            XmlNode node12 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            temp.high4 = int.Parse(node12.Attributes.Item(4).Value);
            int thigh4 = temp.high4;
            TempData["Message12"] = thigh4;

            XmlNode node13 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            temp.low4 = int.Parse(node13.Attributes.Item(5).Value);
            int tlow4 = temp.low4;
            TempData["Message13"] = tlow4;

            //Sunday
            XmlNode node14 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            temp.high5 = int.Parse(node14.Attributes.Item(4).Value);
            int thigh5 = temp.high5;
            TempData["Message14"] = thigh5;

            XmlNode node15 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            temp.low5 = int.Parse(node15.Attributes.Item(5).Value);
            int tlow5 = temp.low5;
            TempData["Message15"] = tlow5;

            //Monday
            XmlNode node16 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            temp.high6 = int.Parse(node16.Attributes.Item(4).Value);
            int thigh6 = temp.high6;
            TempData["Message16"] = thigh6;

            XmlNode node17 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            temp.low6 = int.Parse(node17.Attributes.Item(5).Value);
            int tlow6 = temp.low6;
            TempData["Message17"] = tlow6;

            //Wednesday
            XmlNode wednesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            temp.high8 = int.Parse(wednesday.Attributes.Item(4).Value);
            TempData["NewWednesdayValueHigh"] = temp.high8;

            temp.low8 = int.Parse(wednesday.Attributes.Item(5).Value);
            TempData["NewWednesdayValueLow"] = temp.low8;

            //Thursday
            XmlNode thursday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);
            temp.high9 = int.Parse(thursday.Attributes.Item(4).Value);
            TempData["NewThursdayValueHigh"] = temp.high9;

            temp.low9 = int.Parse(thursday.Attributes.Item(5).Value);
            TempData["NewThursdayValueLow"] = temp.low9;

            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            temp.humidity = pnode.Attributes.Item(1).Value;
            TempData["Humidity"] = temp.humidity;
            temp.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = temp.visibility;

            XmlNode week1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            XmlNode week2 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            XmlNode week3 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            XmlNode week4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            XmlNode week5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            XmlNode week6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            XmlNode week7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            XmlNode week8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            XmlNode week9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            XmlNode week10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);

            temp.day = week1.Attributes.Item(3).Value;
            TempData["day"] = temp.day;
            temp.day1 = week2.Attributes.Item(3).Value;
            TempData["day1"] = temp.day1;
            temp.day2 = week3.Attributes.Item(3).Value;
            TempData["day2"] = temp.day2;
            temp.day3 = week4.Attributes.Item(3).Value;
            TempData["day3"] = temp.day3;
            temp.day4 = week5.Attributes.Item(3).Value;
            TempData["day4"] = temp.day4;
            temp.day5 = week6.Attributes.Item(3).Value;
            TempData["day5"] = temp.day5;
            temp.day6 = week7.Attributes.Item(3).Value;
            TempData["day6"] = temp.day6;
            temp.day7 = week8.Attributes.Item(3).Value;
            TempData["day7"] = temp.day7;
            temp.day8 = week9.Attributes.Item(3).Value;
            TempData["day8"] = temp.day8;
            temp.day9 = week10.Attributes.Item(3).Value;
            TempData["day9"] = temp.day9;
            return View("fahrenheit");

        }

        /// <summary>
        /// get method
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ActionResult Delete(string city)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.cityWeather.Where(x => x.city == city).FirstOrDefault());
            }
        }

        /// <summary>
        /// deletes the values of required location
        /// </summary>
        /// <param name="city"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string city,FormCollection collection)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    CityWeather cityName = db.cityWeather.Where(x => x.city == city).FirstOrDefault();
                    db.cityWeather.Remove(cityName);
                    db.SaveChanges();
                }
                return RedirectToAction("weatherApp");
            }
            catch
            {

                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityValue"></param>
        /// <returns></returns>
        public ActionResult Details(string cityValue)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.cityWeather.Where(x => x.city == cityValue).FirstOrDefault());
            }
        }

        public ActionResult search(string returnUrl)
        {
            var temp = TempData["temperature"];
            ViewBag.temp = temp;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult search(CityWeather cityValue,string returnUrl)
        {
            //CityWeather city = new CityWeather();
            if (cityValue.city != null)
            {
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.cityWeather.Add(cityValue);
                        //db.SaveChanges();

                        ModelState.Clear();
                        XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

                        // Initialize Web Client and set its encoding to UTF8
                        WebClient wc = new WebClient();
                        wc.Encoding = Encoding.UTF8;

                        // Form Actual URL - REST API call
                        StringBuilder sbURL = new StringBuilder();
                        sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
                        // YQL is select * from geo.places where text='sfo'
                        sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
                    (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + cityValue.city + "')")); // Anti XSS encoder - 
                                                                                                                                               // Prevent cross site scripting
                        sbURL.Append(@"&diagnostics=true");

                        // Download string (XML data) from REST API response

                        string XMLresult = wc.DownloadString(sbURL.ToString());

                        //diagnostics diagnostics = new diagnostics();

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(XMLresult);
                        CityWeather Temp = new CityWeather();
                        XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
                        Temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
                        int temperature = convertCelcius(Temp.temperature);
                        TempData["Message"] = temperature;
                        cityValue.temperature = temperature;
                        db.cityWeather.Add(cityValue);

                        XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
                        Temp.humidity = pnode.Attributes.Item(1).Value;
                        TempData["Humidity"] = Temp.humidity;
                        cityValue.humidity = Temp.humidity;
                        db.cityWeather.Add(cityValue);

                        Temp.visibility = pnode.Attributes.Item(4).Value;
                        TempData["visibility"] = Temp.visibility;
                        cityValue.visibility = Temp.visibility;
                        db.cityWeather.Add(cityValue);

                        db.SaveChanges();
                        ViewBag.Message = "Succesfully created";
                    }
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter any location to continue";
            }
            return View();
        }

        /// <summary>
        /// getting the values from post method of weatherApi
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult weatherApi(string returnUrl)
        {
            var listCity = cityList();
            //return Json(result, JsonRequestBehavior.AllowGet);
            return View(listCity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult weatherApi(string cityValue, string returnUrl)
        {
            var city = new CityWeather();
            city.city = cityValue;
            // Initialize XML Serializer Class which hosts the Deserialize() function
            // typeof(query) - query is root node of XML

            XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

            // Initialize Web Client and set its encoding to UTF8
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            // Form Actual URL - REST API call
            StringBuilder sbURL = new StringBuilder();
            sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
            // YQL is select * from geo.places where text='sfo'
            sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
        (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + city.city + "')")); // Anti XSS encoder - 
                                                                                                                              // Prevent cross site scripting
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response
            // Downloads the requested resource as a string.
            string XMLresult = wc.DownloadString(sbURL.ToString());

            //diagnostics diagnostics = new diagnostics();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLresult);
            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            //XmlNode node2 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            var a = node.Attributes.Item(6).Value;
            CityWeather Temp = new CityWeather();
            Temp.humidity = pnode.Attributes.Item(1).Value;
            Temp.high = int.Parse(node.Attributes.Item(4).Value);
            int thigh = convertCelcius(Temp.high);
            Temp.low = int.Parse(node.Attributes.Item(5).Value);
            int tlow = convertCelcius(Temp.low);
            Temp.city = node3.Attributes.Item(1).Value;
            Temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = convertCelcius(Temp.temperature);
            TempData["Message"] = temperature;
            TempData["Message"] = thigh;
            TempData["Message1"] = tlow;
            TempData["Message2"] = node3.Attributes.Item(1).Value;
            TempData["temperature"] = temperature;
            TempData["Humidity"] = Temp.humidity;
            //Temp.precipitation = node.Attributes.Item(7).Value;
            //TempData["precipitation"] = Temp.precipitation;
            Temp.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = Temp.visibility;
            var result = new { Temp.city, temperature, Temp.humidity, Temp.visibility };
            AddCity(Temp.city, temperature, Temp.humidity, Temp.visibility);
            var listCity = cityList();
            return View(listCity);
            //return Json(result,JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult weatherApiDetails(string returnUrl)
        {
            var temp = TempData["Message"];
            ViewBag.temp = temp;

            var city = TempData["Message2"];
            ViewBag.City = city;

            var text = TempData["Message3"];
            ViewBag.condition = text;


            var tuesHigh = TempData["TuesdayHigh"];
            ViewBag.tuesHigh = tuesHigh;

            var tuesLow = TempData["TuesdayLow"];
            ViewBag.tuesLow = tuesLow;

            //Tuesday
            var high = TempData["Message4"];
            ViewBag.high = high;

            var low = TempData["Message5"];
            ViewBag.low = low;

            //Wednesday
            var high1 = TempData["Message6"];
            ViewBag.high1 = high1;

            var low1 = TempData["Message7"];
            ViewBag.low1 = low1;

            //Thursday
            var high2 = TempData["Message8"];
            ViewBag.high2 = high2;

            var low2 = TempData["Message9"];
            ViewBag.low2 = low2;

            //Friday
            var high3 = TempData["Message10"];
            ViewBag.high3 = high3;

            var low3 = TempData["Message11"];
            ViewBag.low3 = low3;

            //Saturday
            var high4 = TempData["Message12"];
            ViewBag.high4 = high4;

            var low4 = TempData["Message13"];
            ViewBag.low4 = low4;

            //Sunday
            var high5 = TempData["Message14"];
            ViewBag.high5 = high5;

            var low5 = TempData["Message15"];
            ViewBag.low5 = low5;

            //Monday
            var high6 = TempData["Message16"];
            ViewBag.high6 = high6;

            var low6 = TempData["Message17"];
            ViewBag.low6 = low6;

            //

            var highWed = TempData["NewWednesdayValueHigh"];
            ViewBag.highWed = highWed;

            var lowWed = TempData["NewWednesdayValueLow"];
            ViewBag.lowWed = lowWed;

            //
            var highThurs = TempData["NewThursdayValueHigh"];
            ViewBag.highThurs = highThurs;

            var lowThurs = TempData["NewThursdayValueLow"];
            ViewBag.lowThurs = lowThurs;

            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;

            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;

            var day = TempData["day"];
            ViewBag.day = day;
            var day1 = TempData["day1"];
            ViewBag.day1 = day1;
            var day2 = TempData["day2"];
            ViewBag.day2 = day2;
            var day3 = TempData["day3"];
            ViewBag.day3 = day3;
            var day4 = TempData["day4"];
            ViewBag.day4 = day4;
            var day5 = TempData["day5"];
            ViewBag.day5 = day5;
            var day6 = TempData["day6"];
            ViewBag.day6 = day6;
            var day7 = TempData["day7"];
            ViewBag.day7 = day7;
            var day8 = TempData["day8"];
            ViewBag.day8 = day8;
            var day9 = TempData["day9"];
            ViewBag.day9 = day9;


            return View();
        }

        [HttpPost]
        public ActionResult weatherApiDetails(string cityValue, string returnUrl)
        {
            var cityName = new CityWeather();
            cityName.city = cityValue;
            // Initialize XML Serializer Class which hosts the Deserialize() function
            // typeof(query) - query is root node of XML

            XmlSerializer XMLdeserialize = new XmlSerializer(typeof(query));

            // Initialize Web Client and set its encoding to UTF8
            // In this we are initializing the web browser and encoding (which is used to upload 
            // and download strings) to UTF8
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            // Form Actual URL - REST API call
            StringBuilder sbURL = new StringBuilder();
            sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
            // YQL is select * from geo.places where text='sfo'
            sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
        (@"select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + cityName.city + "')")); // Anti XSS encoder - 
                                                                                                                                  // Prevent cross site scripting
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response

            string XMLresult = wc.DownloadString(sbURL.ToString());

            //diagnostics diagnostics = new diagnostics();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLresult);
            XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
            CityWeather tempe = new CityWeather();
            tempe.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = convertCelcius(tempe.temperature);
            TempData["Message"] = temperature;

            XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            tempe.city = node3.Attributes.Item(1).Value;
            TempData["Message2"] = node3.Attributes.Item(1).Value;

            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            tempe.text = node.Attributes.Item(6).Value;
            TempData["Message3"] = node.Attributes.Item(6).Value;

            //Tuesday
            XmlNode tuesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            tempe.high7 = int.Parse(tuesday.Attributes.Item(4).Value);
            int tueHigh = convertCelcius(tempe.high7);
            TempData["TuesdayHigh"] = tueHigh;

            tempe.low7 = int.Parse(tuesday.Attributes.Item(5).Value);
            int tueLow = convertCelcius(tempe.low7);
            TempData["TuesdayLow"] = tueLow;

            //Wednesday

            XmlNode node4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            tempe.high = int.Parse(node4.Attributes.Item(4).Value);
            int thigh = convertCelcius(tempe.high);
            TempData["Message4"] = thigh;

            XmlNode node5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            tempe.low = int.Parse(node4.Attributes.Item(5).Value);
            int tlow = convertCelcius(tempe.low);
            TempData["Message5"] = tlow;

            //Thursday

            XmlNode node6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            tempe.high1 = int.Parse(node6.Attributes.Item(4).Value);
            int thigh1 = convertCelcius(tempe.high1);
            TempData["Message6"] = thigh1;

            XmlNode node7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            tempe.low1 = int.Parse(node7.Attributes.Item(5).Value);
            int tlow1 = convertCelcius(tempe.low1);
            TempData["Message7"] = tlow1;

            //Friday
            XmlNode node8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            tempe.high2 = int.Parse(node8.Attributes.Item(4).Value);
            int thigh2 = convertCelcius(tempe.high2);
            TempData["Message8"] = thigh2;

            XmlNode node9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            tempe.low2 = int.Parse(node9.Attributes.Item(5).Value);
            int tlow2 = convertCelcius(tempe.low2);
            TempData["Message9"] = tlow2;

            //Saturday
            XmlNode node10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            tempe.high3 = int.Parse(node10.Attributes.Item(4).Value);
            int thigh3 = convertCelcius(tempe.high3);
            TempData["Message10"] = thigh3;

            XmlNode node11 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            tempe.low3 = int.Parse(node11.Attributes.Item(5).Value);
            int tlow3 = convertCelcius(tempe.low3);
            TempData["Message11"] = tlow3;

            //Sunday
            XmlNode node12 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            tempe.high4 = int.Parse(node12.Attributes.Item(4).Value);
            int thigh4 = convertCelcius(tempe.high4);
            TempData["Message12"] = thigh4;

            XmlNode node13 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            tempe.low4 = int.Parse(node13.Attributes.Item(5).Value);
            int tlow4 = convertCelcius(tempe.low4);
            TempData["Message13"] = tlow4;

            //Monday
            XmlNode node14 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            tempe.high5 = int.Parse(node14.Attributes.Item(4).Value);
            int thigh5 = convertCelcius(tempe.high5);
            TempData["Message14"] = thigh5;

            XmlNode node15 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            tempe.low5 = int.Parse(node15.Attributes.Item(5).Value);
            int tlow5 = convertCelcius(tempe.low5);
            TempData["Message15"] = tlow5;

            //Tuesday
            XmlNode node16 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            tempe.high6 = int.Parse(node16.Attributes.Item(4).Value);
            int thigh6 = convertCelcius(tempe.high6);
            TempData["Message16"] = thigh6;

            XmlNode node17 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            tempe.low6 = int.Parse(node17.Attributes.Item(5).Value);
            int tlow6 = convertCelcius(tempe.low6);
            TempData["Message17"] = tlow6;

            //Wednesday
            XmlNode wednesday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            tempe.high8 = int.Parse(wednesday.Attributes.Item(4).Value);
            int thigh8 = convertCelcius(tempe.high8);
            TempData["NewWednesdayValueHigh"] = thigh8;

            tempe.low8 = int.Parse(wednesday.Attributes.Item(5).Value);
            int tlow8 = convertCelcius(tempe.low8);
            TempData["NewWednesdayValueLow"] = tlow8;

            //Thursday
            XmlNode thursday = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);
            tempe.high9 = int.Parse(thursday.Attributes.Item(4).Value);
            int thigh9 = convertCelcius(tempe.high9);
            TempData["NewThursdayValueHigh"] = thigh9;

            tempe.low9 = int.Parse(thursday.Attributes.Item(5).Value);
            int tlow9 = convertCelcius(tempe.low9);
            TempData["NewThursdayValueLow"] = tlow9;

            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            tempe.humidity = pnode.Attributes.Item(1).Value;
            TempData["Humidity"] = tempe.humidity;
            tempe.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = tempe.visibility;

            XmlNode week1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            XmlNode week2 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            XmlNode week3 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            XmlNode week4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            XmlNode week5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            XmlNode week6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            XmlNode week7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            XmlNode week8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            XmlNode week9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            XmlNode week10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);

            tempe.day = week1.Attributes.Item(3).Value;
            TempData["day"] = tempe.day;
            tempe.day1 = week2.Attributes.Item(3).Value;
            TempData["day1"] = tempe.day1;
            tempe.day2 = week3.Attributes.Item(3).Value;
            TempData["day2"] = tempe.day2;
            tempe.day3 = week4.Attributes.Item(3).Value;
            TempData["day3"] = tempe.day3;
            tempe.day4 = week5.Attributes.Item(3).Value;
            TempData["day4"] = tempe.day4;
            tempe.day5 = week6.Attributes.Item(3).Value;
            TempData["day5"] = tempe.day5;
            tempe.day6 = week7.Attributes.Item(3).Value;
            TempData["day6"] = tempe.day6;
            tempe.day7 = week8.Attributes.Item(3).Value;
            TempData["day7"] = tempe.day7;
            tempe.day8 = week9.Attributes.Item(3).Value;
            TempData["day8"] = tempe.day8;
            tempe.day9 = week10.Attributes.Item(3).Value;
            TempData["day9"] = tempe.day9;
            return View("weatherApiDetails");
        }
    }
}