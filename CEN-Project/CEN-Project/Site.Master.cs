using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Firebase;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Apis;
using Firebase.Database;

namespace CEN_Project
{
    public partial class SiteMaster : MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //better way of doing this?
            if (Session["curUser"] != null)
            {
                lbProfile.Style["display"] = "inline-block";
                return;
            }
            else if (Request.Url.LocalPath.ToLower().Contains("default") || Request.Url.LocalPath.ToLower().Contains("about")) return;
            else if (Page.Request.Form.ToString().Contains("btnLogin=Login")) return;
            else if (Page.Request.Form.ToString().Contains("btnRegister=Register")) return;
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");
        }
        protected void CreateNewUser(object sender, EventArgs e)
        {
            lbProfile.Style["display"] = "inline-block";
        }

        protected void LogIn(object sender, EventArgs e)
        {
            lbProfile.Style["display"] = "inline-block";
            System.Threading.Tasks.Task.Run(async () => await VerifyLogIn());
        }

        protected async System.Threading.Tasks.Task VerifyLogIn()
        {
            lbProfile.Style["display"] = "inline-block";
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Server.MapPath("~") + @"\Scripts\cen-project-d757f-firebase-adminsdk-k6z2f-3a1bdfe3c4.json")
            });
            var auth = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(defaultApp);
            Session["curUser"] = await auth.GetUserByEmailAsync(UserName.Text);
        }
    }
}