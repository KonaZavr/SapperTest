using SapperTest.Contracts;
using SapperTest.Model;
using SapperTest.Models;
using SapperTest.Schemas;

namespace SapperTest.Services
{
    public class MinesweeperService : IMinesweeperService
    {
        public List<List<FieldItem>> GenerateField(int width, int height, int minesCount)
        {
            //создаем матрицу с заданными параметрами
            var newField = new List<List<FieldItem>>();

            for (var i = 0; i < height; i++)
                newField.Add(new int[width].Select(x => new FieldItem { Value = 0, IsOpen = false }).ToList());

            var countElemenet = height * width;
            //создаем список индексов для рандомной генерации поля            
            var list = Enumerable.Range(0, countElemenet).ToList();
            var random = new Random();

            for (var i = 0; i < minesCount; i++)
            {
                //берем значение из списка с рандомным индексом и вычисляем индексы для матрицы
                var index = list[random.Next(0, countElemenet - i)];
                var col = index % width;
                var row = index / height;

                //помечаем что там мина
                newField[row][col].Value = -1;

                //проходимся по всем ячейкам вокруг мины и увеличиваем количество мин вокруг для них
                for (var c = col - 1; c <= col + 1; c++)
                {
                    if (c < 0 || c >= width)
                        continue;
                    for (var r = row - 1; r <= row + 1; r++)
                    {
                        if (r < 0 || r >= height)
                            continue;
                        if (newField[r][c].Value != -1)
                            ++newField[r][c].Value;
                    }
                }

                //удаляем текущий элемент массива для избежания повторений
                list.Remove(index);
            }

            return newField;
        }

        public List<List<string>> CastField(List<List<FieldItem>> field)
        {
            return field.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
        }

        public List<List<FieldItem>> OpenItems(GameInfo gameInfo, int openRow, int openColumn)
        {
            var field = gameInfo.Field!;
            
            //создаем очередь для обхода в ширину матрицы, индексы формируем как для линейного массива, вычисляя их для матрицы
            var indexes = new Queue<int>();

            indexes.Enqueue(openRow * gameInfo.Width + openColumn);
            while (indexes.Any())
            {                
                var index = indexes.Dequeue();

                var col = index % gameInfo.Width;
                var row = index / gameInfo.Width;

                //если пустая ячейка - добавляем в очередь все смежные ячейки, если они не открыты. таким образом мы просмотрим все пустые ячейки и смежные к ним
                if (field[row][col].Value == 0)
                {
                    if (col - 1 >= 0 && !field[row][col - 1].IsOpen)
                        indexes.Enqueue(row * gameInfo.Width + col - 1);

                    if (col + 1 < gameInfo.Width && !field[row][col + 1].IsOpen)
                        indexes.Enqueue(row * gameInfo.Width + col + 1);

                    if (row + 1 < gameInfo.Height && !field[row + 1][col].IsOpen)
                        indexes.Enqueue((row + 1) * gameInfo.Width + col);

                    if (row - 1 >= 0 && !field[row - 1][col].IsOpen)
                        indexes.Enqueue((row - 1) * gameInfo.Width + col);
                }

                //если это мина - помечаем что игра закончена и открываем все ячейки. выходим из цикла
                if (field[row][col].Value == -1)
                {
                    gameInfo.IsCompleted = true;
                    field = field.Select(x => x.Select(y => new FieldItem { IsOpen = true, Value =  y.Value }).ToList()).ToList();
                    break;
                }

                field[row][col].IsOpen = true;
            }

            //если игра не закончилась в цикле (не наткнулись на мину), проверяем количество неоткрытых ячеек
            //если их количество равно количеству мин - игра закончена, помечаем все ячейки открытыми
            if (!gameInfo.IsCompleted)
            {
                var sum = field.Sum(x => x.Count(y => !y.IsOpen));
                if(sum == gameInfo.MinesCount)
                {
                    field = field.Select(x => x.Select(y => new FieldItem { IsOpen = true, Value = y.Value == -1 ? y.Value = -2 : y.Value }).ToList()).ToList();
                    gameInfo.IsCompleted = true;
                }                
            }

            return field;
        }
    }
}
