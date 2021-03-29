using Microsoft.AspNetCore.Mvc;
using MinsweeperWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;

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
                //CREATE AN EMPTY GAME BOARD STATE FOR THE USER AND PUT IN THEIR GAME GAMESTATEID
                return View("Views/Register/RegisterSuccess.cshtml", user);
            }
            else
            {
                return View("Views/Register/RegisterFail.cshtml");
            }

        }
    }
}
