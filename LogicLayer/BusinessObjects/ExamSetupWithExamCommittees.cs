using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamSetupWithExamCommittees
    {
        public int ID { get; set; }
        public int? HeldInProgramRelationId { get; set; }
        public int? ExamCommitteeChairmanDeptId { get; set; }
        public int? ExamCommitteeChairmanId { get; set; }
        public int? ExamCommitteeMemberOneDeptId { get; set; }
        public int? ExamCommitteeMemberOneId { get; set; }
        public int? ExamCommitteeMemberTwoDeptId { get; set; }
        public int? ExamCommitteeMemberTwoId { get; set; }
        public int? ExamCommitteeExternalMemberDeptId { get; set; }
        public int? ExamCommitteeExternalMemberId { get; set; }
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

