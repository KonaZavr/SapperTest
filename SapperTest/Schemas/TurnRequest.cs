using System.Text.Json.Serialization;

namespace SapperTest.Schemas
{
    /// <summary>
    /// Формат запроса на открытие ячейки
    /// </summary>
    public class TurnRequest
    {
        /// <summary>
        /// Идентификатор игры
        /// </summary>
        [JsonPropertyName("game_id")]
        public Guid GameId { get; set; }

        /// <summary>
        /// Индекс ячейки (строка)
        /// </summary>
        [JsonPropertyName("row")]
        public int Row { get; set; }

        /// <summary>
        /// Индекс ячейки (столбец)
        /// </summary>
        [JsonPropertyName("col")]
        public int Column { get; set; }
    }
}
