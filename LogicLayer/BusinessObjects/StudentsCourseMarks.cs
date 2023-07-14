using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessLogic;

namespace LogicLayer.BusinessObjects
{
    public class StudentsCourseMarks
    {
        public decimal Id { get; set; }
        public int StudentId { get; set; }
        public string Roll { get; set; }
        public int StudentCourseHistoryId { get; set; }
        public int ExamTemplateTypeId { get; set; }

         public decimal TotalObtainedMarks { get; set; }

        public List<Marks> StudentMarks { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public StudentsCourseMarks()
        {
            StudentMarks = new List<Marks>();
        }

        public StudentCourseHistory CourseHistory
        {
            get
            {
                StudentCourseHistory cHistory = StudentCourseHistoryManager.GetById(StudentCourseHistoryId);
                return cHistory;
            }
        }
    }
}
