using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinesweeperClasses;
using DataAccessLayer;

namespace MinsweeperWeb.Models
{
    public class GameBoardViewModel
    {
        public Board GameBoard { get; set; }
        public Cell Mine { get; set; }

        public string EndGame { get; set; }

        public string UserName { get; set; }

        public int numOfClick { get; set; }

        public int loadedSeconds { get; set; }

        public GameBoardViewModel(Board gameBoard, Cell mine)
        {
            this.GameBoard = gameBoard;
            this.Mine = mine;
            this.EndGame = "In Progress";
            this.loadedSeconds = 0;
        }

        public GameBoardViewModel() 
        {
            this.EndGame = "In Progress";
        }
    }
}
