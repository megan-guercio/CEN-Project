<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Marketplace.aspx.cs" Inherits="CEN_Project.Marketplace" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="container" class="container body-content">
       
        <h1 style="vertical-align:top; position: fixed absolute"> Make Your Post </h1>
        
        <h4> What Are You Trying to Sell? </h4>

        <! take in the string of the item the user is trying to sell>
        
        <h4> How Much Are You Trying to Sell it For? </h4>
        
        <! take in the int of the item the user is trying to sell>
        
        <h4> Add a Picture of the Item Here </h4>
        
        <! take in the picture the user uploads >
        
        <! add a way for the user to browse their computer and select an image >

        <input id="btnConfirmPost" type="button" style="vertical-align:bottom; position:fixed relative; left: 50%"  class="btn btn-primary" value="Post" onclick="confirmPost()" />



    </div>




    <script>
        function confirmPost() {

            <! upon confirmPost send all necessary components to admin to be checked >
            <! 
        }


    </script>




</asp:Content>