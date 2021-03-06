﻿
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wtd.Core.Helpers;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Wtd.Core.Views;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    class BasketViewModel : BaseModel
    {
        private readonly Realm _realm;

        public IEnumerable<string> Seasons { get; }

        private  string _season;
        public string Season
        {
            get { return _season; }
            set
            {
                if (_season != value)
                {
                    _season = value;
                    OnPropertyChanged();
                }
            }
        }

        public Command HelpClickedCommand { get; }
        public Command<Basket> AddOrUpdateBasketCommand { get; }
        public Command JobClickedCommand { get; }

        public ImageSource HelpIcon { get { return ImageSource.FromFile("help.png"); } }

        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }
        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }

        private ObservableCollection<Basket> _basketList = new ObservableCollection<Basket>();
        public ObservableCollection<Basket> BasketList
        {
            get { return _basketList; }
            set
            {
                _basketList = value;
                OnPropertyChanged();
            }
        }

        public BasketViewModel()
        {
            _realm = Realm.GetInstance();

            AddOrUpdateBasketCommand = new Command<Basket>(AddOrUpdateBasket);
            JobClickedCommand = new Command(JobClicked);
            HelpClickedCommand = new Command(HelpClicked);

            _season = DateTimeOffset.Now.Year.ToString();

            Seasons = ListHelper.GetSeasons(_realm, true);

            GetBaskets();
        }

        private void GetBaskets()
        {
            _basketList.Clear();
            var queryArray = _realm.All<Basket>().AsEnumerable().OrderBy(p => p.PlantName);

            foreach (var basket in new List<Basket>(queryArray))
            {
                _basketList.Add(basket);
            }

            BasketList = _basketList;
        }

        internal void AddOrUpdateBasket(Basket basket)
        {
            if (basket == null)
            {
                basket = new Basket {Season=string.Empty, PlantName = string.Empty, Yield=string.Empty, Notes = string.Empty };
                _realm.Write(() => _realm.Add(basket, update: true));
            }
            else
            {
                basket = _realm.All<Basket>().Where(b => b.BasketID == basket.BasketID).FirstOrDefault();
            }

            var vm = new EditBasketViewModel(_realm, basket);
            NavigationService.Navigate(vm);
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        internal void HelpClicked()
        {
            var vm = new HelpViewModel(_realm, "yield");
            NavigationService.Navigate(vm);
        }

        private IEnumerable<string> GetSeasons()
        {
            var seasons = new List<string>();

            // get first season
            var job = _realm.All<Job>().OrderByDescending(j => j.Date).FirstOrDefault();
            if (job == null)
            {
                seasons.Add(_season);
            }
            else
            {
                var firstSeason = job.Date.Year;

                for (int season = firstSeason; season <= Convert.ToInt32(_season); season++)
                {
                    seasons.Add(season.ToString());
                }
            }
            return seasons;
        }

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs)
        {
            GetBaskets();
        }
    }
}
