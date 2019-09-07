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

        }

        protected void CreateNewUser(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>sessionStorage.setItem('status', 'loggedin');</script>");
        }

        protected void LogIn(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>sessionStorage.setItem('status', 'loggedin');</script>");
        }
    }
}