namespace Common {
    public class CountryOverviewDto {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public int Current => Confirmed - Recovered - Deaths;
    }
}
