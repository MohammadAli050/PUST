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
    
    public partial class ExamTemplateItem
    {
        public int ExamTemplateItemId { get; set; }
        public Nullable<int> ExamTemplateId { get; set; }
        public string ExamName { get; set; }
        public Nullable<decimal> ExamMark { get; set; }
        public Nullable<decimal> PassMark { get; set; }
        public Nullable<int> ColumnSequence { get; set; }
        public Nullable<int> PrintColumnSequence { get; set; }
        public Nullable<int> ColumnType { get; set; }
        public Nullable<int> CalculationType { get; set; }
        public Nullable<decimal> DivideBy { get; set; }
        public Nullable<decimal> MultiplyBy { get; set; }
        public Nullable<bool> ShowInTabulation { get; set; }
        public string TabulationTitle { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> MultipleExaminer { get; set; }
        public Nullable<bool> IsFinalExam { get; set; }
        public Nullable<int> NumberOfExaminer { get; set; }
        public Nullable<bool> SingleQuestionAnswer { get; set; }
        public Nullable<bool> ShowAllContinuousInSubTotal { get; set; }
        public Nullable<bool> ShowAllInGrandTotal { get; set; }
    }
}
