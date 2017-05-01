using System;

namespace next.net
{
    /// <summary>
    /// 資料緩衝區元件
    /// </summary>
    public class DataBuffer
    {
        private const int BUFFER_LENGTH = 8192; // 初始的緩衝區長度

        private int length = 0; // 資料總長度
        private byte[] data = new byte[BUFFER_LENGTH]; // 資料陣列
        private byte[] temp = new byte[BUFFER_LENGTH]; // 暫時陣列, 在擴展資料陣列以及從資料陣列中刪除資料時使用

        /// <summary>
        /// 取得緩衝區中資料長度
        /// </summary>
        /// <returns>資料長度</returns>
        public int Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// 取得緩衝區中資料陣列
        /// </summary>
        /// <returns>資料陣列</returns>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// 新增資料到緩衝區中
        /// </summary>
        /// <param name="data">資料陣列</param>
        /// <param name="length">資料長度</param>
        public void push(byte[] data, int length)
        {
            if (data == null)
                return;

            if (data.Length <= 0)
                return;

            if (length <= 0)
                return;

            expandData(this.length + length);
            Array.Copy(data, 0, this.data, this.length, length);
            this.length += length;
        }

        /// <summary>
        /// 從緩衝區中刪除資料
        /// </summary>
        /// <param name="pos">刪除資料終點</param>
        public void pop(int pos)
        {
            if (pos <= 0)
                return;

            length -= pos;

            if (length <= 0)
                return;

            expandTemp(length);
            Array.Copy(data, pos, temp, 0, length);
            Array.Copy(temp, 0, data, 0, length);
        }

        /// <summary>
        /// 依照長度來擴展資料陣列
        /// </summary>
        /// <param name="length">資料長度</param>
        private void expandData(int length)
        {
            if (data.Length >= length)
                return;

            expandTemp(length);

            if (length > 0)
                Array.Copy(data, 0, temp, 0, data.Length);

            data = new byte[length];

            if (length > 0)
                Array.Copy(temp, 0, data, 0, length);
        }

        /// <summary>
        /// 依照長度來擴展暫時陣列
        /// </summary>
        /// <param name="length">資料長度</param>
        private void expandTemp(int length)
        {
            if (temp.Length >= length)
                return;

            temp = new byte[length];
        }
    }
}