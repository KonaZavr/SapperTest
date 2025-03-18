namespace SapperTest.Models
{
    /// <summary>
    /// Конфиг базы
    /// </summary>
    public class GameInfoDatabaseSettings
    {
        /// <summary>
        /// Строка соединения
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Наименование базы
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Наименование коллекции
        /// </summary>
        public string? GameInfoCollectionName { get; set; }
    }
}
