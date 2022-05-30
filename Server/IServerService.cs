using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server
{
    public interface IServerService
    {
        void InsertMassiveData(List<AccessLog> list);

        void ConvertData(byte[] bytes);
    }

    public class ServerService : IServerService
    {
        public void ConvertData(byte[] bytes)
        {
            var data = System.Text.Encoding.ASCII.GetString(bytes);
            var dataList = data.Split(new char[]{'\n'});
            string pattern = "^([\\d.]+) (\\S+) (\\S+) \\[([\\w:/]+\\s[+\\-]\\d{4})\\] \"(.+?)\" (\\d{3}) (\\d+) \"([^\"]+)\" \"([^\"]+)\"";
            Regex rgx = new Regex(pattern, RegexOptions.Compiled);
            List<AccessLog> list = new List<AccessLog>();
            string dateString, format;
            CultureInfo provider = CultureInfo.InvariantCulture;
            int resultHttpResult, sizeRequest;
            DateTime dt;
            for (int i = 0; i < dataList.Count(); i++)
            {
                Match match = rgx.Match(dataList[i]);

                dateString = match.Groups[4].Value;
                format = "dd/MMM/yyyy:HH:mm:ss zzzz";
                bool result = DateTime.TryParseExact(dateString, format, provider, DateTimeStyles.None, out dt);

                bool isParsable = Int32.TryParse(match.Groups[6].Value, out resultHttpResult);

                bool isParsable2 = Int32.TryParse(match.Groups[7].Value, out sizeRequest);

                list.Add(new AccessLog() { Id = 0, IpOrigin = match.Groups[1].Value, Url = match.Groups[5].Value, HttpResult = resultHttpResult, Date = dt, SizeRequest = sizeRequest, User = null, IpDestination = null });

            }

            InsertMassiveData(list);

        }
        public void InsertMassiveData(List<AccessLog> list)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("IpOrigem", typeof(string));
            table.Columns.Add("Url", typeof(string));
            table.Columns.Add("HttpResult", typeof(int));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("SizeRequest", typeof(int));
            table.Columns.Add("User", typeof(string));
            table.Columns.Add("IpDestination", typeof(string));


            for (int i = 0; i < list.Count(); i++)
            {
                table.Rows.Add(new object[]
                {
                    list[i].Id,
                    list[i].IpOrigin,
                    list[i].Url,
                    list[i].HttpResult,
                    list[i].Date,
                    list[i].SizeRequest,
                    list[i].User,
                    list[i].IpDestination,
                });

            }
            using (var conn = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=TCP;Trusted_Connection=True;"))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                    {
                        try
                        {
                            bulkCopy.DestinationTableName = "[dbo].[access_log3]";
                            bulkCopy.WriteToServer(table);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            conn.Close();
                        }
                    }
                }
            }

        }

    }


}
