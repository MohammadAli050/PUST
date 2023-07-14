using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessLogic;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamCommitteeDashboardDTO
    {
        public int AcaCal_SectionID { get; set; }
        public int ExamSetupId { get; set; }
        public int CourseID { get; set; }
        public int VersionID { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public string ExamName { get; set; }
        public int Year { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TearcherOneName { get; set; }
        public bool IsContinousMarkSubmit { get; set; }
        public DateTime? FinalSubmissionDate { get; set; }

        public int ThirdExaminationEligibleStudentCount { get; set; }
        public int TotalStudent { get; set; }

        public int FirstExaminerMarkSubmissionStatus { get; set; }
        public int SecondExaminerMarkSubmissionStatus { get; set; }
        public int ThirdExaminerMarkSubmissionStatus { get; set; }

        public int ExaminerMarkSubmissionStatus { get; set; }

        public int FirstExaminerId { get; set; }
        public int SecondExaminerId { get; set; }
        public int ThirdExaminerId { get; set; }

        public string FirstExaminerName { get; set; }
        public string SecondExaminerName  { get; set; }
        public string ThirdExaminerName  { get; set; }

        public DateTime? FirstExaminerMarkSubmissionStatusDate { get; set; }
        public DateTime? SecondExaminerMarkSubmissionStatusDate { get; set; }
        public DateTime? ThirdExaminerMarkSubmissionStatusDate { get; set; }

        public DateTime? FirstExaminerMarkSubmissionDate { get; set; }
        public DateTime? SecondExaminerMarkSubmissionDate { get; set; }
        public DateTime? ThirdExaminerMarkSubmissionDate { get; set; }

        public Course Course
        {
            get
            {
                return CourseManager.GetByCourseIdVersionId(CourseID, VersionID);
            }
        }
    }
}
