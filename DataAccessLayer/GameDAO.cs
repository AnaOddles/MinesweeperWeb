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
        /// Data Access Layer - saving a game state into the database
        /// </summary>
        /// <param name="boardString"></param>
        /// <param name="user"></param>
        /// <param name="clicks"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool SaveGame(Game gameDataObj)
        {
            bool success = false;
            String queryString = "";

            //Query will insert the boardstring, userID, number of clicks, and total time
            queryString = "INSERT INTO dbo.tbl_GameState (board, userID, numOfClicks, seconds) " +
              "VALUES (@boardString, @userID, @numOfClicks, @seconds)";


            //Esatblish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection))
                {
                    //Set each argument passsed in to the correct paramtere of the prepared statement
                    sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = gameDataObj.userID;
                    sqlCommand.Parameters.Add("@boardString", SqlDbType.Text).Value = gameDataObj.boardString;
                    sqlCommand.Parameters.Add("@numOfClicks", SqlDbType.Int).Value = gameDataObj.numOfClicks;
                    sqlCommand.Parameters.Add("@seconds", SqlDbType.Int).Value = gameDataObj.seconds;

                    //Debuging
                    Debug.WriteLine(gameDataObj.boardString);
                    Debug.WriteLine(gameDataObj.userID);
                    Debug.WriteLine(gameDataObj.seconds);

                    try
                    {
                        Debug.WriteLine("Query");

                        //Open connection
                        sqlConnection.Open();

                        //Run query
                        sqlCommand.ExecuteNonQuery();

                        //Close connection
                        sqlConnection.Close();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error");
                        Debug.WriteLine(e.Message);
                        success = false;
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
        public Game LoadGame(int userID)
        {
            //Create an instance of the Game data object
            Game gameObject = new Game();


            String queryString = "";

            //Query string that grabs the most recent inserted game state 
            //because it is ordered by DESC
            //only grab the GameState with the UserID of the current user
            queryString = "SELECT TOP 1 * " +
                           "FROM dbo.tbl_GameState " +
                           "WHERE userID = @userID " +
                           "ORDER BY gameStateID DESC ";

            //Establish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection))
                {

                    //Set each argument passsed in to the correct paramtere of the prepared statement
                    sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                    //Debug
                    Debug.WriteLine(userID);
                    try
                    {
                        //Open connection to SQL Server
                        sqlConnection.Open();

                        //Use DataReader to read the data results from query 
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        //While we can continue to read data from the query result
                        while (sqlDataReader.Read())
                        {
                            //Set the properties for our game object 
                            gameObject.gameStateID = sqlDataReader.GetInt32(0);
                            gameObject.boardString = sqlDataReader.GetString(1);
                            gameObject.numOfClicks = sqlDataReader.GetInt32(2);
                            gameObject.userID = sqlDataReader.GetInt32(3);
                            gameObject.seconds = sqlDataReader.GetInt32(4);
                        }

                        //Debug
                        Debug.WriteLine(gameObject.gameStateID);
                        Debug.WriteLine(gameObject.boardString);
                        Debug.WriteLine(gameObject.userID);
                        Debug.WriteLine(gameObject.numOfClicks);
                        Debug.WriteLine(gameObject.seconds);

                        sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Failure");
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return gameObject;
        }

        /// <summary>
        /// Data access - Grabs the GameState the belongs to current user
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
                        games.Add(new Game((int)reader[0], (string)reader[1], (int)reader[2], (int)reader[3], (int)(reader[4])));
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

        /// <summary>
        /// Data access - grabs all games saved for a user
        /// </summary>
        /// <returns></returns>
        public List<Game> AllGamesForUser(int userID)
        {
            //Assume nothing is found
            List<Game> games = new List<Game>();


            string sqlStatement = "SELECT * FROM dbo.tbl_GameState " +
                                            "WHERE userID = @userID " +
                                            "ORDER BY gameStateID DESC";

            //Establish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {

                    //Set each argument passsed in to the correct paramtere of the prepared statement
                    sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                    //Debug
                    Debug.WriteLine(userID);
                    try
                    {
                        //Open connection to SQL Server
                        sqlConnection.Open();

                        //Use DataReader to read the data results from query 
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        //While we can continue to read data from the query result
                        while (sqlDataReader.Read())
                        {
                            //Set the properties for our game object 
                            Game game = new Game(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1),
                                sqlDataReader.GetInt32(4), sqlDataReader.GetInt32(2), sqlDataReader.GetInt32(3));

                            games.Add(game);
                        }
                        
                        sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Failure");
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            //returns list of games
            return games;
        }

        /// <summary>
        /// Data Access - grabs a game with 
        /// provided gamestateID 
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        public Game LoadOneGame(int gameStateID)
        {
            //Create an instance of the Game data object
            Game gameObject = new Game();


            String queryString = "";

            //Query string that grabs the game query woith provided ID
            queryString = "SELECT * " +
                           "FROM dbo.tbl_GameState " +
                           "WHERE gameStateID = @gameStateID;";

            //Establish a connection with the connection string
            using (SqlConnection sqlConnection = new SqlConnection(dbGameConnString))
            {
                //Create a sql command with the query string we created
                using (SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection))
                {

                    //Set each argument passsed in to the correct paramtere of the prepared statement
                    sqlCommand.Parameters.Add("@gameStateID", SqlDbType.Int).Value = gameStateID;

                    //Debug
                    Debug.WriteLine(gameStateID);
                    try
                    {
                        //Open connection to SQL Server
                        sqlConnection.Open();

                        //Use DataReader to read the data results from query 
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        //While we can continue to read data from the query result
                        while (sqlDataReader.Read())
                        {
                            //Set the properties for our game object 
                            gameObject.gameStateID = sqlDataReader.GetInt32(0);
                            gameObject.boardString = sqlDataReader.GetString(1);
                            gameObject.numOfClicks = sqlDataReader.GetInt32(2);
                            gameObject.userID = sqlDataReader.GetInt32(3);
                            gameObject.seconds = sqlDataReader.GetInt32(4);
                        }

                        //Debug
                        Debug.WriteLine(gameObject.gameStateID);
                        Debug.WriteLine(gameObject.boardString);
                        Debug.WriteLine(gameObject.userID);
                        Debug.WriteLine(gameObject.numOfClicks);
                        Debug.WriteLine(gameObject.seconds);

                        sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Failure");
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return gameObject;
        }

        /// <summary>
        /// Data Access - Deletes a save from the datbase
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteSave(int gameStateID)
        {
            bool isSuccessful = false;

            //Open the connection to the database 
            using (SqlConnection conn = new SqlConnection(dbGameConnString))
            {
                //grab the stored procedure comamnd and use it for the query
                using (SqlCommand cmd = new SqlCommand("usp_DeleteSave", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Open up the stored procedure and review the parameters 
                    //required to be passed into the procedure 
                    cmd.Parameters.AddWithValue("@GameStateID", gameStateID);

                    //Run the query 
                    try
                    {
                        conn.Open();
                        //Check if there was a row affected 
                        if (cmd.ExecuteNonQuery() > 0)
                            isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return isSuccessful;

            }

        }
    }
}
