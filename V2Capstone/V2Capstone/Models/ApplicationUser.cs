using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class ApplicationUser : IdentityUser
    {

        [NotMapped]
        public bool isParent { get; set; }

        [NotMapped]
        public bool isTeacher { get; set; }

        [NotMapped]
        public bool isStudent { get; set; }
    }
}
