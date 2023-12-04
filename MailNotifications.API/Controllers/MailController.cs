using MailNotifications.API.Models;
using MailNotifications.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using MailNotifications.API.Enum;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MailNotifications.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailRepository mailRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MailController(IMailRepository mailRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.mailRepository = mailRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<Mail>> QueueMail([FromHeader]string token, Mail mail)
        {
            //Get requesting host name
            string requestHostName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName;

            TextLogger.LogToText(Enum.LoogerType.Information, "HttpPost request " + requestHostName);
            //validate API tocken
            if (!Utilities.Common.ValidateToken(token))
            {
                TextLogger.LogToText(LoogerType.Error, "Invalid Token received. Token  " + token);
                return BadRequest("Invalid token");
            }
            TextLogger.LogToText(LoogerType.Information, "Token validated successfuly");

            //Validating email addresses
            //ToEmails
            string[] valiedEmails = mail.ToEmails.Split(';');
            if (!Utilities.Common.ValidateEmailAddress(valiedEmails))
            {
                return BadRequest("Invalid email in 'ToEmails' field.");
            }

            //CcEmails
            valiedEmails = null;
            if (mail.CcEmails != null)
            {
                valiedEmails = mail.CcEmails.Split(';');
                if (!Utilities.Common.ValidateEmailAddress(valiedEmails))
                {
                    return BadRequest("Invalid email in 'CcEmails' field.");
                }
            }

            //BccEmails
            valiedEmails = null;
            if (mail.BccEmails != null)
            {
                valiedEmails = mail.BccEmails.Split(';');
                if (!Utilities.Common.ValidateEmailAddress(valiedEmails))
                {
                    return BadRequest("Invalid email in 'BccEmails' field.");
                }
            }

            //Save mail details to Mail Queue
            try
            {
                Mail logMail = (Mail)mail.Clone();
                TextLogger.LogToText(Enum.LoogerType.Information, "Request Json : " + JsonSerializer.Serialize(logMail));
                logMail = null;

                //remove "/" from filr name
                if (mail.AttachmentFileName != null)
                {
                    mail.AttachmentFileName = mail.AttachmentFileName.Replace(@"/", @"-").Replace(@"\", @"-").Replace(@"'"," ").Replace(@"""", " ");
                }

                var result = await mailRepository.AddlMail(mail);

                var returnResult = result;
                result.AttachmentFileStream = null;
                TextLogger.LogToText(LoogerType.Information, "Response Json : " + JsonSerializer.Serialize(result));
                TextLogger.LogToText(LoogerType.Information, "Request successfully completed.");
                result.AttachmentFileStream = null;
                return Ok(returnResult);
                
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, "Error sending emails to queue. Error : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending emails to queue. Exception : " + ex.Message);

            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mail>>> GetAllMails([FromHeader] string token)
        {
            //Get requesting host name
            string requestHostName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName;

            TextLogger.LogToText(Enum.LoogerType.Information, "HttpGet request " + requestHostName);

            //validate API tocken
            if (!Utilities.Common.ValidateToken(token))
            {
                TextLogger.LogToText(LoogerType.Error, "Invalid Token received. Token  " + token);
                return BadRequest("Invalid token");
            }
            TextLogger.LogToText(LoogerType.Information, "Token validated successfuly");

            //Read pending email list
            try
            {
                var results = await mailRepository.GetAllMail();

                List<Mail> mails = new List<Mail>();
                foreach(Mail mail in results)
                {
                    mails.Add((Mail)mail.Clone());
                    mails[mails.Count - 1].AttachmentFileStream="Attachment details removed from the log";
                }

                TextLogger.LogToText(LoogerType.Information, "Response Json : " + JsonSerializer.Serialize(mails));
                mails = null;
                TextLogger.LogToText(Enum.LoogerType.Information, "Request successfully completed.");

                return Ok(results);
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, "Error retriving emails from the server. Error : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving details from the database. Exception : " + ex.Message);
            }
        }

        [HttpPut("{notificationId}")]
        public async Task<ActionResult<Mail>> UpdateStatus([FromHeader] string token, int notificationId, Mail mail)
        {
            //Get requesting host name
            string requestHostName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName;

            TextLogger.LogToText(Enum.LoogerType.Information, "HttpPut request " + requestHostName);

            //check for null object
            if (mail == null)
            {
                TextLogger.LogToText(LoogerType.Error, "Invalid request body.");
                return BadRequest("Invalid request body.");
            }

            //validate API tocken
            if (!Utilities.Common.ValidateToken(token))
            {
                TextLogger.LogToText(LoogerType.Error, "Invalid Token received. Token " + token);
                return BadRequest("Invalid token");
            }
            TextLogger.LogToText(LoogerType.Information, "Token validated successfuly");

            if (notificationId != mail.NotificationID)
            {
                TextLogger.LogToText(LoogerType.Warning, "Mail Notification ID mismatched. ID : " + notificationId.ToString());
                TextLogger.LogToText(LoogerType.Information, "Mail Notification ID mismatched. ID : " + notificationId.ToString());
                return BadRequest("Mail Notification ID mismatch.");

            }
            TextLogger.LogToText(LoogerType.Warning, "Mail Notification ID matched.");

            //Update status of the notification
            try
            {
                TextLogger.LogToText(LoogerType.Warning, "Mail Notification ID matched.");
                TextLogger.LogToText(Enum.LoogerType.Information, "Request Json : " + JsonSerializer.Serialize(mail));

                var result = await mailRepository.UpdateMailStatus(mail);
                var returnResult = result;
                result.AttachmentFileStream = null;
                TextLogger.LogToText(Enum.LoogerType.Information, "Response Json : " + JsonSerializer.Serialize(result));
                TextLogger.LogToText(LoogerType.Information, "Request successfully completed.");
                result.AttachmentFileStream = null;
                return Ok(result);
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, "Error updating email status in database. Error : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating email status in database. Exception : " + ex.Message);
            }

        }
    }
}
