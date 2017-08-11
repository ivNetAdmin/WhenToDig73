
using Realms;
using System;
using Xamarin.Forms;

namespace Wtd.Core.Models
{
    public class Job : RealmObject
    {
        [PrimaryKey]
        public string JodID { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string TypeImage { get; set; }

        [Ignored]
        public  DateTime CalendarDate { get; set; }

        [Ignored]
        public Color TextColor { get; set; }
    }
}
