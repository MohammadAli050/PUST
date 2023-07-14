using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamMarkFirstSecondThirdExaminer
    {
        public int ID { get; set; }
        public Nullable<int> CourseHistoryId { get; set; }
        public Nullable<int> ExamTemplateItemId { get; set; }

        public Nullable<decimal> FirstExaminerMark { get; set; }
        public Nullable<int> FirstExaminerMarkSubmissionStatus { get; set; }
        public Nullable<DateTime> FirstExaminerMarkSubmissionStatusDate { get; set; }

        public Nullable<decimal> SecondExaminerMark { get; set; }
        public Nullable<int> SecondExaminerMarkSubmissionStatus { get; set; }
        public Nullable<DateTime> SecondExaminerMarkSubmissionStatusDate { get; set; }

        public Nullable<int> ThirdExaminerStatus { get; set; }
        public Nullable<decimal> ThirdExaminerMark { get; set; }
        public Nullable<int> ThirdExaminerMarkSubmissionStatus { get; set; }
        public Nullable<DateTime> ThirdExaminerMarkSubmissionStatusDate { get; set; }

        public Nullable<bool> IsAbsent { get; set; }

        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }

        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}
