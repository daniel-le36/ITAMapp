using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITAMapp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITAMapp.Controllers
{
    public class AdminController : Controller
    {

        private readonly ITAMContext _context;

        public AdminController(ITAMContext context)

        {
            _context = context;

        }
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }
    }
}
