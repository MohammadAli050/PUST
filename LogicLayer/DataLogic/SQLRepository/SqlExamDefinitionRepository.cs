using LogicLayer.BusinessObjects;
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
    public partial class SqlExamDefinitionRepository : IExamDefinitionRepository
    {

        Database db = null;

        private string sqlInsert = "ExamDefinitionInsert";
        private string sqlUpdate = "ExamDefinitionUpdate";
        private string sqlDelete = "ExamDefinitionDeleteById";
        private string sqlGetById = "ExamDefinitionGetById";
        private string sqlGetAll = "ExamDefinitionGetAll";

        public int Insert(ExamDefinition examdefinition)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examdefinition, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamDefinitionId");

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

        public bool Update(ExamDefinition examdefinition)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examdefinition, isInsert);

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

                db.AddInParameter(cmd, "ExamDefinitionId", DbType.Int32, id);
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

        public ExamDefinition GetById(int? id)
        {
            ExamDefinition _examdefinition = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamDefinition> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamDefinition>(sqlGetById, rowMapper);
                _examdefinition = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examdefinition;
            }

            return _examdefinition;
        }

        public List<ExamDefinition> GetAll()
        {
            List<ExamDefinition> examdefinitionList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamDefinition> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamDefinition>(sqlGetAll, mapper);
                IEnumerable<ExamDefinition> collection = accessor.Execute();

                examdefinitionList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examdefinitionList;
            }

            return examdefinitionList;
        }

        public ExamDefinition GetByAcaCalIdExamTypeId(int? acaCalId, int? ExamTypeId)
        {
            ExamDefinition _examdefinition = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamDefinition> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamDefinition>("sqlGetByAcaCalIdExamTypeId", rowMapper);
                _examdefinition = accessor.Execute(acaCalId, ExamTypeId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examdefinition;
            }

            return _examdefinition;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamDefinition examdefinition, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamDefinitionId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamDefinitionId", DbType.Int32, examdefinition.ExamDefinitionId);
            }

            db.AddInParameter(cmd, "ExamDescription", DbType.String, examdefinition.ExamDescription);
            db.AddInParameter(cmd, "ExamShortName", DbType.String, examdefinition.ExamShortName);
            db.AddInParameter(cmd, "ExamShortNameBMA", DbType.String, examdefinition.ExamShortNameBMA);
            db.AddInParameter(cmd, "AcaCalId", DbType.Int32, examdefinition.AcaCalId);
            db.AddInParameter(cmd, "ProgramType", DbType.String, examdefinition.ProgramType);
            db.AddInParameter(cmd, "ExamTypeId", DbType.Int32, examdefinition.ExamTypeId);
            db.AddInParameter(cmd, "LastDateOfIncourseMarkSubmission", DbType.DateTime, examdefinition.LastDateOfIncourseMarkSubmission);
            db.AddInParameter(cmd, "StartDateOfTermFinal", DbType.DateTime, examdefinition.StartDateOfTermFinal);
            db.AddInParameter(cmd, "EndDateOfTermFinal", DbType.DateTime, examdefinition.EndDateOfTermFinal);
            db.AddInParameter(cmd, "StartDateOfTermFinalBMA", DbType.DateTime, examdefinition.StartDateOfTermFinalBMA);
            db.AddInParameter(cmd, "EndDateOfTermFinalBMA", DbType.DateTime, examdefinition.EndDateOfTermFinalBMA);
            db.AddInParameter(cmd, "LastDateOfTermFinalMarkSubmission", DbType.DateTime, examdefinition.LastDateOfTermFinalMarkSubmission);
            db.AddInParameter(cmd, "LastDateOfScrutinization", DbType.DateTime, examdefinition.LastDateOfScrutinization);
            db.AddInParameter(cmd, "LastDateOfTabulation", DbType.DateTime, examdefinition.LastDateOfTabulation);
            db.AddInParameter(cmd, "ResultPublicationDate", DbType.DateTime, examdefinition.ResultPublicationDate);
            db.AddInParameter(cmd, "Remarks", DbType.String, examdefinition.Remarks);
            db.AddInParameter(cmd, "Attribute1", DbType.String, examdefinition.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examdefinition.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examdefinition.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examdefinition.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examdefinition.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examdefinition.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examdefinition.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamDefinition> GetMaper()
        {
            IRowMapper<ExamDefinition> mapper = MapBuilder<ExamDefinition>.MapAllProperties()

           .Map(m => m.ExamDefinitionId).ToColumn("ExamDefinitionId")
            .Map(m => m.ExamDescription).ToColumn("ExamDescription")
            .Map(m => m.ExamShortName).ToColumn("ExamShortName")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.ProgramType).ToColumn("ProgramType")
            .Map(m => m.ExamTypeId).ToColumn("ExamTypeId")
            .Map(m => m.LastDateOfIncourseMarkSubmission).ToColumn("LastDateOfIncourseMarkSubmission")
            .Map(m => m.StartDateOfTermFinal).ToColumn("StartDateOfTermFinal")
            .Map(m => m.EndDateOfTermFinal).ToColumn("EndDateOfTermFinal")
            .Map(m => m.LastDateOfTermFinalMarkSubmission).ToColumn("LastDateOfTermFinalMarkSubmission")
            .Map(m => m.LastDateOfScrutinization).ToColumn("LastDateOfScrutinization")
            .Map(m => m.LastDateOfTabulation).ToColumn("LastDateOfTabulation")
            .Map(m => m.ResultPublicationDate).ToColumn("ResultPublicationDate")
            .Map(m => m.Remarks).ToColumn("Remarks")
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
        #endregion

        public ExamDefinition GetByAcaCalId(int acaCalId)
        {
            ExamDefinition _examdefinition = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamDefinition> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamDefinition>("ExamDefinitionGetGetByAcaCalId", rowMapper);
                _examdefinition = accessor.Execute(acaCalId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _examdefinition;
            }

            return _examdefinition;
        }

    }
}
