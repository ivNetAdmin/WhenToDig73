
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
    public class FrostViewModel : BaseModel
    {
        private readonly Realm _realm;

        public Command HelpClickedCommand { get; }
        public Command JobClickedCommand { get; }
        public Command AddOrUpdateFrostCommand { get; }

        public ImageSource HelpIcon { get { return ImageSource.FromFile("help.png"); } }

        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }
        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }

        private ObservableCollection<Frost> _frostList = new ObservableCollection<Frost>();
        public ObservableCollection<Frost> FrostList
        {
            get { return _frostList; }
            set
            {
                _frostList = value;
                OnPropertyChanged();
            }
        }

        public FrostViewModel()
        {
            _realm = Realm.GetInstance();

            HelpClickedCommand = new Command(HelpClicked);
            JobClickedCommand = new Command(JobClicked);

            AddOrUpdateFrostCommand = new Command<Frost>(AddOrUpdateFrost);

            GetFrostList();
        }

        private void GetFrostList()
        {

            _frostList.Clear();
            var queryArray = _realm.All<Frost>().AsEnumerable().OrderBy(f => f.Date);

            foreach (var frost in new List<Frost>(queryArray))
            {
                _frostList.Add(frost);
            }

            FrostList = _frostList;
        }

        internal void HelpClicked()
        {
            var vm = new HelpViewModel(_realm, "frost");
            NavigationService.Navigate(vm);
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        internal void AddOrUpdateFrost(Frost frost)
        {
            if (frost == null)
            {
                frost = new Frost { Date = DateTimeOffset.Now };
                _realm.Write(() => _realm.Add(frost, update: true));
            }
            else
            {
                frost = _realm.All<Frost>().Where(f => f.FrostID == frost.FrostID).FirstOrDefault();
            }

            var vm = new EditFrostViewModel(_realm, frost);
            NavigationService.Navigate(vm);
        }

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs)
        {
            GetFrostList();
        }
    }
}
