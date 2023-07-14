namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentApplicationFormAppliedCourse")]
    public partial class StudentApplicationFormAppliedCourse
    {
        [Key]
        public int AppliedCourseId { get; set; }

        public int? StudentApplicationOfficialInfoId { get; set; }

        public int? AppliedCourseCodeId { get; set; }

        public string AppliedCourseSummary { get; set; }

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
