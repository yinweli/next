using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

public sealed class ExcelToSQLite : ExcelExport
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
    /// SQLite連線物件
    /// </summary>
    private SQLiteConnection sqliteConnection = null;

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

        string targetFilePath = Path.Combine(SettingPath.targetPath, settingItem.targetDatabase) + ".db";

        if (File.Exists(targetFilePath) == false)
            SQLiteConnection.CreateFile(targetFilePath);

        sqliteConnection = new SQLiteConnection("Data Source=" + targetFilePath + ";Version=3;");
        sqliteConnection.Open();

        // 刪除舊的資料表
        try
        {
            using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
            {
                sqliteCommand.CommandText = "DROP TABLE IF EXISTS " + settingItem.targetTable;
                sqliteCommand.ExecuteNonQuery();
            }//using
        }
        catch (Exception e)
        {
            return Output.outputError(SettingItem.ToString(), "drop table failed, " + e.Message);
        }

        // 建立資料表
        try
        {
            using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
            {
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
                    sqliteCommand.CommandText = "CREATE TABLE " + settingItem.targetTable + " (" + createSyntax + ", PRIMARY KEY (" + primaryKeySyntax + "))";
                else
                    sqliteCommand.CommandText = "CREATE TABLE " + settingItem.targetTable + " (" + createSyntax + ")";

                sqliteCommand.ExecuteNonQuery();
            }//using
        }
        catch (Exception e)
        {
            return Output.outputError(SettingItem.ToString(), "create table failed, " + e.Message);
        }

        return true;
    }

    protected override bool readData(List<List<string>> datas)
    {
        try
        {
            using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
            {
                using (SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction())
                {
                    string insertSyntax = "";

                    foreach (Field itor in tableFields)
                    {
                        if (insertSyntax.Length > 0)
                            insertSyntax += ", ";

                        insertSyntax += "?";
                    }//for

                    sqliteCommand.CommandText = "INSERT INTO " + settingItem.targetTable + " VALUES (" + insertSyntax + ")";

                    foreach (Field itor in tableFields)
                        sqliteCommand.Parameters.Add(new SQLiteParameter(itor.sqliteType.dbType()));

                    foreach (List<string> itor in datas)
                    {
                        for (int i = 0; i < tableFields.Count; ++i)
                            sqliteCommand.Parameters[i].Value = itor.Count > i ? tableFields[i].sqliteType.parse(itor[i]) : "";

                        sqliteCommand.ExecuteNonQuery();
                    }//for

                    sqliteTransaction.Commit();
                }//using
            }//using
        }
        catch (Exception e)
        {
            return Output.outputError(SettingItem.ToString(), "insert data failed, " + e.Message);
        }

        return true;
    }

    protected override void readFinish()
    {
        if (sqliteConnection != null)
            sqliteConnection.Close();
    }
}