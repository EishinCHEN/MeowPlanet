using MeowPlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Controllers
{
    public class MeowChatController : Controller
    {
        private MeowContext _meowChatContext { get; }
        public MeowChatController(MeowContext meowChatContext)
        {
            _meowChatContext = meowChatContext;
        }
        public IActionResult MessageManage()
        {
            return View();
        }

        public async Task<IActionResult> MessageContent(int other)
        {
            var returndata = new MessageContentViewModel();
            returndata.OtherUserId = other;
            returndata.ChatContentList = await GetMessageSenderList(1, other);
            return View(returndata);
        }

        public IActionResult ChatBox()
        {
            return View();
        }


        //拿到所有的聊天訊息
        [HttpGet]
        public async Task<IEnumerable<ChatList>> GetMessageList()
        {
            return _meowChatContext.ChatLists;
        }

        //拿到與其他人聊天的所有歷史訊息
        [HttpGet]
        public async Task<IEnumerable<ChatList>> GetMessageSenderList(int sender, int receiver)
        {
            var senderlist = await _meowChatContext.ChatLists.Where(u =>
            (u.Sender == sender && u.Receiver == receiver) || (u.Sender == receiver && u.Receiver == sender)).ToListAsync();

            if (senderlist == null || senderlist.Count() == 0)
            {
                Response.StatusCode = 404;
            }
            return senderlist;
        }

        //拿到與每一個聊天的人的最後一筆資料(做成聊天列表)
        [HttpGet]
        public async Task<IEnumerable<MessageManageViewModel>> GetLastMessage(int sender, int receiver)
        {
            //先找出Sender跟Receiver中有自己的資料
            var historyMessageList = await _meowChatContext.ChatLists.Where(u => (u.Sender == sender) || (u.Receiver == sender)).ToListAsync();
            var otherUsersIdList = historyMessageList
                                    .Select(u => u.Receiver)
                                    .Distinct()  //從自己聊天的所有訊息中選出不重複的Receiver對象
                                    .Union(historyMessageList.Select(u => u.Sender).Distinct()) //並從自己聊天的所有訊息中選出不重複的Sender對象取出聯集
                                    .Distinct()  //選出不重複的Receiver、Sender聯集
                                    .Where(u => u != sender);  //去掉自己

            var returnList = new List<MessageManageViewModel>();
            foreach (var other in otherUsersIdList)//跑迴圈取出自己與每一位聊天對象的資料
            {
                var returnData = new MessageManageViewModel();
                returnData.OtherUserId = other;
                //取出與每一位聊天對象的最後一則訊息
                returnData.LastMessage = historyMessageList.Where(u => (u.Sender == other) || (u.Receiver == other)).OrderByDescending(n => n.SendTime).FirstOrDefault();
                //取出Sender是對方同時訊息尚未讀取的數量
                returnData.UnRead = historyMessageList.Where(u => u.Sender == other && u.IsRead == false).Count();
                returnList.Add(returnData);
            }

            if (returnList == null || returnList.Count() == 0)
            {
                Response.StatusCode = 404;
            }
            return returnList;
        }
    }
}
