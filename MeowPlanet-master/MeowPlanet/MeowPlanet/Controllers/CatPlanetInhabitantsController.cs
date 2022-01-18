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

        //預先傳出登入者資訊到前端
        public async Task<IActionResult> CatPlanetInhabitants()
        {
            var claims = HttpContext.User;
            var ID = Convert.ToInt32(claims.Identity.Name);
            var info = await (from a in _dbContext.UserDatas
                              where a.UserId == ID
                              select a).FirstOrDefaultAsync();
            ViewData["UserID"] = ID;
            //ViewData["UserID"] = "123";
            var b = ViewData["UserID"];
            //var ROLE = claims.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            //TempData["Role"] = ROLE;
            return View();
        }




        [RouteAttribute("/CatPlanetInhabitants/Filter")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        //  public List<Models.Cat> catjson([FromBody]Models.Cat Delivery)//師改的
        public List<Models.Cat> CatFilter(IFormCollection Delivery4)
        {
            Console.WriteLine("Account:" + Delivery4["CatColor"]);
            var query = _dbContext.Cats.AsQueryable();

            if (!string.IsNullOrEmpty(Delivery4["CatColor"]))
            {
                query = query.Where(x => x.CatColor == Delivery4["CatColor"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Ligation"]))
            {
                query = query.Where(x => x.Ligation == Delivery4["Ligation"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Age"]))
            {
                query = query.Where(x => x.Age == Delivery4["Age"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Vaccine"]))
            {
                query = query.Where(x => x.Vaccine == Delivery4["Vaccine"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["City"]))
            {
                query = query.Where(x => x.City == Delivery4["City"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["Country"]))
            {
                query = query.Where(x => x.Country == Delivery4["Country"].ToString());
            }

            if (!string.IsNullOrEmpty(Delivery4["CatGender"]))
            {
                query = query.Where(x => x.CatGender.ToString() == Delivery4["CatGender"].ToString());
            }

            //---------------------------老方法------------------------------------

            //--------------------------老方法--------------------------------------
            List<Models.Cat> datacat777 = query.ToList<Models.Cat>();
            Console.WriteLine(datacat777.Count);
            return datacat777;
        }

        //接收前端Cat_ID跟User_ID寫入新的收藏清單資料到後端
        [RouteAttribute("/CatPlanetInhabitants/DeleteCollect")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        //  public List<Models.Cat> catjson([FromBody]Models.Cat Delivery)//師改的
        public List<Models.Cat> Collect(IFormCollection Collectparameter)
        {
            Console.WriteLine("Account:" + Collectparameter["Cat_ID"]);
            Console.WriteLine("Account:" + Collectparameter["User_ID"]);
            // 將前端回傳的資料轉換為符合資料庫的型別
            int CAT_ID = Int32.Parse(Collectparameter["Cat_ID"]);
            int USER_ID = Int32.Parse(Collectparameter["User_ID"]);
            var deletelist = _dbContext.CollectionLists.Where(s => s.CatId == CAT_ID && s.UserId == USER_ID).SingleOrDefault();
            if (deletelist != null)
            {
                _dbContext.CollectionLists.Remove((CollectionList)deletelist);
                _dbContext.SaveChanges();
            }

            _dbContext.SaveChanges();
            var query = _dbContext.Cats.AsQueryable();
            List<Models.Cat> datacat777 = query.ToList<Models.Cat>();
            Console.WriteLine(datacat777.Count);
            return datacat777;
        }

        //接收前端Cat_ID跟User_ID寫入刪除收藏清單資料到後端
        [RouteAttribute("/CatPlanetInhabitants/CreateCollect")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        //  public List<Models.Cat> catjson([FromBody]Models.Cat Delivery)//師改的
        public List<Models.Cat> Delete(IFormCollection Deleteparameter)
        {
            Console.WriteLine("Account:" + Deleteparameter["Cat_ID"]);
            Console.WriteLine("Account:" + Deleteparameter["User_ID"]);
            // 將前端回傳的資料轉換為符合資料庫的型別
            int CAT_ID = Int32.Parse(Deleteparameter["Cat_ID"]);
            int USER_ID = Int32.Parse(Deleteparameter["User_ID"]);
            var collectionList = new CollectionList
            {

                CatId = CAT_ID,
                UserId = USER_ID,
            };
            //SET IDENTITY_INSERT[[database_name. ] schema_name. ] table_name { ON | OFF }
            //Set IDENTITY_INSERT CollectionLists On;   
            _dbContext.CollectionLists.Add(collectionList);

            _dbContext.SaveChanges();
            var query = _dbContext.Cats.AsQueryable();
            List<Models.Cat> datacat777 = query.ToList<Models.Cat>();
            Console.WriteLine(datacat777.Count);
            return datacat777;
        }
    }
}
