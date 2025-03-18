using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SapperTest.Models;

namespace SapperTest.Model
{
    /// <summary>
    /// Модель данных для записи в базу
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Id записи
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Id игры
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Guid? GameId { get; set; }

        /// <summary>
        /// Высота
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Количество мин
        /// </summary>
        public int MinesCount { get; set; }

        /// <summary>
        /// Флаг завершения игры
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Поле
        /// </summary>
        public List<List<FieldItem>>? Field { get; set; }
    }
}
