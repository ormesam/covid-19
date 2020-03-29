using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using DataAccess.Models;
using Newtonsoft.Json;

namespace DataImport {
    class Program {
        private static int newCountryCount = 0;
        private static int newRecordCount = 0;

        static void Main(string[] args) {
            Console.WriteLine("Starting sync...");

            var newData = GetNewData().Result;

            Console.WriteLine($"{newData.Count()} countries found");

            CreateNewCountries(newData);
            CreateNewRecords(newData);
            UpdateCurrentTotals();

            Console.WriteLine($"Sync complete - {newCountryCount} new countries added - {newRecordCount} new records added");
        }

        private static void UpdateCurrentTotals() {
            using (ModelDataContext context = new ModelDataContext()) {
                var countries = context.Country.ToList();

                foreach (var country in countries) {
                    var lastRecord = context.Record
                        .Where(row => row.CountryId == country.CountryId)
                        .OrderByDescending(row => row.Date)
                        .FirstOrDefault();

                    country.CurrentConfirmed = lastRecord?.Confirmed ?? 0;
                    country.CurrentDeaths = lastRecord?.Deaths ?? 0;
                    country.CurrentRecovered = lastRecord?.Recovered ?? 0;

                    context.SaveChanges();
                }
            }

            Console.WriteLine("Current totals updated");
        }

        private static void CreateNewRecords(IEnumerable<CountryDto> newData) {
            using (ModelDataContext context = new ModelDataContext()) {
                var countrys = context.Country
                    .Select(row => new {
                        row.CountryId,
                        row.Name,
                    })
                    .ToList();

                foreach (var country in countrys) {
                    var currentRecordDates = context.Record
                        .Where(row => row.CountryId == country.CountryId)
                        .Select(row => row.Date)
                        .ToList();

                    var newRecords = newData
                        .Where(row => row.Name == country.Name)
                        .SelectMany(row => row.Records)
                        .Where(row => !currentRecordDates.Contains(row.Date))
                        .ToList();

                    foreach (var record in newRecords) {
                        Console.WriteLine($"Creating record for {country.Name} - {record.Date.ToShortDateString()}");

                        context.Record.Add(new Record {
                            Confirmed = record.Confirmed,
                            CountryId = country.CountryId,
                            Date = record.Date,
                            Deaths = record.Deaths,
                            Recovered = record.Recovered,
                        });

                        context.SaveChanges();

                        newRecordCount++;
                    }
                }
            }
        }

        private static void CreateNewCountries(IEnumerable<CountryDto> newData) {
            using (ModelDataContext context = new ModelDataContext()) {
                var countryNames = context.Country
                    .Select(row => row.Name)
                    .ToList();

                var newCountries = newData
                    .Where(row => !countryNames.Contains(row.Name))
                    .ToList();

                foreach (var country in newCountries) {
                    Console.WriteLine($"Creating {country.Name}");

                    context.Country.Add(new Country {
                        Name = country.Name,
                    });

                    context.SaveChanges();

                    newCountryCount++;
                }
            }
        }

        private static async Task<IEnumerable<CountryDto>> GetNewData() {
            using (HttpClient client = new HttpClient()) {
                string jsonData = await client.GetStringAsync("https://pomber.github.io/covid19/timeseries.json");

                var data = JsonConvert.DeserializeObject<IDictionary<string, JsonData[]>>(jsonData);

                return ParseRecords(data);
            }
        }

        private static IEnumerable<CountryDto> ParseRecords(IDictionary<string, JsonData[]> jsonData) {
            foreach (var item in jsonData) {
                yield return new CountryDto {
                    Name = item.Key,
                    Records = item.Value
                        .Where(i => i.Confirmed > 0)
                        .Select(i => new RecordDto {
                            Date = DateTime.Parse(i.Date),
                            Confirmed = i.Confirmed,
                            Deaths = i.Deaths,
                            Recovered = i.Recovered,
                        })
                        .OrderBy(i => i.Date)
                        .ToList(),
                };
            }
        }
    }
}
