using System.Collections.Generic;

namespace Common {
    public class CountryDto {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public IList<RecordDto> Records { get; set; }

        public CountryDto() {
            Records = new List<RecordDto>();
        }
    }
}
