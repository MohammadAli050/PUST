using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    [Serializable]
    public class rExamCommitteePersonInfo
    {
        public string ExamName { get; set; }
        public string FullCode { get; set; }
        public string Shal { get; set; }
        public string ChairmanName { get; set; }
        public string ChairmanDept { get; set; }
        public string MemberOneName { get; set; }
        public string MemberOneDept { get; set; }
        public string MemberTwoName { get; set; }
        public string MemberTwoDept { get; set; }
        public string ExternalMemberName { get; set; }
        public string ExternalMemberDept { get; set; }

    }
}
