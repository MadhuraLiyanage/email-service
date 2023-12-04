using MailNotifications.API.Enum;
using Serilog;

namespace MailNotifications.API.Models
{
    public static class TextLogger
    {
        public static void LogToText(LoogerType loggerEnum, string message)
        {
            switch (loggerEnum)
            {
                case LoogerType.Information:
                    Log.Information(message);
                    break;
                case LoogerType.Warning:
                    Log.Warning(message);
                    break;
                case LoogerType.Error:
                    Log.Error(message);
                    break;
                case LoogerType.Fatal:
                    Log.Fatal(message);
                    break;
                case LoogerType.Verbose:
                    Log.Verbose(message);
                    break;
                default:
                Log.Information(message);
                break;
            }

        }
    }
}
