<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
   body {font-family: Arial, Helvetica, sans-serif;}

/* Full-width input fields */
.txtbox{
    width: 100%;
    padding: 12px 20px;
    margin: 8px 0;
    display: inline-block;
    border: 1px solid #ccc;
    box-sizing: border-box;
}

/* Set a style for all buttons */
.button {
    background-color: #4CAF50;
    color: white;
    padding: 14px 20px;
    margin: 8px 0;
    border: none;
    cursor: pointer;
    width: 100%;
}

button:hover {
    opacity: 0.8;
}

/* Center the image and position the close button */
.imgcontainer {
    text-align: center;
    margin: 24px 0 12px 0;
    position: relative;
}

img.avatar {
    width: 90%;
    /*border-radius: 100%;*/
    
}

.container {
    padding: 16px;
}

/*span.psw {
    float: right;
    padding-top: 16px;
}*/




/* Modal Content/Box */
.modal-content {
    background-color: #fefefe;
    margin: 5% auto 15% auto; /* 5% from the top, 15% from the bottom and centered */
    border: 1px solid #888;
    width: 20%; /* Could be more or less, depending on screen size */
}
    </style>
</head>
<body>
    
    <form id="form1" runat="server" class="modal-content animate" >
    <div class="imgcontainer">
        <img src="Pics/10bits.bmp" alt="Avatar" class="avatar" />
    </div>

    <div class="container">
      <label for="uname"><b>Username</b></label>
    <%--  <input type="text" placeholder="Enter Username" name="uname" required runat="server">--%>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="txtbox"></asp:TextBox>
      <label for="psw"><b>Password</b></label>
     <%-- <input type="password" placeholder="Enter Password" name="psw" required runat="server">--%>
        
     <asp:TextBox ID="TextBox2" runat="server" CssClass="txtbox"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Login"  CssClass="button" OnClick="Button1_Click"/>
    
          
     
    </div>
    </form>
       
</body>
</html>

