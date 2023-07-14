using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.BusinessLogic
{
    public class ExamMarkMasterManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamMarkMasterCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamMarkMaster> GetCacheAsList(string rawKey)
        {
            List<ExamMarkMaster> list = (List<ExamMarkMaster>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamMarkMaster GetCacheItem(string rawKey)
        {
            ExamMarkMaster item = (ExamMarkMaster)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamMarkMaster exammarkmaster)
        {
            int id = RepositoryManager.ExamMarkMaster_Repository.Insert(exammarkmaster);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamMarkMaster exammarkmaster)
        {
            bool isExecute = RepositoryManager.ExamMarkMaster_Repository.Update(exammarkmaster);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMarkMaster_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamMarkMaster GetById(int? id)
        {
            string rawKey = "ExamMarkMasterByID" + id;
            ExamMarkMaster exammarkmaster = GetCacheItem(rawKey);

            if (exammarkmaster == null)
            {
                exammarkmaster = RepositoryManager.ExamMarkMaster_Repository.GetById(id);
                if (exammarkmaster != null)
                    AddCacheItem(rawKey,exammarkmaster);
            }

            return exammarkmaster;
        }

        public static List<ExamMarkMaster> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "ExamMarkMasterGetAll";

            List<ExamMarkMaster> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.ExamMarkMaster_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static ExamMarkMaster GetByAcaCalSectionIdExamTemplateItemId(int acaCalsectionId, int examTemplateItemId) 
        {
            ExamMarkMaster exammarkmaster = RepositoryManager.ExamMarkMaster_Repository.GetByAcaCalSectionIdExamTemplateItemId(acaCalsectionId, examTemplateItemId);
            return exammarkmaster;
        }

        public static bool FinalSubmitByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit) 
        {
            bool isExecute = RepositoryManager.ExamMarkMaster_Repository.FinalSubmitByAcacalSectionId(acaCalSectionId, isFinalSubmit);
            return isExecute;
        }

        public static bool ExamMarkBackToTeacherByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit)
        {
            bool isExecute = RepositoryManager.ExamMarkMaster_Repository.ExamMarkBackToTeacherByAcacalSectionId(acaCalSectionId, isFinalSubmit);
            return isExecute;
        }

        public static List<TempStudentExamMarkColumnWise> GetContinuousMarkColumnWise(int acaCalSectionId)
        {
            List<TempStudentExamMarkColumnWise> examMarkColumnWiseList = RepositoryManager.ExamMarkMaster_Repository.GetContinuousMarkColumnWise(acaCalSectionId);
            return examMarkColumnWiseList;
        }

        public static List<TempStudentExamMarkColumnWise> GetAllMarkColumnWise(int acaCalSectionId)
        {
            List<TempStudentExamMarkColumnWise> examMarkColumnWiseList = RepositoryManager.ExamMarkMaster_Repository.GetAllMarkColumnWise(acaCalSectionId);
            return examMarkColumnWiseList;
        }

        public static List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboard(int programId, int yearNo, int semesterNo, int examId)
        {
            List<ExamCommitteeDashboardDTO> examCommitteeDashboard = RepositoryManager.ExamMarkMaster_Repository.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
            return examCommitteeDashboard;
        }

        public static CountinousMarkTeacherInfoAPI_DTO GetTeacherInfoAPIBySectionId(int sectionId)
        {
            CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = RepositoryManager.ExamMarkMaster_Repository.GetTeacherInfoAPIBySectionId(sectionId);
            return countinousMarkTeacherInfoObj;
        }

        public static int InsertExamMarkMaster(ExamTemplateItem examTemplateItemObj, int acaCalSection, int userId)
        {
            int examMarkMasterId = 0;
            try
            {
                ExamMarkMaster examMarkMasterNewObj = ExamMarkMasterManager.GetByAcaCalSectionIdExamTemplateItemId(acaCalSection, examTemplateItemObj.ExamTemplateItemId);
                if (examMarkMasterNewObj == null)
                {
                    if (examTemplateItemObj != null && acaCalSection > 0)
                    {
                        ExamMarkMaster examMarkMasterObj = new ExamMarkMaster();
                        examMarkMasterObj.ExamMarkEntryDate = DateTime.Now;
                        examMarkMasterObj.ExamMark = examTemplateItemObj.ExamMark;
                        examMarkMasterObj.IsFinalSubmit = false;
                        examMarkMasterObj.ExamTemplateItemId = examTemplateItemObj.ExamTemplateItemId;
                        examMarkMasterObj.AcaCalSectionId = acaCalSection;
                        examMarkMasterObj.CreatedBy = userId;
                        examMarkMasterObj.CreatedDate = DateTime.Now;
                        examMarkMasterObj.ModifiedBy = userId;
                        examMarkMasterObj.ModifiedDate = DateTime.Now;


                        int result = ExamMarkMasterManager.Insert(examMarkMasterObj);
                        if (result > 0)
                        {
                            examMarkMasterId = result;
                        }
                    }
                }
                else
                {
                    examMarkMasterId = examMarkMasterNewObj.ExamMarkMasterId;
                }
                return examMarkMasterId;
            }
            catch (Exception ex)
            {
                return examMarkMasterId;
            }
        }

        public static List<RTabulationData> GetTabulationDataByProgramYearSemesterExam(int programId, int yearNo, int semesterNo, int examId)
        {
            List<RTabulationData> TabulationData = RepositoryManager.ExamMarkMaster_Repository.GetTabulationDataByProgramYearSemesterExam(programId, yearNo, semesterNo, examId);
            return TabulationData;
        }

        public static ExamCommitteeDashboardDTO GetExamCommitteeDashboardExtendByAcaCalSecId(int acaCalSecId)
        {
            ExamCommitteeDashboardDTO examCommitteeDashboard = RepositoryManager.ExamMarkMaster_Repository.GetExamCommitteeDashboardExtendByAcaCalSecId(acaCalSecId);
            return examCommitteeDashboard;
        }

        public static List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(int acaCalSecId)
        {
            List<ExamCommitteeDashboardDTO> examCommitteeDashboard = RepositoryManager.ExamMarkMaster_Repository.GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(acaCalSecId);
            return examCommitteeDashboard;
        }
        
        public static ExamCommitteeDashboardDTO GetExaminerByAcaCalSecId(int acaCalSecId)
        {
            ExamCommitteeDashboardDTO examCommitteeDashboard = RepositoryManager.ExamMarkMaster_Repository.GetExaminerByAcaCalSecId(acaCalSecId);
            return examCommitteeDashboard;
        }


        public static bool AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId( int sectionId)
        {
            bool isExecute = RepositoryManager.ExamMarkMaster_Repository.AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId( sectionId);
            

            return isExecute;
        }
    }
}

