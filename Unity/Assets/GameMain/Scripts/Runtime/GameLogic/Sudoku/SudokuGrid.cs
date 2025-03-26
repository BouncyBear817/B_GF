using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SudokuGrid : MonoBehaviour
    {
        /// <summary>
        /// 所有的子网格
        /// </summary>
        public SudokuSubGrid[,] SubGrids;

        /// <summary>
        /// 所有的单元格
        /// </summary>
        public List<SudokuCell> Cells;

        private void Awake()
        {
            // 建立子网格的二维数组
            SubGrids = new SudokuSubGrid[SudokuConstant.SubGridLength, SudokuConstant.SubGridLength];
            Cells = new List<SudokuCell>(SudokuConstant.GridLength * SudokuConstant.GridLength);
        }

        public void Init()
        {
            var grid = GetComponentsInChildren<SudokuSubGrid>();

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
        }

        public IEnumerator GenerateGrid(SudokuSubGrid sudokuSubGrid, SudokuCell sudokuCell)
        {
            for (int i = 0; i < SudokuConstant.GridLength; i++)
            {
                var subGrid = Instantiate<SudokuSubGrid>(sudokuSubGrid, transform);
                subGrid.name = $"{i / 3}{i % 3}";
                subGrid.gameObject.SetActive(true);
                subGrid.SetGrid(this);
                for (int j = 0; j < SudokuConstant.GridLength; j++)
                {
                    var cell = Instantiate<SudokuCell>(sudokuCell, subGrid.transform);
                    cell.name = $"{i}_{j / 3}{j % 3}";
                    cell.gameObject.SetActive(true);
                    Cells.Add(cell);
                    subGrid.Cells.Add(cell);
                }
                
                yield return new WaitForEndOfFrame();
            }

            yield return 0;
            Init();
        }

        public SudokuCell GetCellByPos(int row, int col)
        {
            // var factor1 = (row / SudokuConstant.SubGridLength) * SudokuConstant.cellLength;
            // var factor2 = col / SudokuConstant.SubGridLength;
            // var index = SudokuConstant.SubGridLength * (row + 2 * (factor1 + factor2)) + col;
            // return Cells[index];
            foreach (var cell in Cells)
            {
                if (cell.Coordinate.x == row && cell.Coordinate.y == col)
                {
                    return cell;
                }
            }

            return null;
        }
    }
}