using Microsoft.EntityFrameworkCore;
using MyCalendar.Data;
using MyCalendar.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This sets up the necessary services for MVC (Model-View-Controller) architecture.
builder.Services.AddControllersWithViews();

// Add session usage
// Sets up distributed memory cache, which is used for storing session data.
builder.Services.AddDistributedMemoryCache();

// Configures session with specific options.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(600); // Set session timeout to 600 seconds.
    options.Cookie.HttpOnly = true; // Enhances security by restricting access to cookies from client-side scripts.
    options.Cookie.IsEssential = true; // Marks the session cookie as essential for the application to function.
});

// Dependency Injection for IHttpContextAccessor.
// Registers IHttpContextAccessor to allow access to the current HTTP context throughout the application.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Dependency Injection for ApplicationDbContext.
// Configures the application's DbContext with SQL Server using the connection string from configuration.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

//DI For HttpClient.
//Allows us to use the app system as client and to do http calls.
builder.Services.AddHttpClient();

//Defining DI for class using appsettings.json.
builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherAPI"));


// Builds WebApplication
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use a custom error handling page in non-development environments.
    app.UseExceptionHandler("/Home/Error");

    // HTTP Strict Transport Security Protocol (HSTS) is enabled in production.
    // It enforces secure (HTTPS) connections to the server.
    app.UseHsts();
}

// Enforce the use of HTTPS, redirecting HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enable serving static files, such as images, CSS, JavaScript, etc.
app.UseStaticFiles();

// Adds route matching to the middleware pipeline.
// This decides which endpoint (controller/action) will handle a given request.
app.UseRouting();

// Adds authorization capabilities to the application.
// Ensures that certain resources can only be accessed by authorized users.
app.UseAuthorization();

// Enables session state across HTTP requests.
// This allows storing user data between requests.
app.UseSession();

// Configures the default routing for MVC.
// In this case, the default controller is "Home" and the default action is "Index".
// For example, accessing the root URL of the site will use this route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// Runs the application.
app.Run();
