using AccountNote.DBSources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccountingNote.Auth
{
    /// <summary>負責處理登入的元件 </summary>
    public class AuthManager
    {
        /// <summary> 檢查目前是否登入 </summary>
        /// <returns></returns>
        public static bool IsLogined()
        {
            if (HttpContext.Current.Session["UserLoginInfo"] == null)
                return false;
            else
                return true;
        }

        public static UserInfoModel GetCurrentUser()
        {
            string account = HttpContext.Current.Session["UserLoginInfo"] as string;

            if (account == null)
                return null;

            DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                HttpContext.Current.Session["UserLoginInfo"] = null;
                return null;
            }
               
            
            UserInfoModel model = new UserInfoModel();
            model.ID = dr["ID"].ToString();
            model.Account = dr["Account"].ToString();
            model.Name = dr["Name"].ToString();
            model.Email = dr["Email"].ToString();

            return model;
        }
        /// <summary>登出 </summary>
        public static void Logout()
        {
            HttpContext.Current.Session["UserLoginInfo"] = null;
        }
        public static bool TryLogin(string account,string pwd,out string errorMsg)
        {
            //check empty
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                errorMsg = "Account/PWD is required.";
                return false;
            }

            //read db and check
            var dr = UserInfoManager.GetUserInfoByAccount(account);

            //check null
            if (dr == null)       // 如果帳號不存在，導至登入頁
            {
                errorMsg = $"Account:{account} doesn't exists.";
                return false;
            }

            // check account /pwd
            //密碼需區分大小寫
            if (string.Compare(dr["Account"].ToString(), account,true) == 0 && string.Compare(dr["PWD"].ToString(), pwd,false) == 0)
            {
                HttpContext.Current.Session["UserLoginInfo"] = dr["Account"].ToString();

                errorMsg = string.Empty;
                return true;
            }
            else
            {
                errorMsg = "Login fail. Please check Account / PWD.";
                return false;
            }
        }
    }
}
