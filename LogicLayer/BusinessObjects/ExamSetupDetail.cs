using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamSetupDetail
    {
        public int ExamSetupDetailId { get; set; }
        public int? ExamSetupId { get; set; }
        public int? SemesterNo { get; set; }
        public string ExamName { get; set; }
        public string ExamShortName { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }
        public DateTime? LastDateOfResultSubmission { get; set; }
        public DateTime? ResultPublishDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
