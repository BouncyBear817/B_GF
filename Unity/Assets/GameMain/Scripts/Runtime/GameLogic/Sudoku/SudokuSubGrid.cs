using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class SudokuSubGrid : MonoBehaviour
    {
        public SudokuGrid mGrid { get; private set; }
        public List<SudokuCell> Cells;
        
        public Vector2Int Coordinate;

        private void Awake()
        {
            Cells = new List<SudokuCell>(SudokuConstant.cellLength * SudokuConstant.cellLength);
        }

        public void SetGrid(SudokuGrid grid)
        {
            mGrid = grid;
        }

        /// <summary>
        /// 设置行列坐标
        /// </summary>
        public void SetCoordinate(int row, int col)
        {
            Coordinate = new Vector2Int(row, col);
        }

        /// <summary>
        /// 初始化网格
        /// </summary>
        public void InitCells()
        {
            for (int i = 0; i < SudokuConstant.cellLength; i++)
            {
                for (int j = 0; j < SudokuConstant.cellLength; j++)
                {
                    var cell = Cells[j + SudokuConstant.cellLength * i];
                    cell.SetCoordinate(Coordinate.y * SudokuConstant.cellLength + j, Coordinate.x * SudokuConstant.cellLength + i);
                    cell.InitValues(0);
                }
            }
        }
    }
}