using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamsWarehouseWebApp.Models;
using SamsWarehouseWebApp.Models.Data;
using SamsWarehouseWebApp.Models.DTO;
using System.Diagnostics;

namespace SamsWarehouseWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ItemDBContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, ItemDBContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;

        }
        /// <summary>
        /// Index controller method used to return a home index view 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
/*            //Automatically sets the user ID to 1 when application is in development to save logging in each time
            if (_webHostEnvironment.IsDevelopment())
            {
                if (HttpContext.Session.Get("ID") == null)
                {
                    HttpContext.Session.SetString("ID", "1");
                }
            }*/
            return View();
        }
        /// <summary>
        /// Returns privacy page view 
        /// </summary>
        /// <returns></returns>
        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        /// Returns error view model that displays error 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Displays login page/view 
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Login controller method that checks user login details (username (Email) and password for authentication
        /// </summary>
        /// <param name="loginDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginUserDTO loginDetails)
        {
            // Assigns the user inputted email and password to the User's properties (email and password) 
            var user = _dbcontext.Users.Where(c => c.Email.Equals(loginDetails.Email)).FirstOrDefault();

            if (user == null)
            {
                return View();
            }
            // BCrypt verifies password by hashing password
            if (BCrypt.Net.BCrypt.EnhancedVerify(loginDetails.Password, user.PasswordHash))
            {
                HttpContext.Session.SetString("ID", user.UserId.ToString());
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Index","Item");
            }

            return View();

        }
        /// <summary>
        /// Displays register page for account creation 
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Register controller method used to register a user's details/create an appuser 
        /// </summary>
        /// <param name="registerDetails"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Register(RegisterUserDTO registerDetails)
        {
            try
            {
                var sanitizer = new HtmlSanitizer();

                AppUser newUser = new AppUser()
                {
                    // Sanitize the Email input
                    Email = sanitizer.Sanitize(registerDetails.Email),
                    // Sanitize the Password input
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(sanitizer.Sanitize(registerDetails.Password))
                };

                _dbcontext.Users.Add(newUser);
                _dbcontext.SaveChanges();

                return RedirectToAction("Login");

            }
            catch (Exception e)
            {
                return View();
            }
        }

        /// <summary>
        /// Home controller that logs a user out and displays home index page 
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            // Clears user session and 'logs out' user 
            HttpContext.Session.Clear();
            // Returns user to home index page
            return RedirectToAction("Index", "Home");
        }
    }
}