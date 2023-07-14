namespace LogicLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Nationality")]
    public partial class Nationality
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string NationalityEng { get; set; }

        [StringLength(250)]
        public string NationalityBng { get; set; }

        [StringLength(5)]
        public string ShortCode { get; set; }

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
