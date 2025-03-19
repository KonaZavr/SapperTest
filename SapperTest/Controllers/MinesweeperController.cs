using Microsoft.AspNetCore.Mvc;
using SapperTest.Contracts;
using SapperTest.Model;
using SapperTest.Schemas;

namespace SapperTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinesweeperController : ControllerBase
    {
        private readonly IGameService _gameService;

        public MinesweeperController(IGameService gameService)
        {
            _gameService = gameService;
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

            var response = await _gameService.NewGame(newGameRequest);

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
            try
            {
                var response = await _gameService.UpdateGame(turnRequest);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
