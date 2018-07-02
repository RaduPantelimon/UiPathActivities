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
    public static class ExtensionClasses
    {

        public static void AddColumns(this DataTable dt, int count)
        {
            for (int i = 0; i < count; i++)
                dt.Columns.Add();
        }
        public static void AddColumns(this DataTable dt, object[] columns)
        {
            foreach (object header in columns)
            {
                dt.Columns.Add(header != null ? header.ToString() : "");
            }
        }

        public static void AddRows(this DataTable dt, List<object[]> rows)
        {
            foreach (object[] row in rows)
            {
                dt.Rows.Add(row);
            }
        }
        public static void AddRows(this DataTable dt, List<List<object>> rows)
        {
            foreach (List<object> row in rows)
            {
                dt.Rows.Add(row.ToArray());
            }
        }
        public static void AddRows(this DataTable dt, LinkedList<object[]> rows, int maxColNo)
        {
            while(rows.Count> 0)
            {
                object[] currentRow = rows.First.Value;
                if (maxColNo > currentRow.Length)
                {
                    List<object> currentRowAsList = (currentRow.ToList());
                    currentRowAsList.AddRange(
                        Enumerable.Repeat<object>(null, maxColNo - currentRow.Length));

                    currentRow = currentRowAsList.ToArray();
                }
                
                dt.Rows.Add(currentRow);
                rows.RemoveFirst();
            }
        }

        public static int GetCellColumn(this Cell cell)
        {
            Regex rgx = new Regex("[A-Z]+");
            MatchCollection colMatches = rgx.Matches(cell.CellReference);
            int stCol;

            //coordinates must begin with a valid [A-Z expression]
            if (colMatches[0].Index == 0)
            {
                //getting col number
                string colString = colMatches[0].Value;
                stCol = ExcelRange.ExcelColumnNameToNumber(colString);

                return stCol;
            }
            else
            {

                throw new Exception("The range received is not valid. Failed to parse expression: " + cell.CellReference);
            }

        }

        public static string Dump(this DataTable table)
        {
            string data = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (null != table && null != table.Rows)
            {
                for (int i=0;i<table.Rows.Count;i++)
                {
                    DataRow dataRow = table.Rows[i];
                    /*foreach (var item in dataRow.ItemArray)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }*/
                    sb.Append(String.Join(",", dataRow.ItemArray));
                    if(i+1 <table.Rows.Count)sb.AppendLine();
                }
                data = sb.ToString();
            }
            return data;
        }

        public static string DumpColumns(this DataTable table)
        {
          
            string Columns = String.Join(";", table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray());
            return Columns;
        }
    }
}
