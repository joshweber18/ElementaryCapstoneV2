using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;
using V2Capstone.Models;

namespace V2Capstone.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly GroupChatContextModel _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GroupController(GroupChatContextModel context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<UserGroupViewModel> GetAll()
        {
            var groups = _context.UserGroup
                          .Where(gp => gp.UserName == _userManager.GetUserName(User))
                          .Join(_context.Groups, ug => ug.GroupId, g => g.ID, (ug, g) =>
                                         new UserGroupViewModel()
                                         {
                                             UserName = ug.UserName,
                                             GroupId = g.ID,
                                             GroupName = g.GroupName
                                         })
                           .ToList();

            return groups;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewGroupViewModel group)
        {
            if (group == null || group.GroupName == "")
            {
                return new ObjectResult(
                    new { status = "error", message = "incomplete request" }
                );
            }
            if ((_context.Groups.Any(gp => gp.GroupName == group.GroupName)) == true)
            {
                return new ObjectResult(
                    new { status = "error", message = "group name already exist" }
                );
            }

            GroupModel newGroup = new GroupModel { GroupName = group.GroupName };
            // Insert this new group to the database...
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
            //Insert into the user group table, group_id and user_id in the user_groups table...
            foreach (string UserName in group.UserNames)
            {
                _context.UserGroup.Add(
                    new UserGroupModel { UserName = UserName, GroupId = newGroup.ID }
                );
                _context.SaveChanges();
            }
            var options = new PusherOptions
            {
                Cluster = "us2",
                Encrypted = true
            };
            var pusher = new Pusher(
                "698055",
                "afb5c6b6c93442def1fd",
                "c0829ef0ef2557f45bb3",
            options);
            var result = await pusher.TriggerAsync(
                "group_chat", //channel name
                "new_group", // event name
            new { newGroup });
            return new ObjectResult(new { status = "success", data = newGroup });
        }
    }
}