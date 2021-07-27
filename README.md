# Component

Notification component (MVC) .NET using Hangfire and Postal. 

Versions:
- Hangfire - 
- Postal

Describe:
Component allows sending scheduled email to users in tasks/ToDo (web) apps.
It may be used as reminder about undone/today/future tasks or events for every user. Developer can also allow users to configure this option and then e.g. user can 
choose that to be notified only for future tasks. 

Configuration:
 You need to install hingfire and Postal v4 (for .Net 4) by NuGet .
In web.conf set your server email. It's example for gmail:

[  <system.net> 
    <mailSettings>
      <smtp from="youremail@gmail.com">
        <network host="smtp.gmail.com" port="587" userName="youremail@gmail.com" password="yourPassword" enableSsl="true" />
      </smtp>
    </mailSettings>
  </system.net> ]

Note: if you use google account you had to enable special option: https://support.google.com/accounts/answer/6010255?hl=en. It allows use your email in own apps. 
  
  Database connection
  
  


In startup class (create OWIN class if you don't have it) create methods, witch turn on ? hangfire service. 


In Noti.cshtml:

 @model YourProjectName.YourFolderName.Noti


By hangfire dashboard

Download:
- project example   link to example (I used 
- only notification component





methods:


Links:
Hangfire documentation: https://docs.hangfire.io/en/latest/
Documentation in PL language:
