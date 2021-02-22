using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperClasses
{
    public class Cell
    {
        //Class properties with getter and setters
        public int row { get; set; }
        public int column { get; set; }
        public bool live { get; set; }
        public int numOfLive { get; set; }
        public bool visited { get; set; }
        public bool flagged { get; set; }
        public int ID { get; set; }

        //Default constructor 
        public Cell()
        {
            //Initlazing the class properties to default values
            row = 0;
            column = 0;
            live = false;
            numOfLive = 0;
            visited = false;
            ID = 0;
        }

        //Paramtized Constructor 
        public Cell(int row, int column, bool live, int numOfLive, bool flagged, int ID)
        {
            //Initlizing the class properties to each parameter
            this.row = row;
            this.column = column;
            this.live = live;
            this.numOfLive = numOfLive;
            this.flagged = flagged;
            this.ID = ID;
        }
    }
}
