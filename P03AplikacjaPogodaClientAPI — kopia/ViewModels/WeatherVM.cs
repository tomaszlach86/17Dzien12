﻿using P03AplikacjaPogodaClientAPI.Models.CityModel;
using P03AplikacjaPogodaClientAPI.Tools;
using P03AplikacjaPogodaClientAPI.ViewModels.Commands;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03AplikacjaPogodaClientAPI.ViewModels
{
    public class WeatherVM : INotifyPropertyChanged
    {
		private string city = "warszawa";
		AccuWeatherTool accuWeatherTool;

        public string City
		{
			get { return city; }
			set { city = value; OnPropertyChanged("City"); }
		}

		public ObservableCollection<CityVM> Cities { get; set; }

		private CityVM selectedCity;

		public CityVM SelectedCity
        {
			get { return selectedCity; }
			set 
			{ 
				selectedCity = value;
               // OnPropertyChanged("SelectedCity");
                GetCurrentWeather(selectedCity);
			}
		}

		private CurrentConditionsOfCity currentConditionsOfCity;

		public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CurrentConditionsOfCity CurrentConditionsOfCity
        {
			get { return currentConditionsOfCity; }
			set { 
				currentConditionsOfCity = value;
                OnPropertyChanged("CurrentConditionsOfCity");
            }
		}



		public WeatherVM()
		{
			SearchCommand = new SearchCommand(this);
			accuWeatherTool = new AccuWeatherTool();
			Cities = new ObservableCollection<CityVM>();
        }

		public SearchCommand SearchCommand { get; set; }

		public async void FindCities()
		{
            City[]? cities = await accuWeatherTool.GetLocations(city);

			Cities.Clear();
			foreach (var c in cities)
				Cities.Add(
					new CityVM()
					{
						CityName = c.LocalizedName,
						Country = c.Country.LocalizedName,
						Key = c.Key
					}
					);
			// tutaj np będzie dodwalnie się do repozytorium bazodanowego 
        }

		public async void GetCurrentWeather(CityVM city)
		{
			var weather=  await accuWeatherTool.GetCurrentConditions(city.Key);

			CurrentConditionsOfCity = new CurrentConditionsOfCity()
			{
				WeatherText = weather.WeatherText,
				TemperatureValue = weather.Temperature.Metric.Value,
				HasPrecipitation = weather.HasPrecipitation,
			};

        }

    }
}
