using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace next.template
{
    public sealed class ExcelToSQLite : Excel
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
                string[] parts = fieldString.Split(Define.TOKEN_FIELD_SEPARATOR.ToCharArray());

                if (parts.Length < 2)
                    return null;

                ISQLiteType sqliteType = SQLiteType.parse(parts[1]);

                if (sqliteType == null)
                    return null;

                return new Field()
                {
                    field = parts[0],
                    sqliteType = sqliteType,
                    primaryKey = parts.Length >= 3 && parts[2].CompareTo(Define.TOKEN_PRIMARY_KEY) == 0,
                };
            }
        }

        protected override bool read(SettingGlobal settingGlobal, SettingDetail settingDetail, List<string> fields, List<List<string>> datas)
        {
            if (settingGlobal == null)
                return Output.outputError(settingDetail.ToString(), "setting global null");

            if (settingDetail == null)
                return Output.outputError(settingDetail.ToString(), "setting detail null");

            List<Field> tableFields = fields.Select(itor => Field.parse(itor)).ToList();

            if (tableFields.Count <= 0)
                return Output.outputError(settingDetail.ToString(), "fields empty");

            if (tableFields.Any(itor => itor == null))
                return Output.outputError(settingDetail.ToString(), "fields error");

            string targetFilePath = Path.Combine(settingGlobal.targetPath, settingDetail.targetDatabase);

            if (File.Exists(targetFilePath) == false)
                SQLiteConnection.CreateFile(targetFilePath);

            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + targetFilePath + ";Version=3;"))
            {
                sqliteConnection.Open();

                // 刪除舊的資料表
                try
                {
                    using (SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand())
                    {
                        sqliteCommand.CommandText = "DROP TABLE IF EXISTS " + settingDetail.targetTable;
                        sqliteCommand.ExecuteNonQuery();
                    }//using
                }
                catch (Exception e)
                {
                    return Output.outputError(settingDetail.ToString(), "drop table failed, " + e.Message);
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
                            sqliteCommand.CommandText = "CREATE TABLE " + settingDetail.targetTable + " (" + createSyntax + ", PRIMARY KEY (" + primaryKeySyntax + "))";
                        else
                            sqliteCommand.CommandText = "CREATE TABLE " + settingDetail.targetTable + " (" + createSyntax + ")";

                        sqliteCommand.ExecuteNonQuery();
                    }//using
                }
                catch (Exception e)
                {
                    return Output.outputError(settingDetail.ToString(), "create table failed, " + e.Message);
                }

                // 新增資料
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

                            sqliteCommand.CommandText = "INSERT INTO " + settingDetail.targetTable + " VALUES (" + insertSyntax + ")";

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
                    return Output.outputError(settingDetail.ToString(), "insert data failed, " + e.Message);
                }
            }//using

            return true;
        }
    }
}