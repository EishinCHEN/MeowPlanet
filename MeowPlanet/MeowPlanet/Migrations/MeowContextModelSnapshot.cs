﻿// <auto-generated />
using System;
using MeowPlanet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeowPlanet.Migrations
{
    [DbContext(typeof(MeowContext))]
    partial class MeowContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MeowPlanet.Models.Cat", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Cat_ID")
                        .HasComment("待領養貓編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adopt")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)")
                        .HasComment("是否已被收養");

                    b.Property<string>("Adopter")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("收養者編號");

                    b.Property<string>("Age")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("年齡");

                    b.Property<string>("CatColor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("毛色");

                    b.Property<int?>("CatGender")
                        .HasColumnType("int")
                        .HasComment("性別");

                    b.Property<string>("Chip")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("晶片");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("市");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("縣");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("照片");

                    b.Property<string>("Image2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("Ligation")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("結紮");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("名稱");

                    b.Property<DateTime?>("PublishedDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remark")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasComment("備註");

                    b.Property<string>("Sick")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_ID")
                        .HasComment("使用者編號");

                    b.Property<string>("Vaccine")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("疫苗");

                    b.HasKey("CatId");

                    b.HasIndex("UserId");

                    b.ToTable("Cat");
                });

            modelBuilder.Entity("MeowPlanet.Models.ChatList", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("聊天室編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("收訊者");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("聊天紀錄");

                    b.Property<int>("Receiver")
                        .HasColumnType("int")
                        .HasComment("傳訊者編號");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime")
                        .HasComment("傳訊時間");

                    b.Property<int>("Sender")
                        .HasColumnType("int")
                        .HasComment("傳訊者編號");

                    b.HasKey("MessageId");

                    b.HasIndex("Sender");

                    b.ToTable("ChatList");
                });

            modelBuilder.Entity("MeowPlanet.Models.CollectionList", b =>
                {
                    b.Property<int>("CollectionListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CollectionList_ID")
                        .HasComment("收藏單編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatId")
                        .HasColumnType("int")
                        .HasColumnName("Cat_ID")
                        .HasComment("待領養貓編號");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_ID")
                        .HasComment("使用者編號");

                    b.HasKey("CollectionListId");

                    b.HasIndex("CatId");

                    b.HasIndex("UserId");

                    b.ToTable("CollectionList");
                });

            modelBuilder.Entity("MeowPlanet.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Role_ID")
                        .HasComment("角色編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleDiscription")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("角色說明");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("MeowPlanet.Models.RoleManagement", b =>
                {
                    b.Property<int>("RoleManagementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RoleManagement_ID")
                        .HasComment("角色管理編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("Role_ID")
                        .HasComment("角色編號");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_ID")
                        .HasComment("使用者編號");

                    b.HasKey("RoleManagementId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleManagement");
                });

            modelBuilder.Entity("MeowPlanet.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .HasColumnType("int")
                        .HasColumnName("Schedule_ID")
                        .HasComment("行程表編號");

                    b.Property<int>("CatId")
                        .HasColumnType("int")
                        .HasColumnName("Cat_ID")
                        .HasComment("貓咪編號");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasComment("預約日期");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_ID")
                        .HasComment("使用者編號");

                    b.HasKey("ScheduleId");

                    b.HasIndex("CatId");

                    b.HasIndex("UserId");

                    b.ToTable("schedule");
                });

            modelBuilder.Entity("MeowPlanet.Models.UserData", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("User_ID")
                        .HasComment("使用者編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AcceptableAmount")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("養貓經費");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("帳號");

                    b.Property<string>("Agents")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("法定代理人");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date")
                        .HasComment("生日");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("電子郵件");

                    b.Property<string>("EmailConfirm")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)")
                        .HasComment("性別");

                    b.Property<string>("Job")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasComment("職業");

                    b.Property<string>("KeepPets")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("養寵物經驗");

                    b.Property<string>("Merrage")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)")
                        .HasComment("婚姻狀態");

                    b.Property<string>("OtherPets")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("是否已養寵物");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasComment("密碼");

                    b.Property<string>("PersonalPhoto")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("個人照片");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nchar(12)")
                        .IsFixedLength(true)
                        .HasComment("連絡電話");

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("真實姓名");

                    b.Property<string>("RelationShip")
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)")
                        .HasComment("關係");

                    b.Property<string>("Salary")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("薪水");

                    b.HasKey("UserId")
                        .HasName("PK_MemberInformation");

                    b.ToTable("UserDatas");
                });

            modelBuilder.Entity("MeowPlanet.Models.Cat", b =>
                {
                    b.HasOne("MeowPlanet.Models.UserData", "User")
                        .WithMany("Cats")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Cat_UserDatas")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeowPlanet.Models.ChatList", b =>
                {
                    b.HasOne("MeowPlanet.Models.UserData", "SenderNavigation")
                        .WithMany("ChatLists")
                        .HasForeignKey("Sender")
                        .HasConstraintName("FK_ChatList_UserDatas")
                        .IsRequired();

                    b.Navigation("SenderNavigation");
                });

            modelBuilder.Entity("MeowPlanet.Models.CollectionList", b =>
                {
                    b.HasOne("MeowPlanet.Models.Cat", "Cat")
                        .WithMany("CollectionLists")
                        .HasForeignKey("CatId")
                        .HasConstraintName("FK_CollectionList_Cat")
                        .IsRequired();

                    b.HasOne("MeowPlanet.Models.UserData", "User")
                        .WithMany("CollectionLists")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_CollectionList_UserDatas")
                        .IsRequired();

                    b.Navigation("Cat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeowPlanet.Models.RoleManagement", b =>
                {
                    b.HasOne("MeowPlanet.Models.Role", "Role")
                        .WithMany("RoleManagements")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_RoleManagement_Role")
                        .IsRequired();

                    b.HasOne("MeowPlanet.Models.UserData", "User")
                        .WithMany("RoleManagements")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_RoleManagement_UserDatas")
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeowPlanet.Models.Schedule", b =>
                {
                    b.HasOne("MeowPlanet.Models.Cat", "Cat")
                        .WithMany("Schedules")
                        .HasForeignKey("CatId")
                        .HasConstraintName("FK_schedule_Cat")
                        .IsRequired();

                    b.HasOne("MeowPlanet.Models.UserData", "User")
                        .WithMany("Schedules")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_schedule_UserDatas")
                        .IsRequired();

                    b.Navigation("Cat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeowPlanet.Models.Cat", b =>
                {
                    b.Navigation("CollectionLists");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("MeowPlanet.Models.Role", b =>
                {
                    b.Navigation("RoleManagements");
                });

            modelBuilder.Entity("MeowPlanet.Models.UserData", b =>
                {
                    b.Navigation("Cats");

                    b.Navigation("ChatLists");

                    b.Navigation("CollectionLists");

                    b.Navigation("RoleManagements");

                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
