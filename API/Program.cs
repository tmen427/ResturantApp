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
using Resturant.Infrastructure.Repository;
using Resturant.Domain.Entity;
using System.Reflection;
using System.Text.Json.Serialization;
using API.DTO;
using API.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Resturant.Application.Extension;
using Resturant.Infrastructure.Repository;


//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // options.KnownProxies.Add(IPAddress.Parse("34.224.64.48"));
});


builder.Services.AddControllers();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddTransient<Resturant.Application.Respository.IRepo<User>, UsersRepo>();

builder.Services.AddTransient<IRepository, OrderRepo>();
//builder.Services.AddTransient<ExceptionHandlingMIddleware>();

//use in memory database instead of sql database right now 
//builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlServer("name=WebApp2"));

builder.Services.AddDbContext<ToDoContext>(options
  //    => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    => options.UseInMemoryDatabase("TestDB").ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
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




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders", builder =>
    {
        // builder.WithOrigins("http://localhost:4200").
        //AllowAnyMethod().
        //AllowCredentials().
        //AllowAnyHeader();
        // allow all origins to work
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:4200")
//                          .AllowAnyHeader().AllowAnyMethod(); 
//                      });
//});


builder.Services.AddHttpContextAccessor();

//the background service , registered as a singleton 
//builder.Services.AddHostedService<DatabaseBackGround>();


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

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.UseCors(MyAllowSpecificOrigins);

app.UseCors("AllowAllHeaders");

//app.UseMiddleware<ExceptionHandlingMIddleware>();
//app.UseHangfireDashboard(); 


app.Run();



//add this for integration testing 
public partial class Program { }

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
