using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web.Http.Description;

namespace MinsweeperWeb.Controllers
{
    //Annotaion to declare controller as an API Controller for resource
    [ApiController]
    //Establoish the route as 'api/gamesapi'
    [Route("api/[controller]")]
    public class GamesAPIController : Controller
    {
        GameDAO repository = null;

        //Class constructor loads in GAMEDAO
        public GamesAPIController()
        {
            repository = new GameDAO();
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //Return a lis of Games items.
        [ResponseType(typeof(List<Game>))]
        public IEnumerable<Game> Index()
        {
            //Call the DAO and get all the games
            return repository.AllGames();

        }
    }
}
