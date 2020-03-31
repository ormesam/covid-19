namespace Common {
    public static class Constants {
#if DEBUG
        public const string ApiUrl = "http://localhost:62702";
#else
        public const string ApiUrl = "https://covid-19-api.samorme.co.uk";
#endif
    }
}
