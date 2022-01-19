using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class TestController : Controller
    {
        public IActionResult QuestionStart()
        {
             return View();
        }

        public IActionResult Question1(Int32? Q1)
        {
            HttpContext.Session.SetInt32("total", 0);
            ViewBag.Test = 0;
            
            return View();
        }


        public IActionResult Question2(Int32? Q1)
        {
           //get source
            Int32? source=HttpContext.Session.GetInt32("total");
            Int32? newValue = Q1 + source;
            HttpContext.Session.SetInt32("total",(Int32)newValue);
            
            return View();
        }

        public IActionResult Question3(Int32? W1)
        {
            //get source
            Int32? source = HttpContext.Session.GetInt32("total");
            Int32? newValue = W1 + source;
            HttpContext.Session.SetInt32("total", (Int32)newValue);
            
            return View();
        }

        public IActionResult Question4(Int32? E1)
        {
           //get source
            Int32? source = HttpContext.Session.GetInt32("total");
            Int32? newValue =  E1 + source;
            HttpContext.Session.SetInt32("total", (Int32)newValue);
                return View();
        }

        public IActionResult Question5(Int32? R1)
        {
            Int32? source = HttpContext.Session.GetInt32("total");
            Int32? newValue = R1 + source;
            if (newValue >= 4 && newValue <= 6)
            {
                return View("QuestionEnd1");
            }
            else if (newValue >= 7 && newValue <= 9)
            {
                return View("QuestionEnd2");
            }
            else if (newValue >= 10 && newValue <= 12)
            {
                return View("QuestionEnd3");
            }
            else if (newValue >= 13 && newValue <= 15)
            {
                return View("QuestionEnd4");
            }
            else if (newValue >= 16 && newValue <= 18)
            {
                return View("QuestionEnd5");
            }
            else if (newValue >= 19 && newValue <= 21)
            {
                return View("QuestionEnd6");
            }
            else if (newValue >= 22 && newValue <= 24)
            {
                return View("QuestionEnd7");
            }
            else if (newValue >= 25 && newValue <= 28)
            {
                return View("QuestionEnd8");
            }
            return View();
        }


        public IActionResult QuestionEnd1( )
        {

            
            return View();
        }

        public IActionResult QuestionEnd2()
        {

            return View();
        }
        public IActionResult QuestionEnd3()
        {

            return View();
        }
        public IActionResult QuestionEnd4()
        {

            return View();
        }
        public IActionResult QuestionEnd5()
        {

            return View();
        }
        public IActionResult QuestionEnd6()
        {

            return View();
        }
        public IActionResult QuestionEnd7()
        {

            return View();
        }
        public IActionResult QuestionEnd8()
        {

            return View();
        }

    }
}
