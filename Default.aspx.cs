using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using XsPDF.Pdf;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        PdfImageConverter pdfConverter = new PdfImageConverter("C:\\Users\\Preeti\\Downloads\\scorereport.pdf");

        // Set the dpi, the output image will be rendered in such resolution
        pdfConverter.DPI = 96;

        // the output image will be rendered to grayscale image or not
        //pdfConverter.GrayscaleOutput = false;

        for (int i = 0; i < pdfConverter.PageCount; i++)
        {
            // Convert each pdf page to jpeg image with original page size
            //Image pageImage = pdfConverter.PageToImage(i);
            // Convert pdf to jpg in customized image size
            System.Drawing.Image pageImage = pdfConverter.PageToImage(i, 500, 800);

            // Save converted image to jpeg format
            pageImage.Save("C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\Upload\\" + "Page " + i + ".png",System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}