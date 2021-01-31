using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusinessLayer
{
    public class UserBusinessService
    {
        //Instantiate our UserDAO
        UserDAO userDAO = new UserDAO();

        
        /// <summary>
        /// Register method -passing in user model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool RegisterUser(User user)
        {
            return userDAO.AddUser(user);
        }

        /// <summary>
        /// Used to auth user
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public int LoginUser(Authentication auth)
        {
            return userDAO.AuthenticateUser(auth);
        }

        /// <summary>
        /// Uses the auth model with UserID to check for user
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public User GrabUserByID(Authentication auth)
        {
            return userDAO.GrabUserByID(auth);
        }
    }
}
