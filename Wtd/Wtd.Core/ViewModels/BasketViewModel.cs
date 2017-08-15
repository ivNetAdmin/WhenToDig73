
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public string Season { get; set; }

        public Command<Basket> AddOrUpdateBasketCommand { get; }
        public Command PlantClickedCommand { get; }        

        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }
        public ImageSource PlantIcon { get { return ImageSource.FromFile("plant.png"); } }

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
            PlantClickedCommand = new Command(PlantClicked);

            Seasons = GetSeasons();

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

        internal void PlantClicked()
        {
            Application.Current.MainPage = new NavigationPage(new PlantPage());
        }

        private IEnumerable<string> GetSeasons()
        {
            var seasons = new List<string>();

            var currentSeason = DateTimeOffset.Now.Year;

            Season = currentSeason.ToString();

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
