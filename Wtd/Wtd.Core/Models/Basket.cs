
using Realms;
using System;

namespace Wtd.Core.Models
{
    public class Basket : RealmObject
    {
        [PrimaryKey]
        public string BasketID { get; set; } = Guid.NewGuid().ToString();

        public string Season { get; set; }
        public string PlantName { get; set; }
        public string Yield { get; set; }
        public string Notes { get; set; }
      
        public string YieldImage { get; set; }       
    }
}
