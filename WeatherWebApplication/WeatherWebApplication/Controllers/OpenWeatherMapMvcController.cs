using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security.AntiXss;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WeatherWebApplication.Models;
using static WeatherWebApplication.Class.AllClasses;

namespace WeatherWebApplication.Controllers
{
    public class OpenWeatherMapMvcController : Controller
    {
        public OpenWeatherMap FillCity()
        {
            OpenWeatherMap openWeatherMap = new OpenWeatherMap();
            openWeatherMap.cities = new Dictionary<string, string>();
            openWeatherMap.cities.Add("Melbourne", "7839805");
            openWeatherMap.cities.Add("Auckland", "2193734");
            openWeatherMap.cities.Add("NewDelhi", "1261481");
            openWeatherMap.cities.Add("Abu Dhabi", "292968");
            openWeatherMap.cities.Add("Lahore", "1172451");
            openWeatherMap.cities.Add("Hyderabad", "1269843");
            openWeatherMap.cities.Add("Gurgaon", "1270642");
            
            return openWeatherMap;
        }


        // GET: OpenWeatherMapMvc
        public ActionResult Index()
        {
            OpenWeatherMap openWeatherMap = FillCity();
            return View();
        }


        public ActionResult Index(OpenWeatherMap openWeatherMap, string cities)
        {
            openWeatherMap = FillCity();

            if (cities != null)
            {
                string apiKey = "c5ca5fc08600882248efe12926c6d411";
                HttpWebRequest apiRequest = WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" + cities + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;
                StringBuilder theWebAddres = new StringBuilder();
                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                /*End*/

                /*http://json2csharp.com*/
                ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table><tr><th>Weather Description</th></tr>");
                sb.Append("<tr><td>City:</td><td>" + rootObject.name + "</td></tr>");
                sb.Append("<tr><td>Country:</td><td>" + rootObject.sys.country + "</td></tr>");
                sb.Append("<tr><td>Wind:</td><td>" + rootObject.wind.speed + " Km/h</td></tr>");
                sb.Append("<tr><td>Current Temperature:</td><td>" + rootObject.main.temp + " °C</td></tr>");
                sb.Append("<tr><td>Humidity:</td><td>" + rootObject.main.humidity + "</td></tr>");
                //sb.Append("<tr><td>Weather:</td><td>" + rootObject.weather[0].description + "</td></tr>");
                sb.Append("</table>");
                openWeatherMap.apiResponse = sb.ToString();
            }
            else
            {
                if (Request.Form["submit"] != null)
                {
                    openWeatherMap.apiResponse = "► Select City";
                }
            }

            return View(openWeatherMap);
        }

        // GET: OpenWeatherMapMvc/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OpenWeatherMapMvc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpenWeatherMapMvc/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OpenWeatherMapMvc/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OpenWeatherMapMvc/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OpenWeatherMapMvc/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OpenWeatherMapMvc/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult weatherApp(string returnUrl)
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //foreach (var item in db.cityWeather)
            //{
            var high = TempData["Message"];
            ViewBag.StatusMessage = high;
            var low = TempData["Message1"];
            ViewBag.Message = low;
            var city = TempData["Message2"];
            ViewBag.City = city;
            var temp = TempData["temperature"];
            ViewBag.temp = temp;
            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;
            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;

            //}

            var listCity = cityList();
            return View(listCity);

        }
        /// <summary>
        /// This action is used to get a details of temperature of required location
        /// </summary>
        /// <param name="objcity"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult weatherApp(string objcity, string returnUrl)
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
            AddCity(Temp.city, temperature);
            var listCity = cityList();
            return View(listCity);
        }

        public int convertCelcius(int b)
        {
            b = (b - 32) * 5 / 9;
            return b;
        }



        [AllowAnonymous]
        public ActionResult weather(string returnUrl)
        {
            var temp = TempData["Message"];
            ViewBag.temp = temp;

            var city = TempData["Message2"];
            ViewBag.City = city;

            var text = TempData["Message3"];
            ViewBag.condition = text;

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

            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;

            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;
            return View();
        }

        /// <summary>
        /// This action is used to show all the details of particular location 
        /// when we click on the location in weatherApp page
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult weather(string cityValue, string returnUrl)
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
            CityWeather temp = new CityWeather();
            temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = convertCelcius(temp.temperature);
            TempData["Message"] = temperature;

            XmlNode node3 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            temp.city = node3.Attributes.Item(1).Value;
            TempData["Message2"] = node3.Attributes.Item(1).Value;

            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            temp.text = node.Attributes.Item(6).Value;
            TempData["Message3"] = node.Attributes.Item(6).Value;

            //Tuesday

            XmlNode node4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            temp.high = int.Parse(node4.Attributes.Item(4).Value);
            int thigh = convertCelcius(temp.high);
            TempData["Message4"] = thigh;

            XmlNode node5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            temp.low = int.Parse(node4.Attributes.Item(5).Value);
            int tlow = convertCelcius(temp.low);
            TempData["Message5"] = tlow;

            //Wednesday

            XmlNode node6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            temp.high1 = int.Parse(node6.Attributes.Item(4).Value);
            int thigh1 = convertCelcius(temp.high1);
            TempData["Message6"] = thigh1;

            XmlNode node7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            temp.low1 = int.Parse(node7.Attributes.Item(5).Value);
            int tlow1 = convertCelcius(temp.low1);
            TempData["Message7"] = tlow1;

            //Thursday
            XmlNode node8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            temp.high2 = int.Parse(node8.Attributes.Item(4).Value);
            int thigh2 = convertCelcius(temp.high2);
            TempData["Message8"] = thigh2;

            XmlNode node9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            temp.low2 = int.Parse(node9.Attributes.Item(5).Value);
            int tlow2 = convertCelcius(temp.low2);
            TempData["Message9"] = tlow2;

            //Friday
            XmlNode node10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            temp.high3 = int.Parse(node10.Attributes.Item(4).Value);
            int thigh3 = convertCelcius(temp.high3);
            TempData["Message10"] = thigh3;

            XmlNode node11 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            temp.low3 = int.Parse(node11.Attributes.Item(5).Value);
            int tlow3 = convertCelcius(temp.low3);
            TempData["Message11"] = tlow3;

            //Saturday
            XmlNode node12 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            temp.high4 = int.Parse(node12.Attributes.Item(4).Value);
            int thigh4 = convertCelcius(temp.high4);
            TempData["Message12"] = thigh4;

            XmlNode node13 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            temp.low4 = int.Parse(node13.Attributes.Item(5).Value);
            int tlow4 = convertCelcius(temp.low4);
            TempData["Message13"] = tlow4;

            //Sunday
            XmlNode node14 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            temp.high5 = int.Parse(node14.Attributes.Item(4).Value);
            int thigh5 = convertCelcius(temp.high5);
            TempData["Message14"] = thigh5;

            XmlNode node15 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            temp.low5 = int.Parse(node15.Attributes.Item(5).Value);
            int tlow5 = convertCelcius(temp.low5);
            TempData["Message15"] = tlow5;

            //Monday
            XmlNode node16 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            temp.high6 = int.Parse(node16.Attributes.Item(4).Value);
            int thigh6 = convertCelcius(temp.high6);
            TempData["Message16"] = thigh6;

            XmlNode node17 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            temp.low6 = int.Parse(node17.Attributes.Item(5).Value);
            int tlow6 = convertCelcius(temp.low6);
            TempData["Message17"] = tlow6;

            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            temp.humidity = pnode.Attributes.Item(1).Value;
            TempData["Humidity"] = temp.humidity;
            temp.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = temp.visibility;
            return View("weather");

        }


        [AllowAnonymous]
        public ActionResult AddCity(string returnUrl)
        {
            var city = TempData["city"];
            ViewBag.city = city;
            var temper = TempData["temper"];
            ViewBag.temper = temper;
            return View();
        }


        /// <summary>
        /// This action is used to add the values to the database
        /// </summary>
        /// <param name="city"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public ActionResult AddCity(string city, int temp)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cityWeather = new CityWeather();
                cityWeather.city = city;
                cityWeather.temperature = temp;
                db.cityWeather.Add(cityWeather);
                TempData["city"] = city;
                TempData["temper"] = temp;
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
                    tm.Add(new CityWeather() { city = item.city, temperature = item.temperature });
                }
            }
            return tm;
        }

        [AllowAnonymous]
        public ActionResult fahrenheit(string returnUrl)
        {
            var temp = TempData["Message"];
            ViewBag.temp = temp;

            var city = TempData["Message2"];
            ViewBag.City = city;

            var text = TempData["Message3"];
            ViewBag.condition = text;

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

            var humidity = TempData["Humidity"];
            ViewBag.humidit = humidity;

            var visibility = TempData["visibility"];
            ViewBag.visibilit = visibility;

            return View("Index","Home");
        }
        /// <summary>
        /// This action is used to convert from celcius to fahrenheit
        /// </summary>
        /// <param name="cityFahrenValue"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fahrenheit(string cityFahrenValue, string returnUrl)
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

            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            temp.humidity = pnode.Attributes.Item(1).Value;
            TempData["Humidity"] = temp.humidity;
            temp.visibility = pnode.Attributes.Item(4).Value;
            TempData["visibility"] = temp.visibility;
            return View("fahrenheit");

        }



    }
}
