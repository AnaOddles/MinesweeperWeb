using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace DataAccessLayer
{
    public class GameDAO
    {

        //Define the class attribute 
        public string dbGameConnString { get; set; }

        /// <summary>
        /// Constructor to set the connection string
        /// </summary>
        public GameDAO()
        {
            this.dbGameConnString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MinesweeperWeb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
        
        /// <summary>
        /// Data access layer for saving a game state into the database
        /// </summary>
        /// <param name="boardString"></param>
        /// <param name="user"></param>
        /// <param name="clicks"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool SaveGame(String boardString, User user, int clicks, DateTime times)
        {
            bool success = false;
            //Grab the gameStateID from the usertable 
            int userID = user.UserID;

            String queryString = "";

            //int gameStateID = GrabGameStateFromUser(userID);

            //Depeding is user is logged in - use their userID or not
            if (userID > 0)
            {
                //Query will insert the boardstring, userID, number of clickss, and total time
                queryString = "INSERT INTO dbo.tbl_GameState (board, userID, numOfClicks, time) " +
                  "VALUES (@boardString, @userID, @numOfClicks, @time)";
            }
            else
            {
                queryString = "INSERT INTO dbo.tbl_GameState (board, numOfClicks, time) " +
                      "VALUES (@boardString, @numOfClicks, @time)";
            }

            //Esatblish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection))
                {
                    //Depending if user logged in - if, else
                    if (userID > 0)
                    {
                        //Set each argument passsed in to the correct paramtere of the prepared statement
                        sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                        sqlCommand.Parameters.Add("@boardString", SqlDbType.Text).Value = boardString;
                        sqlCommand.Parameters.Add("@numOfClicks", SqlDbType.Int).Value = clicks;
                        sqlCommand.Parameters.Add("@time", SqlDbType.DateTime).Value = times;
                    }
                    else
                    {
                        sqlCommand.Parameters.Add("@boardString", SqlDbType.Text).Value = boardString;
                        sqlCommand.Parameters.Add("@numOfClicks", SqlDbType.Int).Value = clicks;
                        sqlCommand.Parameters.Add("@time", SqlDbType.DateTime).Value = times;

                    }


                    Debug.WriteLine(boardString);
                    Debug.WriteLine(user.UserID);
                    Debug.WriteLine(times);

                    try
                    {
                        Debug.WriteLine("Query");
                        
                        sqlConnection.Open();
                        //Run query
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error");
                        Console.WriteLine("Failure");
                        Debug.WriteLine(e.Message);
                    }
                    

                }
            }

            return success;
        }

        /// <summary>
        /// Data Access - loads the most recently saved gamestate from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Game LoadGame(User user)
        {
            Game gameObject = new Game();
            //Grab the gameStateID from the usertable 
            int userID = user.UserID;

            String queryString = "";


                //Queru string that grabs the most recent inserted game state 
                //because it is ordered by DESC
                //only grab the GameState with the UserID of the current user
                queryString = "SELECT TOP 1 * " +
                               "FROM dbo.tbl_GameState " +
                               "WHERE userID = @userID " +
                               "ORDER BY gameStateID DESC ";



            //Esatblish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection))
                {

                    //Set each argument passsed in to the correct paramtere of the prepared statement
                    sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    



                    Debug.WriteLine(user.UserID);
                    try
                    {
                        sqlConnection.Open();

                        //Use DataReader to read the data results from query 
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            gameObject.gameStateID = sqlDataReader.GetInt32(0);
                            gameObject.boardString = sqlDataReader.GetString(1);
                            gameObject.userID = sqlDataReader.GetInt32(4);

                        }

                        sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failure");
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            return gameObject;
        }

        /// <summary>
        /// Grabs the GameState the belongs to current user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GrabGameStateFromUser(int userID)
        {
            //Open the connection to the database 
            using (SqlConnection conn = new SqlConnection(dbGameConnString))
            {
                //grab the stored procedure comamnd and use it for the query
                using (SqlCommand cmd = new SqlCommand("usp_GrabGameState", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Open up the stored procedure and review the parameters 
                    //required to be passed into the procedure 
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    //Run the query 
                    try
                    {
                        conn.Open();
                        //Read the query out so we can interate and make a User 
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        //Iterate through the data rows that were read and add properties to user
                        while (dataReader.Read())
                        {
                            int gameStateID = Convert.ToInt32(dataReader["GameStateID"]);


                            return gameStateID;
                        }
                        return 0;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;

                    }
                }

            }

        }

        /// <summary>
        /// Data access - grabs all games saved
        /// </summary>
        /// <returns></returns>
        public List<Game> AllGames()
        {
            //Assume nothing is found
            List<Game> games = new List<Game>();

            
            string sqlStatement = "SELECT * FROM dbo.tbl_GameState";

            //Open the connection to the database 
            using (SqlConnection connection = new SqlConnection(this.dbGameConnString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                try
                {
                    connection.Open();
                    //Read the query out so we can interate and make a Game 
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //Iterate through the data rows that were read and add properties to Game
                        games.Add(new Game((int)reader[0], (string)reader[1], (DateTime)reader[2], (int)reader[3], (int)(reader[4])));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                };
            }
            //returns list of games
            return games;
        }



    }
}
