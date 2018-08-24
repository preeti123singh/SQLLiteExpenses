using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;

public partial class Display : System.Web.UI.Page
{
    public SQLiteConnection con;
    DataSet ds = new DataSet();
    DataTable dt ;
    public string Value;
    protected void Page_Load(object sender, EventArgs e)
    {
        con = (SQLiteConnection)Session["connection"];
        
        if (!Page.IsPostBack)
        {
            loaddata();
        }
    }
    public void loaddata()
    {
       
        con.Open();
        SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM tbl_expenses ORDER BY id DESC LIMIT 1", con);
        SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
      
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fileName = dt.Rows[0]["VatReceipt"].ToString();
            //int pos = fileName.LastIndexOf("/") + 1;
            Value = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
            GridView1.DataSource = dt;
            GridView1.DataBind();
            DataRow row = dt.Rows[0];
            //Image1.ImageUrl = row["Image"].ToString();
            //Image2.ImageUrl = row["VatReceipt"].ToString();
        }
        
        con.Close();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        con.Open();
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        using (SQLiteCommand cmd = new SQLiteCommand("delete FROM tbl_expenses where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
        dt = new DataTable();
        GridView1.DataSource = dt;
        GridView1.DataBind();
        //Image1.ImageUrl = "";
        //Image2.ImageUrl = "";
        con.Close();       
    }

    string filename;
    string vatfilename;
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        TextBox textDate = (TextBox)row.Cells[2].Controls[0];
        TextBox textMoney = (TextBox)row.Cells[3].Controls[0];
        TextBox textpayment = (TextBox)row.Cells[4].Controls[0];
        TextBox textDescription = (TextBox)row.Cells[5].Controls[0];
        TextBox textComment = (TextBox)row.Cells[6].Controls[0];
        FileUpload FileUpload1 = (FileUpload)GridView1.Rows[e.RowIndex].FindControl("FileUpload1");
        string path = "/Upload/";
        string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
       
        
                if (FileUpload1.HasFile)
                {
                     if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".pdf")
                        {
                            path += FileUpload1.FileName.Replace(" ", "");
                            filename = FileUpload1.FileName.Replace(" ", "");
                             //save image in folder    
                            FileUpload1.SaveAs(MapPath(path));
                            lbl_image.Text = "";
                        }
                     else
                        {

                            path = "/Upload/download.jpg";
                            filename = "download.jpg";
                            lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                            lbl_image.ForeColor = System.Drawing.Color.Red;

                        }
                }
                else
                {
                    // use previous user image if new image is not changed 

                    Image img = (Image)GridView1.Rows[e.RowIndex].FindControl("img_user");
                    if (img.ImageUrl != "")
                    {
                    path = img.ImageUrl;
                    int pos = path.LastIndexOf("/") + 1;
                    filename = path.Substring(pos, path.Length - pos);
                    //lbl_image.Text = "";
                }
                    else
                    {
                        path = "/Upload/download.jpg";
                        filename = "download.jpg";
                        lbl_image.Text = "";
                    }
                }
            

        TextBox textItem = (TextBox)row.Cells[8].Controls[0];
        TextBox textVatAmount = (TextBox)row.Cells[9].Controls[0];
        FileUpload VatFileUpload = (FileUpload)GridView1.Rows[e.RowIndex].FindControl("FileUpload2");
        string Vatext = System.IO.Path.GetExtension(VatFileUpload.PostedFile.FileName).ToLower();
        string Vatpath = "/Vat/";
       
                if (VatFileUpload.HasFile)
                {

                    if (Vatext == ".jpg" || Vatext == ".png" || Vatext == ".gif" || Vatext == ".jpeg" || Vatext == ".pdf")
                        {
                            Vatpath += VatFileUpload.FileName.Replace(" ", "");
                            vatfilename = VatFileUpload.FileName.Replace(" ", "");
                            //save image in folder    
                            VatFileUpload.SaveAs(MapPath(Vatpath));
                            lbl_vat.Text = "";
                        }
                        else
                        {
                             Vatpath = "/Upload/download.jpg";
                             vatfilename = "download.jpg";
                             lbl_vat.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                             lbl_vat.ForeColor = System.Drawing.Color.Red;

                         }
                }
                else
                {
                    // use previous user image if new image is not changed 

                    Image img = (Image)GridView1.Rows[e.RowIndex].FindControl("Image_EditVat");
                    if (img.ImageUrl != "")
                    {
                         Vatpath = img.ImageUrl;
                         int pos = Vatpath.LastIndexOf("/") + 1;
                        vatfilename = Vatpath.Substring(pos, Vatpath.Length - pos);
                        //lbl_vat.Text = "";
                    }
                    else
                    {
                         Vatpath = "/Upload/download.jpg";
                         vatfilename = "download.jpg";
                        lbl_vat.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                        lbl_vat.ForeColor = System.Drawing.Color.Red;
                }
                }
           
            

        GridView1.EditIndex = -1;
        con.Open();
        var date = Convert.ToDateTime(textDate.Text).ToString("yyyy-MM-dd");
        using (SQLiteCommand cmd = new SQLiteCommand("update tbl_expenses set AmountPaid='" + textMoney.Text + 
                                                                              "',payment='" + textpayment.Text + 
                                                                              "',Description='" + textDescription.Text +
                                                                              "',Comments='" + textComment.Text +
                                                                              "',Image='" + path +
                                                                              "',Date='" + date +
                                                                              "',Filename='" + filename +
                                                                              "',Item='" + textItem.Text +
                                                                              "',VatAmount='" + textVatAmount.Text +
                                                                              "',VatReceipt='" + Vatpath +
                                                                              "',VatFileName='"+ vatfilename +
                                                                              "'where id='" + id + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
        con.Close();
        loaddata();
    }

    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {

    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        loaddata();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        loaddata();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}