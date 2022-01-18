using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CatPlanetInhabitants()
        {
            return View();
        }
        [RouteAttribute("/CatPlanetInhabitants/Filter")]
        [HttpPostAttribute]
        [ProducesAttribute("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        //  public List<Models.Cat> catjson([FromBody]Models.Cat Delivery)//師改的
        public List<Models.Cat> Catjson(IFormCollection Delivery4)
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
    }
}
