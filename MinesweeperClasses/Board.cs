using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperClasses
{
    public class Board
    {
        //Class properties 
        public int size { get; set; }
        public Cell[,] grid { get; set; }
        //easy(1) - 10%, medium(2) - 20%, hard(3) - 30%
        public int difficulty { get; set; }
        //num of non-bomb cells
        public int numOfNonLive { get; set; }
        public Board()
        {
            //Default values for class properties
            this.size = 0;
            this.difficulty = 3;
            //Instantiate 2D array
            this.grid = new Cell[size, size];


            //Fill each index with 0
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; i++)
                {
                    //Creta a new Cell obj to put in each index of array
                    Cell c = new Cell(i, j, false, 0, false, 0);
                    this.grid[i, j] = c;
                }
            }
        }

        //Paramitzed constructor
        public Board(int size, int difficulty)
        {
            //Instantiate class properties with parameter
            this.size = size;
            this.difficulty = difficulty;
            //Instantiate 2D array
            this.grid = new Cell[size, size];
            //Fill each index with 0

            int id = 1;
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //Creta a new Cell obj to put in each index of array
                    Cell c = new Cell(i, j, false, 0, false, id);
                    id++;
                    this.grid[i, j] = c;
                }
            }
        }
        /// <summary>
        ///method to set up bombs
        /// </summary>
        public void SetupLiveNeighbors()
        {
            //random number generator
            Random rand = new Random();
            int num = 0;

            //loop through the grid to fill in bombs
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //switch case statement for the difficulty
                    switch (this.difficulty)
                    {
                        //10 % case
                        case 10:
                            //num = rand.Next(1, 11);
                            //if (num == 3)
                            if (rand.Next(100) < 10)
                                //change the live prop to true for each obj in the cell
                                grid[i, j].live = true;
                            break;
                        //20 % case
                        case 20:
                            //num = rand.Next(1, 11);
                            //if (num == 1 || num == 4)
                            if (rand.Next(100) < 20)
                                //change the live prop to true for each obj in the cell
                                grid[i, j].live = true;
                            break;
                        //30 % case
                        case 30:
                            //num = rand.Next(1, 11);
                            //change the live prop to true for each obj in the cell
                            //if (num != 8 && num != 9 && num != 10)
                            if (rand.Next(100) < 30)
                                grid[i, j].live = true;
                            break;
                    }

                }


            }

        }

        /// <summary>
        /// Method to check num of bombs around cell
        /// </summary>

        public void CalculateLiveNeighbors()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //If the current cell in the grid is a bomb - increment counter
                    if (this.grid[i, j].live)
                    {
                        this.grid[i, j].numOfLive++;
                    }
                    //if cell top left is a bomb - increment counter
                    if (i > 0 && j > 0)
                    {
                        if (this.grid[i - 1, j - 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell left is a bomb - increment counter
                    if (j > 0)
                    {
                        if (this.grid[i, j - 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell lower left is a bomb - increment counter
                    if (i < size - 1 && j > 0)
                    {
                        if (this.grid[i + 1, j - 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell top is a bomb - increment counter
                    if (i > 0)
                    {
                        if (this.grid[i - 1, j].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell top right is a bomb - increment counter
                    if (j < size - 1 && i > 0)
                    {
                        if (this.grid[i - 1, j + 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell right is a bomb - increment counter
                    if (j < size - 1)
                    {
                        if (this.grid[i, j + 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell bottom right is a bomb - increment counter
                    if (i < size - 1 && j < size - 1)
                    {
                        if (this.grid[i + 1, j + 1].live) { this.grid[i, j].numOfLive++; }

                    }
                    //if cell bottom is a bomb - increment counter
                    if (i < size - 1)
                    {
                        if (this.grid[i + 1, j].live) { this.grid[i, j].numOfLive++; }

                    }
                }
            }
            //set up the number of non-live neighbors after we have set up the bombs
            CalculateNumOfNonLive();
        }


        /// <summary>
        /// Method to calculate the number of non-bomb cells
        /// </summary>
        /// <returns></returns>
        public int CalculateNumOfNonLive()
        {
            //loop through the 2d array 
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!grid[i, j].live)
                        numOfNonLive++;
                }
            }
            return numOfNonLive;
        }

        /// <summary>
        /// Method to flood fill for cells with no live neighbors
        /// </summary>
        /// <param name="currentCell"></param>
        public void FloodFill(Cell currentCell)
        {
            //FloodFill until we reach a bomb, base case
            if (!grid[currentCell.row, currentCell.column].live && (!grid[currentCell.row, currentCell.column].visited))
            {
                //visit the cell
                grid[currentCell.row, currentCell.column].visited = true;
                //reduce the number of nonLive remaining
                numOfNonLive--;

                //Recurision occurs, we will floodfill in eight different directions 

                //Go up
                if (currentCell.row > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column]);
                //Go left 
                if (currentCell.column > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row, currentCell.column - 1]);
                //Go right
                if (currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row, currentCell.column + 1]);
                //Go down 
                if (currentCell.row < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column]);
                //Go upper right 
                if (currentCell.row > 0 && currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column + 1]);
                //Go down right 
                if (currentCell.row < size - 1 && currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column + 1]);
                //Go down left
                if (currentCell.row < size - 1 && currentCell.column > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column - 1]);
                //go upper left 
                if (currentCell.column > 0 && currentCell.row > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column - 1]);

            }
        }
        /// <summary>
        /// Helper method to grab the number of bombs
        /// </summary>
        /// <returns>int of live bombs</returns>
        public int CalcLive()
        {
            int live = 0;
            //loop through the grid 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    if (this.grid[i, j].live == true)
                        live++;
                }
            }

            return live;

        }

        /// <summary>
        /// Checks if all bombs are flagged
        /// </summary>
        /// <returns>bool for if all bombs are flagged</returns>
        public bool CheckIfAllFlagged()
        {
            int flagged = 0;
            //loop through the grid 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    if (this.grid[i, j].live && this.grid[i, j].flagged)
                        flagged++;
                }
            }
            if (flagged == this.CalcLive()) return true;
            else return false;
        }

        /// <summary>
        /// Method to flood fill for safe landing
        /// </summary>
        /// <param name="currentCell"></param>
        public void FloodFillSafeLanding(Cell currentCell)
        {
            Random gen = new Random();

            //50% chance of keep going
            if (gen.Next(100) < 100)
            {
                //visit the cell
                grid[currentCell.row, currentCell.column].visited = true;
                numOfNonLive--;

                //Recurision occurs, we will floodfill in four different directions 

                //Go up
                if (currentCell.row > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column]);
                //Go left 
                if (currentCell.column > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row, currentCell.column - 1]);
                //Go right
                if (currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row, currentCell.column + 1]);
                //Go down 
                if (currentCell.row < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column]);
                //Go upper right 
                if (currentCell.row > 0 && currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column + 1]);
                //Go down right 
                if (currentCell.row < size - 1 && currentCell.column < size - 1 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column + 1]);
                //Go down left
                if (currentCell.row < size - 1 && currentCell.column > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row + 1, currentCell.column - 1]);
                //go upper left 
                if (currentCell.column > 0 && currentCell.row > 0 && grid[currentCell.row, currentCell.column].numOfLive == 0)
                    FloodFill(grid[currentCell.row - 1, currentCell.column - 1]);

            }
        }

        /// <summary>
        /// Helper method to cell at an ID
        /// </summary>
        /// <returns>Cell obj</returns>
        public Cell GrabCellFromGrid(int ID)
        {
            Cell cell = null;
            //loop through the grid 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    if (this.grid[i, j].ID == ID)
                        cell = this.grid[i, j];
                }
            }

            return cell;

        }

        /// <summary>
        /// Visit a cell that is a bomb
        /// </summary>
        /// <param name="ID"></param>
        public void VisitBombCell(int ID)
        {
            //loop through the grid 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    if (this.grid[i, j].ID == ID)
                        this.grid[i, j].visited = true;
                }
            }
        }

        /// <summary>
        /// Helper method to visit all the cells in the board
        /// </summary>
        public void VisitAll() 
        {
            //loop through the grid 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    this.grid[i, j].visited = true;
                }
            }
        }

        /// <summary>
        /// Helper method to flag or unflag a cell in the grid using the ID attribute
        /// </summary>
        /// <param name="ID"></param>
        public void FlagCell(int ID)
        {
            //loop through the grid
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    //if all cells that are bombs are flagged
                    if (this.grid[i, j].ID == ID)
                    {
                        if (!this.grid[i, j].visited)
                        {
                            if (this.grid[i, j].flagged)
                                this.grid[i, j].flagged = false;
                            else
                                this.grid[i, j].flagged = true;
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Helper function -  Un-flag the whole baord 
        /// </summary>
        public void UnFlagBoard()
        {
            //loop through the grid
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    this.grid[i, j].flagged = false;
                }
            }
        }

    }
}
