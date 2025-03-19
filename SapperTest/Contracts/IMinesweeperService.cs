using SapperTest.Model;
using SapperTest.Models;

namespace SapperTest.Contracts
{
    /// <summary>
    /// Работа с полем для игры "Сапер"
    /// </summary>
    public interface IMinesweeperService
    {
        /// <summary>
        /// Генерирует новое поле
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="minesCount">Количество мин</param>
        /// <returns></returns>
        List<List<FieldItem>> GenerateField(int width, int height, int minesCount);

        /// <summary>
        /// Открывает ячейки поля
        /// </summary>
        /// <param name="gameInfo">Информация по игре</param>
        /// <param name="openRow">Строка открываемой ячейки</param>
        /// <param name="openColumn">Колонка открываемой ячейки</param>
        /// <returns></returns>
        List<List<FieldItem>> OpenItems(GameInfo gameInfo, int openRow, int openColumn);
    }
}
