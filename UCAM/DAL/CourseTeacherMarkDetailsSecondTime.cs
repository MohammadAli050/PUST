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
    
    public partial class CourseTeacherMarkDetailsSecondTime
    {
        public int Id { get; set; }
        public Nullable<int> ExamMarkMasterId { get; set; }
        public Nullable<int> CourseHistoryId { get; set; }
        public Nullable<decimal> Marks { get; set; }
        public Nullable<decimal> ConvertedMark { get; set; }
        public Nullable<bool> IsFinalSubmit { get; set; }
        public Nullable<int> ExamTemplateItemId { get; set; }
        public Nullable<int> ExamStatus { get; set; }
        public Nullable<int> ExamMarkTypeId { get; set; }
        public string Remarks { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
