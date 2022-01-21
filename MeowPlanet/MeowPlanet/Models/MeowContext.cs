using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class MeowContext : DbContext
    {
        public MeowContext()
        {
        }

        public MeowContext(DbContextOptions<MeowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cat> Cats { get; set; }
        public virtual DbSet<ChatList> ChatLists { get; set; }
        public virtual DbSet<CollectionList> CollectionLists { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleManagement> RoleManagements { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<UserData> UserDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MeowContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cat>(entity =>
            {
                entity.ToTable("Cat");

                entity.Property(e => e.CatId)
                    .HasColumnName("Cat_ID")
                    .HasComment("待領養貓編號");

                entity.Property(e => e.Adopt)
                    .HasMaxLength(6)
                    .HasComment("是否已被收養");

                entity.Property(e => e.Adopter)
                    .HasMaxLength(20)
                    .HasComment("收養者編號");

                entity.Property(e => e.Age).HasComment("年齡");

                entity.Property(e => e.CatColor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("毛色");

                entity.Property(e => e.CatGender).HasComment("性別");

                entity.Property(e => e.Sick);

                entity.Property(e => e.Chip).HasComment("晶片");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("市");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("縣");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasComment("照片");
                entity.Property(e => e.Image2);
                entity.Property(e => e.Image3);
                entity.Property(e => e.Image4);

                entity.Property(e => e.IsDeleted);
                entity.Property(e => e.PublishedDay);

                entity.Property(e => e.Ligation).HasComment("結紮");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("名稱");

                entity.Property(e => e.Remark)
                    .HasMaxLength(256)
                    .HasComment("備註");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("使用者編號");

                entity.Property(e => e.Vaccine).HasComment("疫苗");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cats)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cat_UserDatas");
            });

            modelBuilder.Entity<ChatList>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.ToTable("ChatList");

                entity.Property(e => e.MessageId).HasComment("聊天室編號");

                entity.Property(e => e.Image).HasComment("收訊者");

                entity.Property(e => e.Message).HasComment("聊天紀錄");

                entity.Property(e => e.Receiver).HasComment("傳訊者編號");

                entity.Property(e => e.SendTime)
                    .HasColumnType("datetime")
                    .HasComment("傳訊時間");

                entity.Property(e => e.Sender).HasComment("傳訊者編號");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.ChatLists)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatList_UserDatas");
            });

            modelBuilder.Entity<CollectionList>(entity =>
            {
                entity.ToTable("CollectionList");

                entity.Property(e => e.CollectionListId)
                    //.ValueGeneratedNever()
                    .HasColumnName("CollectionList_ID")
                    .HasComment("收藏單編號");

                entity.Property(e => e.CatId)
                    .HasColumnName("Cat_ID")
                    .HasComment("待領養貓編號");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("使用者編號");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.CollectionLists)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionList_Cat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CollectionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionList_UserDatas");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_ID")
                    .HasComment("角色編號");

                entity.Property(e => e.RoleDiscription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("角色說明");
            });

            modelBuilder.Entity<RoleManagement>(entity =>
            {
                entity.ToTable("RoleManagement");

                entity.Property(e => e.RoleManagementId)
                    .HasColumnName("RoleManagement_ID")
                    .HasComment("角色管理編號");

                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_ID")
                    .HasComment("角色編號");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("使用者編號");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleManagements)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleManagement_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoleManagements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleManagement_UserDatas");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.Property(e => e.ScheduleId)
                    .HasColumnName("Schedule_ID")
                    .HasComment("行程表編號");

                entity.Property(e => e.CatId)
                    .HasColumnName("Cat_ID")
                    .HasComment("貓咪編號");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasComment("預約日期");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("使用者編號");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_schedule_Cat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_schedule_UserDatas");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_MemberInformation");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("使用者編號");

                entity.Property(e => e.AcceptableAmount)
                    .HasMaxLength(10)
                    .HasComment("養貓經費");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("帳號");

                entity.Property(e => e.Agents)
                    .HasMaxLength(10)
                    .HasComment("法定代理人");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasComment("生日");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("電子郵件");

                entity.Property(e => e.EmailConfirm)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasComment("性別");

                entity.Property(e => e.Job)
                    .HasMaxLength(15)
                    .HasComment("職業");

                entity.Property(e => e.KeepPets)
                    .HasMaxLength(10)
                    .HasComment("養寵物經驗");

                entity.Property(e => e.Merrage)
                    .HasMaxLength(8)
                    .HasComment("婚姻狀態");

                entity.Property(e => e.OtherPets)
                    .HasMaxLength(10)
                    .HasComment("是否已養寵物");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasComment("密碼");

                entity.Property(e => e.PersonalPhoto).HasComment("個人照片");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsFixedLength(true)
                    .HasComment("連絡電話");

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("真實姓名");

                entity.Property(e => e.RelationShip)
                    .HasMaxLength(18)
                    .HasComment("關係");

                entity.Property(e => e.Salary)
                    .HasMaxLength(10)
                    .HasComment("薪水");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
