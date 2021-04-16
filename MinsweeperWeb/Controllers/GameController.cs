using Microsoft.AspNetCore.Mvc;
using MinesweeperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using MinsweeperWeb.Models;
using DataAccessLayer;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace MinsweeperWeb.Controllers
{
    public class GameController : Controller
    {
        //Create an instance of the board obj with
        //easy difficulty and small size 
        public static Board gameBoard;

        //Business layer for game logic and rules 
        public static GameBusinessService gameRules;

        //Property to keep track of the game status 
        public static string gameStatus;

        //Property to keep track of the current user
        public static string userName;

        //Game properties
        public static int clicks;
        
        public IActionResult Index()
        {
            //Create an instance of the GameBoardViewModel
            GameBoardViewModel game = new GameBoardViewModel();

            //Instantiate the game business class
            gameRules = new GameBusinessService();

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);
            gameStatus = "In Progress";

            //If the session is not empty - user logged in
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                //Grab their username
                userName = HttpContext.Session.GetString("username");
            }
            else
                userName = "";
           
            



            game.GameBoard = gameBoard;
            game.numOfClick = clicks;
            game.UserName = userName;

            //Pass the GameBoardViewModel with the next view
            return View("Index", game);
        }

        //Manage the button click on he grid - left click
        public IActionResult LeftClickContinueGame(int mineID)
        {
            //Update the click property 
            clicks++;

            //Play the move and record outcome
            gameStatus = gameRules.PlayMove(gameBoard, mineID);

            //If the player revealed all mine
            if (gameStatus == "Won")
            {
                //Setup the ViewBags for message
                ViewBag.Message = "Congrats you won!!";

                //End the game and make our game view model
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.UserName = userName;
                game.EndGame = gameStatus;
                game.numOfClick = clicks;


                return PartialView("EndGame", game);
            }

            //The player clicked on a mine
            else if (gameStatus == "Lost")
            {
                //Setup the ViewBags for message
                ViewBag.Message = "You lost. Let's play again!";

                //End the game and make our game view model
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.UserName = userName;
                game.EndGame = gameStatus;
                game.numOfClick = clicks;

                return PartialView("EndGame", game);
            }
            //Player has not won or lost - still playing
            else 
            {
                //Create our game view model
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.UserName = userName;
                game.EndGame = gameStatus;
                game.numOfClick = clicks;

                return PartialView("InProgressGame", game);
            }
        }

        //Manage the right button click on he grid 
        public IActionResult RightClickContinueGame(int mineID)
        {
            //Update the click property 
            clicks++;

            //Play the move and record outcome
            gameStatus = gameRules.FlagMine(gameBoard, mineID);

            //If player flagged all bombs
            if (gameStatus == "Won")
            {
                //Setup the ViewBags for message
                ViewBag.Message = "Congrats you won!!";

                //Create our game view model
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.UserName = userName;
                game.EndGame = gameStatus;
                game.numOfClick = clicks;

                return PartialView("EndGame", game);
            }

            else 
            {
                //Create our game view model
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.UserName = userName;
                game.EndGame = gameStatus;
                game.numOfClick = clicks;

                return PartialView("InProgressGame", game);
            }
        }

        //Manages request when a player wants to play new game
        public IActionResult PlayAgain()
        {
            //Reset the click proprty and gamestatus
            clicks = 0;
            gameStatus = "In Progress";

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);
            
            
            //Create game board view model
            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.UserName = userName;
            game.numOfClick = clicks;

            return View("Index", game);
        }

        //Manages when player saves game 
        public IActionResult OnSave(int seconds)
        {
            //Using JSON object seralization - turn our gameboard into a string for database
            string gameString = JsonConvert.SerializeObject(gameBoard);

            //Create new instance of GameBusinessService
            GameDataBusinessService gameBusiness = new GameDataBusinessService();

            //Use session to grab user
            int userID = (int)HttpContext.Session.GetInt32("userID");

            //Make an instance of Game data object 
            Game gameDataObj = new(gameString, seconds, clicks, userID);
            
            //Save game
            bool success = gameBusiness.SaveGame(gameDataObj);

            //Create game board view model
            GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(1));
            game.UserName = userName;
            game.EndGame = gameStatus;
            game.numOfClick = clicks;

            return PartialView("InProgressGame", game); 
        }


        //Mamages when a player loads their last save from the database
        public IActionResult OnLoad()
        {
            //Create new instance of GameBusinessService
            GameDataBusinessService gameDataBusiness = new GameDataBusinessService();

            //Use session to grab user
            int userID = (int)HttpContext.Session.GetInt32("userID");

            //Load the game
            Game gameObject = gameDataBusiness.LoadGame(userID);

            //Using seralization and object casting convert the boardstring 
            //from the database into a game board object
            gameBoard = JsonConvert.DeserializeObject<Board>(gameObject.boardString);

            //Debug
            Debug.WriteLine(JsonConvert.DeserializeObject<Board>(gameObject.boardString));

            //Create game view model
            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.UserName = userName;
            game.EndGame = gameStatus;
            game.Mine = gameBoard.GrabCellFromGrid(1);
            game.numOfClick = gameObject.numOfClicks;
            game.loadedSeconds = gameObject.seconds;

            //Debug the seconds
            Debug.WriteLine(game.loadedSeconds);

            return PartialView("InProgressGame", game);
        }

        /// <summary>
        /// Loads one save using gameStateID
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        public IActionResult LoadOneGame(int gameStateID)
        {

            ///Create new instance of GameBusinessService
            GameDataBusinessService gameDataBusiness = new GameDataBusinessService();

            //Use session to grab user
            //If the session is not empty - user logged in
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                //Grab their username
                userName = HttpContext.Session.GetString("username");
            }

            //Load the game
            Game gameObject = gameDataBusiness.LoadOneGame(Convert.ToInt32(gameStateID));

            //Using seralization and object casting convert the boardstring 
            //from the database into a game board object
            gameBoard = JsonConvert.DeserializeObject<Board>(gameObject.boardString);

            //Create game view model
            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.UserName = userName;
            game.EndGame = gameStatus;
            game.Mine = gameBoard.GrabCellFromGrid(1);
            game.numOfClick = gameObject.numOfClicks;
            game.loadedSeconds = gameObject.seconds;


            return View("LoadedGame", game);
        }

        //THIS CAN BE AN API
        public int getLoadedSeconds()
        {
            //Use session to grab user
            int userID = (int)HttpContext.Session.GetInt32("userID");

            //Create a new instance of GameDAO and load the game
            GameDAO gameDAO = new GameDAO();
            Game gameObject = gameDAO.LoadGame(userID);

            return gameObject.seconds;
        }














    }
}
