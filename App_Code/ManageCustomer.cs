using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for ManageCustomer
/// </summary>
public class ManageCustomer

{
    SqlConnection connect;

    public ManageCustomer()
    {
        connect = new SqlConnection
            (ConfigurationManager.ConnectionStrings["AutomartConnectionString"].ConnectionString);
    }

    public void WriteCustomer(Customer c)
    {
        string sqlPerson = "Insert into Person (LastName, Firstname) Values(@LastName, @FirstName)";
        string sqlVehicle = " Insert into Customer.Vehicle(LicenseNumber, VehicleMake, VehicleYear, PersonKey) " +
            "Values(@License, @Make, @Year, ident_Current('Person'))";
        string sqlRegisteredCustomer =
            "Insert into Customer.RegisteredCustomer(Email,CustomerPasscode, "
        + "CustomerPassword, CustomerHashedPassword, PersonKey) "
        + "Values(@email, @Passcode, @password, @hashedpass, ident_Current('Person'))";

        SqlCommand personCmd = new SqlCommand(sqlPerson, connect);
        personCmd.Parameters.AddWithValue("@LastName", c.LastName);
        personCmd.Parameters.AddWithValue("@FirstName", c.FirstName);

        SqlCommand vehicleCmd = new SqlCommand(sqlVehicle, connect);
        vehicleCmd.Parameters.AddWithValue("@License", c.LicenseNumber);
        vehicleCmd.Parameters.AddWithValue("@Make", c.VehicleMake);
        vehicleCmd.Parameters.AddWithValue("@Year", c.VehicleYear);

        PasscodeGenerator pg = new PasscodeGenerator();
        PasswordHash ph = new PasswordHash();
        int passcode = pg.GetPasscode();
        SqlCommand regCustomerCmd = new SqlCommand(sqlRegisteredCustomer, connect);
        regCustomerCmd.Parameters.AddWithValue("@Email", c.Email);
        regCustomerCmd.Parameters.AddWithValue("@Passcode", passcode);
        regCustomerCmd.Parameters.AddWithValue("@password", c.PlainPassword);
        regCustomerCmd.Parameters.AddWithValue("@hashedPass", ph.HashIt(c.PlainPassword.ToString(), passcode.ToString()));

        
        connect.Open();
        personCmd.ExecuteNonQuery();
        vehicleCmd.ExecuteNonQuery();
        regCustomerCmd.ExecuteNonQuery();
       connect.Close(); 


    }
}