using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

public sealed class ExcelToSQLite : ExcelExport
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
    /// SQLite連線物件
    /// </summary>
    private SQLiteConnection sqliteConnection = null;

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

        string targetFilePath = Path.Combine(SettingPath.targetPath, settingItem.targetDatabase) + ".sqlite.db";

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

                foreach (Field itor in excelFields)
                {
                    if (createSyntax.Length > 0)
                        createSyntax += ", ";

                    createSyntax += "\"" + itor.field + "\" " + itor.fieldType.fieldType();

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

                    foreach (Field itor in excelFields)
                    {
                        if (insertSyntax.Length > 0)
                            insertSyntax += ", ";

                        insertSyntax += "?";
                    }//for

                    sqliteCommand.CommandText = "INSERT INTO " + settingItem.targetTable + " VALUES (" + insertSyntax + ")";

                    foreach (Field itor in excelFields)
                        sqliteCommand.Parameters.Add(new SQLiteParameter(itor.fieldType.dbType()));

                    foreach (List<string> itor in datas)
                    {
                        for (int i = 0; i < excelFields.Count; ++i)
                            sqliteCommand.Parameters[i].Value = itor.Count > i ? excelFields[i].fieldType.parse(itor[i]) : "";

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