using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security.AntiXss;
using System.Xml;
using System.Xml.Serialization;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[RoutePrefix("api/v1")]
    public class WeatherController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        //[Route("cityList")]
        public List<CityWeather> cityList()
        {
            List<CityWeather> tm = new List<CityWeather>();

            var cWeather = new List<CityWeather>();
            var dv = db.cityWeather.ToList();
            foreach (var item in dv)
            {
                tm.Add(new CityWeather() { city = item.city, temperature = item.temperature, humidity = item.humidity, visibility = item.visibility });
            }
            return tm;
        }

        //[ResponseType(typeof(CityWeather))]
        [HttpGet]
        public CityWeather cityList(string city)
        {
            CityWeather cityValue = new CityWeather();
            var dv = db.cityWeather.ToList();
            CityWeather weather = db.cityWeather.Where(x => x.city == city).FirstOrDefault();
            //if (weather == null)
            //{
            //    return cityValue;
            //}
            //else
            //{
                cityValue.city = weather.city;
                cityValue.temperature = weather.temperature;
                cityValue.humidity = weather.humidity;
                cityValue.visibility = weather.visibility;
            //}
            return cityValue;
        }


        [HttpPost]
        public HttpResponseMessage testing()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Created successfully");
            return response;
        }

        [HttpPost]
        //[Route("Weather")]
        public  HttpResponseMessage weatherApi()
        {
            CityWeather weather = new CityWeather();
            var city = new CityWeather();
            city.city = weather.city;
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
            sbURL.Append("&format = json");
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response
            // Downloads the requested resource as a string.
            string XMLresult = wc.DownloadString(sbURL.ToString());

            JObject jObject = JObject.Parse(XMLresult);
            JArray jArray = (JArray)jObject["query"]["results"]["Result"];
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,"Created successfully");
            return response;
        }

        public int convertCelcius(int b)
        {
            b = (b - 32) * 5 / 9;
            return b;
        }

       
    }
}
