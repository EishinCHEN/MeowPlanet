using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Models
{
    public class MessageManageViewModel  //聊天列表需要的資料內容
    {
        public int OtherUserId { get; set; }
        public ChatList LastMessage { get; set; }
        public int UnRead { get; set; }
    }
}
