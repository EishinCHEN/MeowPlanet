using MailKit.Net.Smtp;
using MeowPlanet.MemberManagement;
using MeowPlanet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class RegisterController : Controller
    {
        private Models.MeowContext _dbcontext;
        public RegisterController(Models.MeowContext _dbcontext)
        {
            this._dbcontext = _dbcontext;
        }
        public IActionResult Register()
        {
            return View();
        }

        //寫入使用者註冊資訊
        [Route("/Register/enrollUser")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> UserEnroll(IFormCollection userData)
        {
            Console.WriteLine("Account:" + userData["Account"]);

            MemberManagement.Message msg = null;
            if (!ModelState.IsValid)    //判斷資料庫是否連接成功
            {
                msg = new MemberManagement.Message()
                {
                    Code = 500,
                    Msg = $"資料庫連線異常",
                    Time = DateTime.Now
                };
            }
            else      //資料庫連接成功 把使用者資訊寫入資料庫並寄送驗證email
            {
                //將前端回傳的資料轉換為符合資料庫的型別
                string AC = userData["Account"];
                string REALNAME = userData["RealName"];
                string PWD = SHAUtility.hashData(userData["Password"]);
                string EMAIL = userData["Email"];
                string GENDER = userData["Gender"];
                string PHONE = userData["Phone"];
                DateTime BTD = Convert.ToDateTime(userData["Birthday"]);


                var user = await _dbcontext.UserDatas.Where(s => s.Account == AC).SingleOrDefaultAsync();

                //將使用者資訊寫入資料庫
                //判斷UserId是否已經被使用

                if (user != null)
                {
                    Console.WriteLine("此帳號已註冊");
                }
                else
                {
                    try
                    {
                        var userdata = new UserData
                        {
                            Account = AC,
                            RealName = REALNAME,
                            Password = PWD,
                            Email = EMAIL,
                            EmailConfirm = "false",
                            Gender = GENDER,
                            Phone = PHONE,
                            Birthday = BTD,
                            PersonalPhoto = "/images/預設大頭貼.png"
                        };
                        _dbcontext.UserDatas.Add(userdata);
                        _dbcontext.SaveChanges();
                        Console.WriteLine($"{AC}帳號註冊成功");

                        msg = new MemberManagement.Message()
                        {
                            Code = 200,
                            Msg = $"連線成功! 使用者{AC}帳號註冊成功",
                            Time = DateTime.Now
                        };

                        //呼叫SendEmail Action 並帶入使用者資訊
                        SendMail(AC, EMAIL, REALNAME);
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine("錯誤:" + ex);
                        msg = new MemberManagement.Message()
                        {
                            Code = 400,
                            Msg = $"資料更新失敗" + ex,
                            Time = DateTime.Now
                        };
                    }
                }
                await AddRole(AC);  //為user添加領養人角色
            }

            return msg;
            //return RedirectToAction("Index","Home");    //註冊成功後預設導回首頁
        }

        //TODO設定使用者角色
        public async Task<MemberManagement.Message> AddRole(string AC)
        {
            var user = await _dbcontext.UserDatas.Where(s => s.Account == AC).SingleOrDefaultAsync();
            MemberManagement.Message msg = null;
            try
            {
                //給與帳號角色
                var userId = user.UserId;
                var role = new RoleManagement
                {
                    RoleId = 1,    //預設給予Adopter角色
                    UserId = userId
                };
                _dbcontext.Add(role);
                _dbcontext.SaveChanges();
                Console.WriteLine($"{AC}角色新增成功");
                msg = new MemberManagement.Message()
                {
                    Code = 200,
                    Msg = $"領養人角色新增成功",
                    Time = DateTime.Now
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("錯誤:" + ex);
                msg = new MemberManagement.Message()
                {
                    Code = 400,
                    Msg = $"領養人角色添加失敗" + ex,
                    Time = DateTime.Now
                };
            }
            return msg;
        }

        //寄送驗證信給新註冊的使用者
        public void SendMail(string Account, string Email, string Name)
        {
            MimeMessage message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(Email));
            message.From.Add(new MailboxAddress("喵星球", "eishinchen@gmail.com"));

            message.Subject = $"Hello {Name}, 歡迎登入喵星球";
            var builder = new BodyBuilder();

            builder.TextBody = (@$"嗨! {Name},
歡迎登入喵星球
請點擊連結來驗證您的信箱
驗證我的信箱");

            builder.HtmlBody = string.Format(@$"<p>嗨! {Name},<br>
<p>歡迎登入喵星球</p>
<p>請點擊連結來驗證您的信箱</p>
<a href='https://localhost:5001/Register/MailValidation/{Account}'>驗證我的信箱</a>");

            message.Body = builder.ToMessageBody();
            string eamilAddress = "eishinchen@gmail.com";
            string password = "N@nam!1997";

            //取得SmtpClient
            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 587, false);
                //client.Connect("smtp.gmail.com", 465, true);
                //client.Connect("localhost", 25, MailKit.Security.SecureSocketOptions.None);
                //client.Connect("localhost", 25, MailKit.Security.SecureSocketOptions.Auto);
                //client.Connect ("localhost", 25);
                client.Authenticate(eamilAddress, password);
                client.Send(message);

                Console.WriteLine("Email Sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        //接收驗證信回傳的user資訊 更新資料庫EmailConfirm欄位
        [Route("/Register/MailValidation/{Account}")]
        [HttpGet("{Account}")]
        public IActionResult MailValidation([FromRoute(Name = "Account")] string Account)
        {

            //Console.WriteLine("userid=" + userId);
            var user = _dbcontext.UserDatas.Where(u => u.Account == Account && u.EmailConfirm == "false").FirstOrDefault();
            if (user != null)
            {
                try
                {
                    user.EmailConfirm = "true";
                    _dbcontext.SaveChanges();
                    Console.WriteLine("成功更新EmailConfirm欄位");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("更新資料庫中EmailConfirm時發生錯誤:" + ex);
                }
            }
            ViewBag.Message = "Email驗證成功!請重新登入開始喵星旅行!";
            return RedirectToAction("Login", "Login");          //打開連結驗證信箱 並自動導向登入頁面
        }
    }
}
