using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient _httpCLient;

        public BattleService(HttpClient httpCLient)
        {
            _httpCLient = httpCLient;
        }

        public BattleResult LastBattle { get; set; } = new BattleResult();
        public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

        public async Task<BattleResult> StartBattle(int opponentId)
        {
            var result = await _httpCLient.PostAsJsonAsync("api/battle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();

            return LastBattle;
        }

        public async Task GetHistory()
        {
            History = await _httpCLient.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
         }
    }
}
