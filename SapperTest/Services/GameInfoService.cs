using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SapperTest.Contracts;
using SapperTest.Model;
using SapperTest.Models;

namespace SapperTest.Services
{
    public class GameInfoService : IGameInfoService
    {
        private readonly IMongoCollection<GameInfo> _gameInfoCollection;

        public GameInfoService(IOptions<GameInfoDatabaseSettings> databaseSettings, IMongoDatabase database)
        {
            _gameInfoCollection = database.GetCollection<GameInfo>(databaseSettings.Value.GameInfoCollectionName);
        }

        public async Task<GameInfo?> GetAsync(Guid gameId) => await _gameInfoCollection.Find(x => x.GameId == gameId).FirstOrDefaultAsync();

        public async Task CreateAsync(GameInfo newGameInfo) => await _gameInfoCollection.InsertOneAsync(newGameInfo);

        public async Task UpdateAsync(string id, GameInfo updatedGameInfo) => await _gameInfoCollection.ReplaceOneAsync(x => x.Id == id, updatedGameInfo);
    }
}
