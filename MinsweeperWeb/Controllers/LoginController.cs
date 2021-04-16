using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinesweeperClasses;
using MinsweeperWeb.Models;
using MinsweeperWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinsweeperWeb.Controllers
{
    public class LoginController : Controller
    {
        //Accessing the user model with the DAL 
        UserData userDAO = new UserData();

        //Create an instance of the board obj with
        //easy difficulty and small size 
        public static Board gameBoard;

        //Business layer for game logic and rules 
        public static GameBusinessService gameRules;

        //Logger dependecy injection 
        public ILogger logger { get; set; }

        //Constructor with injected ILogger
        public LoginController(ILogger logger)
        {
            this.logger = logger;
        }

        //View to the home of Login - login form 
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// POST request for user to log in
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Authentication auth)
        {
            logger.Info("Processing a login attempt.");
            logger.Info("Attempting Login- Username: " + auth.Username + " Password: " + auth.Password);

            //if the auth is valid - take to login successful 
            if (userDAO.LoginUser(auth) > 0)
            {
                logger.Info("Login Successful with- Username: " + auth.Username + " Password: " + auth.Password);
                int userID = userDAO.LoginUser(auth);
                UserData userData = new UserData();
                User user = userData.GrabUserByID(userID);

                //Sessions start 
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetInt32("userID", userID);

                return RedirectToAction("Index", "Game");
            }
            //User not logged in 
            else
            {
                logger.Warning("Login Faliure with- Username: " + auth.Username + " Password: " + auth.Password);
                //Remove all sessions that hold credentials 
                HttpContext.Session.Remove("username");
                HttpContext.Session.Remove("userID");

                return View("Views/Login/LoginFail.cshtml");
            }

        }

        public IActionResult Logout()
        {
            MyLogger.GetInstance().Info("Processing a logout attempt for - " +
                HttpContext.Session.GetString("username"));

            //Remove all sessions that hold credentials
            //HttpContext.Session.SetString("username", String.Empty);
            //HttpContext.Session.SetInt32("userID", 0);

            //Remove all sessions that hold credentials 
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("userID");

            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("username")) &&
                (HttpContext.Session.GetString("userID") != null))
                logger.Warning("Failre removal of session - " + String.IsNullOrEmpty(HttpContext.Session.GetString("username")) + " at logout");
            else
                logger.Info("Sucess removal of session at logout");
                
            return View("Index");
        }
    }
}
