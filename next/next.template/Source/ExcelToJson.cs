using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

public class ExcelToJson : ExcelExport
{
    /// <summary>
    /// 項目設定物件
    /// </summary>
    private SettingItem settingItem = null;

    /// <summary>
    /// 資料表欄位列表
    /// </summary>
    private List<Field> excelFields = null;

    /// <summary>
    /// 目標檔案路徑
    /// </summary>
    private string filepath = "";

    /// <summary>
    /// 目標檔案內容列表
    /// </summary>
    private List<string> fileContent = new List<string>();

    protected override bool readField(List<string> notes, List<string> fields)
    {
        settingItem = SettingItem as SettingItem;

        if (settingItem == null)
            return Output.outputError(SettingItem.ToString(), "setting item fromat error");

        excelFields = fields.Select(itor => Field.parse(itor)).ToList();

        if (excelFields.Count <= 0)
            return Output.outputError(SettingItem.ToString(), "fields empty");

        if (excelFields.Any(itor => itor == null))
            return Output.outputError(SettingItem.ToString(), "fields error");

        filepath = Path.Combine(SettingPath.targetPath, settingItem.targetDatabase) + "." + settingItem.targetTable + ".json.txt";
        fileContent.Clear();

        return true;
    }

    protected override bool readData(List<List<string>> datas)
    {
        foreach (List<string> itor in datas)
        {
            string jsonString = "";

            for (int i = 0; i < itor.Count; ++i)
            {
                if (jsonString.Length > 0)
                    jsonString += ", ";

                jsonString += "\"" + excelFields[i].field + "\":";

                if (excelFields[i].fieldType.dbType() == DbType.String)
                    jsonString += "\"" + itor[i] + "\"";
                else
                    jsonString += itor[i];
            }//for

            fileContent.Add("{" + jsonString + "}");
        }//for

        return true;
    }

    protected override void readFinish()
    {
        File.AppendAllLines(filepath, fileContent);
    }
}