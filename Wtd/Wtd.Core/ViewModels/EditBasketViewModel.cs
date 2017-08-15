﻿
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class EditBasketViewModel : BaseModel
    {
        private readonly Realm _realm;

        public Basket Basket { get; }       

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public Command YieldClickedCommand { get; }
        
        public ImageSource SaveIcon { get { return ImageSource.FromFile("save.png"); } }
        public ImageSource DeleteIcon { get { return ImageSource.FromFile("delete.png"); } }
        public ImageSource PoorBasketIcon { get { return ImageSource.FromFile("poor_basket.png"); } }
        public ImageSource AverageBasketIcon { get { return ImageSource.FromFile("average_basket.png"); } }
        public ImageSource GoodBasketIcon { get { return ImageSource.FromFile("good_basket.png"); } }

        public IEnumerable<string> PlantNames { get; }
        public IEnumerable<string> Seasons { get; }

        public EditBasketViewModel(Realm realm, Basket basket)
        {
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            YieldClickedCommand = new Command(YieldClicked);

            _realm = realm;
            Basket = basket;

            PlantNames = GetPlantNames();
            Seasons = GetSeasons();
        }

        private string _yield;
        public String Yield
        {
            get { return _yield; }
            set
            {
                if (_yield != value)
                {
                    _yield = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Save()
        {
            var basket = _realm.Find<Basket>(Basket.BasketID);
            _realm.Write(() => {
                basket.Yield = Yield;                
            });
            NavigationService.Navigate(true);
        }

        private void Delete()
        {
            var realm = Realm.GetInstance();
            realm.Write(() => realm.Remove(Basket));
            NavigationService.Navigate(true);
        }

        internal void YieldClicked(object param)
        {
            Yield = (string)param;
        }

        private IEnumerable<string> GetPlantNames()
        {
            var plantNames = new List<string>();

            var queryArray = _realm.All<Plant>().AsEnumerable().OrderBy(p => p.Description);

            foreach (var plant in new List<Plant>(queryArray))
            {
                var plantName = string.Format("{0} [{1}]", plant.Description, plant.Variety);
                if (!plantNames.Contains(plantName))
                    plantNames.Add(plantName);
            }

            return plantNames;
        }

        private IEnumerable<string> GetSeasons()
        {
            var seasons = new List<string>();

            var currentSeason = DateTimeOffset.Now.Year;

            // get first season
            var job = _realm.All<Job>().OrderByDescending(j => j.Date).FirstOrDefault();
            if (job == null)
            {
                seasons.Add(currentSeason.ToString());
            }
            else
            {
                var firstSeason = job.Date.Year;

                for (int season = firstSeason; season <= currentSeason; season++)
                {
                    seasons.Add(season.ToString());
                }
            }
            return seasons;
        }
    }
}