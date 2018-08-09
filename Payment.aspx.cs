using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_name_Click(object sender, EventArgs e)
    {
        Session["Value"] = txt_payment.Text;
        Response.Write("<script>window.opener.document.getElementById('heads_btnHidden').click();window.close();</script>");

    }
}