using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public class AdminFunctions
    {
        private IFunctionDepedencies _d;

        public AdminFunctions(IFunctionDepedencies d)
        {
            CheckIsNotNull(nameof(d), d);
            _d = d;
        }

        [FunctionName("AddDecks")]
        public async Task AddDecks(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
//            [Blob("deckofcards/decks", FileAccess.Write)] Stream decksAsStream,
            ILogger log)
        {
            try
            {
                var decks = await req.Deserialize<List<Deck>>();

                foreach (var d in decks)
                {
                    await _d.Store.AddDeck(d);
                }

//                var decksToSave = await _d.Store.GetDecks();                
//                await Serialize(decksToSave, decksAsStream);
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(AddDecks));
                throw;
            }

            await Task.CompletedTask;
        }
    }
}
