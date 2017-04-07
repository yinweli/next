using System.Collections.Generic;
using UnityEngine;

public class Courier : Singleton<Courier>
{
    // 接收者類別
    private class Recipient
    {
        // 接收者列表
        private Dictionary<object, Handler> recipients = new Dictionary<object, Handler>();

        // 新增接收處理
        public void addHandler(object recipient, Handler handler)
        {
            recipients.Add(recipient, handler);
        }

        // 移除接收處理
        public void removeHandler(object recipient)
        {
            recipients.Remove(recipient);
        }

        // 新增通知
        public bool notice(object content)
        {
            foreach (Handler itor in recipients.Values)
                itor(content);

            return recipients.Count > 0;
        }
    }

    // 委派型態:接收處理
    public delegate void Handler(object content);

    // 接收者列表
    private Dictionary<string, Recipient> handlers = new Dictionary<string, Recipient>();

    // 新增接收處理
    public void addHandler(string subject, object recipient, Handler handler)
    {
        if (handlers.ContainsKey(subject) == false)
            handlers.Add(subject, new Recipient());

        handlers[subject].addHandler(recipient, handler);
    }

    // 移除接收處理
    public void removeHandler(string subject, object recipient)
    {
        if (handlers.ContainsKey(subject))
            handlers[subject].removeHandler(recipient);
    }

    // 新增通知
    public void notice(string subject, object content)
    {
        Recipient recipient = null;

        if (handlers.TryGetValue(subject, out recipient) == false)
        {
            missedLog(subject, "handler not found");
            return;
        }//if

        if (recipient.notice(content) == false)
        {
            missedLog(subject, "handler empty");
            return;
        }//if
    }

    // 遺失紀錄
    private static void missedLog(string subject, string cause)
    {
        Debug.Log(string.Format("Courier missed: {0}: {1}", subject, cause));
    }
}