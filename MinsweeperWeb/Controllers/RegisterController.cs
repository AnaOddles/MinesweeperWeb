using Microsoft.AspNetCore.Mvc;
using MinsweeperWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using MinsweeperWeb.Utility;

namespace MinsweeperWeb.Controllers
{
    public class RegisterController : Controller
    {
        //Accessing the user model with the DAL 
        UserData userDAO = new UserData();

        //Logger dependecy injection 
        public ILogger logger { get; set; }

        //Constructor with injected ILogger
        public RegisterController(ILogger logger)
        {
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            logger.Info("Registration attempt.");
            bool isRegistered = false;

            isRegistered = userDAO.RegisterUser(user);
            if (isRegistered)
            {
                logger.Info("Registation attempt successful for: " + user.Username);

                //Log the user in 
                Authentication auth = new Authentication();
                auth.Username = user.Username;
                auth.Password = user.Password;
                int userID = userDAO.LoginUser(auth);

                //Start the user sessions
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetInt32("userID", userID);

                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("username")) &&
                (HttpContext.Session.GetString("userID") != null))
                    logger.Info("Successfully created session for - " + HttpContext.Session.GetString("username") + " at registration");
                else
                    logger.Warning("Failed to create session for " + user.Username);

                return View("Views/Register/RegisterSuccess.cshtml", user);
            }
            else
            {
                logger.Warning("Registation attempt failure for: " + user.Username);
                return View("Views/Register/RegisterFail.cshtml");
            }

        }
    }
}
