using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
namespace BusinessLayer
{
    public class GameDataBusinessService
    {
        //Class properties
        public GameDAO gameData { get; set; }

        /// <summary>xs
        /// Class constructor
        /// </summary>
        public GameDataBusinessService()
        {
            this.gameData = new GameDAO();
        }
        
       /// <summary>
       /// Business layer - saving a game
       /// </summary>
       /// <param name="gameDataObj"></param>
       /// <returns></returns>
        public bool SaveGame(Game gameDataObj)
        {
            return this.gameData.SaveGame(gameDataObj);
        }

        /// <summary>
        /// Business layer - load a game
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Game LoadGame(int userID)
        {
            return this.gameData.LoadGame(userID);
        }

        /// <summary>
        /// Business later - grab all saves for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Game> AllGamesForUser(int userID)
        {
            return this.gameData.AllGamesForUser(userID);
        }

        /// <summary>
        /// Business Layer - grabs one save 
        /// with provided gameStateID
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        public Game LoadOneGame(int gameStateID)
        {
            return this.gameData.LoadOneGame(gameStateID);
        }

        /// <summary>
        /// Business layer - deletes a save
        /// </summary>
        /// <param name="gameStateID"></param>
        /// <returns></returns>
        public bool DeleteSave(int gameStateID)
        {
            return this.gameData.DeleteSave(gameStateID);
        }

    }
}
