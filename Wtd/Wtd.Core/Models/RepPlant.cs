
using System.Collections.Generic;

namespace Wtd.Core.Models
{
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
        public List<string> Notes { get; internal set; }
    }
}
