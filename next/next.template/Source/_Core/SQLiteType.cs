using System;
using System.Data;

namespace next.template
{
    public interface ISQLiteType
    {
        /// <summary>
        /// 取得SQLite型態字串
        /// </summary>
        /// <returns>SQLite型態字串</returns>
        string sqliteType();

        /// <summary>
        /// 取得資料庫型態列舉
        /// </summary>
        /// <returns>資料庫型態列舉</returns>
        DbType dbType();

        /// <summary>
        /// 取得資料值
        /// </summary>
        /// <param name="value">資料字串</param>
        /// <returns>資料值</returns>
        object parse(string value);
    }

    public class SQLiteTypeInteger : ISQLiteType
    {
        public string sqliteType()
        {
            return "INTEGER";
        }

        public DbType dbType()
        {
            return DbType.Int32;
        }

        public object parse(string value)
        {
            return Convert.ToInt32(value);
        }
    }

    public class SQLiteTypeReal : ISQLiteType
    {
        public string sqliteType()
        {
            return "REAL";
        }

        public DbType dbType()
        {
            return DbType.Double;
        }

        public object parse(string value)
        {
            return Convert.ToDouble(value);
        }
    }

    public class SQLiteTypeText : ISQLiteType
    {
        public string sqliteType()
        {
            return "TEXT";
        }

        public DbType dbType()
        {
            return DbType.String;
        }

        public object parse(string value)
        {
            return value;
        }
    }

    public class SQLiteType
    {
        public static ISQLiteType INTEGER = new SQLiteTypeInteger();
        public static ISQLiteType REAL = new SQLiteTypeReal();
        public static ISQLiteType TEXT = new SQLiteTypeText();

        public static ISQLiteType parse(string sqliteType)
        {
            if (sqliteType.CompareTo(INTEGER.sqliteType()) == 0)
                return INTEGER;

            if (sqliteType.CompareTo(REAL.sqliteType()) == 0)
                return REAL;

            if (sqliteType.CompareTo(TEXT.sqliteType()) == 0)
                return TEXT;

            return null;
        }
    }
}