using System;

namespace Web.Models {
    public class Record {
        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public int Current => Confirmed - Recovered - Deaths;
    }
}
