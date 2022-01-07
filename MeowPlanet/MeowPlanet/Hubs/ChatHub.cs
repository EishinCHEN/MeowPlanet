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

        
    }
}
