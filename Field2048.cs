using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game2048
{
    public class Field2048
    {
        private int _rows;
        private int _columns;
        private int[,] _field;
        private bool[,] _emptyTiles; //true if the tile is empty
        private int _score;
        private int _highestScore;

        public int HighestScore
        {
            get 
            { 
                if (_score > _highestScore)
                {
                    _highestScore = _score;
                }

                return _highestScore; 
            }
        }

        public int Score
        {
            get 
            {
                return _score;
            }
        }
        public bool GameOver
        {
            get
            {
                return !(CanMoveUp() || CanMoveDown() || CanMoveLeft() || CanMoveRight());
            }
        }

        public int TilesCount
        {
            get 
            {
                int tilesCount = 0;
                foreach (var tile in _field)
                {
                    if (tile != 0)
                    {
                        tilesCount++;
                    }
                }

                return tilesCount; 
            }
        }

        

        public Field2048(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _field = new int[rows, columns];
            _emptyTiles = new bool[rows, columns];
            _score = 0;            
        }

        public void InitializeField()
        {
            Random random = new Random();

            _field = new int[_rows, _columns];
            _score = 0;

            do
            {
                if (random.Next(4) != 0)
                {
                    _field[random.Next(_rows), random.Next(_columns)] = 2;
                }
                else
                {
                    _field[random.Next(_rows), random.Next(_columns)] = 4;
                }

            } while (TilesCount < 3);
        }

        public void Show(bool displayScore)
        {
            Console.Clear();

            for (int i = 0; i < _rows; i++)
            {
                PrintLine(i);
            }

            if (displayScore)
            {
                Console.WriteLine();
                Console.WriteLine("------------");
                Console.WriteLine($"Счёт: {Score}");
                Console.WriteLine();
            }
        }

        private void PrintLine(int rowNumber)
        {
            var currentPrintPosition = new PointOnBoard();
            currentPrintPosition.Row = rowNumber;

            Console.Write("║");

            for (int i = 0; i < _columns; i++)
            {
                currentPrintPosition.Column = i;
                var pointValue = GetPointValue(currentPrintPosition);
                var separatorChar = i == _columns - 1 ? '║' : '|';
                Console.Write($"{pointValue}{separatorChar}");
            }

            Console.WriteLine();

            string GetPointValue(PointOnBoard currentPrintPosition)
            {
                var cellValue = _field[currentPrintPosition.Row, currentPrintPosition.Column];
                var spacesBefore = string.Empty;
                var spacesAfter = string.Empty;

                if (cellValue != 0)
                {
                    if (cellValue < 10)
                    {
                        spacesBefore = " ";
                        spacesAfter = "  ";
                    }
                    else if (cellValue < 100)
                    {
                        spacesBefore = " ";
                        spacesAfter = " ";
                    }
                    else if (cellValue < 1000)
                    {
                        spacesBefore = " ";
                    }

                    return spacesBefore + cellValue + spacesAfter;
                }
                else
                {
                    return "    ";
                }
            }
        }

        public void MakeMove(ConsoleKeyInfo playersMove)
        {
            switch (playersMove.Key)
            {
                case ConsoleKey.UpArrow:
                    if (!CanMoveUp())
                    {
                        break;
                    }

                    MoveUp();                                        
                    CreateRandomTile();
                    break;

                case ConsoleKey.DownArrow:
                    if (!CanMoveDown())
                    {
                        break;
                    }

                    MoveDown();
                    CreateRandomTile();
                    break;

                case ConsoleKey.LeftArrow:
                    if (!CanMoveLeft())
                    {
                        break;
                    }

                    MoveLeft();
                    CreateRandomTile();
                    break;

                case ConsoleKey.RightArrow:
                    if (!CanMoveRight())
                    {
                        break;
                    }

                    MoveRight();
                    CreateRandomTile();
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Please, use arrow keys to play.");
                    Thread.Sleep(500);
                    break;
            }
        }        

        public void MoveUp()
        {            
            for (int i = 0; i < _columns; i++)
            {
                List<int> listedColumn = GetListedColumn(i); // Переписать значения из колонки в список.
                listedColumn = GetRidOfSpaces(listedColumn); // Избавиться от пробелов.
                listedColumn = SumIfPossible(listedColumn);
                listedColumn = GetRidOfSpaces(listedColumn);
                WriteDownListInColumn(listedColumn, i);
            }            
            

            void WriteDownListInColumn(List<int> list, int columnNumber)
            {
                var zerosCount = _rows - list.Count;
                for (int i = 0; i < zerosCount; i++)
                {
                    list.Add(0);
                }

                for (int i = 0; i < _rows; i++)
                {
                    _field[i, columnNumber] = list[i];
                }
            }

            List<int> SumIfPossible(List<int> list)
            {
                list.Add(0);
                List<int> temp = new List<int>(_rows + 1); // Временный список

                for (int i = 0; i < _rows + 1; i++)
                {
                    temp.Add(0);
                }



                for (int i = 0; i < list.Count; i++)
                {

                    if ((i != list.Count - 1) && list[i] == list[i + 1])
                    {
                        temp[i] = list[i] + list[i + 1];
                        _score += list[i] + list[i + 1];
                        temp[i + 1] = 0;
                        i++;
                    }
                    else
                    {
                        temp[i] = list[i];
                    }
                }

                return temp;
            }

            List<int> GetRidOfSpaces(List<int> list)
            {
                var listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    list.Remove(0);
                }

                return list;

            }

            List<int> GetListedColumn(int columnNumber)
            {
                List<int> listedColumn = new List<int>(_rows); // Создать список, размерность - количество рядов.

                for (int i = 0; i < _rows; i++) // Пройтись по всей колонке и...
                {
                    // ...записать колонку в список.
                    listedColumn.Add(_field[i, columnNumber]);
                }

                return listedColumn;
            }
        }

        public void MoveDown()
        {
            for (int i = 0; i < _columns; i++)
            {
                List<int> listedColumn = GetListedColumn(i); // Переписать значения из колонки в список.

                listedColumn = GetRidOfSpaces(listedColumn); // Избавиться от пробелов.

                listedColumn = SumIfPossible(listedColumn); // todo при сложении плитки складываются в неправильном порядке.

                listedColumn = GetRidOfSpaces(listedColumn);

                WriteDownListInColumn(listedColumn, i);
            }

            void WriteDownListInColumn(List<int> list, int columnNumber)
            {
                var zerosCount = _rows - list.Count;
                for (int i = 0; i < zerosCount; i++)
                {
                    list.Insert(0, 0);
                }

                for (int i = 0; i < _rows; i++)
                {
                    _field[i, columnNumber] = list[i];
                }
            }

            List<int> SumIfPossible(List<int> list)
            {
                list.Add(0);
                List<int> temp = new List<int>(_rows + 1);

                for (int i = 0; i < _rows + 1; i++)
                {
                    temp.Add(0);
                }

                list.Reverse();

                for (int i = 0; i < list.Count; i++)
                {
                    if ((i != list.Count - 1) && list[i] == list[i + 1])
                    {
                        temp[i] = list[i] + list[i + 1];
                        _score += list[i] + list[i + 1];
                        temp[i + 1] = 0;
                        i++;
                    }
                    else
                    {
                        temp[i] = list[i];
                    }
                }

                

                temp.Reverse();

                return temp;
            }

            List<int> GetRidOfSpaces(List<int> list)
            {
                var listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    list.Remove(0);
                }

                return list;
            }

            List<int> GetListedColumn(int columnNumber)
            {
                List<int> listedColumn = new List<int>(_rows); // Создать список, размерность - количество рядов.

                for (int i = 0; i < _rows; i++) // Пройтись по всей колонке и...
                {
                    // ...записать колонку в список.
                    listedColumn.Add(_field[i, columnNumber]);
                }

                return listedColumn;
            }

        }

        public void MoveLeft()
        {
            for (int i = 0; i < _rows; i++)
            {
                List<int> listedRow = GetListedRow(i);

                listedRow = GetRidOfSpaces(listedRow);

                listedRow = SumIfPossible(listedRow);

                listedRow = GetRidOfSpaces(listedRow);

                WriteDownListInRow(listedRow, i);
            }

            List<int> GetListedRow(int rowNumber)
            {
                List<int> listedColumn = new List<int>(_rows); // Создать список, размерность - количество рядов.

                for (int i = 0; i < _rows; i++)
                {
                    listedColumn.Add(_field[rowNumber, i]);
                }

                return listedColumn;
            }

            List<int> GetRidOfSpaces(List<int> list)
            {
                var listCount = list.Count;

                for (int i = 0; i < listCount; i++)
                {
                    list.Remove(0);
                }

                return list;
            }

            List<int> SumIfPossible(List<int> list)
            {
                list.Add(0);
                List<int> temp = new List<int>(_rows + 1);

                for (int i = 0; i < _rows + 1; i++)
                {
                    temp.Add(0);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if ((i != list.Count - 1) && list[i] == list[i + 1])
                    {
                        temp[i] = list[i] + list[i + 1];
                        _score += list[i] + list[i + 1];
                        temp[i + 1] = 0;
                        i++;
                    }
                    else
                    {
                        temp[i] = list[i];
                    }
                }

                return temp;
            }

            void WriteDownListInRow(List<int> list, int rowNumber)
            {
                var zerosCount = _columns - list.Count;
                for (int i = 0; i < zerosCount; i++)
                {
                    list.Add(0);
                }

                for (int i = 0; i < _rows; i++)
                {
                    _field[rowNumber, i] = list[i];
                }
            }
        }

        public void MoveRight()
        {
            for (int i = 0; i < _rows; i++)
            {
                List<int> listedRow = GetListedRow(i);

                listedRow = GetRidOfSpaces(listedRow);

                listedRow = SumIfPossible(listedRow);

                listedRow = GetRidOfSpaces(listedRow);

                WriteDownListInRow(listedRow, i);
            }

            List<int> GetListedRow(int rowNumber)
            {
                List<int> listedColumn = new List<int>(_rows); // Создать список, размерность - количество рядов.

                for (int i = 0; i < _rows; i++)
                {
                    listedColumn.Add(_field[rowNumber, i]);
                }

                return listedColumn;
            }

            List<int> GetRidOfSpaces(List<int> list)
            {
                var listCount = list.Count;

                for (int i = 0; i < listCount; i++)
                {
                    list.Remove(0);
                }

                return list;
            }

            List<int> SumIfPossible(List<int> list)
            {
                list.Add(0);
                List<int> temp = new List<int>(_rows + 1);

                for (int i = 0; i < _rows + 1; i++)
                {
                    temp.Add(0);
                }

                list.Reverse();

                for (int i = 0; i < list.Count; i++)
                {
                    if ((i != list.Count - 1) && list[i] == list[i + 1])
                    {
                        temp[i] = list[i] + list[i + 1];
                        _score += list[i] + list[i + 1];
                        temp[i + 1] = 0;
                        i++;
                    }
                    else
                    {
                        temp[i] = list[i];
                    }
                }

                temp.Reverse();
                return temp;
            }

            void WriteDownListInRow(List<int> list, int rowNumber)
            {
                var zerosCount = _columns - list.Count;
                for (int i = 0; i < zerosCount; i++)
                {
                    list.Insert(0, 0);
                }

                for (int i = 0; i < _rows; i++)
                {
                    _field[rowNumber, i] = list[i];
                }
            }
        }

        protected void CreateRandomTile()
        {
            Show(true);
            Thread.Sleep(250);

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (_field[i, j] == 0)
                    {
                        _emptyTiles[i, j] = true;
                    }
                    else
                    {
                        _emptyTiles[i, j] = false;
                    }
                }
            }

            Random random = new();

            do
            {
                var randomRow = random.Next(_rows);
                var randomColumn = random.Next(_columns);

                if (_field[randomRow, randomColumn] == 0) // Если клетка пустая
                {
                    if (random.Next(100) < 10)
                    {
                        _field[randomRow, randomColumn] = 4;
                        break;
                    }
                    else
                    {
                        _field[randomRow, randomColumn] = 2;
                        break;
                    }
                }

            } while (true);            
        }

        protected bool CanMoveUp()
        {
            return PossibleToSumSomethingInColumn() || CanSwipeColumnUp();
        }

        private bool CanMoveDown()
        {
            return PossibleToSumSomethingInColumn() || CanSwipeColumnDown();
        }

        private bool CanMoveLeft()
        {
            return PossibleToSumSomethingInRow() || CanSwipeRowLeft();
        }

        private bool CanMoveRight()
        {
            return PossibleToSumSomethingInRow() || CanSwipeRowRight();
        }

        private bool CanSwipeColumnUp()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows - 1; row++)
                {
                    if (_field[row, column] == 0 && _field[row + 1, column] != 0)
                    {
                        return true;
                    }
                }
            }            

            return false;
        }

        private bool CanSwipeColumnDown()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows - 1; row++)
                {
                    if (_field[row, column] != 0 && _field[row + 1, column] == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanSwipeRowLeft()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns - 1; column++)
                {
                    if (_field[row, column] == 0 && _field[row, column + 1] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanSwipeRowRight()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns - 1; column++)
                {
                    if (_field[row, column] != 0 && _field[row, column + 1] == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool PossibleToSumSomethingInColumn()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows - 1; row++)
                {
                    if (_field[row, column] != 0 && _field[row, column] == _field[row + 1, column])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool PossibleToSumSomethingInRow()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns - 1; column++)
                {
                    if (_field[row, column] != 0 && _field[row, column] == _field[row, column + 1])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
