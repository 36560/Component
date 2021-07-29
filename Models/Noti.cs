using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace NotiComponent.ViewModels.Noti
{
    public class Noti : Email
    {
        public string NotiName { get; set; }
        public string UserEmail { get; set; }
        public string Info { get; set; }
        public List<string> TaskList { get; internal set; }
    }
}