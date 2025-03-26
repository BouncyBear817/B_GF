using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class SudokuCell : MonoBehaviour
    {
        public Vector2Int Coordinate;
        private TextMeshProUGUI Number;
        private Button Button;

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

        public void InitValues(int value, bool input = false)
        {
            mValue = value;
            if (value != 0)
            {
                Number.color =  input ? new Color32(0, 102, 187, 255) : new Color32(119, 110, 101, 255);
                Number.text = mValue.ToString();
                Button.enabled = false;
            }
            else
            {
                Number.text = "";
                Button.enabled = true;
            }
        }

        public int GetValue()
        {
            return mValue;
        }

        public void CheckError()
        {
            Number.color = Color.red;
            Button.enabled = true;
        }

        public void SetCoordinate(int coordinateX, int coordinateY)
        {
            Coordinate = new Vector2Int(coordinateX, coordinateY);
        }

        private void OnCellClick()
        {
            MainEntry.Messenger.Broadcast(UIMsgId.OpenInputPanel, this);
        }
    }
}