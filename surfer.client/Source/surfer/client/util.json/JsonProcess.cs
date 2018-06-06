namespace FouridStudio
{
    /// <summary>
    /// json處理介面
    /// </summary>
    public interface JsonProcess
    {
        /// <summary>
        /// 轉換物件為json字串
        /// </summary>
        /// <param name="obj">物件</param>
        /// <returns>json字串</returns>
        string toJson(System.Object obj);

        /// <summary>
        /// 轉換json字串為物件
        /// </summary>
        /// <param name="json">json字串</param>
        /// <returns>物件</returns>
        T toObject<T>(string json);
    }
}