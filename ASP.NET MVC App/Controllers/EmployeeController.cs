using ASP.NET_MVC_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data; // used to store data in table
using System.Data.SqlClient; // For sql server connection
using System.Configuration; // Fore web config path

namespace ASP.NET_MVC_App.Controllers
{
    public class EmployeeController : Controller
    {

        string stringconnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Employee employee)
        {
            SqlConnection connect = new SqlConnection(stringconnection);
            if(connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            string str = "Insert into Employee_tbl(Name,Email,Gender,Contact,Password,Address,TC) values ('" + employee.Name +"', '" + employee.Email +"','" + employee.Gender +"','" + Convert.ToInt32(employee.Contact) + "','" + employee.Password +"', '" + employee.Address +"', '" + employee.tc +"')";
            SqlCommand cmd = new SqlCommand(str, connect);
            cmd.ExecuteNonQuery();
            connect.Close();
            ModelState.Clear();
            TempData["msg"] = "Successfully Added Data";
            TempData.Keep();
            return View();
        }
    }
}