using Newtonsoft.Json;
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
        using (AutoStopWatch autoStopWatch = new AutoStopWatch("next.template"))
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

            List<SettingSQLite> settingSQLites = settings.Select(itor => JsonConvert.DeserializeObject<SettingSQLite>(itor)).ToList();

            if (settingSQLites.Count <= 0)
                return Output.outputError("read setting sqlite failed");

            if (settingSQLites.Any(itor => itor == null))
                return Output.outputError("read setting sqlite failed");

            foreach (SettingSQLite itor in settingSQLites)
                new ExcelToSQLite().execute(settingPath, itor);

            return true;
        }//using
    }
}