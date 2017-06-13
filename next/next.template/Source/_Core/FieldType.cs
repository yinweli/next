using System;
using System.Data;

/// <summary>
/// 欄位型態介面
/// </summary>
public interface IFieldType
{
    /// <summary>
    /// 取得欄位型態字串
    /// </summary>
    /// <returns>欄位型態字串</returns>
    string fieldType();

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

/// <summary>
/// 整數欄位類別
/// </summary>
public class FieldTypeInteger : IFieldType
{
    public string fieldType()
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

/// <summary>
/// 浮點數欄位類別
/// </summary>
public class FieldTypeReal : IFieldType
{
    public string fieldType()
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

/// <summary>
/// 字串欄位類別
/// </summary>
public class FieldTypeText : IFieldType
{
    public string fieldType()
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

/// <summary>
/// 欄位型態類別
/// </summary>
public class FieldType
{
    public static IFieldType INTEGER = new FieldTypeInteger();
    public static IFieldType REAL = new FieldTypeReal();
    public static IFieldType TEXT = new FieldTypeText();

    public static IFieldType parse(string fieldType)
    {
        if (fieldType.CompareTo(INTEGER.fieldType()) == 0)
            return INTEGER;

        if (fieldType.CompareTo(REAL.fieldType()) == 0)
            return REAL;

        if (fieldType.CompareTo(TEXT.fieldType()) == 0)
            return TEXT;

        return null;
    }
}