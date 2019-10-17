<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Marketplace.aspx.cs" Inherits="CEN_Project.Marketplace" %>

  
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container" class="container body-content">
        <br />
        <h1 style="vertical-align:top; position: fixed absolute"> MarketPlace </h1>
        <h3 style="vertical-align:top; position:fixed relative; left:50%"> Have Something You Want to Sell? </h3>
        <input id="btnMakePost" type="button" style="vertical-align:top; position:fixed relative; left: 50%"  class="btn btn-primary" value="Make Post" onclick="startMakePostPopUp()" />
        <br />
        
     </div>


    <script>
        function startMakePostPopUp() {
          
        }
    </script>

</asp:Content>