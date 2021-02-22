using System;
using System.Collections.Generic;
using System.Text;
using MinesweeperClasses;

namespace BusinessLayer
{
    //Class used for the game logic and rules for MineSweeperWeb
    public class GameBusinessService
    {
        /// <summary>
        /// Sets up the game 
        /// </summary>
        /// <param name="difficulty"></param>
        public Board SetupGame(int gridSize, Board gameBoard)
        {
            //Make a new instance of the game board
            gameBoard = new Board(gridSize, gridSize);

            //Set up the bombs on the board and live neighbors
            gameBoard.SetupLiveNeighbors();
            gameBoard.CalculateLiveNeighbors();

            return gameBoard;

            
            
        }

        /// <summary>
        /// Method for each player move - click on cell
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string PlayMove(Board gameBoard, int ID) 
        {
            //Grab the cell from out gameboard using the passed ID
            Cell clickedCell = gameBoard.GrabCellFromGrid(ID);

            //only visit a cell with no flags 
            if (!clickedCell.flagged)
            {
                //if the cell tht was clicked is a bomb 
                if (clickedCell.live)
                {
                    gameBoard.VisitBombCell(ID);
                    return "Lost";
                }
                //Not live - just a normal cell
                else
                {
                    gameBoard.FloodFill(clickedCell);

                    //Check if all non live have been vistied 
                    if (gameBoard.numOfNonLive <= 0)
                    {
                        return "Won";
                    }

                    //If the cell is not a bomb, floodfill 
                    return "In progress";
                }
            }
            return "In progress";
        }

        public string FlagMine(Board gameBoard, int ID)
        {
            gameBoard.FlagCell(ID);

            if (gameBoard.CheckIfAllFlagged())
                return "Won";
            else
                return "In progress";
        }

        
    }
}
