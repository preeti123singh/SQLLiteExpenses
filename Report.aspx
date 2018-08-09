<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" MasterPageFile="~/Home.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="heads" Runat="Server">
    <!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        #form1{
           font-family:"Bahnschrift SemiBold" ;
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
        //Maintain Scroll bar position
        function setScroll(val) {
          
            document.getElementById("heads_scrollPos").value = val.scrollTop;
        }
        function scrollTo(what) {
           
            if (what != "0")

                document.getElementById(what).scrollTop = document.getElementById("heads_scrollPos").value;

        }
        //End Scrollbar position
    </script>
</head>
<body style="padding:0px;">
    <form id="form1" runat="server" >
        <input id="scrollPos" runat="server" type="hidden" value="0" />
        <div>
            <table class="auto-style1">
                <tr>
                    <td>
                        <asp:Label ID="lbl_monthly" runat="server" Text="Monthly"></asp:Label>:
                        <asp:DropDownList ID="drp_month" runat="server" OnSelectedIndexChanged="drp_month_SelectedIndexChanged" AutoPostBack="true">  
                        <asp:ListItem Text="--Select--" Value="00" />
                        <asp:ListItem Text="January" Value="01" />
                        <asp:ListItem Text="Febraury" Value="02" />
                        <asp:ListItem Text="March" Value="03" />
                        <asp:ListItem Text="April" Value="04" />
                        <asp:ListItem Text="May" Value="05" />
                        <asp:ListItem Text="June" Value="06" />
                        <asp:ListItem Text="July" Value="07" />
                        <asp:ListItem Text="August" Value="08" />
                        <asp:ListItem Text="September" Value="09" />
                        <asp:ListItem Text="October" Value="10" />
                        <asp:ListItem Text="November" Value="11" />
                        <asp:ListItem Text="December" Value="12" />
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_yearly" runat="server" Text="Yearly:"></asp:Label>
                        <asp:DropDownList ID="drp_yearly" runat="server" OnSelectedIndexChanged="drp_yearly_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="--Select--" Value="0000" />
                            <asp:ListItem Text="2010" Value="01" />
                        <asp:ListItem Text="2011" Value="02" />
                        <asp:ListItem Text="2012" Value="03" />
                        <asp:ListItem Text="2013" Value="04" />
                        <asp:ListItem Text="2014" Value="05" />
                        <asp:ListItem Text="2015" Value="06" />
                        <asp:ListItem Text="2016" Value="07" />
                        <asp:ListItem Text="2017" Value="08" />
                        <asp:ListItem Text="2018" Value="09" />
                        <asp:ListItem Text="2019" Value="10" />
                        <asp:ListItem Text="2020" Value="11" />
                        <asp:ListItem Text="2021" Value="12" />

                        </asp:DropDownList>
                    </td>
                     
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            
                        Start Date:
                        <asp:TextBox ID="txt_StartDate" runat="server" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter Start Date" ControlToValidate="txt_StartDate" ValidationGroup="Require"  Display="None" ></asp:RequiredFieldValidator> 
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1"  runat="server" TargetControlID="txt_StartDate" Format="dd/MM/yyyy"/>
                            End Date:
                         <asp:TextBox ID="txt_Enddate" runat="server" ></asp:TextBox>
                         <asp:Button ID="btn_find" runat="server" OnClick="btn_find_Click" Text="Find" ValidationGroup="Require" />
                            <asp:Button ID="btn_Excel" runat="server" Text="Excel Export" OnClick="btn_Excel_Click"  />
                             <asp:Button ID="btn_Pdf" runat="server" Text="Pdf Export" OnClick="btn_Pdf_Click"  />
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter End Date" ControlToValidate="txt_Enddate" ValidationGroup="Require"  Display="None"></asp:RequiredFieldValidator> 
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender2"  runat="server" TargetControlID="txt_Enddate" Format="dd/MM/yyyy" />
                         <asp:CompareValidator ID="cmpDates" ControlToValidate="txt_StartDate" ControlToCompare="txt_Enddate" Operator="LessThanEqual" Type="Date"  ValidationGroup="Require" ErrorMessage="Start date must be less than End date." runat="server"  Display="None"/>                         
                        </asp:Panel>    
                    </td>  
                   </tr> 
                <tr>    
                    <td>

                    </td>
                    <td>

                    </td>
                    <td>
                        <asp:ValidationSummary id="valSum" 
                             DisplayMode="BulletList"
                             EnableClientScript="true"
                             HeaderText="You must enter a value in the following fields:"
                             runat="server" ShowSummary="true" showmessagebox="false" ValidationGroup="Require" ForeColor="#FF3300" Height="50px"/>

                    </td>

                </tr>
            </table>


        </div>
       
        <div style="height:50px;">

            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="link" NavigateUrl="~/Expense.aspx">Go to First Page</asp:HyperLink>
        </div>
    <div style="margin-top:100px;overflow:auto;height:500px"  id="autoScroll" runat="server" onscroll="javaascript:setScroll(this);">
       
    
        <asp:GridView ID="data_grid" runat="server" DataKeyNames="id" AutoGenerateColumns="False"   Width="100%"   ShowHeaderWhenEmpty="True" OnRowCancelingEdit="data_grid_RowCancelingEdit" OnRowDeleting="data_grid_RowDeleting" OnRowEditing="data_grid_RowEditing" OnRowUpdating="data_grid_RowUpdating" CellPadding="4" ForeColor="#333333" GridLines="None" >
            <AlternatingRowStyle BackColor="White" height="20px" ForeColor="#284775" />
            
            <Columns>
                <asp:CommandField ShowEditButton="true" ButtonType="Link" ItemStyle-Width="20px"/>  
                        <asp:CommandField ShowDeleteButton="true" ButtonType="Link" ItemStyle-Width="20px"/>
            
                <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-HorizontalAlign="Center"  >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>   
                
                     <asp:BoundField DataField="Money" HeaderText="Money"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Payment" HeaderText="Payment"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description"  ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-HorizontalAlign="Center"> 
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                 </asp:BoundField>
             <%--<asp:BoundField DataField="Image" HeaderText="Image" ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                <asp:TemplateField HeaderText="Image"  ItemStyle-Height = "50px" ItemStyle-Width = "70px">  
                <ItemTemplate>  
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%#  Eval("Image") %>' Height="50px" Width="70px" />  
                </ItemTemplate>  
                <EditItemTemplate>  
                    <asp:Image ID="img_user" runat="server" ImageUrl='<%#  Eval("Image")%>' Height="80px" Width="70px" />  
                    <br />  
                    <asp:FileUpload ID="FileUpload1" runat="server" />  
                </EditItemTemplate>  


            </asp:TemplateField> 
             </Columns>
             
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" Height="30px" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Height="30px"/>
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="30px" HorizontalAlign="Left"/>
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"  />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        
    </div>
        <%--<div style="width: 1000px; margin: 0 auto; ">
               <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" ImageAlign="right"  Visible="false"/>
        </div>--%>
    </form>
</body>
</html>
</asp:Content>