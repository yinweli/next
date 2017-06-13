using UnityEngine;

namespace FouridStudio
{
    /// <summary>
    /// 報告類別
    /// </summary>
    public class Report : Singleton<Report>
    {
        /// <summary>
        /// 紀錄
        /// </summary>
        /// <param name="message">訊息</param>
        public void log(string message)
        {
            Debug.Log("[Log]" + message);
        }

        /// <summary>
        /// 訊息
        /// </summary>
        /// <typeparam name="T">傳回值型態</typeparam>
        /// <param name="message">訊息</param>
        /// <param name="obj">傳回物件</param>
        /// <returns>傳回物件</returns>
        public T info<T>(string message, T obj)
        {
            Debug.Log("[Info] " + message);

            return obj;
        }

        /// <summary>
        /// 錯誤
        /// </summary>
        /// <typeparam name="T">傳回值型態</typeparam>
        /// <param name="message">訊息</param>
        /// <param name="obj">傳回物件</param>
        /// <returns>傳回物件</returns>
        public T error<T>(string message, T obj)
        {
            Debug.Log("[Error] " + message);

            return obj;
        }
    }
}