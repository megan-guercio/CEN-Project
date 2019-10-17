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
using System.Text;

namespace CEN_Project
{
    public partial class MakePost : System.Web.UI.Page
    {
        private FirestoreDb db;
        private CollectionReference chatRef;
        private FirebaseAdmin.Auth.UserRecord user;

        protected void Page_Load(object sender, EventArgsArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");

        }

        protected void confirmPost(object sender, EventArgs e)
        {

        }
    }
}