using MailNotifications.WS;
using Microsoft.Extensions.Configuration;
using Serilog;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();


var section = config.GetSection(nameof(GlobalVariables));
var globalConfig = section.Get<GlobalVariables>();

GlobalStaticVaiables.Interval = globalConfig.Interval;
GlobalStaticVaiables.LogFilePath = globalConfig.LogFilePath;
GlobalStaticVaiables.MailURI = globalConfig.MailURI;
GlobalStaticVaiables.MailMethod = globalConfig.MailMethod;
GlobalStaticVaiables.ApiToken = globalConfig.ApiToken;
GlobalStaticVaiables.TempAttachmentPath = globalConfig.TempAttachmentPath;
GlobalStaticVaiables.NotificationMailAddress = globalConfig.NotificationMailAddress;
GlobalStaticVaiables.CompanyPhoneNo = globalConfig.CompanyPhoneNo;
GlobalStaticVaiables.CompanyEmail = globalConfig.CompanyEmail;
GlobalStaticVaiables.CompanyWeb = globalConfig.CompanyWeb;
GlobalStaticVaiables.EmailServer = globalConfig.EmailServer;
GlobalStaticVaiables.EmailSmtpPort = globalConfig.EmailSmtpPort;
GlobalStaticVaiables.EmailUserName = globalConfig.EmailUserName;
GlobalStaticVaiables.EmailPassword = MailNotifications.Utilities.Common.Decrypt(globalConfig.EmailPassword);
GlobalStaticVaiables.EmailEnableSsl = globalConfig.EmailEnableSsl;

//logger
Log.Logger = new LoggerConfiguration().WriteTo.File(GlobalStaticVaiables.LogFilePath, rollingInterval: RollingInterval.Day).CreateLogger();



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseWindowsService()
    .Build();
    

try
{
    Log.Information("Starting up the Mail Nitification Worker Service");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error Starting Mail Nitification Worker Service");
    return;
}
finally
{
    Log.CloseAndFlush();
}


