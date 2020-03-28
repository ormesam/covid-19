using System.Collections.Generic;
using System.Linq;

namespace Web.Models {
    public class Country {
        public string Name { get; set; }
        public IList<Record> Records { get; set; }
        public int CurrentConfirmed => Records.Last().Confirmed;
        public int CurrentDeaths => Records.Last().Deaths;
        public int CurrentRecovered => Records.Last().Recovered;
        public int Current => Records.Last().Current;

        public Country() {
            Records = new List<Record>();
        }
    }
}
