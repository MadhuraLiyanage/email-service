using MailKit.Net.Smtp;
using MailNotifications.Models;
using MailNotifications.WS.Enum;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace MailNotifications.WS
{
    public class SendMail
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(GlobalStaticVaiables.MailURI)

        };

        public async Task FetchMails()
        {
            TextLogger.LogToText(LoogerType.Information, "Call RESTful API enpoint [GET] " + GlobalStaticVaiables.MailURI + GlobalStaticVaiables.MailMethod);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("token", GlobalStaticVaiables.ApiToken);

            try 
            { 
                var result = await _httpClient.GetAsync(GlobalStaticVaiables.MailMethod);

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    IEnumerable<Mail> mails = JsonConvert.DeserializeObject<IEnumerable<Mail>>(json);
                    //TextLogger.LogToText(LoogerType.Information, JsonConvert.SerializeObject(mail));

                    //remove attachment details from loging

                    List<Mail> logMails = new List<Mail>();
                    foreach (Mail mail in mails)
                    {
                        logMails.Add((Mail)mail.Clone());
                        logMails[logMails.Count - 1].AttachmentFileStream = "Attachment details removed from the log";
                    }

                    TextLogger.LogToText(LoogerType.Information, "Response received : " + JsonConvert.SerializeObject(logMails));

                    DisptchSMTPMails(mails);
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, "Received response : " + result.StatusCode);
                }
                return;
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Warning, "Error calling FetchMails method. Exception : " + ex.Message);
                return;
            }
        }

        private void DisptchSMTPMails(IEnumerable<Mail> mails)
        {
            //delete all files in the temprary attachment directory
            System.IO.DirectoryInfo di = new DirectoryInfo(GlobalStaticVaiables.TempAttachmentPath);

            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            //delete all sub folders
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            foreach (Mail mail in mails)
            {
                if (mail.AttachmentFileName != null && mail.AttachmentFilePath != null)
                {
                    if (mail.AttachmentFileName.Trim().Length >0 && mail.AttachmentFilePath.Trim().Length > 0)
                    {
                        //create a randum dir
                        string dirName = Utilities.Common.GetDirectory();
                        di.CreateSubdirectory(dirName);

                        string[] fileName = mail.AttachmentFileName.Trim().Split('\\');
                        //save the attachment to tempt path
                        Utilities.Common.SaveAttachmentToHardisk(GlobalStaticVaiables.TempAttachmentPath + dirName + "\\" + fileName[fileName.Length - 1], mail.AttachmentFileStream);
                        mail.AttachmentFilePath = GlobalStaticVaiables.TempAttachmentPath + dirName + "\\" + fileName[fileName.Length - 1];
                    }
                }

                try
                {
                    SendSMTPMails(mail);
                    TextLogger.LogToText(LoogerType.Information, "Email reference : " + mail.NotificationID.ToString() +  " sent successfully. Email sent to : " + mail.ToEmails);

                }
                catch (Exception ex)
                {
                    TextLogger.LogToText(LoogerType.Error, "Error sending email, Email reference : " + mail.NotificationID.ToString() + ". Email sent to : " + mail.ToEmails);

                }
            }

        }

        private async void SendSMTPMails(Mail mail)
        {
            MailMessage mailMessage = CreateMailMessage(mail);
            
            var smtpClient = new SmtpClient(GlobalStaticVaiables.EmailServer)
            {
                Port = GlobalStaticVaiables.EmailSmtpPort,
                Credentials = new NetworkCredential(GlobalStaticVaiables.EmailUserName, GlobalStaticVaiables.EmailPassword),
                EnableSsl = false,
            };

            try
            {
                smtpClient.Send(mailMessage);

                mail.AttachmentFileStream = "Attachment details removed from the log";
                mail.SentOn = DateTime.Now;
                mail.MailStatus = "Success";
                mail.MailSent = true;
                mail.Remarks = "Email dispatched successfully";
                await UpdateMail(mail);
            }
            catch (Exception ex)
            {
                mail.AttachmentFileStream = "Attachment details removed from the log";
                mail.SentOn = DateTime.Now;
                mail.MailStatus = "Error";
                mail.MailSent = true;
                mail.Remarks = ex.Message;
                await UpdateMail(mail);
            }
            finally
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }

           
        }

        private MailMessage CreateMailMessage(Mail mail)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(GlobalStaticVaiables.NotificationMailAddress),
                Subject = mail.Subject,
                Body = mail.Body,
                IsBodyHtml = mail.BodyInHTML,
            };

            //Add signature
            if (mail.BodyInHTML)
            {
                //html
                mailMessage.Body += "<br/><br/><br/><div>" +
                    "<span lang=EN-US style='font-size:9.0pt;mso-fareast-font-family: \"Times New Roman\";mso-fareast-theme-font:minor-fareast;color:#0070C0; mso-ansi-language:EN-US;mso-fareast-language:#2000;mso-no-proof:yes'>Company Group of Companies | P. O. Box 46, Main Street | Sigatoka | Fiji | P " + GlobalStaticVaiables.CompanyPhoneNo + "</span>" +
                    "<br/>" +
                    "<span lang=EN-US style='font-size:9.0pt;mso-fareast-font-family:\"Times New Roman\";mso-fareast-theme-font:minor-fareast;color:#0070C0;mso-ansi-language:EN-US;mso-fareast-language:#2000;mso-no-proof:yes'>Email : <ahref=" + GlobalStaticVaiables.CompanyEmail + "><span style='color:#0563C1'>" + GlobalStaticVaiables.CompanyWeb + "</span></a>| Website : <a href=\"http://www.Company.com.fj/\"><span style='color:#0070C0'>www.Company.com.fj</span></a></span>" +
                    "</div>";
            }
            else
            {
                //add test signature
                mailMessage.Body += "\n\n\XYZ Group of Companies | Main Street | Sri Lanka | P " + GlobalStaticVaiables.CompanyPhoneNo + "\n\nEmail : " + GlobalStaticVaiables.CompanyEmail + " | Website : " + GlobalStaticVaiables.CompanyEmail;
            }

            //Add  To Addresses
            string[] mailAddress = mail.ToEmails.Split(';');
            foreach (string mailAddresslItem in mailAddress)
            {
                mailMessage.To.Add(mailAddresslItem);
            }

            //Add CC Addresses
            mailAddress = null;
            if (mail.CcEmails != null)
            {
                mailAddress = mail.CcEmails.Split(';');
                foreach (string mailAddresslItem in mailAddress)
                {
                    mailMessage.CC.Add(mailAddresslItem);
                }
            }

            //Add CC Addresses
            mailAddress = null;
            if (mail.BccEmails != null)
            {
                mailAddress = mail.BccEmails.Split(';');
                foreach (string mailAddresslItem in mailAddress)
                {
                    mailMessage.Bcc.Add(mailAddresslItem);
                }
            }

            //add the attacmen
            if (mail.AttachmentFileName != null && mail.AttachmentFilePath != null)
            {
                if (mail.AttachmentFileName.Trim().Length > 0 && mail.AttachmentFilePath.Trim().Length > 0)
                {
                    var attachment = new Attachment(mail.AttachmentFilePath);
                    mailMessage.Attachments.Add(attachment);
                }
            }

            return mailMessage;
        }

        private async Task UpdateMail(Mail mail)
        {
            TextLogger.LogToText(LoogerType.Information, "Call RESTful API enpoint [PUT] " + GlobalStaticVaiables.MailURI + GlobalStaticVaiables.MailMethod);
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add("token", GlobalStaticVaiables.ApiToken);

            mail.AttachmentFileName = null;
            mail.AttachmentFilePath = null;
            mail.AttachmentFileStream = null;
            

            var maiSerialize = System.Text.Json.JsonSerializer.Serialize(mail);
            var content = new StringContent(maiSerialize, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var result = await _httpClient.PutAsync(GlobalStaticVaiables.MailMethod + "/" + mail.NotificationID.ToString(), content);

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    TextLogger.LogToText(LoogerType.Information, "Response received for reference # " + mail.NotificationID.ToString() + " : " + maiSerialize);
                    TextLogger.LogToText(LoogerType.Information, "Status updated successfully");
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, "Received error response received for reference # \" + mail.NotificationID.ToString() + : " + result.StatusCode);
                    TextLogger.LogToText(LoogerType.Information, "Status updated successfully for reference # " + mail.NotificationID.ToString());
                }
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Warning, "Error calling UpdateMail method. Exception : " + ex.Message);
                return;
            }
        }

    }
}
