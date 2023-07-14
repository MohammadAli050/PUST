using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExamMarkFirstSecondThirdExaminerRepository : IExamMarkFirstSecondThirdExaminerRepository
    {

        Database db = null;

        private string sqlInsert = "ExamMarkFirstSecondThirdExaminerInsert";
        private string sqlUpdate = "ExamMarkFirstSecondThirdExaminerUpdate";
        private string sqlDelete = "ExamMarkFirstSecondThirdExaminerDeleteById";
        private string sqlGetById = "ExamMarkFirstSecondThirdExaminerGetById";
        private string sqlGetAll = "ExamMarkFirstSecondThirdExaminerGetAll";

        public int Insert(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, exammarkfirstsecondthirdexaminer, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ID");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                id = 0;
            }

            return id;
        }

        public bool Update(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, exammarkfirstsecondthirdexaminer, isInsert);

                int rowsAffected = db.ExecuteNonQuery(cmd);

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlDelete);

                db.AddInParameter(cmd, "ID", DbType.Int32, id);
                int rowsAffected = db.ExecuteNonQuery(cmd);

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public ExamMarkFirstSecondThirdExaminer GetById(int? id)
        {
            ExamMarkFirstSecondThirdExaminer _exammarkfirstsecondthirdexaminer = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>(sqlGetById, rowMapper);
                _exammarkfirstsecondthirdexaminer = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkfirstsecondthirdexaminer;
            }

            return _exammarkfirstsecondthirdexaminer;
        }

        public ExamMarkFirstSecondThirdExaminer GetByCourseHistoryId(int courseHistoryId)
        {
            ExamMarkFirstSecondThirdExaminer firstSecondThirdExaminer = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>("GetByCourseHistoryId", rowMapper);
                firstSecondThirdExaminer = accessor.Execute(courseHistoryId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return firstSecondThirdExaminer;
            }

            return firstSecondThirdExaminer;
        }

        public List<ExamMarkFirstSecondThirdExaminer> GetAll()
        {
            List<ExamMarkFirstSecondThirdExaminer> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>(sqlGetAll, mapper);
                IEnumerable<ExamMarkFirstSecondThirdExaminer> collection = accessor.Execute();

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }


        public List<ExamMarkEntryDTO> GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(int? firstExaminerId, int? secondExaminerId, int? thirdExaminerId, int? examSetupDetailId)
        {
            List<ExamMarkEntryDTO> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkEntryDTO> mapper = GetMaperExamMarkEntryDTO();

                var accessor = db.CreateSprocAccessor<ExamMarkEntryDTO>("ExamMarkGetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId", mapper);
                IEnumerable<ExamMarkEntryDTO> collection = accessor.Execute(firstExaminerId, secondExaminerId, thirdExaminerId, examSetupDetailId);

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }


        public ExamMarkFirstSecondThirdExaminer GetByCourseHistoryIdExamTemplateTypeId(int courseHistoryId, int examTemplateItemId)
        {
            ExamMarkFirstSecondThirdExaminer _exammarkfirstsecondthirdexaminer = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>("ExamMarkFirstSecondThirdExaminerGetByCourseHistoryIdExamTemplateItemId", rowMapper);
                _exammarkfirstsecondthirdexaminer = accessor.Execute(courseHistoryId, examTemplateItemId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkfirstsecondthirdexaminer;
            }

            return _exammarkfirstsecondthirdexaminer;
        }

        public ExamMarkGridViewShoetInfoDTO GetExamMarkModalGridViewShoetInfoByCourseHistoryId(int acaCalSectionId, int examSetupId)
        {
            ExamMarkGridViewShoetInfoDTO _exammarkfirstsecondthirdexaminer = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkGridViewShoetInfoDTO> rowMapper = GetMaperExamMarkGridViewShoetInfoDTO();

                var accessor = db.CreateSprocAccessor<ExamMarkGridViewShoetInfoDTO>("ExamMarkFirstSecondThirdExaminerGetExamMarkGridViewShoetInfoByAcaCalSectionIdExamSetupId", rowMapper);
                _exammarkfirstsecondthirdexaminer = accessor.Execute(acaCalSectionId, examSetupId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkfirstsecondthirdexaminer;
            }

            return _exammarkfirstsecondthirdexaminer;
        }



        public List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamMarkFirstSecondThirdExaminer> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>("ExamMarkFirstSecondThirdExaminerGetAllAcaCalSectionID", mapper);
                IEnumerable<ExamMarkFirstSecondThirdExaminer> collection = accessor.Execute(acaCalSectionId);

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }

        public List<ExamMarkFirstSecondThirdExaminer> GetAllThirdExaminerStudentListByThirdExaminerStatus(int thirdExaminerStatus)
        {
            List<ExamMarkFirstSecondThirdExaminer> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>("ExamMarkFirstSecondThirdExaminerGetAllThirdExaminerStudentListByThirdExaminerStatus", mapper);
                IEnumerable<ExamMarkFirstSecondThirdExaminer> collection = accessor.Execute(thirdExaminerStatus);

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }


        public List<ExamMarkFirstSecondThirdDTO> GetExamMarkFirstSecondThirdByAcaCalSectionIdFirstSecondThirdTypeId(int acaCalSectionId, int firstSecondThirdTypeId, int examId, int employeeId)
        {
            List<ExamMarkFirstSecondThirdDTO> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdDTO> mapper = GetMaperExamMarkFirstSecondThird();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdDTO>("ExamMarkFirstSecondThirdExaminerGetAllAcaCalSectionIdFirstSecondThirdTypeId", mapper);
                IEnumerable<ExamMarkFirstSecondThirdDTO> collection = accessor.Execute(acaCalSectionId, firstSecondThirdTypeId, examId, employeeId);

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }

        public List<Student> GetThirdExaminerEligibleStudentListByAcaCalSecId(int acaCalSectionId)
        {
            List<Student> thirdExaminerEligibleStudentList = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<Student> mapper = GetStudentMapper();
                var accessor = db.CreateSprocAccessor<Student>("GetThirdExaminerEligibleStudentListByAcaCalSecId", mapper);
                IEnumerable<Student> collection = accessor.Execute(acaCalSectionId);
                thirdExaminerEligibleStudentList = collection.ToList();
            }
            catch (Exception ex)
            {
                return thirdExaminerEligibleStudentList;
            }
            return thirdExaminerEligibleStudentList;
        }
        

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, exammarkfirstsecondthirdexaminer.ID);
            }


            db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, exammarkfirstsecondthirdexaminer.CourseHistoryId);
            db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, exammarkfirstsecondthirdexaminer.ExamTemplateItemId);

            db.AddInParameter(cmd, "FirstExaminerMark", DbType.Decimal, exammarkfirstsecondthirdexaminer.FirstExaminerMark);
            db.AddInParameter(cmd, "FirstExaminerMarkSubmissionStatus", DbType.Int32, exammarkfirstsecondthirdexaminer.FirstExaminerMarkSubmissionStatus);
            db.AddInParameter(cmd, "FirstExaminerMarkSubmissionStatusDate", DbType.DateTime, exammarkfirstsecondthirdexaminer.FirstExaminerMarkSubmissionStatusDate);

            db.AddInParameter(cmd, "SecondExaminerMark", DbType.Decimal, exammarkfirstsecondthirdexaminer.SecondExaminerMark);
            db.AddInParameter(cmd, "SecondExaminerMarkSubmissionStatus", DbType.Int32, exammarkfirstsecondthirdexaminer.SecondExaminerMarkSubmissionStatus);
            db.AddInParameter(cmd, "SecondExaminerMarkSubmissionStatusDate", DbType.DateTime, exammarkfirstsecondthirdexaminer.SecondExaminerMarkSubmissionStatusDate);

            db.AddInParameter(cmd, "ThirdExaminerStatus", DbType.Int32, exammarkfirstsecondthirdexaminer.ThirdExaminerStatus);
            db.AddInParameter(cmd, "ThirdExaminerMark", DbType.Decimal, exammarkfirstsecondthirdexaminer.ThirdExaminerMark);
            db.AddInParameter(cmd, "ThirdExaminerMarkSubmissionStatus", DbType.Int32, exammarkfirstsecondthirdexaminer.ThirdExaminerMarkSubmissionStatus);
            db.AddInParameter(cmd, "ThirdExaminerMarkSubmissionStatusDate", DbType.DateTime, exammarkfirstsecondthirdexaminer.ThirdExaminerMarkSubmissionStatusDate);
            
            db.AddInParameter(cmd, "IsAbsent", DbType.Boolean, exammarkfirstsecondthirdexaminer.IsAbsent);
            
            db.AddInParameter(cmd, "Attribute1", DbType.String, exammarkfirstsecondthirdexaminer.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, exammarkfirstsecondthirdexaminer.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, exammarkfirstsecondthirdexaminer.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, exammarkfirstsecondthirdexaminer.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, exammarkfirstsecondthirdexaminer.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, exammarkfirstsecondthirdexaminer.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, exammarkfirstsecondthirdexaminer.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamMarkFirstSecondThirdExaminer> GetMaper()
        {
            IRowMapper<ExamMarkFirstSecondThirdExaminer> mapper = MapBuilder<ExamMarkFirstSecondThirdExaminer>.MapAllProperties()

           .Map(m => m.ID).ToColumn("ID")
        .Map(m => m.CourseHistoryId).ToColumn("CourseHistoryId")
        .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
        .Map(m => m.FirstExaminerMark).ToColumn("FirstExaminerMark")
        .Map(m => m.FirstExaminerMarkSubmissionStatus).ToColumn("FirstExaminerMarkSubmissionStatus")
        .Map(m => m.FirstExaminerMarkSubmissionStatusDate).ToColumn("FirstExaminerMarkSubmissionStatusDate")
        .Map(m => m.SecondExaminerMark).ToColumn("SecondExaminerMark")
        .Map(m => m.SecondExaminerMarkSubmissionStatus).ToColumn("SecondExaminerMarkSubmissionStatus")
        .Map(m => m.SecondExaminerMarkSubmissionStatusDate).ToColumn("SecondExaminerMarkSubmissionStatusDate")
        .Map(m => m.ThirdExaminerStatus).ToColumn("ThirdExaminerStatus")
        .Map(m => m.ThirdExaminerMark).ToColumn("ThirdExaminerMark")
        .Map(m => m.ThirdExaminerMarkSubmissionStatus).ToColumn("ThirdExaminerMarkSubmissionStatus")
        .Map(m => m.ThirdExaminerMarkSubmissionStatusDate).ToColumn("ThirdExaminerMarkSubmissionStatusDate")
        .Map(m => m.IsAbsent).ToColumn("IsAbsent")
        .Map(m => m.Attribute1).ToColumn("Attribute1")
        .Map(m => m.Attribute2).ToColumn("Attribute2")
        .Map(m => m.Attribute3).ToColumn("Attribute3")
        .Map(m => m.CreatedBy).ToColumn("CreatedBy")
        .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Build();

            return mapper;
        }

        private IRowMapper<ExamMarkEntryDTO> GetMaperExamMarkEntryDTO()
        {
            IRowMapper<ExamMarkEntryDTO> mapper = MapBuilder<ExamMarkEntryDTO>.MapAllProperties()

           .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
        .Map(m => m.CourseSection).ToColumn("CourseSection")
        .Map(m => m.Exam).ToColumn("Exam")
        .Map(m => m.AcaCalSectionID).ToColumn("AcaCalSectionID")
        .Map(m => m.ExamSetupID).ToColumn("ExamSetupID")
        .Map(m => m.ExamName).ToColumn("ExamName")
        .Map(m => m.ExamMark).ToColumn("ExamMark")
        .Map(m => m.IsButtonVisible).ToColumn("IsButtonVisible")

            .Build();

            return mapper;
        }

        private IRowMapper<ExamMarkGridViewShoetInfoDTO> GetMaperExamMarkGridViewShoetInfoDTO()
        {
            IRowMapper<ExamMarkGridViewShoetInfoDTO> mapper = MapBuilder<ExamMarkGridViewShoetInfoDTO>.MapAllProperties()

           .Map(m => m.CourseName).ToColumn("CourseName")
        .Map(m => m.SectionName).ToColumn("SectionName")
        .Map(m => m.ExamName).ToColumn("ExamName")
        .Map(m => m.TotalStudentCount).ToColumn("TotalStudentCount")
        .Map(m => m.AbsentCount).ToColumn("AbsentCount")

        .Map(m => m.DepartmentName).ToColumn("DepartmentName")
        .Map(m => m.ProgramName).ToColumn("ProgramName")
        .Map(m => m.CourseTitle).ToColumn("CourseTitle")
        .Map(m => m.ExamMark).ToColumn("ExamMark")
        .Map(m => m.TeacherName).ToColumn("TeacherName")
        .Map(m => m.TeacherDesignation).ToColumn("TeacherDesignation")
        .Map(m => m.Session).ToColumn("Session")

            .Build();

            return mapper;
        }

        private IRowMapper<ExamMarkFirstSecondThirdDTO> GetMaperExamMarkFirstSecondThird()
        {
            IRowMapper<ExamMarkFirstSecondThirdDTO> mapper = MapBuilder<ExamMarkFirstSecondThirdDTO>.MapAllProperties()

           .Map(m => m.StudentID).ToColumn("StudentID")
        .Map(m => m.Roll).ToColumn("Roll")
        .Map(m => m.StudentName).ToColumn("StudentName")
        .Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
        .Map(m => m.ExamMarkSUM).ToColumn("ExamMarkSUM")
        .Map(m => m.QuestionMark1).ToColumn("QuestionMark1")
        .Map(m => m.QuestionMark2).ToColumn("QuestionMark2")
        .Map(m => m.QuestionMark3).ToColumn("QuestionMark3")
        .Map(m => m.QuestionMark4).ToColumn("QuestionMark4")
        .Map(m => m.QuestionMark5).ToColumn("QuestionMark5")
        .Map(m => m.QuestionMark6).ToColumn("QuestionMark6")
        .Map(m => m.QuestionMark7).ToColumn("QuestionMark7")
        .Map(m => m.QuestionMark8).ToColumn("QuestionMark8")
        .Map(m => m.QuestionMark9).ToColumn("QuestionMark9")
        .Map(m => m.QuestionMark10).ToColumn("QuestionMark10")

            .Build();

            return mapper;
        }

        private IRowMapper<Student> GetStudentMapper()
        {
            IRowMapper<Student> mapper = MapBuilder<Student>.MapAllProperties()
            .Map(m => m.StudentID).ToColumn("StudentID")
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.PersonID).ToColumn("PersonID")
            .Map(m => m.BatchId).ToColumn("BatchId")
            .Map(m => m.StudentAdmissionAcaCalId).ToColumn("StudentAdmissionAcaCalId")
            .Map(m => m.ActiveSession).ToColumn("ActiveSession")
            .Map(m => m.GradeMasterId).ToColumn("GradeMasterId")
            .Map(m => m.AccountHeadsID).ToColumn("AccountHeadsID")
            .Map(m => m.TreeMasterID).ToColumn("TreeMasterID")
            .Map(m => m.TreeCalendarMasterID).ToColumn("TreeCalendarMasterID")
            .Map(m => m.CandidateId).ToColumn("CandidateId")
            .Map(m => m.Major1NodeID).ToColumn("Major1NodeID")
            .Map(m => m.Major2NodeID).ToColumn("Major2NodeID")
            .Map(m => m.Major3NodeID).ToColumn("Major3NodeID")
            .Map(m => m.Minor1NodeID).ToColumn("Minor1NodeID")
            .Map(m => m.Minor2NodeID).ToColumn("Minor2NodeID")
            .Map(m => m.Minor3NodeID).ToColumn("Minor3NodeID")
            .Map(m => m.History).ToColumn("History")
            .Map(m => m.IsCompleted).ToColumn("IsCompleted")
            .Map(m => m.CompletedAcaCalId).ToColumn("CompletedAcaCalId")
            .Map(m => m.TranscriptSerial).ToColumn("TranscriptSerial")
            .Map(m => m.Remarks).ToColumn("Remarks")
            .Map(m => m.Pre_Math).ToColumn("Pre_Math")
            .Map(m => m.Pre_English).ToColumn("Pre_English")
            .Map(m => m.IsDiploma).ToColumn("IsDiploma")
            .Map(m => m.IsActive).ToColumn("IsActive")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.IsProvisionalAdmission).ToColumn("IsProvisionalAdmission")
            .Map(m => m.ValidUptoProAdmissionDate).ToColumn("ValidUptoProAdmissionDate")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            .Map(m => m.CGPA).ToColumn("CGPA")
            .Map(m => m.AutoOpenCr).ToColumn("AutoOpenCr")
            .DoNotMap(m => m.AutoPreRegCr)
            .DoNotMap(m => m.AutoMandaotryCr)
            .DoNotMap(m => m.IsChaked)
            .DoNotMap(m => m.FullName)
            .DoNotMap(m => m.IsRegistered)
            .DoNotMap(m => m.Gender)
            .DoNotMap(m => m.RegistrationNo)
            .DoNotMap(m => m.RegSession)
            .Build();

            return mapper;
        }

        #endregion



        public List<ExaminerQuestionWiseMarkDTO> GetByFinalExamQuestionwiseMarkBySectionIdExaminerType(int acaCalSectionId, int firstSecondThirdTypeId, int examId, int employeeId)
        {
            List<ExaminerQuestionWiseMarkDTO> finalExamQuestionwiseMark = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerQuestionWiseMarkDTO> mapper = GetMaperFinalExamQuestionwiseMark();

                var accessor = db.CreateSprocAccessor<ExaminerQuestionWiseMarkDTO>("FinalExamQuestionwiseMarkBySectionIdExaminerType", mapper);
                IEnumerable<ExaminerQuestionWiseMarkDTO> collection = accessor.Execute(acaCalSectionId, firstSecondThirdTypeId, examId, employeeId);

                finalExamQuestionwiseMark = collection.ToList();
            }

            catch (Exception ex)
            {
                return finalExamQuestionwiseMark;
            }

            return finalExamQuestionwiseMark;
        }

        private IRowMapper<ExaminerQuestionWiseMarkDTO> GetMaperFinalExamQuestionwiseMark()
        {
            IRowMapper<ExaminerQuestionWiseMarkDTO> mapper = MapBuilder<ExaminerQuestionWiseMarkDTO>.MapAllProperties()

            .Map(m => m.StudentID).ToColumn("StudentID")
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.FullName).ToColumn("FullName")
            .Map(m => m.ExamMarkFirstSecondThirdExaminerId).ToColumn("ExamMarkFirstSecondThirdExaminerId")
            .Map(m => m.CourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.Question1Marks).ToColumn("Question1Marks")
            .Map(m => m.Question2Marks).ToColumn("Question2Marks")
            .Map(m => m.Question3Marks).ToColumn("Question3Marks")
            .Map(m => m.Question4Marks).ToColumn("Question4Marks")
            .Map(m => m.Question5Marks).ToColumn("Question5Marks")
            .Map(m => m.Question6Marks).ToColumn("Question6Marks")
            .Map(m => m.Question7Marks).ToColumn("Question7Marks")
            .Map(m => m.Question8Marks).ToColumn("Question8Marks")
            .Map(m => m.Question9Marks).ToColumn("Question9Marks")
            .Map(m => m.Question10Marks).ToColumn("Question10Marks")


            .Build();

            return mapper;
        }




        public List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionIdExamIdExaminerId(int acaCalSectionId, int examId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId)
        {
            List<ExamMarkFirstSecondThirdExaminer> exammarkfirstsecondthirdexaminerList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkFirstSecondThirdExaminer> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkFirstSecondThirdExaminer>("ExamMarkFirstSecondThirdExaminerGetAllByAcaCalSectionIdExamIdExaminerId", mapper);
                IEnumerable<ExamMarkFirstSecondThirdExaminer> collection = accessor.Execute(acaCalSectionId, examId, FirstExaminerId, SecondExaminerId, ThirdExaminerId);

                exammarkfirstsecondthirdexaminerList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkfirstsecondthirdexaminerList;
            }

            return exammarkfirstsecondthirdexaminerList;
        }



    }
}
