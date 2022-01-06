using MeowPlanet.MemberManagement;
using MeowPlanet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class MembershipController : Controller
    {
        private Models.MeowContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MembershipController(Models.MeowContext _dbcontext, IWebHostEnvironment _hostEnvironment)
        {
            this._dbcontext = _dbcontext;
            this._webHostEnvironment = _hostEnvironment;
        }


        [Authorize]          //限定已登入的使用者才能瀏覽
        public async Task<IActionResult> Editor()
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);
            var ROLE = claims.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;

            var info = await (from a in _dbcontext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();

            //處理Birthday DateTime轉換
            String Btd = String.Format("{0:yyyy-MM-dd}", info.Birthday);

            ViewData["Account"] = info.Account;
            ViewData["RealName"] = info.RealName;
            ViewData["Gender"] = info.Gender;
            ViewData["Birthday"] = Btd;
            ViewData["Email"] = info.Email;
            ViewData["OrgPwd"] = info.Password;
            ViewData["Phone"] = info.Phone;
            ViewData["Photo"] = info.PersonalPhoto;
            TempData["Job"] = info.Job;
            TempData["Salary"] = info.Salary;
            TempData["AcceptableAmount"] = info.AcceptableAmount;
            TempData["OtherPets"] = info.OtherPets;
            TempData["KeepPets"] = info.KeepPets;
            TempData["Merrage"] = info.Merrage;
            TempData["Agents"] = info.Agents;
            TempData["RelationShip"] = info.RelationShip;
            //計算年齡
            var org = Convert.ToString(info.Birthday);
            var thisYear = (int)DateTime.Now.Year;
            var bth = (int)DateTime.Parse(org).Year;
            TempData["Age"] = thisYear - bth;


            if (ROLE == "2")   //送養者前端頁面顯示貓咪管理
            {
                return RedirectToAction("");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            String root = _webHostEnvironment.ContentRootPath + @"\wwwroot\images\";

            if (file.Length > 0)
            {
                string fileName = file.FileName;
                using (var stream = System.IO.File.Create(root + fileName))
                {
                    await file.CopyToAsync(stream);
                    Console.WriteLine(stream);
                }
                //將圖片寫入資料庫
                if (ModelState.IsValid)    //判斷資料庫是否連接成功
                {
                    try
                    {
                        var user = await _dbcontext.UserDatas.Where(s => s.UserId == ID).FirstOrDefaultAsync();
                        var photoPath = "/images/" + fileName;
                        user.PersonalPhoto = photoPath;
                        _dbcontext.SaveChanges();
                        Console.WriteLine("使用者大頭照更新成功");
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine("錯誤:" + ex);
                    }
                }
            }
            return RedirectToAction("Editor");
        }

        [Route("/Membership/infoedit")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> InfoEdit(IFormCollection info)
        {
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
            else      //資料庫連接成功 把使用者資訊寫入資料庫
            {
                try
                {
                    //將前端回傳的資料轉換為符合資料庫的型別
                    string AC = info["Account"];
                    string REALNAME = info["RealName"];
                    string EMAIL = info["Email"];
                    string GENDER = info["Gender"];
                    string PHONE = info["Phone"];
                    DateTime BTD = Convert.ToDateTime(info["Birthday"]);
                    //將團片檔轉為路徑儲存
                    //var path = UploadPhoto();
                    string FOTO = info["Photo"];


                    //將使用者資訊寫入資料庫

                    //判斷User是否存在
                    var user = await _dbcontext.UserDatas.Where(s => s.Account == AC).FirstOrDefaultAsync();
                    var PWD = user.Password;
                    if (user != null)
                    {
                        user.RealName = REALNAME;
                        user.Email = EMAIL;
                        user.Phone = PHONE;
                        user.Gender = GENDER;
                        user.Birthday = BTD;
                        user.PersonalPhoto = FOTO;
                        _dbcontext.SaveChanges();
                        Console.WriteLine("使用者基本資訊更新成功");
                    }

                    msg = new MemberManagement.Message()
                    {
                        Code = 200,
                        Msg = $"連線成功! 使用者{AC}的資訊更新成功",
                        Time = DateTime.Now
                    };

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
            return msg;
        }
        [Route("/Membership/pwdchange")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> PwdChange(IFormCollection info)
        {
            //將前端回傳的資料轉換為符合資料庫的型別
            string AC = info["Account"];
            string OP = SHAUtility.hashData(info["OrgPwd"]);
            string NP = SHAUtility.hashData(info["NewPwd"]);

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
            else      //資料庫連接成功 把使用者資訊寫入資料庫
            {
                //判斷User是否存在
                var user = await (from a in _dbcontext.UserDatas
                                  where a.Account == AC && a.Password == OP
                                  select a).FirstOrDefaultAsync();
                if (user != null)
                {
                    try
                    {
                        user.Password = NP;

                        //更新使用者資訊到資料庫
                        _dbcontext.SaveChanges();
                        Console.WriteLine($"使用者{AC}的密碼更新成功");
                        msg = new MemberManagement.Message()
                        {
                            Code = 200,
                            Msg = $"連線成功! 使用者{AC}的密碼更新成功",
                            Time = DateTime.Now
                        };
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
                else     //若使用者輸入的密碼與舊密碼不符合
                {
                    Console.WriteLine($"使用者{AC}輸入的密碼與舊密碼不符合");
                    msg = new MemberManagement.Message()
                    {
                        Code = 400,
                        Msg = $"使用者{AC}輸入的密碼與舊密碼不符合",
                        Time = DateTime.Now
                    };
                }
            }
            return msg;
        }

        [Route("/Membership/adpedit")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> AdpEdit(IFormCollection info)
        {
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
            else      //資料庫連接成功 把使用者資訊寫入資料庫
            {
                try
                {
                    //將前端回傳的資料轉換為符合資料庫的型別
                    string AC = info["Account"];
                    string JB = info["Job"];
                    string SA = info["Salary"];
                    string AA = info["AcceptableAmount"];
                    string MG = info["Merrage"];
                    string OP = info["OtherPets"];
                    string KP = info["KeepPets"];
                    string AG = info["Agents"];
                    string RS = info["RelationShip"];


                    //將使用者資訊寫入資料庫

                    //判斷User是否存在
                    var user = await _dbcontext.UserDatas.Where(s => s.Account == AC).FirstOrDefaultAsync();
                    var PWD = user.Password;
                    if (user != null)
                    {
                        user.Job = JB;
                        user.Salary = SA;
                        user.AcceptableAmount = AA;
                        user.Merrage = MG;
                        user.OtherPets = OP;
                        user.KeepPets = KP;
                        user.Agents = AG;
                        user.RelationShip = RS;


                        _dbcontext.SaveChanges();
                        Console.WriteLine("使用者基本資訊更新成功");
                    }

                    msg = new MemberManagement.Message()
                    {
                        Code = 200,
                        Msg = $"連線成功! 使用者{AC}的資訊更新成功",
                        Time = DateTime.Now
                    };

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
            return msg;
        }

        public IActionResult Sender()
        {
            return View();
        }
        public IActionResult SenderConfirm()
        {
            return View();
        }
        public async Task<MemberManagement.Message> AddRole()
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);
            var ROLE = claims.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;

            var user = await (from a in _dbcontext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();

            MemberManagement.Message msg = null;
            try
            {
                //給與帳號角色
                var userId = user.UserId;
                var role = new RoleManagement
                {
                    RoleId = 2,    //給予Sender角色
                    UserId = userId
                };
                _dbcontext.Add(role);
                _dbcontext.SaveChanges();
                Console.WriteLine($"{user.Account}角色新增成功");
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
    }
}
