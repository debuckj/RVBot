using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RVBot
{
    public static class DiscordTable
    {
        public static K[,] TransposeRowsAndColumns<K>(K[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            K[,] transposed = new K[columnCount, rowCount];
            if (rowCount == columnCount)
            {
                transposed = (K[,])arr.Clone();
                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        K temp = transposed[i, j];
                        transposed[i, j] = transposed[j, i];
                        transposed[j, i] = temp;
                    }
                }
            }
            else
            {
                for (int column = 0; column < columnCount; column++)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        transposed[column, row] = arr[row, column];
                    }
                }
            }
            return transposed;
        }

        public static string CreateTextTable(object[,] data)
        {
            var sb = new StringBuilder();


            var colCount = data.GetLength(1);
            var rowCount = data.GetLength(0);

            var maxColLenght = new int[colCount];

            for (int ix = 0; ix < colCount; ix++)
            {
                for (int iy = 0; iy < rowCount; iy++)
                {
                    var value = StringHelpers.objToString(data[iy, ix]);
                    maxColLenght[ix] = Math.Max(maxColLenght[ix], value.Length);
                }
            }

            for (int iy = 0; iy < rowCount; iy++)
            {
                var row = "";
                for (int ix = 0; ix < colCount; ix++)
                {
                    var value = StringHelpers.objToString(data[iy, ix]);
                    var paddedValue = value.PadLeft(maxColLenght[ix]);
                    //sb.Append($"| {paddedValue} ");
                    //sb.Append($"{paddedValue}|");
                    row += $"{paddedValue} | ";
                }
                //sb.AppendLine("|");

                sb.AppendLine(row.Substring(0, Math.Max(0, row.Length - 2)).TrimEnd());
            }



            var result = sb.ToString();
            return result;
        }
    }

    public static class Temp
    {
        public static string ToString<T>(IList<T> rows, IList<string> headers = null, bool flipRowsWithColumns = false)
        {
            var properties = typeof(T).GetProperties();

            if (headers != null && headers.Count != properties.Length)
            {
                throw new Exception("Number of headers must be same as number of properties or null");
            }
            if (headers == null)
            {
                headers = properties.Select(x => x.Name).ToList();
            }

            var data = new string[rows.Count + 1, headers.Count];
            for (int ix = 0; ix < headers.Count; ix++)
            {
                data[0, ix] = headers[ix];
                for (int iy = 0; iy < rows.Count; iy++)
                {
                    var row = rows[iy];
                    var obj = properties[ix].GetValue(row);
                    var val = StringHelpers.objToString(obj);
                    data[iy + 1, ix] = val;
                }
            }

            if (flipRowsWithColumns)
            {
                data = DiscordTable.TransposeRowsAndColumns(data);
            }

            var res = DiscordTable.CreateTextTable(data);
            return res;
        }
    }

    public class DiscordTable<T>
    {
        public IList<string> Headers { get; set; }

        public IList<Func<int, IEnumerable<T>, object>> ColumnValueSelect { get; set; }

        public IEnumerable<T>[] Rows { get; set; }

        public DiscordTable(params IEnumerable<T>[] rows)
        {
            Rows = rows;
        }

        public string TableAsString()
        {
            var data = new string[Rows.Length + 1, ColumnValueSelect.Count];

            for (int ix = 0; ix < ColumnValueSelect.Count; ix++)
            {
                data[0, ix] = Headers[ix];
                for (int iy = 0; iy < Rows.Length; iy++)
                {
                    var sel = ColumnValueSelect[ix];

                    var row = Rows[iy];

                    var obj = sel(iy, row);
                    var val = StringHelpers.objToString(obj);
                    data[iy + 1, ix] = val;
                }
            }

            data = DiscordTable.TransposeRowsAndColumns(data);

            var res = DiscordTable.CreateTextTable(data);
            return res;
        }

    }


}