namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentApplicationFormPreviousSemesterInfo")]
    public partial class StudentApplicationFormPreviousSemesterInfo
    {
        [Key]
        public int PreviousSemesterId { get; set; }

        public int? StudentApplicationOfficialInfoId { get; set; }

        [StringLength(250)]
        public string PreviousSemesterExamName { get; set; }

        public int? PreviousSemesterExamYearId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PreviousSemesterResult { get; set; }

        public int? PreviousSemseterCourseId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PreviousSemseterCourseGP { get; set; }

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
