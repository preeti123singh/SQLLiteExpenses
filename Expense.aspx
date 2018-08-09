<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Expense.aspx.cs" Inherits="Expense" MasterPageFile="~/Home.master"  EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="heads" Runat="Server">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


    <!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
  
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <link href="css/theme.css" rel="stylesheet" type="text/css" />
    <link href="css/triangle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <style type="text/css">
       
        #img{
            vertical-align: middle;
        }
     
        .RowStyle {
            height: 150px;
        }
        .txtbox
        {
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
            
        }
        .btnbox{
             border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
            float:right;
              margin-top:20px;
        }
    .auto-style1 {
        height: 29px;
    }
    #form1{
        font-family:Bahnschrift SemiBold;
        font-size:20px;
        /*color:#0e5566;*/ 
    }
    
    input[type='text']{
        margin:7px;
      
    }
   .txtboxgap{
       margin-top:20px;
       border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
   }
   .fileupl{
       margin-top:20px;
       border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
   }
    </style>
       
     <script type="text/javascript">

        //window.onload = function () {
        //    SetHeight();
           
        //};
        //function SetHeight() {
            
        //var x =  screen.height + "px";
        //document.getElementById("form1").style.height = x;
        //}
       
    </script>

</head>
<body style="padding:0px;">
    <form id="form1" runat="server" class="tdstyle">
            
        <div class="tdstyle">

            <table style="margin:0 auto;width:50%" >
                <tr>
                    <td >
                       
                      <asp:Label ID="lbl_date" runat="server" Text="Date" Font-Names="Bahnschrift SemiBold" ></asp:Label>
                       
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" AutoCompleteType="Disabled" MaxLength="50" CssClass="txtbox"></asp:TextBox>
                        <asp:Image ID="Img" runat="server" ImageUrl="~/Pics/Calendar.png"  Width="22px" CssClass="txtbox" />
                        
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter Date" ControlToValidate="Textbox1" ValidationGroup="Require" ForeColor="#FF3300"></asp:RequiredFieldValidator> 
                    </td>
                   <td><ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server"  TargetControlID="TextBox1" Format="dd/MM/yyyy"/></td>
                  
                                      
                </tr>
                <tr>
                    <td >
                        Amount
                       <%-- <asp:Label ID="lbl_Money" runat="server" Text="Amount" Font-Names="Bahnschrift SemiBold" ></asp:Label>--%>
                    </td>
                    <td>
                      <asp:TextBox ID="txt_Money" runat="server" AutoCompleteType="Disabled" MaxLength="50" CssClass="txtbox" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter Amount" ControlToValidate="txt_Money" ValidationGroup="Require" ForeColor="#FF3300"></asp:RequiredFieldValidator> 
                          <asp:RegularExpressionValidator id="RegularExpressionValidator1"
                   ControlToValidate="txt_Money" ForeColor="#FF3300"
                   ValidationExpression="\d+" ValidationGroup="Require"
                   Display="Static"
                   EnableClientScript="true"
                   ErrorMessage="Please enter numbers only"
                   runat="server"/>  
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        Payment
                        <%--<asp:Label ID="lbl_pym" runat="server" Text="Payment" Font-Names="Bahnschrift SemiBold" ></asp:Label>--%>
                    </td>
                    <td class="auto-style1" >
                        
                        <asp:DropDownList ID="dropdownlist" runat="server" CssClass="txtbox" Font-Names="Bahnschrift SemiBold">
                            
                        </asp:DropDownList>                  
                         <asp:Button ID="btn_payment" runat="server" Text="Add Payment" Height="22px" Width="95px"  OnClick="btn_payment_Click" CssClass="txtbox" Font-Names="Bahnschrift SemiBold" />
                        <asp:Button ID="btnHidden" runat="Server" style="display:none" OnClick="btnHidden_Click" />
                       
                    </td>
                </tr>
                <tr>
                    <td >
                        Description
                      <%--  <asp:Label ID="lbl_des" runat="server" Text="Description" Font-Names="Bahnschrift SemiBold"></asp:Label>--%>
                    </td>
                    <td >
                        <asp:TextBox ID="txt_Description" runat="server" Height="100px" TextMode="MultiLine" AutoCompleteType="Disabled" CssClass="txtboxgap" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter Description" ControlToValidate="txt_Description" ValidationGroup="Require" ForeColor="#FF3300"></asp:RequiredFieldValidator> 
                    </td>
                </tr>
                <tr>
                    <td >
                        Comments
                       <%-- <asp:Label ID="lbl_Comment" runat="server" Text="Comments" Font-Names="Bahnschrift SemiBold"></asp:Label>--%>
                    </td>
                    <td >
                        <asp:TextBox ID="txt_Comment" runat="server" Height="100px" TextMode="MultiLine" AutoCompleteType="Disabled" CssClass="txtbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:sub;">Receipt(Jpg/Png)
                        <%--<asp:Label ID="lbl_Image" runat="server" Text="Receipt(Jpg/Png)" Font-Names="Bahnschrift SemiBold"  ></asp:Label>--%>
                    </td>
                    <td>  
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="fileupl" Font-Names="Bahnschrift SemiBold" accept=".png,.jpg,.jpeg,.gif"/>
                                <asp:Button ID="btn_upload" runat="server" Text="Show Image" OnClick="btn_upload_Click" CssClass="btnbox" Font-Names="Bahnschrift SemiBold" />
                                <asp:Label ID="lbl_image" runat="server" Text="" ></asp:Label> 
                                <asp:Image ID="Image1" runat="server" Height="150px" Width="200px" ImageAlign="Middle" />
                                
                                 <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .jpg,.jpeg,gif,.png images are allowed." ValidationExpression="/^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpeg|.JPEG|.gif|.GIF| .png|.PNG)$/" ControlToValidate="FileUpload1"></asp:RegularExpressionValidator>--%>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btn_upload" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
           

        </div>

        <div style="margin:0 auto;width:300px;">
             <table >
               <%-- <tr>

                <td></td>

                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>--%>
                <tr>
                    <td>
                        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" CssClass="txtbox" Font-Names="Bahnschrift SemiBold" ValidationGroup="Require"/>
                    </td>
                    <td>
                        <asp:Button ID="btn_reports" runat="server" OnClick="btn_reports_Click" Text="Reports" CssClass="txtbox" Font-Names="Bahnschrift SemiBold"/>
                    </td>
                     
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
</asp:Content>