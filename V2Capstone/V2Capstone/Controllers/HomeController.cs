using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using V2Capstone.Models;

namespace V2Capstone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DirectUser()
        {
            if (User.IsInRole("Teacher".Trim()))
            {
                return RedirectToAction("Index", "TeacherModels");
            }
            if (User.IsInRole("Parent".Trim()))
            {
                return RedirectToAction("Index", "ParentModels");
            }
            if (User.IsInRole("Student".Trim()))
            {
                return RedirectToAction("Index", "StudentModels");
            }
            else
            {
                return null;
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
