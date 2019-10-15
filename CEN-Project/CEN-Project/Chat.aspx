<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Chat.aspx.cs" Inherits="CEN_Project.Chat" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container" class="container body-content">

    
    <br />
    <input id="btnStartThread" type="button" style="float:left; margin-right:50px" class="btn btn-primary" value="Start Thread" onclick="startThreadPopUp()" />
    <div id="threadList" style="float:left;" runat="server">
        <asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
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
             document.getElementById('pMessage').style.marginBottom = "215px";
             document.getElementById('replyBox').style.display = "block";
             document.getElementById('btnPostReply').style.display = "block";
         }
         </script>
    
</asp:Content>