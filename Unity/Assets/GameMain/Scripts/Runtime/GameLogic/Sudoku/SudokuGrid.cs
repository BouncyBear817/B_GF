using System.Collections;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SudokuGrid : MonoBehaviour
    {
        /// <summary>
        /// 所有的子网格
        /// </summary>
        public SudokuSubGrid[,] SubGrids { get; private set; }

        /// <summary>
        /// 所有的单元格
        /// </summary>
        public SudokuCell[] Cells;

        public void Init()
        {
            var grid = GetComponentsInChildren<SudokuSubGrid>();
            // 建立子网格的二维数组
            SubGrids = new SudokuSubGrid[SudokuConstant.SubGridLength, SudokuConstant.SubGridLength];

            var index = 0;
            for (int i = 0; i < SudokuConstant.SubGridLength; i++)
            {
                for (int j = 0; j < SudokuConstant.SubGridLength; j++)
                {
                    SubGrids[i, j] = grid[index++];
                    SubGrids[i, j].SetCoordinate(i, j); // 设置坐标
                    SubGrids[i, j].InitCells(); //初始化网格
                }
            }

            Cells = GetComponentsInChildren<SudokuCell>();
        }

        public IEnumerator GenerateGrid(SudokuSubGrid sudokuSubGrid, SudokuCell sudokuCell)
        {
            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                var subGrid = Instantiate(sudokuSubGrid.transform, transform);
                subGrid.name = $"{i / 3}{i % 3}";
                subGrid.gameObject.SetActive(true);
                yield return new WaitForEndOfFrame();

                for (int j = 0; j < SudokuConstant.GridLength; j++)
                {
                    var cell = Instantiate(sudokuCell.transform, subGrid);
                    cell.name = $"{i}_{j / 3}{j % 3}";
                    cell.gameObject.SetActive(true);
                }
            }

            yield return 0;
            Init();
        }

        public SudokuCell GetCellByPos(int row, int col)
        {
            var factor1 = (row / SudokuConstant.SubGridLength) * SudokuConstant.cellLength;
            var factor2 = col / SudokuConstant.SubGridLength;
            var index = SudokuConstant.SubGridLength * (row + 2 * (factor1 + factor2)) + col;
            return Cells[index];
        }
    }
}