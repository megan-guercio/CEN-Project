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
using Google.Cloud.Firestore;
using System.Diagnostics;

namespace CEN_Project
{
    public partial class SiteMaster : MasterPage
    {
        private FirebaseApp defaultApp;

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
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (Session["curUser"] != null) return;
            lbProfile.Style["display"] = "inline-block";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Server.MapPath("~") + @"Scripts\cen-project-d757f-firebase-adminsdk-k6z2f-53b4c90b47.json");

            if (defaultApp == null)
            {
                defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault()
                });
            }

            var auth = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(defaultApp);
            Session["curUser"] = null;
            try { Session["curUser"] = auth.GetUserByEmailAsync(UserName.Text).Result; }
            catch { }

            Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>console.log('" + Session["curUser"] + "');</script>");

            if (Session["curUser"] == null) return;

            //Check if user is in database
            var x = (FirebaseAdmin.Auth.UserRecord)Session["curUser"];
            FirestoreDb db = FirestoreDb.Create("cen-project-d757f");

            CollectionReference usersRef = db.Collection("users");
            
            DocumentReference docref = db.Collection("users").Document(x.Uid);
            DocumentSnapshot snapshot = docref.GetSnapshotAsync().Result;
            //User already exists in database. Login complete
            if (snapshot.Exists) return;
            //Add user to database
            Dictionary<string, string> user = new Dictionary<string, string>
            {
                {"userName", x.Email.Substring(0, x.Email.IndexOf('@')) }
            };

            _ = docref.SetAsync(user).Result;
        }
    }
}