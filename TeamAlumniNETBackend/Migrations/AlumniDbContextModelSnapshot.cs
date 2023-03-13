﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamAlumniNETBackend.Data;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    [DbContext(typeof(AlumniDbContext))]
    partial class AlumniDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EventGroup", b =>
                {
                    b.Property<int>("EventsEventId")
                        .HasColumnType("int");

                    b.Property<int>("GroupsGroupId")
                        .HasColumnType("int");

                    b.HasKey("EventsEventId", "GroupsGroupId");

                    b.HasIndex("GroupsGroupId");

                    b.ToTable("EventGroup");
                });

            modelBuilder.Entity("EventTopic", b =>
                {
                    b.Property<int>("EventsEventId")
                        .HasColumnType("int");

                    b.Property<int>("TopicsTopicId")
                        .HasColumnType("int");

                    b.HasKey("EventsEventId", "TopicsTopicId");

                    b.HasIndex("TopicsTopicId");

                    b.ToTable("EventTopic");
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.Property<int>("EventsEventId")
                        .HasColumnType("int");

                    b.Property<Guid>("UsersUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EventsEventId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("EventUser");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<int>("GroupsGroupId")
                        .HasColumnType("int");

                    b.Property<Guid>("UsersUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GroupsGroupId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EventId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            Description = "Football game"
                        });
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"), 1L, 1);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            GroupId = 1,
                            Description = "Group for members of Experis",
                            Name = "Experis"
                        });
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"), 1L, 1);

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TargetEvent")
                        .HasColumnType("int");

                    b.Property<int?>("TargetGroup")
                        .HasColumnType("int");

                    b.Property<int?>("TargetPost")
                        .HasColumnType("int");

                    b.Property<int?>("TargetTopic")
                        .HasColumnType("int");

                    b.Property<Guid?>("TargetUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            PostId = 1,
                            Body = "Invite to all who like football to watch the match",
                            LastUpdate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Footbal Match"
                        });
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Rsvp", b =>
                {
                    b.Property<int>("RsvpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RsvpId"), 1L, 1);

                    b.Property<bool>("Accepted")
                        .HasColumnType("bit");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<int?>("GuestCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RsvpId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Rsvps");
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicId");

                    b.ToTable("Topics");

                    b.HasData(
                        new
                        {
                            TopicId = 1,
                            Description = "Topic for people who love football",
                            Name = "Football"
                        });
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FunFact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("ee1b03f4-ac5f-42d8-9562-4fc82e55197b"),
                            Bio = "Love Futsal and footbal",
                            FunFact = "never watched a whole footballmatch",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg",
                            Status = "Attending Experis Academy courses at Noroff",
                            UserName = "Richardinho"
                        },
                        new
                        {
                            UserId = new Guid("14d229c9-5428-4456-8b54-8e9646103dfc"),
                            Bio = "Striker at Manchester United",
                            FunFact = "Almost never score",
                            Image = "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620",
                            Status = "Attending Experis Academy courses at Noroff",
                            UserName = "Kjetilinho"
                        });
                });

            modelBuilder.Entity("TopicUser", b =>
                {
                    b.Property<int>("TopicsTopicId")
                        .HasColumnType("int");

                    b.Property<Guid>("UsersUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TopicsTopicId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("TopicUser");
                });

            modelBuilder.Entity("EventGroup", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamAlumniNETBackend.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventTopic", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamAlumniNETBackend.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicsTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamAlumniNETBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamAlumniNETBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Post", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.User", null)
                        .WithMany("Posts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Rsvp", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Event", "Event")
                        .WithMany("Rsvps")
                        .HasForeignKey("EventId");

                    b.HasOne("TeamAlumniNETBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TopicUser", b =>
                {
                    b.HasOne("TeamAlumniNETBackend.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicsTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamAlumniNETBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.Event", b =>
                {
                    b.Navigation("Rsvps");
                });

            modelBuilder.Entity("TeamAlumniNETBackend.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
