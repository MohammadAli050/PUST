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
    
    public partial class ExamMarkMaster
    {
        public int ExamMarkMasterId { get; set; }
        public Nullable<decimal> ExamMark { get; set; }
        public Nullable<System.DateTime> ExamMarkEntryDate { get; set; }
        public Nullable<System.DateTime> FinalSubmissionDate { get; set; }
        public Nullable<int> ExamTemplateItemId { get; set; }
        public Nullable<int> AcaCalSectionId { get; set; }
        public Nullable<bool> IsFinalSubmit { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
