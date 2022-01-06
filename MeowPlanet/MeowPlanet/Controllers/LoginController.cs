using MailKit.Net.Smtp;
using MeowPlanet.MemberManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class LoginController : Controller
    {
        private Models.MeowContext _dbcontext;
        public LoginController(Models.MeowContext _dbcontext)
        {
            this._dbcontext = _dbcontext;
        }

        public IActionResult Login()
        {
            return View();
        }

        //確認會員登入資訊
        [Route("/Login/userconfirm")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> UserConfirm(IFormCollection d)
        {
            Console.WriteLine($"Account : {d["Account"]} Password : {d["Password"]}");
            string UID = d["Account"];
            string PWD = SHAUtility.hashData(d["Password"]);

            //判斷使用者輸入的帳號密碼
            var user = await (from a in _dbcontext.UserDatas
                              where a.Account == UID && a.Password == PWD
                              select a).SingleOrDefaultAsync();

            MemberManagement.Message msg = null;

            if (user == null)
            {
                ViewBag.LoginError = "帳號或密碼錯誤";
                Console.WriteLine("帳號或密碼錯誤");
                msg = new MemberManagement.Message()
                {
                    Code = 400,
                    Msg = $"帳號或密碼錯誤",
                    Time = DateTime.Now
                };
                return msg;
            }
            else
            {
                var role = await (from r in _dbcontext.RoleManagements
                                  where r.UserId == user.UserId
                                  select r.RoleId).FirstOrDefaultAsync();

                var claims = new List<Claim>();
                switch (role)
                {
                    case 1:
                        claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Convert.ToString(user.UserId)),
                            new Claim(ClaimTypes.GivenName, user.Account),
                            new Claim(ClaimTypes.Role, "Adopter"),
                            new Claim("FullName", user.RealName),
                        };
                        break;
                    case 2:
                        claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Convert.ToString(user.UserId)),
                            new Claim(ClaimTypes.GivenName, user.Account),
                            new Claim(ClaimTypes.Role, "Adopter"),
                            new Claim(ClaimTypes.Role, "Sender"),
                            new Claim("FullName", user.RealName),
                        };
                        break;
                }
                // using Microsoft.AspNetCore.Authentication;
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)    //設定登入時限為20分鐘
                    });

                Console.WriteLine("會員登入成功");
                ViewBag.Message = "會員登入成功";
                msg = new MemberManagement.Message()
                {
                    Code = 200,
                    Msg = $"{UID}會員登入成功",
                    Time = DateTime.Now
                };

                return msg;          //換頁Redirection由前端處理
            }
        }

        //產生隨機碼
        public static string GetRandomStringByFileName(int length)
        {
            var str = Path.GetRandomFileName().Replace(".", "");
            return str.Substring(0, length);
        }

        //忘記密碼
        [Route("/Login/forgetpassword")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> ForgetPwd(IFormCollection d)
        {
            string UID = d["Account"];
            var user = await (from a in _dbcontext.UserDatas
                              where a.Account == UID
                              select a).SingleOrDefaultAsync();
            var Account = user.Account;
            var Mail = user.Email;
            var Name = user.RealName;
            var Password = GetRandomStringByFileName(10);

            var msg = new MemberManagement.Message();
            if (!ModelState.IsValid)                    //判斷資料庫是否連接成功
            {
                msg = new MemberManagement.Message()
                {
                    Code = 500,
                    Msg = $"資料庫連線異常",
                    Time = DateTime.Now
                };
            }
            else                              //資料庫連接成功 將使用者的密碼換為隨機碼
            {
                try
                {
                    if (user != null)
                    {
                        user.Password = SHAUtility.hashData(Password);

                        _dbcontext.SaveChanges();
                        Console.WriteLine("使用者密碼更新成功");
                        msg = new MemberManagement.Message()
                        {
                            Code = 200,
                            Msg = $"連線成功! 使用者{Account}的資訊更新成功",
                            Time = DateTime.Now
                        };
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("錯誤:" + ex);
                    msg = new MemberManagement.Message()
                    {
                        Code = 400,
                        Msg = $"資料更新失敗",
                        Time = DateTime.Now
                    };
                }
            }
            SendPwd(Account, Mail, Name, Password);
            return msg;
        }

        //寄送驗證信給新註冊的使用者
        public void SendPwd(string Account, string Email, string Name, string Password)
        {
            MimeMessage message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(Email));
            message.From.Add(new MailboxAddress("喵星球", "eishinchen@gmail.com"));

            message.Subject = $"Hello {Name}, 為您送上登入星球的密碼";
            var builder = new BodyBuilder();

            builder.TextBody = (@$"嗨! {Name},
為您送上登入星球的密碼
請使用此密碼登入星球{Password}
提醒您登入後請盡速更換密碼!");

            builder.HtmlBody = string.Format(@$"<p>嗨! {Name},<br>
<p>為您送上登入星球的密碼</p>
<p>請使用此密碼登入星球 {Password}</p>
<p>提醒您登入後請盡速更換密碼</p>
");

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

        //會員登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
