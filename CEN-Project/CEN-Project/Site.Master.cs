using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CEN_Project
{
    public partial class SiteMaster : MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //better way of doing this?
            if (Request.Url.LocalPath == "/default.aspx") return;
            else if (Page.Request.Form.ToString().Contains("btnLogin=Login")) return;
            else if (Page.Request.Form.ToString().Contains("btnRegister=Register")) return;
            else if (Convert.ToBoolean(Session["loggedIn"])) return;

            Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>login()</script>");
        }

        protected void CreateNewUser(object sender, EventArgs e)
        {
            Session["loggedIn"] = true;
        }

        protected void LogIn(object sender, EventArgs e)
        {
            Session["loggedIn"] = true;
        }
    }
}