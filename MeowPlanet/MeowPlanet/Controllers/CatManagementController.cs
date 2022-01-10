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
                            where a.UserId == userId
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
        
        //取得貓咪資訊
        [HttpGet]
        public List<Cat> GetCatInfo()
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var cat = (from a in _dbcontext.Cats
                            where a.UserId == userId
                            select a).ToList();

            List<Cat> cats = cat; 


            return cats;
        }

        //上傳貓咪資訊
        [HttpPost]
        public async Task<IActionResult> UploadConfirm(IFormFileCollection files, CatViewModel model)
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            var user = await(from a in _dbcontext.UserDatas
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
                    String root = _env.ContentRootPath + @"\wwwroot\images\catImages";

                    //model.Image = new List<String>();
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
                                var path = root + fileName;
                                model.Image.Add(path);
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
                                Remark = model.Remark,
                                PublishedDay = DateTime.Now
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

        //public async Task<IActionResult> EditCat()
        //{
        //    ClaimsPrincipal claims = HttpContext.User;
        //    int userId = Convert.ToInt32(claims.Identity.Name);
        //    var cat = await(from a in _dbcontext.Cats
        //                    where a.UserId == userId
        //                    select a).FirstOrDefaultAsync();

        //    return View();
        //}

        [HttpGet]
        [Route("/CatManagement/DeleteCat/catId")]
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
                    cat.IsDeleted = 1;

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
                        Msg = $"喵星人:{cat.Name}刪除失敗",
                        Time = DateTime.Now
                    };
                }
            }
            return msg;
        }
    }
}
