﻿using System;
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
        private Guid curThread;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["tid"] != null)
                Guid.TryParse(Request.QueryString["tid"], out curThread);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");
            GetThreads(curThread);
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

            curThread = new Guid();
        }

        private void GetThread(Guid curThread)
        {
            if (Session["curUser"] == null) return;
            if (db == null) db = FirestoreDb.Create("cen-project-d757f");
            if (chatRef == null) chatRef = db.Collection("chatThreads");
            DocumentReference docref = db.Collection("chatThreads").Document(curThread.ToString());
            DocumentSnapshot snapshot = docref.GetSnapshotAsync().Result;

            aThread.HRef = "Chat.aspx?tid=" + curThread.ToString();
            h4Thread.InnerText = snapshot.GetValue<string>("title");
            pMessage.InnerText = snapshot.GetValue<string>("message");

            StringBuilder s = new StringBuilder();
            s.AppendLine("Posted by user: " + snapshot.GetValue<string>("postedBy"));
            s.AppendLine(" at: " + new DateTime(1970, 1, 1).AddMilliseconds(snapshot.GetValue<double>("milliseconds")).ToString());
            s.AppendLine("<br/><br/>");
            s.AppendLine(snapshot.GetValue<string>("message"));
            s.AppendLine("<br/><br/>");

            pMessage.InnerHtml = s.ToString();

            CollectionReference colref = db.Collection("chatThreads").Document(curThread.ToString()).Collection("replies");

            var x = colref.OrderBy("milliseconds");
            QuerySnapshot qs = x.GetSnapshotAsync().Result;
            s = new StringBuilder();
            int i = 1;
            foreach (DocumentSnapshot sp in qs.Documents)
            {
                s.AppendLine("<div id=\"reply" + i.ToString() + "\" runat=\"server\" class=\"oldreply\">");
                s.AppendLine("<p style=\"color:lightslategrey;\">");
                s.AppendLine("Posted by user: " + sp.GetValue<string>("postedBy"));
                s.AppendLine(" at: " + new DateTime(1970, 1, 1).AddMilliseconds(sp.GetValue<double>("milliseconds")).ToString());
                s.AppendLine("<br/><br/>");
                s.AppendLine(sp.GetValue<string>("message"));
                s.AppendLine("</p></div>");
                s.AppendLine("<br/>");

                i++;
            }

            oldReplies.InnerHtml = s.ToString();
            threadIsolated.Style["display"] = "block";
        }

        protected void PostReply(object sender, EventArgs e)
        {
            if (Session["curUser"] == null || curThread == null) return;
            if (db == null) db = FirestoreDb.Create("cen-project-d757f");
            if (chatRef == null) chatRef = db.Collection("chatThreads");

            //Check if user is in database
            user = (FirebaseAdmin.Auth.UserRecord)Session["curUser"];
            db = FirestoreDb.Create("cen-project-d757f");

            chatRef = db.Collection("chatThreads");

            DocumentReference docref = db.Collection("chatThreads").Document(curThread.ToString()).Collection("replies").Document(Guid.NewGuid().ToString());
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;

            double milliseconds = (now - unixEpoch).TotalMilliseconds;

            //Add thread to database
            Dictionary<string, object> newReply = new Dictionary<string, object>
            {
                {"postedBy", user.Email.Substring(0, user.Email.IndexOf('@')) },
                {"message", replyBox.InnerText },
                {"milliseconds", milliseconds }
            };
            _ = docref.SetAsync(newReply).Result;
        }

        public void GetThreads(Guid curThread)
        {
            if (Session["curUser"] == null) return;
            db = FirestoreDb.Create("cen-project-d757f");
            chatRef = db.Collection("chatThreads");

            if (curThread != new Guid())
            {
                GetThread(curThread);
                return;
            }

            var x = chatRef.OrderByDescending("milliseconds");
            QuerySnapshot qs = x.GetSnapshotAsync().Result;
            StringBuilder s = new StringBuilder();
            int i = 1;
            foreach(DocumentSnapshot snapshot in qs.Documents)
            {
                s.AppendLine("<div id=\"thread" + i.ToString() + "\" runat=\"server\" class=\"box\">");
                s.AppendLine("<div style=\"height:100px;width:500px;bottom:0;position:absolute;background:linear-gradient(to bottom, transparent 0%, white 90%);\"></div><h4 style=\"color: darkslategray;\"><a href=\"Chat.aspx?tid=" + snapshot.Id + "\">"+ snapshot.GetValue<string>("title") + "</a></h4><p style=\"color:lightslategrey; text-overflow:clip\">");
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