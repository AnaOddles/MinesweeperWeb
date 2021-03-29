using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
namespace BusinessLayer
{
    class GameDataBusinessService
    {
        public GameDAO gameData { get; set; }

        public GameDataBusinessService()
        {
            this.gameData = new GameDAO();
        }
        public bool SaveGame(String boardString, User user, int clicks, DateTime times)
        {
            return this.gameData.SaveGame(boardString, user, clicks, times);
        }

        public Game LoadGame( User user)
        {
            return this.gameData.LoadGame(user);
        }

    }
}
