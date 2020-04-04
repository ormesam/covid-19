using System.Linq;
using Common;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WorldController : ControllerBase {
        private readonly ModelDataContext context;

        public WorldController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public WorldDto Get() {
            var world = new WorldDto();

            world.Records = context.Record
                .GroupBy(row => row.Date)
                .OrderBy(row => row.Key)
                .Select(row => new RecordDto {
                    Date = row.Key,
                    AccumulatedConfirmed = row.Sum(i => i.AccumulatedConfirmed),
                    AccumulatedDeaths = row.Sum(i => i.AccumulatedDeaths),
                    AccumulatedRecovered = row.Sum(i => i.AccumulatedRecovered),
                    NewConfirmed = row.Sum(i => i.NewConfirmed),
                    NewDeaths = row.Sum(i => i.NewDeaths),
                    NewRecovered = row.Sum(i => i.NewRecovered),
                })
                .ToList();

            return world;
        }
    }
}
