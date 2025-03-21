using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GameMain
{
    public class SudokuBoard : MonoBehaviour
    {
        private int[,] gridNumbers = new int[SudokuConstant.GridLength, SudokuConstant.GridLength];

        private int[,] puzzleNumbers = new int[SudokuConstant.GridLength, SudokuConstant.GridLength];

        private int[,] puzzleBak = new int[SudokuConstant.GridLength, SudokuConstant.GridLength];

        public SudokuGrid Grid { get; private set; }

        public int DifficultLevel { get; private set; }

        public void Init()
        {
            CreateGrid();
            CreatePuzzle();

            InitGrid();
        }

        private void InitGrid()
        {
            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                for (int j = 0; j < SudokuConstant.GridLength; j++)
                {
                    var cell = Grid.GetCellByPos(i, j);
                    if (cell != null)
                    {
                        cell.InitValues(puzzleNumbers[i, j]);
                    }
                }
            }
        }

        public void SetGrid(SudokuGrid grid)
        {
            Grid = grid;
        }

        public void SetDifficultLevel(int difficulty)
        {
            DifficultLevel = difficulty;
        }

        /// <summary>
        /// 更新到谜题中
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void UpdatePuzzle(int row, int col, int value)
        {
            puzzleNumbers[row, col] = value;
        }

        /// <summary>
        /// 检查数独是否已完成
        /// </summary>
        /// <returns></returns>
        public bool CheckCompleted()
        {
            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                for (int j = 0; j < SudokuConstant.GridLength; j++)
                {
                    if (puzzleNumbers[i, j] != gridNumbers[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 创建数独网格
        /// </summary>
        private void CreateGrid()
        {
            var rowList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var colList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // 先在（0,0）位置生成一个数据
            var value = rowList[Utility.Random.GetRandom(0, rowList.Count)];
            gridNumbers[0, 0] = value;
            rowList.Remove(value);
            colList.Remove(value);

            // 将其他8个数字随机到第一行中
            for (int i = 1; i < SudokuConstant.GridLength; i++)
            {
                value = rowList[Utility.Random.GetRandom(0, rowList.Count)];
                gridNumbers[i, 0] = value;
                rowList.Remove(value);
            }

            // 将其他8个数字随机到第一列中
            for (int i = 1; i < SudokuConstant.GridLength; i++)
            {
                value = colList[Utility.Random.GetRandom(0, colList.Count)];
                // 如果第一个网格中重复数字，就重新生成
                if (i < 3)
                {
                    while (SquareContainsValue(0, 0, value))
                    {
                        value = colList[Utility.Random.GetRandom(0, colList.Count)];
                    }
                }

                gridNumbers[0, i] = value;
                colList.Remove(value);
            }

            for (int i = 6; i < 9; i++)
            {
                value = Utility.Random.GetRandom(1, 10);
                while (SquareContainsValue(0, 8, value) || SquareContainsValue(8, 0, value) || SquareContainsValue(8, 8, value))
                {
                    value = Utility.Random.GetRandom(1, 10);
                }

                gridNumbers[i, i] = value;
            }

            SolveSudoku();
        }

        /// <summary>
        /// 数独求解
        /// </summary>
        private bool SolveSudoku()
        {
            var row = 0;
            var col = 0;

            if (IsValid())
            {
                return true;
            }

            var isFind = false;
            for (var i = 0; i < SudokuConstant.GridLength; i++)
            {
                for (var j = 0; j < SudokuConstant.GridLength; j++)
                {
                    if (gridNumbers[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        isFind = true;
                        break;
                    }
                }

                if (isFind)
                {
                    break;
                }
            }

            for (int i = 1; i <= SudokuConstant.GridLength; i++)
            {
                if (CheckAll(row, col, i))
                {
                    gridNumbers[row, col] = i;

                    if (SolveSudoku())
                    {
                        return true;
                    }
                    else
                    {
                        gridNumbers[row, col] = 0;
                    }
                }
            }

            return false;
        }

        private bool IsValid()
        {
            for (var i = 0; i < SudokuConstant.GridLength; i++)
            {
                for (var j = 0; j < SudokuConstant.GridLength; j++)
                {
                    if (gridNumbers[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查某个数是否已存在于行、列、网格中
        /// </summary>
        private bool CheckAll(int row, int col, int value)
        {
            if (ColumnContainsValue(col, value))
            {
                return false;
            }

            if (RowContainsValue(row, value))
            {
                return false;
            }

            if (SquareContainsValue(row, col, value))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 某列是否存在某个数
        /// </summary>
        private bool ColumnContainsValue(int col, int value)
        {
            for (var i = 0; i < SudokuConstant.GridLength; i++)
            {
                if (gridNumbers[i, col] == value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 某行是否存在某个数
        /// </summary>
        private bool RowContainsValue(int row, int value)
        {
            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                if (gridNumbers[row, i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 某网格是否存在某个数
        /// </summary>
        private bool SquareContainsValue(int row, int col, int value)
        {
            for (int i = 0; i < SudokuConstant.SubGridLength; i++)
            {
                for (int j = 0; j < SudokuConstant.SubGridLength; j++)
                {
                    if (gridNumbers[(row / SudokuConstant.SubGridLength) * SudokuConstant.cellLength + i, (col / SudokuConstant.SubGridLength) * SudokuConstant.cellLength + j] == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 根据已创建的答案来创建谜题
        /// </summary>
        private void CreatePuzzle()
        {
            System.Array.Copy(gridNumbers, puzzleNumbers, gridNumbers.Length);

            // 移除数字，制造难度
            for (int i = 0; i < DifficultLevel; i++)
            {
                int row = Utility.Random.GetRandom(0, SudokuConstant.GridLength);
                int col = Utility.Random.GetRandom(0, SudokuConstant.GridLength);

                // 循环随机，直到随到一个没有处理过的位置
                while (puzzleNumbers[row, col] == 0)
                {
                    row = Utility.Random.GetRandom(0, SudokuConstant.GridLength);
                    col = Utility.Random.GetRandom(0, SudokuConstant.GridLength);
                }

                puzzleNumbers[row, col] = 0;
            }

            // 确保至少要出现8个不同的数字，才能保证唯一解
            var onBoard = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            RandomizeList(onBoard);

            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                for (int j = 0; j < SudokuConstant.GridLength; j++)
                {
                    for (int k = 0; k < onBoard.Count - 1; k++)
                    {
                        if (onBoard[k] == puzzleNumbers[i, j])
                        {
                            onBoard.RemoveAt(k);
                        }
                    }
                }
            }

            // 如果剩余的数量大于1，说明已存在8个不同的数字，还原几个数字回来
            while (onBoard.Count - 1 > 1)
            {
                int row = Utility.Random.GetRandom(0, SudokuConstant.GridLength);
                int col = Utility.Random.GetRandom(0, SudokuConstant.GridLength);

                if (gridNumbers[row, col] == onBoard[0])
                {
                    puzzleNumbers[row, col] = gridNumbers[row, col];
                    onBoard.RemoveAt(0);
                }
            }

            // 备份谜题，方便重开本局
            System.Array.Copy(puzzleNumbers, puzzleBak, gridNumbers.Length);
        }

        /// <summary>
        /// 打乱一个list
        /// </summary>
        /// <param name="list"></param>
        private void RandomizeList(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                var rand = Utility.Random.GetRandom(i, list.Count);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }
    }
}