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
    
    public partial class ExaminerSetup
    {
        public int ID { get; set; }
        public Nullable<int> AcaCalSectionId { get; set; }
        public Nullable<int> FirstExaminer { get; set; }
        public Nullable<int> SecondExaminor { get; set; }
        public Nullable<int> ThirdExaminor { get; set; }
        public Nullable<int> ExamSetupDetailId { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
