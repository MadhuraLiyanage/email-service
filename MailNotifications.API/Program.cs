using MailNotifications.API.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
//PM
//Install-Package Serilog.Extensions.Logging.File -Version 2.0.0-dev-00024

//var builder = WebApplication.CreateBuilder(args);


//To run as a windows service (Desable when add-migration called)
var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};
var builder = WebApplication.CreateBuilder(webApplicationOptions);


//Make API a Windows Service
builder.Host.UseWindowsService();


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<IMailRepository, MailRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//logger
Log.Logger = new LoggerConfiguration().WriteTo.File(builder.Configuration.GetValue<string>("FilePaths:LogFilePath"), rollingInterval: RollingInterval.Day).CreateLogger();
GlobalVariables.AttattachementPath = builder.Configuration.GetValue<string>("FilePaths:AttattachementPath");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

/*app.UseEndpoints(endpoints =>
{
    //endpoints.MapRazorPages(); //Routes for pages
    endpoints.MapControllers(); //Routes for my API controllers
});*/


app.MapControllers();

app.Run();
