using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards.Leaderboard
{
    public class LeaderboardFunction
    {
        private IFunctionDependencies _d;

        public LeaderboardFunction(IFunctionDependencies d)
        {
            CheckIsNotNull(nameof(d), d);
            _d = d;
        }

        [FunctionName("GetTop10")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var topTen = await _d.Store.Boards.OrderByDescending(x => x.NumOfWorkouts).Take(10).ToListAsync();

            return new JsonResult(topTen);
        }
    }
}
