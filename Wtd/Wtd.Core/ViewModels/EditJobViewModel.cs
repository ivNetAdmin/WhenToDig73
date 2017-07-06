
using Realms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wtd.Core.Enums;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class EditJobViewModel
    {
        private readonly Realm _realm;

        public Job Job { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public EditJobViewModel(Realm realm, Job job )
        {
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            _realm = realm;
            Job = job;
            Job.CalendarDate = DateTime.SpecifyKind(job.Date.DateTime.ToLocalTime(), DateTimeKind.Local);

            var jobTypes = new List<string>();

            for (int i = 0; i<Enum.GetNames(typeof(PlantType)).Length;i++)
            {
                jobTypes.Add(Enum.GetName(typeof(PlantType), i));
            }

            JobTypes = jobTypes;

        }
     
        public IEnumerable<string> JobTypes { get; }
       
        private void Save()
        {
            var jobDate = new DateTimeOffset(Job.CalendarDate, TimeZoneInfo.Local.GetUtcOffset(Job.CalendarDate));
            var job = _realm.Find<Job>(Job.JodID);
            _realm.Write(() => {
                job.Date = jobDate.LocalDateTime;
                job.TypeImage = string.Format("t{0}.png", job.Type + 1);
            });
            NavigationService.Navigate(true);          
        }

        private void Delete()
        {
            var realm = Realm.GetInstance();
            realm.Write(()=>realm.Remove(Job));
            NavigationService.Navigate(true);
        }
    }
}
