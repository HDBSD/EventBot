using EventBotCore.PythonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwitchLib.Client;
using TwitchLib.Api;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace EventBotCore.IRC
{

    public class Data
    {

        public string User {  get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string RawData { get; set; }
        public string ServiceType { get; set; }

        public int MessageType { get; set; }


        public bool IsChatMessage()
        {
            if (MessageType == 0)
                return true;
            else
                return false;
        }

        public bool IsRawData()
        {
            //TODO: implement propperly for compat
            return false;
        }

        public bool IsFromTwitch()
        {
            return true;
        }

        public bool IsFromYoutube()
        {
            return false;
        }

        public bool IsFromMixer()
        {
            return false;
        }

        public bool IsFromDiscord()
        {
            return false;
        }

        public bool IsWhisper()
        {
            if (MessageType == 0)
                return false;
            else
                return true;
        }

        public string GetParam(int id)
        {
            return this.Message.Split(' ')[id].Trim();
        }

        public string GetParamCount()
        {
            return this.Message.Split(' ').Count().ToString();
        }

    }

    internal class IRC
    {

        internal TwitchClient client;
        internal PythonScriptEngine engine;

        public async void Connect(string botUserName, string BotAuthToken, string targetChannel)
        {
            ConnectionCredentials credentials = new ConnectionCredentials(botUserName, BotAuthToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, targetChannel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnUserJoined += Client_OnUserJoined;
            client.OnUserLeft += Client_OnUserLeft;
            client.OnConnected += Client_OnConnected;

            client.Connect();

            var API = new TwitchAPI();
            API.Settings.ClientId = "cudnohm8ky367khkcxkna4qwqq03ez";
            API.Settings.Secret = "eulc19jnh7rau4d8oev99smzgl3zby";
            var task = await API.Undocumented.GetChattersAsync(targetChannel);
            
        }

        public void Disconnect()
        {
            client.Disconnect();
            client.OnLog -= Client_OnLog;
            client.OnJoinedChannel -= Client_OnJoinedChannel;
            client.OnMessageReceived -= Client_OnMessageReceived;
            client.OnWhisperReceived -= Client_OnWhisperReceived;
            client.OnNewSubscriber -= Client_OnNewSubscriber;
            client.OnUserJoined -= Client_OnUserJoined;
            client.OnUserLeft -= Client_OnUserLeft;
            client.OnConnected -= Client_OnConnected;
            client = null;
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
#if DEBUG
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
#endif
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("[INF] bot connected to chat");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"[MSG] {e.ChatMessage.Username}: {e.ChatMessage.Message}");

            if (engine == null)
                return;

            Data messageObject = new Data
            {
                User = e.ChatMessage.UserId,
                UserName = e.ChatMessage.Username,
                Message = e.ChatMessage.Message,
                RawData = e.ChatMessage.RawIrcMessage,
                ServiceType = "twitch",
                MessageType = 0
            };

            engine.performExecute(messageObject);

        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Console.WriteLine($"[WSP] {e.WhisperMessage.Username}: {e.WhisperMessage.Message}");

            if (engine == null)
                return;

            Data messageObject = new Data
            {
                User = e.WhisperMessage.UserId,
                UserName = e.WhisperMessage.Username,
                Message = e.WhisperMessage.Message,
                RawData = e.WhisperMessage.RawIrcMessage,
                ServiceType = "twitch",
                MessageType = 1
            };

            engine.performExecute(messageObject);
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            Console.WriteLine($"[SUB] {e.Subscriber.DisplayName} Subbed");
            
            /*
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
            */
        }

        private void Client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            //e.Username;
        }

        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            
        }

        public void PassScriptEngine(PythonScriptEngine scriptEngine)
        {
            engine = scriptEngine;
        }
    }
}
