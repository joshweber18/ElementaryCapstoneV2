using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;
using V2Capstone.Models;

namespace V2Capstone.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly GroupChatContextModel _context;
        private readonly UserManager<IdentityUser> _userManager;
        public MessageController(GroupChatContextModel context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{group_id}")]
        public IEnumerable<MessageModel> GetById(int group_id)
        {
            return _context.Message.Where(gb => gb.GroupId == group_id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageViewModel message)
        {
            MessageModel new_message = new MessageModel { AddedBy = _userManager.GetUserName(User), message = message.message, GroupId = message.GroupId };

            _context.Message.Add(new_message);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "us2",
                Encrypted = true
            };
            var pusher = new Pusher(
                "698055",
                "afb5c6b6c93442def1fd",
                "c0829ef0ef2557f45bb3",
                options
            );
            var result = await pusher.TriggerAsync(
                "private-" + message.GroupId,
                "new_message",
            new { new_message },
            new TriggerOptions() { SocketId = message.SocketId });

            return new ObjectResult(new { status = "success", data = new_message });
        }
    }
}