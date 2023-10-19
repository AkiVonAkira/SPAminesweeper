using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


// Jobba på denna i senare skede, Skapa controller som visar top 5 
namespace SPAmineseweeper.Controllers
{
    public class ScoreController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

