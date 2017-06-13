using System;
using System.Diagnostics;
using System.IO;

/// <summary>
/// 輸出訊息類別
/// </summary>
public class Output
{
    /// <summary>
    /// 錯誤記錄檔案名稱
    /// </summary>
    public static string errorLog = "error.log";

    /// <summary>
    /// 輸出一般訊息
    /// </summary>
    /// <param name="message">訊息字串</param>
    /// <returns></returns>
    public static bool output(string message)
    {
        string temp = string.Format("{0:T} [Info] {1}", DateTime.Now, message);

        Debug.WriteLine(temp);
        Console.WriteLine(temp);

        return true;
    }

    /// <summary>
    /// 輸出錯誤訊息
    /// </summary>
    /// <param name="message">訊息字串</param>
    /// <returns></returns>
    public static bool outputError(string message)
    {
        string temp = string.Format("{0:T} [Error] {1}", DateTime.Now, message);

        Debug.WriteLine(temp);
        Console.WriteLine(temp);
        File.AppendAllText(errorLog, temp + "\n");

        return false;
    }

    /// <summary>
    /// 輸出錯誤訊息
    /// </summary>
    /// <param name="title">標題字串</param>
    /// <param name="message">訊息字串</param>
    /// <returns></returns>
    public static bool outputError(string title, string message)
    {
        return outputError(string.Format("{0} : {1}", title, message));
    }
}