using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer
{
    public class UserDAO
    {

        //Define the class attribute 
        public string dbUserConn { get; set; }

        /// <summary>
        /// Constructor to set the connection string
        /// </summary>
        public UserDAO()
        {
            this.dbUserConn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MinesweeperWeb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// Data Access - Adds a user to Database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            bool isSuccessful = false;

            //Open the connection to the database 
            using (SqlConnection conn = new SqlConnection(dbUserConn))
            { 
                //grab the stored procedure comamnd and use it for the query
                using(SqlCommand cmd = new SqlCommand("usp_InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Open up the stored procedure and review the parameters 
                    //required to be passed into the procedure 
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@Age", user.Age);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@State", user.State);

                    //Run the query 
                    try
                    {
                        conn.Open();
                        //Check if there was a row affected 
                        if (cmd.ExecuteNonQuery() > 0)
                            isSuccessful = true;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return isSuccessful;

            }
        
        }

      /// <summary>
      /// Data Access - Used to authenticate user and see if exists 
      /// </summary>
      /// <param name="auth"></param>
      /// <returns></returns>
        public int AuthenticateUser(Authentication auth)
        {
            //User user = null;
            //Open the connection to the database 
            using (SqlConnection conn = new SqlConnection(dbUserConn))
            {
                //grab the stored procedure comamnd and use it for the query
                using (SqlCommand cmd = new SqlCommand("usp_LoginUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Open up the stored procedure and review the parameters 
                    //required to be passed into the procedure 
                    cmd.Parameters.AddWithValue("@Username", auth.Username);
                    cmd.Parameters.AddWithValue("@Password", auth.Password);

                    //Run the query 
                    try
                    {
                        conn.Open();
                        //Read the query out so we can interate and make a User 
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        //Iterate through the data rows that were read 
                        while (dataReader.Read())
                        {
                            auth.UserID = Convert.ToInt32(dataReader["UserID"]);
                            return auth.UserID;
                            
                        }
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                }

            }

        }

        /// <summary>
        /// Data Access - Grabs User ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GrabUserByID(int userID)
        {
            User user = new User();
            //Open the connection to the database 
            using (SqlConnection conn = new SqlConnection(dbUserConn))
            {
                //grab the stored procedure comamnd and use it for the query
                using (SqlCommand cmd = new SqlCommand("usp_GrabUserByID", conn))
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
                            user.UserID = Convert.ToInt32(dataReader["UserID"]);
                            user.FirstName = dataReader["FirstName"].ToString();
                            user.LastName = dataReader["LastName"].ToString();
                            user.Gender = dataReader["Gender"].ToString();
                            user.State = dataReader["State"].ToString();
                            user.Age = Convert.ToInt32(dataReader["Age"]);
                            user.Email = dataReader["EmailAddress"].ToString();
                            user.Username = dataReader["Username"].ToString();
                            user.Password = dataReader["Password"].ToString();

                            return user;
                        }
                        return null;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                        
                    }
                }

            }

        }

        



    }
}
