using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class ChatList
    {
        public int MessageId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public DateTime SendTime { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public bool IsRead { get; set; }

        public virtual UserData SenderNavigation { get; set; }
    }
}
