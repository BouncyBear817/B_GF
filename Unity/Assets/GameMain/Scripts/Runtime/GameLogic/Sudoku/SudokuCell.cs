using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class SudokuCell : MonoBehaviour
    {
        public Vector2Int Coordinate { get; private set; }
        public SudokuSubGrid SubGrid;
        public TextMeshProUGUI Number;
        public Button Button;
        public RectTransform InputTransform;

        /// <summary>
        /// 当前格子的值
        /// </summary>
        private int mValue = 0;

        private void Awake()
        {
            Number = transform.Find("Number").GetComponent<TextMeshProUGUI>();
            Button = GetComponent<Button>();
            Button.enabled = true;
            Button.onClick.AddListener(OnCellClick);
        }

        public void InitValues(int value)
        {
            mValue = value;
            Number.color = new Color(0.32f, 0.31f, 0.31f);
            if (mValue > 0)
            {
                Number.text = mValue.ToString();
                Button.enabled = false;
            }
        }

        public void SetValue(int value)
        {
            InitValues(value);
            Number.color = Color.blue;
        }

        public void SetCoordinate(int coordinateX, int coordinateY)
        {
            Coordinate = new Vector2Int(coordinateX, coordinateY);
        }

        public void SetSubGridParent(SudokuSubGrid sudokuSubGrid)
        {
            SubGrid = sudokuSubGrid;
        }

        private void OnCellClick()
        {
            MainEntry.Messenger.Broadcast(UIMsgId.OpenInputPanel, this);
        }
        
        
    }
}