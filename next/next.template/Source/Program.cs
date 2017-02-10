using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace next.template
{
    public class Program
    {
        private static void Main(string[] args)
        {
            start(args);
        }

        private static bool start(string[] args)
        {
            using (AutoStopWatch autoStopWatch = new AutoStopWatch("nextTemplate"))
            {
                File.Delete(Define.FILE_NAME_ERROR);

                if (args.Length <= 0)
                    return Output.outputError("missing parameter");

                if (File.Exists(args[0]) == false)
                    return Output.outputError("setting file not exist");

                List<string> settings = File.ReadAllLines(args[0]).Where(itor => itor.Length > 0).ToList();

                if (settings.Count <= 0)
                    return Output.outputError("setting file empty");

                SettingGlobal settingGlobal = SettingGlobal.deserialize(settings[0]);

                if (settingGlobal == null)
                    return Output.outputError("read setting global failed");

                settings.RemoveAt(0);

                List<SettingDetail> settingDetails = settings.Select(itor => SettingDetail.deserialize(itor)).ToList();

                if (settingDetails.Count <= 0)
                    return Output.outputError("read setting detail failed");

                foreach (SettingDetail itor in settingDetails)
                    new ExcelToSQLite().execute(settingGlobal, itor);

                return true;
            }//using
        }
    }
}