using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Record
    {
        public int RecordId { get; set; }
        public int CountryId { get; set; }
        public DateTime Date { get; set; }
        public int AccumulatedConfirmed { get; set; }
        public int AccumulatedRecovered { get; set; }
        public int AccumulatedDeaths { get; set; }
        public int NewConfirmed { get; set; }
        public int NewRecovered { get; set; }
        public int NewDeaths { get; set; }

        public virtual Country Country { get; set; }
    }
}
