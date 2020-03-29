using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Country
    {
        public Country()
        {
            Record = new HashSet<Record>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public int CurrentConfirmed { get; set; }
        public int CurrentRecovered { get; set; }
        public int CurrentDeaths { get; set; }

        public virtual ICollection<Record> Record { get; set; }
    }
}
