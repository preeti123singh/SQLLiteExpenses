using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SQLite;

public partial class Login : System.Web.UI.Page
{
    
   
    protected void Page_Load(object sender, EventArgs e)
    {
        //GetConnectionString();

            //connection = new SQLiteConnection("Data Source= \\SQLlite\\Expense.db;Version=3;");
            //connection.Open();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\Users\\Preeti\\Documents\\Visual Studio 2015\\WebSites\\CalculateExpenseSQLLite\\SQLlite\\Expense.db;Version=3");
            
                connection.Open();
                //string uid = TextBox1.Text;
                //string pass = TextBox2.Text;
                string uid = "preeti";
                string pass = "preeti";

                string qry = "select * from Ulogins where UserId='" + uid + "' and Password='" + pass + "'";
                using (SQLiteCommand cmd = new SQLiteCommand(qry, connection))
                {
                    using (SQLiteDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Login Sucessfully!')", true);
                            connection.Close();
                         Session["Connection"] = connection;
                        Response.Redirect("~\\Expense.aspx");
                        }
                    }
                }
            
            //else
            //{
            //    //Label4.Text = "UserId & Password Is not correct Try again..!!";

            //}
            //connection.Close();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    //public void GetConnectionString()
    //{
    //    try
    //    {
    //        connection = new  SQLiteConnection("Data Source= \\SQLlite\\Expense.db;Version=3;");
    //        connection.Open();
    //    }
    //    catch (Exception e)
    //    {
    //        Page.ClientScript.RegisterStartupScript(Page.GetType(),
    //            "MessageBox", "alert('" + e.Message + "');", true);
    //    }
    //    connection.Close();
      
    //}
}

    
