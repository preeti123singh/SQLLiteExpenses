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
        }
    }

    public void PopulateDropdown()
    {
        var value = Session["Value"];
        
        
        if (value != null)
        {
            string query;
            query = "insert into tbl_payments (Payment) values('" + value + "')" ;           
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
            //DataTable dt = ds.Tables[0];
            dropdownlist.DataSource= ds.Tables[0];
            dropdownlist.DataTextField = "Payment";
            //dropdownlist.DataValueField = "ID";
            dropdownlist.DataBind();
            connection.Close();
    }

    
    
    string Filename;
    public string GetPhotos() {
        string Images = "";
        //string ext="";
        string ext = System.IO.Path.GetExtension(this.FileUpload1.PostedFile.FileName).ToLower();
        if (ViewState["Filename"]!=null)
        {   string extension = ViewState["Filename"].ToString();
            ext = "."+ extension.Substring(extension.LastIndexOf(".") + 1).ToLower();
        }
       
        if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg")
        {
            if (FileUpload1.HasFile)
            {
                Filename = FileUpload1.FileName.Replace(" ", "");
                FileUpload1.PostedFile.SaveAs(Server.MapPath(("~/Upload/" + Filename)));
                Images = "/Upload/" + Filename.ToString();
                lbl_image.Text = "";
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
                    lbl_image.Text = "";
                }
            }
        }
        else
        {
            Images = "/Upload/download.jpg";
            Filename = "download.jpg";
            lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif extensions.";
            lbl_image.ForeColor = System.Drawing.Color.Red;
        }
        return (Images);
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "insert into tbl_expenses(Date,Money,Payment,Description,Comments,Image,Filename) values('" + Convert.ToDateTime(TextBox1.Text).ToString("yyyy-MM-dd") + "','"+txt_Money.Text+"','"+ dropdownlist.SelectedItem.Text+"','"+ txt_Description.Text+"','"+ txt_Comment.Text+"','"+ GetPhotos()+"','"+Filename+"')";
            connection.Open();
            using (SQLiteCommand sqlcom = new SQLiteCommand(query, connection))
            { 
                        sqlcom.ExecuteNonQuery();
            }
            connection.Close();

            Response.Redirect("Display.aspx");
        }
        catch(Exception es)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(),
                "MessageBox", "alert('" + es.Message + "');", true);
        }
    }


    protected void btn_reports_Click(object sender, EventArgs e)
    {
        //string parameter = txt_Money.Text + "|" + dropdownlist.SelectedItem.Text + "|" + txt_Comment.Text + "|" + txt_Description.Text + "|" + GetPhotos() + "|" + DateTime.Parse(TextBox1.Text) + "|" + "insert";

        Response.Redirect("~\\Report.aspx");
    }

    protected void btn_upload_Click(object sender, EventArgs e)
    {
        ViewState["Filename"] = null;
        Image1.ImageUrl= GetPhotos();
        ViewState["File"] = Image1.ImageUrl;
        int pos = Image1.ImageUrl.LastIndexOf("/") + 1;
        Image1.ImageUrl.Substring(pos, Image1.ImageUrl.Length - pos);
        ViewState["Filename"]= Image1.ImageUrl.Substring(pos, Image1.ImageUrl.Length - pos);
       
    }


    protected void btnHidden_Click(object sender, EventArgs e)
    {
        PopulateDropdown();
    }

    protected void btn_payment_Click(object sender, EventArgs e)
    {
        string popupScript = "<script language='javascript'>" +
                           "window.open('Payment.aspx', 'ThisPopUp', " +
                           "'left = 300, top=150, width=400, height=300, " +
                           "menubar=no, scrollbars=no, resizable=no')" +
                           "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
    }

    
}