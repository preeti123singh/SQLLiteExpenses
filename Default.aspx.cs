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

//using XsPDF.Pdf;
//using XsPDF.Drawing;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //PdfImageConverter pdfConverter = new PdfImageConverter("C:\\Users\\Preeti\\Downloads\\scorereport.pdf");

        //// Set the dpi, the output image will be rendered in such resolution
        //pdfConverter.DPI = 96;

        //// the output image will be rendered to grayscale image or not
        ////pdfConverter.GrayscaleOutput = false;

        //for (int i = 0; i < pdfConverter.PageCount; i++)
        //{
        //    // Convert each pdf page to jpeg image with original page size
        //    //Image pageImage = pdfConverter.PageToImage(i);
        //    // Convert pdf to jpg in customized image size
        //    System.Drawing.Image pageImage = pdfConverter.PageToImage(i, 500, 800);

        //    // Save converted image to jpeg format
        //    pageImage.Save("C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\" + "Page " + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
        //}
    }







    protected void Button2_Click(object sender, EventArgs e)
    {
        string[] filenames = { "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\29-08-2018-133000-img-20141011-wa0012.pdf", "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\29-08-2018-133000-img-20141011-wa0012.pdf" };
        //MergePDFs(filenames, "C:\\Test.pdf");
        string FileName = "C:\\Expense"  + ".pdf";
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //using (FileStream stream = new FileStream(FileName, FileMode.Create))
        //{
        //    Document document = new Document();
        //    var writer = PdfWriter.GetInstance(document,Response.OutputStream);
        //    PdfCopy pdf = new PdfCopy(document, stream);
        //    PdfReader reader = null;
        //    try
        //{
        //        document.Open();
        //        document.NewPage();
        //        foreach (string file in filenames)
        //        {
        //            reader = new PdfReader(file);
        //            pdf.AddDocument(reader);
        //            int pageno = reader.NumberOfPages;
        //            reader.Close();
        //        }
        //        Chunk chunk = new Chunk("Go to First Page");
        //        //chunk.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
        //        Paragraph p5 = new Paragraph(chunk);
        //        p5.Add(chunk);
        //        document.Add(p5);
        //        //writer.Close();
        //    }
        //catch (Exception)
        //{

        //    //if (reader != null)
        //    //{
        //    //    reader.Close();
        //    //}
        //}
        //finally
        //{
        //    if (document != null)
        //    {
        //        document.Close();
        //    }
        //}
        Document document = new Document();
        var writer = PdfWriter.GetInstance(document, Response.OutputStream);
        document.Open();
        Chunk chunk1 = new Chunk("Go to First Page");
        Paragraph p5 = new Paragraph(chunk1);
        chunk1.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
        p5.Add(chunk1);
        document.Add(p5);
       
        foreach (string s in filenames) { 
            PdfReader reader = new PdfReader(s) ;
            //FileStream fstream = new FileStream(FileName,FileMode.Create);
            ////PdfStamper pdfStamper = new PdfStamper(reader, fstream);
            PdfContentByte cb = writer.DirectContent;
            PdfImportedPage page = null;


            for (int i = 1; i <= reader.NumberOfPages; i++)
            {

               
                //iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                page = writer.GetImportedPage(reader, i);
                document.NewPage();
                
                
                cb.AddTemplate(page,30,30);
                Chunk a = new Chunk("Back");
                a.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
                document.Add(a);

                //cb.AddTemplate(page, 0.0F, 0.0F);
                //cb.BeginText();
                //cb.SetFontAndSize(BaseFont.CreateFont(), 12);
                //cb.SetTextMatrix(0, 0);
                //cb.ShowText("aaa");
                //cb.SetAction(new PdfAction(PdfAction.FIRSTPAGE), 0, 0, 0, 0);
                //cb.EndText();


                //page.BeginText(); // Start working with text.
                //BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
                //page.SetFontAndSize(baseFont, 15);
                //page.SetRGBColorFill(255, 0, 0);
                //float left = 0; // pageSize.Width / 20;
                //float bottom = pageSize.Height - 100;
                //page.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Some text", left, bottom, 0);
                //float width = page.GetEffectiveStringWidth("Some text", false);
                //page.SetAction(new PdfAction(PdfAction.FIRSTPAGE), left, bottom, left + width, bottom + 20);

                //page.EndText();

                //document.Add(iTextSharp.text.Image.GetInstance(page));

            }
            //reader.Close();
            //pdfStamper.FormFlattening = true;
            //pdfStamper.Close();
       

            //Chunk chunk1 = new Chunk("Go to First Page");
            //Paragraph p5 = new Paragraph(chunk1);
            //chunk1.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
            //p5.Add(chunk1);
            
        }
        
        document.Close();
        //}
    }

    //public static bool MergePDFs(IEnumerable<string> fileNames, string targetPdf)
    //{


    //    bool merged = true;
    //    //using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
    //    //{Response.ContentType = "application/pdf";

    //    Document document = new Document();
    //        //PdfCopy pdf = new PdfCopy(document, stream);
    //        PdfReader reader = null;
    //        try
    //        {
    //            document.Open();
    //            document.NewPage();
    //            foreach (string file in fileNames)
    //            {
    //                reader = new PdfReader(file);
    //            //pdf.AddDocument(reader);
    //                int pageno = reader.NumberOfPages;
    //                reader.Close();
    //            }
    //        Chunk chunk = new Chunk("Go to First Page");
    //        chunk.SetAction(new PdfAction(PdfAction.FIRSTPAGE));
    //        Paragraph p5 = new Paragraph();
    //        p5.Add(chunk);
    //        document.Add(p5);
    //    }
    //    catch (Exception)
    //        {
    //            merged = false;
    //            if (reader != null)
    //            {
    //                reader.Close();
    //            }
    //        }
    //        finally
    //        {
    //            if (document != null)
    //            {
    //                document.Close();
    //            }
    //        }
    //    //}
    //    //return merged;
    //    return merged;
    //}

    protected void Button3_Click(object sender, EventArgs e)
    {
        //PdfDocument doc = new PdfDocument();
        //doc.Pages.Add(new PdfPage());
        //XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
        //XImage img = XImage.FromFile("C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\downloadnew.jpg");

        //xgr.DrawImage(img, 0, 0);
        //doc.Save("C:\\Test.pdf");
        //doc.Close();
    }



    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\IMG_20180808_192954.jpg");
    //    using (FileStream fs = new FileStream("C:\\Test.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
    //    {
    //        using (Document doc = new Document(image))
    //        {
                
    //            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
    //            {
                    
    //                    doc.Open();
    //                    image.ScaleToFit(doc.PageSize);
    //                    image.IndentationLeft = 9f;
    //                    image.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
    //                    image.SetAbsolutePosition(0,0);
    //                    writer.DirectContent.AddImage(image);
    //                    doc.Close();
                    
    //            }
    //        }
    //    }
    //}
}