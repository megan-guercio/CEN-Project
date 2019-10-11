using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace CEN_Project
{
    public partial class Marketplace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Please don't remove
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");
        }
    }
}