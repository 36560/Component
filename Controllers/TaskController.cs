using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webComponentT2.Models;

namespace TestLoginKomponent.Controllers
{   
    [Authorize]  //Add task
    public class TaskController : Controller
    {
        string useremail;
        // GET: Task
        public ActionResult Create()
        {
            return View();
        }
        public string getCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                useremail = User.Identity.Name;
                return useremail;
            }
            else
            {
                return "Brak usera"; //error page
            }
        }
        [HttpPost]
        public ActionResult create(TaskTable model)
        {
            if(ModelState.IsValid)
            {
                // To open a connection to the database
                using (var context = new Entities())
                {
                    model.UserEmail = getCurrentUser();
                    context.TaskTables.Add(model);

                    context.SaveChanges();
                    string message = "Created the record successfully";
                    ViewBag.Info = message;
                }                
            }

            return View();
        }
    }
}