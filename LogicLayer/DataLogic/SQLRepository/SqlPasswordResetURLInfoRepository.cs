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
    public partial class SqlPasswordResetURLInfoRepository : IPasswordResetURLInfoRepository
    {

        Database db = null;

        private string sqlInsert = "PasswordResetURLInfoInsert";
        private string sqlUpdate = "PasswordResetURLInfoUpdate";
        private string sqlDelete = "PasswordResetURLInfoDelete";
        private string sqlGetById = "PasswordResetURLInfoGetById";
        private string sqlGetAll = "PasswordResetURLInfoGetAll";

        public int Insert(PasswordResetURLInfo passwordreseturlinfo)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, passwordreseturlinfo, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "Id");

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

        public bool Update(PasswordResetURLInfo passwordreseturlinfo)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, passwordreseturlinfo, isInsert);

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

                db.AddInParameter(cmd, "Id", DbType.Int32, id);
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

        public PasswordResetURLInfo GetById(int? id)
        {
            PasswordResetURLInfo _passwordreseturlinfo = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PasswordResetURLInfo> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PasswordResetURLInfo>(sqlGetById, rowMapper);
                _passwordreseturlinfo = accessor.Execute(id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _passwordreseturlinfo;
            }

            return _passwordreseturlinfo;
        }

        public PasswordResetURLInfo GetByLoginId(string LoginID)
        {
            PasswordResetURLInfo _passwordreseturlinfo = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PasswordResetURLInfo> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PasswordResetURLInfo>("PasswordResetURLInfoGetByLoginID", rowMapper);
                _passwordreseturlinfo = accessor.Execute(LoginID).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _passwordreseturlinfo;
            }

            return _passwordreseturlinfo;
        }

        public List<PasswordResetURLInfo> GetAll()
        {
            List<PasswordResetURLInfo> passwordreseturlinfoList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PasswordResetURLInfo> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PasswordResetURLInfo>(sqlGetAll, mapper);
                IEnumerable<PasswordResetURLInfo> collection = accessor.Execute();

                passwordreseturlinfoList = collection.ToList();
            }

            catch (Exception ex)
            {
                return passwordreseturlinfoList;
            }

            return passwordreseturlinfoList;
        }

        #region Mapper

        private Database addParam(Database db, DbCommand cmd, PasswordResetURLInfo passwordreseturlinfo, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "Id", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "Id", DbType.Int32, passwordreseturlinfo.Id);
            }


            db.AddInParameter(cmd, "LoginId", DbType.String, passwordreseturlinfo.LoginId);
            db.AddInParameter(cmd, "GeneratedId", DbType.String, passwordreseturlinfo.GeneratedId);
            db.AddInParameter(cmd, "OfficialEmail", DbType.String, passwordreseturlinfo.OfficialEmail);
            db.AddInParameter(cmd, "Validity", DbType.DateTime, passwordreseturlinfo.Validity);
            db.AddInParameter(cmd, "IsPasswordReset", DbType.Boolean, passwordreseturlinfo.IsPasswordReset);
            db.AddInParameter(cmd, "IsEmailSend", DbType.Boolean, passwordreseturlinfo.IsEmailSend);

            return db;
        }

        private IRowMapper<PasswordResetURLInfo> GetMaper()
        {
            IRowMapper<PasswordResetURLInfo> mapper = MapBuilder<PasswordResetURLInfo>.MapAllProperties()

            .Map(m => m.Id).ToColumn("Id")
            .Map(m => m.LoginId).ToColumn("LoginId")
            .Map(m => m.GeneratedId).ToColumn("GeneratedId")
            .Map(m => m.OfficialEmail).ToColumn("OfficialEmail")
            .Map(m => m.Validity).ToColumn("Validity")
            .Map(m => m.IsPasswordReset).ToColumn("IsPasswordReset")
            .Map(m => m.IsEmailSend).ToColumn("IsEmailSend")

            .Build();

            return mapper;
        }

        #endregion

        public int CheckLoginIdAndOfficialEmailAddressValidity(string loginID)
        {
            int flag = 0;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("CheckLoginIDAndOfficialEmailByLoginID");

                db.AddOutParameter(cmd, "Flag", DbType.Int32, 0);
                db.AddInParameter(cmd, "LoginID", DbType.String, loginID);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "Flag");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out flag);
                }
            }
            catch (Exception ex)
            {
                flag = 0;
            }

            return flag;
        }


    }
}
