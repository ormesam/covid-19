using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Record
    {
        public int RecordId { get; set; }
        public int CountryId { get; set; }
        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }

        public virtual Country Country { get; set; }
    }
}
