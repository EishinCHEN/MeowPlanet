﻿using MeowPlanet.Models;
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
        /// <param name="receiver">SignalR的userId預設默認為ClaimTypes.NameIdentifier</param>
        /// <param name="message">發送的文字訊息</param>
        /// <returns></returns>
        public async Task SendMessageToUser(int receiver, string message, DateTime sendtime, int sender)
        {
            await Clients.User(receiver.ToString()).SendAsync("ReceiveMessage", receiver, sender, sendtime, message);
            var newmessagelist = new ChatList();
            newmessagelist.Receiver = receiver;
            newmessagelist.Sender = sender;
            newmessagelist.SendTime = sendtime;
            newmessagelist.Message = message;
            newmessagelist.IsRead = false;
            _meowContext.ChatLists.Add(newmessagelist); //將訊息寫入資料庫
            await _meowContext.SaveChangesAsync();
        }
    }
}
