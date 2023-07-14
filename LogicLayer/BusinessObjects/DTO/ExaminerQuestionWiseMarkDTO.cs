using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExaminerQuestionWiseMarkDTO
    {
        public int StudentID { get; set; }
        public int Roll { get; set; }
        public string FullName { get; set; }
        public string CourseHistoryId { get; set; }
        public int ExamMarkFirstSecondThirdExaminerId { get; set; }
        public Nullable<decimal> Question1Marks { get; set; }
        public Nullable<decimal> Question2Marks { get; set; }
        public Nullable<decimal> Question3Marks { get; set; }
        public Nullable<decimal> Question4Marks { get; set; }
        public Nullable<decimal> Question5Marks { get; set; }
        public Nullable<decimal> Question6Marks { get; set; }
        public Nullable<decimal> Question7Marks { get; set; }
        public Nullable<decimal> Question8Marks { get; set; }
        public Nullable<decimal> Question9Marks { get; set; }
        public Nullable<decimal> Question10Marks { get; set; }
        public Nullable<decimal> ExaminerTotalMark { get; set; }
    }
}
