using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailNotifications.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToEmails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CcEmails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BccEmails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyInHTML = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailSent = table.Column<bool>(type: "bit", nullable: false),
                    MailStatus = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    QueueOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.NotificationID);
                });

            migrationBuilder.InsertData(
                table: "Mails",
                columns: new[] { "NotificationID", "AttachmentFileName", "AttachmentFilePath", "BccEmails", "Body", "BodyInHTML", "CcEmails", "MailSent", "MailStatus", "QueueOn", "Remarks", "SentOn", "Subject", "ToEmails" },
                values: new object[] { -1, null, null, null, "Test Mail Body", false, null, false, "New", new DateTime(2022, 9, 11, 15, 40, 8, 136, DateTimeKind.Local).AddTicks(3298), null, null, "Test Mail Subject", "madhura@gmail.com;indu@email.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mails");
        }
    }
}
