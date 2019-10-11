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
    public partial class Chat : System.Web.UI.Page
    {
        private FirestoreDb db;
        private CollectionReference chatRef;
        private FirebaseAdmin.Auth.UserRecord user;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");
            GetThreads();
        }

        protected void btnPostThread_Click(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) return;

            //Check if user is in database
            user = (FirebaseAdmin.Auth.UserRecord)Session["curUser"];
            db = FirestoreDb.Create("cen-project-d757f");

            chatRef = db.Collection("chatThreads");

            DocumentReference docref = db.Collection("chatThreads").Document(Guid.NewGuid().ToString());
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;

            double milliseconds = (now - unixEpoch).TotalMilliseconds;
            //Add thread to database
            Dictionary<string, object> newThread = new Dictionary<string, object>
            {
                {"postedBy", user.Email.Substring(0, user.Email.IndexOf('@')) },
                {"message", @txtMessage.Text },
                {"milliseconds", milliseconds },
                {"title", @txtTitle.Text }
            };
            _ = docref.SetAsync(newThread).Result;
            GetThreads();
        }

        public void GetThreads()
        {
            if (Session["curUser"] == null) return;

            user = (FirebaseAdmin.Auth.UserRecord)Session["curUser"];
            db = FirestoreDb.Create("cen-project-d757f");
            chatRef = db.Collection("chatThreads");

            var x = chatRef.OrderByDescending("milliseconds");
            QuerySnapshot qs = x.GetSnapshotAsync().Result;
            StringBuilder s = new StringBuilder();
            int i = 1;
            foreach(DocumentSnapshot snapshot in qs.Documents)
            {
                s.AppendLine("<div id=\"thread" + i.ToString() + "\" runat=\"server\" class=\"box\"><p>");
                s.AppendLine("Thread ID: " + snapshot.Id);
                s.AppendLine("<br/>");
                s.AppendLine("Posted by user: " + snapshot.GetValue<string>("postedBy"));
                s.AppendLine("<br/>");
                s.AppendLine("Posted at: " + new DateTime(1970, 1, 1).AddMilliseconds(snapshot.GetValue<double>("milliseconds")).ToString());
                s.AppendLine("<br/>");
                s.AppendLine("Title of thread: " + snapshot.GetValue<string>("title"));
                s.AppendLine("<br/>");
                s.AppendLine("Initial thread message: " + snapshot.GetValue<string>("message"));
                s.AppendLine("</p></div>");
                s.AppendLine("<br/><br/>");

                i++;
            }

            threadList.InnerHtml = s.ToString();
        }
    }
}