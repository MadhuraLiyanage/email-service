using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace MailNotifications.Models 
{
    public class Mail : ICloneable
    {
        [Key]
        [Required]
        public int NotificationID { get; set; }
        [Required]       
        public string ToEmails { get; set; } = "";
        public string? CcEmails { get; set; } = "";
        public string? BccEmails { get; set; } = "";
        [Required]
        public string Subject { get; set; } = "";
        [Required]
        public string Body { get; set; } = "";
        [Required]
        public bool BodyInHTML { get; set; } = false;
        public string? AttachmentFileName { get; set; } = "";
        [NotMapped]
        public string? AttachmentFileStream { get; set; }
        public string? AttachmentFilePath { get; set; }
        [DefaultValue(false)]
        public bool MailSent { get; set; } = false;
        [MaxLength(15)]
        [DefaultValue("New")]
        public string? MailStatus { get; set; } = "New";
        [DefaultValue("getutcdate()")]
        public DateTime? QueueOn { get; set; } = DateTime.Now;
        public DateTime? SentOn { get; set; }
        public string? Remarks { get; set; }

        public object Clone()
        {
            return new Mail
            {
                NotificationID = this.NotificationID,
                ToEmails = this.ToEmails,
                CcEmails = this.CcEmails,
                BccEmails = this.BccEmails,
                Subject = this.Subject,
                Body = this.Body,
                BodyInHTML = this.BodyInHTML,
                AttachmentFileName = this.AttachmentFileName,
                AttachmentFileStream = null,
                AttachmentFilePath = this.AttachmentFilePath,
                MailSent = this.MailSent,
                MailStatus = this.MailStatus,
                QueueOn = this.QueueOn,
                SentOn = this.SentOn,
                Remarks = this.Remarks
    };
        }
    }
}