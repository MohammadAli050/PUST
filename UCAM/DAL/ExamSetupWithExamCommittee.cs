//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExamSetupWithExamCommittee
    {
        public int ID { get; set; }
        public Nullable<int> HeldInProgramRelationId { get; set; }
        public Nullable<int> ExamCommitteeChairmanDeptId { get; set; }
        public Nullable<int> ExamCommitteeChairmanId { get; set; }
        public Nullable<int> ExamCommitteeMemberOneDeptId { get; set; }
        public Nullable<int> ExamCommitteeMemberOneId { get; set; }
        public Nullable<int> ExamCommitteeMemberTwoDeptId { get; set; }
        public Nullable<int> ExamCommitteeMemberTwoId { get; set; }
        public Nullable<int> ExamCommitteeExternalMemberDeptId { get; set; }
        public Nullable<int> ExamCommitteeExternalMemberId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}