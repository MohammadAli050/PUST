using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.BusinessLogic
{
    public class ExamMarkDetailsManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamMarkDetailsCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamMarkDetails> GetCacheAsList(string rawKey)
        {
            List<ExamMarkDetails> list = (List<ExamMarkDetails>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamMarkDetails GetCacheItem(string rawKey)
        {
            ExamMarkDetails item = (ExamMarkDetails)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamMarkDetails exammarkdetails)
        {
            int id = RepositoryManager.ExamMarkDetails_Repository.Insert(exammarkdetails);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamMarkDetails exammarkdetails)
        {
            bool isExecute = RepositoryManager.ExamMarkDetails_Repository.Update(exammarkdetails);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMarkDetails_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamMarkDetails GetById(int? id)
        {
            ExamMarkDetails exammarkdetails =  RepositoryManager.ExamMarkDetails_Repository.GetById(id);
            return exammarkdetails;
        }

        public static List<ExamMarkDetails> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "ExamMarkDetailsGetAll";

            List<ExamMarkDetails> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.ExamMarkDetails_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<ExamMarkDTO> GetByExamMarkDtoByParameter(int programId, int yearNo, int semesterNo, int courseId, int versionId, int acaCalSectionId, int examTemplateItemId)
        {
            List<ExamMarkDTO> list = RepositoryManager.ExamMarkDetails_Repository.GetByExamMarkDtoByParameter(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
            return list;
        }

        public static ExamMarkDetails GetByCourseHistoryIdExamTemplateItemId(int studentCourseHistoryId, int examTemplateItemId)
        {
            ExamMarkDetails exammarkdetails = RepositoryManager.ExamMarkDetails_Repository.GetByCourseHistoryIdExamTemplateItemId(studentCourseHistoryId, examTemplateItemId);
            return exammarkdetails;
        }


        public static bool InsertEditExamMarkDetails(int studentCourseHistoryId, bool isAbsent, int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, decimal studentExaMark, int userId)
        {
            ExamMarkDetails examMarkDetailsObj = ExamMarkDetailsManager.GetByCourseHistoryIdExamTemplateItemId(studentCourseHistoryId, examTemplateItemId);
            //decimal mark = GetStudentMark(studentCourseHistoryId);
            if (examMarkDetailsObj == null)
            {
                //ExamMarkMaster examMarkMaserObj = ExamMarkMasterManager.GetById(examMarkMasterId);

                ExamMarkDetails examMarkDetails = new ExamMarkDetails();
                examMarkDetails.ExamMarkMasterId = examMarkMasterId;
                examMarkDetails.CourseHistoryId = studentCourseHistoryId;
                if (!isAbsent)
                {
                    examMarkDetails.Marks = Convert.ToDecimal(studentExaMark);
                    examMarkDetails.ConvertedMark = Convert.ToDecimal(studentExaMark);
                    examMarkDetails.ExamStatus = 1;
                }
                else
                {
                    examMarkDetails.ExamStatus = 2;
                    examMarkDetails.Marks = 0;
                    examMarkDetails.ConvertedMark = 0;
                }
                examMarkDetails.IsFinalSubmit = false;
                examMarkDetails.ExamMarkTypeId = 0;
                examMarkDetails.ExamTemplateItemId = examTemplateItemId;
                examMarkDetails.CreatedBy = userId;
                examMarkDetails.CreatedDate = DateTime.Now;
                examMarkDetails.ModifiedBy = userId;
                examMarkDetails.ModifiedDate = DateTime.Now;

                int result = ExamMarkDetailsManager.Insert(examMarkDetails);
                if (result > 0)
                {
                    //lblMsg.Text = "Student marks inserted successfully.";
                    return true;
                }
                else
                {
                    //lblMsg.Text = "Student marks could not inserted.";
                    return false;
                }
            }
            else
            {
                examMarkDetailsObj.ExamMarkMasterId = examMarkMasterId;
                if (!isAbsent)
                {
                    examMarkDetailsObj.Marks = Convert.ToDecimal(studentExaMark);
                    examMarkDetailsObj.ConvertedMark = Convert.ToDecimal(studentExaMark);
                    examMarkDetailsObj.ExamStatus = 1;
                }
                else
                {
                    examMarkDetailsObj.ExamStatus = 2;
                    examMarkDetailsObj.Marks = 0;
                    examMarkDetailsObj.ConvertedMark = 0;
                }
                examMarkDetailsObj.ExamTemplateItemId = examTemplateItemId;
                examMarkDetailsObj.ExamMarkTypeId = 0;
                examMarkDetailsObj.IsFinalSubmit = false;
                examMarkDetailsObj.ExamMarkTypeId = 0;
                examMarkDetailsObj.ModifiedBy = userId;
                examMarkDetailsObj.ModifiedDate = DateTime.Now;
                bool result = ExamMarkDetailsManager.Update(examMarkDetailsObj);
                if (result)
                {
                    //lblMsg.Text = "Student marks edited successfully.";
                    return true;
                }
                else
                {
                    //lblMsg.Text = "Student marks could not edited.";
                    return false;
                }
            }
        }


        public static List<ExamMarkDetails> GetAllByCourseHistoryIdColumnTypeIdMultipleExaminerIdExamStatusId(int CourseHistoryId, int? ColumnTypeId, int? MultipleExaminerId, int? ExamStatusId)
        {
            List<ExamMarkDetails> list = RepositoryManager.ExamMarkDetails_Repository.GetAllByCourseHistoryIdColumnTypeIdMultipleExaminerIdExamStatusId(CourseHistoryId, ColumnTypeId, MultipleExaminerId, ExamStatusId);
                
            return list;
        }
    
    
    }
}

