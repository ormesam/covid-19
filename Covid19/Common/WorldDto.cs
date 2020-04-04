using System.Collections.Generic;
using System.Linq;

namespace Common {
    public class WorldDto {
        public IList<RecordDto> Records { get; set; }
        public RecordDto LastRecord => Records
            .OrderByDescending(i => i.Date)
            .FirstOrDefault();

        public WorldDto() {
            Records = new List<RecordDto>();
        }
    }
}
