using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase {
        private int newCountryCount = 0;
        private int newRecordCount = 0;
        private int updatedRecordCount = 0;

        [HttpGet]
        public async Task<IActionResult> Get() {
            await WriteLine("Starting sync...");

            var newData = await GetNewData();

            await WriteLine($"{newData.Count()} countries found");

            await CreateNewCountries(newData);
            await CreateNewRecords(newData);
            await UpdateCurrentTotals();

            string syncMessage = $"Sync complete - {newCountryCount} countries added - {newRecordCount} records added - {updatedRecordCount} records updated";

            await WriteLine(syncMessage);

            return Ok(syncMessage);
        }

        private async Task UpdateCurrentTotals() {
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

            await WriteLine("Current totals updated");
        }

        private async Task CreateNewRecords(IEnumerable<CountryDto> newData) {
            using (ModelDataContext context = new ModelDataContext()) {
                var countrys = context.Country
                    .Select(row => new {
                        row.CountryId,
                        row.Name,
                    })
                    .ToList();

                foreach (var country in countrys) {
                    var currentRecords = context.Record
                        .Where(row => row.CountryId == country.CountryId)
                        .ToList();

                    var importRecords = newData
                        .Where(row => row.Name == country.Name)
                        .SelectMany(row => row.Records)
                        .ToList();

                    foreach (var record in importRecords) {
                        var currentRecord = currentRecords
                            .Where(row => row.Date == record.Date)
                            .SingleOrDefault();

                        if (currentRecord == null) {
                            await WriteLine($"Creating record for {country.Name} - {record.Date.ToShortDateString()}");

                            context.Record.Add(new Record {
                                Confirmed = record.Confirmed,
                                CountryId = country.CountryId,
                                Date = record.Date,
                                Deaths = record.Deaths,
                                Recovered = record.Recovered,
                            });

                            context.SaveChanges();

                            newRecordCount++;
                        } else if (record.Confirmed != currentRecord.Confirmed ||
                                record.Recovered != currentRecord.Recovered ||
                                record.Deaths != currentRecord.Deaths) {

                            await WriteLine($"Updating record for {country.Name} - {record.Date.ToShortDateString()}");

                            currentRecord.Confirmed = record.Confirmed;
                            currentRecord.Recovered = record.Recovered;
                            currentRecord.Deaths = record.Deaths;

                            context.SaveChanges();

                            updatedRecordCount++;
                        }
                    }
                }
            }
        }

        private async Task CreateNewCountries(IEnumerable<CountryDto> newData) {
            using (ModelDataContext context = new ModelDataContext()) {
                var countryNames = context.Country
                    .Select(row => row.Name)
                    .ToList();

                var newCountries = newData
                    .Where(row => !countryNames.Contains(row.Name))
                    .ToList();

                foreach (var country in newCountries) {
                    await WriteLine($"Creating {country.Name}");

                    context.Country.Add(new Country {
                        Name = country.Name,
                    });

                    context.SaveChanges();

                    newCountryCount++;
                }
            }
        }

        private async Task<IEnumerable<CountryDto>> GetNewData() {
            using (HttpClient client = new HttpClient()) {
                string jsonData = await client.GetStringAsync("https://pomber.github.io/covid19/timeseries.json");

                var data = JsonConvert.DeserializeObject<IDictionary<string, JsonData[]>>(jsonData);

                return ParseRecords(data);
            }
        }

        private IEnumerable<CountryDto> ParseRecords(IDictionary<string, JsonData[]> jsonData) {
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

        private async Task WriteLine(string message) {
            message += Environment.NewLine;

            await Response.Body.WriteAsync(Encoding.ASCII.GetBytes(message), 0, message.Length);
        }
    }
}
