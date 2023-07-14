using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace LogicLayer.BusinessLogic
{
    public class ExamMarkManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamMarkCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamMark> GetCacheAsList(string rawKey)
        {
            List<ExamMark> list = (List<ExamMark>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamMark GetCacheItem(string rawKey)
        {
            ExamMark item = (ExamMark)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamMark exammark)
        {
            int id = RepositoryManager.ExamMark_Repository.Insert(exammark);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamMark exammark)
        {
            bool isExecute = RepositoryManager.ExamMark_Repository.Update(exammark);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMark_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamMark GetById(int? id)
        {
            string rawKey = "ExamMarkByID" + id;
            ExamMark exammark = GetCacheItem(rawKey);

            if (exammark == null)
            {
                exammark = RepositoryManager.ExamMark_Repository.GetById(id);
                if (exammark != null)
                    AddCacheItem(rawKey, exammark);
            }

            return exammark;
        }

        public static List<ExamMark> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "ExamMarkGetAll";

            List<ExamMark> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.ExamMark_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<ExamMarkDTO> GetByExamMarkDtoByParameter(int programId, int yearNo, int semesterNo, int courseId, int versionId, int acaCalSectionId, int examTemplateItemId)
        {
            List<ExamMarkDTO> list = RepositoryManager.ExamMark_Repository.GetByExamMarkDtoByParameter(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
            return list;
        }

        public static ExamMark GetStudentMarkByExamId(int courseHistoryId, int examId)
        {
            ExamMark studentresult = RepositoryManager.ExamMark_Repository.GetStudentMarkByExamId(courseHistoryId, examId);
            return studentresult;
        }

        public static List<TempStudentExamMarkColumnWise> GetAllMarkColumnWise(int acaCalSectionId)
        {
            List<TempStudentExamMarkColumnWise> examMarkColumnWiseList = RepositoryManager.ExamMark_Repository.GetAllMarkColumnWise(acaCalSectionId);
            return examMarkColumnWiseList;
        }


        public static bool InsertStudentCourseQuestionMarksAndInsertOrUpdateTotalMarks(List<StudentsCourseMarks> studentsCourseMarks)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    foreach (StudentsCourseMarks aStudentCourseMarks in studentsCourseMarks)
                    {
                        ExamMark studentresult = RepositoryManager.ExamMark_Repository.GetStudentMarkByExamId(aStudentCourseMarks.StudentCourseHistoryId, aStudentCourseMarks.ExamTemplateTypeId);

                        if (studentresult != null)
                        {
                            studentresult.Mark = aStudentCourseMarks.TotalObtainedMarks;
                            studentresult.ModifiedBy = aStudentCourseMarks.ModifiedBy;
                            studentresult.ModifiedDate = aStudentCourseMarks.ModifiedDate;
                            Update(studentresult);
                        }
                        else
                        {
                            ExamMark examMark = new ExamMark();
                            examMark.CreateBy = aStudentCourseMarks.CreateBy;
                            examMark.CreatedDate = aStudentCourseMarks.CreatedDate;
                            examMark.ModifiedBy = aStudentCourseMarks.ModifiedBy;
                            examMark.ModifiedDate = aStudentCourseMarks.ModifiedDate;
                            examMark.CourseHistoryId = aStudentCourseMarks.StudentCourseHistoryId;
                            examMark.ExamTemplateItemId = aStudentCourseMarks.ExamTemplateTypeId;
                            examMark.Mark = aStudentCourseMarks.TotalObtainedMarks;
                            Insert(examMark);

                            //AlertSuccess.InnerText = "Student marks inserted.";
                        }
                        //RepositoryManager.ExamMark_Repository.DeleteStudentCourseQuestionMarksMasterDetail(aStudentCourseMarks);
                        //int id = RepositoryManager.ExamMark_Repository.InsertStudentCourseQuestionMarksMaster(aStudentCourseMarks);
                        //foreach (Marks studentMark in aStudentCourseMarks.StudentMarks)
                        //{
                        //    studentMark.Id = id;
                        //    RepositoryManager.ExamMark_Repository.InsertStudentCourseQuestionMarksDetail(studentMark);
                        //}

                    }
                    transactionScope.Complete();
                }
                catch (Exception)
                {
                    transactionScope.Dispose();
                    throw;
                }
            }

            return true;
        }

        public static List<Marks> GetAllQuestionMarksByCourseHistoryIdAndExamTypeItemId(int
            studentCourseHistoryId, int examTypeItemId)
        {
            return RepositoryManager.ExamMark_Repository.GetAllQuestionMarksByCourseHistoryIdAndExamTypeItemId(studentCourseHistoryId,
                    examTypeItemId);

        }




        public static ExamMark GetByCourseHistoryId(int courseHistoryId)
        {
            ExamMark exammark = RepositoryManager.ExamMark_Repository.GetByCourseHistoryId(courseHistoryId);
            return exammark;
        }

    }
}

