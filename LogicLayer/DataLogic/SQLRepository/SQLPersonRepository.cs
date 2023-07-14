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
    public partial class SQLPersonRepository : IPersonRepository
    {
        Database db = null;

        private string sqlInsert = "PersonInsert";
        private string sqlUpdate = "PersonUpdate";
        private string sqlDelete = "PersonDeleteById";
        private string sqlGetById = "PersonGetById";
        private string sqlGetAll = "PersonGetAll";
        private string sqlGetByUserID = "PersonGetByUserID";

        public int Insert(Person person)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, person, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "PersonID");

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

        public bool Update(Person person)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, person, isInsert);

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

                db.AddInParameter(cmd, "PersonID", DbType.Int32, id);
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
        public bool ExaminerSetupGetAllByAcaCalProgramDataInsert(int programId, int yearno, int semesterno, int examid)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("ExaminerSetupGetAllByAcaCalProgramDataInsert");

                db.AddInParameter(cmd, "ProgramID", DbType.Int32, programId);
                db.AddInParameter(cmd, "YearId", DbType.Int32, yearno);
                db.AddInParameter(cmd, "SemesterNo", DbType.Int32, semesterno);
                db.AddInParameter(cmd, "ExamId", DbType.Int32, examid);
                db.AddOutParameter(cmd, "ReturnValue", DbType.Int32, 0);
         
                //db.AddInParameter(cmd, "RegistrationSession", DbType.Int32, registrationSession);

                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ReturnValue");

                int id = 0;
                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }

                if (id > 0)
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
        public Person GetById(int? id)
        {
            Person _person = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Person> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Person>(sqlGetById, rowMapper);
                _person = accessor.Execute(id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _person;
            }

            return _person;
        }

        public List<Person> GetAll()
        {
            List<Person> paperList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Person> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Person>(sqlGetAll, mapper);
                IEnumerable<Person> collection = accessor.Execute();

                paperList = collection.ToList();
            }

            catch (Exception ex)
            {
                return paperList;
            }

            return paperList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, Person person, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "PersonID", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, person.CreatedBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, person.CreatedDate);
            }
            else
            {
                db.AddInParameter(cmd, "PersonID", DbType.Int32, person.PersonID);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, person.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, person.ModifiedDate);
            }


            db.AddInParameter(cmd, "FullName", DbType.String, person.FullName);
            db.AddInParameter(cmd, "BanglaName", DbType.String, person.BanglaName);
            db.AddInParameter(cmd, "DOB", DbType.DateTime, person.DOB);
            db.AddInParameter(cmd, "Gender", DbType.String, person.Gender);
            db.AddInParameter(cmd, "MatrialStatus", DbType.String, person.MatrialStatus);
            db.AddInParameter(cmd, "BloodGroup", DbType.String, person.BloodGroup);
            db.AddInParameter(cmd, "Religion", DbType.Int32, person.ReligionId);
            db.AddInParameter(cmd, "FatherName", DbType.String, person.FatherName);
            db.AddInParameter(cmd, "FatherProfession", DbType.String, person.FatherProfession);
            db.AddInParameter(cmd, "MotherName", DbType.String, person.MotherName);
            db.AddInParameter(cmd, "MotherProfession", DbType.String, person.MotherProfession);
            db.AddInParameter(cmd, "Nationality", DbType.String, person.Nationality);
            db.AddInParameter(cmd, "PhotoPath", DbType.String, person.PhotoPath);
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, person.IsActive);
            db.AddInParameter(cmd, "IsDeleted", DbType.Boolean, person.IsDeleted);
            db.AddInParameter(cmd, "Remarks", DbType.String, person.Remarks);
            db.AddInParameter(cmd, "Phone", DbType.String, person.Phone);
            db.AddInParameter(cmd, "Email", DbType.String, person.Email);
            db.AddInParameter(cmd, "GuardianName", DbType.String, person.GuardianName);
            db.AddInParameter(cmd, "SMSContactSelf", DbType.String, person.SMSContactSelf);
            db.AddInParameter(cmd, "SMSContactGuardian", DbType.String, person.SMSContactGuardian);
            db.AddInParameter(cmd, "TypeId", DbType.Int32, person.TypeId);
            db.AddInParameter(cmd, "GuardianEmail", DbType.String, person.GuardianEmail);
            return db;
        }

        private IRowMapper<Person> GetMaper()
        {
            IRowMapper<Person> mapper = MapBuilder<Person>.MapAllProperties()
            .Map(m => m.PersonID).ToColumn("PersonID")
            .Map(m => m.FullName).ToColumn("FullName")
            .Map(m => m.BanglaName).ToColumn("BanglaName")
            .Map(m => m.DOB).ToColumn("DOB")
            .Map(m => m.Gender).ToColumn("Gender")
            .Map(m => m.MatrialStatus).ToColumn("MatrialStatus")
            .Map(m => m.BloodGroup).ToColumn("BloodGroup")
            .Map(m => m.ReligionId).ToColumn("Religion")
            .Map(m => m.FatherName).ToColumn("FatherName")
            .Map(m => m.FatherProfession).ToColumn("FatherProfession")
            .Map(m => m.MotherName).ToColumn("MotherName")
            .Map(m => m.MotherProfession).ToColumn("MotherProfession")
            .Map(m => m.Nationality).ToColumn("Nationality")
            .Map(m => m.PhotoPath).ToColumn("PhotoPath")
            .Map(m => m.IsActive).ToColumn("IsActive")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.Remarks).ToColumn("Remarks")
            .Map(m => m.Phone).ToColumn("Phone")
            .Map(m => m.Email).ToColumn("Email")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            .Map(m => m.TypeId).ToColumn("TypeId")
            .Map(m => m.GuardianName).ToColumn("GuardianName")
            .Map(m => m.SMSContactSelf).ToColumn("SMSContactSelf")
            .Map(m => m.SMSContactGuardian).ToColumn("SMSContactGuardian")
            .Map(m => m.GuardianEmail).ToColumn("GuardianEmail")
            .Build();

            return mapper;
        }
        #endregion

        public Person GetByUserId(int User_ID)
        {
            Person _person = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Person> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Person>(sqlGetByUserID, rowMapper);
                _person = accessor.Execute(User_ID).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _person;
            }

            return _person;
        }
    }
}
