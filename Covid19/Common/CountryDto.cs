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

        public int GetEstimatedRecovered(RecordDto record) {
            // Assuming it takes on average 3 weeks to recover
            int? numberOfConfirmed = Records
                .Where(row => row.Date == record.Date.AddDays(-21))
                .Select(row => row.AccumulatedConfirmed)
                .SingleOrDefault();

            if (numberOfConfirmed == null) {
                return record.AccumulatedRecovered;
            }

            int estimatedRecoveredNotRecorded = numberOfConfirmed.Value - record.AccumulatedDeaths - record.AccumulatedRecovered;

            if (estimatedRecoveredNotRecorded < 0) {
                estimatedRecoveredNotRecorded = 0;
            }

            return record.AccumulatedRecovered + estimatedRecoveredNotRecorded;
        }

        public int GetEstimatedCurrent(RecordDto record) {
            return record.AccumulatedConfirmed - GetEstimatedRecovered(record) - record.AccumulatedDeaths;
        }
    }
}
