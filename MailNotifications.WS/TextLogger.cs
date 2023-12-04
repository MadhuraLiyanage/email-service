using MailNotifications.WS.Enum;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailNotifications.WS
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
