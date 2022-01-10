using MeowPlanet.Models;
using MeowPlanet.ViewModels;
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
    [Authorize(Roles = "CatSender")]
    public class CatManagementController : Controller
    {
        private readonly Models.MeowContext _dbcontext;
        private readonly IWebHostEnvironment _env;
        public CatManagementController(Models.MeowContext dbcontext, IWebHostEnvironment env)
        {
            this._dbcontext = dbcontext;
            this._env = env;
        }
        //顯示沒有貓咪的畫面(新增角色後的畫面)
        public IActionResult AddCat()
        {
            return View();
        }
        
        //顯示上傳貓咪的畫面
        public IActionResult UploadCat()
        {
            TempData["Photo1"] = "/images/我是誰(小問號).PNG";
            return View();
        }

        //顯示管理貓咪的畫面
        public async Task<IActionResult> ManageCat()
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = await(from a in _dbcontext.Cats
                            where a.UserId == userId  && a.IsDeleted == 1
                            select a).ToListAsync();
            if (cat == null)
            {
                return View("AddCat");
            }
            else
            {
                return View();
            }
        }

        //上傳貓咪資訊
        [HttpPost]
        public async Task<IActionResult> UploadConfirm(IFormFileCollection files, CatViewModel model)
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            var user = await (from a in _dbcontext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();

            MemberManagement.Message msg = null;
            if (user != null)
            {
                if (!ModelState.IsValid)
                {
                    msg = new MemberManagement.Message()
                    {
                        Code = 500,
                        Msg = $"資料庫連線異常",
                        Time = DateTime.Now
                    };
                }
                else
                {
                    String root = _env.ContentRootPath + @"\wwwroot\images\";

                    model.Image = new List<String>();
                    var sum = files.Sum(f => f.Length);
                    if (files == null || sum == 0)
                    {
                        msg = new MemberManagement.Message()
                        {
                            Code = 400,
                            Msg = $"請上傳圖片",
                            Time = DateTime.Now
                        };
                        return View("UploadCat");
                    }
                    else
                    {
                        foreach (IFormFile file in files)
                        {
                            string fileName = file.FileName;
                            using (var stream = System.IO.File.Create(root + fileName))
                            {
                                await file.CopyToAsync(stream);
                                model.Image.Add("/images/" + fileName);
                            }
                        }
                        //List<String> to string
                        //var result = String.Join(", ", model.Image.ToArray());
                        try
                        {
                            var cat = new Cat
                            {
                                UserId = ID,
                                Image = model.Image[0],
                                Image2 = model.Image[1],
                                Image3 = model.Image[2],
                                Image4 = model.Image[3],
                                Name = model.Name,
                                Country = model.Country,
                                City = model.City,
                                CatColor = model.CatColor,
                                CatGender = Convert.ToInt32(model.CatGender),
                                Age = model.Age,
                                Ligation = model.Ligation,
                                Vaccine = model.Vaccine,
                                Chip = model.Chip,
                                Sick = model.Sick,
                                Remark = model.Remark,
                                PublishedDay = DateTime.Now,
                                IsDeleted = 1
                            };
                            _dbcontext.Cats.Add(cat);
                            _dbcontext.SaveChanges();
                            Console.WriteLine("貓咪資訊新增成功");

                            msg = new MemberManagement.Message()
                            {
                                Code = 200,
                                Msg = $"連線成功! 貓咪{model.Name}新增成功",
                                Time = DateTime.Now
                            };
                        }
                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine("錯誤:" + ex);
                            msg = new MemberManagement.Message()
                            {
                                Code = 400,
                                Msg = $"資料新增失敗" + ex,
                                Time = DateTime.Now
                            };
                        }
                    }

                }
            }
            return View("ManageCat");
        }

        //取得個人貓咪資訊
        [HttpGet]
        public List<Cat> GetPersonalCatInfo()
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = (from a in _dbcontext.Cats
                            where a.UserId == userId && a.IsDeleted == 1
                            select a).ToList();

            List<Cat> cats = cat; 


            return cats;
        }
        
        //編輯貓咪畫面
        [HttpGet("{catId}")]
        [Route("/CatManagement/EditCat/{catId}")]
        public async Task<IActionResult> EditCat([FromRoute(Name = "catId")] int catId)
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = await (from a in _dbcontext.Cats
                             where a.CatId == catId && a.UserId == userId
                             select a).FirstOrDefaultAsync();

            
            if (cat != null)
            {
                
                String PD = String.Format("{0:yyyy-MM-dd}", cat.PublishedDay);

                ViewData["CatId"] = cat.CatId;
                ViewData["Name"] = cat.Name;
                ViewData["Image"] = cat.Image;
                ViewData["Image2"] = cat.Image2;
                ViewData["Image3"] = cat.Image3;
                ViewData["Image4"] = cat.Image4;
                ViewData["CatGender"] = cat.CatGender;
                ViewData["CatColor"] = cat.CatColor;
                ViewData["Age"] = cat.Age;
                ViewData["Vaccine"] = cat.Vaccine;
                ViewData["Ligation"] = cat.Ligation;
                ViewData["City"] = cat.City;
                ViewData["Country"] = cat.Country;
                ViewData["PublishedDay"] = PD;
                ViewData["Chip"] = cat.Chip;
                ViewData["Sick"] = cat.Sick;
                ViewData["Remark"] = cat.Remark;

            }            
            return View();
        }

        //確認修改貓咪
        [HttpPost]
        public async Task<IActionResult> EditConfirm(IFormFileCollection files, CatViewModel model)
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            var cat = await (from a in _dbcontext.Cats
                              where a.UserId == ID && a.CatId == model.CatId
                              select a).FirstOrDefaultAsync();

            MemberManagement.Message msg = null;
            if (cat != null)
            {
                if (!ModelState.IsValid)
                {
                    msg = new MemberManagement.Message()
                    {
                        Code = 500,
                        Msg = $"資料庫連線異常",
                        Time = DateTime.Now
                    };
                }
                else
                {
                    String root = _env.ContentRootPath + @"\wwwroot\images\";

                    model.Image = new List<String>();
                    var sum = files.Sum(f => f.Length);
                    if (files == null || sum == 0)
                    {
                        msg = new MemberManagement.Message()
                        {
                            Code = 400,
                            Msg = $"請上傳圖片",
                            Time = DateTime.Now
                        };
                        return View("UploadCat");
                    }
                    else
                    {
                        foreach (IFormFile file in files)
                        {
                            string fileName = file.FileName;
                            using (var stream = System.IO.File.Create(root + fileName))
                            {
                                await file.CopyToAsync(stream);
                                model.Image.Add("/images/" + fileName);
                            }
                        }
                        //List<String> to string
                        //var result = String.Join(", ", model.Image.ToArray());
                        try
                        {
                            cat.Image = model.Image[0];
                            cat.Image2 = model.Image[1];
                            cat.Image3 = model.Image[2];
                            cat.Image4 = model.Image[3];
                            cat.Name = model.Name;
                            cat.Country = model.Country;
                            cat.City = model.City;
                            cat.CatColor = model.CatColor;
                            cat.CatGender = Convert.ToInt32(model.CatGender);
                            cat.Age = model.Age;
                            cat.Ligation = model.Ligation;
                            cat.Vaccine = model.Vaccine;
                            cat.Chip = model.Chip;
                            cat.Sick = model.Sick;
                            cat.Remark = model.Remark;
                            cat.PublishedDay = DateTime.Now;
                            
                            _dbcontext.SaveChanges();
                            Console.WriteLine("貓咪資訊修改成功");

                            msg = new MemberManagement.Message()
                            {
                                Code = 200,
                                Msg = $"連線成功! 貓咪{model.Name}修改成功",
                                Time = DateTime.Now
                            };
                        }
                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine("錯誤:" + ex);
                            msg = new MemberManagement.Message()
                            {
                                Code = 400,
                                Msg = $"資料修改失敗" + ex,
                                Time = DateTime.Now
                            };
                        }
                    }

                }
            }
            return View("ManageCat");
        }

        [HttpGet("{catId}")]
        [Route("/CatManagement/DeleteCat/{catId}")]
        public async Task<MemberManagement.Message> DeleteCat([FromRoute(Name = "catId")] int catId)
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = await (from a in _dbcontext.Cats
                             where a.UserId == userId && a.CatId == catId
                             select a).FirstOrDefaultAsync();
            MemberManagement.Message msg = null;

            if (cat != null)
            {
                try
                {
                    cat.IsDeleted = 2;

                    _dbcontext.SaveChanges();
                    Console.WriteLine($"喵星人:{cat.Name}刪除成功");

                    msg = new MemberManagement.Message()
                    {
                        Code = 200,
                        Msg = $"喵星人:{cat.Name}刪除成功",
                        Time = DateTime.Now
                    };

                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"喵星人:{cat.Name}刪除失敗!" + ex);
                    msg = new MemberManagement.Message()
                    {
                        Code = 400,
                        Msg = $"喵星人:{cat.Name}刪除失敗"+ex,
                        Time = DateTime.Now
                    };
                }
            }
            else
            {
                Console.WriteLine($"刪除失敗!請提供喵星人資訊");
                msg = new MemberManagement.Message()
                {
                    Code = 400,
                    Msg = $"刪除失敗!請提供喵星人資訊",
                    Time = DateTime.Now
                };
            }
            return msg;
        }

        [HttpGet("{catId}")]
        [Route("/CatManagement/SendCat/{catId}")]
        public async Task<MemberManagement.Message> SendCat([FromRoute(Name = "catId")] int catId)
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = await (from a in _dbcontext.Cats
                             where a.UserId == userId && a.CatId == catId
                             select a).FirstOrDefaultAsync();
            MemberManagement.Message msg = null;

            if (cat != null)
            {
                try
                {
                    cat.Adopt = "已送養";

                    _dbcontext.SaveChanges();
                    Console.WriteLine($"喵星人:{cat.Name}回報送養成功");

                    msg = new MemberManagement.Message()
                    {
                        Code = 200,
                        Msg = $"喵星人:{cat.Name}回報送養成功",
                        Time = DateTime.Now
                    };

                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"喵星人:{cat.Name}回報送養失敗" + ex);
                    msg = new MemberManagement.Message()
                    {
                        Code = 400,
                        Msg = $"喵星人:{cat.Name}回報送養失敗" + ex,
                        Time = DateTime.Now
                    };
                }
            }
            else
            {
                Console.WriteLine($"回報送養失敗!請提供喵星人資訊");
                msg = new MemberManagement.Message()
                {
                    Code = 400,
                    Msg = $"刪除失敗!請提供喵星人資訊",
                    Time = DateTime.Now
                };
            }
            return msg;
        }
    }
}
