using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.MemberManagement
{
    public class Message
    {
        public Int32 Code { set; get; }  //自訂的訊息代號
        public String Msg { set; get; }
        public DateTime Time { set; get; }
    }
}
