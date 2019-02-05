using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class AnalyticsViewModel
    {
        public StudentModel student { get; set; }
        public ParentModel parent { get; set; }
        public TeacherModel teacher { get; set; }
        public AlertsModel alert { get; set; }
        public List<StudentModel> Students { get; set; }
        public List<ParentModel> Parents { get; set; }
        public List<AlertsModel> Alerts { get; set; }
    }
}
