﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class MeowEncyclopediaController : Controller
    {
        public IActionResult MeowBook()
        {
            return View();
        }
    }
}
