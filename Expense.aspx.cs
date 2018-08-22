using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Data.SQLite;

public partial class Expense : System.Web.UI.Page
{
    public SQLiteConnection connection;


    protected void Page_Load(object sender, EventArgs e)
    {
        connection = (SQLiteConnection)Session["connection"];

        if (!Page.IsPostBack)
        {
            PopulateDropdown();
            PopulateDropdownitem();
        }
    }

    public void PopulateDropdownitem()
    {
        var value = Session["item"];


        if (value != null)
        {
            string query;
            query = "insert into tbl_item(ItemType) values('" + value + "')";
            connection.Open();
            using (SQLiteCommand sqlcmd = new SQLiteCommand(query, connection))
            {
                sqlcmd.ExecuteNonQuery();
            }
            connection.Close();
        }
        string query1;
        DataSet ds = new DataSet();
        connection.Open();
        query1 = "select distinct(ItemType) from tbl_item";
        SQLiteCommand sqlcmd1 = new SQLiteCommand(query1, connection);
        SQLiteDataAdapter da1 = new SQLiteDataAdapter(sqlcmd1);
        da1.Fill(ds);
        drp_item.DataSource = ds.Tables[0];
        drp_item.DataTextField = "ItemType";
        drp_item.DataBind();
        connection.Close();
    }

    public void PopulateDropdown()
    {
        var value = Session["Value"];


        if (value != null)
        {
            string query;
            query = "insert into tbl_payments (payment) values('" + value + "')";
            connection.Open();
            using (SQLiteCommand sqlcmd = new SQLiteCommand(query, connection))
            {
                sqlcmd.ExecuteNonQuery();
            }

            connection.Close();
        }
        string query1;
        DataSet ds = new DataSet();
        connection.Open();
        query1 = "select distinct(payment) from tbl_payments";
        SQLiteCommand sqlcmd1 = new SQLiteCommand(query1, connection);
        SQLiteDataAdapter da1 = new SQLiteDataAdapter(sqlcmd1);
        da1.Fill(ds);
        dropdownlist.DataSource = ds.Tables[0];
        dropdownlist.DataTextField = "payment";
        dropdownlist.DataBind();
        connection.Close();
    }



    string Filename;
    public string GetPhotos()
    {
        string Images = "";
        //string ext="";
        string ext = System.IO.Path.GetExtension(this.FileUpload1.PostedFile.FileName).ToLower();
        if (ViewState["Filename"] != null)
        {
            string extension = ViewState["Filename"].ToString();
            ext = "." + extension.Substring(extension.LastIndexOf(".") + 1).ToLower();
        }

        
            if (FileUpload1.HasFile)
            {
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".pdf")
            {
                Filename = FileUpload1.FileName.Replace(" ", "");
                FileUpload1.PostedFile.SaveAs(Server.MapPath(("~/Upload/" + Filename)));
                Images = "/Upload/" + Filename.ToString();
                lbl_image.Text = "";
            }
            else
            {
                Images = "/Upload/download.jpg";
                Filename = "download.jpg";
                lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_image.ForeColor = System.Drawing.Color.Red;

            }
            }
            else
            {
                if (ViewState["File"] != null)
                {
                    Images = ViewState["File"].ToString();
                    Filename = ViewState["Filename"].ToString();
                    lbl_image.Text = "";
                }
                else
                {
                    Images = "/Upload/download.jpg";
                    Filename = "download.jpg";
                lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_image.ForeColor = System.Drawing.Color.Red;
            }
            }
        
        //else
        //{
        //    Images = "/Upload/download.jpg";
        //    Filename = "download.jpg";
        //    lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
        //    lbl_image.ForeColor = System.Drawing.Color.Red;

        //}
        return (Images);
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            connection.Open();
            string query = "insert into tbl_expenses(Date,AmountPaid,payment,Description,Comments,Image,Filename,Item,VatAmount,VatReceipt,VatFileName) values('" + Convert.ToDateTime(TextBox1.Text).ToString("yyyy-MM-dd") + "','" + txt_Money.Text + "','" + dropdownlist.SelectedItem.Text + "','" + txt_Description.Text + "','" + txt_Comment.Text + "','" + GetPhotos() + "','" + Filename + "','" + drp_item.SelectedItem.Text + "','" + txt_Amount.Text + "','" + GetVatPhotos() + "','" + VatFileName + "')";
            using (SQLiteCommand sqlcom = new SQLiteCommand(query, connection))
            {
                sqlcom.ExecuteNonQuery();
            }

            connection.Close();
            Response.Redirect("Display.aspx");
        }
        catch (Exception es)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(),
                "MessageBox", "alert('" + es.Message + "');", true);
        }
    }


    protected void btn_reports_Click(object sender, EventArgs e)
    {
        Response.Redirect("~\\Report.aspx");
    }

    protected void btn_upload_Click(object sender, EventArgs e)
    {
        Image1.Attributes.Add("width", "200");
        Image1.Attributes.Add("height", "150");
        ViewState["Filename"] = null;
        Image1.ImageUrl = GetPhotos();
        ViewState["File"] = Image1.ImageUrl;
        int pos = Image1.ImageUrl.LastIndexOf("/") + 1;
        Image1.ImageUrl.Substring(pos, Image1.ImageUrl.Length - pos);
        ViewState["Filename"] = Image1.ImageUrl.Substring(pos, Image1.ImageUrl.Length - pos);

    }


    protected void btnHidden_Click(object sender, EventArgs e)
    {
        PopulateDropdown();
    }



    protected void btn_payment_Click(object sender, EventArgs e)
    {
        string popupScript = "<script language='javascript'>" +
                           "window.open('payment.aspx', 'ThisPopUp', " +
                           "'left = 300, top=150, width=400, height=300, " +
                           "menubar=no, scrollbars=no, resizable=no')" +
                           "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
    }



    protected void btn_item_hidden_Click(object sender, EventArgs e)
    {
        PopulateDropdownitem();
    }

    protected void btn_item_Click(object sender, EventArgs e)
    {
        string popupScript = "<script language='javascript'>" +
                           "window.open('item.aspx', 'ThisPopUp', " +
                           "'left = 300, top=150, width=400, height=300, " +
                           "menubar=no, scrollbars=no, resizable=no')" +
                           "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
    }
    string VatFileName;
    public string GetVatPhotos()
    {
        string Images = "";
        string ext = System.IO.Path.GetExtension(this.vat_fileupload.PostedFile.FileName).ToLower();
        if (ViewState["VatFilename"] != null)
        {
            string extension = ViewState["VatFilename"].ToString();
            ext = "." + extension.Substring(extension.LastIndexOf(".") + 1).ToLower();
        }

        if (vat_fileupload.HasFile)
        {

            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".pdf")
            {
                //This code executes when user presses the fileupload button and then presses submit button
                VatFileName = vat_fileupload.FileName.Replace(" ", "");
                vat_fileupload.PostedFile.SaveAs(Server.MapPath(("~/Vat/" + VatFileName)));
                Images = "/Vat/" + VatFileName.ToString();
                lbl_vatImage.Text = "";
            }
            else
            {
                Images = "/Vat/download.jpg";
                VatFileName = "download.jpg";
                lbl_vatImage.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_vatImage.ForeColor = System.Drawing.Color.Red;

            }
        }
        else
        {
            //This code executes when user presses the fileupload button then presses show image button and
            //when it press submit button the value inside fileupload gets empty then we get value from image 
            //control.
            if (ViewState["VAtFile"] != null)
            {
                Images = ViewState["VAtFile"].ToString();
                int pos = Images.LastIndexOf("/") + 1;
                string fileName = Images.Substring(pos, Images.Length - pos).ToString();
                VatFileName = fileName;
                lbl_vatImage.Text = "";
            }
            else
            {
                Images = "/Vat/download.jpg";
                VatFileName = "download.jpg";
                lbl_vatImage.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_vatImage.ForeColor = System.Drawing.Color.Red;
            }
        }

        //else
        //{
        //    Images = "/Upload/download.jpg";
        //    VatFileName = "download.jpg";

        //    lbl_vatImage.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
        //    lbl_vatImage.ForeColor = System.Drawing.Color.Red;

        //}
        return (Images);
    }
    protected void btn_vatupload_Click(object sender, EventArgs e)
    {
       
        ViewState["VatFilename"] = null;
        Img_Vat.ImageUrl = GetVatPhotos();
       
        int pos = Img_Vat.ImageUrl.LastIndexOf("/") + 1;       
        string extension = Img_Vat.ImageUrl.Substring(pos, Img_Vat.ImageUrl.Length - pos).ToString();
        string ext = "." + extension.Substring(extension.LastIndexOf(".") + 1).ToLower();
        if (ext == ".pdf")
        {
            myframe.Attributes.Add("style", "visibility:visible");
            myframe.Attributes.Add("style", "height:150px;width:200px;");
            myframe.Attributes["src"] = Img_Vat.ImageUrl;
            Img_Vat.Attributes.Add("visibility", "none");
            Img_Vat.Attributes.Add("width", "0px");
            Img_Vat.Attributes.Add("height", "0px");
        }
        else
        {
            Img_Vat.Attributes.Add("visibility", "visible");
            Img_Vat.Attributes.Add("width", "200px");
            Img_Vat.Attributes.Add("height", "150px");
            myframe.Attributes.Add("style", "visibility:none");
        }
        ViewState["VAtFile"] = Img_Vat.ImageUrl;
        
        ViewState["VatFilename"] = Img_Vat.ImageUrl.Substring(pos, Img_Vat.ImageUrl.Length - pos);
        

    }


    protected void btn_reset_Click(object sender, EventArgs e)
    {
        TextBox1.Text = "";
        txt_Money.Text = "";
        txt_Description.Text = "";
        txt_Comment.Text = "";
        FileUpload1.Dispose();
        vat_fileupload.Dispose();
        Image1.ImageUrl = "";
        txt_Amount.Text = "";
        Img_Vat.ImageUrl = "";
        myframe.Src = "";
        Img_Vat.Attributes.Add("width", "0px");
        Img_Vat.Attributes.Add("height", "0px");
        myframe.Attributes.Add("style", "height:0px;width:0px;");
        Image1.Attributes.Add("width", "0px");
        Image1.Attributes.Add("height", "0px");
    }
}