using System;

namespace Common {
    public class RecordDto {
        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public int Current => Confirmed - Recovered - Deaths;
    }
}
