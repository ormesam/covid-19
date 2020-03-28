using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Web.Models;

namespace Web.Utility {
    public static class DataUtility {
        public static async Task<IEnumerable<Country>> GetData(HttpClient client) {
            var jsonData = await client.GetJsonAsync<IDictionary<string, JsonData[]>>("https://pomber.github.io/covid19/timeseries.json");

            return ParseRecords(jsonData);
        }

        private static IEnumerable<Country> ParseRecords(IDictionary<string, JsonData[]> jsonData) {
            foreach (var item in jsonData) {
                yield return new Country {
                    Name = item.Key,
                    Records = item.Value
                        .Where(i => i.Confirmed > 0)
                        .Select(i => new Record {
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
