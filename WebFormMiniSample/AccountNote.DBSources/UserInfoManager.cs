using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AccountNote.DBSources
{
    public class UserInfoManager
    {
        public static void InsertIntoUser(string userName, string pwd)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = @" INSERT INTO UserInfo(Name,PWD) VALUES(@name,@pwd)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(dbCommandString, connection))
                {
                    command.Parameters.AddWithValue("@name", userName);
                    command.Parameters.AddWithValue("@pwd", pwd);
                    try
                    {
                        connection.Open();
                        int effectRows = command.ExecuteNonQuery();
                        Console.WriteLine($"{effectRows} has changed.  ");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
        public static DataRow GetUserInfoByAccount(string account)
        {
            string connectionString = DBHelper.GetConnectionString(); ;
            string dbCommandString =
                @"SELECT ID,Account,Name,PWD,Email FROM UserInfo WHERE Account=@account";

            //使用USING包覆程式碼可以省略 connectinon.Close()，且可以在程式運行完時，馬上將記憶體清空回收至CLR。

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@account", account));
            
            try
                    {
                        return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }     
}
