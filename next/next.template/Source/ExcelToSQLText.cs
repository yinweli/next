using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ExcelToSQLText : ExcelExport
{
    private class Field
    {
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string field = "";

        /// <summary>
        /// 欄位型態
        /// </summary>
        public ISQLiteType sqliteType = null;

        /// <summary>
        /// 主鍵旗標
        /// </summary>
        public bool primaryKey = false;

        public static Field parse(string fieldString)
        {
            string[] parts = fieldString.Split(TOKEN_FIELD_SEPARATOR.ToCharArray());

            if (parts.Length < 2)
                return null;

            ISQLiteType sqliteType = SQLiteType.parse(parts[1]);

            if (sqliteType == null)
                return null;

            return new Field()
            {
                field = parts[0],
                sqliteType = sqliteType,
                primaryKey = parts.Length >= 3 && parts[2].CompareTo(TOKEN_PRIMARY_KEY) == 0,
            };
        }
    }

    /// <summary>
    /// 欄位分隔符號
    /// </summary>
    private const string TOKEN_FIELD_SEPARATOR = "#";

    /// <summary>
    /// 主鍵符號
    /// </summary>
    private const string TOKEN_PRIMARY_KEY = "PK";

    /// <summary>
    /// SQLite設定物件
    /// </summary>
    private SettingItem settingItem = null;

    /// <summary>
    /// 資料表欄位列表
    /// </summary>
    private List<Field> tableFields = null;

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

        tableFields = fields.Select(itor => Field.parse(itor)).ToList();

        if (tableFields.Count <= 0)
            return Output.outputError(SettingItem.ToString(), "fields empty");

        if (tableFields.Any(itor => itor == null))
            return Output.outputError(SettingItem.ToString(), "fields error");

        filepath = Path.Combine(SettingPath.targetPath, settingItem.targetDatabase + "." + settingItem.targetTable) + ".txt";
        fileContent.Clear();

        // 刪除舊的資料表
        fileContent.Add("DROP TABLE IF EXISTS " + settingItem.targetTable);

        // 建立資料表
        string createSyntax = "";
        string primaryKeySyntax = "";

        foreach (Field itor in tableFields)
        {
            if (createSyntax.Length > 0)
                createSyntax += ", ";

            createSyntax += "\"" + itor.field + "\" " + itor.sqliteType.sqliteType();

            if (itor.primaryKey)
            {
                if (primaryKeySyntax.Length > 0)
                    primaryKeySyntax += ", ";

                primaryKeySyntax += "\"" + itor.field + "\"";
            } //if
        }//for

        if (primaryKeySyntax.Length > 0)
            fileContent.Add("CREATE TABLE " + settingItem.targetTable + " (" + createSyntax + ", PRIMARY KEY (" + primaryKeySyntax + "))");
        else
            fileContent.Add("CREATE TABLE " + settingItem.targetTable + " (" + createSyntax + ")");

        return true;
    }

    protected override bool readData(List<List<string>> datas)
    {
        foreach (List<string> itor in datas)
        {
            string insertSyntax = "";

            foreach (string itorData in itor)
            {
                if (insertSyntax.Length > 0)
                    insertSyntax += ", ";

                insertSyntax += itorData;
            }//for

            fileContent.Add("INSERT INTO " + settingItem.targetTable + " VALUES (" + insertSyntax + ")");
        }//for

        return true;
    }

    protected override void readFinish()
    {
        File.WriteAllLines(filepath, fileContent);
    }
}