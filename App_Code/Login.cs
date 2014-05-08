using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for Login
/// </summary>
public class Login
{
    SqlConnection connect;
    public Login()
    {
        connect = new SqlConnection
            (ConfigurationManager.ConnectionStrings
            ["AutomartConnectionString"].ConnectionString);
    }

    public int ValidateLogin(string user, string pass)
    {
        int result = 0;
        PasswordHash ph = new PasswordHash();
        string sql = "Select PersonKey, CustomerPassCode, CustomerHashedPassword " +
            "From Customer.RegisteredCustomer Where Email = @User";
        SqlCommand cmd = new SqlCommand(sql, connect);
        cmd.Parameters.Add("@User", user);

        SqlDataReader reader;
        int passCode = 0;
        byte[] originalPassword = null;
        int personKey = 0;

        connect.Open();
        reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                passCode = (int)reader["CustomerPassCode"];
                originalPassword = (byte[])reader["CustomerHashedPassword"];
                personKey = (int)reader["PersonKey"];
            }

            byte[] newhash = ph.HashIt(pass, passCode.ToString());

            if (newhash.SequenceEqual(originalPassword))
            {
                result = personKey;
            }


        }
        connect.Close();
        return result;
    }
}