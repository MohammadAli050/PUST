using LogicLayer.BusinessObjects.RO;
using LogicLayer.DataLogic.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace LogicLayer.DataLogic.SQLRepository
{
    public class SQLStudentApplicationFormRepository : IStudentApplicationFormRepository
    {
        Database db = null;

        public List<StudentAppliFormOfficialAndPersonalRO> GetStudentApplicationDetailsByOfficialId(int officialInfoId)
        {
            var studentAppliFormData = new List<StudentAppliFormOfficialAndPersonalRO>();
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAppliFormOfficialAndPersonalRO> rowMapper = MapBuilder<StudentAppliFormOfficialAndPersonalRO>.MapAllProperties()
                    .Map(m => m.Department).ToColumn("Department")
                    .Map(m => m.Faculty).ToColumn("Faculty")
                    .Map(m => m.Hall).ToColumn("Hall")
                    .Map(m => m.StudentType).ToColumn("StudentType")
                    .Map(m => m.StudentIDNo).ToColumn("StudentIDNo")
                    .Map(m => m.StudentRegNo).ToColumn("StudentRegNo")
                    .Map(m => m.StudentAcademicYear).ToColumn("StudentAcademicYear")         
                    .Map(m => m.AppliedSession).ToColumn("AppliedSession")
                    .Map(m => m.AppliedSemester).ToColumn("AppliedSemester")
                    .Map(m => m.AppliedProgram).ToColumn("AppliedProgram")
                    .Map(m => m.AppliedYear).ToColumn("AppliedYear")
                    .Map(m => m.NameBng).ToColumn("NameBng")
                    .Map(m => m.NameEng).ToColumn("NameEng")
                    .Map(m => m.FatherName).ToColumn("FatherName")
                    .Map(m => m.MotherName).ToColumn("MotherName")
                    .Map(m => m.GuardianName).ToColumn("GuardianName")
                    .Map(m => m.Mobile).ToColumn("Mobile")
                    .Map(m => m.NationalityBng).ToColumn("NationalityBng")
                    .Map(m => m.NationalityEng).ToColumn("NationalityEng")
                    .Map(m => m.DateofBirth).ToColumn("DateofBirth")
                    .Map(m => m.PresentAddress).ToColumn("PresentAddress")
                    .Map(m => m.PermanentAddress).ToColumn("PermanentAddress")
                    .Map(m => m.PhotoPath).ToColumn("PhotoPath")
                    .Map(m => m.SignaturePath).ToColumn("SignaturePath")
                    .Build();


                var accessor = db.CreateSprocAccessor<StudentAppliFormOfficialAndPersonalRO>("GetStudentApplicationDetailsByOfficialId", rowMapper);
                IEnumerable<StudentAppliFormOfficialAndPersonalRO> collection = accessor.Execute(officialInfoId);
                studentAppliFormData = collection.ToList();

                return studentAppliFormData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }        

        public List<StudentAppliFormEducationInfoRO> GetStudentApplicationEducationDetailsByOfficialId(int officialInfoId)
        {
            var educationInfoList = new List<StudentAppliFormEducationInfoRO>();
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAppliFormEducationInfoRO> rowMapper = MapBuilder<StudentAppliFormEducationInfoRO>.MapAllProperties()
                    .Map(m => m.ExamNameBng).ToColumn("ExamNameBng")
                    .Map(m => m.ExamNameEng).ToColumn("ExamNameEng")
                    .Map(m => m.BoardBng).ToColumn("BoardBng")
                    .Map(m => m.BoardEng).ToColumn("BoardEng")
                    .Map(m => m.SchoolCollege).ToColumn("AppliedStudentSchoolOrCollege")
                    .Map(m => m.Roll).ToColumn("AppliedStudentRoll")
                    .Map(m => m.Year).ToColumn("AppliedStudentYearId")
                    .Map(m => m.Grade).ToColumn("AppliedStudentGrade")
                    .Map(m => m.Remarks).ToColumn("AppliedStudentExamRemarks")
                    .Build();

                var accessor = db.CreateSprocAccessor<StudentAppliFormEducationInfoRO>("GetStudentApplicationFormEducationDetailsByOfficialId", rowMapper);
                IEnumerable<StudentAppliFormEducationInfoRO> collection = accessor.Execute(officialInfoId);
                educationInfoList = collection.ToList();

                return educationInfoList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<StudentAppliFromPreviousSemesterInfoRO> GetStudentApplicationPreviousSemesterDetailsByOfficialId(int officialInfoId)
        {
            var semesterInfoList = new List<StudentAppliFromPreviousSemesterInfoRO>();
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAppliFromPreviousSemesterInfoRO> rowMapper = MapBuilder<StudentAppliFromPreviousSemesterInfoRO>.MapAllProperties()
                    .Map(m => m.ExamName).ToColumn("ExamName")
                    .Map(m => m.Year).ToColumn("Year")
                    .Map(m => m.Result).ToColumn("Result")
                    .Map(m => m.CourseCode).ToColumn("CourseCode")
                    .Map(m => m.CourseGP).ToColumn("CourseGP")                    
                    .Build();

                var accessor = db.CreateSprocAccessor<StudentAppliFromPreviousSemesterInfoRO>("GetStudentApplicationFromPreviousSemesterResultByOfficialId", rowMapper);
                IEnumerable<StudentAppliFromPreviousSemesterInfoRO> collection = accessor.Execute(officialInfoId);
                semesterInfoList = collection.ToList();

                return semesterInfoList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<StudentAppliFormAppliedCourseRO> GetStudentApplicationAppliedCourseDetailsByOfficialId(int officialInfoId)
        {
            var courseInfoList = new List<StudentAppliFormAppliedCourseRO>();
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAppliFormAppliedCourseRO> rowMapper = MapBuilder<StudentAppliFormAppliedCourseRO>.MapAllProperties()
                    .Map(m => m.CourseCode).ToColumn("CourseCode")
                    .Map(m => m.AppliedCourseSummary).ToColumn("AppliedCourseSummary")                    
                    .Build();

                var accessor = db.CreateSprocAccessor<StudentAppliFormAppliedCourseRO>("GetStudentApplicationFormAppliedCourseDetailsByOfficialId", rowMapper);
                IEnumerable<StudentAppliFormAppliedCourseRO> collection = accessor.Execute(officialInfoId);
                courseInfoList = collection.ToList();

                return courseInfoList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
