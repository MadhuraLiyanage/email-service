using MailNotifications.Models;
using Microsoft.EntityFrameworkCore;

namespace MailNotifications.API.Models
{
    public class MailRepository : IMailRepository
    {
        private readonly AppDbContext appDbContext;
        public MailRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        public async Task<Mail> AddlMail(Mail mail)
        {
            
            //Update Hex file name to the attachment
            if (mail.AttachmentFileStream != null && mail.AttachmentFileStream.Trim().Length > 0)
            {
                mail.AttachmentFilePath = GlobalVariables.AttattachementPath + Utilities.Common.GetFileName(mail.AttachmentFileName);
            }
                
            var result = await appDbContext.Mails.AddAsync(mail);
            await appDbContext.SaveChangesAsync();
            //save attachment to Harddisk
            if (mail.AttachmentFileName != null && mail.AttachmentFileStream != null)
            {
                if (mail.AttachmentFileName.Trim().Length > 0  && mail.AttachmentFileStream.Trim().Length > 0)
                {
                    Utilities.Common.SaveAttachmentToHardisk(mail.AttachmentFilePath, mail.AttachmentFileStream);
                }
            }


            return result.Entity;
        }

        public async Task<IEnumerable<Mail>> GetAllMail()
        {
            var result = await appDbContext.Mails.ToListAsync();

 
            foreach (var mail in result)
            {
                if (mail.AttachmentFileName != null && mail.AttachmentFilePath != null)
                {
                    if (mail.AttachmentFileName.Trim().Length != 0 && mail.AttachmentFilePath.Trim().Length != 0)
                    {
                        mail.AttachmentFileStream = Utilities.Common.ReadAttachmentFromHardisk(mail.AttachmentFilePath);
                    }
                }
            }

            return await appDbContext.Mails.ToListAsync();
        }

        public async Task<Mail> UpdateMailStatus(Mail mail)
        {
            var result =  await appDbContext.Mails.FirstOrDefaultAsync(m => m.NotificationID == mail.NotificationID);
            if (result != null)
            {
                result.MailSent = mail.MailSent;
                result.MailStatus = mail.MailStatus;
                result.SentOn = mail.SentOn;
                result.Remarks = mail.Remarks;
                await appDbContext.SaveChangesAsync();

                return result;
            }
            return null;
        }
    }
}
