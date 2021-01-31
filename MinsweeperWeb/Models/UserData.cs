using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;

namespace MinsweeperWeb.Models
{
    public class UserData
    {
        UserBusinessService userDAL = new UserBusinessService();
        /// <summary>
        /// Sending the data to the Business Layer to add one employee
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool RegisterUser(User user)
        {
            return userDAL.RegisterUser(user);
        }

        /// <summary>
        /// Sending the data to the Business layer to check for login
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public int LoginUser(Authentication auth)
        {
            return userDAL.LoginUser(auth);
        }

        /// <summary>
        /// Uses the Authentication obj to the Business Layer and grabs the user
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public User GrabUserByID(Authentication auth)
        {
            return userDAL.GrabUserByID(auth);
        }
    }
}
