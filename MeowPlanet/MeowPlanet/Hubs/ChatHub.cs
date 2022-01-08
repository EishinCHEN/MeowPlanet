using MeowPlanet.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Hubs
{
    public class ChatHub:Hub
    {
        public MeowContext _meowContext { get; }
        public ChatHub(MeowContext meowContext)
        {
            _meowContext = meowContext;
        }

        static List<UserData> ConnectedUsers = new List<UserData>();
        static List<ChatList> CurrentMessage = new List<ChatList>();

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 傳送訊息給指定userId
        /// </summary>
        /// <param name="userId">SignalR的userId預設默認為ClaimTypes.NameIdentifier</param>
        /// <param name="message">發送的文字訊息</param>
        /// <returns></returns>
        public async Task SendMessageToUser(int receiver, string message)
        {
            var sendTime = DateTime.Now;
            var sender = Context.UserIdentifier;
            await Clients.User(receiver.ToString()).SendAsync("ReceiveMessage", receiver, sender, message, sendTime);
        }
    }
}
