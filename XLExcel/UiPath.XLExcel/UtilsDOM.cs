using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UiPath.XLExcel
{
    public static partial class Utils
    {
        public static DataTable ReadDOMRange(string fileName, string sheetName)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                string relId = workbookPart.Workbook.Descendants<Sheet>().First(s => sheetName.Equals(s.Name)).Id;

                //open reader for Sheet
                WorksheetPart worksheetPart = workbookPart.GetPartById(relId) as WorksheetPart;
                Worksheet workSheet = worksheetPart.Worksheet;

                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                //get shared string array
                SharedStringItem[] sharedStringItemsArray = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray<SharedStringItem>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(sharedStringItemsArray,spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        dataRow[i] = GetCellValue(sharedStringItemsArray, spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        private static string GetCellValue(SharedStringItem[] sharedStringItemsArray, SpreadsheetDocument document, Cell cell)
        {
           
            

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string index = cell.CellValue.InnerXml;
                return sharedStringItemsArray[Int32.Parse(index)].InnerText;
            }
            else if (cell.CellValue != null)
            {
                string value = cell.CellValue.InnerXml;
                return value;
            }

            return null;
        }
    }
}
