using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class AlertsModel
    {
        [Key]
        public int AlertId { get; set; }

        [Display(Name = "Alerts")]
        public string Message { get; set; }

        public string Recipients { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }
    }
}
