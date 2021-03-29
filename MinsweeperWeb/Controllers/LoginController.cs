using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using MinesweeperClasses;
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

        //Create an instance of the board obj with
        //easy difficulty and small size 
        public static Board gameBoard;

        //Business layer for game logic and rules 
        public static GameBusinessService gameRules;

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
                int userID = userDAO.LoginUser(auth);
                //return the login successful view and the user
                //return View("Views/Login/LoginSuccess.cshtml", userDAO.GrabUserByID(auth));

                //Instantiate the game business class
                gameRules = new GameBusinessService();

                //Setup the game board 
                gameBoard = gameRules.SetupGame(10, gameBoard);

                //Used only for testing purposes
                gameBoard.VisitAll();

                UserData userData = new UserData();
                User user = userData.GrabUserByID(userID);
                return RedirectToAction("Index", "Game", user);
            }
            else
            {
                return View("Views/Login/LoginFail.cshtml");
            }

        }
    }
}
