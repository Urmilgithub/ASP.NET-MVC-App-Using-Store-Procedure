using ASP.NET_MVC_App.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_App.Controllers
{
    public class LoginController : Controller
    {
        string stringconnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        Employee employeeModel = null;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Employee employee)
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand cmd = new SqlCommand("SpEmployee_tbl", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Password", employee.Password);
            cmd.Parameters.AddWithValue("@flag", 7);
            cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 50);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50);
            cmd.Parameters["@username"].Direction = ParameterDirection.Output;
            SqlDataReader dr = cmd.ExecuteReader();

            if(cmd.Parameters["@username"].Value.ToString() != "")
            {
                Session["loggedname"] = cmd.Parameters["@username"].Value.ToString();
                return RedirectToAction("Index", "Employee");
            }
            
            TempData["msg"] = cmd.Parameters["@msg"].Value.ToString();
            TempData.Keep();

            while (dr.Read())
            {

            } 
            connection.Close();
            return View();
        }
    }
}