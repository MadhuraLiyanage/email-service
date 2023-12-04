using MailNotifications.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MailNotifications.API.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Mail> Mails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed Mail table for testing
            modelBuilder.Entity<Mail>().Property(m => m.NotificationID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Mail>().HasData(
                new Mail
                {
                    NotificationID = -1,
                    ToEmails = "madhura@gmail.com;indu@ghmail.com",
                    CcEmails = null,
                    BccEmails = null,
                    Subject = "Test Mail Subject",
                    Body = "Test Mail Body",
                    BodyInHTML = false,
                    AttachmentFileName = null,
                    AttachmentFileStream = null,
                    AttachmentFilePath = null,
                    MailSent = false,
                    MailStatus = "New"
                });
            /*
             *  database migration
                Command : Add-Migration InitialCreate
                
                Updating the Database
                Command : Update-Database
            */
        }

    }
}
