Install as windows service
------------------------------
sc create MailNotifications binPath=D:\Build\MailNotifications\MailNotifications.API.exe

Remove windows service
-----------------------------
sc create MailNotifications


Components needed to make a web service
---------------------------------------
Install-Package Microsoft.Extensions.Hosting.WindowsServices
builder.Host.UseWindowsService(); //In program.cs
--Add to project file--
  <PropertyGroup>
    <OutPutType>Exe</OutPutType>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>



Database Creation
-----------------------------
Add-Migration InitialCreate
Update-Database

