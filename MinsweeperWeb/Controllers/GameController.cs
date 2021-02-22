using Microsoft.AspNetCore.Mvc;
using MinesweeperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;


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
        string gameStatus;

        public IActionResult Index()
        {
            //Instantiate the game business class
            gameRules = new GameBusinessService();

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);

            //Used only for testing purposes
            gameBoard.VisitAll();

            return View("Index", gameBoard);
        }

       

        //Manage the button click on he grid 
        public IActionResult HandleButtonClick(int ID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.PlayMove(gameBoard, ID);

            if (gameStatus == "Won")
            {
                //Setup another game 
                gameBoard = gameRules.SetupGame(10, gameBoard);
                ViewBag.Message = "Congrats you won!!";
            }
            else if (gameStatus == "Lost")
            {
                //Restart game 
                gameBoard = gameRules.SetupGame(10, gameBoard);
                ViewBag.Message = "You lost. Let's play again!";
            }

            //Return the index view again with passing the gameboard
            return View("Index", gameBoard);
        }

        //Manage the right button click on he grid 
        public IActionResult HandleRightButtonClick(int ID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.FlagMine(gameBoard, ID);

            if (gameStatus == "Won")
            {
                //Setup another game 
                gameBoard = gameRules.SetupGame(10, gameBoard);
                ViewBag.Message = "Congrats you won!!";
            }

            
            //Return the index view again with passing the gameboard
            return View("Index", gameBoard);
        }

    }
}
