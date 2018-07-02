using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Security;
using System.Reflection;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace UiPath.XLExcel
{
    public class ExcelRange
    {
        public string rangeAsString;
        public int StartRow;
        public int EndRow;
        public int StartColumn;
        public int EndColumn;

        public int ColumnCount { get { return EndColumn - StartColumn + 1; } }
        public int RowCount { get { return EndRow - StartRow + 1; } }

        public ExcelRange(string rangeEntry)
        {
            if (String.IsNullOrEmpty(rangeEntry))
            {
                EndRow = -1; EndColumn = -1;
                StartRow = 1; StartColumn = 1;
            }
            else if (rangeEntry.Contains(":"))
            {
                string[] cellArray = rangeEntry.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                //we need exactly 2 cell coordinates for the expression to be valid
                if (cellArray.Length != 2)
                    throw new Exception("The range received is not valid: " + rangeEntry);

                //calculate both end and start coordinates
                ExcelColumnNameToCoordinates(cellArray[0], ref StartRow, ref StartColumn);
                ExcelColumnNameToCoordinates(cellArray[1], ref EndRow, ref EndColumn);


                //if end coordinates are smaller than start coords, we have an invalid range
                if (EndRow < StartRow || EndColumn < StartColumn)
                {
                    throw new Exception("The range received is not valid: " + rangeEntry);
                }
            }
            else
            {
                ExcelColumnNameToCoordinates(rangeEntry, ref StartRow, ref StartColumn);
                EndRow = -1; EndColumn = -1;
            }

        }

        public static void ExcelColumnNameToCoordinates(string cellCoords, ref int stRow, ref int stCol)
        {
            Regex rgx = new Regex("[A-Z]+");
            MatchCollection colMatches = rgx.Matches(cellCoords);

            //coordinates must begin with a valid [A-Z expression]
            if (colMatches[0].Index == 0)
            {
                //getting col number
                string colString = colMatches[0].Value;
                stCol = ExcelColumnNameToNumber(colString);

                string rowString = rgx.Replace(cellCoords, "", 1);
                if (rowString.IndexOf("0") == 0) throw new Exception("Row number cannot begin with 0: " + rowString);
                stRow = int.Parse(rowString);
            }
            else
            {

                throw new Exception("The range received is not valid. Failed to parse expression: " + cellCoords);
            }
            string result = rgx.Replace(cellCoords, "", 1);
        }

        //convert excel column name to number
        public static int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }
    }
}
