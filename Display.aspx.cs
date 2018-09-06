using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data.SQLite;
using System.Web.UI.HtmlControls;

public partial class Display : System.Web.UI.Page
{
    public SQLiteConnection con;
    DataSet ds = new DataSet();
    DataTable dt;
    public string Value;
    protected void Page_Load(object sender, EventArgs e)
    {
        con = (SQLiteConnection)Session["connection"];
        Session["data_grid"]="";
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
        Session["valuegrid"] = dt;
        if (ds.Tables[0].Rows.Count > 0)
        {
           
            GridView1.DataSource = dt;
            GridView1.DataBind();
            DataRow row = dt.Rows[0];
           
        }

        con.Close();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        con.Open();
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        dt = (DataTable)Session["valuegrid"];
        string[] files = { dt.Rows[row.RowIndex]["Image"].ToString(), dt.Rows[row.RowIndex]["VatReceipt"].ToString() };
        int pos = files[0].LastIndexOf("/") + 1;
        string downloadfilename = files[0].Substring(pos, files[0].Length - pos);
        int poss = files[1].LastIndexOf("/") + 1;
        string downloadfilenames = files[1].Substring(pos, files[1].Length - pos);
        if (downloadfilename == "download.pdf"|| downloadfilenames == "download.pdf")
        { }
        else
        { 
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = Server.MapPath(files[i].ToString());
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        }
        using (SQLiteCommand cmd = new SQLiteCommand("delete FROM tbl_expenses where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
        dt.Rows.RemoveAt(e.RowIndex);
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
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg")
            {
                filename = FileUpload1.FileName.Replace(" ", "");
                filename = DateTime.Now + "-" + filename;
                filename = filename.Replace(" ", "-").Replace("/", "-").Replace(":", "");
                FileUpload1.SaveAs(Server.MapPath(("~/Upload/" + filename)));
                string ImagePath = Convert.ToString(Server.MapPath(("~/Upload/" + filename)));
                path = ConvertImageToPdf(ImagePath);
                int pos = path.LastIndexOf("/") + 1;
                filename = path.Substring(pos, path.Length - pos);
                lbl_image.Text = "";
            }
            else
            {
                if (ext == ".pdf")
                {
                    filename = FileUpload1.FileName.Replace(" ", "");
                    filename = DateTime.Now + "-" + filename;
                    filename = filename.Replace(" ", "-").Replace("/", "-").Replace(":", "");
                    FileUpload1.SaveAs(Server.MapPath(("~/Upload/" + filename)));
                    path = "/Upload/" + filename;
                    lbl_image.Text = "";
                }
                else
                {

                    path = "/Upload/download.pdf";
                    filename = "download.pdf";
                    lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                    lbl_image.ForeColor = System.Drawing.Color.Red;

                }
            }
        }
        else
        {
            // use previous user image if new image is not changed 

            System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)GridView1.Rows[e.RowIndex].FindControl("img_user");
            if (img.ImageUrl != "")
            {
                path = img.ImageUrl;
                int pos = path.LastIndexOf("/") + 1;
                filename = path.Substring(pos, path.Length - pos);
                //lbl_image.Text = "";
            }
            else
            {
                path = "/Upload/download.pdf";
                filename = "download.pdf";
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

            if (Vatext == ".jpg" || Vatext == ".png" || Vatext == ".gif" || Vatext == ".jpeg" )
            {
                vatfilename = VatFileUpload.FileName.Replace(" ", "");
                vatfilename = DateTime.Now + "-" + vatfilename;
                vatfilename = vatfilename.Replace(" ", "-").Replace("/", "-").Replace(":", "");
                VatFileUpload.SaveAs(Server.MapPath(("~/Vat/" + vatfilename)));
                string ImagePath = Convert.ToString(Server.MapPath(("~/Vat/" + vatfilename)));
                Vatpath = ConvertVatImageToPdf(ImagePath);
                int pos = Vatpath.LastIndexOf("/") + 1;
                vatfilename = Vatpath.Substring(pos, Vatpath.Length - pos);
                lbl_vat.Text = "";
            }
            else
            {
                if (ext == ".pdf")
                {
                    vatfilename = VatFileUpload.FileName.Replace(" ", "");
                    vatfilename = DateTime.Now + "-" + vatfilename;
                    vatfilename = vatfilename.Replace(" ", "-").Replace("/", "-").Replace(":", "");
                    VatFileUpload.SaveAs(Server.MapPath(("~/Vat/" + vatfilename)));
                    Vatpath = "/Vat/" + vatfilename;
                    lbl_image.Text = "";
                }
                else
                { 
                Vatpath = "/Vat/download.pdf";
                vatfilename = "download.pdf";
                lbl_vat.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_vat.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        else
        {
            // use previous user image if new image is not changed 

           System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)GridView1.Rows[e.RowIndex].FindControl("Image_EditVat");
            if (img.ImageUrl != "")
            {
                Vatpath = img.ImageUrl;
                int pos = Vatpath.LastIndexOf("/") + 1;
                vatfilename = Vatpath.Substring(pos, Vatpath.Length - pos);
                //lbl_vat.Text = "";
            }
            else
            {
                Vatpath = "/Vat/download.pdf";
                vatfilename = "download.pdf";
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
                                                                              "',VatFileName='" + vatfilename +
                                                                              "'where id='" + id + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
        con.Close();
       
        Session["valuegrid"] = dt;
        loaddata();
    }
    public String ConvertImageToPdf(string Name)
    {
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Name);
        int pos = Name.LastIndexOf("\\") + 1;
        filename = Name.Substring(pos, Name.Length - pos).Replace(" ", "");
        filename = filename.Remove(filename.LastIndexOf(".") + 1).ToLower();
        filename = DateTime.Now + "-" + filename + "pdf";
        filename = filename.Replace("/", "-").Replace(":", "").Replace(" ", "-");
        string OutPutFile = Server.MapPath(("~/Upload/" + filename));
        using (FileStream fs = new FileStream(OutPutFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f))
            {


                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {

                    doc.Open();
                    image.ScaleToFit(doc.PageSize);
                    image.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
                    image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight) / 2);
                    writer.DirectContent.AddImage(image);
                    doc.Close();
                   
                }
            }
          
        }
        if (File.Exists(Name))
        {
            File.Delete(Name);
        }
        OutPutFile = "/Upload/" + filename;
        return OutPutFile;
    }

    public String ConvertVatImageToPdf(string Name)
    {
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Name);
        int pos = Name.LastIndexOf("\\") + 1;
        vatfilename = Name.Substring(pos, Name.Length - pos).Replace(" ", "");
        vatfilename = vatfilename.Remove(vatfilename.LastIndexOf(".") + 1).ToLower();
        vatfilename = DateTime.Now + "-" + vatfilename + "pdf";
        vatfilename = vatfilename.Replace("/", "-").Replace(":", "").Replace(" ", "-");
        string OutPutFile = Server.MapPath(("~/Vat/" + vatfilename));
        using (FileStream fs = new FileStream(OutPutFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f))
            {

                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {

                    doc.Open();
                    image.ScaleToFit(doc.PageSize);
                    image.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
                    image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight) / 2);
                    writer.DirectContent.AddImage(image);
                    doc.Close();
                  
                }
            }
         
        }

        if (File.Exists(Name))
        {
            File.Delete(Name);
        }
        OutPutFile = "/Vat/" + vatfilename;
        return OutPutFile;
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