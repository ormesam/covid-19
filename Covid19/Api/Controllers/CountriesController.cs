using System.Collections.Generic;
using System.Linq;
using Common;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase {
        private readonly ModelDataContext context;

        public CountriesController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CountryOverviewDto> Get() {
            return context.Country
                .OrderBy(row => row.Name)
                .Select(row => new CountryOverviewDto {
                    CountryId = row.CountryId,
                    Name = row.Name,
                    Confirmed = row.CurrentConfirmed,
                    Deaths = row.CurrentDeaths,
                    Recovered = row.CurrentRecovered,
                })
                .ToList();
        }

        [HttpGet]
        [Route("{name}")]
        public CountryDto Get(string name) {
            var country = context.Country
                .Where(row => row.Name == name)
                .Select(row => new CountryDto {
                    CountryId = row.CountryId,
                    Name = row.Name,
                })
                .SingleOrDefault();

            if (country == null) {
                return null;
            }

            country.Records = context.Record
                .Where(row => row.CountryId == country.CountryId)
                .OrderBy(row => row.Date)
                .Select(row => new RecordDto {
                    Date = row.Date,
                    AccumulatedConfirmed = row.AccumulatedConfirmed,
                    AccumulatedDeaths = row.AccumulatedDeaths,
                    AccumulatedRecovered = row.AccumulatedRecovered,
                    NewConfirmed = row.NewConfirmed,
                    NewDeaths = row.NewDeaths,
                    NewRecovered = row.NewRecovered,
                })
                .ToList();

            return country;
        }
    }
}
