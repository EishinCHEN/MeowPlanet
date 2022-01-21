using MeowPlanet.Models;
using MeowPlanet.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly Models.MeowContext _dbcontext;
        public ScheduleController(Models.MeowContext dbcontext)
        {
            this._dbcontext = dbcontext;

        }
        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
        }

        [HttpGet]
        [Route("/Schedule/Appointment/{catId}")]
        public IActionResult Appointment(int catId)
        {
            ClaimsPrincipal claims = HttpContext.User;
            int userId = Convert.ToInt32(claims.Identity.Name);

            var cat = (from item in _dbcontext.Cats
                       where catId == item.CatId
                       select item).FirstOrDefault();

            var schedule = (from item in _dbcontext.Schedules
                            where item.CatId == catId
                            select item);

            if (cat != null)
            {
                ViewData["CatId"] = cat.CatId;
                ViewData["Sender"] = cat.UserId;
                ViewData["Name"] = cat.Name;

            }
            if (userId != 0)
            {
                ViewData["UserId"] = userId;
            }

            return View();
        }


        [HttpGet("{catId}")]
        [Route("/GetSchedules/{catId}")]
        public List<Schedule> GetSchedules(int catId)
        {
            var data = (from item in _dbcontext.Schedules
                        where item.CatId == catId
                        select item).ToList();

            List<Schedule> Schedule = data;

            return Schedule;
        }

        [HttpPost]
        [Route("/Schedule/UpdateSchedule/")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public void UpdateSchedule([FromBody] ScheduleViewMode ScheduleVM)
        {
            Schedule schedule = new Schedule();
            if (ScheduleVM != null)
            {
                schedule.CatId = ScheduleVM.CatId;
                schedule.UserId = ScheduleVM.UserId;
                schedule.Date = ScheduleVM.Date;
            }

            _dbcontext.Schedules.Add(schedule);
            _dbcontext.SaveChanges();
        }

    }
}
