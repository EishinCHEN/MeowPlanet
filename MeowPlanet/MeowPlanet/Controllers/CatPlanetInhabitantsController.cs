using MeowPlanet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class CatPlanetInhabitantsController : Controller
    {
        private Models.MeowContext _dbContext;
        public CatPlanetInhabitantsController(Models.MeowContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        //預先傳出登入者資訊和已收藏貓咪到前端
        public async Task<IActionResult> CatPlanetInhabitants()
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);
            var info = await (from a in _dbContext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();//用不到待刪
            ViewData["UserID"] = ID;
            return View();
        }

        [RouteAttribute("/CatPlanetInhabitants/Filter")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public List<Models.CatFilterList> CatFilter(IFormCollection Delivery4)
        {
            Console.WriteLine("Account:" + Delivery4["CatColor"]);
            Console.WriteLine("Account:" + Delivery4["City"]);
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);

            var kj = from w in _dbContext.CollectionLists
                     where w.UserId == ID
                     select new CatFilterList
                     {
                         UserId = w.UserId.ToString(),
                         CatId = w.CatId,
                     };

            var jj = from e in _dbContext.Cats
                     select e;
            if (!string.IsNullOrEmpty(Delivery4["CatColor"]))
            {
                jj = jj.Where(x => x.CatColor == Delivery4["CatColor"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Ligation"]))
            {
                jj = jj.Where(x => x.Ligation == Delivery4["Ligation"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Age"]))
            {
                jj = jj.Where(x => x.Age == Delivery4["Age"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Vaccine"]))
            {
                jj = jj.Where(x => x.Vaccine == Delivery4["Vaccine"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["City"]))
            {
                jj = jj.Where(x => x.City == Delivery4["City"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Country"]))
            {
                jj = jj.Where(x => x.Country == Delivery4["Country"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["CatGender"]))
            {
                jj = jj.Where(x => x.CatGender.ToString() == Delivery4["CatGender"].ToString());
            }


            jj = jj.Where(x => x.IsDeleted.ToString() == "1")
                   .Where(x => x.Adopt != "已送養");
            

            if (jj.Count() == 0)
            {
                Console.WriteLine("無條件符合");
            }
            //SQL left join 在 Linq的寫法
            var join = (IQueryable<CatFilterList>)(from j in jj
                                                   join k in kj on j.CatId equals k.CatId into we
                                                   from f in we.DefaultIfEmpty()
                                                   select new CatFilterList
                                                   {
                                                       UserId = f.UserId ?? string.Empty,
                                                       CatId = j.CatId,
                                                       CatColor = j.CatColor,
                                                       Ligation = j.Ligation,
                                                       Age = j.Age,
                                                       Name = j.Name,
                                                       Image = j.Image,
                                                       CatGender = j.CatGender,
                                                       Vaccine = j.Vaccine,
                                                       Country = j.Country,
                                                       City = j.City,
                                                       IsDeleted = j.IsDeleted
                                                   });

            List<Models.CatFilterList> datacat777 = join.ToList<Models.CatFilterList>();
            Console.WriteLine(datacat777.Count);
            return datacat777;

        }

        //接收前端Cat_ID跟User_ID寫入新的收藏清單資料到後端
        [RouteAttribute("/CatPlanetInhabitants/DeleteCollect")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public string Delete(IFormCollection Collectparameter)
        {
            Console.WriteLine("ount:" + Collectparameter["Cat_ID"]);
            Console.WriteLine("ount:" + Collectparameter["User_ID"]);
            var msg = "";
            // 將前端回傳的資料轉換為符合資料庫的型別
            int CAT_ID = Int32.Parse(Collectparameter["Cat_ID"]);
            int USER_ID = Int32.Parse(Collectparameter["User_ID"]);
            var deletelist = _dbContext.CollectionLists.Where(s => s.CatId == CAT_ID && s.UserId == USER_ID).SingleOrDefault();
            if (deletelist == null)
            {

                msg = "查無此資料";
            }
            else
            {
                _dbContext.CollectionLists.Remove((CollectionList)deletelist);
                _dbContext.SaveChanges();
                msg = "已取消收藏";
            }

            return msg;
        }

        //接收前端Cat_ID跟User_ID寫入刪除收藏清單資料到後端
        [RouteAttribute("/CatPlanetInhabitants/CreateCollect")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public List<Models.Cat> Create(IFormCollection Deleteparameter)
        {
            Console.WriteLine("Accot:" + Deleteparameter["Cat_ID"]);
            Console.WriteLine("Accou:" + Deleteparameter["User_ID"]);
            // 將前端回傳的資料轉換為符合資料庫的型別
            int CAT_ID = Int32.Parse(Deleteparameter["Cat_ID"]);
            int USER_ID = Int32.Parse(Deleteparameter["User_ID"]);
            var collectionList = new CollectionList
            {

                CatId = CAT_ID,
                UserId = USER_ID,
            };

            _dbContext.CollectionLists.Add(collectionList);

            _dbContext.SaveChanges();
            var query = _dbContext.Cats.AsQueryable();
            List<Models.Cat> datacat777 = query.ToList<Models.Cat>();//用不到待刪
            Console.WriteLine(datacat777.Count);
            return datacat777;//用不到待刪
        }
    }
}
