using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class Game
    {
        //Class properties 
        public int gameStateID { get; set; }
        public string boardString { get; set; }
        public DateTime time { get; set; }
        public int numOfClicks  {get; set;}
        public int userID { get; set; }

        //Constructor
        public Game(int gameStateID, string boardString, DateTime time, int numOfClicks, int userID)
        {
            this.gameStateID = gameStateID;
            this.boardString = boardString;
            this.time = time;
            this.numOfClicks = numOfClicks;
            this.userID = userID;
        }

        public Game() { }

         

    }
}
