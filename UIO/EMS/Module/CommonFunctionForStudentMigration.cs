using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class CommonFunctionForStudentMigration
    {

        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;


        BussinessObject.UIUMSUser BaseCurrentUserObj = null;
        public static StudentMigrationObj MigrateStudent(string AdmissionRoll, int ProgramId, int SessionId, string Roll, string Name, string DOB, string FName, string MName, string Hall, int MigratedBy, string LogInID, string _pageId, string _pageName, string _pageUrl)
        {
            StudentMigrationObj MigrationObj = new StudentMigrationObj();
            try
            {
                MigrationObj.StudentID = Roll;
                MigrationObj.Name = Name;


                bool RollExists = CheckRoll(Roll);
                if (!RollExists) // New Student
                {
                    int PersonId = InsertIntoPersonTable(Name, DOB, FName, MName, MigratedBy);
                    if (PersonId > 0)
                    {
                        int StudentId = InsertIntoStudentTable(AdmissionRoll, ProgramId, SessionId, Roll, PersonId, MigratedBy, Hall);

                        if (StudentId > 0)
                        {
                            int UserId = InsertIntoUserAndUserInPersonTable(Roll, PersonId, MigratedBy);

                            MigrationObj.Status = 1;
                            MigrationObj.Reason = "Student inserted";

                            #region Log Insert

                            try
                            {
                                InsertLog("New Student Migration", LogInID + " Migrated a new student with StudentID : " + Roll + " and Name : " + Name, Roll, LogInID, _pageId, _pageName, _pageUrl);
                            }
                            catch (Exception ex)
                            {
                            }

                            #endregion

                        }
                        else
                        {
                            Person PerObj = PersonManager.GetById(PersonId);
                            if (PerObj != null)
                                PersonManager.Delete(PerObj.PersonID);

                            MigrationObj.Status = 0;
                            MigrationObj.Reason = "Student not inserted";
                        }

                    }
                    else
                    {
                        MigrationObj.Status = 0;
                        MigrationObj.Reason = "Person not inserted";
                    }
                }
                else
                {
                    MigrationObj.Status = 0;
                    MigrationObj.Reason = "Student Id already exists";
                }



            }
            catch (Exception ex)
            {
            }
            return MigrationObj;
        }

        private static int InsertIntoUserAndUserInPersonTable(string Roll, int PersonId, int MigratedBy)
        {
            int UserId = 0;
            try
            {
                User usr = UserManager.GetByLogInId(Roll);
                if (usr == null)
                {

                    User NewObj = new User();

                    NewObj.LogInID = Roll;
                    NewObj.Password = "123456@#";
                    NewObj.RoleID = Convert.ToInt32(CommonUtility.CommonEnum.Role.Student);
                    NewObj.RoleExistStartDate = DateTime.Now;
                    NewObj.RoleExistEndDate = DateTime.Now.AddYears(6);
                    NewObj.IsActive = true;
                    NewObj.CreatedBy = MigratedBy;
                    NewObj.CreatedDate = DateTime.Now;
                    NewObj.ModifiedBy = MigratedBy;
                    NewObj.ModifiedDate = DateTime.Now;

                    UserId = UserManager.Insert(NewObj);

                    if (UserId > 0)
                    {
                        UserInPerson uip = new UserInPerson();

                        uip.User_ID = UserId;
                        uip.PersonID = PersonId;
                        uip.CreatedBy = MigratedBy;
                        uip.CreatedDate = DateTime.Now;
                        uip.ModifiedBy = MigratedBy;
                        uip.ModifiedDate = DateTime.Now;

                        UserInPersonManager.Insert(uip);
                    }
                }
                else
                    UserId = -1;
            }
            catch (Exception ex)
            {
            }
            return UserId;
        }

        private static int InsertIntoStudentTable(string AdmissionRoll, int ProgramId, int SessionId, string Roll, int PersonId, int MigratedBy, string Hall)
        {
            int StudentId = 0;
            try
            {
                Student stdObj = StudentManager.GetByRoll(Roll);
                if (stdObj == null)
                {
                    int HallId = 0;

                    try
                    {
                        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                        var HallInfo = ucamContext.HallInformations.Where(x => x.HallCode == Hall).FirstOrDefault();
                        if (HallInfo != null)
                            HallId = HallInfo.Id;
                    }
                    catch (Exception ex)
                    {
                    }

                    Student NewObj = new Student();

                    NewObj.ProgramID = ProgramId;
                    NewObj.StudentAdmissionAcaCalId = SessionId;
                    NewObj.Roll = Roll;
                    NewObj.IsActive = true;
                    NewObj.PersonID = PersonId;
                    NewObj.CreatedBy = MigratedBy;
                    NewObj.CreatedDate = DateTime.Now;
                    NewObj.Attribute3 = AdmissionRoll;
                    NewObj.HallInfoId = HallId;
                    StudentId = StudentManager.Insert(NewObj);

                }
            }
            catch (Exception ex)
            {
            }

            return StudentId;
        }

        private static int InsertIntoPersonTable(string Name, string DOB, string FName, string MName, int MigratedBy)
        {
            int Id = 0;
            try
            {
                Person NewObj = new Person();


                NewObj.FullName = Name;
                if (DOB != "")
                {
                    DateTime DateInDateTime = DateTime.ParseExact(DOB.Replace("/", string.Empty), "ddMMyyyy", null);
                    NewObj.DOB = Convert.ToDateTime(DateInDateTime);
                }
                NewObj.FatherName = FName;
                NewObj.MotherName = MName;
                NewObj.CreatedBy = MigratedBy;
                NewObj.CreatedDate = DateTime.Now;

                Id = PersonManager.Insert(NewObj);

            }
            catch (Exception ex)
            {
            }

            return Id;
        }

        private static bool CheckRoll(string Roll)
        {
            bool IsExists = false;

            Student std = StudentManager.GetByRoll(Roll);

            if (std != null)
                IsExists = true;

            return IsExists;

        }

        private static void InsertLog(string EventName, string Message, string Roll, string LoginId, string _pageId, string _pageName, string _pageUrl)
        {
            LogGeneralManager.Insert(
                                      DateTime.Now,
                                      "",
                                      "",
                                      LoginId,
                                      "",
                                      "",
                                      EventName,
                                      Message,
                                      "Normal",
                                       _pageId.ToString(),
                                      _pageName.ToString(),
                                      _pageUrl,
                                      Roll);
        }


    }
}