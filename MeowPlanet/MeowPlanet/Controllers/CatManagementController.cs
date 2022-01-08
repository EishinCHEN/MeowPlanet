using MeowPlanet.Models;
using MeowPlanet.ViewModels;
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
    public class CatManagementController : Controller
    {
        private readonly Models.MeowContext _dbcontext;
        private readonly IWebHostEnvironment _env;
        public CatManagementController(Models.MeowContext dbcontext, IWebHostEnvironment env)
        {
            this._dbcontext = dbcontext;
            this._env = env;
        }
        //顯示沒有貓咪的畫面
        public async Task<IActionResult> AddCat()
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);
            var info = await(from a in _dbcontext.Cats
                             where a.UserId == userId
                             select a).FirstOrDefaultAsync();
            var ROLE = claims.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            return View();
        }
        //顯示上傳貓咪的畫面
        public IActionResult UploadCat()
        {
            TempData["Photo1"] = "/images/我是誰(小問號).PNG";
            return View();
        }

        //上傳貓咪資訊
        [HttpPost]
        public async Task<MemberManagement.Message> UploadConfirm(IFormFileCollection files, CatViewModel model)
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
                    String root = _env.ContentRootPath + @"\wwwroot\images\catManagement";

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
                                model.Image.Add(fileName);
                            }
                        }
                        //List<String> to string
                        var result = String.Join(", ", model.Image.ToArray());
                        try
                        {
                            var cat = new Cat
                            {
                                UserId = ID,
                                Image = result,
                                Name = model.Name,
                                Country = model.Country,
                                City = model.City,
                                CatColor = model.CatColor,
                                CatGender = Convert.ToInt32(model.CatGender),
                                Age = model.Age,
                                Ligation = model.Ligation,
                                Vaccine = model.Vaccine,
                                Chip = model.Chip,
                                Remark = model.Remark
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
            return msg;
        }
    
        
    }
}
