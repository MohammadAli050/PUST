namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EducationBoard")]
    public partial class EducationBoard
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string BoardEng { get; set; }

        [StringLength(250)]
        public string BoardBng { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public bool? IsActive { get; set; }

        public int? IndexValue { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
