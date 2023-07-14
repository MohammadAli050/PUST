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
    public partial class SqlGradeRepository : IGradeRepository
    {

        Database db = null;

        private string sqlInsert = "GradeInsert";
        private string sqlUpdate = "GradeUpdate";
        private string sqlDelete = "GradeDelete";
        private string sqlGetById = "GradeGetById";
        private string sqlGetAll = "GradeGetAll";
        private string sqlGetByGradeMasterId = "GradeGetByGradeMasterId";

        public int Insert(Grade grade)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, grade, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "GradeId");

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

        public bool Update(Grade grade)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, grade, isInsert);

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

                db.AddInParameter(cmd, "GradeId", DbType.Int32, id);
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

        public Grade GetById(int? id)
        {
            Grade _grade = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Grade> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Grade>(sqlGetById, rowMapper);
                _grade = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _grade;
            }

            return _grade;
        }

        public List<Grade> GetAll()
        {
            List<Grade> gradeList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Grade> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Grade>(sqlGetAll, mapper);
                IEnumerable<Grade> collection = accessor.Execute();

                gradeList = collection.ToList();
            }

            catch (Exception ex)
            {
                return gradeList;
            }

            return gradeList;
        }

        public List<Grade> GetByGradeMasterId(int gradeMasterId)
        {
            List<Grade> gradeList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Grade> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Grade>(sqlGetByGradeMasterId, mapper);
                IEnumerable<Grade> collection = accessor.Execute(gradeMasterId);

                gradeList = collection.ToList();
            }

            catch (Exception ex)
            {
                return gradeList;
            }

            return gradeList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, Grade grade, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "GradeId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "GradeId", DbType.Int32, grade.GradeId);
            }


            db.AddInParameter(cmd, "GradeMasterId", DbType.Int32, grade.GradeMasterId);
            db.AddInParameter(cmd, "GradeLetter", DbType.String, grade.GradeLetter);
            db.AddInParameter(cmd, "GradePoint", DbType.Decimal, grade.GradePoint);
            db.AddInParameter(cmd, "MinMarks", DbType.Decimal, grade.MinMarks);
            db.AddInParameter(cmd, "MaxMarks", DbType.Decimal, grade.MaxMarks);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, grade.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, grade.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, grade.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, grade.ModifiedDate);
            db.AddInParameter(cmd, "Sequence", DbType.Int32, grade.Sequence);

            return db;
        }

        private IRowMapper<Grade> GetMaper()
        {
            IRowMapper<Grade> mapper = MapBuilder<Grade>.MapAllProperties()

            .Map(m => m.GradeId).ToColumn("GradeId")
            .Map(m => m.GradeMasterId).ToColumn("GradeMasterId")
            .Map(m => m.GradeLetter).ToColumn("GradeLetter")
            .Map(m => m.GradePoint).ToColumn("GradePoint")
            .Map(m => m.MinMarks).ToColumn("MinMarks")
            .Map(m => m.MaxMarks).ToColumn("MaxMarks")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            .Map(m => m.Sequence).ToColumn("Sequence")

            .Build();

            return mapper;
        }
        #endregion

    }
}

