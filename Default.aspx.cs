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







    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    string[] filenames = { "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\Expense26_07_201815_11_56.pdf", "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\Expense26_07_201818_27_17.pdf", "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\Expense27_07_201819_13_6.pdf", "C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\Expense30_07_201820_53_23.pdf" };
    //    MergePDFs(filenames, "C:\\Test.pdf");
    //}

    //public static bool MergePDFs(IEnumerable<string> fileNames, string targetPdf)
    //{
    //    bool merged = true;
    //    using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
    //    {
    //        Document document = new Document();
    //        PdfCopy pdf = new PdfCopy(document, stream);
    //        PdfReader reader = null;
    //        try
    //        {
    //            document.Open();
    //            foreach (string file in fileNames)
    //            {
    //                reader = new PdfReader(file);
    //                pdf.AddDocument(reader);
    //                reader.Close();
    //            }
    //        }
    //        catch (Exception)
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
    //    }
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



    protected void Button2_Click(object sender, EventArgs e)
    {
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\IMG_20180808_192954.jpg");
        using (FileStream fs = new FileStream("C:\\Test.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (Document doc = new Document(image))
            {
                
                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {
                    
                        doc.Open();
                        image.ScaleToFit(doc.PageSize);
                        image.IndentationLeft = 9f;
                        image.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
                        image.SetAbsolutePosition(0,0);
                        writer.DirectContent.AddImage(image);
                        doc.Close();
                    
                }
            }
        }
    }
}