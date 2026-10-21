// Creates the application builder.
var builder = WebApplication.CreateBuilder(args);

// Adds MVC Controller and View services.
builder.Services.AddControllersWithViews();

// Adds IMemoryCache service.
builder.Services.AddMemoryCache();

// Adds memory-based distributed cache for Session.
builder.Services.AddDistributedMemoryCache();

// Adds Session support.
builder.Services.AddSession(options =>
{
    // Session will expire after 3 minutes of inactivity.
    options.IdleTimeout = TimeSpan.FromMinutes(3);

    // Makes the session cookie accessible only through HTTP.
    options.Cookie.HttpOnly = true;

    // Makes the session cookie essential.
    options.Cookie.IsEssential = true;
});

// Registers HttpClient for API requests.
builder.Services.AddHttpClient();

// Adds Response Caching service.
builder.Services.AddResponseCaching();

// Builds the application.
var app = builder.Build();

// Checks whether the application is running outside Development mode.
if (!app.Environment.IsDevelopment())
{
    // Handles errors in production.
    app.UseExceptionHandler("/Home/Error");
}

// Enables static files such as CSS and JavaScript.
app.UseStaticFiles();

// Enables routing.
app.UseRouting();

// Enables Response Caching middleware.
app.UseResponseCaching();

// Enables Session middleware.
app.UseSession();

// Enables authorization middleware.
app.UseAuthorization();

// Defines the default MVC route.
// DoctorController and Index action will open by default.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctor}/{action=Index}/{id?}");

// Starts the application.
app.Run();