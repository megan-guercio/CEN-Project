using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEN_Project
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginPopup()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "LoginBox", "<script type='text/javascript'>login()</script>");
            return;
        }

        protected void GoHome(object sender, EventArgs e)
        {

        }
        
        protected void GoAbout(object sender, EventArgs e)
        {
            LoginPopup();
        }

        protected void GoLivestream(object sender, EventArgs e)
        {
            LoginPopup();
        }

        protected void GoMarketplace(object sender, EventArgs e)
        {
            LoginPopup();
        }

        protected void GoTournaments(object sender, EventArgs e)
        {
            LoginPopup();
        }

        protected void GoProfile(object sender, EventArgs e)
        {
            LoginPopup();
        }

        protected void GoChat(object sender, EventArgs e)
        {
            LoginPopup();
        }
    }
}