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

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExamSetupWithExamCommitteesRepository : IExamSetupWithExamCommitteesRepository
    {

        Database db = null;

        private string sqlInsert = "ExamSetupWithExamCommitteesInsert";
        private string sqlUpdate = "ExamSetupWithExamCommitteesUpdate";
        private string sqlDelete = "ExamSetupWithExamCommitteesDeleteById";
        private string sqlGetById = "ExamSetupWithExamCommitteesGetById";
        private string sqlGetAll = "ExamSetupWithExamCommitteesGetAll";

        public int Insert(ExamSetupWithExamCommittees examsetupwithexamcommittees)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examsetupwithexamcommittees, isInsert);
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

        public bool Update(ExamSetupWithExamCommittees examsetupwithexamcommittees)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examsetupwithexamcommittees, isInsert);

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

        public ExamSetupWithExamCommittees GetById(int? id)
        {
            ExamSetupWithExamCommittees _examsetupwithexamcommittees = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupWithExamCommittees> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupWithExamCommittees>(sqlGetById, rowMapper);
                _examsetupwithexamcommittees = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetupwithexamcommittees;
            }

            return _examsetupwithexamcommittees;
        }

        public List<ExamSetupWithExamCommittees> GetAll()
        {
            List<ExamSetupWithExamCommittees> examsetupwithexamcommitteesList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupWithExamCommittees> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupWithExamCommittees>(sqlGetAll, mapper);
                IEnumerable<ExamSetupWithExamCommittees> collection = accessor.Execute();

                examsetupwithexamcommitteesList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupwithexamcommitteesList;
            }

            return examsetupwithexamcommitteesList;
        }


        public ExamSetupWithExamCommittees GetByExamSetupId(int examSetupId)
        {
            ExamSetupWithExamCommittees _examsetupwithexamcommittees = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupWithExamCommittees> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupWithExamCommittees>("ExamSetupWithExamCommitteesGetByExamSetupId", rowMapper);
                _examsetupwithexamcommittees = accessor.Execute(examSetupId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetupwithexamcommittees;
            }

            return _examsetupwithexamcommittees;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamSetupWithExamCommittees examsetupwithexamcommittees, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, examsetupwithexamcommittees.ID);
            }


            db.AddInParameter(cmd, "HeldInProgramRelationId", DbType.Int32, examsetupwithexamcommittees.HeldInProgramRelationId);
            db.AddInParameter(cmd, "ExamCommitteeChairmanDeptId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeChairmanDeptId);
            db.AddInParameter(cmd, "ExamCommitteeChairmanId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeChairmanId);
            db.AddInParameter(cmd, "ExamCommitteeMemberOneDeptId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeMemberOneDeptId);
            db.AddInParameter(cmd, "ExamCommitteeMemberOneId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeMemberOneId);
            db.AddInParameter(cmd, "ExamCommitteeMemberTwoDeptId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeMemberTwoDeptId);
            db.AddInParameter(cmd, "ExamCommitteeMemberTwoId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeMemberTwoId);
            db.AddInParameter(cmd, "ExamCommitteeExternalMemberDeptId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeExternalMemberDeptId);
            db.AddInParameter(cmd, "ExamCommitteeExternalMemberId", DbType.Int32, examsetupwithexamcommittees.ExamCommitteeExternalMemberId);
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, examsetupwithexamcommittees.IsActive);
            db.AddInParameter(cmd, "Attribute1", DbType.String, examsetupwithexamcommittees.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examsetupwithexamcommittees.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examsetupwithexamcommittees.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examsetupwithexamcommittees.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examsetupwithexamcommittees.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examsetupwithexamcommittees.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examsetupwithexamcommittees.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamSetupWithExamCommittees> GetMaper()
        {
            IRowMapper<ExamSetupWithExamCommittees> mapper = MapBuilder<ExamSetupWithExamCommittees>.MapAllProperties()

           .Map(m => m.ID).ToColumn("ID")
        .Map(m => m.HeldInProgramRelationId).ToColumn("HeldInProgramRelationId")
        .Map(m => m.ExamCommitteeChairmanDeptId).ToColumn("ExamCommitteeChairmanDeptId")
        .Map(m => m.ExamCommitteeChairmanId).ToColumn("ExamCommitteeChairmanId")
        .Map(m => m.ExamCommitteeMemberOneDeptId).ToColumn("ExamCommitteeMemberOneDeptId")
        .Map(m => m.ExamCommitteeMemberOneId).ToColumn("ExamCommitteeMemberOneId")
        .Map(m => m.ExamCommitteeMemberTwoDeptId).ToColumn("ExamCommitteeMemberTwoDeptId")
        .Map(m => m.ExamCommitteeMemberTwoId).ToColumn("ExamCommitteeMemberTwoId")
        .Map(m => m.ExamCommitteeExternalMemberDeptId).ToColumn("ExamCommitteeExternalMemberDeptId")
        .Map(m => m.ExamCommitteeExternalMemberId).ToColumn("ExamCommitteeExternalMemberId")
        .Map(m => m.IsActive).ToColumn("IsActive")
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

    }
}

