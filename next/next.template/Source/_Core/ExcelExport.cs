using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// 路徑設定類別
/// </summary>
public class SettingPath
{
    /// <summary>e流光
    /// 來源資料路徑
    /// </summary>
    public string sourcePath = "";

    /// <summary>
    /// 目標輸出路徑
    /// </summary>
    public string targetPath = "";
}

/// <summary>
/// 項目設定類別
/// </summary>
public class SettingItem
{
    /// <summary>
    /// 來源Excel檔名
    /// </summary>
    public string sourceXls = "";

    /// <summary>
    /// 來源Excel表單名稱
    /// </summary>
    public string sourceSheet = "";

    public override string ToString()
    {
        return string.Format("{0}#{1}", sourceXls, sourceSheet);
    }
}

/// <summary>
/// Excel匯出類別
/// </summary>
public abstract class ExcelExport
{
    /// <summary>
    /// 註解行號
    /// </summary>
    private const int LINE_NOTE = 1;

    /// <summary>
    /// 欄位行號
    /// </summary>
    private const int LINE_FIELD = 2;

    /// <summary>
    /// 資料起始行號
    /// </summary>
    private const int LINE_DATA = 3;

    /// <summary>
    /// 路徑設定物件
    /// </summary>
    private SettingPath settingPath = null;

    /// <summary>
    /// 項目設定物件
    /// </summary>
    private SettingItem settingItem = null;

    protected SettingPath SettingPath
    {
        get
        {
            return settingPath;
        }
    }

    protected SettingItem SettingItem
    {
        get
        {
            return settingItem;
        }
    }

    public bool execute(SettingPath settingPath, SettingItem settingItem)
    {
        using (AutoStopWatch autoStopWatch = new AutoStopWatch(settingItem.ToString()))
        {
            if (settingPath == null)
                return Output.outputError("setting path null");

            if (settingItem == null)
                return Output.outputError("setting item null");

            this.settingPath = settingPath;
            this.settingItem = settingItem;

            string sourceFilePath = Path.Combine(settingPath.sourcePath, settingItem.sourceXls);

            if (File.Exists(sourceFilePath) == false)
                return Output.outputError(settingItem.ToString(), "excel not exist");

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(sourceFilePath)))
                {
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[settingItem.sourceSheet];

                    if (excelWorkSheet == null)
                        return Output.outputError(settingItem.ToString(), "sheet not exist");

                    if (excelWorkSheet.Dimension.Columns <= 0)
                        return Output.outputError(settingItem.ToString(), "sheet column empty");

                    List<string> notes = Util.getExcelRows(excelWorkSheet, LINE_NOTE);
                    List<string> fields = Util.getExcelRows(excelWorkSheet, LINE_FIELD);

                    if (readField(notes, fields) == false)
                        return Output.outputError(settingItem.ToString(), "read field failed");

                    int countOfDataRow = Math.Max(Util.getExcelRowCount(excelWorkSheet) - LINE_FIELD, 0);
                    List<List<string>> datas = Enumerable.Range(LINE_DATA, countOfDataRow)
                        .Select(itor => Util.getExcelRows(excelWorkSheet, itor, fields.Count)).ToList();

                    if (readData(datas) == false)
                        return Output.outputError(settingItem.ToString(), "read data failed");
                }//using
            }
            catch (Exception e)
            {
                return Output.outputError(settingItem.ToString(), "open excel file failed, " + e.ToString());
            }
            finally
            {
                readFinish();
            }

            return true;
        }//using
    }

    /// <summary>
    /// 讀取欄位
    /// </summary>
    /// <param name="notes">註解列表</param>
    /// <param name="fields">欄位列表</param>
    /// <returns>true表示成功, false則否</returns>
    protected abstract bool readField(List<string> notes, List<string> fields);

    /// <summary>
    /// 讀取資料
    /// </summary>
    /// <param name="datas">資料列表</param>
    /// <returns>true表示成功, false則否</returns>
    protected abstract bool readData(List<List<string>> datas);

    /// <summary>
    /// 讀取完畢
    /// </summary>
    protected abstract void readFinish();
}