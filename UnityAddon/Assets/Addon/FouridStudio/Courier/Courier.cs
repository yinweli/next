using System.Collections.Generic;
using UnityEngine;

namespace FouridStudio
{
    /// <summary>
    /// 訊息接收發佈類別
    /// 用來處理不同系統間的訊息傳遞
    /// </summary>
    public class Courier : Singleton<Courier>
    {
        /// <summary>
        /// 委派型態:接收處理
        /// </summary>
        /// <param name="content">內容</param>
        public delegate void Receiver(System.Object content);

        /// <summary>
        /// 接收者列表
        /// </summary>
        private Dictionary<string, Receiver> receivers = new Dictionary<string, Receiver>();

        /// <summary>
        /// 新增接收處理
        /// </summary>
        /// <param name="subject">標題</param>
        /// <param name="handler">處理委派</param>
        public void addHandler(string subject, Receiver handler)
        {
            if (receivers.ContainsKey(subject) == false)
                receivers.Add(subject, handler);
            else
                receivers[subject] += handler;
        }

        /// <summary>
        /// 移除接收處理
        /// </summary>
        /// <param name="subject">標題</param>
        /// <param name="handler">處理委派</param>
        public void removeHandler(string subject, Receiver handler)
        {
            if (receivers.ContainsKey(subject))
                receivers[subject] -= handler;
        }

        /// <summary>
        /// 發出通知
        /// </summary>
        /// <param name="subject">標題</param>
        /// <param name="content">內容</param>
        public void notice(string subject, System.Object content)
        {
            if (receivers.ContainsKey(subject))
                receivers[subject].Invoke(content);
            else
                Debug.Log(string.Format("Courier missed: {0}", subject));
        }
    }
}