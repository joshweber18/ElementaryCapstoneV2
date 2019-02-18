using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace V2Capstone.Controllers
{
    public class GradeGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}