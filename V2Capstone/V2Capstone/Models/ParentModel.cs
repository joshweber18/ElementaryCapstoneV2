using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class ParentModel
    {
        [Key]
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsNotified { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
    }
}
