namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Education")]
    public partial class Education
    {
        public int EducationId { get; set; }
        [StringLength(250)]
        public string ExamNameEng { get; set; }
        [StringLength(250)]
        public string ExamNameBng { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(250)]
        public string Attribute1 { get; set; }
        [StringLength(250)]
        public string Attribute2 { get; set; }
        [StringLength(250)]
        public string Attribute3 { get; set; }
        public int? IndexValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
