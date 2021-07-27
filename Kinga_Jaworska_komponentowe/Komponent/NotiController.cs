using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestKomponent5.Models;
using TestKomponent5.ViewModels.Users;

namespace TestLoginKomponent.Controllers
{
    [Authorize]
    public class NotiController : Controller
    {
        //private int days=2;
       // public  int Days { get; set; }
        public NotiController()
        {}
        static SqlCommand command = null;
        List<String> NotiList = new List<String>();


        // GET: Noti
        public ActionResult Index()
        {
            return View();
        }

        public List<String> GetNotiToEmail(string con, string columnName, string query, string option, int days) //zadania, ktore sa zalegle- EMAIL
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();
                    
                    // var currentTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    //@"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[Table_Task] where [TimeTask]<'" + currentTime + "' AND [UserEmail]='" + useremail + "'"
                   
                    using (command = new SqlCommand(query  + "< '"+ currentTime + "'" /*+ " <'" + currentTime + "'"*/, connection)) //zaległe zadania
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<String> GetNotiFuture(string con, string columnName, string query, string option, int days) //zadania, ktore sa na przyszlosci (max 2 dni przed)  EMAIL
        {
            var futureTime = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");                         //mozliwosc konfuguracji na ile dni przed wyznaczoną datą
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
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<String> GetNotiToday(string con, string columnName, string query, string option, int days) //zadania, ktore sa na przyszlosc- EMAIL
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (command = new SqlCommand(query + "='" + currentTime + "'", connection)) //zaległe zadania
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

            catch (Exception ex)
            {
                return null;
            }
        }

        public void MailInBackground(string con, string columnName, string query, string useremail, string option, int days)
        {
            RecurringJob.AddOrUpdate(useremail, () => Mailing(con,columnName, query, useremail, option, days), Cron.Daily); // w ramach testów mozliwosc ustawienia częstotliwości Cron.Minutely
        }
        public void turnEmail(string con, string columnName, string query, string useremail, string option, int days)
        {
            try
            {
                MailInBackground(con, columnName, query, useremail,option, days);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Mailing(string con, string columnName, string query, string useremail, string option, int days)
        {
            List<String> NotiList2 = new List<String>();
          
            //spr czy notiBase mozna scalic z Noti

            switch(option)
            {
                case "1":
                    NotiList2 = GetNotiToEmail(con, columnName, query,option, days);

                    if (NotiList2.Count > 0)
                    {
                        //NotiBase notif = new NotiBase();
                        Noti notif = new Noti();
                        notif.Info = "Misje, o ktorych zapomniales :(";
                        notif.NotiName = "Zadanie";
                        notif.UserEmail = useremail;  //getCurrentUser przesylane do komponentu 

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2   //pobierane z bazy nazwy zadań do powiadomienia
                        };

                        email.Send();
                    }
                    break;

                case "2":
                    NotiList2 = GetNotiFuture(con, columnName, query,option, days);

                    if (NotiList2.Count > 0)
                    {
                        // NotiBase notif = new NotiBase();
                        Noti notif = new Noti();
                        notif.NotiName = "Zadanie";
                        notif.Info = "Misje z przyszłosci, ukoncz je przed terminem :D";
                        notif.UserEmail = useremail;  //getCurrentUser przesylane do komponentu 

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2     //pobierane z bazy nazwy zadań do powiadomienia
                        };

                        email.Send();
                    }
                    break;

                case "3":
                    NotiList2 = GetNotiToday(con, columnName, query,option, days);

                    if (NotiList2.Count > 0)
                    {
                        //NotiBase notif = new NotiBase();
                        Noti notif = new Noti();
                        notif.NotiName = "Zadanie";
                        notif.Info = "Misje, ktore czekaja na to, by wykonac je dzisiaj :)";
                        notif.UserEmail = useremail;  //getCurrentUser przesylane do komponentu 

                        var email = new Noti()
                        {
                            Info = notif.Info,
                            UserEmail = notif.UserEmail,
                            NotiName = notif.NotiName,
                            TaskList = NotiList2   //pobierane z bazy nazwy zadań do powiadomienia
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