
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using Wtd.Core.Helpers;
using Wtd.Core.Models;
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

            GetReportItems();
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private ReportModel GetReportItems()
        {
            var reportModel = new ReportModel();

            var queryArray = PlantName == "All"
                ? _realm.All<Plant>().AsEnumerable().OrderBy(p => p.Description)
                : _realm.All<Plant>().Where(p => p.Description == PlantName).AsEnumerable().OrderBy(p => p.Description);

            foreach (var plant in new List<Plant>(queryArray))
            {
                var repPlant = new RepPlant
                {
                    Name = plant.Description,
                    Varieties = GetVarieties(plant.Description)
                };

                reportModel.Plants.Add(repPlant);
            }

            return reportModel;
        }

        private List<RepVariety> GetVarieties(string plantName)
        {
            var varieties = new List<RepVariety>();

            var queryArray = _realm.All<Basket>().Where(p => p.PlantName.Contains(PlantName)).AsEnumerable().OrderBy(p => p.PlantName);

            foreach (var basket in new List<Basket>(queryArray))
            {
                var repVariety = new RepVariety
                {
                    Name = basket.PlantName.Substring(basket.PlantName.IndexOf("["), basket.PlantName.Length - 1),
                    Yield = basket.Yield
                };

                varieties.Add(repVariety);

            }
            return varieties;
        }
    }
}
