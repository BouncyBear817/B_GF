using UnityEngine;

namespace GameMain
{
    public class SudokuSubGrid : MonoBehaviour
    {
        public Vector2Int Coordinate { get; private set; }

        public SudokuCell[,] Cells { get; private set; }

        private void Awake()
        {
            Cells = new SudokuCell[SudokuConstant.cellLength, SudokuConstant.cellLength];
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
            var cellList = GetComponentsInChildren<SudokuCell>();
            for (int i = 0; i < SudokuConstant.cellLength; i++)
            {
                for (int j = 0; j < SudokuConstant.cellLength; j++)
                {
                    Cells[i, j] = cellList[i + j];
                    Cells[i, j].SetCoordinate(Coordinate.x * SudokuConstant.cellLength + i, Coordinate.y * SudokuConstant.cellLength + j);
                    Cells[i, j].SetSubGridParent(this);
                    Cells[i, j].InitValues(0);
                }
            }
        }
    }
}