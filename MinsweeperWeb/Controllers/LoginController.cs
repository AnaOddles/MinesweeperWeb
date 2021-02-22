using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using MinsweeperWeb.Models;
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
            
            //if the auth is valid - take to login successful 
            if (userDAO.LoginUser(auth) > 0)
            {
                //return the login successful view and the user
                //return View("Views/Login/LoginSuccess.cshtml", userDAO.GrabUserByID(auth));
                return View("Views/Game/Index.cshtml");
            }
            else
            {
                return View("Views/Login/LoginFail.cshtml");
            }

        }
    }
}
