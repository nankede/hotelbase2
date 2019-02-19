using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HotelBase.Common
{
    public class ExcelHelper

    {


        #region 设置表格内容
        private static void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ExcelImport(string strFileName)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);// 第二列为表头
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取excel,读取第二个Sheet
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable ExcelImport2(string strFileName)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(1);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(1); //读取第二个Sheet
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取Excel，第二行为表头
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable ExcelImport3(string strFileName, string fileEextension)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (fileEextension == ".xls")//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(1);// 第二行为表头
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i + 1);// 从第二行开始
                DataRow dataRow = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).StringCellValue;
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取多个Sheet
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static List<DataTable> ExcelImportAllSheets(string strFileName)
        {
            List<DataTable> dtList = new List<DataTable>();
            int sheetCount = 0;
            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheetCount = hssfworkbook.NumberOfSheets;
                    for (int i = 0; i < sheetCount; i++)
                    {
                        sheet = hssfworkbook.GetSheetAt(i);
                        dtList.Add(GetSheetTable(sheet));
                    }
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheetCount = xssfworkbook.NumberOfSheets;
                    for (int i = 0; i < sheetCount; i++)
                    {
                        sheet = xssfworkbook.GetSheetAt(i);
                        dtList.Add(GetSheetTable(sheet));
                    }
                }
            }
            return dtList;
        }

        public static DataTable GetSheetTable(ISheet sheet)
        {
            DataTable dt = new DataTable();
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {

                ICell cell = headerRow.GetCell(j);
                if (string.IsNullOrWhiteSpace(cell.ToString()))
                {
                    // 遇到空列，直接跳过
                    break;
                }
                else
                {
                    var headerCell = Regex.Replace(cell.ToString(), @"\s", "");
                    dt.Columns.Add(headerCell);
                }
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                // 去掉空行
                object[] itemArray = null;
                bool emptyRow = false;
                if (row != null)
                {
                    itemArray = new object[dt.Columns.Count];
                    for (int j = row.FirstCellNum; j < dt.Columns.Count; j++) //cellCount
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                                    itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                    itemArray[j] = row.GetCell(j).NumericCellValue;
                                }
                            }
                            if (row.GetCell(j).CellType == CellType.Formula)
                            {
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {
                                    case CellType.Numeric:
                                        dataRow[j] = row.GetCell(j).NumericCellValue;
                                        itemArray[j] = row.GetCell(j).NumericCellValue;
                                        break;
                                    case CellType.String:
                                        dataRow[j] = row.GetCell(j).StringCellValue;
                                        itemArray[j] = row.GetCell(j).StringCellValue;
                                        break;
                                    default:
                                        dataRow[j] = row.GetCell(j).ToString();
                                        itemArray[j] = row.GetCell(j).ToString();
                                        break;
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                                itemArray[j] = row.GetCell(j).ToString();
                            }
                            if (itemArray[j] != null && !string.IsNullOrEmpty(itemArray[j].ToString().Trim()))
                            {
                                emptyRow = true;
                            }
                        }
                    }
                    if (emptyRow)
                    {
                        dt.Rows.Add(dataRow);
                    }
                }
            }
            dt.TableName = sheet.SheetName; //tableName;
            return dt;
        }
        #endregion

        #region RGB颜色转NPOI颜色
        private static short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    s = XlColour.Indexed;
                }

            }
            else
                s = XlColour.Indexed;
            return s;
        }
        #endregion

        #region 设置列的对齐方式
        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static HorizontalAlignment getAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return HorizontalAlignment.Center;
                case "left":
                    return HorizontalAlignment.Left;
                case "right":
                    return HorizontalAlignment.Right;
                case "fill":
                    return HorizontalAlignment.Fill;
                case "justify":
                    return HorizontalAlignment.Justify;
                case "centerselection":
                    return HorizontalAlignment.CenterSelection;
                case "distributed":
                    return HorizontalAlignment.Distributed;
            }
            return NPOI.SS.UserModel.HorizontalAlignment.General;


        }

        #endregion

        /// <summary>
        /// 将DataTable的数据导入到一个已知的Excel中去
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DataTableToExcel(DataTable dt, string filePath)
        {
            bool result = false;
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;
            ISheet sheet = null;
            ICell cell = null;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet("Sheet1");//创建一个名称为Sheet0的表  
                    int rowCount = dt.Rows.Count;//行数  
                    int columnCount = dt.Columns.Count;//列数  

                    //设置列头  
                    row = sheet.CreateRow(0);//excel第一行设为列头  
                    for (int c = 0; c < columnCount; c++)
                    {
                        cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].ColumnName);
                    }

                    //设置每行每列的单元格,  
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);//excel第二行开始写入数据  
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    using (fs = File.OpenWrite(filePath))
                    {
                        workbook.Write(fs);//向打开的这个xls文件中写入数据  
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return false;
            }
        }

        /// <summary>
        /// 生成一个带表头的空的Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CreateEmptyExcel(DataTable dt, string filePath)
        {
            bool result = false;
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;
            ISheet sheet = null;
            ICell cell = null;
            try
            {
                workbook = new HSSFWorkbook();
                sheet = workbook.CreateSheet("Sheet1");//创建一个名称为Sheet0的表  
                int rowCount = dt.Rows.Count;//行数  
                int columnCount = dt.Columns.Count;//列数  
                int columnWidth = sheet.GetColumnWidth(columnCount) / 256;//获取当前列宽度
                //设置列头  
                row = sheet.CreateRow(0);//excel第一行设为列头  
                for (int c = 0; c < columnCount; c++)
                {
                    cell = row.CreateCell(c);
                    cell.SetCellValue(dt.Columns[c].ColumnName);
                    // row.GetCell(0).CellStyle = titleStyle;

                    ICellStyle style = workbook.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center; //居中
                    style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                    style.WrapText = false;//自动换行 


                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 14;
                    font.FontName = "宋体";
                    font.Boldweight = (short)FontBoldWeight.Bold;
                    style.SetFont(font);
                    row.GetCell(c).CellStyle = style;
                    IRow currentRow = sheet.GetRow(0);
                    ICell currentCell = currentRow.GetCell(c);
                    int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度
                    if (columnWidth < length + 1)
                    {
                        //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符
                        columnWidth = length + 1;
                    }
                    sheet.SetColumnWidth(c, columnWidth * 256);
                }
                using (fs = File.OpenWrite(filePath))
                {
                    workbook.Write(fs);//向打开的这个xls文件中写入数据  
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return false;
            }
        }

        /// <summary>
        /// 将多个DataTable导入到一张已知的Excel表中的多个Sheet中去，每个表的列数及列名可能不一样
        /// 需要注意的是，要给每个DataTable起不同的名字，这样便于给Sheet命名
        /// 注：ISheet，IRow，ICell应该放在循环之内声明
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DataTableToExcelMoreSheet(List<DataTable> dtList, string filePath)
        {
            bool result = false;
            IWorkbook workbook = new HSSFWorkbook();

            try
            {
                foreach (var item in dtList)
                {
                    if (item != null && item.Rows.Count > 0)
                    {
                        ICell cell = null;
                        ISheet sheet = (HSSFSheet)workbook.CreateSheet(item.TableName);//创建一个名称为Sheet0的表  
                        int rowCount = item.Rows.Count;//行数  
                        int columnCount = item.Columns.Count;//列数  

                        //设置列头  
                        IRow row = sheet.CreateRow(0);//excel第一行设为列头  
                        for (int c = 0; c < columnCount; c++)
                        {
                            cell = row.CreateCell(c);
                            cell.SetCellValue(item.Columns[c].ColumnName);
                        }

                        //设置每行每列的单元格,  
                        for (int i = 0; i < rowCount; i++)
                        {
                            row = sheet.CreateRow(i + 1);
                            for (int j = 0; j < columnCount; j++)
                            {
                                cell = row.CreateCell(j);//excel第二行开始写入数据  
                                cell.SetCellValue(item.Rows[i][j].ToString());
                            }
                        }
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fs);
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个新的Excel，并保存到文件夹
        /// </summary>
        /// <param name="fullPath">文件全路径</param>
        public static void NPOICreateExcel(string fullPath)
        {
            HSSFWorkbook hssfworkbook = null;
            FileStream file = null;
            try
            {
                hssfworkbook = new HSSFWorkbook();
                var sheet = hssfworkbook.CreateSheet("Sheet1");
                using (file = new FileStream(fullPath, FileMode.Create))
                {
                    hssfworkbook.Write(file);
                    file.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// DataTable导入到Excel中，并且下载，不保存到服务器上
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DataTableToExcelMoreSheetAndDown(List<DataTable> dtList, string fileName)
        {
            bool result = false;
            IWorkbook workbook = new HSSFWorkbook();
            try
            {
                foreach (var item in dtList)
                {
                    if (item != null && item.Rows.Count > 0)
                    {
                        ICell cell = null;
                        ISheet sheet = (HSSFSheet)workbook.CreateSheet(item.TableName);//创建一个名称为Sheet0的表  
                        int rowCount = item.Rows.Count;//行数  
                        int columnCount = item.Columns.Count;//列数  

                        //设置列头  
                        IRow row = sheet.CreateRow(0);//excel第一行设为列头  
                        for (int c = 0; c < columnCount; c++)
                        {
                            cell = row.CreateCell(c);
                            cell.SetCellValue(item.Columns[c].ColumnName);
                        }

                        //设置每行每列的单元格,  
                        for (int i = 0; i < rowCount; i++)
                        {
                            row = sheet.CreateRow(i + 1);
                            for (int j = 0; j < columnCount; j++)
                            {
                                cell = row.CreateCell(j);//excel第二行开始写入数据  
                                cell.SetCellValue(item.Rows[i][j].ToString());
                            }
                        }
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    //using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    //{
                    //    workbook.Write(fs);
                    //    result = true;
                    //}
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    //设置下载的Excel文件名
                    if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
                    {
                        fileName = HttpUtility.UrlPathEncode(fileName);
                    }

                    if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                    {
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                    }
                    else
                    {
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpContext.Current.Server.UrlEncode(fileName));
                    }
                    //将内存流转换成字节数组发送到客户端
                    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    System.Web.HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region 初始化
        /// <summary>
        /// 声明 HSSFWorkbook 对象
        /// </summary>
        private static HSSFWorkbook _workbook;
        /// <summary>
        /// 声明 HSSFSheet 对象
        /// </summary>
        private static HSSFSheet _sheet;
        #endregion

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="fileName">文件名称 如果为空或NULL，则默认“新建Excel.xls”</param>
        /// <param name="list"></param>
        /// <param name="method">导出方式 1：WEB导出（默认）2：按文件路径导出</param>
        /// <param name="folderPath">文件夹路径 如果WEB导出，则可以为空；如果按文件路径导出，则必须指定有权限访问的文件夹路径</param>
        /// <param name="deleteHead">是否删除表头(0:有表头；1：没有表头)</param>
        /// <param name="isSave">是否保存到服务器</param>
        /// <param name="isMoreSheet">是否多个Sheet</param>
        public static string NewExport(string fileName, IList<NPOIModel> list, int deleteHead, int method = 1, string folderPath = null, bool isSave = false, bool isMoreSheet = false, bool isEmpty = false)
        {
            // 文件名称
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.IndexOf('.') == -1)
                    fileName += ".xls";
                else
                    fileName = fileName.Substring(1, fileName.IndexOf('.')) + ".xls";
            }
            else
                fileName = "新建Excel.xls";

            // 文件路径
            if (2 == method && string.IsNullOrEmpty(folderPath))
                folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // 调用导出处理程序
            if (isMoreSheet)
            {
                NewExportMoreSheet(list, deleteHead);
            }
            else if (isEmpty)
            {
                NewExport(list, deleteHead, isEmpty);
            }
            else
            {
                NewExport(list, deleteHead);
            }
            var fullpath = folderPath + fileName;
            // WEB导出
            if (1 == method)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //将工作簿的内容放到内存流中
                        _workbook.Write(ms);
                        if (isSave)
                        {
                            //将生成的文件保存到服务器上
                            FileStream file = new FileStream(fullpath, FileMode.Create);
                            _workbook.Write(file);
                            file.Close();
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Response.Clear();
                            System.Web.HttpContext.Current.Response.ClearHeaders();
                            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                            //设置下载的Excel文件名
                            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
                            {
                                fileName = HttpUtility.UrlPathEncode(fileName);
                            }

                            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                            {
                                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                            }
                            else
                            {
                                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpContext.Current.Server.UrlEncode(fileName));
                            }
                            //将内存流转换成字节数组发送到客户端
                            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                            System.Web.HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());

                            //if (System.Web.HttpContext.Current.Response.IsClientConnected)
                            //    System.Web.HttpContext.Current.Response.WriteFile(filePath);
                            System.Web.HttpContext.Current.Response.Flush();
                            //System.Web.HttpContext.Current.Response.Close();

                            //System.Web.HttpContext.Current.Response.End();
                        }

                        _sheet = null;
                        _workbook = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            else if (2 == method)
            {
                using (FileStream fs = File.Open(folderPath, FileMode.Append))
                {
                    _workbook.Write(fs);
                    _sheet = null;
                    _workbook = null;
                }
            }
            return fullpath;
        }

        /// <summary>
        /// 导出方法实现
        /// </summary>
        /// <param name="list"></param>
        /// <param name="deleteHead">是否</param>
        private static void NewExport(IList<NPOIModel> list, int deleteHead)
        {
            #region 变量声明
            // 初始化
            _workbook = new HSSFWorkbook();
            // 声明 Row 对象
            IRow _row = null;
            // 声明 Cell 对象
            ICell _cell = null;
            // 总列数
            int cols = 0;
            // 总行数
            int rows = 0;
            // 行数计数器
            int rowIndex = 0;
            // 单元格值
            string drValue = null;
            //是否是HR导出
            var flag = false;
            ICellStyle myBodyStyle = bodyStyle;
            ICellStyle mybodyRightStyle = bodyRightStyle;
            ICellStyle mytitleStyle = titleStyle;
            ICellStyle mydateStyle = dateStyle;

            //当Status等于1时，需要改变颜色
            ICellStyle myRedBodyStyle = bodyRedStyle;
            ICellStyle myRedbodyRightStyle = bodyRedRightStyle;

            ICellStyle myReddateStyle = dateRedStyle;
            IFont myRedRowColorStyle = rowRedColorStyle;
            #endregion
            try
            {

                foreach (NPOIModel model in list)
                {
                    // 工作薄命名
                    if (model.sheetName != null)
                        _sheet = (HSSFSheet)_workbook.CreateSheet(model.sheetName);
                    else
                        _sheet = (HSSFSheet)_workbook.CreateSheet();

                    // 获取数据源
                    DataTable dt = model.dataSource;
                    // 初始化
                    rowIndex = 0;
                    // 获取总行数
                    rows = GetRowCount(model.headerName);
                    // 获取总列数
                    cols = GetColCount(model.headerName);

                    //    //HR导出工资单时，已经禁用的字体变红
                    if (dt.Columns.Contains("Status"))
                    {
                        flag = true;
                    }

                    // 循环行数
                    foreach (DataRow row in dt.Rows)
                    {
                        #region 新建表，填充表头，填充列头，样式
                        if (rowIndex == 65535 || rowIndex == 0)
                        {
                            if (rowIndex != 0)
                                _sheet = (HSSFSheet)_workbook.CreateSheet();

                            // 构建行
                            for (int i = 0; i < rows + model.isTitle; i++)
                            {
                                _row = _sheet.GetRow(i);
                                // 创建行
                                if (_row == null)
                                    _row = _sheet.CreateRow(i);

                                for (int j = 0; j < cols; j++)
                                {
                                    _row.CreateCell(j).CellStyle = flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                }
                            }

                            // 如果存在表标题
                            if (model.isTitle > 0)
                            {
                                // 获取行
                                _row = _sheet.GetRow(0);
                                // 合并单元格
                                CellRangeAddress region = new CellRangeAddress(0, 0, 0, (cols - 1));
                                _sheet.AddMergedRegion(region);
                                // 填充值
                                _row.CreateCell(0).SetCellValue(model.tableTitle);
                                // 设置样式
                                _row.GetCell(0).CellStyle = mytitleStyle; //titleStyle;
                                                                          // 设置行高
                                _row.HeightInPoints = 20;
                            }

                            // 取得上一个实体
                            NPOIHeader lastRow = null;
                            IList<NPOIHeader> hList = GetHeaders(model.headerName, rows, model.isTitle);
                            if (deleteHead == 0)
                            {
                                // 创建表头
                                foreach (NPOIHeader m in hList)
                                {
                                    var data = hList.Where(c => c.firstRow == m.firstRow && c.lastCol == m.firstCol - 1);
                                    if (data.Count() > 0)
                                    {
                                        lastRow = data.First();
                                        if (m.headerName == lastRow.headerName)
                                            m.firstCol = lastRow.firstCol;
                                    }

                                    // 获取行
                                    _row = _sheet.GetRow(m.firstRow);
                                    // 合并单元格
                                    CellRangeAddress region = new CellRangeAddress(m.firstRow, m.lastRow, m.firstCol, m.lastCol);
                                    _sheet.AddMergedRegion(region);
                                    // 填充值
                                    _row.CreateCell(m.firstCol).SetCellValue(m.headerName);
                                }

                                // 填充表头样式
                                for (int i = 0; i < rows + model.isTitle; i++)
                                {
                                    _row = _sheet.GetRow(i);
                                    for (int j = 0; j < cols; j++)
                                    {
                                        var colwidth = (model.colWidths[j] + 20) * 256;
                                        _row.GetCell(j).CellStyle = myBodyStyle;
                                        if (colwidth > 20000)
                                        {
                                            colwidth = 20000;
                                        }

                                        //设置列宽
                                        _sheet.SetColumnWidth(j, colwidth);//
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < rows + model.isTitle; i++)
                                {
                                    _row = _sheet.GetRow(i);
                                    for (int j = 0; j < cols; j++)
                                    {
                                        var colwidth = (model.colWidths[j] + 20) * 256;
                                        _row.GetCell(j).CellStyle = myBodyStyle;
                                        if (colwidth > 20000)
                                        {
                                            colwidth = 20000;
                                        }

                                        //设置列宽
                                        _sheet.SetColumnWidth(j, colwidth);//
                                    }
                                }
                            }
                            rowIndex = (rows + model.isTitle);
                        }
                        #endregion

                        #region 填充内容
                        // 构建列
                        _row = _sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in dt.Columns)
                        {
                            // 添加序号列
                            if (1 == model.isOrderby && column.Ordinal == 0)
                            {
                                _cell = _row.CreateCell(0);
                                _cell.SetCellValue(rowIndex - rows);
                                _cell.CellStyle = flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                            }

                            // 创建列
                            _cell = _row.CreateCell(column.Ordinal + model.isOrderby);

                            // 获取值
                            drValue = row[column].ToString();

                            switch (column.DataType.ToString())
                            {
                                case "System.String"://字符串类型
                                    _cell.SetCellValue(drValue);
                                    _cell.CellStyle = flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    if (DateTime.TryParse(drValue, out dateV))
                                    {
                                        _cell.SetCellValue(dateV.ToString("yyyy-MM-dd"));
                                    }
                                    else
                                    {
                                        _cell.SetCellValue("");
                                    }
                                    _cell.CellStyle = flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    _cell.SetCellValue(boolV);
                                    _cell.CellStyle = flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    _cell.SetCellValue(intV);
                                    _cell.CellStyle = flag == true ? (isRed(row) ? myRedbodyRightStyle : mybodyRightStyle) : mybodyRightStyle; //bodyRightStyle;
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    _cell.SetCellValue(doubV);
                                    _cell.CellStyle = flag == true ? (isRed(row) ? myRedbodyRightStyle : mybodyRightStyle) : mybodyRightStyle;
                                    break;
                                case "System.DBNull"://空值处理
                                    _cell.SetCellValue("");
                                    break;
                                default:
                                    _cell.SetCellValue("");
                                    break;
                            }

                        }

                        #endregion

                        rowIndex++;
                    }


                }
            }
            catch (Exception ex)
            {

            }

            if (deleteHead == 1)
            {
                if (_sheet.LastRowNum >= 1)
                {
                    _sheet.ShiftRows(1, _sheet.LastRowNum, -1);
                }
            }

            //最后一列隐藏
            if (flag)
            {
                _sheet.SetColumnHidden(cols - 1, true);
            }
        }

        /// <summary>
        /// 导出一个空的Excel，只有表头，支持表头合并
        /// 生成的Excel可以不用在服务器上保存，适合生成动态模板
        /// </summary>
        /// <param name="list"></param>
        /// <param name="deleteHead"></param>
        /// <param name="isEmpty"></param>
        private static void NewExport(IList<NPOIModel> list, int deleteHead, bool isEmpty = false)
        {
            #region 变量声明
            // 初始化
            _workbook = new HSSFWorkbook();
            // 声明 Row 对象
            IRow _row = null;
            // 声明 Cell 对象
            ICell _cell = null;
            // 总列数
            int cols = 0;
            // 总行数
            int rows = 0;
            // 行数计数器
            int rowIndex = 0;
            // 单元格值
            string drValue = null;
            //是否是HR导出
            var flag = false;
            ICellStyle myBodyStyle = bodyStyle;
            ICellStyle mybodyRightStyle = bodyRightStyle;
            ICellStyle mytitleStyle = titleStyle;
            ICellStyle mydateStyle = dateStyle;

            //当Status等于1时，需要改变颜色
            ICellStyle myRedBodyStyle = bodyRedStyle;
            ICellStyle myRedbodyRightStyle = bodyRedRightStyle;

            ICellStyle myReddateStyle = dateRedStyle;
            IFont myRedRowColorStyle = rowRedColorStyle;
            #endregion

            foreach (NPOIModel model in list)
            {
                // 工作薄命名
                if (model.sheetName != null)
                    _sheet = (HSSFSheet)_workbook.CreateSheet(model.sheetName);
                else
                    _sheet = (HSSFSheet)_workbook.CreateSheet();

                // 获取数据源
                DataTable dt = model.dataSource;
                // 初始化
                rowIndex = 0;
                // 获取总行数
                rows = GetRowCount(model.headerName);
                // 获取总列数
                cols = GetColCount(model.headerName);

                //    //HR导出工资单时，已经禁用的字体变红
                if (dt.Columns.Contains("Status"))
                {
                    flag = true;
                }

                if (isEmpty)
                {
                    // 循环行数
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                            _sheet = (HSSFSheet)_workbook.CreateSheet();

                        // 构建行
                        for (int i = 0; i < rows + model.isTitle; i++)
                        {
                            _row = _sheet.GetRow(i);
                            // 创建行
                            if (_row == null)
                                _row = _sheet.CreateRow(i);

                            for (int j = 0; j < cols; j++)
                            {
                                _row.CreateCell(j).CellStyle = flag == true ? (isRed(dt.Rows[0]) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                            }
                        }

                        // 如果存在表标题
                        if (model.isTitle > 0)
                        {
                            // 获取行
                            _row = _sheet.GetRow(0);
                            // 合并单元格
                            CellRangeAddress region = new CellRangeAddress(0, 0, 0, (cols - 1));
                            _sheet.AddMergedRegion(region);
                            // 填充值
                            _row.CreateCell(0).SetCellValue(model.tableTitle);
                            // 设置样式
                            _row.GetCell(0).CellStyle = mytitleStyle;
                            // 设置行高
                            _row.HeightInPoints = 20;
                        }

                        // 取得上一个实体
                        NPOIHeader lastRow = null;
                        IList<NPOIHeader> hList = GetHeaders(model.headerName, rows, model.isTitle);
                        if (deleteHead == 0)
                        {
                            // 创建表头
                            foreach (NPOIHeader m in hList)
                            {
                                var data = hList.Where(c => c.firstRow == m.firstRow && c.lastCol == m.firstCol - 1);
                                if (data.Count() > 0)
                                {
                                    lastRow = data.First();
                                    if (m.headerName == lastRow.headerName)
                                        m.firstCol = lastRow.firstCol;
                                }

                                // 获取行
                                _row = _sheet.GetRow(m.firstRow);
                                // 合并单元格
                                CellRangeAddress region = new CellRangeAddress(m.firstRow, m.lastRow, m.firstCol, m.lastCol);
                                _sheet.AddMergedRegion(region);
                                // 填充值
                                _row.CreateCell(m.firstCol).SetCellValue(m.headerName);
                            }

                            // 填充表头样式
                            for (int i = 0; i < rows + model.isTitle; i++)
                            {
                                _row = _sheet.GetRow(i);
                                for (int j = 0; j < cols; j++)
                                {
                                    var colwidth = (model.colWidths[j] + 20) * 256;
                                    _row.GetCell(j).CellStyle = myBodyStyle;
                                    if (colwidth > 20000)
                                    {
                                        colwidth = 20000;
                                    }

                                    //设置列宽
                                    //_sheet.SetColumnWidth(j, colwidth);//

                                    int columnWidth = _sheet.GetColumnWidth(cols) / 256;//获取当前列宽度
                                    IRow currentRow = _sheet.GetRow(0);
                                    ICell currentCell = currentRow.GetCell(j);
                                    int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度
                                    if (columnWidth < length + 1)
                                    {
                                        //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符
                                        columnWidth = length + 1;
                                    }
                                    _sheet.SetColumnWidth(j, columnWidth * 256);

                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < rows + model.isTitle; i++)
                            {
                                _row = _sheet.GetRow(i);
                                for (int j = 0; j < cols; j++)
                                {
                                    var colwidth = (model.colWidths[j] + 20) * 256;
                                    _row.GetCell(j).CellStyle = myBodyStyle;
                                    if (colwidth > 20000)
                                    {
                                        colwidth = 20000;
                                    }

                                    //设置列宽
                                    //_sheet.SetColumnWidth(j, colwidth);//
                                    int columnWidth = _sheet.GetColumnWidth(cols) / 256;//获取当前列宽度
                                    IRow currentRow = _sheet.GetRow(0);
                                    ICell currentCell = currentRow.GetCell(j);
                                    int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度
                                    if (columnWidth < length + 1)
                                    {
                                        //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符
                                        columnWidth = length + 1;
                                    }
                                    _sheet.SetColumnWidth(j, columnWidth * 256);

                                }
                            }
                        }
                        rowIndex = (rows + model.isTitle);
                    }
                    #endregion
                    rowIndex++;
                }
            }

            if (deleteHead == 1)
            {
                if (_sheet.LastRowNum >= 1)
                {
                    _sheet.ShiftRows(1, _sheet.LastRowNum, -1);
                }
            }

            //最后一列隐藏
            if (flag)
            {
                _sheet.SetColumnHidden(cols - 1, true);
            }
        }


        /// <summary>
        /// 导出多个Sheet的方法实现
        /// 只适合特定的导出
        /// </summary>
        /// <param name="list"></param>
        /// <param name="deleteHead"></param>
        private static void NewExportMoreSheet(IList<NPOIModel> list, int deleteHead)
        {
            #region 变量声明
            // 初始化
            _workbook = new HSSFWorkbook();
            // 声明 Row 对象
            IRow _row = null;
            // 声明 Cell 对象
            ICell _cell = null;
            // 总列数
            int cols = 0;
            // 总行数
            int rows = 0;
            // 行数计数器
            int rowIndex = 0;
            // 单元格值
            string drValue = null;
            //是否是HR导出
            //var flag = false;
            ICellStyle myBodyStyle = bodyStyle;
            ICellStyle mybodyRightStyle = bodyRightStyle;
            ICellStyle mytitleStyle = titleStyle;
            ICellStyle mydateStyle = dateStyle;

            //当Status等于1时，需要改变颜色
            //ICellStyle myRedBodyStyle = bodyRedStyle;
            //ICellStyle myRedbodyRightStyle = bodyRedRightStyle;
            //ICellStyle myReddateStyle = dateRedStyle;
            //IFont myRedRowColorStyle = rowRedColorStyle;
            #endregion

            foreach (NPOIModel model in list)
            {
                string[] thisSheet = new string[3];
                // 工作薄命名
                if (model.sheetName != null)
                {
                    thisSheet = model.sheetName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                }
                foreach (var item in thisSheet)
                {
                    _sheet = (HSSFSheet)_workbook.CreateSheet(item);
                    // 获取数据源
                    DataTable dt = model.dataSource.Clone();
                    DataRow[] dr = model.dataSource.Select("SocialSecurityTypeName ='" + item + "'");
                    for (int i = dr.Length - 1; i >= 0; i--)
                    {
                        dt.ImportRow((DataRow)dr[i]);
                    }
                    //DataTable dt = model.dataSource;
                    // 初始化
                    rowIndex = 0;
                    // 获取总行数
                    rows = GetRowCount(model.headerName);
                    // 获取总列数
                    cols = GetColCount(model.headerName);
                    ////    //HR导出工资单时，已经禁用的字体变红
                    //if (dt.Columns.Contains("Status"))
                    //{
                    //    flag = true;
                    //}
                    // 循环行数
                    foreach (DataRow row in dt.Rows)
                    {
                        #region 新建表，填充表头，填充列头，样式
                        if (rowIndex == 65535 || rowIndex == 0)
                        {
                            if (rowIndex != 0)
                                _sheet = (HSSFSheet)_workbook.CreateSheet();

                            // 构建行
                            for (int i = 0; i < rows + model.isTitle; i++)
                            {
                                _row = _sheet.GetRow(i);
                                // 创建行
                                if (_row == null)
                                    _row = _sheet.CreateRow(i);

                                for (int j = 0; j < cols; j++)
                                {
                                    _row.CreateCell(j).CellStyle = myBodyStyle; //flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                }
                            }

                            // 如果存在表标题
                            if (model.isTitle > 0)
                            {
                                // 获取行
                                _row = _sheet.GetRow(0);
                                // 合并单元格
                                CellRangeAddress region = new CellRangeAddress(0, 0, 0, (cols - 1));
                                _sheet.AddMergedRegion(region);
                                // 填充值
                                _row.CreateCell(0).SetCellValue(model.tableTitle);
                                // 设置样式
                                _row.GetCell(0).CellStyle = mytitleStyle; //titleStyle;
                                                                          // 设置行高
                                _row.HeightInPoints = 20;
                            }

                            // 取得上一个实体
                            NPOIHeader lastRow = null;
                            IList<NPOIHeader> hList = GetHeaders(model.headerName, rows, model.isTitle);
                            if (deleteHead == 0)
                            {
                                // 创建表头
                                foreach (NPOIHeader m in hList)
                                {
                                    var data = hList.Where(c => c.firstRow == m.firstRow && c.lastCol == m.firstCol - 1);
                                    if (data.Count() > 0)
                                    {
                                        lastRow = data.First();
                                        if (m.headerName == lastRow.headerName)
                                            m.firstCol = lastRow.firstCol;
                                    }

                                    // 获取行
                                    _row = _sheet.GetRow(m.firstRow);
                                    // 合并单元格
                                    CellRangeAddress region = new CellRangeAddress(m.firstRow, m.lastRow, m.firstCol, m.lastCol);
                                    _sheet.AddMergedRegion(region);
                                    // 填充值
                                    _row.CreateCell(m.firstCol).SetCellValue(m.headerName);
                                }

                                // 填充表头样式
                                for (int i = 0; i < rows + model.isTitle; i++)
                                {
                                    _row = _sheet.GetRow(i);
                                    for (int j = 0; j < cols; j++)
                                    {
                                        var colwidth = (model.colWidths[j] + 20) * 256;
                                        _row.GetCell(j).CellStyle = myBodyStyle;
                                        if (colwidth > 20000)
                                        {
                                            colwidth = 20000;
                                        }

                                        //设置列宽
                                        _sheet.SetColumnWidth(j, colwidth);//
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < rows + model.isTitle; i++)
                                {
                                    _row = _sheet.GetRow(i);
                                    for (int j = 0; j < cols; j++)
                                    {
                                        var colwidth = (model.colWidths[j] + 20) * 256;
                                        _row.GetCell(j).CellStyle = myBodyStyle;
                                        if (colwidth > 20000)
                                        {
                                            colwidth = 20000;
                                        }

                                        //设置列宽
                                        _sheet.SetColumnWidth(j, colwidth);
                                    }
                                }
                            }
                            rowIndex = (rows + model.isTitle);
                        }
                        #endregion
                        #region 填充内容
                        // 构建列
                        _row = _sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in dt.Columns)
                        {
                            // 添加序号列
                            if (1 == model.isOrderby && column.Ordinal == 0)
                            {
                                _cell = _row.CreateCell(0);
                                _cell.SetCellValue(rowIndex - rows);
                                _cell.CellStyle = myBodyStyle; //flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                            }

                            // 创建列
                            _cell = _row.CreateCell(column.Ordinal + model.isOrderby);

                            // 获取值
                            drValue = row[column].ToString();

                            switch (column.DataType.ToString())
                            {
                                case "System.String"://字符串类型
                                    _cell.SetCellValue(drValue);
                                    _cell.CellStyle = myBodyStyle; //flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    if (DateTime.TryParse(drValue, out dateV))
                                    {
                                        _cell.SetCellValue(dateV.ToString("yyyy-MM-dd"));
                                    }
                                    else
                                    {
                                        _cell.SetCellValue("");
                                    }
                                    _cell.CellStyle = myBodyStyle; //flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    _cell.SetCellValue(boolV);
                                    _cell.CellStyle = myBodyStyle;// flag == true ? (isRed(row) ? myRedBodyStyle : myBodyStyle) : myBodyStyle;
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    _cell.SetCellValue(intV);
                                    _cell.CellStyle = mybodyRightStyle; //flag == true ? (isRed(row) ? myRedbodyRightStyle : mybodyRightStyle) : mybodyRightStyle; //bodyRightStyle;
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    _cell.SetCellValue(doubV);
                                    _cell.CellStyle = mybodyRightStyle;// flag == true ? (isRed(row) ? myRedbodyRightStyle : mybodyRightStyle) : mybodyRightStyle;
                                    break;
                                case "System.DBNull"://空值处理
                                    _cell.SetCellValue("");
                                    break;
                                default:
                                    _cell.SetCellValue("");
                                    break;
                            }
                        }
                        // 隐藏最后一列
                        _sheet.SetColumnHidden(cols - 1, true);
                        #endregion
                        rowIndex++;
                    }
                }

                if (deleteHead == 1)
                {
                    if (_sheet.LastRowNum >= 1)
                    {
                        _sheet.ShiftRows(1, _sheet.LastRowNum, -1);
                    }
                }

            }
        }

        #region 辅助方法
        /// <summary>
        /// 表头解析
        /// </summary>
        /// <remarks>
        /// author:zhujt
        /// create date:2015-9-10 19:24:51
        /// </remarks>
        /// <param name="header">表头</param>
        /// <param name="rows">总行数</param>
        /// <param name="addRows">外加行</param>
        /// <param name="addCols">外加列</param>
        /// <returns></returns>
        private static IList<NPOIHeader> GetHeaders(string header, int rows, int addRows)
        {
            // 临时表头数组
            string[] tempHeader;
            string[] tempHeader2;
            // 所跨列数
            int colSpan = 0;
            // 所跨行数
            int rowSpan = 0;
            // 单元格对象
            NPOIHeader model = null;
            // 行数计数器
            int rowIndex = 0;
            // 列数计数器
            int colIndex = 0;
            // 
            IList<NPOIHeader> list = new List<NPOIHeader>();
            // 初步解析
            string[] headers = header.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            // 表头遍历
            for (int i = 0; i < headers.Length; i++)
            {
                // 行数计数器清零
                rowIndex = 0;
                // 列数计数器清零
                colIndex = 0;
                // 获取所跨行数
                rowSpan = GetRowSpan(headers[i], rows);
                // 获取所跨列数
                colSpan = GetColSpan(headers[i]);

                // 如果所跨行数与总行数相等，则不考虑是否合并单元格问题
                if (rows == rowSpan)
                {
                    colIndex = GetMaxCol(list);
                    model = new NPOIHeader(headers[i],
                        addRows,
                        (rowSpan - 1 + addRows),
                        colIndex,
                        (colSpan - 1 + colIndex),
                        addRows);
                    list.Add(model);
                    rowIndex += (rowSpan - 1) + addRows;
                }
                else
                {
                    // 列索引
                    colIndex = GetMaxCol(list);
                    // 如果所跨行数不相等，则考虑是否包含多行
                    tempHeader = headers[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < tempHeader.Length; j++)
                    {

                        // 如果总行数=数组长度
                        if (1 == GetColSpan(tempHeader[j]))
                        {
                            if (j == tempHeader.Length - 1 && tempHeader.Length < rows)
                            {
                                model = new NPOIHeader(tempHeader[j],
                                    (j + addRows),
                                    (j + addRows) + (rows - tempHeader.Length),
                                    colIndex,
                                    (colIndex + colSpan - 1),
                                    addRows);
                                list.Add(model);
                            }
                            else
                            {
                                model = new NPOIHeader(tempHeader[j],
                                        (j + addRows),
                                        (j + addRows),
                                        colIndex,
                                        (colIndex + colSpan - 1),
                                        addRows);
                                list.Add(model);
                            }
                        }
                        else
                        {
                            // 如果所跨列数不相等，则考虑是否包含多列
                            tempHeader2 = tempHeader[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            for (int m = 0; m < tempHeader2.Length; m++)
                            {
                                // 列索引
                                colIndex = GetMaxCol(list) - colSpan + m;
                                if (j == tempHeader.Length - 1 && tempHeader.Length < rows)
                                {
                                    model = new NPOIHeader(tempHeader2[m],
                                        (j + addRows),
                                        (j + addRows) + (rows - tempHeader.Length),
                                        colIndex,
                                        colIndex,
                                        addRows);
                                    list.Add(model);
                                }
                                else
                                {
                                    model = new NPOIHeader(tempHeader2[m],
                                            (j + addRows),
                                            (j + addRows),
                                            colIndex,
                                            colIndex,
                                            addRows);
                                    list.Add(model);
                                }
                            }
                        }
                        rowIndex += j + addRows;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取最大列
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int GetMaxCol(IList<NPOIHeader> list)
        {
            int maxCol = 0;
            if (list.Count > 0)
            {
                foreach (NPOIHeader model in list)
                {
                    if (maxCol < model.lastCol)
                        maxCol = model.lastCol;
                }
                maxCol += 1;
            }

            return maxCol;
        }

        /// <summary>
        /// 获取表头行数
        /// </summary>
        /// <param name="newHeaders">表头文字</param>
        /// <returns></returns>
        private static int GetRowCount(string newHeaders)
        {
            string[] ColumnNames = newHeaders.Split(new char[] { '@' });
            int Count = 0;
            if (ColumnNames.Length <= 1)
                ColumnNames = newHeaders.Split(new char[] { '#' });
            foreach (string name in ColumnNames)
            {
                int TempCount = name.Split(new char[] { ' ' }).Length;
                if (TempCount > Count)
                    Count = TempCount;
            }
            return Count;
        }

        /// <summary>
        /// 获取表头列数
        /// </summary>
        /// <param name="newHeaders">表头文字</param>
        /// <returns></returns>
        private static int GetColCount(string newHeaders)
        {
            string[] ColumnNames = newHeaders.Split(new char[] { '@' });
            int Count = 0;
            if (ColumnNames.Length <= 1)
                ColumnNames = newHeaders.Split(new char[] { '#' });
            Count = ColumnNames.Length;
            foreach (string name in ColumnNames)
            {
                int TempCount = name.Split(new char[] { ',' }).Length;
                if (TempCount > 1)
                    Count += TempCount - 1;
            }
            return Count;
        }

        /// <summary>
        /// 列头跨列数
        /// </summary>
        /// <remarks>
        /// author:zhujt
        /// create date:2015-9-9 09:17:34
        /// </remarks>
        /// <param name="newHeaders">表头文字</param>
        /// <returns></returns>
        private static int GetColSpan(string newHeaders)
        {
            return newHeaders.Split(',').Count();
        }

        /// <summary>
        /// 列头跨行数
        /// </summary> 
        /// <remarks>
        /// author:zhujt
        /// create date:2015-9-9 09:17:14
        /// </remarks>
        /// <param name="newHeaders">列头文本</param>
        /// <param name="rows">表头总行数</param>
        /// <returns></returns>
        private static int GetRowSpan(string newHeaders, int rows)
        {
            int Count = newHeaders.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
            // 如果总行数与当前表头所拥有行数相等
            if (rows == Count)
                Count = 1;
            else if (Count < rows)
                Count = 1 + (rows - Count);
            else
                throw new Exception("表头格式不正确！");
            return Count;
        }
        #endregion

        #region 单元格样式
        private static ICellStyle tbStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //水平居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行
                style.FillForegroundColor = IndexedColors.Yellow.Index;
                style.FillBackgroundColor = IndexedColors.Yellow.Index;
                style.FillPattern = FillPattern.LessDots;
                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                style.SetFont(font);

                return style;
            }
        }
        /// <summary>
        /// 数据单元格样式
        /// </summary>
        private static ICellStyle bodyStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //水平居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行
                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                style.SetFont(font);

                return style;
            }
        }

        private static ICellStyle bodyRedStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //水平居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行
                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                font.Color = IndexedColors.Red.Index;
                style.SetFont(font);

                return style;
            }
        }

        private static IFont rowRedColorStyle
        {
            get
            {
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                font.Color = IndexedColors.Red.Index;
                return font;
            }

        }

        public static bool isRed(DataRow row)
        {
            var flag = false;
            if (row != null && row["Status"] != null && row["Status"].ToString() == "1")
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 数据单元格样式
        /// </summary>
        private static ICellStyle bodyRightStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行

                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "黑体";
                style.SetFont(font);

                return style;
            }
        }

        private static ICellStyle bodyRedRightStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行

                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "黑体";
                font.Color = IndexedColors.Red.Index;
                style.SetFont(font);

                return style;
            }
        }

        /// <summary>
        /// 标题单元格样式
        /// </summary>
        private static ICellStyle titleStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行 

                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 14;
                font.FontName = "宋体";
                font.Boldweight = (short)FontBoldWeight.Bold;
                style.SetFont(font);

                return style;
            }
        }


        /// <summary>
        /// 日期单元格样式
        /// </summary>
        private static ICellStyle dateStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行
                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                style.SetFont(font);

                IDataFormat format = _workbook.CreateDataFormat();
                style.DataFormat = format.GetFormat("yyyy-MM-dd");
                return style;
            }
        }

        private static ICellStyle dateRedStyle
        {
            get
            {
                ICellStyle style = _workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center; //居中
                style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
                style.WrapText = true;//自动换行
                // 边框
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                // 字体
                IFont font = _workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "宋体";
                font.Color = IndexedColors.Red.Index;
                style.SetFont(font);

                IDataFormat format = _workbook.CreateDataFormat();
                style.DataFormat = format.GetFormat("yyyy-MM-dd");
                return style;
            }
        }

        #endregion

        /// <summary>
        /// 实体类
        /// </summary>
        public class NPOIModel
        {
            /// <summary>
            /// 数据源
            /// </summary>
            public DataTable dataSource { get; private set; }
            /// <summary>
            /// 要导出的数据列数组
            /// </summary>
            public string[] fileds { get; private set; }
            /// <summary>
            /// 工作薄名称数组
            /// </summary>
            public string sheetName { get; private set; }
            /// <summary>
            /// 表标题
            /// </summary>
            public string tableTitle { get; private set; }
            /// <summary>
            /// 表标题是否存在 1：存在 0：不存在
            /// </summary>
            public int isTitle { get; private set; }
            /// <summary>
            /// 是否添加序号
            /// </summary>
            public int isOrderby { get; private set; }
            /// <summary>
            /// 表头
            /// </summary>
            public string headerName { get; private set; }
            /// <summary>
            /// 取得列宽
            /// </summary>
            public int[] colWidths { get; private set; }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <remarks>
            /// author:zhujt
            /// create date:2015-9-10 11:17:54
            /// </remarks>
            /// <param name="dataSource">数据来源 DataTable</param>
            /// <param name="filed">要导出的字段，如果为空或NULL，则默认全部</param> 
            /// <param name="sheetName">工作薄名称</param>
            /// <param name="headerName">表头名称 如果为空或NULL，则默认数据列字段
            /// 相邻父列头之间用'#'分隔,父列头与子列头用空格(' ')分隔,相邻子列头用逗号分隔(',')
            /// 两行：序号#分公司#组别#本日成功签约单数 预警,续约,流失,合计#累计成功签约单数 预警,续约,流失,合计#任务数#完成比例#排名 
            /// 三行：等级#级别#上期结存 件数,重量,比例#本期调入 收购调入 件数,重量,比例#本期发出 车间投料 件数,重量,比例#本期发出 产品外销百分比 件数,重量,比例#平均值 
            /// 三行时请注意：列头要重复
            /// </param>
            /// <param name="tableTitle">表标题</param> 
            /// <param name="isOrderby">是否添加序号 0：不添加 1：添加</param>
            public NPOIModel(DataTable dataSource, string filed, string sheetName, string headerName, string tableTitle = null, int isOrderby = 0)
            {
                if (!string.IsNullOrEmpty(filed))
                {
                    this.fileds = filed.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    // 移除多余数据列
                    for (int i = dataSource.Columns.Count - 1; i >= 0; i--)
                    {
                        DataColumn dc = dataSource.Columns[i];
                        if (!this.fileds.Contains(dataSource.Columns[i].Caption.ToUpper()))
                        {
                            dataSource.Columns.Remove(dataSource.Columns[i]);
                        }
                    }

                    // 列索引
                    int colIndex = 0;
                    // 循环排序
                    for (int i = 0; i < dataSource.Columns.Count; i++)
                    {
                        // 获取索引
                        colIndex = GetColIndex(dataSource.Columns[i].Caption.ToUpper());
                        // 设置下标
                        dataSource.Columns[i].SetOrdinal(colIndex);
                    }
                }
                else
                {
                    this.fileds = new string[dataSource.Columns.Count];
                    for (int i = 0; i < dataSource.Columns.Count; i++)
                    {
                        this.fileds[i] = dataSource.Columns[i].ColumnName;
                    }
                }
                this.dataSource = dataSource;

                if (!string.IsNullOrEmpty(sheetName))
                    this.sheetName = sheetName;

                if (!string.IsNullOrEmpty(headerName))
                    this.headerName = headerName;
                else
                    this.headerName = string.Join("#", this.fileds);

                if (!string.IsNullOrEmpty(tableTitle))
                {
                    this.tableTitle = tableTitle;
                    this.isTitle = 1;
                }

                // 取得数据列宽 数据列宽可以和表头列宽比较，采取最长宽度  
                colWidths = new int[this.dataSource.Columns.Count];
                foreach (DataColumn item in this.dataSource.Columns)
                    colWidths[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                // 循环比较最大宽度
                for (int i = 0; i < this.dataSource.Rows.Count; i++)
                {
                    for (int j = 0; j < this.dataSource.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(this.dataSource.Rows[i][j].ToString()).Length;
                        if (intTemp > colWidths[j])
                            colWidths[j] = intTemp;
                    }
                }

                if (isOrderby > 0)
                {
                    this.isOrderby = isOrderby;
                    this.headerName = "序号#" + this.headerName;
                }

            }

            /// <summary>
            /// 获取列名下标
            /// </summary>
            /// <param name="colName">列名称</param>
            /// <returns></returns>
            private int GetColIndex(string colName)
            {
                for (int i = 0; i < this.fileds.Length; i++)
                {
                    if (colName == this.fileds[i])
                        return i;
                }
                return 0;
            }
        }

        /// <summary>
        /// 表头构建类
        /// </summary>
        public class NPOIHeader
        {
            /// <summary>
            /// 表头
            /// </summary>
            public string headerName { get; set; }
            /// <summary>
            /// 起始行
            /// </summary>
            public int firstRow { get; set; }
            /// <summary>
            /// 结束行
            /// </summary>
            public int lastRow { get; set; }
            /// <summary>
            /// 起始列
            /// </summary>
            public int firstCol { get; set; }
            /// <summary>
            /// 结束列
            /// </summary>
            public int lastCol { get; set; }
            /// <summary>
            /// 是否跨行
            /// </summary>
            public int isRowSpan { get; private set; }
            /// <summary>
            /// 是否跨列
            /// </summary>
            public int isColSpan { get; private set; }
            /// <summary>
            /// 外加行
            /// </summary>
            public int rows { get; set; }

            public NPOIHeader() { }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="headerName">表头</param>
            /// <param name="firstRow">起始行</param>
            /// <param name="lastRow">结束行</param>
            /// <param name="firstCol">起始列</param>
            /// <param name="lastCol">结束列</param>
            /// <param name="rows">外加行</param>
            /// <param name="cols">外加列</param>
            public NPOIHeader(string headerName, int firstRow, int lastRow, int firstCol, int lastCol, int rows = 0)
            {
                this.headerName = headerName;
                this.firstRow = firstRow;
                this.lastRow = lastRow;
                this.firstCol = firstCol;
                this.lastCol = lastCol;
                // 是否跨行判断
                if (firstRow != lastRow)
                    isRowSpan = 1;
                if (firstCol != lastCol)
                    isColSpan = 1;

                this.rows = rows;
            }

        }


    }
}
