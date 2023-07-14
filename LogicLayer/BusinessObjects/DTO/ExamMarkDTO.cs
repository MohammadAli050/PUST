using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkDTO
    {
        public int CourseHistoryId { get; set; }
        public int ExamMarkDetailId { get; set; }
        public string StudentName { get; set; }
        public string StudentRoll { get; set; }
        public Nullable<decimal> Marks { get; set; }
        public int ExamStatus { get; set; }
        public string Remarks { get; set; }
        public bool IsFinalSubmit { get; set; }
    }
}
