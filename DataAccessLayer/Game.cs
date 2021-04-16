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
        public int seconds { get; set; }
        public int numOfClicks  {get; set;}
        public int userID { get; set; }

        //Constructor
        public Game(int gameStateID, string boardString, int seconds, int numOfClicks, int userID)
        {
            this.gameStateID = gameStateID;
            this.boardString = boardString;
            this.seconds = seconds;
            this.numOfClicks = numOfClicks;
            this.userID = userID;
        }

        //Constructor
        public Game(string boardString, int seconds, int numOfClicks, int userID)
        {
            this.boardString = boardString;
            this.seconds = seconds;
            this.numOfClicks = numOfClicks;
            this.userID = userID;
        }

        //Default constructor
        public Game() { }




    }
}
