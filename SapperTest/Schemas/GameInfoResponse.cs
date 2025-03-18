using System.Text.Json.Serialization;

namespace SapperTest.Schemas
{
    /// <summary>
    /// Формат ответа на запросы (информация о текущей игре)
    /// </summary>
    public class GameInfoResponse
    {
        /// <summary>
        /// Идентификатор игры
        /// </summary>
        [JsonPropertyName("game_id")]
        public Guid GameId { get; set; }

        /// <summary>
        /// Ширина
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Высота
        /// </summary>
        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// Количество мин
        /// </summary>
        [JsonPropertyName("mines_count")]
        public int MinesCount { get; set; }

        /// <summary>
        /// Поле
        /// </summary>
        [JsonPropertyName("field")]
        public List<List<string>>? Field { get; set; }

        /// <summary>
        /// Флаг завершения игры
        /// </summary>
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
