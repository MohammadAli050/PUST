using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamMarkQuestion
    {
        public int ID { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> CourseHistoryId { get; set; }
        public Nullable<int> QuestionNo { get; set; }
        public Nullable<decimal> FirstExaminerMark { get; set; }
        public Nullable<decimal> SecondExaminerMark { get; set; }
        public Nullable<decimal> ThirdExaminerMark { get; set; }
        public string Remarks { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<int> ExamTemplateItemId { get; set; }
        
    }
}
