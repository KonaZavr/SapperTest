using Microsoft.AspNetCore.Mvc;
using SapperTest.Contracts;
using SapperTest.Model;
using SapperTest.Schemas;
using SapperTest.Services;

namespace SapperTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinesweeperController : ControllerBase
    {
        private readonly IMinesweeperService _minesweeperService;
        private readonly IGameInfoService _gameInfoService;

        public MinesweeperController(IMinesweeperService minesweeperService, IGameInfoService gameInfoService)
        {
            _minesweeperService = minesweeperService;
            _gameInfoService = gameInfoService;
        }

        /// <summary>
        /// Создает новую игру
        /// </summary>
        /// <param name="newGameRequest">Параметры новой игры</param>
        /// <returns>Информация по игре</returns>
        [HttpPost("new")]
        public async Task<ActionResult<GameInfoResponse>> New([FromBody] NewGameRequest newGameRequest)
        {
            if (newGameRequest.Width > 30 || newGameRequest.Height > 30 || newGameRequest.MinesCount >= newGameRequest.Width * newGameRequest.Height)
                return BadRequest($"Некорректные входные данные. Ширина и высота не должны превышать 30, а количество мин должно быть меньше {newGameRequest.Width * newGameRequest.Height}");

            //генерируем поле для игры и преобразуем в нужный вид 
            var newField = _minesweeperService.GenerateField(newGameRequest.Width, newGameRequest.Height, newGameRequest.MinesCount);

            var castField = _minesweeperService.CastField(newField);

            var gameId = Guid.NewGuid();

            var response = new GameInfoResponse
            {
                Completed = false,
                Field = castField,
                GameId = gameId,
                Height = newGameRequest.Height,
                Width = newGameRequest.Width,
                MinesCount = newGameRequest.MinesCount
            };

            //запись в базу
            var gameInfo = new GameInfo
            {
                Field = newField,
                Height = newGameRequest.Height,
                Width = newGameRequest.Width,
                GameId = gameId,
                IsCompleted = false,
                MinesCount = newGameRequest.MinesCount
            };

            await _gameInfoService.CreateAsync(gameInfo);

            return Ok(response);
        }

        /// <summary>
        /// Открывает ячейку поля
        /// </summary>
        /// <param name="turnRequest">Индексы открываемой ячейки</param>
        /// <returns>Информация по игре</returns>
        [HttpPost("turn")]
        public async Task<ActionResult<GameInfoResponse>> Turn([FromBody] TurnRequest turnRequest)
        {
            var gameInfo = await _gameInfoService.GetAsync(turnRequest.GameId);

            if (gameInfo == null)
            {
                return BadRequest($"Не найдена игра id {turnRequest}");
            }

            if(gameInfo.IsCompleted)
            {
                return BadRequest("Игра окончена");
            }
                        
            if (gameInfo.Field == null)
            {
                return BadRequest($"Не найдены данные по игре id {turnRequest}");
            }

            if (gameInfo.Field[turnRequest.Row][turnRequest.Column].IsOpen)
                return BadRequest("Ячейка уже проверена");

            //открываем ячейки и преобразуем результат
            var field = _minesweeperService.OpenItems(gameInfo, turnRequest.Row, turnRequest.Column);

            var castField = _minesweeperService.CastField(field);
            var response = new GameInfoResponse
            {
                Completed = gameInfo.IsCompleted,
                Field = castField,
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

            await _gameInfoService.UpdateAsync(gameInfo.Id!, updateInfo);

            return Ok(response);
        }
    }
}
