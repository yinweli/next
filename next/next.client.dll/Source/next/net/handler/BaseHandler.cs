namespace next.net
{
    /// <summary>
    /// 基礎封包處理類別
    /// </summary>
    public abstract class BaseHandler
    {
        /// <summary>
        /// 接收封包處理
        /// </summary>
        /// <param name="data">資料陣列</param>
        /// <param name="length">資料長度</param>
        protected internal abstract void recv(byte[] data, int length);

        /// <summary>
        /// 傳送封包處理
        /// </summary>
        /// <param name="objects">資料物件列表</param>
        /// <returns>資料陣列</returns>
        protected internal abstract byte[] send(params System.Object[] objects);
    }
}