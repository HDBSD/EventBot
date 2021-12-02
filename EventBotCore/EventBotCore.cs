using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBotCore.PythonScripting;
using EventBotCore.IRC ;
using Microsoft.Data.Sqlite;

namespace EventBotCore
{
    public class EventBotCore
    {

        private readonly PythonScriptEngine scriptEngine;
        private readonly IRC.IRC twitchClient;
        SqliteConnection dbConnection;

        public EventBotCore(string db)
        {
            scriptEngine = new PythonScriptEngine();
            twitchClient = new IRC.IRC();
            dbConnection = new SqliteConnection($@"Data Source=.\db\{db}");
        }

        ~EventBotCore()
        {

        }

        public void ConnectToChat(string Username, string Auth, string channel)
        {
            twitchClient.Connect(Username, Auth, channel);
            twitchClient.PassScriptEngine(scriptEngine);
        }

        public void DisconnectFromChat()
        {
            twitchClient.Disconnect();
        }

        public void LoadScript(string path)
        {
            scriptEngine.loadScript(path);
        }


#if DEBUG
        public void simulateMessage(string user, string message)
        {
            var data = new IRC.Data()
            {
                User = "1",
                UserName = user,
                Message = message,
                RawData = $"{user}: {message}",
                ServiceType = "TTV"
            };

            scriptEngine.performExecute(data);
        }
#endif


    }
}
