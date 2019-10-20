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
    public partial class Marketplace : System.Web.UI.Page
    {
        private FirestoreDb db;
        private CollectionReference chatRef;
        private FirebaseAdmin.Auth.UserRecord user;
        private Guid curThread;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Please don't remove
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["curUser"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "LoggedIn", "<script type='text/javascript'>loginPopUp()</script>");

        }

        protected void startMakePostPopUp(object sender, EventArgs e)
        {

        }

        private void sellerInbox(object sender, EventArgs e)//sellers and buyers have "private messaging" inbox to the left of the screen
        {

        }

        private void interestedBtnClicked(object sender, EventArgs e)      //buyer marks "interested" on a post, by putting a checkmark in the box
        {

        }

        private void sellerStats(object sender, EventArgs e)  //seller can view items for sale and "who's interested" option
        {

        }

        private void whosInterested(object sender, EventArgs e)      //seller selects "Who's Interested?" and a list of interested buyers pops up
        {

        }




    }
}
