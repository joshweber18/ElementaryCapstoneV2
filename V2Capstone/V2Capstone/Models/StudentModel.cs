using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class StudentModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int StudentId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Science Grade")]
        public double? ScienceGrade { get; set; }


        [Display(Name = "Math Grade")]
        public double? MathGrade { get; set; }

 
        [Display(Name = "History Grade")]
        public double? HistoryGrade { get; set; }

        [Display(Name = "Math Rank")]
        public int? MathRank { get; set; }

      
        [Display(Name = "Science Rank")]
        public int? ScienceRank { get; set; }

  
        [Display(Name = "History Rank")]
        public int? HistoryRank { get; set; }

    
        [Display(Name = "OverallRank")]
        public int? OverallRank { get; set; }

        [Display(Name = "Overall Grade")]
        public double? OverallGrade { get; set; }

        [Display(Name = "Grade Level")]
        public int? GradeLevel { get; set; }

        public bool IsNotified { get; set; }

        public bool UpdatedGrade { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }

   
        [ForeignKey("Parent")]
        public int ParentId { get; set; }
        public ParentModel Parent { get; set; }

   
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
    }
}
