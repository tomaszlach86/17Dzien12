﻿using Newtonsoft.Json;
using P01AplikacjaPogodaClientAPI.Models.CityModel;
using P01AplikacjaPogodaClientAPI.Models.TemperatureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace P01AplikacjaPogodaClientAPI.Tools
{
    public class AccuWeatherTool
    {

        private const string base_url = "http://dataservice.accuweather.com";
       
        private const string autocomplete_endpoint = "locations/v1/cities/autocomplete?apikey={0}&q={1}&language={2}";
        private const string current_conditions_endpoint= "currentconditions/v1/{0}?apikey={1}&language={2}";

        private const string default_languge = "pl";

        private const string api_key = " ";

        /// <summary>
        /// Downloads locations based on locatin name 
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns>Returns array of Cities. Possible null</returns>
        public async Task<City[]?> GetLocations(string locationName)
        {
            string url = base_url + "/" + string.Format(autocomplete_endpoint,api_key, locationName, default_languge);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                City[]? cities= JsonConvert.DeserializeObject<City[]>(json);
                return cities;
            }
        }

       public async Task<Weather?> GetCurrentConditions(string cityKey)
        {
            string url = base_url + "/" + string.Format(current_conditions_endpoint, cityKey, api_key, default_languge);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                Weather[]? weathers = JsonConvert.DeserializeObject<Weather[]>(json);
                
                return weathers?.FirstOrDefault();
                // ten znak zapytania działa w ten sposób, że 
                // jeżeli weathers jest null to zwróć null w przeciwym 
                // wypadku wywołaj metode firstOrDefault
            }
        }



    }
}
