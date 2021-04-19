using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace LeaderboardTestConsole
{
    public class LeaderboardClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public LeaderboardClient(string apiUrl)
        {
            CheckIsNotNull(nameof(apiUrl), apiUrl);
            CheckIsWellFormedUri(nameof(apiUrl), apiUrl);
            _apiUrl = apiUrl;
        }

        public IEnumerable<Board>
    }
}
