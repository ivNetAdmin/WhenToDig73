
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

        public Command HelpClickedCommand { get; }
        public Command<Plant> AddOrUpdatePlantCommand { get; }
        public Command JobClickedCommand { get; }

        public ImageSource HelpIcon { get { return ImageSource.FromFile("help.png"); } }

        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }
        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }       

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

            HelpClickedCommand = new Command(HelpClicked);
            JobClickedCommand = new Command(JobClicked);            

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

            var vm = new EditPlantViewModel(_realm, plant);
            NavigationService.Navigate(vm);
        }

        internal void HelpClicked()
        {
            var vm = new HelpViewModel(_realm, "plant");
            NavigationService.Navigate(vm);
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }       

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs)
        {
            GetPlants();
        }
    }
}
