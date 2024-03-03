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
            BindCountry();
            return View();
        }

        public void BindCountry()
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            string str = "select * from Employee_tbl";
            SqlCommand cmd = new SqlCommand(str, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<SelectListItem> Countrylist = new List<SelectListItem>();
            while (dr.Read())
            {
                Countrylist.Add(new SelectListItem { Text = dr["countryid"].ToString(), Value = dr["countryname"].ToString() });
            }
            ViewBag.CountryList = Countrylist;

            connection.Close();
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