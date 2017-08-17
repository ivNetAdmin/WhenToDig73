
using Realms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wtd.Core.Helpers;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class EditFrostViewModel : BaseModel
    {
        private readonly Realm _realm;

        public Frost Frost { get; }

        public IEnumerable<string> Seasons { get; }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ImageSource SaveIcon { get { return ImageSource.FromFile("save.png"); } }
        public ImageSource DeleteIcon { get { return ImageSource.FromFile("delete.png"); } }

        public EditFrostViewModel(Realm realm, Frost frost)
        {
            _realm = realm;
            Frost = frost;
            Frost.CalendarDate = DateTime.SpecifyKind(frost.Date.DateTime.ToLocalTime(), DateTimeKind.Local);

            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            Seasons = ListHelper.GetSeasons(_realm);
        }

        private void Save()
        {
            var frostDate = new DateTimeOffset(Frost.CalendarDate, TimeZoneInfo.Local.GetUtcOffset(Frost.CalendarDate));
            var frost = _realm.Find<Frost>(Frost.FrostID);
            _realm.Write(() => {
                frost.Date = frostDate.LocalDateTime;               
            });
            NavigationService.Navigate(true);
        }

        private void Delete()
        {
            var realm = Realm.GetInstance();
            realm.Write(() => realm.Remove(Frost));
            NavigationService.Navigate(true);
        }
    }
}
