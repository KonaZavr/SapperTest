namespace SapperTest.Models
{
    /// <summary>
    /// Данные по ячейке
    /// </summary>
    public class FieldItem
    {
        /// <summary>
        /// Значение
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Флаг открытой\закрытой ячейки
        /// </summary>
        public bool IsOpen { get; set; }

        public override string ToString()
        {
            if (!IsOpen)
                return " ";

            if (Value == -1)
                return "X";

            if (Value == -2)
                return "M";

            return Value.ToString();
        }
    }
}
