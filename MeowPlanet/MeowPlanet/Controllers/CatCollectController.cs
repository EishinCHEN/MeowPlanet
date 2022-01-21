using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// //
/// </summary>
/// using MeowPlanet.MemberManagement;
using MeowPlanet.Models;

using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace MeowPlanet.Controllers
{
    public class CatCollectController : Controller
    {

        private Models.MeowContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CatCollectController(Models.MeowContext _dbcontext, IWebHostEnvironment _hostEnvironment)
        {
            this._dbcontext = _dbcontext;
            this._webHostEnvironment = _hostEnvironment;//不知道功能 先省略
        }

        /*[Authorize]  */        //限定已登入的使用者才能瀏覽
        //預先傳出登入者資訊到前端
        public async Task<IActionResult> CatCollect()
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            var info = await (from a in _dbcontext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();
            //var info = ID;

            ViewData["UserID"] = ID;
            //ViewData["UserID"] = "123";
            var b = ViewData["UserID"];
            //var ROLE = claims.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            //TempData["Role"] = ROLE;
            return View();
        }

        //接收要抓取的User資訊
        [RouteAttribute("/CatCollect/SearchCollect")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public List<Models.CollectionListJoinCat> Catjson(IFormCollection search)
        //public ActionResult Catjson(IFormCollection search)
        {
            Console.WriteLine("Account:" + search["User_ID"]);
            //var join = _dbcontext.Cats.AsQueryable();
            var join = (IQueryable<CollectionListJoinCat>)(from j in _dbcontext.CollectionLists
                                                           join k in _dbcontext.Cats
                                                           on j.CatId equals k.CatId
                                                           where j.UserId == Int32.Parse(search["User_ID"])
                                                           select new CollectionListJoinCat   /// 這行記得要對應要使用的新表單的model 不能只有new
                                                           {
                                                               UserId = j.UserId,
                                                               CatId = j.CatId,
                                                               Name = k.Name,
                                                               Age = k.Age,
                                                               City = k.City,
                                                               Country = k.Country,
                                                               IsDeleted = k.IsDeleted,
                                                               Image = k.Image,
                                                               Adopt = k.Adopt
                                                           });

            List<Models.CollectionListJoinCat> datacat777 = join.ToList<Models.CollectionListJoinCat>();
            Console.WriteLine(datacat777.Count);
            return datacat777;

        }
    }
}

