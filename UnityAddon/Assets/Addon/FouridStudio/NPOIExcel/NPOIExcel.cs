using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace FouridStudio
{
    /// <summary>
    /// Excel操作類別
    /// </summary>
    public class NPOIExcel
    {
        /// <summary>
        /// Workbook物件
        /// </summary>
        private IWorkbook workbook = null;

        /// <summary>
        /// 建立新的Excel
        /// </summary>
        public NPOIExcel()
        {
            workbook = new HSSFWorkbook();
        }

        /// <summary>
        /// 讀取已有的Excel
        /// </summary>
        /// <param name="path"></param>
        public NPOIExcel(string path)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                workbook = new HSSFWorkbook(fileStream);
            }
        }

        public NPOISheet this[string sheetName]
        {
            get
            {
                ISheet sheet = workbook.GetSheet(sheetName);

                return new NPOISheet(sheet == null ? workbook.CreateSheet(sheetName) : sheet);
            }
        }

        public void save(string path)
        {
            using (FileStream fileStream = File.OpenWrite(path))
            {
                workbook.Write(fileStream);
            }
        }
    }

    public class NPOISheet
    {
        private ISheet sheet = null;

        public NPOISheet(ISheet sheet)
        {
            this.sheet = sheet;
        }

        public NPOICell this[int rowNumber, int cellNumber]
        {
            get
            {
                IRow row = sheet.GetRow(rowNumber);

                if (row == null)
                    row = sheet.CreateRow(rowNumber);

                ICell cell = row.GetCell(cellNumber);

                if (cell == null)
                    cell = row.CreateCell(cellNumber);

                return new NPOICell(cell);
            }
        }
    }

    public class NPOICell
    {
        private ICell cell = null;

        public NPOICell(ICell cell)
        {
            this.cell = cell;
        }

        public int getInteger()
        {
            return (int)cell.NumericCellValue;
        }

        public double getDouble()
        {
            return cell.NumericCellValue;
        }

        public string getString()
        {
            return cell.StringCellValue;
        }

        public DateTime getDateTime()
        {
            return cell.DateCellValue;
        }

        public void setValue(object obj)
        {
            if (obj.GetType() == typeof(int))
                cell.SetCellValue((int)obj);
            else if (obj.GetType() == typeof(double))
                cell.SetCellValue((double)obj);
            else if (obj.GetType() == typeof(string))
                cell.SetCellValue(obj.ToString());
            else if (obj.GetType() == typeof(DateTime))
                cell.SetCellValue((DateTime)obj);
            else if (obj.GetType() == typeof(bool))
                cell.SetCellValue((bool)obj);
            else
                cell.SetCellValue(obj.ToString());
        }
    }
}