using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceSite.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace ECommerceSite.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
		// here we can "inject" our context service into the constructor
		public HomeController(MyContext context)
		{
			dbContext = context;
		}

        public IActionResult Index()
        {
            return View("Dashboard");
        }

        [HttpGet("Login")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult RegisterPage()
        {
            return View();
        }

        //Register a new user
        [HttpPost("Register")]
        public IActionResult Register(User newUser)
        {
            //If ModelState contains no errors
            if(ModelState.IsValid)
            {
                //Check to see if Email address already exists in database
                bool notUnique = dbContext.Users.Any(a => a.Email == newUser.Email);

                //If Email already taken,display error and redirect to index.
                if(notUnique)
                {
                    ModelState.AddModelError("Email", "Email already in use. Please use a new one.");
                    return View("RegisterPage");
                }

                //If unique password, hash the new user's password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hash = hasher.HashPassword(newUser, newUser.Password);
                newUser.Password = hash;

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                //Store new user's id into session
                var last_added_User = dbContext.Users.Last().UserId;
                HttpContext.Session.SetInt32("UserId", last_added_User);
            
                return RedirectToAction("Dashboard");
            }
        return View("RegisterPage");
        }

        // Let a new user login
        [HttpPost("Login")]
        public IActionResult Login(LogUser logUser)
        {
            if(ModelState.IsValid)
            {
                // Look to see if user exists in database
                var found_user = dbContext.Users.FirstOrDefault(user => user.Email == logUser.LogEmail);

                // If no user found via that email address, display error and redirect back to index page.
                if(found_user == null)
                {
                    ModelState.AddModelError("LogEmail", "Incorrect Email or Password");
                    return View("LoginPage");
                }

                //If a user is found, Verify their password to the hashed password stored in the database.
                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                var user_verified = Hasher.VerifyHashedPassword(logUser, found_user.Password, logUser.LogPassword);

                //If VerifyHashedPassword returns a 0, Passwords didn't match. Return user to Index.
                if(user_verified == 0)
                {
                    ModelState.AddModelError("LogEmail", "Incorrect Email or Password");
                    return View("LoginPage");
                }

                //Store logged in user's id into session.
                HttpContext.Session.SetInt32("UserId", found_user.UserId);

                //Store logged in user's id into ViewBag.
                ViewBag.Logged_in_user_id = found_user.UserId;

                return RedirectToAction("Dashboard");
            }
            return View("LoginPage");
        }

        //Navigate to the Dashboard on successful Login/Registration
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            //Checked to see if user is in session or not. If not, redirec to index.
            if(HttpContext.Session.GetInt32("UserId") == null){
                return View("Index");
            }

            //Get user id from session
            int? UserId = HttpContext.Session.GetInt32("UserId");

            //If no user in session, redirect to index
            if(UserId == null)
            {
                return View("Index");
            }

            //Place current logged in user's name in Viewbag.FirstName
            var current_user = dbContext.Users.First(usr => usr.UserId == UserId);
            ViewBag.FirstName = current_user.FirstName;

            //Place current logged in user's id in Viewbag.Logged_in_user_id
            ViewBag.Logged_in_user_id = HttpContext.Session.GetInt32("UserId");

            // var hobbies = dbContext.Hobbies
            //                 .Include(a => a.Enthusiasts)
            //                     .ThenInclude(a => a.User)
            //                 .Include(a => a.Creator)
            //                 .ToList();

            // return View("Dashboard", hobbies);

            ViewBag.name = current_user.FirstName;
            

            return View("Dashboard");
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
