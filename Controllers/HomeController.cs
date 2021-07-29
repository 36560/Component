using NotiComponent.Controllers.NotiController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webComponentT2.Models;

namespace webComponentT2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        static string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static SqlDependency dependency = null;
        List<String> NotiList = new List<String>();
        static string useremail;
        SqlCommand command = null;
        string option;

        public string getCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                useremail = User.Identity.Name;
                return useremail;
            }
            else
            {
                return "User doesn't exist";  //Error  page
            }
        }

        public string getOption(string useremail)
        {
            string conn = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-webComponentT2-20210728083506.mdf;Initial Catalog=aspnet-webComponentT2-20210728083506;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    string query = @"SELECT [option] from [dbo].[AspNetUsers] where [Email]='" + useremail + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                option = reader.GetString(0);
                            }
                        }
                    }
                }

                return option;
            }
            catch (SqlException e)
            {
                return e.ToString();
            }
        }

        public ActionResult Index()
        {
            string columnName = "NotiName";
            useremail = getCurrentUser();
            //option = "3";                           //for entire app
            option = getOption(useremail); //special for user

            System.Diagnostics.Debug.WriteLine(useremail);
            System.Diagnostics.Debug.WriteLine(option);

            NotiController notiController2 = new NotiController();
           
            //string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            string query = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[TaskTable] where [UserEmail]='" + useremail + "' AND [TimeTask]"; //fragment of query, get tasks

            string conn = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-webComponentT2-20210728083506.mdf;Initial Catalog=aspnet-webComponentT2-20210728083506;Integrated Security=True";
            
            
            //TEST:
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    string query2 = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[TaskTable] where [UserEmail]='" + useremail + "'";
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                option = reader.GetString(0);
                            }
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine(option);
                //return option;
            }
            catch (SqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }


            //start component:             
            notiController2.turnEmail(con, columnName, query, useremail, option);

            return RedirectToAction("TaskList");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        #region CRUD OPERATION
        //READ: 
        [HttpGet] // Set the attribute to Read
        public ActionResult TaskList()
        {
            using (var context = new Entities())
            {
                // Return the list of data from the database
                string currentUser = getCurrentUser();
                var data = (from task in context.TaskTables where task.UserEmail == currentUser select task).ToList();

                return View(data);
            }
        }
        public ActionResult Edit(int id)
        {
            using (var context = new Entities())
            {
                var data = context.TaskTables.Where(x => x.TaskID == id).SingleOrDefault();
                return View(data);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TaskTable model)
        {
            using (var context = new Entities())
            {
                var data = context.TaskTables.FirstOrDefault(x => x.TaskID == id);

                // Checking if any such record exist 
                if (data != null)
                {
                    data.NotiName = model.NotiName;
                    data.TimeTask = model.TimeTask;
                    context.SaveChanges();

                    return RedirectToAction("TaskList");
                }
                else
                    return View();
            }
        }
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var context = new Entities())
            {
                var data = context.TaskTables.FirstOrDefault(x => x.TaskID == id);
                if (data != null)
                {
                    context.TaskTables.Remove(data);
                    context.SaveChanges();
                    return RedirectToAction("TaskList");
                }
                else
                    return View();
            }
        }
        #endregion

    }
}