using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RVBot
{
    public static class StringHelpers
    {
        public static string objToString(object o)
        {
            if (o is TimeSpan)
            {
                var ts = (TimeSpan)o;
                //return $"{ts.Hours.ToString("D2")}:{ts.Minutes.ToString("D2")}:{ts.Seconds.ToString("D2")}";
                var s = $"{ts.Minutes:D2}:{ts.Seconds:D2}";
                if (ts.Hours > 0)
                {
                    s = $"{ts.Hours:D2}:{s}";
                }
                return s;
            }
            else
            {
                return (o ?? string.Empty).ToString();
            }
        }

        public static string PrintNiceList(IEnumerable<string> list)
        {
            if (!list.Any())
            {
                return "";
            }
            if (list.Count() == 1)
            {
                return list.First();
            }
            var res = $"{string.Join(", ", list.Reverse<string>().Skip(1).Reverse())} and {list.Last()}";

            return res;
        }


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
}
