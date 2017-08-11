
using Realms;
using System;

namespace Wtd.Core.Models
{
    public class Plant : RealmObject
    {
        [PrimaryKey]
        public string PlantID { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; }
        public string Variety { get; set; }
        public string Notes { get; set; }
    }
}
