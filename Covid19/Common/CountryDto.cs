using System.Collections.Generic;
using System.Linq;

namespace Common {
    public class CountryDto {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public IList<RecordDto> Records { get; set; }
        public RecordDto LastRecord => Records
            .OrderByDescending(i => i.Date)
            .FirstOrDefault();

        public CountryDto() {
            Records = new List<RecordDto>();
        }
    }
}
