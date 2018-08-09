<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payment.aspx.cs" Inherits="Payment"  %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
       .txtbox
        {
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
        }
    </style>

  
</head>
<body style="background-color:#E9E9E9;">
    
    <form id="form1" runat="server"  >
    <div style="width:400px; margin:0 auto;">
    
        <table class="auto-style1">
            <tr>
                <td><asp:Label ID="lbl_payment" runat="server" Text="Payment" Font-Names="Bahnschrift SemiBold"></asp:Label></td>
                <td><asp:TextBox ID="txt_payment" runat="server" AutoCompleteType="disabled" CssClass="txtbox" height="25px"></asp:TextBox></td>
                <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter Payment methods." ControlToValidate="txt_payment" ValidationGroup="Require" ForeColor="#FF3300"></asp:RequiredFieldValidator> </td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btn_name" runat="server" Text="Add" OnClick="btn_name_Click"  CssClass="txtbox" Font-Names="Bahnschrift SemiBold" ValidationGroup="Require"/></td>
            </tr>
        </table>
    
    </div>
    </form>
        
</body>
</html>
   