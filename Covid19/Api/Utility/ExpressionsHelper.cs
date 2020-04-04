using System;
using System.Linq.Expressions;
using Common;
using DataAccess.Models;

namespace Api.Utility {
    public static class ExpressionsHelper {
        public static Expression<Func<Record, RecordDto>> RecordRowToDto => row => new RecordDto {
            Date = row.Date,
            AccumulatedConfirmed = row.AccumulatedConfirmed,
            AccumulatedDeaths = row.AccumulatedDeaths,
            AccumulatedRecovered = row.AccumulatedRecovered,
            NewConfirmed = row.NewConfirmed,
            NewDeaths = row.NewDeaths,
            NewRecovered = row.NewRecovered,
        };
    }
}
