using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinesweeperClasses;

namespace MinsweeperWeb.Models
{
    public class GameBoardViewModel
    {
        public Board GameBoard { get; set; }
        public Cell Mine { get; set; }

        public string EndGame { get; set; }

        public GameBoardViewModel(Board gameBoard, Cell mine)
        {
            this.GameBoard = gameBoard;
            this.Mine = mine;
            this.EndGame = "In Progress";
        }

        public GameBoardViewModel() 
        {
            this.EndGame = "In Progress";
        }
    }
}
