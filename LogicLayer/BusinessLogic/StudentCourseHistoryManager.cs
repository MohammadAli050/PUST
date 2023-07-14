using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class StudentCourseHistoryManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "StudentCourseHistoryCache" };
        const double CacheDuration = 1.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<StudentCourseHistory> GetCacheAsList(string rawKey)
        {
            List<StudentCourseHistory> list = (List<StudentCourseHistory>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static StudentCourseHistory GetCacheItem(string rawKey)
        {
            StudentCourseHistory item = (StudentCourseHistory)HttpRuntime.Cache[GetCacheKey(rawKey)];
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

        public static int Insert(StudentCourseHistory studentCourseHistory)
        {
            int id = RepositoryManager.StudentCourseHistory_Repository.Insert(studentCourseHistory);
            InvalidateCache();
            return id;
        }

        public static bool Update(StudentCourseHistory studentCourseHistory)
        {
            bool isExecute = RepositoryManager.StudentCourseHistory_Repository.Update(studentCourseHistory);
            InvalidateCache();
            return isExecute;
        }

        public static bool UpdateLevelByStudentIdAcaCalId(int StudentId, int AcaCalId, int LevelId)
        {
            bool isExecute = RepositoryManager.StudentCourseHistory_Repository.UpdateLevelByStudentIdAcaCalId(StudentId, AcaCalId, LevelId);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.StudentCourseHistory_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }
        public static bool DeleteByExamIdCourseIdStudentId(int examId, int courseId, int studentId)
        {
            bool isExecute = RepositoryManager.StudentCourseHistory_Repository.DeleteByExamIdCourseIdStudentId(examId, courseId, studentId);
            return isExecute;
        }

        public static StudentCourseHistory GetById(int id)
        {
            // return RepositoryAdmission.Program_Repository.GetById(id);

            string rawKey = "StudentCourseHistoryById" + id;
            StudentCourseHistory studentCourseHistory = GetCacheItem(rawKey);

            if (studentCourseHistory == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                studentCourseHistory = RepositoryManager.StudentCourseHistory_Repository.GetById(id);
                if (studentCourseHistory != null)
                    AddCacheItem(rawKey, studentCourseHistory);
            }

            return studentCourseHistory;
        }

        public static List<StudentCourseHistory> GetAll()
        {
            const string rawKey = "StudentCourseHistoryGetAll";

            List<StudentCourseHistory> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                list = RepositoryManager.StudentCourseHistory_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<StudentCourseHistory> GetAllByAcaCalSectionId(int id)
        {
            List<StudentCourseHistory> list = list = RepositoryManager.StudentCourseHistory_Repository.GetAllByAcaCalSectionId(id);

            return list;
        }

        public static List<StudentCourseHistory> GetAllByStudentId(int StudentID)
        {
            // string rawKey = "StudentCourseHistoryGetAllByStudentId" + StudentID;

            List<StudentCourseHistory> list = RepositoryManager.StudentCourseHistory_Repository.GetAllByStudentId(StudentID);

            //if (list == null)
            //{
            //    list = RepositoryManager.StudentCourseHistory_Repository.GetAllByStudentId(StudentID);
            //    if (list != null)
            //        AddCacheItem(rawKey, list);
            //}

            return list;
        }

        public static List<StudentCourseHistory> GetAllMultiSpanCourseByStudentID(int studentID)
        {
            string rawKey = "StudentCourseHistoryGetAllMultiSpanCourseByStudentID" + studentID;

            List<StudentCourseHistory> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                list = GetAllByStudentId(studentID);

                if (list != null)
                {
                    list = list.Where(l => l.IsMultipleACUSpan == true).ToList();

                    if (list != null && list.Count > 0)
                        AddCacheItem(rawKey, list);
                }
            }

            return list;
        }

        public static List<StudentCourseHistory> GetAllByStudentIdAcaCalId(int studentID, int acaCalId)
        {
            //string rawKey = "StudentCourseHistoryGetAllByStudentIdAcaCalId" + studentID + acaCalId;

            //List<StudentCourseHistory> list = GetCacheAsList(rawKey);

            //if (list == null)
            //{
            //    list = RepositoryManager.StudentCourseHistory_Repository.GetAllByStudentIdAcaCalId(studentID, acaCalId);
            //    if (list != null)
            //        AddCacheItem(rawKey, list);
            //}

            //return list;
            List<StudentCourseHistory> list = RepositoryManager.StudentCourseHistory_Repository.GetAllByStudentIdAcaCalId(studentID, acaCalId);
            return list;
        }

        public static List<StudentCourseHistory> GetBy(int AcaCalId, int StudentId)
        {
            List<StudentCourseHistory> list = RepositoryManager.StudentCourseHistory_Repository.GetByStudentIdAcaCalId(AcaCalId, StudentId);
            return list;
        }

        public static decimal GetCompletedCreditByRoll(string Roll)
        {
            decimal completedCredit = RepositoryManager.StudentCourseHistory_Repository.GetCompletedCredit(Roll);
            return completedCredit;
        }

        public static decimal GetAttemptedCreditByRoll(string Roll)
        {
            decimal attemptedCredit = RepositoryManager.StudentCourseHistory_Repository.GetAttemptedCredit(Roll);
            return attemptedCredit;
        }

        public static List<StudentCourseHistoryDTO> GetAllByAcaCalIdCourseId(int acaCalId, int CourseId)
        {
            List<StudentCourseHistoryDTO> list = RepositoryManager.StudentCourseHistory_Repository.GetAllByAcaCalIdCourseId(acaCalId, CourseId);
            return list;
        }

        public static List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchCourse(int programId, int sessionId, int batchId, int courseId, int versionId)
        {
            List<StudentCourseHistory> list = RepositoryManager.StudentCourseHistory_Repository.GetAllRegisteredStudentByProgramSessionBatchCourse(programId, sessionId, batchId, courseId, versionId);
            return list;
        }

        //public static List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRoll(int programId, int sessionId, int batchId, string Roll)
        //{
        //    List<StudentAdmitCardInfo> list = RepositoryManager.StudentCourseHistory_Repository.GetAllStudentAdmitCardInfoByProgramBatchSessionRoll(programId, sessionId, batchId, Roll);
        //    return list;
        //}

        //public static List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRollV2(int programId, int sessionId, int batchId, string Roll)
        //{
        //    List<StudentAdmitCardInfo> list = RepositoryManager.StudentCourseHistory_Repository.GetAllStudentAdmitCardInfoByProgramBatchSessionRollV2(programId, sessionId, batchId, Roll);
        //    return list;
        //}

        //public static List<rResultCheck> GetStudentResultHisroyForResultCheck(int programId, int sessionId, int batchId, int year)
        //{
        //    List<rResultCheck> list = RepositoryManager.StudentCourseHistory_Repository.GetStudentResultHisroyForResultCheck(programId, sessionId, batchId, year);
        //    return list;
        //}

        public static bool IsDiscollegiateByStudentCourseHistoryId(int CourseHistoryId)
        {
            bool IsDiscollegiate = RepositoryManager.StudentCourseHistory_Repository.IsDiscollegiateByStudentCourseHistoryId(CourseHistoryId);
            return IsDiscollegiate;
        }

        public static bool IsAbsentInExamByStudentCourseHistoryId(int CourseHistoryId)
        {
            bool IsDiscollegiate = RepositoryManager.StudentCourseHistory_Repository.IsAbsentInExamByStudentCourseHistoryId(CourseHistoryId);
            return IsDiscollegiate;
        }

        public static List<StudentCourseHistory> GetAllByAcaCalId(int acaCalId)
        {
            return RepositoryManager.StudentCourseHistory_Repository.GetAllByAcaCalId(acaCalId);

        }

        public static List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchId(int programId, int sessionId, int batchId)
        {
            return RepositoryManager.StudentCourseHistory_Repository.GetAllRegisteredStudentByProgramSessionBatchId(programId, sessionId, batchId);
        }

        //public static List<rStudentCourse> GetAllCourseHistoryByCampusProgramSessionBatchCreditGroupDateRegStatus(int campusId, int programId, int sessionId, int batchId, int creditGroup, DateTime fromDate, DateTime toDate, int ResStatus, int dateFilter)
        //{
        //    List<rStudentCourse> list = RepositoryManager.StudentCourseHistory_Repository.GetAllCourseHistoryByCampusProgramSessionBatchCreditGroupDateRegStatus(campusId, programId, sessionId, batchId, creditGroup, fromDate, toDate, ResStatus, dateFilter);
        //    return list;
        //}

        public static List<StudentCourseHistoryNewDTO> GetAllCourseHistoryDetailByStudentId(int studentId)
        {
            return RepositoryManager.StudentCourseHistory_Repository.GetAllCourseHistoryDetailByStudentId(studentId);
        }

        public static List<StudentCourseHistory> GetAllByStudentIdYearNoSemesterNoExamId(int studentID, int yearNo, int semesterNo, int examId)
        {
            List<StudentCourseHistory> list = RepositoryManager.StudentCourseHistory_Repository.GetAllByStudentIdYearNoSemesterNoExamId(studentID, yearNo, semesterNo, examId);
            return list;
        }

        public static bool DeleteCourseByProgramYearSemesterExamId(int programId, int yearId, int semesterId, int yearNo, int semesterNo, int examId, int courseId, int versionId) 
        {
            bool isExecute = RepositoryManager.StudentCourseHistory_Repository.DeleteCourseByProgramYearSemesterExamId(programId, yearId, semesterId, yearNo, semesterNo, examId, courseId, versionId);
            return isExecute;
        }

        public static List<StudentCourseHistory> GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(int ExamSetupDetailId, int acaCalSectionId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId)
        {
            List<StudentCourseHistory> list = list = RepositoryManager.StudentCourseHistory_Repository.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(ExamSetupDetailId, acaCalSectionId, FirstExaminerId, SecondExaminerId, ThirdExaminerId);

            return list;
        }
    
    }
}
