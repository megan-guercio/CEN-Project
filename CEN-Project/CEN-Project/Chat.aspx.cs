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
        private Guid curThread;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");

            //OnClick for btnPostReply
            

            if (Request.QueryString["tid"] != null)
                Guid.TryParse(Request.QueryString["tid"], out curThread);

            if (Page.Request.Form.ToString().Contains("btnPostReply=Post+Reply")) PostReply(null, EventArgs.Empty);
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
            GetThreads(new Guid());
        }

        private void GetThread(Guid curThread)
        {
            if (Session["curUser"] == null) return;
            if (db == null) db = FirestoreDb.Create("cen-project-d757f");
            if (chatRef == null) chatRef = db.Collection("chatThreads");
            DocumentReference docref = db.Collection("chatThreads").Document(curThread.ToString());
            DocumentSnapshot snapshot = docref.GetSnapshotAsync().Result;

            System.Web.UI.HtmlControls.HtmlGenericControl divThread = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            divThread.ID = "thread1";
            divThread.Attributes["runat"] = "server";
            divThread.Attributes["class"] = "thread";

            System.Web.UI.HtmlControls.HtmlGenericControl imgThread = new System.Web.UI.HtmlControls.HtmlGenericControl("img");
            imgThread.ID = "imgReply";
            imgThread.Attributes["onclick"] = "startReply()";
            imgThread.Attributes["src"] = "Images/reply-arrow.png";
            imgThread.Attributes["style"] = "position:absolute;bottom:10px;right:10px;height:20px;";

            divThread.Controls.Add(imgThread);

            System.Web.UI.HtmlControls.HtmlGenericControl h4Thread = new System.Web.UI.HtmlControls.HtmlGenericControl("h4");
            h4Thread.Attributes["style"] = "color: darkslategray;";

            System.Web.UI.HtmlControls.HtmlGenericControl aThread = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
            aThread.Attributes["href"] = "Chat.aspx?tid=" + snapshot.Id;
            aThread.InnerText = snapshot.GetValue<string>("title");

            h4Thread.Controls.Add(aThread);

            divThread.Controls.Add(h4Thread);

            System.Web.UI.HtmlControls.HtmlGenericControl pThread = new System.Web.UI.HtmlControls.HtmlGenericControl("p");
            pThread.ID = "pMessage";
            pThread.Attributes["style"] = "color: lightslategray";
            pThread.InnerHtml = "Posted by user: " + snapshot.GetValue<string>("postedBy") + " at: " + 
                new DateTime(1970, 1, 1).AddMilliseconds(snapshot.GetValue<double>("milliseconds")).ToString() + 
                "<br/><br/>" + @snapshot.GetValue<string>("message") + "<br/><br/>";

            divThread.Controls.Add(pThread);

            System.Web.UI.HtmlControls.HtmlGenericControl textAreaThread = new System.Web.UI.HtmlControls.HtmlGenericControl("textarea");
            textAreaThread.ID = "replyBox";
            textAreaThread.Attributes["name"] = "replyBox";
            textAreaThread.Attributes["rows"] = "2";
            textAreaThread.Attributes["cols"] = "20";
            textAreaThread.Attributes["class"] = "reply";
            textAreaThread.Attributes["runat"] = "server";

            divThread.Controls.Add(textAreaThread);

            var button = new Button
            {
                ID = "btnPostReply",
                Text = "Post Reply"
            };

            button.Attributes["class"] = "btn btn-primary";
            button.Attributes["style"] = "display:none";
            button.Attributes["runat"] = "server";
            button.Command += PostReply;
            button.CausesValidation = false;

            divThread.Controls.Add(button);

            PlaceHolder1.Controls.Add(divThread);
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
            var ct = Page.FindControl("ctl00");
            var form = ct.Controls[3];
            //var placeholder = form.FindControl("MainContent");
            //var list = placeholder.FindControl("threadList");
            //var placeholder1 = list.FindControl("PlaceHolder1");
            //var thread = placeholder1.FindControl("thread1");
            //var box = thread.FindControl("replyBox");

            //Add thread to database
            Dictionary<string, object> newThread = new Dictionary<string, object>
            {
                {"postedBy", user.Email.Substring(0, user.Email.IndexOf('@')) },
                {"message", "" },
                {"milliseconds", milliseconds }
            };
            _ = docref.SetAsync(newThread).Result;
            GetThreads(new Guid());

        }

        protected void GetReplies()
        {
            CollectionReference colref = db.Collection("chatThreads").Document(curThread.ToString()).Collection("replies");
            var x = colref.OrderByDescending("milliseconds");
            QuerySnapshot qs = x.GetSnapshotAsync().Result;
            StringBuilder s = new StringBuilder();
            foreach (DocumentSnapshot snapshot in qs.Documents)
            {
                s.AppendLine(snapshot.Id);
                s.AppendLine(snapshot.GetValue<string>("postedBy"));
                s.AppendLine(new DateTime(1970, 1, 1).AddMilliseconds(snapshot.GetValue<double>("milliseconds")).ToString());
                s.AppendLine(snapshot.GetValue<string>("message"));
            }
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