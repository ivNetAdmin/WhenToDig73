
using Realms;
using System;

namespace Wtd.Core.Models
{
    public class Frost : RealmObject
    {
        [PrimaryKey]
        public string FrostID { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset Date { get; set; }

        [Ignored]
        public DateTime CalendarDate { get; set; }
    }
}
