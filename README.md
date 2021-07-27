# Notification component (MVC) .NET using Hangfire and Postal

## Versions
 - .Net Framework v4.8
 - Hangfire 1.7.22
 - Postal.Mvc4.1.2.0

## Describe

 Component allows sending scheduled email to users in Tasks/ToDo (web)apps.
It may be used as reminder about undone/today/future tasks or events for every user. Developer can also allow users to configure this option and then e.g. user can 
choose that to be notified only for future tasks. 

## Configuration
### Web.config
 You need to install Hangfire and Postal v4 (for .Net 4) by NuGet. In web.conf set your server email (from this address emails will be send to every user). It's example for gmail:
 
 ![new1](https://user-images.githubusercontent.com/67658221/127113257-62e10bc1-d4cb-4fad-803e-66ff3499e944.PNG)

```
<system.net> 
    <mailSettings>
      <smtp from="youremail@gmail.com">
        <network host="smtp.gmail.com" port="587" userName="youremail@gmail.com" password="yourPassword" enableSsl="true" />
      </smtp>
    </mailSettings>
</system.net>
```

### Startup.class
 Create startup OWIN class if you don't have it. Then invoke this methods:
 
  ```
 public partial class Startup
     {
         public void Configuration(IAppBuilder app)
         {
             //your other methods like connection with database
             app.UseHangfireDashboard();
             app.UseHangfireServer();            
         }
     }
```
 
#### Note: 
 If you use google account, you have to enable special option: https://support.google.com/accounts/answer/6010255?hl=en. It allows use your email in own apps. 
  
### Database connection, current user and notification options
  In own way you need to get database connection and email of current logged user.  

## Attaching component to project

 Component includes:
 - Noti.cs (add this to controller folder)
 - Noti.cshtml (after installing of Postal add this to Views/Emails folder
 - NotiController.cs (
 
 
 
 In Noti.cshtml:

 @model YourProjectName.YourFolderName.Noti

## Using component

 Create instance of NotificationController in place where you want run notification component (eg. controller for index page after successfull login)
 
 static string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
 (con, columnName, query, useremail,option
 
 
   ```
  NotiController notiController2 = new NotiController();
   ```
   
   Invoke method turnEmail which contains:
   - your database connection
   - email of current user
   - query 
   - name of coumn with task content
   
 Query shoud contains:
  - name or content notifications (String)
  - email of current user
  - time, task deadline
 
 eg. string query = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[Table_Task] where [UserEmail]='" + useremail + "' AND [TimeTask]"; //get tasks for current user

   ```
            option = getOption(useremail); 
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            string query = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[Table_Task] where [UserEmail]='" + useremail + "' AND [TimeTask]"; //get tasks

            //Wyzwalanie komponentu:             
            notiController2.turnEmail(con, columnName, query, useremail,option);
            return RedirectToAction("TaskList");
   ```

By hangfire dashboard you can control scheduled jobs (in this case: state of every notification send to user). Just after your address add '/hangfire'.

## Download
- project example (I used embeded sql base)
- only notification component

## Useful Links
Hangfire documentation: https://docs.hangfire.io/en/latest/
Documentation in PL language: 
