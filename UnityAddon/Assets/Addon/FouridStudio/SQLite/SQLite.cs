using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FouridStudio
{
    /// <summary>
    /// 用來讀取SQLite資料庫的元件
    /// 注意!不支援多執行緒
    /// </summary>
    public class SQLite : IDisposable
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private SqliteConnection connect = null;

        public void Dispose()
        {
            close();
        }

        /// <summary>
        /// 開啟資料庫
        /// 通常用在要讀取SQLiteDB檔案時
        /// </summary>
        /// <param name="dataSource">資料來源字串</param>
        /// <returns>資料庫物件</returns>
        public SQLite open(string dataSource)
        {
            close();

            connect = new SqliteConnection(dataSource);
            connect.Open();

            return this;
        }

        /// <summary>
        /// 開啟資料庫
        /// 通常用在要讀取SQLite文字檔案時
        /// </summary>
        /// <param name="sqls">文字檔案內容列表</param>
        /// <returns>資料庫物件</returns>
        public SQLite open(IEnumerable<string> sqls)
        {
            close();

            connect = new SqliteConnection("Data Source=:memory:;Version=3;");
            connect.Open();

            foreach (string itor in sqls)
            {
                using (SqliteCommand command = connect.CreateCommand())
                {
                    command.CommandText = itor;
                    command.ExecuteNonQuery();
                }//using
            }//for

            return this;
        }

        /// <summary>
        /// 關閉資料庫
        /// </summary>
        public void close()
        {
            if (connect != null)
            {
                connect.Close();
                connect = null;
            }//if
        }

        /// <summary>
        /// 執行Sql語句
        /// </summary>
        /// <param name="sql">Sql語句</param>
        /// <returns>結果列表</returns>
        public List<SQLiteResult> query(string sql)
        {
            if (connect == null)
                throw new Exception("sqlite not open");

            using (SqliteCommand command = connect.CreateCommand())
            {
                command.CommandText = sql;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    List<SQLiteResult> results = new List<SQLiteResult>();

                    while (reader.Read())
                        results.Add(new SQLiteResult(reader));

                    return results;
                }//using
            }//using
        }
    }

    /// <summary>
    /// 結果類別
    /// </summary>
    public class SQLiteResult : IEnumerable
    {
        /// <summary>
        /// 結果列表
        /// </summary>
        private Dictionary<string, string> results = new Dictionary<string, string>();

        public SQLiteResult(SqliteDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; ++i)
                results[reader.GetName(i)] = Convert.ToString(reader.GetValue(i));
        }

        public string this[string key]
        {
            get
            {
                return results[key];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return results.GetEnumerator();
        }

        public Dictionary<string, string>.KeyCollection keys()
        {
            return results.Keys;
        }

        public Dictionary<string, string>.ValueCollection values()
        {
            return results.Values;
        }

        public bool isExist(string key)
        {
            return results.ContainsKey(key);
        }
    }
}