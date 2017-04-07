using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;

// 用來讀取SQLite資料庫的元件
// 不支援多執行緒
// 
// 要正常使用需要將以下檔案放到Plugins資料夾中
// Mono.Data.Sqlite.dll
// Mono.Data.SqliteClient.dll
// sqlite3.dll
// System.Data.dll
// System.Data.Linq.dll
public class SQLiteResult : IEnumerable
{
    // 結果列表
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

public class SQLiteDataBase : IDisposable
{
    // 資料庫名稱
    private string dataSource = "";

    // 資料庫物件
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

    // 連線到資料庫
    public void open()
    {
        if (connect == null)
        {
            connect = new SqliteConnection(dataSource);
            connect.Open();
        }//if
    }

    // 執行sql語句
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