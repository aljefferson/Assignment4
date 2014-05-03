using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Customer c = new Customer();
        c.LastName = txtLastName.Text;
        c.FirstName = txtFirstName.Text;
        c.LicenseNumber = txtLicense.Text;
        c.VehicleMake = txtMake.Text;
        c.VehicleYear = txtYear.Text;
        c.PlainPassword = txtPassword.Text;
        c.Email = txtEmail.Text;

        ManageCustomer mc = new ManageCustomer();
        mc.WriteCustomer(c);

        lblError.Text= "Thanks for registering";
    }
}