
using Realms;
using System;
using System.Collections.Generic;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class MainViewModel
    {
        private readonly Realm _realm;

        public IEnumerable<Job> JobList { get; }

        public Command<Job> AddOrUpdateJobCommand { get; }

        public MainViewModel()
        {
            //Realm.DeleteRealm(new RealmConfiguration());
            _realm = Realm.GetInstance();
            JobList = _realm.All<Job>();

            AddOrUpdateJobCommand = new Command<Job>(AddOrUpdateJob);
        }       
        internal void AddOrUpdateJob(Job job)
        {
            if(job==null)
            {
                job = new Job { Description = string.Empty, Date = DateTimeOffset.Now, Type = 1 };
            }

            _realm.Write(() => _realm.Add(job, update: true));

            var vm = new EditJobViewModel(_realm, job);
            NavigationService.Navigate(vm);
        }
    }
}
