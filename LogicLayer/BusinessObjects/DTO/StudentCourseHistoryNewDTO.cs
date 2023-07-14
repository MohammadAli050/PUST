using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class StudentCourseHistoryNewDTO
    {
        public int CourseHistoryId { get; set; }
        public string Session { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public int ExamYear { get; set; }
        public string ExamName { get; set; }
        public string YearName { get; set; }
        public string Semester { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public decimal CourseCredit { get; set; }
        public string ObtainedGrade { get; set; }
        public decimal ObtainedGPA { get; set; }
        public string CourseStatusCode { get; set; }
        public string CourseStatus { get; set; }


        public string ExamDetail
        {
            get
            {
                //return "Semester" + " " + Year.ToString();

                return YearNo.ToString() + " Year " + SemesterNo + " Semester " + ExamName + "-" + ExamYear;
            }
        }
    }

}
