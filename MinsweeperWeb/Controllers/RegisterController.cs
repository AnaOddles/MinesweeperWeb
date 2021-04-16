using Microsoft.AspNetCore.Mvc;
using MinsweeperWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;

namespace MinsweeperWeb.Controllers
{
    public class RegisterController : Controller
    {
        //Accessing the user model with the DAL 
        UserData userDAO = new UserData();
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            bool isRegistered = false;

            isRegistered = userDAO.RegisterUser(user);
            if (isRegistered)
            {
                //Log the user in 
                Authentication auth = new Authentication();
                auth.Username = user.Username;
                auth.Password = user.Password;
                int userID = userDAO.LoginUser(auth);

                //Start the user sessions
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetInt32("userID", userID);

                return View("Views/Register/RegisterSuccess.cshtml", user);
            }
            else
            {
                return View("Views/Register/RegisterFail.cshtml");
            }

        }
    }
}
