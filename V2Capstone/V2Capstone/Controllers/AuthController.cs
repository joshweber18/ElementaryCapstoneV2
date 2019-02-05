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
    public class AuthController : Controller
    {
        private readonly GroupChatContextModel _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(GroupChatContextModel context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult ChannelAuth(string channel_name, string socket_id)
        {
            int group_id;
            if (!User.Identity.IsAuthenticated)
            {
                return new ContentResult { Content = "Access forbidden", ContentType = "application/json" };
            }

            try
            {
                group_id = Int32.Parse(channel_name.Replace("private-", ""));
            }
            catch (FormatException e)
            {
                return Json(new { Content = e.Message });
            }

            var IsInChannel = _context.UserGroup
                                      .Where(gb => gb.GroupId == group_id
                                            && gb.UserName == _userManager.GetUserName(User))
                                      .Count();

            if (IsInChannel > 0)
            {
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

                var auth = pusher.Authenticate(channel_name, socket_id).ToJson();
                return new ContentResult { Content = auth, ContentType = "application/json" };
            }
            return new ContentResult { Content = "Access forbidden", ContentType = "application/json" };
        }
    }
}