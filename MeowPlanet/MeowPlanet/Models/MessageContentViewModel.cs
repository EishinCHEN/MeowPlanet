using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Models
{
    public class MessageContentViewModel  //聊天內容需要的資料內容
    {
        public int OtherUserId { get; set; }
        public IEnumerable<ChatList> ChatContentList { get; set; }
    }
}
