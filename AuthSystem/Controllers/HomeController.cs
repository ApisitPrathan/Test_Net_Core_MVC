using InterviewTest_Clicknext.Areas.Identity.Data;
using InterviewTest_Clicknext.Data;
using InterviewTest_Clicknext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InterviewTest_Clicknext.Controllers
{
    [Authorize] // check autherize have user login
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TransactionDbContext _context;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager,TransactionDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            
            ViewData["UserID"] =_userManager.GetUserId(this.User);
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