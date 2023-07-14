namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentApplicationFormPersonalInformation")]
    public partial class StudentApplicationFormPersonalInformation
    {
        [Key]
        public int StudentApplicationPersonalInfoId { get; set; }

        public int? StudentApplicationOfficialInfoId { get; set; }

        [StringLength(250)]
        public string AppliedStudentNameEng { get; set; }

        [StringLength(250)]
        public string AppliedStudentNameBng { get; set; }

        [StringLength(250)]
        public string AppliedStudentMotherName { get; set; }

        [StringLength(250)]
        public string AppliedStudentFatherName { get; set; }

        [StringLength(250)]
        public string AppliedStudentGuardianName { get; set; }

        public int? StudentNationalityId { get; set; }

        public DateTime? AppliedStudentDOB { get; set; }

        [StringLength(15)]
        public string AppliedStudentMobile { get; set; }

        public string AppliedStudentPresentAddress { get; set; }

        public string AppliedStudentPermanentAddress { get; set; }

        [StringLength(250)]
        public string PhotoPath { get; set; }

        [StringLength(250)]
        public string SignaturePath { get; set; }

        [StringLength(250)]
        public string Attribute1 { get; set; }

        [StringLength(250)]
        public string Attribute2 { get; set; }

        [StringLength(250)]
        public string Attribute3 { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
