using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace next.template
{
    public abstract class Excel
    {
        /// <summary>
        /// 執行Excel內容的轉換程序
        /// </summary>
        /// <param name="settingGlobal">全域設定物件</param>
        /// <param name="settingDetail">細節設定物件</param>
        /// <returns>true表示成功, false則否</returns>
        public bool execute(SettingGlobal settingGlobal, SettingDetail settingDetail)
        {
            using (AutoStopWatch autoStopWatch = new AutoStopWatch(settingDetail.ToString()))
            {
                if (settingGlobal == null)
                    return Output.outputError("setting global null");

                if (settingDetail == null)
                    return Output.outputError("setting detail null");

                string sourceFilePath = Path.Combine(settingGlobal.sourcePath, settingDetail.sourceXls);

                if (File.Exists(sourceFilePath) == false)
                    return Output.outputError(settingDetail.ToString(), "excel not exist");

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(sourceFilePath)))
                    {
                        ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[settingDetail.sourceSheet];

                        if (excelWorkSheet == null)
                            return Output.outputError(settingDetail.ToString(), "sheet not exist");

                        if (excelWorkSheet.Dimension.Columns <= 0)
                            return Output.outputError(settingDetail.ToString(), "sheet column empty");

                        int countOfDataRow = Math.Max(Util.getExcelRowCount(excelWorkSheet) - Define.LINE_FIELD, 0);
                        List<string> fields = Util.getExcelRows(excelWorkSheet, Define.LINE_FIELD);
                        List<List<string>> datas = Enumerable.Range(Define.LINE_DATA, countOfDataRow).Select(itor => Util.getExcelRows(excelWorkSheet, itor, fields.Count)).ToList();

                        if (read(settingGlobal, settingDetail, fields, datas) == false)
                            return Output.outputError(settingDetail.ToString(), "read failed");
                    }//using
                }
                catch (Exception e)
                {
                    return Output.outputError(settingDetail.ToString(), "open excel file failed, " + e.ToString());
                }

                return true;
            }//using
        }

        /// <summary>
        /// 讀取資料
        /// </summary>
        /// <param name="settingGlobal">全域設定物件</param>
        /// <param name="settingDetail">細節設定物件</param>
        /// <param name="fields">欄位列表</param>
        /// <param name="datas">資料列表</param>
        /// <returns>true表示成功, false則否</returns>
        protected abstract bool read(SettingGlobal settingGlobal, SettingDetail settingDetail, List<string> fields, List<List<string>> datas);
    }
}