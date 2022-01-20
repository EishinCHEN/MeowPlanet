using MeowPlanet.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class CatDetailController : Controller
    {
        private readonly Models.MeowContext _dbcontext;
        public CatDetailController(MeowContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("{catId}")]
        [Route("/CatDetail/{catId}")]
        public IActionResult CatDetail(int catId)
        {
            var cat = (from item in _dbcontext.Cats
                       where item.CatId == catId
                       select item).FirstOrDefault();

            var user = (from item in _dbcontext.UserDatas
                        where item.UserId == cat.UserId
                        select item).FirstOrDefault();


            int userCount = (from c in _dbcontext.Cats
                             where c.UserId == user.UserId
                             select c).Count();
            //世光新增
            //var claims = HttpContext.User;
            //var ID = Convert.ToInt32(claims.Identity.Name);
            //var collect = from w in _dbcontext.CollectionLists
            //                   where w.UserId == ID
            //                   select new CatFilterList
            //                   {
            //                       UserId = w.UserId.ToString(),
            //                       CatId = w.CatId,
            //                   };
            //var cat2 = from e in _dbcontext.Cats
            //         where e.CatId == catId
            //         select e;
            //var collectCheck =  (from j in cat2
            //                    join k in collect on j.CatId equals k.CatId into we
            //                    from f in we.DefaultIfEmpty()
            //                    select new CatFilterList
            //                    {
            //                        UserId = f.UserId ?? string.Empty,
            //                    }).FirstOrDefault();
            //世光新增
            if (cat != null)
            {
                ViewData["Image"] = cat.Image;
                ViewData["Image2"] = cat.Image2;
                ViewData["Image3"] = cat.Image3;
                ViewData["Image4"] = cat.Image4;

                ViewData["Name"] = cat.Name;
                ViewData["CatGender"] = cat.CatGender;
                ViewData["CatColor"] = cat.CatColor;
                ViewData["Age"] = cat.Age;
                ViewData["Vaccine"] = cat.Vaccine;
                ViewData["Ligation"] = cat.Ligation;
                ViewData["City"] = cat.City;
                ViewData["Country"] = cat.Country;
                ViewData["Chip"] = cat.Chip;
                ViewData["Sick"] = cat.Sick;
                ViewData["Remark"] = cat.Remark;
                ViewData["CatId"] = cat.CatId;
                ViewData["PublishedDay"] = cat.PublishedDay;
                ViewData["UserId"] = cat.UserId;
                ViewData["RealName"] = user.RealName;
                ViewData["userCount"] = userCount;
                ViewData["userPhoto"] = user.PersonalPhoto;

                //世光新增
                //ViewData["CollectCheck"] = collectCheck.UserId;
            }
            return View();
        }

    }
}
