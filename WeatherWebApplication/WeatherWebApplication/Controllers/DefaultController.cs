using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using System.Web.Security.AntiXss;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityValue"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("Weather")]
        public HttpResponseMessage weatherApi(CityWeather cityValue)
        {
            CityWeather weather = new CityWeather();
            var city = new CityWeather();
            city.city = cityValue.city;
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
            //sbURL.Append("&format = json");
            sbURL.Append(@"&diagnostics=true");

            // Download string (XML data) from REST API response
            // Downloads the requested resource as a string.
            string XMLresult = wc.DownloadString(sbURL.ToString());

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLresult);

            //using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(XMLresult), XmlDictionaryReaderQuotas.Max))
            //{
            //    XElement xml = XElement.Load(reader);
            //    xmlDocument.LoadXml(xml.ToString());
            //}

            XmlNode node = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(6);
            XmlNode week2 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(7);
            XmlNode week3 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(8);
            XmlNode week4 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(9);
            XmlNode week5 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(10);
            XmlNode week6 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(11);
            XmlNode week7 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(12);
            XmlNode week8 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(13);
            XmlNode week9 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(14);
            XmlNode week10 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(15);
            XmlNode node1 = xmlDocument.SelectSingleNode("/query/results/channel/item").ChildNodes.Item(5);
            XmlNode pnode = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(9);
            XmlNode pnode1 = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(10);
            XmlNode wind = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(8);
            XmlNode country = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(7);
            XmlNode astronomy = xmlDocument.SelectSingleNode("/query/results/channel").ChildNodes.Item(10);

            CityWeather Temp = new CityWeather();
            Temp.humidity = pnode.Attributes.Item(1).Value;
            Temp.pressure = pnode.Attributes.Item(2).Value;
            Temp.rising = pnode.Attributes.Item(3).Value;
            Temp.visibility = pnode.Attributes.Item(4).Value;

            Temp.country = country.Attributes.Item(2).Value;

            Temp.chill = int.Parse(wind.Attributes.Item(1).Value);
            Temp.direction = wind.Attributes.Item(2).Value;
            Temp.speed = wind.Attributes.Item(3).Value;

            Temp.sunrise = astronomy.Attributes.Item(1).Value;
            Temp.sunset = astronomy.Attributes.Item(2).Value;

            Temp.high = int.Parse(node.Attributes.Item(4).Value);
            int thigh = convertCelcius(Temp.high);
            Temp.low = int.Parse(node.Attributes.Item(5).Value);
            int tlow = convertCelcius(Temp.low);
            Temp.temperature = int.Parse(node1.Attributes.Item(3).Value);
            int temperature = convertCelcius(Temp.temperature);
            Temp.date = node1.Attributes.Item(2).Value;
            Temp.text = node1.Attributes.Item(4).Value;

            Temp.date1 = week2.Attributes.Item(2).Value;
            Temp.high1 = int.Parse(week2.Attributes.Item(4).Value);
            int thigh1 = convertCelcius(Temp.high1);
            Temp.low1 = int.Parse(week2.Attributes.Item(5).Value);
            int tlow1 = convertCelcius(Temp.low1);
            Temp.day1 = week2.Attributes.Item(3).Value;

            Temp.date2 = week3.Attributes.Item(2).Value;
            Temp.high2 = int.Parse(week3.Attributes.Item(4).Value);
            int thigh2 = convertCelcius(Temp.high2);
            Temp.low2 = int.Parse(week3.Attributes.Item(5).Value);
            int tlow2 = convertCelcius(Temp.low2);
            Temp.day2 = week3.Attributes.Item(3).Value;

            Temp.date3 = week4.Attributes.Item(2).Value;
            Temp.high3 = int.Parse(week4.Attributes.Item(4).Value);
            int thigh3 = convertCelcius(Temp.high3);
            Temp.low3 = int.Parse(week4.Attributes.Item(5).Value);
            int tlow3 = convertCelcius(Temp.low3);
            Temp.day3 = week4.Attributes.Item(3).Value;

            Temp.date4 = week5.Attributes.Item(2).Value;
            Temp.high4 = int.Parse(week5.Attributes.Item(4).Value);
            int thigh4 = convertCelcius(Temp.high4);
            Temp.low4= int.Parse(week5.Attributes.Item(5).Value);
            int tlow4 = convertCelcius(Temp.low4);
            Temp.day4 = week5.Attributes.Item(3).Value;

            Temp.date5 = week6.Attributes.Item(2).Value;
            Temp.high5 = int.Parse(week6.Attributes.Item(4).Value);
            int thigh5 = convertCelcius(Temp.high5);
            Temp.low5 = int.Parse(week6.Attributes.Item(5).Value);
            int tlow5 = convertCelcius(Temp.low5);
            Temp.day5 = week6.Attributes.Item(3).Value;

            Temp.date6 = week7.Attributes.Item(2).Value;
            Temp.high6 = int.Parse(week7.Attributes.Item(4).Value);
            int thigh6 = convertCelcius(Temp.high6);
            Temp.low6 = int.Parse(week7.Attributes.Item(5).Value);
            int tlow6 = convertCelcius(Temp.low6);
            Temp.day6 = week7.Attributes.Item(3).Value;

            Temp.date7 = week8.Attributes.Item(2).Value;
            Temp.high7 = int.Parse(week8.Attributes.Item(4).Value);
            int thigh7 = convertCelcius(Temp.high7);
            Temp.low7 = int.Parse(week8.Attributes.Item(5).Value);
            int tlow7 = convertCelcius(Temp.low7);
            Temp.day7 = week8.Attributes.Item(3).Value;

            Temp.date8 = week9.Attributes.Item(2).Value;
            Temp.high8 = int.Parse(week9.Attributes.Item(4).Value);
            int thigh8 = convertCelcius(Temp.high8);
            Temp.low8 = int.Parse(week9.Attributes.Item(5).Value);
            int tlow8 = convertCelcius(Temp.low8);
            Temp.day8 = week9.Attributes.Item(3).Value;

            Temp.date9 = week10.Attributes.Item(2).Value;
            Temp.high9 = int.Parse(week10.Attributes.Item(4).Value);
            int thigh9 = convertCelcius(Temp.high9);
            Temp.low9 = int.Parse(week10.Attributes.Item(5).Value);
            int tlow9 = convertCelcius(Temp.low9);
            Temp.day9 = week10.Attributes.Item(3).Value;

            AddCity(cityValue.city, temperature, Temp.humidity, Temp.visibility);

            var result = new { cityValue.city,Temp.country,Temp.date, temperature, Temp.humidity, Temp.pressure,
                               Temp.rising,Temp.visibility,Temp.chill, Temp.direction,Temp.speed,Temp.sunrise,
                               Temp.sunset,thigh, tlow ,Temp.text, Temp.date1,Temp.day1,thigh1,tlow1,Temp.date2,
                               Temp.day2,thigh2,tlow2,Temp.date3,Temp.day3,thigh3,tlow3,Temp.date4,Temp.day4,
                               thigh4,tlow4,Temp.date5,Temp.day5,tlow5,thigh5,Temp.date6,Temp.day6,thigh6,
                               tlow6,Temp.date7,Temp.day7,thigh7,tlow7,Temp.date8,Temp.day8,thigh8,tlow8,
                               Temp.date9,Temp.day9,thigh9,tlow9};
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK , result);
            //response.Content = new StringContent(xmlDocument.OuterXml, Encoding.UTF8, "application/xml");
            return response;
        }

        /// <summary>
        /// This action is used to add the values to the database
        /// </summary>
        /// <param name="city"></param>
        /// <param name="temp"></param>
        /// <param name="humidity"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        public string  AddCity(string city, int temp, string humidity, string visibility)
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
                return city;
            }
        }

        public int convertCelcius(int b)
        {
            b = (b - 32) * 5 / 9;
            return b;
        }
    }
}
