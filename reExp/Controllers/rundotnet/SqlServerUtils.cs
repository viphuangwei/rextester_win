using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace reExp.Controllers.rundotnet
{
    public class SqlServerUtils
    {
        public void DoShrinkJob()
        {
            try
            {
                string sql = @"select sum(size/128) from [rextester].[sys].[database_files]";
                using (SqlConnection conn = new SqlConnection(GlobalUtils.TopSecret.SqlServerCS))
                using (SqlCommand command = new SqlCommand(sql))
                {
                    conn.Open();
                    command.Connection = conn;
                    int sizeInMb = Convert.ToInt32(command.ExecuteScalar());

                    if (sizeInMb > 100)
                    {
                        sql = @"DBCC SHRINKDATABASE(N'rextester' )";
                        using (SqlCommand command2 = new SqlCommand(sql))
                        {
                            command2.Connection = conn;
                            command2.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                reExp.Utils.Log.LogInfo("Error while shrinking db. " + e.Message, "RunSqlServer");
            }
        }
    }
}