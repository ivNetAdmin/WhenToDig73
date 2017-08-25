
using Realms;
using System;

namespace Wtd.Core.Models
{
    public class HelpSection : RealmObject
    {
        [PrimaryKey]
        public string HelpSectionId { get; set; } = Guid.NewGuid().ToString();

        public string Topic { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        
    }
}
