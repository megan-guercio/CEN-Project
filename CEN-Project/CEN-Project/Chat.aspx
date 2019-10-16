<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Chat.aspx.cs" Inherits="CEN_Project.Chat" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container" class="container body-content">
    
    <br />
    <input id="btnStartThread" type="button" style="float:left; margin-right:50px" class="btn btn-primary" value="Start Thread" onclick="startThreadPopUp()" />
        <div id="threadList" style="float: left;" runat="server">
            <div id="threadIsolated" runat="server" style="display: none;margin-bottom:20px" class="thread">
                <img id="imgReply" onclick="startReply()" src="Images/reply-arrow.png" style="position: absolute; bottom: 10px; right: 10px; height: 20px;">
                <h4 style="color: darkslategray;" runat="server" id="h4Thread"><a id="aThread" runat="server"></a></h4>
                <p id="pMessage" runat="server" style="color: lightslategray; position:relative"></p>
                <textarea id="replyBox" name="replyBox" rows="2" cols="20" class="reply" style="display:none" runat="server"></textarea>
                <asp:Button runat="server" ID="btnPostReply" Text="Post Reply" OnClick="PostReply" class="btn btn-primary" Style="display: none;"></asp:Button></div>
            <div id="oldReplies" runat="server"></div>
        </div>
    <br />
    
    <div id="emptyThread" class="popup" style="padding: 0px 15px 15px 15px;">
        <div class="center">
                <h2>Let's start a thread!</h2>
                <br />
                <b>Title </b><br />
                <asp:TextBox CssClass="form-control" runat="server" ID="txtTitle" style="display: inline-block;"></asp:TextBox>
                <br />
                <b>Message </b><br />
                <asp:TextBox CssClass="form-control" runat="server" id="txtMessage" TextMode="MultiLine" style="display: inline-block; width:500px; height:250px"></asp:TextBox>
                <br />
                <asp:Button ID="btnPostThread" OnClick="btnPostThread_Click" OnClientClick="if(startThreadHide()) return true;" runat="server" style="margin-bottom: 20px" CssClass="btn btn-primary" Text="Post Thread"/>
            </div>
        </div>
    </div>
     <script>
         function startThreadPopUp() {
             $("#emptyThread").center();
             $("#emptyThread").fadeIn();
             $("#versionMask").fadeIn();
         }
         function startThreadHide() {
             $("#emptyThread").fadeOut();
             $("#versionMask").fadeOut();
             return true;
         }

         function startReply() {
             document.getElementById('imgReply').style.display = "none";
             document.getElementById('MainContent_replyBox').style.display = "block";
             document.getElementById('MainContent_btnPostReply').style.display = "block";
         }
         </script>
    
</asp:Content>