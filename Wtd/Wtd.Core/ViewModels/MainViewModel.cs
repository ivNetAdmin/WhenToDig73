
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
    public class MainViewModel : BaseModel
    {
        private readonly Realm _realm;
        private DateTimeOffset _currentDate;

        public Command<Job> AddOrUpdateJobCommand { get; }
        public Command ChangeCalendarCommand { get; }
        public Command CalendarDatePickedCommand{ get; }

        public Command HelpClickedCommand { get; }
        public Command PlantClickedCommand { get; }
        public Command ReportClickedCommand { get; }
        public Command BasketClickedCommand { get; }
        public Command FrostClickedCommand { get; }

        public ImageSource HelpIcon { get { return ImageSource.FromFile("help.png"); } }

        public ImageSource AddIcon { get { return ImageSource.FromFile("add.png"); } }
        public ImageSource ReportIcon { get { return ImageSource.FromFile("report.png"); } }
        public ImageSource BasketIcon { get { return ImageSource.FromFile("basket.png"); } }
        public ImageSource PlantIcon { get { return ImageSource.FromFile("plant.png"); } }
        public ImageSource FrostIcon { get { return ImageSource.FromFile("frost.png"); } }
        

        public MainViewModel()
        {
            //Realm.DeleteRealm(new RealmConfiguration());
            _realm = Realm.GetInstance();
            _currentDate = DateTimeOffset.Now;
            _dateRangeDate = new ObservableCollection<string>();
            _calendarDates = new ObservableCollection<DateTime>();
            _dateRangeJobType = new ObservableCollection<string>();
            _dateRangeTextColour = new ObservableCollection<Color>();
            _dateRangeVisible = new ObservableCollection<Boolean>();
                      
            AddOrUpdateJobCommand = new Command<Job>(AddOrUpdateJob);
            ChangeCalendarCommand = new Command(ChangeCalendar);
            CalendarDatePickedCommand = new Command(CalendarDatePicked);

            HelpClickedCommand = new Command(HelpClicked);
            PlantClickedCommand = new Command(PlantClicked);
            BasketClickedCommand = new Command(BasketClicked);
            ReportClickedCommand = new Command(ReportClicked);
            FrostClickedCommand = new Command(FrostClicked);

            SetDateRange();
        }

        private string _displayCalendarDate;
        public String DisplayCalendarDate
        {
            get { return _displayCalendarDate; }
            set
            {
                if (_displayCalendarDate != value)
                {
                    _displayCalendarDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<DateTime> _calendarDates;
        public ObservableCollection<DateTime> CalendarDates
        {
            get { return _calendarDates; }
            set
            {
                if (_calendarDates != value)
                {
                    _calendarDates = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _dateRangeDate;
        public ObservableCollection<string> DateRangeDate
        {
            get { return _dateRangeDate; }
            set
            {
                _dateRangeDate = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Color> _dateRangeTextColour;
        public ObservableCollection<Color> DateRangeTextColour
        {
            get { return _dateRangeTextColour; }
            set
            {
                _dateRangeTextColour = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Boolean> _dateRangeVisible;
        public ObservableCollection<Boolean> DateRangeVisible
        {
            get { return _dateRangeVisible; }
            set
            {
                _dateRangeVisible = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _dateRangeJobType;
        public ObservableCollection<String> DateRangeJobType
        {
            get { return _dateRangeJobType; }
            set
            {
                _dateRangeJobType = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Job> _jobList = new ObservableCollection<Job>();
        public ObservableCollection<Job> JobList
        {
            get { return _jobList; }
            set
            {
                _jobList = value;
                OnPropertyChanged();
            }
        }

        private void SetDateRange()
        {
            _jobList.Clear();
            _calendarDates.Clear();
            _dateRangeDate.Clear();
            _dateRangeTextColour.Clear();
            _dateRangeVisible.Clear();
            _dateRangeJobType.Clear();

            var fistOfTheMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            var firstDayofMonth = fistOfTheMonth.DayOfWeek;
            var startDate = (int)firstDayofMonth == 0 ? fistOfTheMonth.AddDays(-6) : fistOfTheMonth.AddDays(-1 * ((int)firstDayofMonth - 1));
            var endDate = startDate.AddDays(42);
            var visible = true;

            DisplayCalendarDate = fistOfTheMonth.ToString("MMM yyyy");

            var queryArray = _realm.All<Job>().AsEnumerable().Where(j => j.Date >= startDate && j.Date <= endDate).OrderBy(j => j.Date).ThenBy(j => j.Type);

            foreach (var job in new List<Job>(queryArray))
            {
                var fontColour = Color.DarkSlateGray;
                if (job.Date.Month < _currentDate.Month || job.Date.Month > _currentDate.Month) fontColour = Color.Gray;
                
                job.TextColor = fontColour;
                _jobList.Add(job);
            }
         

            for (int i = 0; i < 42; i++)
            {
                var date = startDate.Date.AddDays(i);
                var day = date.Day;
                var fontColour = Color.White;

                if (date.Day == DateTime.Now.Day
                    && date.Month == DateTime.Now.Month
                    && date.Year == DateTime.Now.Year) fontColour = Color.Aqua;
                if (i < 7 && day > 10) fontColour = Color.Gray;
                if (i > 20 && day < 10) fontColour = Color.Gray;

                _dateRangeTextColour.Add(fontColour);
                _dateRangeDate.Add(day.ToString("D2"));
                _calendarDates.Add(date);

                if (i == 35 && day < 10) visible = false;
                _dateRangeVisible.Add(visible);

                SetCellBackgroundImage(date);
            }

            DateRangeJobType = _dateRangeJobType;
            DateRangeDate = _dateRangeDate;
            DateRangeTextColour = _dateRangeTextColour;
            DateRangeVisible = _dateRangeVisible;
            CalendarDates = _calendarDates;
            JobList = _jobList;
        }

        private void SetCellBackgroundImage(DateTime date)
        {
            var startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            var queryArray = _realm.All<Job>().AsEnumerable()
                .Where(j => j.Date >= startDate
                && j.Date <= endDate).OrderBy(j => j.Date).ThenBy(j => j.Type);

            var jobs = new List<Job>(queryArray);

            var types = new string[3];

            foreach (var job in jobs)
            {
                switch (job.Type)
                {
                    case 0:
                        types[0] = "1";
                        break;
                    case 1:
                        types[1] = "2";
                        break;
                    case 2:
                        types[2] = "3";
                        break;
                }
            }
            var image = string.Format("tt{0}{1}{2}.png", types[0], types[1], types[2]);
            if (image.Length == 6)
            {
                _dateRangeJobType.Add("ttblank.png");
            }
            else
            {
                _dateRangeJobType.Add(image);
            }

        }

        internal void AddOrUpdateJob(Job job)
        {
            if (job == null || !job.IsManaged)
            {
                if (job == null)
                    job = new Job { Description = string.Empty, PlantName = string.Empty, Notes = string.Empty, Date = DateTimeOffset.Now, Type = 1 };

                _realm.Write(() => _realm.Add(job, update: true));
            }
            else
            {
                job = _realm.All<Job>().Where(j => j.JodID == job.JodID).FirstOrDefault();
            }

            var vm = new EditJobViewModel(_realm, job);
            NavigationService.Navigate(vm);
        }

        internal void ChangeCalendar(object param)
        {
            switch ((string)param)
            {
                case "NextMonth":
                    _currentDate = _currentDate.AddMonths(1);
                    break;
                case "NextYear":
                    _currentDate = _currentDate.AddYears(1);
                    break;
                case "LastMonth":
                    _currentDate = _currentDate.AddMonths(-1);
                    break;
                case "LastYear":
                    _currentDate = _currentDate.AddYears(-1);
                    break;
            }
            SetDateRange();
        }    

        internal void CalendarDatePicked (object param)
        {
            var job = new Job { Description = string.Empty, PlantName = string.Empty, Notes = string.Empty, Date = new DateTimeOffset((DateTime)param), Type = 1 };
            AddOrUpdateJob(job);
        }

        internal void HelpClicked()
        {
            var vm = new HelpViewModel(_realm, "main");
            NavigationService.Navigate(vm);
        }

        internal void PlantClicked()
        {
            Application.Current.MainPage = new NavigationPage(new PlantPage());
        }

        internal void BasketClicked()
        {
            Application.Current.MainPage = new NavigationPage(new BasketPage());
        }

        internal void ReportClicked()
        {
            Application.Current.MainPage = new NavigationPage(new ReportPage());
        }

        internal void FrostClicked()
        {

            Application.Current.MainPage = new NavigationPage(new FrostPage());
        }        

        protected override void CurrentPageOnAppearing(object sender, EventArgs eventArgs) {
            SetDateRange();
        }
    }
}
