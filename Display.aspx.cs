using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Display : System.Web.UI.Page
{
    public SqlConnection con;
    DataSet ds = new DataSet();
    DataTable dt ;
    protected void Page_Load(object sender, EventArgs e)
    {
        con = (SqlConnection)Session["connection"];
        
        if (!Page.IsPostBack)
        {
            loaddata();
        }
    }
    public void loaddata()
    {
        
        con.Open();
        SqlCommand cmd = new SqlCommand("select top 1 * from tbl_expenses order by ID desc", con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
      
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView1.DataSource = dt;
            GridView1.DataBind();
            DataRow row = dt.Rows[0];
            Image1.ImageUrl = row["Image"].ToString();
        }
        
        con.Close();
    }



   
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        con.Open();
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        SqlCommand cmd = new SqlCommand("delete FROM tbl_expenses where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", con);
        cmd.ExecuteNonQuery();
        dt = new DataTable();
        GridView1.DataSource = dt;
        GridView1.DataBind();
        Image1.ImageUrl = "";
        con.Close();       
    }

    string filename;
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        TextBox textDate = (TextBox)row.Cells[2].Controls[0];
        TextBox textMoney = (TextBox)row.Cells[3].Controls[0];
        TextBox textPayment = (TextBox)row.Cells[4].Controls[0];
        TextBox textDescription = (TextBox)row.Cells[5].Controls[0];
        TextBox textComment = (TextBox)row.Cells[6].Controls[0];
        //TextBox textImage = (TextBox)row.Cells[7].Controls[0];
        FileUpload FileUpload1 = (FileUpload)GridView1.Rows[e.RowIndex].FindControl("FileUpload1");
        string path = "/Upload/";
        if (FileUpload1.HasFile)
        {   
            path += FileUpload1.FileName.Replace(" ","");
            filename= FileUpload1.FileName.Replace(" ", "");
            //save image in folder    
            FileUpload1.SaveAs(MapPath(path));
        }
        else
        {
            // use previous user image if new image is not changed 
            
                Image img = (Image)GridView1.Rows[e.RowIndex].FindControl("img_user");
            if (img.ImageUrl != "")
            {
                path = img.ImageUrl;
                int pos = Image1.ImageUrl.LastIndexOf("/") + 1;
                filename = path.Substring(pos, path.Length - pos);
                
            }
            else
            {
                path= "/Upload/download.jpg";
                filename = "download.jpg";
            }
        }


        
        GridView1.EditIndex = -1;
        con.Open();
        var date = Convert.ToDateTime(textDate.Text).ToString("yyyy-MM-dd"); 
        SqlCommand cmd = new SqlCommand("update tbl_expenses set Money='" + textMoney.Text + "',Payment='" + textPayment.Text + "',Description='" + textDescription.Text + "',Comments='" + textComment.Text + "',Image='" + path + "',Date='" + date + "',Filename='" + filename+ "' where id='" + id + "'", con);
        cmd.ExecuteNonQuery();
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
}