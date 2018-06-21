using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Xml;
using System.Xml.Serialization;
using WeatherWebApplication.Models;
using static WeatherWebApplication.Class.AllClasses;

namespace WeatherWebApplication.Controllers
{
    public class WeatherWebController : Controller
    {
        // GET: WeatherWeb
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// weatherApi
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult weatherApi(string returnUrl)
        {
            var listCity = cityList();
            return View(listCity);
        }

        [HttpPost]
        public ActionResult weatherApi(string cityValue, string returnUrl)
        {
            var city = new CityWeather();
            city.city = cityValue;
            // Initialize XML Serializer Class which hosts the Deserialize() function
            // typeof(query) - query is root node of XML

            XmlSerializer XMLdeserialize = new XmlSerializer(typeof(CityWeather));

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

        public ActionResult AddCity(string city, int temp, string humidity, string visibility)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
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

        public List<CityWeather> cityList()
        {
            List<CityWeather> tm = new List<CityWeather>();

            var cWeather = new List<CityWeather>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dv = db.cityWeather.ToList();
                foreach (var item in dv)
                {
                    tm.Add(new CityWeather() { city = item.city, temperature = item.temperature, humidity = item.humidity, visibility = item.visibility });
                }
            }
            return tm;
        }

        public int convertCelcius(int b)
        {
            b = (b - 32) * 5 / 9;
            return b;
        }


        public ActionResult weatherForecast()
        {
            OpenWeatherMap openWeatherMap = FillCity();
            return View(openWeatherMap);
        }


        [HttpPost]
        public ActionResult weatherForecast(OpenWeatherMap openWeatherMap, string cities)
        {
            openWeatherMap = FillCity();

            if (cities != null)
            {
                /*Calling API http://openweathermap.org/api */
                string apiKey = "c5ca5fc08600882248efe12926c6d411";
                HttpWebRequest apiRequest = WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" + cities + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

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
                sb.Append("<tr><td>Weather:</td><td>" + rootObject.weather[0].description + "</td></tr>");
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

    }
}