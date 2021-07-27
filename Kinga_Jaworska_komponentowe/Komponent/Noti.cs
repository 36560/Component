using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestKomponent5.ViewModels.Users
{
    public class Noti : Email
    {
        //public int NotiID { get; set; }
      //  public int TaskID { get; set; }
      //  public int UserID { get; set; }
        public string NotiName { get; set; }
       // public System.DateTime Time { get; set; }
        public string UserEmail { get; set; }
        public string Info { get; set; }

        public List<string> TaskList { get; internal set; }
    }
}