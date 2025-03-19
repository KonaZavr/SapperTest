using SapperTest.Model;

namespace SapperTest.Contracts
{
    /// <summary>
    /// Работа с базой данных
    /// </summary>
    public interface IGameInfoRepository
    {
        /// <summary>
        /// Получить данные по идентификатору игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <returns></returns>
        Task<GameInfo?> GetAsync(Guid gameId);

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="newGameInfo">Новая запись</param>
        /// <returns></returns>
        Task CreateAsync(GameInfo newGameInfo);

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="updatedGameInfo">Данные записи</param>
        /// <returns></returns>
        Task UpdateAsync(string id, GameInfo updatedGameInfo);
    }
}
