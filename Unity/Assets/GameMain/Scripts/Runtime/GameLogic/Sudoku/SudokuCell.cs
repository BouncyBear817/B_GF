using System;
using TMPro;
using UnityEngine;

namespace GameMain
{
    public class SudokuCell : MonoBehaviour
    {
        public Vector2Int Coordinate { get; private set; }

        public SudokuSubGrid SubGrid;

        public TextMeshProUGUI Number;

        /// <summary>
        /// 当前格子的值
        /// </summary>
        private int mValue = 0;

        private void Awake()
        {
            Number = transform.Find("Number").GetComponent<TextMeshProUGUI>();
        }

        public void InitValues(int value)
        {
            mValue = value;
            if (mValue > 0)
            {
                Number.text = mValue.ToString(); 
            }
        }

        public void SetCoordinate(int coordinateX, int coordinateY)
        {
            Coordinate = new Vector2Int(coordinateX, coordinateY);
        }

        public void SetSubGridParent(SudokuSubGrid sudokuSubGrid)
        {
            SubGrid = sudokuSubGrid;
        }
    }
}