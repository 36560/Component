using Hangfire;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using NotiComponent.ViewModels.Noti;

namespace NotiComponent.Controllers.NotiController
{
    [Authorize]
    public class NotiController : Controller
    {
        public NotiController()
        {}
        static SqlCommand command = null;
        List<String> NotiList = new List<String>();
        List<String> NotiList2;

        // GET: Noti
        public ActionResult Index()
        {
            return View();
        }

        public List<String> GetNotiToEmail(string con, string columnName, string query, string option)      //Forgotten tasks - still waiting to be completed
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();                    
                  
                    using (command = new SqlCommand(query  + "< '"+ currentTime + "'", connection)) //Forgotten tasks
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            NotiList.Add(reader[columnName] != DBNull.Value ? (string)reader[columnName] : "");
                        }
                    }
                }

                return NotiList;
            }
            catch
            {
                return null;
            }
        }

        public List<String> GetNotiFuture(string con, string columnName, string query, string option)   //Future tasks (max 2 days before current day)
        {

            var futureTime = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");                            //CONFIGURATION: you may set how many days before date of task, notification will be sent
            string currentTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            
            try
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();
                    
                    using (command = new SqlCommand(query + " BETWEEN '" + currentTime + "' AND '"+ futureTime +"'", connection)) 
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            NotiList.Add(reader[columnName] != DBNull.Value ? (string)reader[columnName] : "");
                        }
                    }
                }

                return NotiList;
            }
            catch             {
                return null;
            }
        }
        public List<String> GetNotiToday(string con, string columnName, string query, string option) //Task you have to complete today
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (command = new SqlCommand(query + "='" + currentTime + "'", connection)) //Today tasks
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            NotiList.Add(reader[columnName] != DBNull.Value ? (string)reader[columnName] : "");
                        }
                    }
                }
                return NotiList;
            }
            catch
            {
                return null;
            }
        }

        public void MailInBackground(string con, string columnName, string query, string useremail, string option)
        {
            RecurringJob.AddOrUpdate(useremail, () => Mailing(con,columnName, query, useremail, option), Cron.Daily); //CONFIGURATION: you can set frequency of sending notifications eg. Cron.Minutely
        }
        public void turnEmail(string con, string columnName, string query, string useremail, string option)
        {
            try
            {
                MailInBackground(con, columnName, query, useremail,option);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Mailing(string con, string columnName, string query, string useremail, string option)
        {
            NotiList2 = new List<String>();
            //System.Diagnostics.Debug.WriteLine(option);
            switch (option)
            {
                case "1":
                    NotiList2 = GetNotiToEmail(con, columnName, query,option);

                    if (NotiList2.Count > 0) //&& NotiList2 != null
                    {                        
                        Noti notif = new Noti();
                        notif.Info = "Missions you forgot :("; //EMAIL MESSAGE
                        notif.NotiName = "Task";
                        notif.UserEmail = useremail;  //getCurrentUser

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2   //getting from base names of tasks to notification
                        };

                        email.Send();
                    }
                    break;

                case "2":
                    NotiList2 = GetNotiFuture(con, columnName, query,option);

                    if (NotiList2.Count > 0) //&& NotiList2 != null
                    {
                        Noti notif = new Noti();
                        notif.NotiName = "Task";
                        notif.Info = "Future missions- complete it before deadline :D";    //EMAIL MESSAGE
                        notif.UserEmail = useremail;  //getCurrentUser

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2     //getting from base names of tasks to notification
                        };

                        email.Send();
                    }
                    break;

                case "3":
                    NotiList2 = GetNotiToday(con, columnName, query,option);

                    if (NotiList2.Count > 0) //&& NotiList2 != null
                    {                        
                        Noti notif = new Noti();
                        notif.NotiName = "Task";
                        notif.Info = "Missions waiting to be completed today :)"; //EMAIL MESSAGE
                        notif.UserEmail = useremail;  //getCurrentUser

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2   //getting from base names of tasks to notification
                        };

                        email.Send();
                    }
                    break;

                default:
                    break;

                }          
            }
        }
    }
