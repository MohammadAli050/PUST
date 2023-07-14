namespace LogicLayer.DataEntity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    public partial class StudentApplicationFormModel : DbContext
    {
        public StudentApplicationFormModel()
            : base("name=StudentApplicationFormModel")
        {
        }

        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<StudentApplicationFormAppliedCourse> StudentApplicationFormAppliedCourses { get; set; }
        public virtual DbSet<StudentApplicationFormEducationInfo> StudentApplicationFormEducationInfoes { get; set; }
        public virtual DbSet<StudentApplicationFormPreviousSemesterInfo> StudentApplicationFormPreviousSemesterInfoes { get; set; }
        public virtual DbSet<StudentApplicationOfficialInformation> StudentApplicationOfficialInformations { get; set; }        
        public virtual DbSet<EducationBoard> EducationBoards { get; set; }
        public virtual DbSet<StudentApplicationFormPersonalInformation> StudentApplicationFormPersonalInformations { get; set; }
        public virtual DbSet<Education> Education { get; set; }
    }
}
