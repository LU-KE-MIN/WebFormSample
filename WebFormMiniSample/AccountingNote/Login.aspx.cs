using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountNote.DBSources;

namespace AccountingNote
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserLoginInfo"] != null)
            {
                this.plcLogin.Visible = false;
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
            else
            {
                this.plcLogin.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string db_Account = string.Empty;
            string db_Password = string.Empty;

            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            //check empty
            if(string.IsNullOrWhiteSpace(inp_Account) || string.IsNullOrWhiteSpace(inp_PWD))
            {
                this.ltlMsg.Text = "Account/PWD is required.";
                return;
            }

            var dr = UserInfoManager.GetUserInfoByAccount(inp_Account);

            if (dr == null)       // 如果帳號不存在，導至登入頁
            {
                this.Session["UserLoginInfo"] = null;
                Response.Redirect("/Login.aspx");
                return;
            }
            db_Account = dr["Account"].ToString();
            db_Password = dr["PWD"].ToString();
            // check account /pwd
            //密碼需區分大小寫
            if (string.Compare(db_Account,inp_Account,true)==0 && string.Compare(db_Password, inp_PWD )==0)
            {
                this.Session["UserLoginInfo"] = dr["Account"].ToString();
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
        }
    }
}