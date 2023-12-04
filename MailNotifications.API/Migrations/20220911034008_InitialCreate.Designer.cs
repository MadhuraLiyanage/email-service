﻿// <auto-generated />
using System;
using MailNotifications.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MailNotifications.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220911034008_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MailNotifications.Models.Mail", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"), 1L, 1);

                    b.Property<string>("AttachmentFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AttachmentFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BccEmails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("BodyInHTML")
                        .HasColumnType("bit");

                    b.Property<string>("CcEmails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MailSent")
                        .HasColumnType("bit");

                    b.Property<string>("MailStatus")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime?>("QueueOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SentOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToEmails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationID");

                    b.ToTable("Mails");

                    b.HasData(
                        new
                        {
                            NotificationID = -1,
                            Body = "Test Mail Body",
                            BodyInHTML = false,
                            MailSent = false,
                            MailStatus = "New",
                            QueueOn = new DateTime(2022, 9, 11, 15, 40, 8, 136, DateTimeKind.Local).AddTicks(3298),
                            Subject = "Test Mail Subject",
                            ToEmails = "madhura@gmail.com;indu@gmail.com"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
