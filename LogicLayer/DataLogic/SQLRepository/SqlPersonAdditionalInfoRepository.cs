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
    public partial class SqlPersonAdditionalInfoRepository : IPersonAdditionalInfoRepository
    {

        Database db = null;

        private string sqlInsert = "PersonAdditionalInfoInsert";
        private string sqlUpdate = "PersonAdditionalInfoUpdate";
        private string sqlDelete = "PersonAdditionalInfoDelete";
        private string sqlGetByIdPerson = "PersonAdditionalInfoGetByPersonId";
        private string sqlGetAll = "PersonAdditionalInfoGetAll";

        public int Insert(PersonAdditionalInfo personadditionalinfo)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, personadditionalinfo, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "PersonAdditionalInfoId");

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

        public bool Update(PersonAdditionalInfo personadditionalinfo)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, personadditionalinfo, isInsert);

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

                db.AddInParameter(cmd, "PersonAdditionalInfoId", DbType.Int32, id);
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

        public PersonAdditionalInfo GetByPersonId(int? id)
        {
            PersonAdditionalInfo _personadditionalinfo = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PersonAdditionalInfo> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PersonAdditionalInfo>(sqlGetByIdPerson, rowMapper);
                _personadditionalinfo = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _personadditionalinfo;
            }

            return _personadditionalinfo;
        }

        public List<PersonAdditionalInfo> GetAll()
        {
            List<PersonAdditionalInfo> personadditionalinfoList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PersonAdditionalInfo> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PersonAdditionalInfo>(sqlGetAll, mapper);
                IEnumerable<PersonAdditionalInfo> collection = accessor.Execute();

                personadditionalinfoList = collection.ToList();
            }

            catch (Exception ex)
            {
                return personadditionalinfoList;
            }

            return personadditionalinfoList;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, PersonAdditionalInfo personadditionalinfo, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "PersonAdditionalInfoId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "PersonAdditionalInfoId", DbType.Int32, personadditionalinfo.PersonAdditionalInfoId);
            }


            db.AddInParameter(cmd, "PersonId", DbType.Int32, personadditionalinfo.PersonId);
            db.AddInParameter(cmd, "PersonCategoryEnumValueId", DbType.Int32, personadditionalinfo.PersonCategoryEnumValueId);
            db.AddInParameter(cmd, "PersonStatusEnumValueId", DbType.Int32, personadditionalinfo.PersonStatusEnumValueId);
            db.AddInParameter(cmd, "SpouseName", DbType.String, personadditionalinfo.SpouseName);
            db.AddInParameter(cmd, "IsMilitary", DbType.Boolean, personadditionalinfo.IsMilitary);
            db.AddInParameter(cmd, "NationalIdNumber", DbType.String, personadditionalinfo.NationalIdNumber);
            db.AddInParameter(cmd, "BirthCertificateNumber", DbType.String, personadditionalinfo.BirthCertificateNumber);
            db.AddInParameter(cmd, "PersonalNo", DbType.String, personadditionalinfo.PersonalNo);
            db.AddInParameter(cmd, "IsMigrate", DbType.Boolean, personadditionalinfo.IsMigrate);
            db.AddInParameter(cmd, "BankId", DbType.Int32, personadditionalinfo.BankId);
            db.AddInParameter(cmd, "BankAccountNo", DbType.String, personadditionalinfo.BankAccountNo);

            return db;
        }

        private IRowMapper<PersonAdditionalInfo> GetMaper()
        {
            IRowMapper<PersonAdditionalInfo> mapper = MapBuilder<PersonAdditionalInfo>.MapAllProperties()

           .Map(m => m.PersonAdditionalInfoId).ToColumn("PersonAdditionalInfoId")
        .Map(m => m.PersonId).ToColumn("PersonId")
        .Map(m => m.PersonCategoryEnumValueId).ToColumn("PersonCategoryEnumValueId")
        .Map(m => m.PersonStatusEnumValueId).ToColumn("PersonStatusEnumValueId")
        .Map(m => m.SpouseName).ToColumn("SpouseName")
        .Map(m => m.IsMilitary).ToColumn("IsMilitary")
        .Map(m => m.NationalIdNumber).ToColumn("NationalIdNumber")
        .Map(m => m.BirthCertificateNumber).ToColumn("BirthCertificateNumber")
        .Map(m => m.PersonalNo).ToColumn("PersonalNo")
        .Map(m => m.IsMigrate).ToColumn("IsMigrate")
        .Map(m => m.BankId).ToColumn("BankId")
        .Map(m => m.BankAccountNo).ToColumn("BankAccountNo")

            .Build();

            return mapper;
        }
        #endregion

    }
}
