using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWPF.Model;
using WeatherWPF.ViewModel.Helpers;
using WeatherWPF.ViewModel.Commands;
using System.Collections.ObjectModel; 

namespace WeatherWPF.ViewModel
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        public SearchCommand SearchCommand { get; set; }
        public ObservableCollection<City> Cities { get; set; }
        // data binding to view
        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                OnPropertyChanged("Query");
            }
        }

        public async void MakeQuery()
        {
            var qCities = await AccuWeatherHelper.GetCities(Query);
            
            Cities.Clear();
            foreach (var item in qCities)
            {
                Cities.Add(item);
            }
        }

   
        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            { 
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity 
        {
            get { return selectedCity; }
            set { 
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentConditions();
            }
        }

        private async void GetCurrentConditions() 
        {
            Query = string.Empty;
            Cities.Clear();
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
        }

        public WeatherViewModel() 
        {
            /*SelectedCity = new City{ LocalizedName = "Ankara"};
            CurrentConditions = new CurrentConditions 
            {
                WeatherText = "Güneşli", 
                Temperature = new Temperature
                {
                    Metric = new Units
                    {
                        Value = 11
                    }
                }
            };*/

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            { 
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName)); 
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

   


    }
}
