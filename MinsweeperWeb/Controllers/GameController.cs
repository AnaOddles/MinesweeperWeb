using Microsoft.AspNetCore.Mvc;
using MinesweeperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using MinsweeperWeb.Models;

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

        //public static GameBoardViewModel game;

        public IActionResult Index()
        {
            //Instantiate the game business class
            gameRules = new GameBusinessService();

            //Setup the game board 
            gameBoard = gameRules.SetupGame(10, gameBoard);
            gameStatus = "In Progress";
            //Used only for testing purposes
            gameBoard.VisitAll();

            GameBoardViewModel game = new GameBoardViewModel();
            game.GameBoard = gameBoard;
            return View("Index", game);
        }

       

        //Manage the button click on he grid - left click
        public IActionResult ShowOneMine(int mineID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.PlayMove(gameBoard, mineID);

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

            GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
            game.EndGame = gameStatus;

            //Return the index view again with passing the gameboard
            return PartialView(game);
        }

        //Manage the right button click on he grid 
        public IActionResult ShowOneMineRightClick(int mineID)
        {
            //Play the move and record outcome
            gameStatus = gameRules.FlagMine(gameBoard, mineID);

            if (gameStatus == "Won")
            {
                //Setup another game 
                gameBoard = gameRules.SetupGame(10, gameBoard);
                ViewBag.Message = "Congrats you won!!";
            }

            GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(mineID));
            game.EndGame = gameStatus;

            //Return the index view again with passing the gameboard
            return PartialView("ShowOneMine", game);
        }

        public IActionResult CheckForGameEnd()
        {
            if (!gameStatus.Equals("Active"))
            {
                gameBoard.VisitAll();
            }

            GameBoardViewModel game = new GameBoardViewModel(gameBoard, gameBoard.GrabCellFromGrid(0));

            return PartialView("ShowOneMine", game);
        }



    }
}
