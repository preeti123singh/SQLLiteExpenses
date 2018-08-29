<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Display.aspx.cs" Inherits="Display" MasterPageFile="~/Home.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="heads" Runat="Server">
    <!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <link href="css/theme.css" rel="stylesheet" type="text/css" />
    <link href="css/triangle.css" rel="stylesheet" type="text/css" />
    <script src="pikaday.js" type="text/javascript"></script>
     <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <title></title>
    <style type="text/css">
      
        .link{
            text-decoration: underline; color: green; 
        }
    </style>

    <script type="text/javascript">

        window.onload = function () {
            SetHeight();
        };
        function SetHeight() {
            
        var x =  screen.height + "px";
        document.getElementById("form1").style.height = x;
        }

    </script>
</head>
<body style="padding:0px;">
    <form id="form1" runat="server">
  
        <div style="margin-top:100px;" >
<asp:Label ID="lbl_image" runat="server" Text=""></asp:Label><asp:Label ID="lbl_vat" runat="server" Text=""></asp:Label>
           
            <asp:GridView ID="GridView1" runat="server" Width="100%"   CellPadding="4" DataKeyNames="ID" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting" ShowHeaderWhenEmpty="True" OnRowEditing="GridView1_RowEditing" OnRowUpdated="GridView1_RowUpdated" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"  >
                <AlternatingRowStyle BackColor="White" height="20px" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowEditButton="true" ButtonType="Image"  EditImageUrl="~/Pics/NewEdit.png"  UpdateImageUrl="~/Pics/Update.png" CancelImageUrl="~/Pics/NewCancel.png" ItemStyle-Wrap="false"/>  
                        <asp:CommandField ShowDeleteButton="true" ButtonType="Image"  DeleteImageUrl="~/Pics/NewDelete.jpg"  ItemStyle-Wrap="false"/>
                     
                    <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>   
                
                     <asp:BoundField DataField="AmountPaid" HeaderText="AmountPaid" SortExpression="ESACCode" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                     
                    <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="WaiverCode" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="RefundType" ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-HorizontalAlign="Center"> 
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Image" HeaderStyle-Width="200px">  
                <ItemTemplate>  
                   <%-- <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="80px" Width="80px" />--%>
                    <iframe id="DisplayExpense" runat="server" src='<%# Eval("Image")%>' width="200px" height="150px"></iframe>   
                </ItemTemplate>  
                <EditItemTemplate>  
                     <iframe id="DisplayEditExpense" runat="server" src='<%# Eval("Image")%>' width="200px" height="150px"></iframe>
                          <%--<asp:Image ID="Image_EditVat" runat="server" ImageUrl='<%# Eval("VatReceipt") %>' Height="0px" Width="0px" />--%>
                    <asp:Image ID="img_user" runat="server" ImageUrl='<%# Eval("Image") %>' Height="0px" Width="0px" />  
                    <br />  
                    <asp:FileUpload ID="FileUpload1" runat="server" />  
                </EditItemTemplate>  
                <HeaderStyle Width="200px"></HeaderStyle>
                 </asp:TemplateField> 
                    <asp:BoundField DataField="Item" HeaderText="Item" ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                     <asp:BoundField DataField="VATAmount" HeaderText="VAT Amount" SortExpression="ESACCode" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                      <asp:TemplateField HeaderText="VATReceipt" HeaderStyle-Width="200px">  
                        <ItemTemplate>
                      
                             
                            <iframe id="ifDisplayPdf" runat="server" src='<%# Eval("VatReceipt")%>' width="200px" height="150px"></iframe> 
                        </ItemTemplate>  
                        <EditItemTemplate>  
                          
                            <iframe id="ifDisplayEditPdf" runat="server" src='<%# Eval("VatReceipt")%>' width="200px" height="150px"></iframe>
                          <asp:Image ID="Image_EditVat" runat="server" ImageUrl='<%# Eval("VatReceipt") %>' Height="0px" Width="0px" /> 
                            <br />  
                        <asp:FileUpload ID="FileUpload2" runat="server" />  
                        </EditItemTemplate>  
                        <HeaderStyle Width="200px"></HeaderStyle>
                       </asp:TemplateField>
                </Columns>
                 <EmptyDataTemplate>Record Deleted</EmptyDataTemplate>
              
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
       
        <div style="width: 1000px; margin: 0 auto;">
        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="link" NavigateUrl="~/Expense.aspx">Go to First Page</asp:HyperLink></div>

    
        <br />
    </form>
</body>
</html>
</asp:Content>