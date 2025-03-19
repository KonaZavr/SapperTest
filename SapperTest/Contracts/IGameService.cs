using SapperTest.Schemas;

namespace SapperTest.Contracts
{
    /// <summary>
    /// Работа с данными игры
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">Параметры игры</param>
        /// <returns></returns>
        Task<GameInfoResponse> NewGame(NewGameRequest request);

        /// <summary>
        /// Изменение существующей игры
        /// </summary>
        /// <param name="request">Параметры изменений</param>
        /// <returns></returns>
        Task<GameInfoResponse> UpdateGame(TurnRequest request);
        
    }
}
