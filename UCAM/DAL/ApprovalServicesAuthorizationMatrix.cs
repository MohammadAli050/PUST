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
    
    public partial class ApprovalServicesAuthorizationMatrix
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string ApprovalMatrix { get; set; }
        public Nullable<int> ActiveStatus { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Attribute1 { get; set; }
        public Nullable<int> Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}