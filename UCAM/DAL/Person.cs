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
    
    public partial class Person
    {
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string MatrialStatus { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string FatherName { get; set; }
        public string FatherProfession { get; set; }
        public string MotherName { get; set; }
        public string MotherProfession { get; set; }
        public string GuardianName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string SMSContactSelf { get; set; }
        public string SMSContactGuardian { get; set; }
        public string GuardianEmail { get; set; }
        public string BanglaName { get; set; }
    }
}
