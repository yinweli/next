/// <summary>
/// 欄位類別
/// </summary>
public class Field
{
    /// <summary>
    /// 欄位分隔符號
    /// </summary>
    private const string TOKEN_FIELD_SEPARATOR = "#";

    /// <summary>
    /// 主鍵符號
    /// </summary>
    private const string TOKEN_PRIMARY_KEY = "PK";

    /// <summary>
    /// 欄位名稱
    /// </summary>
    public string field = "";

    /// <summary>
    /// 欄位型態
    /// </summary>
    public IFieldType fieldType = null;

    /// <summary>
    /// 主鍵旗標
    /// </summary>
    public bool primaryKey = false;

    public static Field parse(string fieldString)
    {
        string[] parts = fieldString.Split(TOKEN_FIELD_SEPARATOR.ToCharArray());

        if (parts.Length < 2)
            return null;

        IFieldType fieldType = FieldType.parse(parts[1]);

        if (fieldType == null)
            return null;

        return new Field()
        {
            field = parts[0],
            fieldType = fieldType,
            primaryKey = parts.Length >= 3 && parts[2].CompareTo(TOKEN_PRIMARY_KEY) == 0,
        };
    }
}