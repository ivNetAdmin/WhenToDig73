
using System;
using System.Collections.Generic;

namespace Wtd.Core.Models
{
    public class ReportModel
    {
        public ReportModel()
        {
            Plants = new List<RepPlant>();
        }

        public List<RepPlant> Plants { get; set; }
    }

    public class RepPlant
    {
        public RepPlant()
        {
            Varieties = new List<RepVariety>();
            Jobs = new List<RepJob>();
        }

        public string Name { get; set; }
        public List<RepVariety> Varieties { get; set; }
        public List<RepJob> Jobs { get; set; }
    }

    public class RepVariety
    {
        public string Name { get; set; }
        public string Yield { get; set; }
    }

    public class RepJob
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
