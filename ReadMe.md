# Follow this for an A+

## Create the Project

- Create a New MVC (Choose MVC, not Empty) Project named `FinalExam`
- Add the Entity Framework package to the project thru Manage NuGet Packages

## Create the Database in SQL Server

- Copy the provided script in a new query and run
- Make sure the right ID's are assigned and edited
	- Click table and choose design
	- Double click on `Identity Specification` and choose seed/increment and make it a key if needs be
	- Check for any other database requirements that Anderson has stated in prompt
- Copy connection string from SQL Server Object explorer
- Add the connection string to the RIGHT Web.config file

```xml
<connectionStrings>
    <add name="DatabaseNameContext" connectionString="PASTE HERE"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
```
- Now create the database model by right clicking the Models folder and choosing Add | Class
- Name it the table name
	- Remember that columns named ID or with ID at the end of the name will be primary keys
	- You can add the attribute `[DatabaseGenerated(DatabaseGeneratedOption.None)]` to turn off the system trying to maintain int primary keys
	- Add the [Table] and [Key] annotations (along with other annotations)


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

        [Required(ErrorMessage = "A Last Name is required")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "An Address is required")]
        [Display(Name = "Address")]
        public string clientAddress { get; set; }

        [Required(ErrorMessage = "A City is required")]
        [Display(Name = "City")]
        public string clientCity { get; set; }

        [Required(ErrorMessage = "A State is required")]
        [Display(Name = "State")]
        public string clientState { get; set; }

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
Or for the virtual table to be linked, use this:

```c#
namespace BlowOut.Models
{
    [Table("Instrument")]
    public class Instrument
    {
        [Key]
        public int instrumentID { get; set; }

        [Display(Name = "Description")]
        public string instruDescription { get; set; }

        [Display(Name = "Price")]
        public string instruPrice { get; set; }

        [Display(Name = "Type")]
        public string instruType { get; set; }
	
	[ForeignKey("Client")]
        public virtual int? clientID { get; set; }
        public virtual Client Client { get; set; }
    }
}
```

- Modify Global.asax file to include using for models and the folder that will contain the context class (It's the Database.Set)
```c#
namespace BlowOut
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<InstrumentRentalContext>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
```
- Add a new folder to the project called DAL
- Add a new class to this folder called NBAContext.cs
- NOTE: This is the name of your dbContext variable and string in the connection string (web.config)

```c#
namespace FantasyBasketball.DAL
{
    public class NBAContext : DbContext
    {
        public NBAContext() : base("NBAContext")
        {

        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
```
- BUILD THE PROJECT
- Now go add scaffolded controllers making sure that generate views is selected and adding the model and the context to the scaffolding
	- You can do this by right mouse clicking on the controller folder and choosing New Scaffolded Item.
	- Choose MVC 5 Controller with views, using EntityFramework. Click Add
	- In the Model class click on the down arrow and choose Player. Click Add
- Save and build the project
- Run the project

## Helpful Code

### To create a login method
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

### The form for the Login view
```c#
@model BlowOut.Models.Instrument
@{
    ViewBag.Title = "Login";
}

<h2>User Login</h2>

<head>
    <meta name="viewport" content="width=device-width" />
    <title>RsvpForm</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-2.1.4.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
</head>
<body>
    <div class="jumbotron">
        <div class="container">
            @using (Html.BeginForm("Login", "Home", FormMethod.Post))
            {
                <label for="inputEmail">Email address</label>
                @Html.TextBox("Email address")
                <label for="inputPassword">Password</label>
                @Html.Password("Password")
                <button type="submit">Login</button>  
            }
        </div>
    </div>
</body>
```

### Part of the shared layout for navbar
```c#
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                @Html.ActionLink("BlowOut Instrument Rentals", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    <li>@Html.ActionLink("Rentals", "Rental", "Home")</li>
                    <li>@Html.ActionLink("About", "About", "Home")</li>
                    <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                    <li>@Html.ActionLink("Clients", "Index", "Clients")</li>
                    <li>@Html.ActionLink("Instruments", "Index", "Instruments")</li>
                    <li>@Html.ActionLink("Login", "Login", "Home")</li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required: false)
</body>
```
 
  
