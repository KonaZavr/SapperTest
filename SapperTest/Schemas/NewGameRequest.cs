using System.Text.Json.Serialization;

namespace SapperTest.Schemas
{
    /// <summary>
    /// Формат запроса на генерацию новой игры
    /// </summary>
    public class NewGameRequest
    {
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
    }
}
