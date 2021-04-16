using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinsweeperWeb.Utility
{
    public class MyLogger : ILogger
    {
        //Singleton design pattern

        private static MyLogger instance;
        private static Logger logger;

        //Ensure that we are only using one instance
        public static MyLogger GetInstance()
        {
            if (instance == null)
            { 
                instance = new MyLogger();
            }
            return instance;
        }

        //Grab the nLog logger with configuration
        public Logger GetLogger()
        {
            if (MyLogger.logger == null)
                MyLogger.logger = LogManager.GetLogger("MinesweeperRule");
            return MyLogger.logger;
        }

        //logging Methods
        public void Debug(string message)
        {
            GetLogger().Debug(message);
        }

        public void Error(string message)
        {
            GetLogger().Error(message);
        }

        public void Info(string message)
        {
            GetLogger().Info(message);
        }

        public void Warning(string message)
        {
            GetLogger().Warn(message);
        }
    }
}
