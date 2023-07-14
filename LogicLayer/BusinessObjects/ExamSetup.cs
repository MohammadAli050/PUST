using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamSetup
    {
        public int ID { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> YearNo { get; set; }
        public Nullable<int> SemesterNo { get; set; }
        public Nullable<int> AcaCalId { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Shal { get; set; }
        public string ExamName { get; set; }
        public string ExamShortName { get; set; }
        public Nullable<DateTime> ExamStartDate { get; set; }
        public Nullable<DateTime> ExamEndDate { get; set; }
        public Nullable<DateTime> LastDateOfResultSubmission { get; set; }
        public Nullable<DateTime> ResultPublishDate { get; set; }
        public string Remarks { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
