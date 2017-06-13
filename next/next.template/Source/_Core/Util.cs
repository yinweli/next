using OfficeOpenXml;
using System.Collections.Generic;

/// <summary>
/// 工具類別
/// </summary>
public class Util
{
    /// <summary>
    /// 取得Execel表單中行數
    /// </summary>
    /// <param name="excelWorkSheet">Excel表單</param>
    /// <returns>行數</returns>
    public static int getExcelRowCount(ExcelWorksheet excelWorkSheet)
    {
        for (int i = 1; i <= excelWorkSheet.Dimension.End.Row; ++i)
        {
            var cell = excelWorkSheet.Cells[i, 1];

            if (cell == null || cell.Value == null || cell.Value.ToString().Length <= 0)
                return i - 1;
        }//for

        return excelWorkSheet.Dimension.End.Row;
    }

    /// <summary>
    /// 從Excel表單中以字串列表的形式取得指定行的內容
    /// 遇到空白格時會停止
    /// </summary>
    /// <param name="excelWorkSheet">Excel表單</param>
    /// <param name="row">行號, 從1起算</param>
    /// <returns>字串列表</returns>
    public static List<string> getExcelRows(ExcelWorksheet excelWorkSheet, int row)
    {
        List<string> rows = new List<string>();

        for (int i = 1; i <= excelWorkSheet.Dimension.End.Column; ++i)
        {
            var cell = excelWorkSheet.Cells[row, i];

            if (cell == null || cell.Value == null || cell.Value.ToString().Length <= 0)
                return rows;

            rows.Add(cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty);
        }//for

        return rows;
    }

    /// <summary>
    /// 從Excel表單中以字串列表的形式取得指定行的內容
    /// 讀取到指定的欄號
    /// </summary>
    /// <param name="excelWorkSheet">Excel表單</param>
    /// <param name="row">行號, 從1起算</param>
    /// <param name="column">欄號, 從1起算</param>
    /// <returns>字串列表</returns>
    public static List<string> getExcelRows(ExcelWorksheet excelWorkSheet, int row, int column)
    {
        List<string> rows = new List<string>();

        for (int i = 1; i <= column; ++i)
        {
            var cell = excelWorkSheet.Cells[row, i];

            rows.Add(cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty);
        }//for

        return rows;
    }
}