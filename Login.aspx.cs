using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
public partial class Login : System.Web.UI.Page
{
    public SqlConnection connection;
    System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/CalculateExpense");
    System.Configuration.ConnectionStringSettings connString;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetConnectionString();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            //string uid = TextBox1.Text;
            //string pass = TextBox2.Text;
            string uid = "preeti";
            string pass = "preeti";
            connection.Open();
            string qry = "select * from Ulogins where UserId='" + uid + "' and Password='" + pass + "'";
            SqlCommand cmd = new SqlCommand(qry, connection);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Login Sucessfully!')", true);
                connection.Close();
                Response.Redirect("~\\Expense.aspx");
            }
            else
            {
                //Label4.Text = "UserId & Password Is not correct Try again..!!";

            }
            connection.Close();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    public void GetConnectionString()
    {
        try
        {

            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString = rootWebConfig.ConnectionStrings.ConnectionStrings["Expenses"];
                if (connection == null)
                {
                    connection = new SqlConnection(connString.ToString());

                    Session["Connection"] = connection;
                    //connection.Open();

                }
            }
        }
        catch (Exception e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(),
                "MessageBox", "alert('" + e.Message + "');", true);
        }

        finally
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}

    
