
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
            JobType = "All"; // string.Empty;

            GetReportItems();
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private ReportModel GetReportItems()
        {
            var reportModel = new ReportModel();

            var tempPlantList = PlantName == "All" ? PlantNames : new List<string> { PlantName };

            foreach (var plant in tempPlantList)
            {
                if (plant != "All")
                {
                    var repPlant = new RepPlant
                    {
                        Name = plant,
                        Varieties = GetVarieties(plant),
                        Jobs = GetJobs(plant)
                    };

                    reportModel.Plants.Add(repPlant);
                }
            }

            return reportModel;
        }

        private List<RepVariety> GetVarieties(string plantName)
        {
            var varieties = new List<RepVariety>();

            var queryPlantArray = _realm.All<Plant>().Where(p => p.Description == plantName).AsEnumerable().OrderBy(p => p.Variety);

            foreach (var plant in new List<Plant>(queryPlantArray))
            {
                var fullPlantName = StringHelper.FullPlantName(plantName, plant.Variety);
                var queryArray = Season == "All"
                    ? _realm.All<Basket>().Where(p => p.PlantName == fullPlantName).AsEnumerable().OrderBy(p => p.PlantName)
                    : _realm.All<Basket>().Where(p => p.PlantName == fullPlantName && Season == Season).AsEnumerable().OrderBy(p => p.PlantName);

                var noSeasonYield = true;
                foreach (var basket in new List<Basket>(queryArray))
                {
                    var repVariety = new RepVariety
                    {
                        Name = plant.Variety,
                        Yield = basket.Yield,
                        Season = basket.Season
                    };
                    noSeasonYield = false;
                    varieties.Add(repVariety);
                }

                if (noSeasonYield)
                {
                    varieties.Add(new RepVariety { Name = plant.Variety });
                }
            }

            return varieties;

        }

        private List<RepJob> GetJobs(string plantName)
        {
            var jobs = new List<RepJob>();

            IOrderedEnumerable<Job> queryArray;
            int year;

            if (JobType == "All")
            {
                if (Season == "All")
                {
                    queryArray = _realm.All<Job>().Where(j => j.PlantName == plantName).AsEnumerable().OrderBy(j => j.Date).ThenBy(j => j.Type);
                }
                else
                {
                    queryArray = _realm.All<Job>().Where(j => j.PlantName == plantName && j.Season == Season).AsEnumerable().OrderBy(j => j.Date).ThenBy(j => j.Type);
                }
            }
            else
            {
                var jobType = (int)((Enums.JobType)Enum.Parse(typeof(Enums.JobType), JobType));
                if (Season == "All")
                {
                    queryArray = _realm.All<Job>().Where(j => j.PlantName == plantName && j.Type == jobType).AsEnumerable().OrderBy(j => j.Date).ThenBy(j => j.Type);
                }
                else
                {
                    queryArray = _realm.All<Job>().Where(j => j.PlantName == plantName && j.Type == jobType && j.Season == Season).AsEnumerable().OrderBy(j => j.Date).ThenBy(j => j.Type);
                }
            }

            foreach (var job in new List<Job>(queryArray))
            {
                jobs.Add(new RepJob
                {
                    Description = job.Description,
                    Date = job.CalendarDate,
                    Type = (Enum.GetName(typeof(Enums.JobType), job.Type))
                });
            }

            return jobs;
        }
    }
}
