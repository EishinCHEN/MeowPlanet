﻿using MeowPlanet.Models;
using Microsoft.AspNetCore.Authorization;
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
        private MeowContext _meowContext { get; }
        public MeowChatController(MeowContext meowContext)
        {
            _meowContext = meowContext;
        }

        [Authorize]
        public async Task<IActionResult> MessageManage()
        {
            var loginId = Convert.ToInt32(HttpContext.User.Identity.Name);
            //判斷登入者是否有任何歷史訊息
            var messagecount = await _meowContext.ChatLists
                .Where(u => (u.Sender == loginId || u.Receiver == loginId)).CountAsync();
            if (messagecount == 0)
            {
                Console.WriteLine("沒有訊息");
                return View("MessageNone");
            }
            else
            {
                Console.WriteLine("有訊息");
                return View("MessageManage");
            }

        }

        [Authorize]
        public IActionResult MessageNone()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MessageContent(int other, int sender)
        {
            var loginId = Convert.ToInt32(HttpContext.User.Identity.Name);
            var returndata = new MessageContentViewModel();
            returndata.OtherUserId = other;
            returndata.OtherUser = await GetUserDataById(other);
            returndata.UserRole = await GetUserRole();  //得到sender的role
            returndata.ChatContentList = await GetMessageSenderList(loginId, other);
            //消除Context有關更動的紀錄，以免不必要存取已修改過的台北時間
            DetachAllContextChanges();

            var unread = await _meowContext.ChatLists
                .Where(u => (u.Sender == other) && (u.Receiver ==loginId) && (u.IsRead == false))
                //.GroupBy(u => new {u.Sender})
                .ToListAsync();

            if (unread != null)
            {
                //var result = unread.GroupBy(x => new { x.Sender });
                unread.ForEach(x => x.IsRead = true);
                var savecounts = _meowContext.SaveChanges();
                Console.WriteLine($"資料庫已讀成功，共{savecounts}筆");
            }
            else
            {
                Console.WriteLine("資料庫沒有訊息未讀");
                return View(returndata);         
            }
            return View(returndata);
        }

        
        public IActionResult ChatBox()
        {
            return View();
        }


        //拿到所有的聊天訊息
        //[HttpGet]
        //public async Task<IEnumerable<ChatList>> GetMessageList()
        //{
        //    return _meowContext.ChatLists;
        //}

        //拿到與其他人聊天的所有歷史訊息
        [HttpGet]
        public async Task<IEnumerable<ChatList>> GetMessageSenderList(int sender, int receiver)
        {
            var senderlist = await _meowContext.ChatLists
                .Where(u => (u.Sender == sender && u.Receiver == receiver)
                        || (u.Sender == receiver && u.Receiver == sender))
                .ToListAsync();

            //把所有歷史訊息的sendtime換成台北時間
            senderlist.ForEach(t => t.SendTime = TimeZoneInfo.ConvertTimeFromUtc(t.SendTime, TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time")));

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
            var historyMessageList = await _meowContext.ChatLists.Where(u => (u.Sender == sender) || (u.Receiver == sender)).ToListAsync();
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
                returnData.OtherUser = await GetUserDataById(other);

                //取出與每一位聊天對象的最後一則訊息
                returnData.LastMessage = historyMessageList
                    .Where(u => (u.Sender == other) || (u.Receiver == other))
                    .OrderByDescending(n => n.SendTime)
                    .FirstOrDefault();

                //把sendtime換成台北時間
                returnData.LastMessage.SendTime = TimeZoneInfo.ConvertTimeFromUtc(returnData.LastMessage.SendTime, TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time"));

                //取出Sender是對方同時訊息尚未讀取的數量
                returnData.UnRead = historyMessageList
                    .Where(u => u.Sender == other && u.IsRead == false)
                    .Count();
                returnList.Add(returnData);
            }

            if (returnList == null || returnList.Count() == 0)
            {
                Response.StatusCode = 404;
            }
            return returnList;
        }

        //用UserId取出相對Userdata需要的欄位資料
        internal async Task<object> GetUserDataById(int userId)
        {
            return await _meowContext.UserDatas
                    .Where(u => u.UserId == userId)
                    .Select(u => new { u.RealName, PersonalPhoto = Url.Content(u.PersonalPhoto), u.Job, u.Salary, u.AcceptableAmount,
                        u.Merrage, u.OtherPets, u.KeepPets, u.Agents, u.RelationShip,
                        Over20 = ((DateTime.Now.Subtract(u.Birthday).Days/365 ) > 20 ?"是":"否") })
                    .FirstOrDefaultAsync();
        }

        
        //取出登入者的身分
        [HttpGet]
        public async Task<int> GetUserRole()
        {
            var loginId = Convert.ToInt32(HttpContext.User.Identity.Name);
            var userrole = await _meowContext.RoleManagements.Where(u => u.UserId == loginId).Select(u => u.RoleId).ToListAsync();
            var result = userrole.Contains(2) ? 2 : 1;
            return result;
        }

        private void DetachAllContextChanges()
        {
            _meowContext.ChangeTracker.DetectChanges();
            //Console.WriteLine("Context追蹤變更取消前:");
            //Console.WriteLine(_meowContext.ChangeTracker.DebugView.ShortView);
            //Context追蹤取消
            _meowContext.ChangeTracker.Entries().ToList().ForEach(x => x.State = EntityState.Detached);

            _meowContext.ChangeTracker.DetectChanges();
            //Console.WriteLine("Context追蹤變更取消後:");
            //Console.WriteLine(_meowContext.ChangeTracker.DebugView.ShortView);
        }

    }
}
