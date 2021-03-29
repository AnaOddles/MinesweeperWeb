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

        //Propert to keep track of the current user
        public static User user;

        //Game properties
        static int clicks = 0;
        static Stopwatch Timer = new Stopwatch();
        public TimeSpan ts = Timer.Elapsed;

        public IActionResult Index(User userModel)
        {
            //grab the current user and set it to the controller so we can keep track of it
            user = userModel;

            //Instantiate the game business class
            gameRules = new GameBusinessService();

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);
            gameStatus = "In Progress";

            Timer.Start(); //Starts timer

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            @ViewBag.Timer = elapsedTime;

            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.User = user;
            return View("Index", game);
        }

        //Manage the button click on he grid - left click
        public IActionResult ShowOneMine(int mineID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.PlayMove(gameBoard, mineID);

            //If the player revealed all mine
            if (gameStatus == "Won")
            {
                Timer.Stop(); //Stops Timer
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);

                //Sent timer and clicks to the view
                @ViewBag.Timer = elapsedTime;
                @ViewBag.Clicks = "Clicks: " + clicks;
                ViewBag.Message = "Congrats you won!!";

                //End the game and make our game view model
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.User = user;
                game.EndGame = gameStatus;

                return PartialView("EndGame", game);
            }

            //The player clicked on a mine
            else if (gameStatus == "Lost")
            {
                //Stio the timer and format it 
                Timer.Stop(); //Stops Timer
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10);
                @ViewBag.Timer = elapsedTime;
                @ViewBag.Clicks = "Clicks: " + clicks;

                //Sent the lost game message and create our game view model
                ViewBag.Message = "You lost. Let's play again!";
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.User = user;
                game.EndGame = gameStatus;

                return PartialView("EndGame", game);
            }
            //Player has not won or lost - still playing
            else 
            {
                //Increment the clicks 
                clicks++;

                //Send clicks to view
                @ViewBag.Clicks = "Clicks: " + clicks;

                //Calc time and pass to ViewBag
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
                @ViewBag.Timer = elapsedTime;

                //Create our game view model
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.User = user;
                game.EndGame = gameStatus;

                return PartialView("ShowOneMine", game);
            }


        }

        //Manage the right button click on he grid 
        public IActionResult ShowOneMineRightClick(int mineID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.FlagMine(gameBoard, mineID);

            //If player flagged all bombs
            if (gameStatus == "Won")
            {
                //Stop the timer and format it
                Timer.Stop(); //Stops Timer
                @ViewBag.Clicks = "Clicks: " + clicks;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                  ts.Hours, ts.Minutes, ts.Seconds,
                  ts.Milliseconds / 10);
                @ViewBag.Timer = elapsedTime;


                //Create win message and sent through view bag
                ViewBag.Message = "Congrats you won!!";
                gameBoard = gameRules.EndGame(gameBoard);
                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.User = user;
                game.EndGame = gameStatus;

                return PartialView("EndGame", game);
            }

            else 
            {
                Timer.Stop(); //Stops Timer
                clicks++;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                  ts.Hours, ts.Minutes, ts.Seconds,
                  ts.Milliseconds / 10);
                @ViewBag.Timer = elapsedTime;
                @ViewBag.Clicks = "Clicks: " + clicks;

                GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
                game.User = user;
                game.EndGame = gameStatus;

                return PartialView("ShowOneMine", game);
            }

            

            //Return the index view again with passing the gameboard
            //return PartialView("ShowOneMine", game);
        }

        //Manages request when a player wants to play new game
        public IActionResult PlayAgain()
        {
            //Reset timer and clicks
            clicks = 0;
            Timer.Restart();

            //Format timer
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            @ViewBag.Timer = elapsedTime;

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);
            gameStatus = "In Progress";
            
            //Create game board view model
            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.User = user;
           
            return View("Index", game);
        }


        //Method to create a new game state and return the index game method 
        //allow the user an option to create a new gamestate
        //that will make a new gameboard and replace the current one in the database 

        //Method that will publish the game score and then start a new game 
        //will run the index view from the game views


        //Manages when player saves game 
        public IActionResult OnSave()
        {
            //Stop timer 
            Timer.Stop();

            //Create a string for formated time useing the timer
            string e = String.Format("{0:00}:{1:00}:{2:00}",
             Timer.Elapsed.Hours, Timer.Elapsed.Minutes, Timer.Elapsed.Seconds
            );

            string myTime = e;
            var v = myTime.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Create a DateTime property for our database
            DateTime times = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2]));


            //Game SaveData = new Game();
            //SaveData.numOfClicks = clicks;

            //Using KSON object seralization - turn our gameboard into a string for database
            string gameString = JsonConvert.SerializeObject(gameBoard);

            //Create new instance of gameDAO obj
            GameDAO gameDAO = new GameDAO();
            bool success = gameDAO.SaveGame(gameString, user, clicks, times); 
            return View("Results", success);
        }


        //Mamages when a player loads their last save from the database
        public IActionResult OnLoad()
        {
            //Create a new instance of GameDAO and load the game
            GameDAO gameDAO = new GameDAO();
            Game gameObject = gameDAO.LoadGame(user);

            //Using seralization and object casting conver the boardstring 
            //from the database into a game board object
            gameBoard = JsonConvert.DeserializeObject<Board>(gameObject.boardString);

            
            Debug.WriteLine(JsonConvert.DeserializeObject<Board>(gameObject.boardString));

            //Create game view model
            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            game.User = user;
            game.EndGame = gameStatus;
            game.Mine = gameBoard.GrabCellFromGrid(1);

            //Sent time and clicks
            
            return View("LoadedGame", game);
        }










    }
}
