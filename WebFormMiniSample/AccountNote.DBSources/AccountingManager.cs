using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AccountNote.DBSources
{
    public class AccountingManager
    {
        public static DataTable GetAccountingList(string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT
                         [ID]
                        ,[Caption]
                        ,[Amount]
                       ,[ActType]
                       ,[CreateDate]                     
                       FROM Accounting
                       WHERE UserID = @userID
                     ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userID", userID));

            try
            {
                return DBHelper.ReadDataTable(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        public static void CreateAccouting(string userID, string caption, int amount, int actType, string body)
        {
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");
            //<<<<< check input >>>>>
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@"INSERT INTO [dbo].[Accounting]
                       (
                        UserID
                       ,Caption
                       ,Amount
                       ,ActType
                       ,CreateDate
                       ,Body
                   )
                 VALUES
                       (
                       @userID
                       ,@caption
                       ,@amount
                       ,@actType
                       ,@createDate
                       ,@body
                       ) ";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(dbCommand, connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    command.Parameters.AddWithValue("@caption", caption);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@actType", actType);
                    command.Parameters.AddWithValue("@createDate", DateTime.Now);
                    command.Parameters.AddWithValue("@body", body);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);

                    }
                }
            }

        }
        public static DataRow GetAccounting(int id, string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT 
                        ID,
                        Caption,
                        Amount,
                        ActType,
                        CreateDate,
                        Body
                    FROM Accounting
                    WHERE id = @id AND UserID = @userID";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@id", id));
            list.Add(new SqlParameter("@userID", userID));

            try
            {
                return DBHelper.ReadDataRow(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        /// <summary> 變更流水帳</summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <param name="caption"></param>
        /// <param name="amount"></param>
        /// <param name="actType"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool UpdateAccounting(int id, string userID, string caption, int amount, int actType, string body)
        {
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");
            //<<<<< check input >>>>>
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@"UPDATE [dbo].[Accounting]
                        SET 
                        UserID = @userID
                       ,Caption = @caption
                       ,Amount = @amount
                       ,ActType = @actType
                       ,CreateDate = @crateDate
                       ,Body = @body
                      
                 WHERE
                      ID = @id";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", id));
            paramList.Add(new SqlParameter("@userID", userID));
            paramList.Add(new SqlParameter("@caption", caption));
            paramList.Add(new SqlParameter("@amount", amount));
            paramList.Add(new SqlParameter("@actType", actType));
            paramList.Add(new SqlParameter("@crateDate", DateTime.Now));
            paramList.Add(new SqlParameter("@body", body));

            try
            {
                int effectRows = DBHelper.ModifyDate(connStr, dbCommand, paramList);

                if (effectRows == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        public static void DeleteAccounting(int ID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@"DELETE [dbo].[Accounting]             
                       WHERE ID = @id";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", ID));

            try
            {
                DBHelper.ModifyDate(connStr, dbCommand, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
    }
}
