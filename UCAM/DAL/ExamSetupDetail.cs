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
    
    public partial class ExamSetupDetail
    {
        public int ExamSetupDetailId { get; set; }
        public Nullable<int> ExamSetupId { get; set; }
        public Nullable<int> SemesterNo { get; set; }
        public string ExamName { get; set; }
        public string ExamShortName { get; set; }
        public Nullable<System.DateTime> ExamStartDate { get; set; }
        public Nullable<System.DateTime> ExamEndDate { get; set; }
        public Nullable<System.DateTime> LastDateOfResultSubmission { get; set; }
        public Nullable<System.DateTime> ResultPublishDate { get; set; }
        public string Remarks { get; set; }
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
