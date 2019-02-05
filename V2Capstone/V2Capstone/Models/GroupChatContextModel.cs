using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class GroupChatContextModel : DbContext
    {
        public GroupChatContextModel(DbContextOptions<GroupChatContextModel> options)
            : base(options)
        {
        }

        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<MessageModel> Message { get; set; }
        public DbSet<UserGroupModel> UserGroup { get; set; }
    }
}
