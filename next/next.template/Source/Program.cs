﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    private static void Main(string[] args)
    {
        start(args);
    }

    private static bool start(string[] args)
    {
        using (AutoStopWatch autoStopWatchGlobal = new AutoStopWatch("next.template"))
        {
            File.Delete(Output.errorLog);

            if (args.Length <= 0)
                return Output.outputError("missing parameter");

            if (File.Exists(args[0]) == false)
                return Output.outputError("setting file not exist");

            List<string> settings = File.ReadAllLines(args[0]).Where(itor => itor.Length > 0).ToList();

            if (settings.Count <= 0)
                return Output.outputError("setting file empty");

            SettingPath settingPath = JsonConvert.DeserializeObject<SettingPath>(settings[0]);

            if (settingPath == null)
                return Output.outputError("read setting path failed");

            settings.RemoveAt(0);

            List<SettingItem> settingItem = settings.Select(itor => JsonConvert.DeserializeObject<SettingItem>(itor)).ToList();

            if (settingItem.Count <= 0)
                return Output.outputError("read setting item failed");

            if (settingItem.Any(itor => itor == null))
                return Output.outputError("read setting item failed");

            foreach (SettingItem itor in settingItem)
            {
                Output.output(string.Format("{0} start", itor.ToString()));

                // 輸出成SQliteDB
                using (AutoStopWatch autoStopWatchLocal = new AutoStopWatch("ExcelToSQLite"))
                    new ExcelToSQLite().execute(settingPath, itor);

                // 輸出成內含SQL命令的文字檔案
                using (AutoStopWatch autoStopWatchLocal = new AutoStopWatch("ExcelToSQLText"))
                    new ExcelToSQLText().execute(settingPath, itor);
            }//for

            return true;
        }//using
    }
}