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
using System.Text;
using System.Net.Mail;

public partial class Report : System.Web.UI.Page
{

    public SQLiteConnection con;


    public string Value;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["valuegrid"] = "";
        con = (SQLiteConnection)Session["connection"];
        Page.ClientScript.RegisterStartupScript(typeof(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + autoScroll.ClientID + "');", true);
        if (data_grid.Rows.Count == 0)
        {
            btn_Excel.Visible = false;
            btn_Pdf.Visible = false;
            btn_Pdf_Vat.Visible = false;
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
            btn_Pdf_Vat.Visible = true;
        }
    }
    public void loaddata()
    {

        data_grid.DataSource = (DataTable)Session["data_grid"];
        data_grid.DataBind();
        if (data_grid.Rows.Count == 0)
        {

            btn_Excel.Visible = false;
            btn_Pdf.Visible = false;
            btn_Pdf_Vat.Visible = false;
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
            btn_Pdf_Vat.Visible = true;
        }
        con.Close();
    }

    protected void drp_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        SQLiteDataAdapter adp;
        DataSet ds;
        DataTable dt = new DataTable();
        string query;
        var result = drp_month.Text;
        query = "select id,strftime('%d/%m/%Y',Date) as Date,AmountPaid,Payment,Description,Comments,Image,Filename,Item,VatAmount,VatReceipt,VatFileName from tbl_expenses where strftime('%m', Date)='" + result + "'";
        con.Open();
        SQLiteCommand sqlcmd = new SQLiteCommand(query, con);
        adp = new SQLiteDataAdapter(sqlcmd);
        ds = new DataSet();
        adp.Fill(ds);
        dt = ds.Tables[0];
        
        Session["data_grid"] = dt;
        loaddata();
        con.Close();
    }

    protected void data_grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        GridViewRow row = (GridViewRow)data_grid.Rows[e.RowIndex];
        lbl_image.Text = "";
        lbl_vat.Text = "";
        con.Open();
        DataTable dt = (DataTable)Session["data_grid"];

        using (SQLiteCommand cmd = new SQLiteCommand("delete FROM tbl_expenses where id='" + Convert.ToInt32(data_grid.DataKeys[e.RowIndex].Values[0].ToString()) + "'", con))
        {
            cmd.ExecuteNonQuery();
        }


        string[] files = { dt.Rows[row.RowIndex]["Image"].ToString(), dt.Rows[row.RowIndex]["VatReceipt"].ToString() };
        int pos = files[0].LastIndexOf("/") + 1;
        string downloadfilename = files[0].Substring(pos, files[0].Length - pos);
        int poss = files[1].LastIndexOf("/") + 1;
        string downloadfilenames = files[1].Substring(pos, files[1].Length - pos);

        if (downloadfilename == "default.pdf" || downloadfilenames == "default.pdf")
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
        
        dt.Rows.RemoveAt(e.RowIndex);
        Session["data_grid"] = dt;
        loaddata();
        con.Close();

    }

    protected void data_grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        data_grid.EditIndex = e.NewEditIndex;

        loaddata();
    }
    string filename;
    string vatfilename;
    protected void data_grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = Convert.ToInt32(data_grid.DataKeys[e.RowIndex].Value.ToString());
        GridViewRow row = (GridViewRow)data_grid.Rows[e.RowIndex];
        DataTable dt = (DataTable)Session["data_grid"];
        TextBox textDate = (TextBox)row.Cells[2].Controls[0];
        TextBox textMoney = (TextBox)row.Cells[3].Controls[0];
        TextBox textpayment = (TextBox)row.Cells[4].Controls[0];
        TextBox textDescription = (TextBox)row.Cells[5].Controls[0];
        TextBox textComment = (TextBox)row.Cells[6].Controls[0];
        FileUpload FileUpload1 = (FileUpload)data_grid.Rows[e.RowIndex].FindControl("FileUpload1");
        string path = "/Upload/";
        string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
        if (ext != null)
        {

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

                        path = "/Upload/default.pdf";
                        filename = "default.pdf";
                        lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                        lbl_image.ForeColor = System.Drawing.Color.Red;

                    }

                }
            }
            else
            {
                // use previous user image if new image is not changed 

                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)data_grid.Rows[e.RowIndex].FindControl("img_user");
                if (img.ImageUrl != "")
                {
                    path = img.ImageUrl;
                    int pos = img.ImageUrl.LastIndexOf("/") + 1;
                    filename = path.Substring(pos, path.Length - pos);
                    lbl_image.Text = "";
                }
                else
                {
                    path = "/Upload/default.pdf";
                    filename = "default.pdf";
                    lbl_image.Text = "";
                }
            }

        }
        TextBox textItem = (TextBox)row.Cells[8].Controls[0];
        TextBox textVatAmount = (TextBox)row.Cells[9].Controls[0];
        FileUpload VatFileUpload = (FileUpload)data_grid.Rows[e.RowIndex].FindControl("FileUpload2");
        string Vatext = System.IO.Path.GetExtension(VatFileUpload.PostedFile.FileName).ToLower();
        string Vatpath = "/Vat/";
        var date = Convert.ToDateTime(textDate.Text).ToString("yyyy-MM-dd");
        if (VatFileUpload.HasFile)
        {

            if (Vatext == ".jpg" || Vatext == ".png" || Vatext == ".gif" || Vatext == ".jpeg")
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

                if (Vatext == ".pdf")
                {
                    vatfilename = VatFileUpload.FileName.Replace(" ", "");
                    vatfilename = DateTime.Now + "-" + vatfilename;
                    vatfilename = vatfilename.Replace(" ", "-").Replace("/", "-").Replace(":", "");
                    VatFileUpload.SaveAs(Server.MapPath(("~/Vat/" + vatfilename)));
                    Vatpath = "/Vat/" + vatfilename;
                    lbl_vat.Text = "";
                }
                else
                {
                    Vatpath = "/Vat/default.pdf";
                    vatfilename = "default.pdf";
                    lbl_vat.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                    lbl_vat.ForeColor = System.Drawing.Color.Red;

                }

            }
        }
        else
        {
            // use previous user image if new image is not changed 

            System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)data_grid.Rows[e.RowIndex].FindControl("Image_EditVat");

            if (img.ImageUrl != "")
            {
                Vatpath = img.ImageUrl;
                int pos = Vatpath.LastIndexOf("/") + 1;
                vatfilename = Vatpath.Substring(pos, Vatpath.Length - pos);
                lbl_vat.Text = "";
            }
            else
            {
                Vatpath = "/Vat/default.pdf";
                vatfilename = "default.pdf";
                lbl_vat.Text = "Please upload image with .jpg,.jpeg,.png,.gif,.pdf extensions.";
                lbl_vat.ForeColor = System.Drawing.Color.Red;
            }
        }
        con.Open();

        using (SQLiteCommand cmd = new SQLiteCommand("update tbl_expenses set AmountPaid='" + textMoney.Text +
                                        "',payment='" + textpayment.Text +
                                        "',Description='" + textDescription.Text +
                                        "',Comments='" + textComment.Text +
                                        "',Image='" + path +
                                        "',Filename='" + filename +
                                        "',Date='" + date +
                                        "',Item='" + textItem.Text +
                                        "',VatAmount='" + textVatAmount.Text +
                                        "',VatReceipt='" + Vatpath +
                                        "',VatFileName='" + vatfilename +
                                        "' where id='" + id + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
        dt.Rows[row.RowIndex]["AmountPaid"] = textMoney.Text;
        dt.Rows[row.RowIndex]["Date"] = date;
        dt.Rows[row.RowIndex]["payment"] = textpayment.Text;
        dt.Rows[row.RowIndex]["Description"] = textDescription.Text;
        dt.Rows[row.RowIndex]["Comments"] = textComment.Text;
        dt.Rows[row.RowIndex]["Image"] = path;
        dt.Rows[row.RowIndex]["Item"] = textItem.Text;
        dt.Rows[row.RowIndex]["VatAmount"] = textVatAmount.Text;
        dt.Rows[row.RowIndex]["VatReceipt"] = Vatpath;
        dt.Rows[row.RowIndex]["Filename"] = filename;
        dt.Rows[row.RowIndex]["VatFileName"] = vatfilename;
        Session["data_grid"] = dt;
        data_grid.EditIndex = -1;
        con.Close();
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

    protected void data_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        data_grid.EditIndex = -1;
        loaddata();
    }

    protected void btn_find_Click(object sender, EventArgs e)
    {
        SQLiteDataAdapter adp;
        DataSet ds;
        DataTable dt = new DataTable();
        string query;
        var Startdate = Convert.ToDateTime(txt_StartDate.Text).ToString("yyyy-MM-dd");
        var Enddate = Convert.ToDateTime(txt_Enddate.Text).ToString("yyyy-MM-dd");
        query = "select id,strftime('%d/%m/%Y',Date) as Date,AmountPaid,Payment,Description,Comments,Image,Filename,Item,VatAmount,VatReceipt,VatFileName from tbl_expenses where date>= '" + Startdate + "' and " + "date<= '" + Enddate + "'";
        con.Open();
        SQLiteCommand sqlcmd = new SQLiteCommand(query, con);
        adp = new SQLiteDataAdapter(sqlcmd);
        ds = new DataSet();
        adp.Fill(ds);
        dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
        {
            btn_Excel.Visible = false;
            btn_Pdf.Visible = false;
            btn_Pdf_Vat.Visible = false;
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
            btn_Pdf_Vat.Visible = true;
        }
        Session["data_grid"] = dt;
        loaddata();
        con.Close();
    }

    protected void drp_yearly_SelectedIndexChanged(object sender, EventArgs e)
    {
        SQLiteDataAdapter adp;
        DataSet ds;
        DataTable dt = new DataTable();
        string query;
        var result = drp_yearly.SelectedItem.Text;
        if (result == "--Select--")
        {
            result = "0";
        }
        query = "select id,strftime('%d/%m/%Y',Date) as Date,AmountPaid,Payment,Description,Comments,Image,Filename,Item,VatAmount,VatReceipt,VatFileName from tbl_expenses where strftime('%Y', Date)='" + result + "'";
        con.Open();
        SQLiteCommand sqlcmd = new SQLiteCommand(query, con);
        adp = new SQLiteDataAdapter(sqlcmd);
        ds = new DataSet();
        adp.Fill(ds);
        dt = ds.Tables[0];
        Session["data_grid"] = dt;
        loaddata();
        con.Close();

    }

    protected void btn_Excel_Click(object sender, EventArgs e)
    {
        if (data_grid.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Expense" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            data_grid.Columns[1].Visible = false;
            data_grid.Columns[0].Visible = false;
            data_grid.Columns[7].Visible = false;
            data_grid.GridLines = GridLines.Both;
            data_grid.HeaderStyle.Font.Bold = true;
            data_grid.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public int GetLastPageNoExpenseTableInPdf(DataTable dt1)
    {
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        PdfPTable PdfTable1 = new PdfPTable(dt1.Columns.Count);
        var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        PdfTable1.WidthPercentage = 100f;
        if (dt1 != null)
        {

            PdfPCell PdfPCell = null;
            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Date", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("AmountPaid", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("payment", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Description", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Comments", font)));
            PdfTable1.AddCell(PdfPCell);
            //How add the data from datatable to pdf table
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            for (int rows = 0; rows < dt1.Rows.Count; rows++)
            {
                for (int column = 0; column < dt1.Columns.Count; column++)
                {

                    PdfPCell = new PdfPCell(new Paragraph(new Chunk(dt1.Rows[rows][column].ToString(), font1)));
                    PdfTable1.AddCell(PdfPCell);

                }
            }
            PdfTable1.SpacingBefore = 15f; // Give some space after the text or it may overlap the tabl    
        }
        pdfDoc.NewPage();
        pdfDoc.Add(PdfTable1);
        int currentPage = writer.PageNumber;
       
        pdfDoc.Close();
        return currentPage;
       
        
    }

    protected void btn_Pdf_Click(object sender, EventArgs e)
    {

        iTextSharp.text.Font font2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.RED);
        Response.ContentType = "application/pdf";
        string FileName = "Expense" + DateTime.Now + ".pdf";
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        data_grid.Columns[1].Visible = false;
        data_grid.Columns[0].Visible = false;
        data_grid.Columns[2].Visible = false;
       
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        data_grid.RenderControl(hw);
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)Session["data_grid"];
               
        Boolean columnPageno = dt1.Columns.Contains("PageNo");
        
        if (!columnPageno)
        {
            dt1.Columns.Add("PageNo");

        }
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        pdfDoc.NewPage();


        List<string> ImageStr = GetPdfImagesInExpenseHTMLString(sw.ToString());    
        var unique_payments_Expense = new HashSet<string>(ImageStr);       
        int startOfImages = GetLastPageNoExpenseTableInPdf(dt1) + 1;
        Dictionary<int, string> dict = new Dictionary<int, string>();
        dict = ReturnDictionary(startOfImages, unique_payments_Expense);


        for (int rows = 0; rows < dt1.Rows.Count; rows++)
        {
            for (int count1 = 0; count1 < dict.Count; count1++)
            {
                var element = dict.ElementAt(count1);
                var Key = element.Key;
                var Value = element.Value;

                if (dt1.Rows[rows]["Filename"].ToString() == Value)
                {
                    dt1.Rows[rows]["PageNo"] = Key;
                }
            }
        }


        PdfPTable PdfTable1 = new PdfPTable(dt1.Columns.Count - 4);
        PdfTable1.WidthPercentage = 100f;
        int pageno = 0;
        if (dt1 != null)
        {

            PdfPCell PdfPCell = null;
            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Date", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("AmountPaid", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("payment", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Description", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Comments", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Receipt", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Item", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("VatAmount", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("PageNo", font)));
            PdfTable1.AddCell(PdfPCell);
            //How add the data from datatable to pdf table
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            for (int rows = 0; rows < dt1.Rows.Count; rows++)
            {
                for (int column = 0; column < dt1.Columns.Count; column++)
                {

                    if (column == 7)
                    {
                        Chunk chunk = new Chunk("View", font2);
                        var des = new PdfDestination(PdfDestination.XYZ, 0, pdfDoc.PageSize.Height, 0.99f);
                        pageno = int.Parse(dt1.Rows[rows]["PageNo"].ToString());
                        PdfAction action = PdfAction.GotoLocalPage(pageno, des, writer);
                        chunk.SetAction(action);
                        PdfPCell = new PdfPCell(new Paragraph(chunk));
                        PdfTable1.AddCell(PdfPCell);
                    }
                    else
                    {
                        if (column == 0 || column == 6 || column == 10 || column == 11)
                        {


                        }
                        else
                        {

                            PdfPCell = new PdfPCell(new Paragraph(new Chunk(dt1.Rows[rows][column].ToString(), font1)));
                            PdfTable1.AddCell(PdfPCell);
                        }
                    }

                }
            }
            PdfTable1.SpacingBefore = 15f; // Give some space after the text or it may overlap the table    
        }


        pdfDoc.Add(PdfTable1);
      

        PdfImportedPage page = null;
        PdfContentByte cb = writer.DirectContent;

        foreach (string s in unique_payments_Expense)
        {
            PdfReader pdfReader = new PdfReader(s);
            int numberOfPages = pdfReader.NumberOfPages;

            for (int i = 1; i <= numberOfPages; i++)
            {

                page = writer.GetImportedPage(pdfReader, i);
                pdfDoc.NewPage();
                cb.AddTemplate(page, 3, 3);
                Chunk a = new Chunk("Top", font2);
                a.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
                pdfDoc.Add(a);

            }

        }
        dt1.Columns.Remove("PageNo");
       
       
        Session["data_grid"] = dt1;
        pdfDoc.Close();
        
        Response.Write(pdfDoc);
        Response.End();
        
    }

    public Dictionary<int, string> ReturnDictionary(int StartOfImages, HashSet<string> unique_payments_Expense)
    {
        int count = 0;
        List<int> iList = new List<int>();
        int accessNoPages = 0;
        Dictionary<int, string> dict1 = new Dictionary<int, string>();
        PdfReader pdfReader = null;
        foreach (string s in unique_payments_Expense)
        {
            int pos = s.LastIndexOf("\\") + 1;
            filename = s.Substring(pos, s.Length - pos);

            pdfReader = new PdfReader(s);

            int numberOfPages = pdfReader.NumberOfPages;
            iList.Add(numberOfPages);
            if (count == 0)
            {
                dict1.Add(StartOfImages, filename);
                count++;
            }
            else
            {
                StartOfImages = StartOfImages + iList.ElementAt(accessNoPages);
                dict1.Add(StartOfImages, filename);
                accessNoPages++;
            }
            pdfReader.Close();
            pdfReader.Dispose();
        }

        return dict1;
    }

    private List<string> GetPdfImagesInExpenseHTMLString(string htmlString)
    {
        List<string> ExpenseVatimages = new List<string>();
        string pattern = "<iframe.+?src=[\"'](.+?)[\"'].*?id=[\"'](.+?)[\"'].*?>";
        Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = rgx.Matches(htmlString);
        for (int i = 0, l = matches.Count; i < l; i++)
        {
            string idStr = matches[i].Groups[2].Value;
            if (idStr.Contains("DisplayExpensePdf"))
            {
                string modifyStr = matches[i].Groups[1].Value.Remove(0, 7);
                ExpenseVatimages.Add(Server.MapPath("Upload") + modifyStr.Replace("/", "\\").Replace("%20", " "));
            }
        }

        return ExpenseVatimages;
    }

    private List<string> GetPdfImagesInVatHTMLString(string htmlString)
    {
        List<string> ExpenseVatimages = new List<string>();
        string pattern = "<iframe.+?src=[\"'](.+?)[\"'].*?id=[\"'](.+?)[\"'].*?>";
        Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = rgx.Matches(htmlString);
        for (int i = 0, l = matches.Count; i < l; i++)
        {
            string idStr = matches[i].Groups[2].Value;
            if (idStr.Contains("DisplayPdf"))
            {
                string modifyVatStr = matches[i].Groups[1].Value.Remove(0, 4);
                ExpenseVatimages.Add(Server.MapPath("Vat") + modifyVatStr.Replace("/", "\\").Replace("%20", " "));
            }

        }
        return ExpenseVatimages;
    }

    public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
    {
        throw new NotImplementedException();
    }

    protected void data_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btn_Pdf_Vat_Click(object sender, EventArgs e)
    {

        iTextSharp.text.Font font2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.RED);
        Response.ContentType = "application/pdf";
        string FileName = "Vat" + DateTime.Now + ".pdf";
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        data_grid.Columns[1].Visible = false;
        data_grid.Columns[0].Visible = false;
        data_grid.Columns[2].Visible = false;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        data_grid.RenderControl(hw);

        DataTable dt1 = new DataTable();
        dt1 = (DataTable)Session["data_grid"];

        Boolean columnExists = dt1.Columns.Contains("Filename");
        Boolean columnPageno = dt1.Columns.Contains("PageNo");
               
        if (!columnPageno)
        {
            dt1.Columns.Add("PageNo");

        }
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        pdfDoc.NewPage();


       
        List<string> ImageVatStr = GetPdfImagesInVatHTMLString(sw.ToString());
        var unique_payments_Expense = new HashSet<string>(ImageVatStr);
       

        int startOfImages = GetLastPageNoExpenseTableInPdf(dt1) + 1;
        Dictionary<int, string> dict = new Dictionary<int, string>();
        dict = ReturnDictionary(startOfImages, unique_payments_Expense);


        for (int rows = 0; rows < dt1.Rows.Count; rows++)
        {
            for (int count1 = 0; count1 < dict.Count; count1++)
            {
                var element = dict.ElementAt(count1);
                var Key = element.Key;
                var Value = element.Value;

                if (dt1.Rows[rows]["VatFileName"].ToString() == Value)
                {
                    dt1.Rows[rows]["PageNo"] = Key;
                }
            }
        }


        PdfPTable PdfTable1 = new PdfPTable(dt1.Columns.Count - 4);
        PdfTable1.WidthPercentage = 100f;
        int pageno = 0;
        if (dt1 != null)
        {

            PdfPCell PdfPCell = null;
            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Date", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("AmountPaid", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Payment", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Description", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Comments", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Item", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("VatAmount", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("VatReceipt", font)));
            PdfTable1.AddCell(PdfPCell);
            PdfPCell = new PdfPCell(new Phrase(new Chunk("PageNo", font)));
            PdfTable1.AddCell(PdfPCell);
            //How add the data from datatable to pdf table
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            for (int rows = 0; rows < dt1.Rows.Count; rows++)
            {
                for (int column = 0; column < dt1.Columns.Count; column++)
                {

                    if (column == 10)
                    {
                        //Chunk chunk = new Chunk(dt1.Rows[rows]["Filename"].ToString(), font2);
                        Chunk chunk = new Chunk("View", font2);
                        var des = new PdfDestination(PdfDestination.XYZ, 0, pdfDoc.PageSize.Height, 0.99f);
                        pageno = int.Parse(dt1.Rows[rows]["PageNo"].ToString());
                        PdfAction action = PdfAction.GotoLocalPage(pageno, des, writer);
                        chunk.SetAction(action);
                        PdfPCell = new PdfPCell(new Paragraph(chunk));
                        PdfTable1.AddCell(PdfPCell);
                    }
                    else
                    {
                        if (column == 0 || column == 6 || column == 7 || column == 11)
                        {


                        }
                        else
                        {

                            PdfPCell = new PdfPCell(new Paragraph(new Chunk(dt1.Rows[rows][column].ToString(), font1)));
                            PdfTable1.AddCell(PdfPCell);
                        }
                    }

                }
            }
            PdfTable1.SpacingBefore = 15f; // Give some space after the text or it may overlap the table   
        }

        pdfDoc.Add(PdfTable1);

        PdfImportedPage page = null;
        PdfContentByte cb = writer.DirectContent;

        foreach (string s in unique_payments_Expense)
        {
            PdfReader pdfReader = new PdfReader(s);
            int numberOfPages = pdfReader.NumberOfPages;

            for (int i = 1; i <= numberOfPages; i++)
            {

                page = writer.GetImportedPage(pdfReader, i);
                pdfDoc.NewPage();
                cb.AddTemplate(page, 5, 5);
                Chunk a = new Chunk("Top", font2);
                a.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
                pdfDoc.Add(a);

            }

        }
        dt1.Columns.Remove("PageNo");


        
        Session["data_grid"] = dt1;
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }
}

