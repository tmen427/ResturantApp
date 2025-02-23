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
using Restuarant.Infrastucture.Context;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;
using System.Reflection;
using MediatR;
using Resturant.Application.Extension; 


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // options.KnownProxies.Add(IPAddress.Parse("34.224.64.48"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddTransient<IRepo<CartItems>, CartItemsRepo>();
builder.Services.AddTransient<IRepo<User>, UsersRepo>();
//builder.Services.AddTransient<ExceptionHandlingMIddleware>();

//use in memory database instead of sql database right now 
builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlServer("name=WebApp2"));
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

    //mainly for testing with swagger-authorization header using a bearer token
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        In = ParameterLocation.Header,
        Description = "Please enter a token ",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        //   BearerFormat = "JWT",
        //  Scheme = "bearer"
    });


    //this HAS to be included to authorize a route with the authorize attribute-NOT for authentication 
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "http",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
    }
});

});

//the authentciation schema 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(x => x.Cookie.Name = "token")
    .AddJwtBearer(
       options => {
           options.TokenValidationParameters = new TokenValidationParameters
                                               {
                                                   ValidateIssuer = false,
                                                   ValidateAudience = false,
                                                   ValidateIssuerSigningKey = true,
                                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                                               };
//allows this application to access the Bearer token cookie - allwoing authorization to work 
options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        context.Token = context.Request.Cookies["token"];
        return Task.CompletedTask;
    }
};

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
        builder.AllowAnyOrigin()

                   .AllowAnyHeader()
                 .AllowAnyMethod();


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
