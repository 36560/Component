using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.ComponentModel;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(webComponentT2.Startup))]
namespace webComponentT2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
