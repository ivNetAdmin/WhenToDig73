using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using Wtd.Core.Enums;
using Wtd.Core.Models;

namespace Wtd.Core.Helpers
{
    public static class ListHelper
    {
        public static IEnumerable<string> GetPlantNames(Realm _realm, bool variety = false, bool all = false)
        {
            var plantNames = new List<string>();
            if (all) plantNames.Add("All");

            var queryArray = _realm.All<Plant>().AsEnumerable().OrderBy(p => p.Description);

            foreach (var plant in new List<Plant>(queryArray))
            {

                var plantName = variety ? string.Format("{0} [{1}]", plant.Description, plant.Variety) : plant.Description;

                if (!plantNames.Contains(plantName))
                    plantNames.Add(plantName);
            }

            return plantNames;
        }

        public static IEnumerable<string> GetSeasons(Realm _realm, bool all = false)
        {
            var seasons = new List<string>();
            if (all) seasons.Add("All");
            var currentSeason = DateTimeOffset.Now.Year;

            // get first season
            var job = _realm.All<Job>().OrderByDescending(j => j.Date).FirstOrDefault();
            if (job == null)
            {
                seasons.Add(currentSeason.ToString());
            }
            else
            {
                var firstSeason = job.Date.Year;

                for (int season = firstSeason; season <= currentSeason; season++)
                {
                    seasons.Add(season.ToString());
                }
            }
            return seasons;
        }

        public static List<string> GetJobTypes(bool all = false)
        {
            var jobTypes = new List<string>();
            if (all) jobTypes.Add("All");

            for (int i = 0; i < Enum.GetNames(typeof(JobType)).Length; i++)
            {
                jobTypes.Add(Enum.GetName(typeof(JobType), i));
            }

            return jobTypes;
        }
    }
}
