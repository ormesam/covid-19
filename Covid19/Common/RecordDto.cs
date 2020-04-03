using System;

namespace Common {
    public class RecordDto {
        public DateTime Date { get; set; }
        public int AccumulatedConfirmed { get; set; }
        public int AccumulatedRecovered { get; set; }
        public int AccumulatedDeaths { get; set; }
        public int NewConfirmed { get; set; }
        public int NewRecovered { get; set; }
        public int NewDeaths { get; set; }
        public int CurrentlyActive => AccumulatedConfirmed - AccumulatedRecovered - AccumulatedDeaths;
    }
}
