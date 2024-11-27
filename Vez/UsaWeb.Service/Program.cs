using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using UsaWeb.Service;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Implementations;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Features.SurgicalSiteInfection.Implementations;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);


//jwt auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
//jwt auth

IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var hosts = configuration.GetSection("Cors:AllowedOrigins").Get<List<string>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                                policy =>
                                {
                                    policy.WithOrigins(hosts.ToArray()).AllowAnyHeader().AllowAnyMethod();
                                });

});

// Add services to the container.

//builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<Usaweb_DevContext>(options =>
    options.UseSqlServer(configuration.GetSection("ConnectionStrings:DefaultConnection").ToString()));
builder.Services.AddScoped<ISurgicalSiteInfectionService, SurgicalSiteInfectionService>();
builder.Services.AddScoped<ISurgicalSiteInfectionRepository, SurgicalSiteInfectionRepository>();
builder.Services.AddScoped<ISurgicalSiteInfectionSkinPrepRepository, SurgicalSiteInfectionSkinPrepRepository>();
builder.Services.AddScoped<ISurgicalSiteInfectionSkinPrepService, SurgicalSiteInfectionSkinPrepService>();
builder.Services.AddScoped<IQrtCaseMeetingRepository, QrtCaseMeetingRepository>();
builder.Services.AddScoped<IQrtCaseMeetingService, QrtCaseMeetingService>();

var app = builder.Build();

app.UseHttpsRedirection();      //jwt

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();//jwt
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();
