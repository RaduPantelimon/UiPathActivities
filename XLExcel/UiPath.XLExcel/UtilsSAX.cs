using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Security;
using System.Reflection;
using System.Activities;


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace UiPath.XLExcel
{
    public static partial class Utils
    {
        

        public static XLExcelContextInfo GetXLExcelContextInfo(NativeActivityContext context)
        {
            var property = context.DataContext.GetProperties()[XLExcelContextInfo.XLExcelContextInfoTag];
            var ctx = property.GetValue(context.DataContext) as XLExcelContextInfo;

            return ctx;
        }
        public static XLExcelContextInfo GetXLExcelContextInfo(CodeActivityContext context)
        {
            var property = context.DataContext.GetProperties()[XLExcelContextInfo.XLExcelContextInfoTag];
            var ctx = property.GetValue(context.DataContext) as XLExcelContextInfo;

            return ctx;
        }

        public static DataTable ReadSAXRange(
          ExcelRange range,
          string filePath,
          string sheetName,
          bool AddHeaders)
         {
            DataTable result= new DataTable();

            LinkedList<object[]> rows = new LinkedList<object[]>();
            int previousRow = range.StartRow - 1, maxColNo;

            //if we know exactly the range we will read (we know the start and end columns) 
            //we do not have to save the rows in the linked list and we can add them directly to the output
            bool limitedRange = (range.EndRow != -1 && range.EndColumn != -1) ? true : false;


            //open file
            using (SpreadsheetDocument myDoc = SpreadsheetDocument.Open(filePath, true))
            {

                WorkbookPart workbookPart = myDoc.WorkbookPart;
                //determine ID of the Sheet
                string relId = workbookPart.Workbook.Descendants<Sheet>().First(s => sheetName.Equals(s.Name)).Id;

                //open reader for Sheet
                WorksheetPart worksheetPart = workbookPart.GetPartById(relId) as WorksheetPart;
                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);

                //get shared string array
                SharedStringItem[] sharedStringItemsArray = GetSharedStringItemsArray(workbookPart);


                //keep track of current expected row
                int prevRow = range.StartRow - 1;

                while (reader.Read())
                {
                    //we found a row
                    if (reader.ElementType == typeof(Row))
                    {

                        //current row number
                        //get XML attr
                        OpenXmlAttribute attr = reader.Attributes.FirstOrDefault(a => a.LocalName == "r");

                        if (attr != null && attr.Value != null)
                        {
                            string rowNum = attr.Value;

                            int currentRow = int.Parse(rowNum);

                            ///make sure row index is correct
                            if (currentRow < 1 || currentRow > 1048576) throw new Exception("Cannot process rows whose index number is below 1 or above 1048576");

                            //add any missing rows to the table
                            FillMissingRows(prevRow, currentRow, range, rows, limitedRange, result, AddHeaders);



                            if (currentRow > range.EndRow && range.EndRow != -1)
                            {
                                //we're out of the current range, we simply break
                                break;
                            }
                            else if (currentRow >= range.StartRow && (currentRow <= range.EndRow || range.EndRow == -1))
                            {
                                //add the current row to the table
                                List<object> row = GetSAXRowArray(reader, workbookPart, range, sharedStringItemsArray);
                                if (!limitedRange)
                                {
                                    rows.AddLast(row.ToArray());
                                }
                                else
                                {
                                    AddFullRowToDT(row.ToArray(), AddHeaders, result);
                                }


                                //Console.WriteLine(String.Join(";", (row.ToList()).Select(x => x != null ? x.ToString() : "") ));
                                prevRow = currentRow;
                            }


                        }
                    }
                }

                //add any missing rows to the table
                FillMissingRows(prevRow, range.EndRow + 1, range, rows, limitedRange, result, AddHeaders);

                

                if (!limitedRange)
                {
                    maxColNo = rows.Max(x => x.Length);
                    Utils.AddColumnsToDT(range, AddHeaders, result, rows, maxColNo);
                    //POPULATING THE RESULT
                    result.AddRows(rows, maxColNo);
                }
            }

            return result;
            
         }


        private static void AddFullRowToDT(object[] row, bool AddHeaders, DataTable dt)
        {
            //if we are supposed to add headers but no columns added yet
            if (AddHeaders && dt.Columns.Count == 0)
            {
                dt.AddColumns(row);
                return;
            } else if (dt.Columns.Count == 0)
            {
                dt.AddColumns(row.Length);
            }

            //add the row as normal
            dt.Rows.Add(row);
        }

        //adding the columns to the table
        private static void AddColumnsToDT(ExcelRange range, bool AddHeaders, DataTable result, LinkedList<object[]> rows, int maxColNo)
        {
            //adding column to the table
            if (AddHeaders)
            {
                object[] headers = rows.First();

                result.AddColumns(headers);

                //add more columns in case the header is smaller than the maximum sized row
                result.AddColumns(maxColNo - headers.Length);
                rows.RemoveFirst();
            }
            else
            {

                //simply adding the correct number of empty column rows
                if (range.EndRow == -1)
                {
                    result.AddColumns(maxColNo);
                }
                else
                {
                    result.AddColumns(range.ColumnCount);
                }
            }
        }

        private static void FillMissingRows(int prevRow, int currentRow, ExcelRange range, LinkedList<object[]> rows, bool limitedRange =false, DataTable dt = null, bool AddHeaders = false)
        {
            //add any missing rows to the table
            while (prevRow < currentRow - 1)
            {
                prevRow++;
                if (limitedRange)
                {
                    AddFullRowToDT(Enumerable.Repeat<object>(null,range.ColumnCount).ToArray(), AddHeaders, dt);
                }
                else
                {
                    rows.AddLast(Enumerable.Repeat<object>(null, (range.EndColumn != -1 ? range.ColumnCount : 1)).ToArray());
                }
                
            }
        }


        public static List<object> GetSAXRowArray(OpenXmlReader reader, WorkbookPart workbookPart, ExcelRange range, SharedStringItem[] ssArray)
        {
            List<object> results = new List<object>();
            reader.ReadFirstChild();


            int previousCellNo = range.StartColumn - 1;
            do
            {
                try
                {
                    if (reader.ElementType == typeof(Cell))
                    {

                        Cell c = (Cell)reader.LoadCurrentElement();

                        //get current column
                        int currentCol = c.GetCellColumn();
                        if (currentCol> range.EndColumn && range.EndColumn != -1)
                        {
                            //we're out of the current range, we simply break
                            break;
                        }
                        else if (currentCol >= range.StartColumn && ( currentCol <= range.EndColumn || range.EndColumn == -1))
                        {

                            //fill in gaps
                            while (previousCellNo < currentCol-1)
                            {
                                results.Add(null);
                                previousCellNo++;
                            }

                            //get the cell value
                            string cellValue;
                            if (c.DataType != null && c.DataType == CellValues.SharedString && ssArray!= null)
                            {
                                SharedStringItem ssi = ssArray[int.Parse(c.CellValue.InnerText)];

                                cellValue = ssi.Text.Text;
                            }
                            else if (c.CellValue != null)
                            {
                                cellValue = c.CellValue.InnerText;
                            }
                            else
                            {
                                cellValue = null;
                            }


                            //check if the value can be parsed to remove extra decimals
                            double parsedValue;
                            if(Double.TryParse(cellValue, out parsedValue))
                            {
                                cellValue = parsedValue.ToString();
                            }

                            //add received cell Value
                            results.Add(cellValue);
                            previousCellNo = currentCol;

                        }

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }



              
                


            } while (reader.ReadNextSibling());
            
            //fill in the last gaps
            if (range.EndRow != -1)
            {
                while (previousCellNo < range.EndColumn)
                {
                    results.Add(null);
                    previousCellNo++;
                }
            }

            //return resulted Rows
            return results;
        }

        
       

        public static SharedStringItem[] GetSharedStringItemsArray( WorkbookPart workbookPart)
        {
            SharedStringItem[] sharedStringItemsArray;

            try
            {
                sharedStringItemsArray = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray<SharedStringItem>();

            }
            catch (Exception ex)
            {
                sharedStringItemsArray = null;
            }

            return sharedStringItemsArray;
        }

    }

  


    
}
