namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentApplicationOfficialInformation")]
    public partial class StudentApplicationOfficialInformation
    {
        [Key]
        public int StudentApplicationOfficialInfoId { get; set; }        

        [StringLength(250)]
        public string AppliedDepartment { get; set; }

        [StringLength(250)]
        public string AppliedFaculty { get; set; }

        [StringLength(250)]
        public string AppliedHall { get; set; }

        public int? StudentTypeId { get; set; }

        [StringLength(250)]
        public string StudentIDNo { get; set; }

        [StringLength(250)]
        public string StudentRegNo { get; set; }

        [StringLength(250)]
        public string StudentAcademicYear { get; set; }

        public int AppliedSessionId { get; set; }
        
        public int AppliedSemesterId { get; set; }

        public int AppliedYearId { get; set; }

        public int AppliedProgramId { get; set; }

        [StringLength(250)]
        public string Attribute1 { get; set; }

        [StringLength(250)]
        public string Attribute2 { get; set; }

        [StringLength(250)]
        public string Attribute3 { get; set; }

        public bool? IsDelete { get; set; }

        [StringLength(100)]
        public string IpAddress { get; set; }
        
        public bool? IsFinalSubmit { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
