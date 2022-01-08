using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class CatManagementController : Controller
    {
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

        //顯示管理貓咪的畫面
        public IActionResult ManageCat()
        {
            return View();
        }
    }
}
