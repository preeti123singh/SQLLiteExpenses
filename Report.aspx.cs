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

public partial class Report : System.Web.UI.Page
{

    public SQLiteConnection con;
    public SQLiteDataAdapter adp;
    public DataSet ds;
    private static DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        con = (SQLiteConnection)Session["connection"];
        Page.ClientScript.RegisterStartupScript(typeof(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + autoScroll.ClientID + "');", true);
        if (data_grid.Rows.Count == 0)
        {
            btn_Excel.Visible = false;
            btn_Pdf.Visible = false;
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
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
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
        }
        con.Close();
    }

    protected void drp_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        string query;
        var result = drp_month.Text;
        query = "select * from tbl_expenses where strftime('%m', Date)='" + result + "'";
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
        lbl_image.Text = "";
        lbl_vat.Text = "";
        con.Open();
        DataTable dt = (DataTable)Session["data_grid"];
        dt.Rows.RemoveAt(e.RowIndex);
        Session["data_grid"] = dt;
        GridViewRow row = (GridViewRow)data_grid.Rows[e.RowIndex];
        using (SQLiteCommand cmd = new SQLiteCommand("delete FROM tbl_expenses where id='" + Convert.ToInt32(data_grid.DataKeys[e.RowIndex].Values[0].ToString()) + "'", con))
        {
            cmd.ExecuteNonQuery();
        }
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
                    lbl_image.Text = "Please upload image with .jpg,.jpeg,.png,.gif extensions.";
                    lbl_image.ForeColor = System.Drawing.Color.Red;

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
                        path = "/Upload/download.jpg";
                        filename = "download.jpg";
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

            System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)data_grid.Rows[e.RowIndex].FindControl("Image_EditVat");
            if (img.ImageUrl != "")
            {
                Vatpath = img.ImageUrl;
                //int pos = Image2.ImageUrl.LastIndexOf("/") + 1;
                //vatfilename = Vatpath.Substring(pos, Vatpath.Length - pos);
                lbl_vat.Text = "";
            }
            else
            {
                Vatpath = "/Upload/download.jpg";
                vatfilename = "download.jpg";
                lbl_vat.Text = "";
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
                                        "',VatFileName='"+ vatfilename +
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
        Session["data_grid"] = dt;
        data_grid.EditIndex = -1;
        con.Close();
        loaddata();
    }

    protected void data_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        data_grid.EditIndex = -1;
        loaddata();
    }

    protected void btn_find_Click(object sender, EventArgs e)
    {
        string query;
        var Startdate = Convert.ToDateTime(txt_StartDate.Text).ToString("yyyy-MM-dd");
        var Enddate = Convert.ToDateTime(txt_Enddate.Text).ToString("yyyy-MM-dd");
        query = "select * from tbl_expenses where date>= '" + Startdate + "' and " + "date<= '" + Enddate + "'";
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
        }
        else
        {
            btn_Excel.Visible = true;
            btn_Pdf.Visible = true;
        }
        Session["data_grid"] = dt;
        loaddata();
        con.Close();
    }

    protected void drp_yearly_SelectedIndexChanged(object sender, EventArgs e)
    {

        string query;
        var result = drp_yearly.SelectedItem.Text;
        if (result == "--Select--")
        {
            result = "0";
        }
        query = "select * from tbl_expenses where strftime('%Y', Date)='" + result + "'";
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


    public int GetLastPageNoExpenseTableInPdf()
    {
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        PdfPTable PdfTable1 = new PdfPTable(dt.Columns.Count);
        var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        PdfTable1.WidthPercentage = 100f;
        if (dt != null)
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

            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    
                        PdfPCell = new PdfPCell(new Paragraph(new Chunk(dt.Rows[rows][column].ToString(), font1)));
                        PdfTable1.AddCell(PdfPCell);
                    
                }
            }
            PdfTable1.SpacingBefore = 15f; // Give some space after the text or it may overlap the tabl    
        }
        pdfDoc.NewPage();
        pdfDoc.Add(PdfTable1);
        int currentPage = writer.PageNumber;
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
        //DataTable dt = new DataTable();
        dt = (DataTable)Session["data_grid"];
        Boolean columnExists = dt.Columns.Contains("ID");
        Boolean columnPageno = dt.Columns.Contains("PageNo");
        if (columnExists)
        {
            dt.Columns.Remove("ID");
            dt.Columns.Remove("Image");
        }
        if (!columnPageno)
        {
            dt.Columns.Add("PageNo");
        }
        List<string> ImageStr = GetImagesInHTMLString(sw.ToString());
        var unique_payments = new HashSet<string>(ImageStr);
        IDictionary<int, string> dict = new Dictionary<int, string>();
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();

        //Create object for image
        int startOfImages= GetLastPageNoExpenseTableInPdf()+1;

        foreach (string s in unique_payments)
        {
            int pos = s.LastIndexOf("\\") + 1;
            filename = s.Substring(pos, s.Length - pos);
            string ext = "." + filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            if (ext == "pdf")
            {
                PdfReader pdfReader = new PdfReader(s);
                int numberOfPages = pdfReader.NumberOfPages;
               
                dict.Add(startOfImages + numberOfPages - 1, filename);

            }
            else
            {
                dict.Add(startOfImages, filename);
                startOfImages++;
            }
        } 
        

             
            
        for (int rows = 0; rows < dt.Rows.Count; rows++)
        {
            for (int count1 = 0; count1 < dict.Count; count1++)
            {
                var element = dict.ElementAt(count1);
                var Key = element.Key;
                var Value = element.Value;

                if (dt.Rows[rows]["Filename"].ToString() == Value)
                {
                    dt.Rows[rows]["PageNo"] = Key;
                }


            }
        }

        PdfPTable PdfTable1 = new PdfPTable(dt.Columns.Count);
        PdfTable1.WidthPercentage = 100f;
        if (dt != null)
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
            PdfPCell = new PdfPCell(new Phrase(new Chunk("Link", font)));
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

            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    if (column == 5)
                    {
                        Chunk chunk = new Chunk(dt.Rows[rows]["Filename"].ToString(), font2);
                        //Chunk chunk = new Chunk("Go to Page");
                        var des = new PdfDestination(PdfDestination.XYZ, 0, pdfDoc.PageSize.Height, 0.99f);
                        int pageno = int.Parse(dt.Rows[rows]["PageNo"].ToString());
                        PdfAction action = PdfAction.GotoLocalPage(pageno, des, writer);
                        chunk.SetAction(action);
                        PdfPCell = new PdfPCell(new Paragraph(chunk));
                        PdfTable1.AddCell(PdfPCell);
                    }
                    else
                    {
                        PdfPCell = new PdfPCell(new Paragraph(new Chunk(dt.Rows[rows][column].ToString(), font1)));
                        PdfTable1.AddCell(PdfPCell);
                    }
                }
            }
            PdfTable1.SpacingBefore = 15f; // Give some space after the text or it may overlap the tabl    
        }
        pdfDoc.NewPage();      
        pdfDoc.Add(PdfTable1);
     
        foreach (string s in unique_payments)
        {
           
            pdfDoc.NewPage(); 
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(s);    
            gif.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
            //gif.ScalePercent(10f);
            gif.ScaleToFit(pdfDoc.PageSize);
            gif.IndentationLeft = 9f;
            gif.SetAbsolutePosition((PageSize.A4.Width - gif.ScaledWidth) / 2, (PageSize.A4.Height - gif.ScaledHeight) / 2);
            gif.SpacingAfter = 9f;
            pdfDoc.Add(gif);
          

            Chunk chunk = new Chunk("Go to First Page", font2);
            chunk.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
            Paragraph p5 = new Paragraph();
            p5.Add(chunk);
            pdfDoc.Add(p5);
        }

        pdfDoc.Close();
        Response.Write(pdfDoc);       
        Response.End();
       
    }

    private List<string> GetImagesInHTMLString(string htmlString)
    {
        List<string> images = new List<string>();
        string pattern = "<img.+?id=[\"'](.+?)[\"'].*?src=[\"'](.+?)[\"'].*?>";
       
        Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = rgx.Matches(htmlString);
        for (int i = 0, l = matches.Count; i < l; i++)
        {
            string idStr = matches[i].Groups[1].Value;
            if (idStr.Contains("Image1"))
            {
                string modifyStr = matches[i].Groups[2].Value.Remove(0, 7);
                images.Add(Server.MapPath("Upload") + modifyStr.Replace("/", "\\").Replace("%20", " "));
            }
            else
            {
                string modifyVatStr = matches[i].Groups[2].Value.Remove(0, 4);
                images.Add(Server.MapPath("Vat") + modifyVatStr.Replace("/", "\\").Replace("%20", " "));
            }
            
        }
        return images;
    }

    public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
    {
        throw new NotImplementedException();
    }

    protected void data_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

