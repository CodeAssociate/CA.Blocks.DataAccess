using System.Data;
using System.Text;

namespace CA.Blocks.DataAccess.DataTableHelpers
{
    public static class DataTableToTextHelper
    {
        public static string OutPutAsAlignedText(DataTable dt)
        {
            var maxLengths = new int[dt.Columns.Count];

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        var length = row[i].ToString().Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }

                sb.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(!row.IsNull(i)
                            ? row[i].ToString().PadRight(maxLengths[i] + 2)
                            : new string(' ', maxLengths[i] + 2));
                    }

                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
