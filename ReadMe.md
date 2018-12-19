# Follow this for an A+

## Create the Project

- Create a New MVC (Choose MVC, not Empty) Project named `FinalExam`
- Add the Entity Framework package to the project through Manage NuGet Packages

## Create the Database in SQL Server and connect it to Visual Studios

- Copy the provided script in a new query and run, this will make all of the tables
- Double check to see if there are keys in the database. If no data needs to be entered manually, click on the database diagrams and design a new diagram. This will create an ERD which will be the easiest to use when writing the models. 
- Now create the database model by right clicking the Models folder and choosing Add | Class
- Name it the table name
	- Remember that columns named ID or with ID at the end of the name will be primary keys
	- You can add the attribute `[DatabaseGenerated(DatabaseGeneratedOption.None)]` to turn off the system trying to maintain int primary keys
	- Add the [Table("Table Name")], [Key], [Display(Name = "New Name")], and virtual annotations (along with other annotations)


```c#
namespace BlowOut.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        [Display(Name = "ID Number")]
        public int clientID { get; set; }

        [Required(ErrorMessage = "A First Name is required")]
        [Display(Name ="First Name")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "A Zip Code is required")]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "Zip Code should contain only numbers")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Invalid Zip. Must be 5 numerical digits")]
        public string clientZip { get; set; }

        [Required(ErrorMessage = "An Email is required")]
        [Display(Name = "Email")]
        [RegularExpression(@"[\w-]+@([\w -]+\.)+[\w-]+", ErrorMessage = "Email should follow the format of: test@test.com")]
        public string clientEmail { get; set; }

        [RegularExpression(@"^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}$", ErrorMessage = "Phone Numbers should follow the format of: (123) 456-7890")]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "An Phone Number is required")]
        public string clientPhone { get; set; }
    }
}
```
Or for the annotation is a virtual key, use this: 
```c#
        public virtual int? clientID { get; set; }
        public virtual Client Client { get; set; }
```

After the models are all created to represent exactly what appears in the ERD, Create the DAL folder, which is where the connection to the database will be built. 
- Add a new class to this folder called This should be named something like DatabaseNameContext.cs
- NOTE: This is the name of your dbContext variable and string in the connection string (web.config)

```c#
namespace FantasyBasketball.DAL
{
    public class DatabaseNameContext : DbContext
    //:DbContext needs to be added to the code to
    {
        public DatabaseNameContext() : base("DatabaseNameContext"){} 
	//all of the created models go here in a DbSet which will allow visual studios to recognize the tables
	
        public DbSet<Team> Teams { get; set; }  
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
```
Now we will add the connection string to the Web.config file so that visual studios can connect to the database in SSMS
- Use the "Server Explorer" to navigate to the newly created database
- Right click the database and click porperties Copy connection string from SQL Server Object explorer
- Add the connection string to the Web.config file (the one at the bottom of the file structures" 

```xml
<connectionStrings>
    <add name="DatabaseNameContext" connectionString="PASTE HERE"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
```

-While we're here we will also add the code so that we can authorize logins 
```xml
<system.web>
  <compilation debug="true" targetFramework="4.6.1" />
  <httpRuntime targetFramework="4.6.1" 
  <authentication mode="Forms">
    <forms loginUrl="~/Home/Login" timeout="2880" />
  </authentication>
</system.web>
```

- Modify Global.asax file to include using for models and the folder that will contain the context class (It's the Database.Set)
```c#
namespace BlowOut
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<DatabaseNameContext>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
```

- BUILD THE PROJECT
- BUILD THE PROJECT
- BUILD THE PROJECT

- Now go add scaffolded controllers making sure that generate views is selected and adding the model and the context to the scaffolding
	- You can do this by right mouse clicking on the controller folder and choosing New Scaffolded Item.
	- Choose MVC 5 Controller with views, using EntityFramework. Click Add
	- In the Model class click on the down arrow and choose Player. Click Add
- Save and build the project
- Run the project

## Helpful Code

### To create a login method
- Go to your home controller and add these two methods 
- Be weary of SQL Statement syntax!!!
```c#
// GET: Home
public ActionResult Login()
{
    return View();
}

[HttpPost]
public ActionResult Login(FormCollection form, bool rememberMe = false)
{
    String email = form["Email address"].ToString();
    String password = form["Password"].ToString();

    var currentUser = db.Database.SqlQuery<Users>(
    "Select * " +
    "FROM Users " +
    "WHERE UserID = '" + email + "' AND " +
    "UserPassword = '" + password + "'");

    if (currentUser.Count() > 0)
    {
	FormsAuthentication.SetAuthCookie(email, rememberMe);
	return RedirectToAction("Index", "Home", new { userlogin = email });
    }
    else
    {
	return View();
    }
}
```
- After pasting this code, right click the login method to create the view. 

### The form for the Login view
```c#
@using (Html.BeginForm("Login", "Home", FormMethod.Post))
{
    <div class="container">
        <div class="form-group row">

            @Html.Hidden("ReturnURL", new { String = ViewBag.ReturnURL })

            <label for="username" class="col-sm-2 col-form-label">Email</label>
            <div class="col-sm-10">
                <input name="username" type="text" class="form-control" id="" placeholder="Name">

            </div>
        </div>
        <div class="form-group row">
            <label for="password" class="col-sm-2 col-form-label">Password</label>
            <div class="col-sm-10">
                <input name="password" type="password" class="form-control" id="" placeholder="Password">
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-8">
                <input type="submit" value="Log In" class="btn btn-default" />
            </div>
        </div>

    </div>

}
```

 
  
