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
    
    public partial class PersonAdditionalInfo
    {
        public int PersonAdditionalInfoId { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Nullable<int> PersonCategoryEnumValueId { get; set; }
        public Nullable<int> PersonStatusEnumValueId { get; set; }
        public string SpouseName { get; set; }
        public Nullable<bool> IsMilitary { get; set; }
        public string NationalIdNumber { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string PersonalNo { get; set; }
        public Nullable<bool> IsMigrate { get; set; }
        public Nullable<int> BankId { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankRoutingNo { get; set; }
    }
}
