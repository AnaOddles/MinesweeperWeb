using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MinesweeperClasses;
using System.Diagnostics;
using MinsweeperWeb.Models;

namespace MinsweeperWeb.Controllers
{
    public class SavesController : Controller
    {
        /// <summary>
        /// Index for Saves Page 
        /// Only accessible to users logged in 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorization]
        public IActionResult Index()
        {
            GameDataBusinessService gameData = new GameDataBusinessService();
            //Grab userID from session
            int userID = (int)HttpContext.Session.GetInt32("userID");

            return View(gameData.AllGamesForUser(userID));
        }

        /// <summary>
        /// Redirects back to GmeController to load game
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Load(int gameStateID)
        {
            return RedirectToAction("LoadOneGame", "Game", gameStateID);
        }

        /// <summary>
        /// Deletes from the saves
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int gameStateID)
        {
            GameDataBusinessService gameData = new GameDataBusinessService();
            gameData.DeleteSave(gameStateID);
            int userID = (int)HttpContext.Session.GetInt32("userID");
            return View("Index", gameData.AllGamesForUser(userID));
        }


    }
}
