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
                s.AppendLine("<div id=\"thread" + i.ToString() + "\" runat=\"server\" class=\"box\">");
                s.AppendLine("<div style=\"height:100px;width:500px;bottom:0;position:absolute;background:linear-gradient(to bottom, transparent 0%, white 90%);\"></div><h4 style=\"color: darkslategray;\">" + snapshot.GetValue<string>("title") + "</h4><p style=\"color:lightslategrey; text-overflow:clip\">");
                s.AppendLine("Posted by user: " + snapshot.GetValue<string>("postedBy"));
                s.AppendLine(" at: " + new DateTime(1970, 1, 1).AddMilliseconds(snapshot.GetValue<double>("milliseconds")).ToString());
                s.AppendLine("<br/><br/>");
                s.AppendLine(snapshot.GetValue<string>("message"));
                s.AppendLine("</p></div>");
                s.AppendLine("<br/><br/>");

                i++;
            }

            threadList.InnerHtml = s.ToString();
        }
    }
}