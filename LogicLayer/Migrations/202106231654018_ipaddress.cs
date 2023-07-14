namespace LogicLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ipaddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Education",
                c => new
                    {
                        EducationId = c.Int(nullable: false, identity: true),
                        ExamNameEng = c.String(maxLength: 250),
                        ExamNameBng = c.String(maxLength: 250),
                        Remarks = c.String(maxLength: 250),
                        IsActive = c.Boolean(),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        IndexValue = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EducationId);
            
            CreateTable(
                "dbo.EducationBoard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoardEng = c.String(maxLength: 250),
                        BoardBng = c.String(maxLength: 250),
                        Remarks = c.String(maxLength: 250),
                        IsActive = c.Boolean(),
                        IndexValue = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nationality",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NationalityEng = c.String(maxLength: 250),
                        NationalityBng = c.String(maxLength: 250),
                        ShortCode = c.String(maxLength: 5),
                        Remarks = c.String(maxLength: 250),
                        IsActive = c.Boolean(),
                        IndexValue = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentApplicationFormAppliedCourse",
                c => new
                    {
                        AppliedCourseId = c.Int(nullable: false, identity: true),
                        StudentApplicationOfficialInfoId = c.Int(),
                        AppliedCourseCodeId = c.Int(),
                        AppliedCourseSummary = c.String(),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AppliedCourseId);
            
            CreateTable(
                "dbo.StudentApplicationFormEducationInfo",
                c => new
                    {
                        StudentApplicationFormEducationId = c.Int(nullable: false, identity: true),
                        StudentApplicationOfficialInfoId = c.Int(),
                        AppliedStudentExamNameId = c.Int(),
                        AppliedStudentBoardId = c.Int(),
                        AppliedStudentSchoolOrCollege = c.String(),
                        AppliedStudentRoll = c.String(maxLength: 250),
                        AppliedStudentYearId = c.Int(),
                        AppliedStudentGrade = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        AppliedStudentExamRemarks = c.String(),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StudentApplicationFormEducationId);
            
            CreateTable(
                "dbo.StudentApplicationFormPersonalInformation",
                c => new
                    {
                        StudentApplicationPersonalInfoId = c.Int(nullable: false, identity: true),
                        StudentApplicationOfficialInfoId = c.Int(),
                        AppliedStudentNameEng = c.String(maxLength: 250),
                        AppliedStudentNameBng = c.String(maxLength: 250),
                        AppliedStudentMotherName = c.String(maxLength: 250),
                        AppliedStudentFatherName = c.String(maxLength: 250),
                        AppliedStudentGuardianName = c.String(maxLength: 250),
                        StudentNationalityId = c.Int(),
                        AppliedStudentDOB = c.DateTime(),
                        AppliedStudentMobile = c.String(maxLength: 15),
                        AppliedStudentPresentAddress = c.String(),
                        AppliedStudentPermanentAddress = c.String(),
                        PhotoPath = c.String(maxLength: 250),
                        SignaturePath = c.String(maxLength: 250),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StudentApplicationPersonalInfoId);
            
            CreateTable(
                "dbo.StudentApplicationFormPreviousSemesterInfo",
                c => new
                    {
                        PreviousSemesterId = c.Int(nullable: false, identity: true),
                        StudentApplicationOfficialInfoId = c.Int(),
                        PreviousSemesterExamName = c.String(maxLength: 250),
                        PreviousSemesterExamYearId = c.Int(),
                        PreviousSemesterResult = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        PreviousSemseterCourseId = c.Int(),
                        PreviousSemseterCourseGP = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PreviousSemesterId);
            
            CreateTable(
                "dbo.StudentApplicationOfficialInformation",
                c => new
                    {
                        StudentApplicationOfficialInfoId = c.Int(nullable: false, identity: true),
                        AppliedDepartment = c.String(maxLength: 250),
                        AppliedFaculty = c.String(maxLength: 250),
                        AppliedHall = c.String(maxLength: 250),
                        StudentTypeId = c.Int(),
                        StudentIDNo = c.String(maxLength: 250),
                        StudentRegNo = c.String(maxLength: 250),
                        AppliedSession = c.String(maxLength: 250),
                        AppliedSemester = c.String(maxLength: 250),
                        Attribute1 = c.String(maxLength: 250),
                        Attribute2 = c.String(maxLength: 250),
                        Attribute3 = c.String(maxLength: 250),
                        IsDelete = c.Boolean(),
                        IpAddress = c.String(maxLength: 100),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StudentApplicationOfficialInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentApplicationOfficialInformation");
            DropTable("dbo.StudentApplicationFormPreviousSemesterInfo");
            DropTable("dbo.StudentApplicationFormPersonalInformation");
            DropTable("dbo.StudentApplicationFormEducationInfo");
            DropTable("dbo.StudentApplicationFormAppliedCourse");
            DropTable("dbo.Nationality");
            DropTable("dbo.EducationBoard");
            DropTable("dbo.Education");
        }
    }
}
