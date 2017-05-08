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
    public class SQLiteDataBase : IDisposable
    {
        /// <summary>
        /// 資料庫名稱
        /// </summary>
        private string dataSource = "";

        /// <summary>
        /// 資料庫物件
        /// </summary>
        private SqliteConnection connect = null;

        public SQLiteDataBase(string dataSource)
        {
            this.dataSource = dataSource;
        }

        public void Dispose()
        {
            if (connect != null)
            {
                connect.Close();
                connect = null;
            }//if
        }

        /// <summary>
        /// 連線到資料庫
        /// </summary>
        public void open()
        {
            if (connect == null)
            {
                connect = new SqliteConnection(dataSource);
                connect.Open();
            }//if
        }

        /// <summary>
        /// 執行Sql語句
        /// </summary>
        /// <param name="sql">Sql語句</param>
        /// <returns>結果列表</returns>
        public List<SQLiteResult> query(string sql)
        {
            open();

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