using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace Leaderboard
{
    public class LeaderboardFunction
    {
        private readonly IFunctionDependencies _d;

        public LeaderboardFunction(IFunctionDependencies d)
        {
            CheckIsNotNull(nameof(d), d);
            _d = d;
        }

        [Function("GetTopTen")]
        public async Task<HttpResponseData> GetTopTen(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<LeaderboardFunction>();

            logger.LogInformation("Get Top Ten Request");

            var topTen = await _d.Store.Boards
                .Include(x => x.User)
                .OrderByDescending(x => x.NumOfWorkouts)
                .Take(10)
                .ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);

            var json = JsonConvert.SerializeObject(topTen, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await response.WriteStringAsync(json).ConfigureAwait(false);
            return response;
        }

        
        [Function ("InsertIntoLeaderboard")]
        public async Task InsertIntoLeaderboard(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<LeaderboardFunction>();

            logger.LogInformation("Insert Into Leaderboard");

            try
            {
                var e = await req.Deserialize<BoardEntry>();

                var board = new Board { UserId = e.UserId, NumOfWorkouts = e.NumOfWorkouts };
                var boardUser = new BoardUser
                {
                    UserId = e.UserId,
                    Email = e.Email,
                    LastWorkout = e.LastWorkout,
                    Name = e.Name
                };

                board.User = boardUser;

                _d.Store.Boards.Add(board);
                await _d.Store.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, nameof(InsertIntoLeaderboard));
                throw;
            }
        }
    }
}
