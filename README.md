# Notification component (MVC) .NET using Hangfire and Postal

## Versions
 - .Net Framework v4.8
 - Hangfire 1.7.22
 - Postal.Mvc4.1.2.0

## Describe

 Component allows sending scheduled email to users in Tasks/ToDo (web)apps.
It may be used as reminder about forgotten/todays/future tasks or events for every user. Developer can also allow users to configure this option and then eg. user can 
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
  In own way you need to get database connection and email of current logged user. In base you may stored also notification option for every user or set default for app.
 More about notification options you find below in using component.

## Attaching component to project

 Component includes:
 - Noti.cs (add this to model folder) MODEL
 - Noti.cshtml (after installing of Postal add this to Views/Emails folder) VIEW
 - NotiController.cs (add this to controller folder) CONTROLLER
 
 [add image]
  
 In Noti.cshtml change first line for namespace of model Noti:
  
  ```
  @model YourProjectName.NameofFolderwithModel.Users.Noti
  To: @Model.UserEmail
  Subject: @Model.Info

  <p>This task waiting for you. Go to the app to complete it.</p>
  @foreach (string item in @Model.TaskList)
  {
      <p> >> @item</p><br>
  }
   ```

## Using component

 Create instance of NotificationController in place where you want run notification component (eg. in controller for page after successfull login)

  ```
  NotiController notiController2 = new NotiController();
  ```
   
   Invoke method turnEmail which contains:
   - con: your database connection {String}
   - columnName: name of coumn with task content/name (it will be desplay in email message) {String}
   - query {String}
   - useremail: email of current user {String}
   - option : notification option {String}
   
   ```
    notiController2.turnEmail(con, columnName, query, useremail,option);
   ```
   
 Query shoud contains:
  - name or content notifications (String)
  - email of current user
  - time, task deadline (it will be compared with current day and choose tasks to notification depends on notification option)
 
 eg. string query = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[Table_Task] where [UserEmail]='" + useremail + "' AND [TimeTask]"; //get tasks for current user

Notification options:
 - option = 1 notification send to user about forgotten tasks
 - option = 2 notification send to user max 2 days before tasks deadline
 - option = 3 notification send to user about today tasks

 All could looks like that:
   ```
   static string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
   static string useremail;
   SqlCommand command = null;
   string option;
        
  public ActionResult Index()
  {
     option = getOption(useremail);     
     string query = @"SELECT [NotiName], [TimeTask], [UserEmail] from [dbo].[Table_Task] where [UserEmail]='" + useremail + "' AND [TimeTask]"; //get tasks
     
     //Start working with component:             
     notiController2.turnEmail(con, columnName, query, useremail, option);
     
     return RedirectToAction("TaskList"); 
  }
   ```

By hangfire dashboard you can control scheduled jobs (in this case: state of every notification send to user). Just after your address add '/hangfire'.

## Download
- project example (I used embeded sql base)
- only notification component

## Useful Links
Hangfire documentation: https://docs.hangfire.io/en/latest/
Documentation in PL language: 
