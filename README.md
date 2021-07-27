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
 You need to install Hangfire and Postal v4 (for .Net 4) by NuGet. In web.conf set your server email. It's example for gmail:
 
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
 Create startup OWIN class if you don't have it. Ten create methods, witch turn on ? hangfire service. 
 
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

In Noti.cshtml:

 @model YourProjectName.YourFolderName.Noti


By hangfire dashboard you can control scheduled jobs (in this case: state of every notification send to user)



 
#### Note: 
If you use google account, you had to enable special option: https://support.google.com/accounts/answer/6010255?hl=en. It allows use your email in own apps. 
  
### Database connection, current user and notification options
  You need to in own way get database connection and email of current logged use. 




 Wywołuje na niej metodę turnEmailprzekazując jako argumenty połącznie z bazą, emailzalogowanego użytkownika,  kwerendę uzyskującą zadania  z  bazy  oraz  nazwę  kolumny pod  jaką  znajdują  sięzawartościpowiadomień. Programista w dowolny sposób przekazuje również informację o sposobie generowania powiadamiania.Umożliwia to dostarczenie  do  komponentunumeruopcji. Może on być spersonalizowany dla każdego użytkownika, bądź wybrany odgórnie przez programistę


## Using component

 Create instance of NotificationController in place where you want run notification component (eg. controller for index page after successfull login)
 

## Download
- project example   link to example (I used 
- only notification component

## Useful Links
Hangfire documentation: https://docs.hangfire.io/en/latest/
Documentation in PL language: 
