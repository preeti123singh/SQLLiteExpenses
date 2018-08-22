using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Item : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_name_Click(object sender, EventArgs e)
    {
        Session["item"] = txt_item.Text;
        Response.Write("<script>window.opener.document.getElementById('heads_btn_item_hidden').click();window.close();</script>");

    }
}