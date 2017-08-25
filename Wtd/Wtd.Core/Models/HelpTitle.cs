
using Realms;
using System;

namespace Wtd.Core.Models
{
    public class HelpTitle : RealmObject
    {
        [PrimaryKey]
        public string HelpTitleId { get; set; } = Guid.NewGuid().ToString();

        public string Topic { get; set; }
        public string Text { get; set; }

    }
}
