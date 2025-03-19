using SapperTest.Contracts;
using SapperTest.Model;
using SapperTest.Models;
using SapperTest.Schemas;

namespace SapperTest.Services
{
    public class GameService : IGameService
    {
        private readonly IMinesweeperService _minesweeperService;
        private readonly IGameInfoRepository _gameInfoRepository;

        public GameService(IMinesweeperService minesweeperService, IGameInfoRepository gameInfoRepository)
        {
            _minesweeperService = minesweeperService;
            _gameInfoRepository = gameInfoRepository;
        }

        public async Task<GameInfoResponse> NewGame(NewGameRequest request)
        {
            //генерируем поле для игры и преобразуем в нужный вид 
            var newField = _minesweeperService.GenerateField(request.Width, request.Height, request.MinesCount);

            var gameId = Guid.NewGuid();

            var response = new GameInfoResponse
            {
                Completed = false,
                Field = CastField(newField),
                GameId = gameId,
                Height = request.Height,
                Width = request.Width,
                MinesCount = request.MinesCount
            };

            //запись в базу
            var gameInfo = new GameInfo
            {
                Field = newField,
                Height = request.Height,
                Width = request.Width,
                GameId = gameId,
                IsCompleted = false,
                MinesCount = request.MinesCount
            };

            await _gameInfoRepository.CreateAsync(gameInfo);

            return response;
        }

        public async Task<GameInfoResponse> UpdateGame(TurnRequest request)
        {
            var gameInfo = await _gameInfoRepository.GetAsync(request.GameId);

            if (gameInfo == null)            
                throw new ArgumentException($"Не найдена игра id {request.GameId}");
            

            if (gameInfo.IsCompleted)            
                throw new ArgumentException("Игра окончена");
            

            if (gameInfo.Field == null)            
                throw new ArgumentException($"Не найдены данные по игре id {request.GameId}");
            

            if (gameInfo.Field[request.Row][request.Column].IsOpen)
                throw new ArgumentException("Ячейка уже проверена");

            //открываем ячейки и преобразуем результат
            var field = _minesweeperService.OpenItems(gameInfo, request.Row, request.Column);

            var response = new GameInfoResponse
            {
                Completed = gameInfo.IsCompleted,
                Field = CastField(field),
                GameId = (Guid)gameInfo.GameId!,
                Height = gameInfo.Height,
                Width = gameInfo.Width,
                MinesCount = gameInfo.MinesCount
            };

            //обновляем данные в базе
            var updateInfo = new GameInfo
            {
                Field = field,
                Height = gameInfo.Height,
                Width = gameInfo.Width,
                GameId = gameInfo.GameId,
                IsCompleted = gameInfo.IsCompleted,
                MinesCount = gameInfo.MinesCount,
                Id = gameInfo.Id,
            };

            await _gameInfoRepository.UpdateAsync(gameInfo.Id!, updateInfo);

            return response;
        }

        private List<List<string>> CastField(List<List<FieldItem>> field)
        {
            return field.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
        }
    }
}
