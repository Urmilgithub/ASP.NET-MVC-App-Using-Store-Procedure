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
        City cityModel = null;

        // GET: Employee
        public ActionResult Index(int? employeeid)
        {
            if(employeeid > 0)
            {
                SqlConnection connection = new SqlConnection(stringconnection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

               // string str = "select * from Employee_tbl where Id =" + employeeid;
                SqlCommand cmd = new SqlCommand("SpEmployee_tbl", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", employeeid);
                cmd.Parameters.AddWithValue("@flag", 6);
                SqlDataReader dr = cmd.ExecuteReader();
                Employee model = null;
                while (dr.Read())

                {
                    model = new Employee();

                    TempData["EmployeeId"] = employeeid;
                    TempData.Keep("EmployeeId");

                    model.EmployeeId = Convert.ToInt32(dr["Id"]);
                    model.Name = dr["Name"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.Gender = dr["Gender"].ToString();
                    model.Contact = Convert.ToInt32(dr["Contact"]);
                    model.Password = dr["Password"].ToString();
                    model.Address = dr["Address"].ToString();

                    BindCountry();
                    model.countryid = Convert.ToInt32(dr["countryid"]);

                    GetStates(model.countryid);
                    model.stateid = Convert.ToInt32(dr["stateid"]);

                    GetCities(model.stateid);
                    model.cityid = Convert.ToInt32(dr["cityid"]);

                    ViewBag.Image = dr["Image"].ToString();

                }
                BindData();
                return View(model);
            }
            else
            {
                TempData["EmployeeId"]= null;
                BindCountry();
                BindData();
                return View();
            }
        }

        public JsonResult GetCities(int Stateid)
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            string str = "select * from City_tbl where stateid=" + Stateid;
            SqlCommand cmd = new SqlCommand(str, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<SelectListItem> citylist = new List<SelectListItem>();
            while (dr.Read())
            {
                citylist.Add(new SelectListItem { Text = dr["cityname"].ToString(), Value = dr["cityid"].ToString() });
            }

            ViewBag.CityList = citylist;
            return Json(citylist, JsonRequestBehavior.AllowGet);
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

            ViewBag.StateList = statelist;
            return Json(statelist, JsonRequestBehavior.AllowGet);
        }

        public void BindData()
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            //string str = "SELECT Employee_tbl.Id, Employee_tbl.Name, Employee_tbl.Email, Employee_tbl.Gender, Employee_tbl.Contact, Employee_tbl.Password, Employee_tbl.Address, Country_tbl.countryname, State_tbl.statename, City_tbl.cityname,Employee_tbl.Image " +
            //             "FROM Employee_tbl INNER JOIN Country_tbl " +
            //             "ON Employee_tbl.countryid = Country_tbl.countryid " +
            //             "INNER JOIN State_tbl ON Employee_tbl.stateid = State_tbl.stateid " +
            //             "INNER JOIN City_tbl ON Employee_tbl.cityid = City_tbl.cityid " +
            //             "ORDER BY Employee_tbl.Id DESC";

            SqlCommand cmd =new SqlCommand("SpEmployee_tbl", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@flag", 1);
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

                cityModel = new City();
                cityModel.cityname = dr["cityname"].ToString();
                employeeModel.cityobj = cityModel;

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
            // Update code
            if (TempData["EmployeeId"] != null)
            {              
                SqlConnection connection = new SqlConnection(stringconnection);
                if (connection.State == ConnectionState.Closed)
                {
                   connection.Open();
                }
                //string str;
                //if (Image != null)
                //{
                //    Image.SaveAs(Server.MapPath("~/Image/") + Image.FileName);
                //    str = "Update Employee_tbl set Name = '" + employee.Name + "',Email='" + employee.Email + "',Gender='" + employee.Gender + "',Contact=" + Convert.ToInt32(employee.Contact) + ",Password='" + employee.Password + "',Address= '" + employee.Address + "',Image= '" + Image.FileName + "' ,tc= '" + Convert.ToBoolean(employee.tc) + "',countryid= " + Convert.ToInt32(employee.countryid) + " ,stateid= " + Convert.ToInt32(employee.stateid) + ",cityid= " + Convert.ToInt32(employee.cityid) + " where Id= " + Convert.ToInt32(TempData["EmployeeId"]);
                //}
                //else
                //{
                //    str = "Update Employee_tbl set Name='" + employee.Name + "',Email='" + employee.Email + "',Gender='" + employee.Gender + "',Contact=" + Convert.ToInt32(employee.Contact) + ",Password='" + employee.Password + "',Address= '" + employee.Address + "',tc= '" + Convert.ToBoolean(employee.tc) + "',countryid= " + Convert.ToInt32(employee.countryid) + " ,stateid= " + Convert.ToInt32(employee.stateid) + ",cityid= " + Convert.ToInt32(employee.cityid) + " where Id= " + Convert.ToInt32(TempData["EmployeeId"]);
                //}

                SqlCommand cmd = new SqlCommand("SpEmployee_tbl", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(TempData["EmployeeId"]));
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Contact", Convert.ToInt32(employee.Contact));
                cmd.Parameters.AddWithValue("@Password", employee.Password);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                cmd.Parameters.AddWithValue("@countryid", Convert.ToInt32(employee.countryid));
                cmd.Parameters.AddWithValue("@stateid", Convert.ToInt32(employee.stateid));
                cmd.Parameters.AddWithValue("@Cityid", Convert.ToInt32(employee.cityid));
                cmd.Parameters.AddWithValue("@TC", Convert.ToBoolean(employee.tc));

                if (Image != null)
                {
                    Image.SaveAs(Server.MapPath("~/Image/") + Image.FileName);
                    cmd.Parameters.AddWithValue("@Image", Image.FileName);
                    cmd.Parameters.AddWithValue("@flag", 3);
                    cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@flag", 4);
                    cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                }

                    cmd.ExecuteNonQuery();
                    connection.Close();
                    ModelState.Clear();
                    TempData["msg"] = cmd.Parameters["@msg"].Value.ToString();
                    TempData.Keep();
                    BindCountry();
                    BindData();
                    TempData["EmployeeId"] = null;
                    return View();
            }
            else
            {
                // Insert code
                if (Image != null)
                {
                    Image.SaveAs(Server.MapPath("~/Image/") + Image.FileName);
                    SqlConnection connection = new SqlConnection(stringconnection);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    //string str = "Insert into Employee_tbl(Name,Email,Gender,Contact,Password,Address,Image,TC,countryid,stateid,cityid) values ('" + employee.Name + "', '" + employee.Email + "','" + employee.Gender + "'," + Convert.ToInt32(employee.Contact) + ",'" + employee.Password + "', '" + employee.Address + "', '" + Image.FileName + "' , '" + Convert.ToBoolean(employee.tc) + "', " + Convert.ToInt32(employee.countryid) + " , " + Convert.ToInt32(employee.stateid) + ", " + Convert.ToInt32(employee.cityid) + ")";
                   
                    SqlCommand cmd = new SqlCommand("SpEmployee_tbl", connection);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Contact", Convert.ToInt32(employee.Contact));
                    cmd.Parameters.AddWithValue("@Password", employee.Password);
                    cmd.Parameters.AddWithValue("@Address", employee.Address);
                    cmd.Parameters.AddWithValue("@countryid", Convert.ToInt32(employee.countryid));
                    cmd.Parameters.AddWithValue("@stateid", Convert.ToInt32(employee.stateid));
                    cmd.Parameters.AddWithValue("@Cityid", Convert.ToInt32(employee.cityid));
                    cmd.Parameters.AddWithValue("@TC", Convert.ToBoolean(employee.tc));
                    Image.SaveAs(Server.MapPath("~/Image/") + Image.FileName);
                    cmd.Parameters.AddWithValue("@Image", Image.FileName);
                    cmd.Parameters.AddWithValue("@flag", 2);
                    cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    connection.Close();
                    ModelState.Clear();
                    TempData["msg"] = cmd.Parameters["@msg"].Value.ToString();
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


        public ActionResult DeleteEmployee(int employeeid)
        {
            SqlConnection connection = new SqlConnection(stringconnection);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            //string str = "Delete from Employee_tbl where Id=" + employeeid;
            SqlCommand cmd = new SqlCommand("SpEmployee_tbl", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", employeeid);
            cmd.Parameters.AddWithValue("@flag", 5);
            cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 50);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connection.Close();
            ModelState.Clear();
            TempData["msg"] = cmd.Parameters["@msg"].Value.ToString();
            TempData.Keep("msg");
            BindCountry();
            BindData();
            TempData["EmployeeId"] = null;
            //return RedirectToAction("Index", new { employeeid = 0 });
            return View("Index");
        }

    }
}