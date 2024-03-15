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

        Employee employeeModel = null;
        Country countryModel = null;
        State stateModel = null;

        // GET: Employee
        public ActionResult Index()
        {
            BindCountry();
            BindData();
            return View();
        }

        public JsonResult GetCities(int stateid)
        {
            return null;
        }

        public JsonResult GetStates(int Countryid)
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            string str = "select * from State_tbl where countryid=" + Countryid;
            SqlCommand cmd = new SqlCommand(str, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<SelectListItem> statelist = new List<SelectListItem>();
            while (dr.Read())
            {
                statelist.Add(new SelectListItem { Text = dr["statename"].ToString(), Value = dr["stateid"].ToString() });
            }
            return Json(statelist, JsonRequestBehavior.AllowGet);
        }

        public void BindData()
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            string str = "SELECT Employee_tbl.Id, Employee_tbl.Name, Employee_tbl.Email, Employee_tbl.Gender, Employee_tbl.Contact, Employee_tbl.Password, Employee_tbl.Address, Country_tbl.countryname, State_tbl.statename, Employee_tbl.Image " +
                         "FROM Employee_tbl INNER JOIN Country_tbl " +
                         "ON Employee_tbl.countryid = Country_tbl.countryid " +
                         "INNER JOIN State_tbl ON Employee_tbl.stateid = State_tbl.stateid";

            SqlCommand cmd =new SqlCommand(str, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Employee> employeelist = new List<Employee>();
            while (dr.Read())

            {
                employeeModel = new Employee();
                employeeModel.EmployeeId = Convert.ToInt32(dr["Id"]);
                employeeModel.Name = dr["Name"].ToString();
                employeeModel.Email = dr["Email"].ToString();
                employeeModel.Gender = dr["Gender"].ToString();
                employeeModel.Contact = Convert.ToInt32(dr["Contact"]);
                employeeModel.Password = dr["Password"].ToString();
                employeeModel.Address = dr["Address"].ToString();
                
                countryModel = new Country();
                countryModel.countryname = dr["countryname"].ToString();
                employeeModel.countryobj = countryModel;

                stateModel = new State();
                stateModel.statename = dr["statename"].ToString();
                employeeModel.stateobj = stateModel;

                employeeModel.Image = dr["Image"].ToString();

                employeelist.Add(employeeModel);
            }

            ViewData["EmployeeList"] = employeelist;
        }

        public void BindCountry()
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            string str = "select * from Country_tbl";
            SqlCommand cmd = new SqlCommand(str, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<SelectListItem> Countrylist = new List<SelectListItem>();
            while (dr.Read())
            {
                Countrylist.Add(new SelectListItem { Text = dr["countryname"].ToString(), Value = dr["countryid"].ToString() });
            }
            ViewBag.CountryList = Countrylist;

            connection.Close();
        }

        [HttpPost]
        public ActionResult Index(Employee employee, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                Image.SaveAs(Server.MapPath("~/Image/") + Image.FileName);
                SqlConnection connection = new SqlConnection(stringconnection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string str = "Insert into Employee_tbl(Name,Email,Gender,Contact,Password,Address,Image,TC,countryid,stateid) values ('" + employee.Name + "', '" + employee.Email + "','" + employee.Gender + "'," + Convert.ToInt32(employee.Contact) + ",'" + employee.Password + "', '" + employee.Address + "', '" + Image.FileName + "' , '" + Convert.ToBoolean(employee.tc) + "', " + Convert.ToInt32(employee.countryid) + " , " + Convert.ToInt32(employee.stateid) + ")";
                SqlCommand cmd = new SqlCommand(str, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                ModelState.Clear();
                TempData["msg"] = "Successfully Added Data";
                TempData.Keep();
                BindCountry();
                BindData();
                return View();
            }
            else
            {
                TempData["msg"] = "Please Upload Image";
                TempData.Keep();
                BindCountry();
                BindData();
                return View();
            }
        }
    }
}