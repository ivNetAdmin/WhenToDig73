
using Realms;
using System.Windows.Input;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class EditPlantViewModel
    {
        //private readonly Realm _realm;

        public Plant Plant { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ImageSource SaveIcon { get { return ImageSource.FromFile("save.png"); } }
        public ImageSource DeleteIcon { get { return ImageSource.FromFile("delete.png"); } }


        public EditPlantViewModel(Plant plant)
        {
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            //_realm = realm;
            Plant = plant;
        }

        private void Save()
        {
            ////var jobDate = new DateTimeOffset(Job.CalendarDate, TimeZoneInfo.Local.GetUtcOffset(Job.CalendarDate));
            //var plant = _realm.Find<Plant>(Plant.PlantID);
            //_realm.Write(() => {
            //    plant.Variety = plant.Variety;
            //});
            NavigationService.Navigate(true);
        }

        private void Delete()
        {
            var realm = Realm.GetInstance();
            realm.Write(() => realm.Remove(Plant));
            NavigationService.Navigate(true);
        }
    }
}
