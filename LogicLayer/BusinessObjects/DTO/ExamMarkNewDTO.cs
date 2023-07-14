using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkNewDTO
    {
        public int ExamMarkDetailId { get; set; }
        public int CourseHistoryId { get; set; }
        public string StudentName { get; set; }
        public string StudentRoll { get; set; }
        public string YearLevelName { get; set; }
        public int YearLevelNo { get; set; }
        public decimal EarnedCredit { get; set; }
        public string ExamRoll { get; set; }
        public string ExaminerRemarks { get; set; }
        public Nullable<decimal> Mark { get; set; }
        public Nullable<decimal> ScrutinizerMark { get; set; }
        public int ExamMarkTypeId { get; set; }
        public string ExamTemplateBasicItemName { get; set; }
        public Nullable<bool> IsFinalSubmit { get; set; }
        public int ColumnSequence { get; set; }
    }
}
