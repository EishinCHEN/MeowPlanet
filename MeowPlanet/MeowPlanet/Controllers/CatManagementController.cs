using MeowPlanet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class CatManagementController : Controller
    {
        private readonly Models.MeowContext _dbcontext;
        public CatManagementController(Models.MeowContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }
        //顯示沒有貓咪的畫面
        public IActionResult AddCat()
        {
            return View();
        }
        //顯示上傳貓咪的畫面
        public IActionResult UploadCat()
        {
            return View();
        }
        [HttpPost]
        [Route("/CatManagement/uploadconfirm")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<MemberManagement.Message> UploadConfirm(IFormCollection info)
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
                    try
                    {
                        var cat = new Cat
                        {
                            UserId = ID,
                            Image = "/images/預設大頭貼.png",
                            Name = info["Name"],
                            Country = info["Country"],
                            City = info["City"],
                            CatColor = info["CatColor"],
                            CatGender = Convert.ToInt32(info["CatGender"]),
                            Age = info["Age"],
                            Ligation = info["Ligation"],
                            Vaccine = info["Vaccine"],
                            Chip = info["Chip"],
                            Remark = info["Remark"]
                        };
                        _dbcontext.Cats.Add(cat);
                        _dbcontext.SaveChanges();
                        Console.WriteLine("貓咪資訊新增成功");

                        msg = new MemberManagement.Message()
                        {
                            Code = 200,
                            Msg = $"連線成功! 貓咪資訊新增成功",
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
            return msg;
        }
    }
}
