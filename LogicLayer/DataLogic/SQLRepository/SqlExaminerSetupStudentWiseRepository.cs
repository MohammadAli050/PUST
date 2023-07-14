using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExaminerSetupStudentWiseRepository : IExaminerSetupStudentWiseRepository
    {

        Database db = null;

        private string sqlInsert = "ExaminerSetupStudentWiseInsert";
        private string sqlUpdate = "ExaminerSetupStudentWiseUpdate";
        private string sqlDelete = "ExaminerSetupStudentWiseDelete";
        private string sqlGetById = "ExaminerSetupStudentWiseGetById";
        private string sqlGetAll = "ExaminerSetupStudentWiseGetAll";
               
        public int Insert(ExaminerSetupStudentWise examinersetupstudentwise)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examinersetupstudentwise, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExaminerSetupStudentWiseId");

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

        public bool Update(ExaminerSetupStudentWise examinersetupstudentwise)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examinersetupstudentwise, isInsert);

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

                db.AddInParameter(cmd, "ExaminerSetupStudentWiseId", DbType.Int32, id);
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

        public ExaminerSetupStudentWise GetByCourseHistoryId(int id)
        {
            ExaminerSetupStudentWise _examinersetupstudentwise = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerSetupStudentWise> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminerSetupStudentWise>("ExaminerSetupStudentWiseGetByCourseHistoryId", rowMapper);
                _examinersetupstudentwise = accessor.Execute(id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _examinersetupstudentwise;
            }

            return _examinersetupstudentwise;
        }

        public List<ExaminerSetupStudentWise> GetAll()
        {
            List<ExaminerSetupStudentWise> examinersetupstudentwiseList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerSetupStudentWise> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminerSetupStudentWise>(sqlGetAll, mapper);
                IEnumerable<ExaminerSetupStudentWise> collection = accessor.Execute();

                examinersetupstudentwiseList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examinersetupstudentwiseList;
            }

            return examinersetupstudentwiseList;
        }

        public List<ExaminerSetupStudentWiseDTO> ExaminerSetupStudentWiseGetByProgramSessionYearNoSemesterNoExamAndCourse(int programId, int yearNo, int semesterNo, int courseId, int versionId, int examId)
        {
            List<ExaminerSetupStudentWiseDTO> ExaminerSetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerSetupStudentWiseDTO> mapper = ExaminerStudentWiseGetMapper();

                var accessor = db.CreateSprocAccessor<ExaminerSetupStudentWiseDTO>("ExaminerSetupStudentWiseGetByProgramSessionYearNoSemesterNoExamAndCourse", mapper);
                IEnumerable<ExaminerSetupStudentWiseDTO> collection = accessor.Execute(programId, yearNo, semesterNo, courseId, versionId, examId);

                ExaminerSetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return ExaminerSetupList;
            }

            return ExaminerSetupList;
        }

        public List<ExaminerSetupStudentWise> GetExaminersByAcaCalSectionId(int acaCalSecId)
        {
            List<ExaminerSetupStudentWise> examinersList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerSetupStudentWise> mapper = GetExaminersByAcaCalSectionIdMapper();

                var accessor = db.CreateSprocAccessor<ExaminerSetupStudentWise>("ExaminerSetupStudentWiseGetExaminersByAcaCalSectionId", mapper);
                IEnumerable<ExaminerSetupStudentWise> collection = accessor.Execute(acaCalSecId);

                examinersList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examinersList;
            }

            return examinersList;
        }

        public List<ExaminerSetupStudentWise> ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo(int acaCalSecId,int examinerId, int examinerNo)
        {
            List<ExaminerSetupStudentWise> examinersList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminerSetupStudentWise> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminerSetupStudentWise>("ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo", mapper);
                IEnumerable<ExaminerSetupStudentWise> collection = accessor.Execute(acaCalSecId, examinerId, examinerNo);

                examinersList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examinersList;
            }

            return examinersList;
        }
       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExaminerSetupStudentWise examinersetupstudentwise, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExaminerSetupStudentWiseId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExaminerSetupStudentWiseId", DbType.Int32, examinersetupstudentwise.ExaminerSetupStudentWiseId);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examinersetupstudentwise.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examinersetupstudentwise.ModifiedDate);
            }

            	
		db.AddInParameter(cmd,"AcaCalSectionId",DbType.Int32,examinersetupstudentwise.AcaCalSectionId);
		db.AddInParameter(cmd,"StudentCourseHistoryId",DbType.Int32,examinersetupstudentwise.StudentCourseHistoryId);
		db.AddInParameter(cmd,"ExamSetupDetailId",DbType.Int32,examinersetupstudentwise.ExamSetupDetailId);
		db.AddInParameter(cmd,"FirstExaminer",DbType.Int32,examinersetupstudentwise.FirstExaminer);
		db.AddInParameter(cmd,"SecondExaminer",DbType.Int32,examinersetupstudentwise.SecondExaminer);
		db.AddInParameter(cmd,"ThirdExaminer",DbType.Int32,examinersetupstudentwise.ThirdExaminer);
		db.AddInParameter(cmd,"Attribute1",DbType.String,examinersetupstudentwise.Attribute1);
		db.AddInParameter(cmd,"Attribute2",DbType.String,examinersetupstudentwise.Attribute2);
		db.AddInParameter(cmd,"Attribute3",DbType.String,examinersetupstudentwise.Attribute3);
		db.AddInParameter(cmd,"IsDeleted",DbType.Boolean,examinersetupstudentwise.IsDeleted);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,examinersetupstudentwise.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,examinersetupstudentwise.CreatedDate);
		
            
            return db;
        }

        private IRowMapper<ExaminerSetupStudentWise> GetMaper()
        {
            IRowMapper<ExaminerSetupStudentWise> mapper = MapBuilder<ExaminerSetupStudentWise>.MapAllProperties()

       	   .Map(m => m.ExaminerSetupStudentWiseId).ToColumn("ExaminerSetupStudentWiseId")
		.Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
		.Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
		.Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
		.Map(m => m.FirstExaminer).ToColumn("FirstExaminer")
		.Map(m => m.SecondExaminer).ToColumn("SecondExaminer")
		.Map(m => m.ThirdExaminer).ToColumn("ThirdExaminer")
		.Map(m => m.Attribute1).ToColumn("Attribute1")
		.Map(m => m.Attribute2).ToColumn("Attribute2")
		.Map(m => m.Attribute3).ToColumn("Attribute3")
		.Map(m => m.IsDeleted).ToColumn("IsDeleted")
		.Map(m => m.CreatedBy).ToColumn("CreatedBy")
		.Map(m => m.CreatedDate).ToColumn("CreatedDate")
		.Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
		.Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

           .DoNotMap(m => m.ExaminerId)
           .DoNotMap(m => m.ExaminerName)
            
            .Build();

            return mapper;
        }

        private IRowMapper<ExaminerSetupStudentWiseDTO> ExaminerStudentWiseGetMapper()
        {
            IRowMapper<ExaminerSetupStudentWiseDTO> mapper = MapBuilder<ExaminerSetupStudentWiseDTO>.MapAllProperties()
                .Map(m => m.ExaminerSetupStudentWiseId).ToColumn("ExaminerSetupStudentWiseId")
                .Map(m => m.AcaCalSectionId).ToColumn("AcaCal_SectionID")
                .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
                .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
                .Map(m => m.StudentName).ToColumn("StudentName")
                .Map(m => m.Roll).ToColumn("Roll")
                .Map(m => m.FirstExaminer).ToColumn("FirstExaminer")
                .Map(m => m.SecondExaminer).ToColumn("SecondExaminer")
                .Map(m => m.ThirdExaminer).ToColumn("ThirdExaminer")
                .Map(m => m.ExamName).ToColumn("ExamName")
                .Build();

            return mapper;
        }

        private IRowMapper<ExaminerSetupStudentWise> GetExaminersByAcaCalSectionIdMapper()
        {
            IRowMapper<ExaminerSetupStudentWise> mapper = MapBuilder<ExaminerSetupStudentWise>.MapAllProperties()

                
                .Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")               
                .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
                .Map(m => m.FirstExaminer).ToColumn("FirstExaminer")
                .Map(m => m.SecondExaminer).ToColumn("SecondExaminer")
                .Map(m => m.ThirdExaminer).ToColumn("ThirdExaminer")
                .Map(m => m.IsDeleted).ToColumn("IsDeleted")

                .DoNotMap(m => m.ExaminerSetupStudentWiseId)
                .DoNotMap(m => m.StudentCourseHistoryId)
                .DoNotMap(m => m.Attribute1)
                .DoNotMap(m => m.Attribute2)
                .DoNotMap(m => m.Attribute3)
                .DoNotMap(m => m.CreatedBy)
                .DoNotMap(m => m.CreatedDate)
                .DoNotMap(m => m.ModifiedBy)
                .DoNotMap(m => m.ModifiedDate)
                .DoNotMap(m => m.ExaminerId)
                .DoNotMap(m => m.ExaminerName)

                .Build();

            return mapper;
        }

        #endregion

    }
}

