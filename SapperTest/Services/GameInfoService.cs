using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SapperTest.Model;
using SapperTest.Models;

namespace SapperTest.Services
{
    /// <summary>
    /// Работа с базой данных
    /// </summary>
    public class GameInfoService
    {
        private readonly IMongoCollection<GameInfo> _gameInfoCollection;

        public GameInfoService(IOptions<GameInfoDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

            _gameInfoCollection = mongoDatabase.GetCollection<GameInfo>(bookStoreDatabaseSettings.Value.GameInfoCollectionName);
        }

        /// <summary>
        /// Получить данные по идентификатору игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <returns></returns>
        public async Task<GameInfo?> GetAsync(Guid gameId) => await _gameInfoCollection.Find(x => x.GameId == gameId).FirstOrDefaultAsync();

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="newGameInfo">Новая запись</param>
        /// <returns></returns>
        public async Task CreateAsync(GameInfo newGameInfo) => await _gameInfoCollection.InsertOneAsync(newGameInfo);

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="updatedGameInfo">Данные записи</param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, GameInfo updatedGameInfo) => await _gameInfoCollection.ReplaceOneAsync(x => x.Id == id, updatedGameInfo);
    }
}
