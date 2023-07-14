using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI.WebControls;
using CommonUtility;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.BusinessLogic
{
    public class BillHistoryMasterManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "BillHistoryMasterCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<BillHistoryMaster> GetCacheAsList(string rawKey)
        {
            List<BillHistoryMaster> list = (List<BillHistoryMaster>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static BillHistoryMaster GetCacheItem(string rawKey)
        {
            BillHistoryMaster item = (BillHistoryMaster)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return item;
        }

        public static void AddCacheItem(string rawKey, object value)
        {
            System.Web.Caching.Cache DataCache = HttpRuntime.Cache;

            // Make sure MasterCacheKeyArray[0] is in the cache - if not, add it
            if (DataCache[MasterCacheKeyArray[0]] == null)
                DataCache[MasterCacheKeyArray[0]] = DateTime.Now;

            // Add a CacheDependency
            System.Web.Caching.CacheDependency dependency = new System.Web.Caching.CacheDependency(null, MasterCacheKeyArray);
            DataCache.Insert(GetCacheKey(rawKey), value, dependency, DateTime.Now.AddMinutes(CacheDuration), System.Web.Caching.Cache.NoSlidingExpiration);
        }



        public static void InvalidateCache()
        {
            // Remove the cache dependency
            HttpRuntime.Cache.Remove(MasterCacheKeyArray[0]);
        }

        #endregion

        #region Bill Generation

        public static void GenerateBill(int studentId, int sessionId, User user)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Student student = StudentManager.GetById(studentId);
                    AcademicCalender academicCalender = AcademicCalenderManager.GetById(sessionId);

                    if (academicCalender.CalenderUnitTypeID == 5 || academicCalender.CalenderUnitTypeID == 6)
                    {
                        InsertShort_SupplementaryBillMaster(student, sessionId, academicCalender.CalenderUnitTypeID, user.User_ID);
                    }
                    //else if ((student.StudentAddionalInformation.StudentQuotaEnumId != 37 && student.StudentAddionalInformation.StudentQuotaEnumId != 38))
                    //{
                        InsertBillMaster(student, sessionId, user);
                    //}

                    //if ((student.StudentAddionalInformation.StudentQuotaEnumId != 37 &&
                    //     student.StudentAddionalInformation.StudentQuotaEnumId != 38) ||
                    //    (academicCalender.CalenderUnitTypeID == 5 || academicCalender.CalenderUnitTypeID == 6))
                    //{
                    //    InsertBillMaster(student, sessionId, user);
                    //}
                    //else
                    //{
                        //RegistrationWorksheetManager.confirmRegistrationAfterPaymentPosting(studentId, sessionId, user.User_ID);
                    //}

                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    scope.Complete();
                }

            }
        }

        public static void InsertBillMaster(Student student, int sessionId, User user)
        {
            try
            {
                List<StudentBillDetailsDTO> studentBillDeatilList = BillHistoryManager.GetStudentBillingDetails(student.StudentID, student.ProgramID, student.BatchId, sessionId); //acaCal.AcademicCalenderID

                List<StudentBillDetailsDTO> studentAnnualBillDeatilList = BillHistoryManager.GenerateStudentAnnualBillingDetails(student.StudentID, student.ProgramID, student.BatchId, sessionId);
                if (studentAnnualBillDeatilList != null && studentAnnualBillDeatilList.Any())
                {
                    studentBillDeatilList = studentBillDeatilList.Concat(studentAnnualBillDeatilList).ToList();
                }

                decimal billMasterAmount = studentBillDeatilList.Sum(totalAmount => totalAmount.Amount);
                if (billMasterAmount > 0)
                {
                    BillHistoryMaster billHistoryMaster = new BillHistoryMaster();
                    billHistoryMaster.StudentId = student.StudentID;
                    billHistoryMaster.Amount = billMasterAmount;
                    billHistoryMaster.BillingDate = DateTime.Now;
                    billHistoryMaster.ReferenceNo = Convert.ToString(BillHistoryMasterManager.GetBillMasterMaxReferenceNo(billHistoryMaster.BillingDate));

                    billHistoryMaster.IsDeleted = false;
                    billHistoryMaster.IsDue = false;
                    billHistoryMaster.ParentBillHistroyMasterId = 0;
                    billHistoryMaster.AcaCalId = sessionId;

                    billHistoryMaster.CreatedBy = user.User_ID;
                    billHistoryMaster.CreatedDate = DateTime.Now;
                    billHistoryMaster.ModifiedBy = user.User_ID;
                    billHistoryMaster.ModifiedDate = DateTime.Now;

                    int billHistoryMasterId = BillHistoryMasterManager.Insert(billHistoryMaster);
                    if (billHistoryMasterId > 0)
                    {
                        InsertBillHistory(student, billHistoryMasterId, sessionId, studentBillDeatilList, user);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void InsertBillHistory(Student student, int billHistoryMasterId, int sessionId, List<StudentBillDetailsDTO> studentBillDeatilList, User user)
        {
            try
            {

                foreach (StudentBillDetailsDTO studentBillDetails in studentBillDeatilList)
                {
                    decimal feeAmount = studentBillDetails.Amount;
                    if (feeAmount > 0)
                    {
                        BillHistory billHistroyObj = new BillHistory();
                        billHistroyObj.StudentId = studentBillDetails.StudentID;
                        billHistroyObj.StudentCourseHistoryId = studentBillDetails.StudentCourseHistoryId;
                        billHistroyObj.FeeSetupId = studentBillDetails.FeeSetupId;
                        billHistroyObj.TypeDefinitionId = studentBillDetails.TypeDefinitionID;
                        billHistroyObj.AcaCalId = sessionId;
                        billHistroyObj.Fees = feeAmount;
                        billHistroyObj.BillingDate = DateTime.Now;
                        billHistroyObj.IsDeleted = false;
                        billHistroyObj.BillHistoryMasterId = Convert.ToInt32(billHistoryMasterId);
                        billHistroyObj.CreatedBy = user.User_ID;
                        billHistroyObj.CreatedDate = DateTime.Now;
                        billHistroyObj.ModifiedBy = user.User_ID;
                        billHistroyObj.ModifiedDate = DateTime.Now;

                        int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                        if (billInsertId > 0)
                        {
                            if (feeAmount > 0)
                            {
                                #region Log Insert
                                //LogGeneralManager.Insert(
                                //     DateTime.Now,
                                //     BaseAcaCalCurrent.Code,
                                //     BaseAcaCalCurrent.FullCode,
                                //     BaseCurrentUserObj.LogInID,
                                //     "",
                                //     "",
                                //     "Bill History Entry",
                                //     user.LogInID + " posted bill for Roll : " + student.Roll + ", Fee Type : " + billHistroyObj.TypeDefinitionId + ", Amount: " + feeAmount.ToString() + ", Session : " + sessionId,
                                //     "normal",
                                //     ((int)CommonEnum.PageName.BillPosting).ToString(),
                                //     CommonEnum.PageName.BillPosting.ToString(),
                                //     _pageUrl,
                                //     student.Roll);
                                #endregion
                            }
                        }
                        else
                        {

                            #region Log Insert
                            //LogGeneralManager.Insert(
                            //     DateTime.Now,
                            //     BaseAcaCalCurrent.Code,
                            //     BaseAcaCalCurrent.FullCode,
                            //     BaseCurrentUserObj.LogInID,
                            //     "",
                            //     "",
                            //     "Unsuccessful Bill History Entry",
                            //     BaseCurrentUserObj.LogInID + " unsuccessfull posted bill for Roll : " + student.Roll + ", Fee Type : " + studentBillDetails.TypeDefinitionID + ", Amount: " + studentBillDetails.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                            //     "normal",
                            //     ((int)CommonEnum.PageName.BillPosting).ToString(),
                            //     CommonEnum.PageName.BillPosting.ToString(),
                            //     _pageUrl,
                            //     student.Roll);
                            #endregion
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion bill generate

        #region Non Collegiate Bill Generation

        public static void GenerateNonCollegiateBill(int? campusId, int? programId, int? acaCalId, int? acaCalSectionId, int usrId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    List<StudentBillDetailsDTO> studentBillDetailList =
                        BillHistoryManager.GetNcBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId(campusId,
                            programId, acaCalId, acaCalSectionId);
                    foreach (StudentBillDetailsDTO billDetailsDto in studentBillDetailList)
                    {
                        InsertNCBillMaster(billDetailsDto, usrId);
                    }

                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    scope.Complete();
                }
            }
        }

        public static void InsertNCBillMaster(StudentBillDetailsDTO studentBillDetail, int userId)
        {
            try
            {
                if (studentBillDetail.Amount > 0)
                {
                    BillHistoryMaster billHistoryMaster = new BillHistoryMaster();
                    billHistoryMaster.StudentId = studentBillDetail.StudentID;
                    billHistoryMaster.Amount = studentBillDetail.Amount;
                    billHistoryMaster.BillingDate = DateTime.Now;
                    billHistoryMaster.ReferenceNo = Convert.ToString(BillHistoryMasterManager.GetBillMasterMaxReferenceNo(billHistoryMaster.BillingDate));

                    billHistoryMaster.IsDeleted = false;
                    billHistoryMaster.IsDue = false;
                    billHistoryMaster.ParentBillHistroyMasterId = 0;
                    billHistoryMaster.AcaCalId = studentBillDetail.AcaCalId;

                    billHistoryMaster.CreatedBy = userId;
                    billHistoryMaster.CreatedDate = DateTime.Now;
                    billHistoryMaster.ModifiedBy = userId;
                    billHistoryMaster.ModifiedDate = DateTime.Now;

                    int billHistoryMasterId = BillHistoryMasterManager.Insert(billHistoryMaster);
                    if (billHistoryMasterId > 0)
                    {
                        InsertNcBillHistory(studentBillDetail, billHistoryMasterId, userId);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void InsertNcBillHistory(StudentBillDetailsDTO studentBillDetail, int billHistoryMasterId, int userId)
        {
            try
            {
                BillHistory billHistroyObj = new BillHistory();
                billHistroyObj.StudentId = studentBillDetail.StudentID;
                billHistroyObj.StudentCourseHistoryId = studentBillDetail.StudentCourseHistoryId;
                billHistroyObj.FeeSetupId = studentBillDetail.FeeSetupId;
                billHistroyObj.TypeDefinitionId = studentBillDetail.TypeDefinitionID;
                billHistroyObj.AcaCalId = studentBillDetail.AcaCalId;
                billHistroyObj.Fees = studentBillDetail.Amount;
                billHistroyObj.BillingDate = DateTime.Now;
                billHistroyObj.IsDeleted = false;
                billHistroyObj.BillHistoryMasterId = billHistoryMasterId;
                billHistroyObj.CreatedBy = userId;
                billHistroyObj.CreatedDate = DateTime.Now;
                billHistroyObj.ModifiedBy = userId;
                billHistroyObj.ModifiedDate = DateTime.Now;

                int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                if (billInsertId > 0)
                {
                    #region Log Insert
                    //LogGeneralManager.Insert(
                    //     DateTime.Now,
                    //     BaseAcaCalCurrent.Code,
                    //     BaseAcaCalCurrent.FullCode,
                    //     BaseCurrentUserObj.LogInID,
                    //     "",
                    //     "",
                    //     "Bill History Entry",
                    //     user.LogInID + " posted bill for Roll : " + student.Roll + ", Fee Type : " + billHistroyObj.TypeDefinitionId + ", Amount: " + feeAmount.ToString() + ", Session : " + sessionId,
                    //     "normal",
                    //     ((int)CommonEnum.PageName.BillPosting).ToString(),
                    //     CommonEnum.PageName.BillPosting.ToString(),
                    //     _pageUrl,
                    //     student.Roll);
                    #endregion
                }
                else
                {
                    #region Log Insert
                    //LogGeneralManager.Insert(
                    //     DateTime.Now,
                    //     BaseAcaCalCurrent.Code,
                    //     BaseAcaCalCurrent.FullCode,
                    //     BaseCurrentUserObj.LogInID,
                    //     "",
                    //     "",
                    //     "Unsuccessful Bill History Entry",
                    //     BaseCurrentUserObj.LogInID + " unsuccessfull posted bill for Roll : " + student.Roll + ", Fee Type : " + studentBillDetails.TypeDefinitionID + ", Amount: " + studentBillDetails.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                    //     "normal",
                    //     ((int)CommonEnum.PageName.BillPosting).ToString(),
                    //     CommonEnum.PageName.BillPosting.ToString(),
                    //     _pageUrl,
                    //     student.Roll);
                    #endregion
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Insert Short / Supplementary Bill

        private static void InsertShort_SupplementaryBillMaster(Student student, int sessionId, int calenderUnitTypeId, int userId)
        {
            try
            {
                List<StudentBillDetailsDTO> studentBillDetailList = BillHistoryManager.GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(student.StudentID, sessionId, calenderUnitTypeId); //acaCal.AcademicCalenderID

                decimal billMasterAmount = studentBillDetailList.Sum(totalAmount => totalAmount.Amount);
                if (billMasterAmount > 0)
                {
                    BillHistoryMaster billHistoryMaster = new BillHistoryMaster();
                    billHistoryMaster.StudentId = student.StudentID;
                    billHistoryMaster.Amount = billMasterAmount;
                    billHistoryMaster.BillingDate = DateTime.Now;
                    billHistoryMaster.ReferenceNo = Convert.ToString(BillHistoryMasterManager.GetBillMasterMaxReferenceNo(billHistoryMaster.BillingDate));

                    billHistoryMaster.IsDeleted = false;
                    billHistoryMaster.IsDue = false;
                    billHistoryMaster.ParentBillHistroyMasterId = 0;
                    billHistoryMaster.AcaCalId = sessionId;

                    billHistoryMaster.CreatedBy = userId;
                    billHistoryMaster.CreatedDate = DateTime.Now;
                    billHistoryMaster.ModifiedBy = userId;
                    billHistoryMaster.ModifiedDate = DateTime.Now;

                    int billHistoryMasterId = BillHistoryMasterManager.Insert(billHistoryMaster);
                    if (billHistoryMasterId > 0)
                    {
                        //#region Log Insert Bill Master Short Supplementary
                        //LogGeneralManager.Insert(
                        //    DateTime.Now,
                        //    BaseAcaCalCurrent.Code,
                        //    BaseAcaCalCurrent.FullCode,
                        //    BaseCurrentUserObj.LogInID,
                        //    "",
                        //    "",
                        //    "Bill History Master for Short Supplementary Entry",
                        //    BaseCurrentUserObj.LogInID + " posted Short Supplementary bill history Master for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + billHistoryMaster.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                        //    "normal",
                        //    ((int)CommonEnum.PageName.BillGeneration).ToString(),
                        //    CommonEnum.PageName.BillGeneration.ToString(),
                        //    _pageUrl,
                        //    student.Roll);
                        //#endregion
                        InsertShort_SupplementaryBillHistory(student, billHistoryMasterId, sessionId, studentBillDetailList, userId);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private static void InsertShort_SupplementaryBillHistory(Student student, int billHistoryMasterId, int sessionId, List<StudentBillDetailsDTO> studentBillDetailList, int userId)
        {
            try
            {
                int noOfFees = studentBillDetailList.Count;
                int billInsertCounter = 0;

                foreach (StudentBillDetailsDTO studentBillDetails in studentBillDetailList)
                {
                    decimal feeAmount = studentBillDetails.Amount;
                    if (feeAmount > 0)
                    {
                        BillHistory billHistroyObj = new BillHistory();
                        billHistroyObj.StudentId = studentBillDetails.StudentID;
                        billHistroyObj.StudentCourseHistoryId = studentBillDetails.StudentCourseHistoryId;
                        billHistroyObj.FeeSetupId = studentBillDetails.FeeSetupId;
                        billHistroyObj.TypeDefinitionId = studentBillDetails.TypeDefinitionID;
                        billHistroyObj.AcaCalId = sessionId;
                        billHistroyObj.Fees = feeAmount;
                        billHistroyObj.BillingDate = DateTime.Now;
                        billHistroyObj.IsDeleted = false;
                        billHistroyObj.BillHistoryMasterId = Convert.ToInt32(billHistoryMasterId);
                        billHistroyObj.CreatedBy = userId;
                        billHistroyObj.CreatedDate = DateTime.Now;
                        billHistroyObj.ModifiedBy = userId;
                        billHistroyObj.ModifiedDate = DateTime.Now;

                        int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                        if (billInsertId > 0)
                        {
                            //lblMessage.Text = "Bill inserted successfully.";
                            billInsertCounter = billInsertCounter + 1;
                            if (feeAmount > 0)
                            {
                                //#region Log Insert
                                //LogGeneralManager.Insert(
                                //     DateTime.Now,
                                //     BaseAcaCalCurrent.Code,
                                //     BaseAcaCalCurrent.FullCode,
                                //     BaseCurrentUserObj.LogInID,
                                //     "",
                                //     "",
                                //     "Bill History for Short Supplementary Entry",
                                //     BaseCurrentUserObj.LogInID + " posted bill for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + feeAmount.ToString() + ", Session : " + sessionDropDownList.SelectedItem.Text,
                                //     "normal",
                                //     ((int)CommonEnum.PageName.BillGeneration).ToString(),
                                //     CommonEnum.PageName.BillGeneration.ToString(),
                                //     _pageUrl,
                                //     student.Roll);
                                //#endregion
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        public static int Insert(BillHistoryMaster billhistorymaster)
        {
            int id = RepositoryManager.BillHistoryMaster_Repository.Insert(billhistorymaster);
            InvalidateCache();
            return id;
        }

        public static bool Update(BillHistoryMaster billhistorymaster)
        {
            bool isExecute = RepositoryManager.BillHistoryMaster_Repository.Update(billhistorymaster);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id, int tag)
        {
            bool isExecute = RepositoryManager.BillHistoryMaster_Repository.Delete(id, tag);
            InvalidateCache();
            return isExecute;
        }

        public static BillHistoryMaster GetById(int? id)
        {
            string rawKey = "BillHistoryMasterByID" + id;
            BillHistoryMaster billhistorymaster = GetCacheItem(rawKey);

            if (billhistorymaster == null)
            {
                billhistorymaster = RepositoryManager.BillHistoryMaster_Repository.GetById(id);
                if (billhistorymaster != null)
                    AddCacheItem(rawKey, billhistorymaster);
            }

            return billhistorymaster;
        }

        public static List<BillHistoryMaster> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "BillHistoryMasterGetAll";

            List<BillHistoryMaster> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.BillHistoryMaster_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static string GetBillMasterMaxReferenceNo(DateTime date)
        {
            string referenceNo = RepositoryManager.BillHistoryMaster_Repository.GetBillMasterMaxReferenceNo(date);
            return referenceNo;
        }

        public static bool IsDuplicateBill(int studentId, int acaCalId, decimal fees, bool isDue)
        {
            BillHistoryMaster billHistoryMasterObj = GetByStudentIdAcaCalIdFees(studentId, acaCalId, fees, isDue);
            if (billHistoryMasterObj == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static BillHistoryMaster GetByStudentIdAcaCalIdFees(int studentId, int acaCalId, decimal fees, bool isDue)
        {
            BillHistoryMaster billhistoryMaster = RepositoryManager.BillHistoryMaster_Repository.GetByStudentIdAcaCalIdFees(studentId, acaCalId, fees, isDue);
            return billhistoryMaster;
        }

        public static int CheckBillMasterDueBillCount(int studentId, int acaCalId)
        {
            List<BillHistoryMaster> billHistoryMasterList = new List<BillHistoryMaster>();
            billHistoryMasterList = BillHistoryMasterManager.GetBillDueCountByStudentIdAcaCalId(studentId, acaCalId);
            int count = 0;
            if (billHistoryMasterList != null)
            {
                count = billHistoryMasterList.Count;
            }
            return count;
        }

        public static List<BillHistoryMaster> GetBillDueCountByStudentIdAcaCalId(int studentId, int acaCalId)
        {
            List<BillHistoryMaster> billhistoryMaster = RepositoryManager.BillHistoryMaster_Repository.GetBillDueCountByStudentIdAcaCalId(studentId, acaCalId);
            return billhistoryMaster;
        }

        public static BillHistoryMaster GetByReferenceId(int studentId, string referenceNo)
        {
            BillHistoryMaster billhistoryMaster = RepositoryManager.BillHistoryMaster_Repository.GetByReferenceId(studentId, referenceNo);
            return billhistoryMaster;
        }

        //public static BillHistoryOrder GetOrderIdByBillHistoryIdList(string BillHistoryIdList, int UserId)
        //{
        //    BillHistoryOrder billHistoryOrder = RepositoryManager.BillHistoryMaster_Repository.GetOrderIdByBillHistoryIdList(BillHistoryIdList, UserId);
        //    return billHistoryOrder;
        //}

        //public static BillHistoryOrder InsertCollectionHistoryFromOnlinePayment(string refNo, int billHistoryMasterId, decimal amount, string paymentType)
        //{
        //    BillHistoryOrder billHistoryOrder = RepositoryManager.BillHistoryMaster_Repository.InsertCollectionHistoryFromOnlinePayment(refNo, billHistoryMasterId, amount, paymentType);
        //    return billHistoryOrder;
        //}

        public static List<BillDeleteDTO> GetStudentsForBillPrintByProgramIdSessionIdStudentAdmissionSessionId(int programId, int sessionId, int? admissionSessionId)
        {
            return RepositoryManager.BillHistoryMaster_Repository.GetStudentsForBillPrintByProgramIdSessionIdStudentAdmissionSessionId(programId, sessionId, admissionSessionId);
        }

    }
}

