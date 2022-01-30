using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeowPlanet.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_ID = table.Column<int>(type: "int", nullable: false, comment: "角色編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleDiscription = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "角色說明")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Role_ID);
                });

            migrationBuilder.CreateTable(
                name: "UserDatas",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false, comment: "使用者編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "帳號"),
                    RealName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "真實姓名"),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "密碼"),
                    EmailConfirm = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "電子郵件"),
                    Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "性別"),
                    Phone = table.Column<string>(type: "nchar(12)", fixedLength: true, maxLength: 12, nullable: false, comment: "連絡電話"),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false, comment: "生日"),
                    Job = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true, comment: "職業"),
                    Salary = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "薪水"),
                    AcceptableAmount = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "養貓經費"),
                    Merrage = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true, comment: "婚姻狀態"),
                    OtherPets = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "是否已養寵物"),
                    KeepPets = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "養寵物經驗"),
                    Agents = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "法定代理人"),
                    RelationShip = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true, comment: "關係"),
                    PersonalPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "個人照片")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInformation", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Cat",
                columns: table => new
                {
                    Cat_ID = table.Column<int>(type: "int", nullable: false, comment: "待領養貓編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adopt = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true, comment: "是否已被收養"),
                    Adopter = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "收養者編號"),
                    User_ID = table.Column<int>(type: "int", nullable: false, comment: "使用者編號"),
                    CatColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "毛色"),
                    Ligation = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "結紮"),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "年齡"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "名稱"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "照片"),
                    Image2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatGender = table.Column<int>(type: "int", nullable: true, comment: "性別"),
                    Vaccine = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "疫苗"),
                    Chip = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "晶片"),
                    Sick = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "備註"),
                    Country = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "縣"),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "市"),
                    IsDeleted = table.Column<int>(type: "int", nullable: true),
                    PublishedDay = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat", x => x.Cat_ID);
                    table.ForeignKey(
                        name: "FK_Cat_UserDatas",
                        column: x => x.User_ID,
                        principalTable: "UserDatas",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatList",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false, comment: "聊天室編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<int>(type: "int", nullable: false, comment: "傳訊者編號"),
                    Receiver = table.Column<int>(type: "int", nullable: false, comment: "傳訊者編號"),
                    SendTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "傳訊時間"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "聊天紀錄"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "收訊者"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatList", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_ChatList_UserDatas",
                        column: x => x.Sender,
                        principalTable: "UserDatas",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleManagement",
                columns: table => new
                {
                    RoleManagement_ID = table.Column<int>(type: "int", nullable: false, comment: "角色管理編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_ID = table.Column<int>(type: "int", nullable: false, comment: "角色編號"),
                    User_ID = table.Column<int>(type: "int", nullable: false, comment: "使用者編號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleManagement", x => x.RoleManagement_ID);
                    table.ForeignKey(
                        name: "FK_RoleManagement_Role",
                        column: x => x.Role_ID,
                        principalTable: "Role",
                        principalColumn: "Role_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleManagement_UserDatas",
                        column: x => x.User_ID,
                        principalTable: "UserDatas",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionList",
                columns: table => new
                {
                    CollectionList_ID = table.Column<int>(type: "int", nullable: false, comment: "收藏單編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false, comment: "使用者編號"),
                    Cat_ID = table.Column<int>(type: "int", nullable: false, comment: "待領養貓編號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionList", x => x.CollectionList_ID);
                    table.ForeignKey(
                        name: "FK_CollectionList_Cat",
                        column: x => x.Cat_ID,
                        principalTable: "Cat",
                        principalColumn: "Cat_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionList_UserDatas",
                        column: x => x.User_ID,
                        principalTable: "UserDatas",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    Schedule_ID = table.Column<int>(type: "int", nullable: false, comment: "行程表編號"),
                    User_ID = table.Column<int>(type: "int", nullable: false, comment: "使用者編號"),
                    Cat_ID = table.Column<int>(type: "int", nullable: false, comment: "貓咪編號"),
                    Date = table.Column<DateTime>(type: "date", nullable: false, comment: "預約日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule", x => x.Schedule_ID);
                    table.ForeignKey(
                        name: "FK_schedule_Cat",
                        column: x => x.Cat_ID,
                        principalTable: "Cat",
                        principalColumn: "Cat_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_schedule_UserDatas",
                        column: x => x.User_ID,
                        principalTable: "UserDatas",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cat_User_ID",
                table: "Cat",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatList_Sender",
                table: "ChatList",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionList_Cat_ID",
                table: "CollectionList",
                column: "Cat_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionList_User_ID",
                table: "CollectionList",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleManagement_Role_ID",
                table: "RoleManagement",
                column: "Role_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleManagement_User_ID",
                table: "RoleManagement",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_Cat_ID",
                table: "schedule",
                column: "Cat_ID");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_User_ID",
                table: "schedule",
                column: "User_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatList");

            migrationBuilder.DropTable(
                name: "CollectionList");

            migrationBuilder.DropTable(
                name: "RoleManagement");

            migrationBuilder.DropTable(
                name: "schedule");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Cat");

            migrationBuilder.DropTable(
                name: "UserDatas");
        }
    }
}
