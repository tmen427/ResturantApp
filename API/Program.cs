using Hangfire;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text;
//using WebApplication2.BackgroundServices;

//using Restuarant.Infrastucture.Repository; 
using Newtonsoft.Json;
using Resturant.Infrastructure.Context;

using Resturant.Domain.Entity;
using System.Reflection;
using System.Text.Json.Serialization;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Resturant.Application.Extension;
using Resturant.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddTransient<Resturant.Application.Respository.IRepo<User>, UsersRepo>();

//builder.Services.AddTransient<IRepository, OrderRepo>();

builder.Services.AddRepositoryService();

//builder.Services.AddTransient<ExceptionHandlingMIddleware>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // ... other options
    })
    .AddCookie();


builder.Services.AddAuthorization();



builder.Services.AddIdentityApiEndpoints<WebUser>()
    .AddEntityFrameworkStores<RestaurantContext>()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    // options.Password.RequireDigit = true;
    // options.Password.RequireLowercase = true;
    // options.Password.RequireNonAlphanumeric = true;
    // options.Password.RequireUppercase = true;
    // options.Password.RequiredLength = 6;
    // options.Password.RequiredUniqueChars = 1;
    //
    // // Lockout settings.
    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // options.Lockout.MaxFailedAccessAttempts = 5;
    // options.Lockout.AllowedForNewUsers = true;

    // User settings.
   // options.User.AllowedUserNameCharacters =
     //   "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<RestaurantContext>(options
      => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), 
          optionsBuilder => optionsBuilder.MigrationsAssembly("Resturant.Infrastructure"))
      
 //  => options.UseInMemoryDatabase("TestDB").ConfigureWarnings(warning => warning.Ignore(InMemoryEventId.TransactionIgnoredWarning))
);


//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//builder.AddServices();
builder.Services.AddAnotherService(); 
//builder.Services.AddHangfire(configuration =>
//{
//    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//           .UseColouredConsoleLogProvider()
//           .UseSimpleAssemblyNameTypeSerializer()
//           .UseRecommendedSerializerSettings()
//           .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));



//}
//);
//builder.Services.AddHangfireServer(); 

//builder.Services.AddDbContext<ToDoContext>(opt =>
//   opt.UseInMemoryDatabase("TodoList"));




builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyResturantAPI", Version = "v1" });
});

// In Program.cs
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://18.234.93.248", "https://yummy.tonymdesigns.com/", "http://localhost:4200") // The URL of your front-end
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // IMPORTANT for cookies
        });
});

// Add the CORS middleware
//app.UseCors(MyAllowSpecificOrigins);
builder.Services.ConfigureApplicationCookie(options =>
{
    // options.Cookie.Domain = "http://54.235.37.4"; // Set the domain for the authentication cookie
    // options.Cookie.Path = "/"; // Ensure the cookie is accessible across the entire domain
    // options.Cookie.HttpOnly = true; 
    //this is needed if you want cookie to work in an http setting
   // options.Cookie.HttpOnly = true; 
    
    options.Cookie.SameSite = SameSiteMode.None;

    //this turns off https secure cookies
   // options.Cookie.HttpOnly = true;
   
   //this always needs to be on for http.context.user.indetity to work 
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   //does not really work when using cookiesecurepolicy.none ---switch to https tommorow
 //  options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Add your Nginx IP or subnet to KnownProxies if Nginx is not directly exposed
    // options.KnownProxies.Add(IPAddress.Parse("192.168.1.100")); 
});


builder.Services.AddHttpContextAccessor();



//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//this is needed to convert http to https 
app.UseHttpsRedirection();

app.UseForwardedHeaders();

app.MapControllers();
//app.UseCors(MyAllowSpecificOrigins);
app.UseCors(MyAllowSpecificOrigins);
//app.UseCors("AllowAllHeaders");

//app.UseMiddleware<ExceptionHandlingMIddleware>();
//app.UseHangfireDashboard(); 
app.UseAuthentication();
app.UseAuthorization();
//app.MapIdentityApi<WebUser>();

app.Run();



//add this for integration testing 
public partial class Program { }
