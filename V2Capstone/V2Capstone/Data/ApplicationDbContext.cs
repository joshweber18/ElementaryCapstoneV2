using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using V2Capstone.Models;

namespace V2Capstone.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TeacherModel> Teacher { get; set; }
        public DbSet<StudentModel> Student { get; set; }
        public DbSet<ParentModel> Parent { get; set; }
        public DbSet<AlertsModel> Alert { get; set; }
    }
}
