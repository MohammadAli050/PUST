using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamSetupDTO
    {
        public int ExamSetupID { get; set; }
        public int ExamSetupDetailId { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> YearNo { get; set; }
        public string YearNoName { get; set; }
        public Nullable<int> SemesterNo { get; set; }
        public string SemesterNoName { get; set; }
        public string ExamName { get; set; }
        public string FullCode { get; set; }
        public Nullable<int> Shal { get; set; }

        public int ExamSetupWithExamCommitteesId { get; set; }
        public string ChairmanName { get; set; }
        public string MemberOneName { get; set; }
        public string MemberTwoName { get; set; }
        public string ExternalMemberName { get; set; }
        
    }
}
