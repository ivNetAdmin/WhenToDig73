
using Realms;
using System;
using System.Collections.Generic;
using Wtd.Core.Helpers;
using Wtd.Core.Views;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class ReportViewModel : BaseModel
    {
        private readonly Realm _realm;

        public string PlantName { get; set; }
        public string Season { get; set; }
        public string JobType { get; set; }

        public IEnumerable<string> JobTypes { get; }
        public IEnumerable<string> PlantNames { get; }
        public IEnumerable<string> Seasons { get; }

        public Command JobClickedCommand { get; }

        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }

        public ReportViewModel()
        {
            _realm = Realm.GetInstance();

            JobClickedCommand = new Command(JobClicked);

            JobTypes = ListHelper.GetJobTypes(true);
            PlantNames = ListHelper.GetPlantNames(_realm, false, true);
            Seasons = ListHelper.GetSeasons(_realm, true);

            PlantName = "All";
            Season = DateTimeOffset.Now.Year.ToString();
            JobType = string.Empty;
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
