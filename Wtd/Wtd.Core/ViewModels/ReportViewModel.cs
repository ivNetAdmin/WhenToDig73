
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
    public class ReportViewModel : BaseModel
    {
        private readonly Realm _realm;

        public string PlantName { get; set; }
        public string Season { get; set; }
        public string JobType { get; set; }

        public IEnumerable<string> JobTypes { get; }
        public IEnumerable<string> PlantNames { get; }
        public IEnumerable<string> Seasons { get; }

        public Command HelpClickedCommand { get; }
        public Command JobClickedCommand { get; }

        public ImageSource HelpIcon { get { return ImageSource.FromFile("help.png"); } }
        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }

        private ObservableCollection<RepLine> _reportList = new ObservableCollection<RepLine>();
        public ObservableCollection<RepLine> ReportList
        {
            get { return _reportList; }
            set
            {
                _reportList = value;
                OnPropertyChanged();
            }
        }

        public ReportViewModel()
        {
            _realm = Realm.GetInstance();

            HelpClickedCommand = new Command(HelpClicked);
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

        private void GetReportItems()
        {
            var reportItems = new List<RepPlant>();

            var tempPlantList = PlantName == "All" ? PlantNames : new List<string> { PlantName };


            foreach (var plant in tempPlantList)
            {
                if (plant != "All")
                {
                    var notes = new List<string>();
                    if (!string.IsNullOrEmpty(plant))
                    {
                        var repPlant = new RepPlant
                        {
                            Name = plant,
                            Varieties = GetVarieties(plant, notes),
                            Jobs = GetJobs(plant, notes),
                            Notes = notes
                        };
                        reportItems.Add(repPlant);
                    }
                }
            }

            GetReportLines(reportItems);
        }

        private void GetReportLines(List<RepPlant> reportItems)
        {
            _reportList.Clear();

            foreach (var reportItem in reportItems)
            {
                var row = 0;
                _reportList.Add(new RepLine { Col0 = 0, Cell0 = reportItem.Name, CellColspan0 = 4, Font="Bold", Row = row });
                row++;
                foreach (var variety in reportItem.Varieties)
                {
                    if (!string.IsNullOrEmpty(variety.Name))
                    {
                        _reportList.Add(new RepLine
                        {
                            Col0 = 0,
                            Cell0 = variety.Name,
                            CellColspan0 = 2,
                            Font = "Italic",

                            Col2 = 2,
                            Cell2 = variety.Season,
                            CellColspan2 = 1,

                            Col3 = 3,
                            Cell3 = variety.Yield,
                            CellColspan3 = 1
                        });
                        row++;
                    }
                }

                foreach (var note in reportItem.Notes)
                {
                    if (!string.IsNullOrEmpty(note))
                    {
                        _reportList.Add(new RepLine
                        {
                            Col0 = 0,
                            Cell0 = note,
                            CellColspan0 = 4,
                            Font = "None"

                        });
                        row++;
                    }
                }

                foreach (var job in reportItem.Jobs)
                {
                    if (!string.IsNullOrEmpty(job.Description))
                    {
                        _reportList.Add(new RepLine
                        {
                            Col0 = 0,
                            Cell0 = job.Description,
                            CellColspan0 = 2,
                            Font = "Italic",

                            Col2 = 2,
                            Cell2 = job.Date.ToString("dd/MM/yy"),
                            CellColspan2 = 1,

                            Col3 = 3,
                            Cell3 = job.Type,
                            CellColspan3 = 1

                        });
                        row++;
                    }
                }
            }
            ReportList = _reportList;
        }

        private List<RepVariety> GetVarieties(string plantName, List<string> notes)
        {
            var varieties = new List<RepVariety>();

            var queryPlantArray = _realm.All<Plant>().Where(p => p.Description == plantName).AsEnumerable().OrderBy(p => p.Variety);

            foreach (var plant in new List<Plant>(queryPlantArray))
            {
                notes.Add(plant.Notes);

                var fullPlantName = StringHelper.FullPlantName(plantName, plant.Variety);
                var queryArray = Season == "All"
                    ? _realm.All<Basket>().Where(p => p.PlantName == fullPlantName).AsEnumerable().OrderBy(p => p.PlantName)
                    : _realm.All<Basket>().Where(p => p.PlantName == fullPlantName && Season == Season).AsEnumerable().OrderBy(p => p.PlantName);

                var noSeasonYield = true;
                foreach (var basket in new List<Basket>(queryArray))
                {
                    notes.Add(basket.Notes);

                    var repVariety = new RepVariety
                    {
                        Name = plant.Variety,
                        Yield = basket.YieldImage,
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

        private List<RepJob> GetJobs(string plantName, List<string> notes)
        {
            var jobs = new List<RepJob>();

            IOrderedEnumerable<Job> queryArray;

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
                notes.Add(job.Notes);

                jobs.Add(new RepJob
                {
                    Description = string.Format("[{0}] {1}", job.PlantName,job.Description),
                    Date = job.Date,
                    Type = job.TypeImage
                });
            }

            return jobs;
        }

        internal void HelpClicked()
        {
            var vm = new HelpViewModel(_realm, "report");
            NavigationService.Navigate(vm);
        }

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs)
        {
            GetReportItems();
        }
    }
}
