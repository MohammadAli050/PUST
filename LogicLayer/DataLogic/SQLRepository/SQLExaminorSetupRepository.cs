using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using LogicLayer.BusinessObjects.DTO;


namespace LogicLayer.DataLogic.SQLRepository
{
    public class SQLExaminorSetupRepository : IExaminorSetupRepository
    {
        Database db = null;

        private string sqlInsert = "ExaminerSetupInsert";
        private string sqlUpdate = "ExaminerSetupUpdate";
        private string sqlDelete = "ExaminerSetupDeleteById";
        private string sqlGetById = "ExaminerSetupGetById";
        private string sqlGetAll = "ExaminerSetupGetAll";
       

        public int Insert(ExaminorSetups examinorsetup)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examinorsetup, isInsert);
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

        public bool Update(ExaminorSetups examinorsetup)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examinorsetup, isInsert);

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

        public ExaminorSetups GetById(int? id)
        {
            ExaminorSetups _examsetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminorSetups> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminorSetups>(sqlGetById, rowMapper);
                _examsetup = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetup;
            }

            return _examsetup;
        }

        public ExaminorSetups GetByAcaCalSecId(int acaCalId)
        {
            ExaminorSetups _examsetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminorSetups> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminorSetups>("ExaminerSetupGetByAcaCalSecId", rowMapper);
                _examsetup = accessor.Execute(acaCalId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetup;
            }

            return _examsetup;
        }


        public List<ExaminorSetups> GetAll()
        {
            List<ExaminorSetups> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminorSetups> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExaminorSetups>(sqlGetAll, mapper);
                IEnumerable<ExaminorSetups> collection = accessor.Execute();

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }



        public List<ExaminorSetupsDTO> ExaminerSetupGetAllByAcaCalProgram(int ProgramId, int yearno, int semesterno, int examid)
        {
            List<ExaminorSetupsDTO> ExaminerSetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExaminorSetupsDTO> mapper = ExaminorGetMaper();

                var accessor = db.CreateSprocAccessor<ExaminorSetupsDTO>("ExaminerSetupGetAllByAcaCalProgram", mapper);
                IEnumerable<ExaminorSetupsDTO> collection = accessor.Execute(ProgramId, yearno, semesterno, examid);

                ExaminerSetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return ExaminerSetupList;
            }

            return ExaminerSetupList;
        }








        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExaminorSetups examinorsetup, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, examinorsetup.ID);
            }


            db.AddInParameter(cmd, "AcaCalSectionId", DbType.Int32, examinorsetup.AcaCalSectionId);
            db.AddInParameter(cmd, "FirstExaminer", DbType.Int32, examinorsetup.FirstExaminer);
            db.AddInParameter(cmd, "SecondExaminor", DbType.Int32, examinorsetup.SecondExaminor);
            db.AddInParameter(cmd, "ThirdExaminor", DbType.Int32, examinorsetup.ThirdExaminor);
            db.AddInParameter(cmd, "ExamSetupDetailId", DbType.Int32, examinorsetup.ExamSetupDetailId);

            db.AddInParameter(cmd, "Attribute1", DbType.String, examinorsetup.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examinorsetup.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examinorsetup.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examinorsetup.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examinorsetup.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examinorsetup.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examinorsetup.ModifiedDate);

            return db;
        }

        private IRowMapper<ExaminorSetups> GetMaper()
        {
            IRowMapper<ExaminorSetups> mapper = MapBuilder<ExaminorSetups>.MapAllProperties()

           .Map(m => m.ID).ToColumn("ID")
        .Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
        .Map(m => m.FirstExaminer).ToColumn("FirstExaminer")
        .Map(m => m.SecondExaminor).ToColumn("SecondExaminor")
        .Map(m => m.ThirdExaminor).ToColumn("ThirdExaminor")
        .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
    
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

        private IRowMapper<ExaminorSetupsDTO> ExaminorGetMaper()
        {
            IRowMapper<ExaminorSetupsDTO> mapper = MapBuilder<ExaminorSetupsDTO>.MapAllProperties()
            .Map(m => m.ID).ToColumn("ID")
           .Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
            .Map(m => m.Title).ToColumn("Title")
           
            .Map(m => m.FirstExaminer).ToColumn("FirstExaminer")
            .Map(m => m.SecondExaminor).ToColumn("SecondExaminor")
            .Map(m => m.ThirdExaminor).ToColumn("ThirdExaminor")
            .Map(m => m.ExamName).ToColumn("ExamName")
        

        

            .Build();

            return mapper;
        }
       

        #endregion

    }
}
