namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentApplicationFormEducationInfo")]
    public partial class StudentApplicationFormEducationInfo
    {
        [Key]
        public int StudentApplicationFormEducationId { get; set; }

        public int? StudentApplicationOfficialInfoId { get; set; }
        
        public int? AppliedStudentExamNameId { get; set; }

        public int? AppliedStudentBoardId { get; set; }

        public string AppliedStudentSchoolOrCollege { get; set; }

        [StringLength(250)]
        public string AppliedStudentRoll { get; set; }

        public int? AppliedStudentYearId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AppliedStudentGrade { get; set; }

        public string AppliedStudentExamRemarks { get; set; }

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
