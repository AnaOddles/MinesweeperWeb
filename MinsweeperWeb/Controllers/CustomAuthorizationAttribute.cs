using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
namespace MinsweeperWeb.Controllers
{
    //Class that checks for user login to access saves
    internal class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Grab the username from the session
            string username = context.HttpContext.Session.GetString("username");

            //If the session is empty - redirect back to login
            if (username == null)
            {
                //Return the user back to login
                context.Result = new RedirectResult("/login");
            }
        }
    }
}