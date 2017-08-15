
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
    public class PlantViewModel : BaseModel
    {
        private readonly Realm _realm;

        public Command<Plant> AddOrUpdatePlantCommand { get; }
        public Command JobClickedCommand { get; }
        public Command BasketClickedCommand { get; }

        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }
        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }
        public ImageSource BasketIcon { get { return ImageSource.FromFile("basket.png"); } }

        private ObservableCollection<Plant> _plantList = new ObservableCollection<Plant>();
        public ObservableCollection<Plant> PlantList
        {
            get { return _plantList; }
            set
            {
                _plantList = value;
                OnPropertyChanged();
            }
        }

        public PlantViewModel()
        {
            _realm = Realm.GetInstance();

            AddOrUpdatePlantCommand = new Command<Plant>(AddOrUpdatePlant);
            JobClickedCommand = new Command(JobClicked);
            BasketClickedCommand = new Command(BasketClicked);

            GetPlants();
        }

        private void GetPlants()
        {

            _plantList.Clear();
            var queryArray = _realm.All<Plant>().AsEnumerable().OrderBy(p => p.Description);

            foreach (var plant in new List<Plant>(queryArray))
            {
                _plantList.Add(plant);
            }         

            PlantList = _plantList;
        }

        internal void AddOrUpdatePlant(Plant plant)
        {          
            if (plant == null)
            {
                plant = new Plant { Description = string.Empty, Variety = "Common", Notes = string.Empty };
                _realm.Write(() => _realm.Add(plant, update: true));
            }
            else
            {
                plant = _realm.All<Plant>().Where(p => p.PlantID == plant.PlantID).FirstOrDefault();
            }

            var vm = new EditPlantViewModel(plant);
            NavigationService.Navigate(vm);
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        internal void BasketClicked()
        {
            Application.Current.MainPage = new NavigationPage(new BasketPage());
        }

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs)
        {
            GetPlants();
        }
    }
}
