using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
using System.Data;

namespace LogicLayer.BusinessLogic
{
    public class ExamTemplateMasterManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamTemplateMasterCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamTemplateMaster> GetCacheAsList(string rawKey)
        {
            List<ExamTemplateMaster> list = (List<ExamTemplateMaster>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamTemplateMaster GetCacheItem(string rawKey)
        {
            ExamTemplateMaster item = (ExamTemplateMaster)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamTemplateMaster examtemplatemaster)
        {
            int id = RepositoryManager.ExamTemplateMaster_Repository.Insert(examtemplatemaster);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamTemplateMaster examtemplatemaster)
        {
            bool isExecute = RepositoryManager.ExamTemplateMaster_Repository.Update(examtemplatemaster);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamTemplateMaster_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamTemplateMaster GetById(int? id)
        {
            string rawKey = "ExamTemplateMasterByID" + id;
            ExamTemplateMaster examtemplatemaster =  RepositoryManager.ExamTemplateMaster_Repository.GetById(id);
            return examtemplatemaster;
        }

        public static List<ExamTemplateMaster> GetAll()
        {
            List<ExamTemplateMaster> list = RepositoryManager.ExamTemplateMaster_Repository.GetAll();
            return list;
        }

        public static bool GetExamTemplateMasterByName(string examTemplateMasterName)
        {
            ExamTemplateMaster examtemplatemaster = RepositoryManager.ExamTemplateMaster_Repository.GetExamTemplateMasterByName(examTemplateMasterName);
            if (examtemplatemaster != null)
            {
                return false;
            }
            else { return true; }
        }

        public static List<ExamTemplateBasicCalculativeItemDTO> ExamTemplateItemGetByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamTemplateBasicCalculativeItemDTO> list = RepositoryManager.ExamTemplateMaster_Repository.ExamTemplateItemGetByAcaCalSectionId(acaCalSectionId);
            return list;
        }

        public static List<ExamTemplateBasicCalculativeItemDTO> InCoursesExamTemplateItemGetByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamTemplateBasicCalculativeItemDTO> list = RepositoryManager.ExamTemplateMaster_Repository.InCoursesExamTemplateItemGetByAcaCalSectionId(acaCalSectionId);
            return list;
        }

        public static List<ExamTemplateBasicCalculativeItemDTO> AllExamTemplateItemGetByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamTemplateBasicCalculativeItemDTO> list = RepositoryManager.ExamTemplateMaster_Repository.AllExamTemplateItemGetByAcaCalSectionId(acaCalSectionId);
            return list;
        }

        public static List<ExamTemplateBasicCalculativeItemDTO> FinalExamTemplateItemGetByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamTemplateBasicCalculativeItemDTO> list = RepositoryManager.ExamTemplateMaster_Repository.FinalExamTemplateItemGetByAcaCalSectionId(acaCalSectionId);
            return list;
        }

        public static List<ExamMarkColumnWiseDTO> GetStudentExamMarkColumnWise(int courseId, int versionId, int acaCalId, int acaCalSectionId)
        {
            List<ExamMarkColumnWiseDTO> list = RepositoryManager.ExamTemplateMaster_Repository.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCalSectionId);
            return list;
        }

        public static List<ExamMarkColumnWiseDTO> GetStudentExamMarkColumnWiseByStudentId(int courseId, int versionId, int acaCalId, int acaCalSectionId, int courseHistoryId)
        {
            List<ExamMarkColumnWiseDTO> list = RepositoryManager.ExamTemplateMaster_Repository.GetStudentExamMarkColumnWiseByStudentId(courseId, versionId, acaCalId, acaCalSectionId, courseHistoryId);
            return list;
        }

        public static List<ExamMarkNewDTO> GetFinalExamResultDTOForReport(int programId, int sessionId, int courseId, int versionId, int acaCalSectionId, int examTemplateBasicItemId)
        {
            List<ExamMarkNewDTO> examResultList = null; // ExamMarkDetailsManager.GetFinalExamResultDTOForReport(programId, sessionId, courseId, versionId, acaCalSectionId, examTemplateBasicItemId);

            return examResultList;
        }

        public static List<ExamMarkNewDTO> GetFinalExamResultDTOForReportBMA(int programId, int sessionId, int courseId, int versionId, int acaCalSectionId, int examTemplateBasicItemId)
        {
            List<ExamMarkNewDTO> examResultList = null; // ExamMarkDetailsManager.GetFinalExamResultDTOForReportBMA(programId, sessionId, courseId, versionId, acaCalSectionId, examTemplateBasicItemId);

            return examResultList;
        }

        #region Common method for Exam Result report related

        public static List<ExamResultDTO> GetExamResultDTO(int courseId, int versionId, int acaCalId, int acaCaSectionId)
        {
            Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            if (courseObj != null)
            {
                if (courseObj.TypeDefinitionID == 1)
                {
                    DataTable dt = Get_30_Percent_Theory_ExamResultDataTable(courseId, versionId, acaCalId, acaCaSectionId, 1);
                    examResultList = GetTheoryResultFromTable(dt);
                    //return examResultList;
                }
                else
                {
                    DataTable dt = Get_100_Percent_SessionalIndustialTraining_ExamResultForDataTable(courseId, versionId, acaCalId, acaCaSectionId);
                    examResultList = GetSessionalIndustrialResultFromTable(dt);
                    //return examResultList;
                }
            }
            else { }
            return examResultList;
        }

        public static List<ExamResultDTO> GetTheoryResultFromTable(DataTable dt)
        {
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    ExamResultDTO examResultObj = new ExamResultDTO();
                    examResultObj.PageBrakeOne = (i / 39) + 1;
                    examResultObj.PageBrakeTwo = (i / 20) + 1;
                    examResultObj.StudentName = dt.Rows[i].ItemArray[0].ToString();
                    examResultObj.Roll = dt.Rows[i].ItemArray[1].ToString();
                    examResultObj.RegType = dt.Rows[i].ItemArray[2].ToString();
                    //examResultObj.ExamRoll = dt.Rows[i].ItemArray[2].ToString();
                    examResultObj.ExamName = dt.Columns[j].ColumnName.ToString();
                    examResultObj.ColumnSequence = Convert.ToInt32(j - 1);
                    examResultObj.MarksOrGrade = dt.Rows[i].ItemArray[j].ToString();
                    examResultList.Add(examResultObj);

                }
            }
            return examResultList;
        }

        public static List<ExamResultDTO> GetSessionalIndustrialResultFromTable(DataTable dt)
        {
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    ExamResultDTO examResultObj = new ExamResultDTO();
                    examResultObj.PageBrakeOne = (i / 39) + 1;
                    examResultObj.PageBrakeTwo = (i / 20) + 1;
                    examResultObj.StudentName = dt.Rows[i].ItemArray[0].ToString();
                    examResultObj.Roll = dt.Rows[i].ItemArray[1].ToString();
                    examResultObj.ExamRoll = dt.Rows[i].ItemArray[2].ToString();
                    examResultObj.RegType = dt.Rows[i].ItemArray[3].ToString();
                    examResultObj.ExamName = dt.Columns[j].ColumnName.ToString();
                    examResultObj.ColumnSequence = Convert.ToInt32(j - 1);
                    examResultObj.MarksOrGrade = dt.Rows[i].ItemArray[j].ToString();
                    examResultList.Add(examResultObj);
                }
            }
            return examResultList;
        }
        
        #endregion #region Common method for Exam Result report related

        #region 100% mark with grade report Theory Incourse exam mark and Industrial training, Sessional, Thesis

        public static DataTable Get_30_Percent_Theory_ExamResultDataTable(int courseId, int versionId, int acaCalId, int acaCaSectionId, int ExamMetaTypeId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {
                AcademicCalenderSection acaCalSectionObj = AcademicCalenderSectionManager.GetById(acaCaSectionId);
                if (acaCalSectionObj != null)
                {
                    List<ExamTemplateBasicCalculativeItemDTO> examTemplateBasicCalculativeItemList = null;

                    if (ExamMetaTypeId == 5)
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.FinalExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }
                    else
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.InCoursesExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }

                    List<ExamMarkColumnWiseDTO> examMarkColumnWiseList = ExamTemplateMasterManager.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCaSectionId);

                    List<int> studentIdList = examMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();

                    List<int> examSequenceList = examTemplateBasicCalculativeItemList.Select(d => d.ColumnSequence).ToList();

                    ExamMetaType examMetaTypeTypeObj = ExamMetaTypeManager.GetAll().Where(d => d.ExamMetaTypeName == "Final Exam").FirstOrDefault();


                    table.Columns.Add("Student Name", typeof(string));
                    table.Columns.Add("Roll", typeof(string));
                    table.Columns.Add("Reg Type", typeof(string));
                    decimal final = 0;

                    #region For Column Sequence

                    //if (examTemplateBasicCalculativeItemList != null && examTemplateBasicCalculativeItemList.Count > 0)
                    //{
                    //    examTemplateBasicCalculativeItemList = examTemplateBasicCalculativeItemList.OrderBy(o => o.ColumnSequence).ToList();
                    //}

                    //if (examSequenceList != null && examSequenceList.Count > 0)
                    //{
                    //    examSequenceList.Sort();
                    //} 

                    #endregion

                    for (int j = 0; j < examTemplateBasicCalculativeItemList.Count; j++)
                    {
                        ExamTemplateBasicCalculativeItemDTO examBasicCalItemDto = examTemplateBasicCalculativeItemList[j];
                        
                        if (examTemplateBasicCalculativeItemList[j].CalculationType == 0)
                        {
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                        }
                        else
                        {
                            decimal ctbestmarkColumn = Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark)) - Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault());
                            if (acaCalId > 41 && examBasicCalItemDto.ExamMetaTypeId == 3)
                            {
                                ctbestmarkColumn = 20 * acaCalSectionObj.Course.Credits; //Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark));
                            }
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + ctbestmarkColumn.ToString("#") + ")", typeof(string));
                        }
                    }

                    //decimal headerColumnTotalMark = examTemplateBasicCalculativeItemList.Sum(d => d.ExamTemplateBasicItemMark);
                    //headerColumnTotalMark = headerColumnTotalMark - examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault();

                    decimal courseTotalMark = 100 * acaCalSectionObj.Course.Credits;
                    decimal incourseMark = 0;
                    if (acaCalId > 41)
                    {
                        incourseMark = (courseTotalMark / 100) * 40;
                    }
                    else 
                    {
                        incourseMark = (courseTotalMark / 100) * 30;
                    }



                    //decimal incourseMark = 100 - final;
                    //if (final == 0)
                    //    //table.Columns.Add("Total \n ", typeof(string));
                    //    table.Columns.Add("Total \n (" + headerColumnTotalMark.ToString("#") + ")", typeof(string));
                    //else
                    //    table.Columns.Add("Total (" + final.ToString("#") + "+" + incourseMark.ToString("#") + ")%", typeof(string));

                    if (final == 0)
                        //table.Columns.Add("Total \n ", typeof(string));
                        table.Columns.Add("Total \n (" + incourseMark.ToString("#") + ")", typeof(string));
                    else
                        table.Columns.Add("Total (" + final.ToString("#") + "+" + courseTotalMark.ToString("#") + ")%", typeof(string));


                    List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(1);

                    for (int i = 0; i < studentIdList.Count; i++)
                    {
                        int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                        int studentId = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                        ////List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                        DataRow newRow;
                        //if (studentObj != null)
                        //{
                        //    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(Convert.ToInt32(studentObj.GradeMasterId));
                        //}

                        string retakeNo = string.Empty;
                        StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        if (studentCourseHistory != null)
                        {
                            if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 9)
                            {
                                retakeNo = "R";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 10) 
                            {
                                retakeNo = "RT";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 11)
                            {
                                retakeNo = "SS";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 12)
                            {
                                retakeNo = "IM";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 13)
                            {
                                retakeNo = "SRE";
                            }
                        }


                        object[] rowArray = new object[examTemplateBasicCalculativeItemList.Count + 4];
                        int newRowCounter = 0;
                        rowArray[0] = studentObj.Name;
                        rowArray[1] = studentObj.Roll;
                        rowArray[2] = retakeNo;
                        newRowCounter = 2;
                        decimal totalMark = 0;
                        decimal inCourseExamMark = 0;
                        decimal finalWithoutConvertMark = 0;

                        for (int j = 0; j < examSequenceList.Count; j++)
                        {
                            decimal examMarks = 0;
                            int sequenceNo = Convert.ToInt32(examSequenceList[j]);
                            ExamTemplateBasicCalculativeItemDTO examItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                            if (examItemObj != null)
                            {
                                if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Basic) //(int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                                {
                                    decimal studentExamMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ConvertedMark).FirstOrDefault());
                                    examMarks = studentExamMark;
                                    examMarks = Math.Round(examMarks, 2);

                                    ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType != 0).FirstOrDefault();
                                    if (examCalculativeItemObj == null)
                                    {
                                        totalMark = totalMark + examMarks;
                                        if (examItemObj.ExamMetaTypeId != 8 || !examItemObj.ExamTemplateBasicItemName.Contains("Final"))
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                        }
                                        else
                                        {
                                            finalWithoutConvertMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                        }
                                    }
                                }
                                else if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Calculative)
                                {
                                    if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Average)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark));
                                        int itemCount = Convert.ToInt32(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 0).ToList().Count);
                                        examMarks = marks / itemCount;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 1).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Max(d => d.ConvertedMark));

                                        examMarks = marks;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        examMarks = (maxNumber + secondMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;
                                        
                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 40);
                                        }

                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;
                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 60);
                                        }

                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();
                                        var fourthMax = maxArrayObj.OrderByDescending(z => z).Skip(3).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax + fourthMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;
                                        
                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 80);
                                        }

                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Sum)
                                    {
                                        decimal mark = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark);

                                        examMarks = mark;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                }
                            }
                            rowArray[newRowCounter + 1] = examMarks;
                            newRowCounter = newRowCounter + 1;

                        }
                        totalMark = Math.Round(totalMark, 2); //Math.Ceiling(totalMark);
                        rowArray[newRowCounter + 1] = totalMark;
                        newRowCounter = newRowCounter + 1;

                        //decimal gradePoint = 0;
                        //string gradeLetter = "Grading System Not Assigned";

                        //if (gradeDetailList != null && gradeDetailList.Count > 0)
                        //{
                        //    gradePoint = gradeDetailList.Where(d => d.MinMarks <= totalMark && d.MaxMarks >= totalMark).FirstOrDefault().GradePoint;
                        //    gradeLetter = gradeDetailList.Where(d => d.MinMarks <= totalMark && d.MaxMarks >= totalMark).FirstOrDefault().Grade;
                        //    //gradeId = gradeDetailList.Where(d => d.MinMarks <= totalMark && d.MaxMarks >= totalMark).FirstOrDefault().GradeId;
                        //}
                        //if (examMetaTypeTypeObj != null)
                        //{
                        //    ExamMarkColumnWiseDTO examMarkColumnWiseObj = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examMetaTypeTypeObj.ExamMetaTypeId).FirstOrDefault();
                        //    if (examMarkColumnWiseObj != null)
                        //    {
                        //        if (examMarkColumnWiseObj.ExamMarkTypeId == 2)
                        //        {
                        //            gradePoint = Convert.ToDecimal(0.00);
                        //            gradeLetter = "Absent";
                        //        }
                        //    }
                        //}

                        //if(){}

                        //rowArray[newRowCounter + 1] = gradePoint;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = gradeLetter;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = studentObj.Roll;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = inCourseExamMark ;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = finalWithoutConvertMark;
                        //newRowCounter = newRowCounter + 1;


                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }

        

        public static List<ExamResultDTO> GetCourseGradeExamResultDTO(int courseId, int versionId, int acaCalId, int acaCaSectionId)
        {
            Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            if (courseObj != null)
            {
                if (courseObj.TypeDefinitionID == 1)
                {
                    DataTable dt = Get_100_Percent_Theory_CourseGradeExamResultDataTable(courseId, versionId, acaCalId, acaCaSectionId, 1);
                    //dt.Columns.Contains().Remove("");
                    examResultList = GetTheoryResultFromTable(dt);
                    //return examResultList;
                }
                else
                {
                    DataTable dt = Get_100_Percent_SessionalIndustialTraining_ExamResultForDataTable(courseId, versionId, acaCalId, acaCaSectionId);
                    examResultList = GetSessionalIndustrialResultFromTable(dt);
                    //return examResultList;
                }
            }
            else { }
            return examResultList;
        }

        private static DataTable Get_100_Percent_Theory_CourseGradeExamResultDataTable(int courseId, int versionId, int acaCalId, int acaCaSectionId, int ExamMetaTypeId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {
                AcademicCalenderSection acaCalSectionObj = AcademicCalenderSectionManager.GetById(acaCaSectionId);
                if (acaCalSectionObj != null)
                {
                    List<ExamTemplateBasicCalculativeItemDTO> examTemplateBasicCalculativeItemList = null;

                    if (ExamMetaTypeId == 5)
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.FinalExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }
                    else
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.AllExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }

                    List<ExamMarkColumnWiseDTO> examMarkColumnWiseList = ExamTemplateMasterManager.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCaSectionId);

                    List<int> studentIdList = examMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();

                    List<int> examSequenceList = examTemplateBasicCalculativeItemList.Select(d => d.ColumnSequence).ToList();

                    ExamMetaType examMetaTypeTypeObj = ExamMetaTypeManager.GetAll().Where(d => d.ExamMetaTypeName == "Final Exam").FirstOrDefault();
                    ExamTemplateMaster examTemplateMasterObj = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);

                    table.Columns.Add("Student Name", typeof(string));
                    table.Columns.Add("Roll", typeof(string));
                    table.Columns.Add("Reg Type", typeof(string));
                    decimal final = 0;

                    #region For Column Sequence

                    //if (examTemplateBasicCalculativeItemList != null && examTemplateBasicCalculativeItemList.Count > 0)
                    //{
                    //    examTemplateBasicCalculativeItemList = examTemplateBasicCalculativeItemList.OrderBy(o => o.ColumnSequence).ToList();
                    //}

                    //if (examSequenceList != null && examSequenceList.Count > 0)
                    //{
                    //    examSequenceList.Sort();
                    //} 

                    #endregion

                    for (int j = 0; j < examTemplateBasicCalculativeItemList.Count; j++)
                    {
                        if (examTemplateBasicCalculativeItemList[j].CalculationType == 0)
                        {
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                        }
                        else
                        {
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                        }
                    }

                    decimal courseTotalMark = 100 * acaCalSectionObj.Course.Credits;
                    decimal incourseMark = 0;
                    if (acaCalId > 41)
                    {
                        incourseMark = (courseTotalMark / 100) * 40;
                    }
                    else
                    {
                        incourseMark = (courseTotalMark / 100) * 30;
                    }

                    //decimal incourseMark = 100 - final;
                    if (final == 0)
                        //table.Columns.Add("Total \n ", typeof(string));
                        table.Columns.Add("Total \n (" + examTemplateMasterObj.ExamTemplateMasterTotalMark.ToString("#") + ")", typeof(string));
                    else
                        table.Columns.Add("Total (" + final.ToString("#") + "+" + incourseMark.ToString("#") + ")%", typeof(string));

                    table.Columns.Add("Mark in \n100%", typeof(string));
                    table.Columns.Add("Grade", typeof(string));
                    table.Columns.Add("Grade \nPoint", typeof(string));

                    //table.Columns.Add("Student Roll", typeof(string));
                    //table.Columns.Add("Incourse " + incourseMark.ToString("#") + "%", typeof(string));
                    //table.Columns.Add("Final Mark", typeof(string));

                    List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(1);

                    for (int i = 0; i < studentIdList.Count; i++)
                    {
                        int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                        bool isDiscollegiate = StudentCourseHistoryManager.IsDiscollegiateByStudentCourseHistoryId(studentCourseHistoryId);
                        bool isAbsent = StudentCourseHistoryManager.IsAbsentInExamByStudentCourseHistoryId(studentCourseHistoryId);
                        int studentId = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                        ////List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                        DataRow newRow;
                        //if (studentObj != null)
                        //{
                        //    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(Convert.ToInt32(studentObj.GradeMasterId));
                        //}

                        string retakeNo = string.Empty;
                        StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        if (studentCourseHistory != null)
                        {
                            if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 9)
                            {
                                retakeNo = "R";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 10)
                            {
                                retakeNo = "RT";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 11)
                            {
                                retakeNo = "SS";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 12)
                            {
                                retakeNo = "IM";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 13)
                            {
                                retakeNo = "SRE";
                            }
                        }

                        object[] rowArray = new object[examTemplateBasicCalculativeItemList.Count + 7];
                        int newRowCounter = 0;
                        rowArray[0] = studentObj.Name;
                        rowArray[1] = studentObj.Roll;
                        rowArray[2] = retakeNo;
                        newRowCounter = 2;
                        decimal totalMark = 0;
                        decimal inCourseExamMark = 0;
                        decimal finalWithoutConvertMark = 0;

                        for (int j = 0; j < examSequenceList.Count; j++)
                        {
                            decimal examMarks = 0;
                            string stringExamMark = null;
                            int sequenceNo = Convert.ToInt32(examSequenceList[j]);
                            ExamTemplateBasicCalculativeItemDTO examItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                            if (examItemObj != null)
                            {
                                if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Basic) //(int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                                {
                                    if (examItemObj.ExamMetaTypeId != 3)
                                    {

                                        decimal studentExamMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ConvertedMark).FirstOrDefault());
                                        examMarks = studentExamMark;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);
                                        if (examItemObj.ExamMetaTypeId == 5)
                                        {
                                            decimal scutinizerMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ScrutinizerMark).FirstOrDefault());
                                            if (scutinizerMark > 0)
                                            {
                                                stringExamMark = Convert.ToString(examMarks) + "(" + Convert.ToString(scutinizerMark) + ")";
                                                examMarks = scutinizerMark;
                                            }
                                            else
                                            {
                                                stringExamMark = Convert.ToString(examMarks);
                                            }
                                        }

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType != 0).FirstOrDefault();
                                        if (examCalculativeItemObj == null)
                                        {
                                            totalMark = totalMark + examMarks;
                                            if (examItemObj.ExamMetaTypeId != 8 || !examItemObj.ExamTemplateBasicItemName.Contains("Final"))
                                            {
                                                inCourseExamMark = inCourseExamMark + examMarks;
                                            }
                                            else
                                            {
                                                finalWithoutConvertMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                                else if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Calculative)
                                {
                                    if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Average)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark));
                                        int itemCount = Convert.ToInt32(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 0).ToList().Count);
                                        examMarks = marks / itemCount;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 1).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Max(d => d.ConvertedMark));

                                        examMarks = marks;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();

                                        //examMarks = (maxNumber + secondMax) / 2;
                                        examMarks = (maxNumber + secondMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 40);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 60);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();
                                        var fourthMax = maxArrayObj.OrderByDescending(z => z).Skip(3).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax + fourthMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 80);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Sum)
                                    {
                                        decimal mark = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark);

                                        examMarks = mark;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                }
                            }
                            rowArray[newRowCounter + 1] = stringExamMark;//examMarks;
                            newRowCounter = newRowCounter + 1;

                        }
                        totalMark = Math.Round(totalMark, 2); //Math.Ceiling(totalMark);
                        rowArray[newRowCounter + 1] = totalMark;
                        newRowCounter = newRowCounter + 1;

                        decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateMasterObj.ExamTemplateMasterTotalMark, 2), 2, MidpointRounding.AwayFromZero);
                        decimal markAddedTotalMark = 0;

                        markAddedTotalMark = convertedTotalMark + Convert.ToDecimal(0.99);
                        if (markAddedTotalMark > 100)
                        {
                            markAddedTotalMark = convertedTotalMark;
                        }

                        decimal markAddedGradePoint = 0;
                        decimal gradePoint = 0;
                        string gradeLetter = "Grading System Not Assigned";

                        //int retakeNo = 0;
                        //StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        //if (studentCourseHistory != null)
                        //{
                        //    retakeNo = Convert.ToInt32(studentCourseHistory.RetakeNo);
                        //}

                        if (gradeDetailList != null && gradeDetailList.Count > 0)
                        {

                            markAddedGradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                            gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                            if (gradePoint != markAddedGradePoint)
                            {
                                //convertedTotalMark = decimal.Round(markAddedTotalMark, 0, MidpointRounding.AwayFromZero);
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().Grade;
                                //gradeId = gradeDetailList.Where(d => d.MinMarks <= totalMark && d.MaxMarks >= totalMark).FirstOrDefault().GradeId;
                            }
                            else
                            {
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().Grade;
                            }
                        }
                        if (examMetaTypeTypeObj != null)
                        {
                            ExamMarkColumnWiseDTO examMarkColumnWiseObj = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examMetaTypeTypeObj.ExamMetaTypeId).FirstOrDefault();
                            if (examMarkColumnWiseObj != null)
                            {
                                if (examMarkColumnWiseObj.ExamMarkTypeId == 2)
                                {
                                    gradePoint = Convert.ToDecimal(0.00);
                                    gradeLetter = "Absent";
                                }
                            }
                        }

                        //if(){}

                        

                        if (isAbsent)
                        {
                            gradeLetter = "AB";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        if (isDiscollegiate)
                        {
                            gradeLetter = "DC";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        if ((studentCourseHistory.RetakeNo == 10 || studentCourseHistory.RetakeNo == 12) && gradePoint > Convert.ToDecimal("3.25"))
                        {
                            gradeLetter = "B+";
                            gradePoint = Convert.ToDecimal("3.25");
                        }

                        if (studentCourseHistory.CourseStatusID == 4)
                        {
                            gradeLetter = "E";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        rowArray[newRowCounter + 1] = convertedTotalMark;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradeLetter;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradePoint;
                        newRowCounter = newRowCounter + 1;

                        //rowArray[newRowCounter + 1] = studentObj.Roll;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = inCourseExamMark ;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = finalWithoutConvertMark;
                        //newRowCounter = newRowCounter + 1;


                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }

        private static DataTable Get_100_Percent_SessionalIndustialTraining_ExamResultForDataTable(int courseId, int versionId, int acaCalId, int acaCaSectionId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {
                AcademicCalenderSection acaCalSectionObj = AcademicCalenderSectionManager.GetById(acaCaSectionId);
                if (acaCalSectionObj != null)
                {
                    ExamTemplateMaster examtemplatemasterobject = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);
                    List<ExamTemplateBasicCalculativeItemDTO> examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.ExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();

                    List<ExamMarkColumnWiseDTO> examMarkColumnWiseList = ExamTemplateMasterManager.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCaSectionId);

                    List<int> studentIdList = examMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();

                    List<int> examSequenceList = examTemplateBasicCalculativeItemList.Select(d => d.ColumnSequence).ToList();

                    ExamMetaType examMetaTypeTypeObj = ExamMetaTypeManager.GetAll().Where(d => d.ExamMetaTypeName == "Final Exam").FirstOrDefault();
                    ExamTemplateMaster examTemplateMasterObj = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);

                    table.Columns.Add("Student Name", typeof(string));
                    table.Columns.Add("Roll", typeof(string));
                    table.Columns.Add("Exam Roll", typeof(string));
                    table.Columns.Add("Reg Type", typeof(string));
                    decimal final = 0;

                    #region For Column Sequence

                    //if (examTemplateBasicCalculativeItemList != null && examTemplateBasicCalculativeItemList.Count > 0)
                    //{
                    //    examTemplateBasicCalculativeItemList = examTemplateBasicCalculativeItemList.OrderBy(o => o.ColumnSequence).ToList();
                    //}

                    //if (examSequenceList != null && examSequenceList.Count > 0)
                    //{
                    //    examSequenceList.Sort();
                    //} 

                    #endregion

                    if (acaCalId <= 41)
                    {
                        for (int j = 0; j < examTemplateBasicCalculativeItemList.Count; j++)
                        {
                            ExamTemplateBasicCalculativeItemDTO examBasicCalItemDto = examTemplateBasicCalculativeItemList[j];

                            if (examTemplateBasicCalculativeItemList[j].CalculationType == 0)
                            {
                                table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                            }
                            else
                            {
                                decimal ctbestmarkColumn = Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark)) - Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault());
                                if (acaCalId > 41 && examBasicCalItemDto.ExamMetaTypeId == 3)
                                {
                                    ctbestmarkColumn = 20 * acaCalSectionObj.Course.Credits; //Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark));
                                }
                                //table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + ctbestmarkColumn.ToString("#") + ")", typeof(string));
                            }
                        }
                    }

                    decimal headerColumnTotalMark = examTemplateBasicCalculativeItemList.Sum(d => d.ExamTemplateBasicItemMark);
                    //headerColumnTotalMark = headerColumnTotalMark - examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault();
                    decimal courseTotalMark = 100 * acaCalSectionObj.Course.Credits;

                    //decimal incourseMark = 100 - final;
                    //if (final == 0)
                    //{
                    //    //table.Columns.Add("Total \n ", typeof(string));
                        
                    //}
                    //else
                    //{
                    //    table.Columns.Add("Total (" + final.ToString("#") + "+" + incourseMark.ToString("#") + ")%", typeof(string));
                    //}

                    if (acaCalId <= 41)
                    {
                        table.Columns.Add("Total \n (" + headerColumnTotalMark.ToString("#") + ")", typeof(string));
                    }
                    else if (acaCalId > 41)
                    {
                        table.Columns.Add("Mark in \n (" + courseTotalMark.ToString("#") + ")", typeof(string));
                    }
                    table.Columns.Add("Mark in\n 100%", typeof(string));
                    table.Columns.Add("Grade", typeof(string));
                    table.Columns.Add("Grade Point", typeof(string));

                    List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(1);

                    for (int i = 0; i < studentIdList.Count; i++)
                    {
                        int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                        bool isDiscollegiate = StudentCourseHistoryManager.IsDiscollegiateByStudentCourseHistoryId(studentCourseHistoryId);
                        bool isAbsent = StudentCourseHistoryManager.IsAbsentInExamByStudentCourseHistoryId(studentCourseHistoryId);
                        int studentId = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                        //StudentExamRoll studentExamRoll = StudentExamRollManager.GetByStudentIdAcaCalId(studentId, acaCalId);

                        DataRow newRow;
                        //if (studentObj != null)
                        //{

                        //}
                        string retakeNo = string.Empty;
                        StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        if (studentCourseHistory != null)
                        {
                            if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 9)
                            {
                                retakeNo = "R";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 10)
                            {
                                retakeNo = "RT";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 11)
                            {
                                retakeNo = "SS";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 12)
                            {
                                retakeNo = "IM";
                            }
                            else if (Convert.ToInt32(studentCourseHistory.RetakeNo) == 13)
                            {
                                retakeNo = "SRE";
                            }
                        }


                        object[] rowArray = null;
                        if (acaCalId <= 41)
                        { rowArray = new object[examTemplateBasicCalculativeItemList.Count + 8]; }
                        else if (acaCalId > 41)
                        { rowArray = new object[8]; }
                        int newRowCounter = 0;
                        rowArray[0] = studentObj.Name;
                        rowArray[1] = studentObj.Roll;
                        rowArray[2] = "";
                        //if (studentExamRoll != null)
                        //{
                        //    rowArray[2] = null; //studentExamRoll.ExamRoll;
                        //}
                        //else { rowArray[2] = ""; }
                        rowArray[3] = retakeNo;
                        newRowCounter = 3;
                        decimal totalMark = 0;
                        decimal inCourseExamMark = 0;
                        decimal finalWithoutConvertMark = 0;



                        for (int j = 0; j < examSequenceList.Count; j++)
                        {
                            decimal examMarks = 0;
                            int sequenceNo = Convert.ToInt32(examSequenceList[j]);
                            ExamTemplateBasicCalculativeItemDTO examItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                            if (examItemObj != null)
                            {
                                if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Basic) //(int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                                {
                                    decimal studentExamMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ConvertedMark).FirstOrDefault());
                                    examMarks = studentExamMark;
                                    examMarks = Math.Round(examMarks, 2);

                                    ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType != 0).FirstOrDefault();
                                    if (examCalculativeItemObj == null)
                                    {
                                        totalMark = totalMark + examMarks;
                                        if (examItemObj.ExamMetaTypeId != 8 || !examItemObj.ExamTemplateBasicItemName.Contains("Final"))
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                        }
                                        else
                                        {
                                            finalWithoutConvertMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                        }
                                    }
                                }
                                else if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Calculative)
                                {
                                    if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Average)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark));
                                        int itemCount = Convert.ToInt32(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 0).ToList().Count);
                                        examMarks = marks / itemCount;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 1).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Max(d => d.ConvertedMark));

                                        examMarks = marks;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();

                                        //examMarks = (maxNumber + secondMax) / 2;
                                        examMarks = (maxNumber + secondMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 40);
                                        }
                                        examMarks = Math.Round(examMarks, 2);


                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 60);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();
                                        var fourthMax = maxArrayObj.OrderByDescending(z => z).Skip(3).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax + fourthMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 80);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Sum)
                                    {
                                        decimal mark = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark);

                                        examMarks = mark;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                }
                            }
                            if (acaCalId <= 41)
                            { 
                                rowArray[newRowCounter + 1] = examMarks;
                                newRowCounter = newRowCounter + 1;
                            }
                        }
                        totalMark = Math.Round(totalMark, 2); //Math.Ceiling(totalMark);
                        rowArray[newRowCounter + 1] = totalMark;
                        newRowCounter = newRowCounter + 1;

                        decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateMasterObj.ExamTemplateMasterTotalMark, 2), 2, MidpointRounding.AwayFromZero);
                        decimal markAddedTotalMark = 0;

                        markAddedTotalMark = convertedTotalMark + Convert.ToDecimal(0.99);
                        if (markAddedTotalMark > 100)
                        {
                            markAddedTotalMark = convertedTotalMark;
                        }


                        decimal markAddedGradePoint = 0;
                        decimal gradePoint = 0;
                        string gradeLetter = "Grading System Not Assigned";

                        //int retakeNo = 0;
                        //StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        //if (studentCourseHistory != null)
                        //{
                        //    retakeNo = Convert.ToInt32(studentCourseHistory.RetakeNo);
                        //}

                        int gradeId = 0;
                        if (gradeDetailList != null && gradeDetailList.Count > 0)
                        {

                            markAddedGradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                            gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                            if (gradePoint != markAddedGradePoint)
                            {
                                //convertedTotalMark = decimal.Round(markAddedTotalMark, 0, MidpointRounding.AwayFromZero);
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradeId;
                            }
                            else
                            {
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradeId;
                            }
                        }
                        if (examMetaTypeTypeObj != null)
                        {
                            ExamMarkColumnWiseDTO examMarkColumnWiseObj = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examMetaTypeTypeObj.ExamMetaTypeId).FirstOrDefault();
                            if (examMarkColumnWiseObj != null)
                            {
                                if (examMarkColumnWiseObj.ExamMarkTypeId == 2)
                                {
                                    gradePoint = Convert.ToDecimal(0.00);
                                    gradeLetter = "Absent";
                                }
                            }
                        }

                        

                        if (isAbsent)
                        {
                            gradeLetter = "AB";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        if (isDiscollegiate)
                        {
                            gradeLetter = "DC";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        if ((studentCourseHistory.RetakeNo == 10 || studentCourseHistory.RetakeNo == 12) && gradePoint > Convert.ToDecimal("3.25"))
                        {
                            gradeLetter = "B+";
                            gradePoint = Convert.ToDecimal("3.25");
                        }

                        if (studentCourseHistory.CourseStatusID == 4)
                        {
                            gradeLetter = "E";
                            gradePoint = Convert.ToDecimal("0.00");
                        }

                        rowArray[newRowCounter + 1] = Math.Round(((totalMark * 100) / examtemplatemasterobject.ExamTemplateMasterTotalMark), 2);
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradeLetter;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradePoint;
                        newRowCounter = newRowCounter + 1;



                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }

        #endregion 100% mark with grade report

        #region publish exam mark

        public static List<ExamResultDTO> PublishExamMark(int courseId, int versionId, int acaCalId, int acaCaSectionId, int userId)
        {
            Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            if (courseObj != null)
            {
                if (courseObj.TypeDefinitionID == 1)
                {
                    DataTable dt = PublishExamMarkTheoryCourse(courseId, versionId, acaCalId, acaCaSectionId, 1, userId);
                    examResultList = GetTheoryResultFromTable(dt);
                    //return examResultList;
                }
                else
                {
                    DataTable dt = PublishExamMarkTheoryCourseSessionalIndustialTraining(courseId, versionId, acaCalId, acaCaSectionId, userId);
                    examResultList = GetSessionalIndustrialResultFromTable(dt);
                    //return examResultList;
                }
            }
            else { }
            return examResultList;
        }

        private static DataTable PublishExamMarkTheoryCourse(int courseId, int versionId, int acaCalId, int acaCaSectionId, int ExamMetaTypeId, int userId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {
                AcademicCalenderSection acaCalSectionObj = AcademicCalenderSectionManager.GetById(acaCaSectionId);
                if (acaCalSectionObj != null)
                {
                    List<ExamTemplateBasicCalculativeItemDTO> examTemplateBasicCalculativeItemList = null;

                    if (ExamMetaTypeId == 5)
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.FinalExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }
                    else
                    {
                        examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.AllExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();
                    }

                    List<ExamMarkColumnWiseDTO> examMarkColumnWiseList = ExamTemplateMasterManager.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCaSectionId);

                    List<int> studentIdList = examMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();

                    List<int> examSequenceList = examTemplateBasicCalculativeItemList.Select(d => d.ColumnSequence).ToList();

                    ExamMetaType examMetaTypeTypeObj = ExamMetaTypeManager.GetAll().Where(d => d.ExamMetaTypeName == "Final Exam").FirstOrDefault();
                    ExamTemplateMaster examTemplateMasterObj = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);

                    table.Columns.Add("Student Name", typeof(string));
                    table.Columns.Add("Roll", typeof(string));
                    decimal final = 0;

                    #region For Column Sequence

                    //if (examTemplateBasicCalculativeItemList != null && examTemplateBasicCalculativeItemList.Count > 0)
                    //{
                    //    examTemplateBasicCalculativeItemList = examTemplateBasicCalculativeItemList.OrderBy(o => o.ColumnSequence).ToList();
                    //}

                    //if (examSequenceList != null && examSequenceList.Count > 0)
                    //{
                    //    examSequenceList.Sort();
                    //} 

                    #endregion

                    for (int j = 0; j < examTemplateBasicCalculativeItemList.Count; j++)
                    {
                        ExamTemplateBasicCalculativeItemDTO examBasicCalItemDto = examTemplateBasicCalculativeItemList[j];
                        if (examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName == "Final")
                        {
                            ExamTemplateBasicItemDetails examTemplateDetails = ExamTemplateBasicItemDetailsManager.GetById(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemId);
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateDetails.ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                            final = examTemplateDetails.ExamTemplateBasicItemMark;
                        }
                        else
                        {
                            if (examTemplateBasicCalculativeItemList[j].CalculationType == 0)
                            {
                                table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                            }
                            else
                            {
                                decimal ctbestmarkColumn = Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark)) - Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault());
                                if (acaCalId > 41 && examBasicCalItemDto.ExamMetaTypeId == 3)
                                {
                                    ctbestmarkColumn = 20 * acaCalSectionObj.Course.Credits; //Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark));
                                }
                                table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + ctbestmarkColumn.ToString("#") + ")", typeof(string));
                            }
                        }
                    }

                    //decimal headerColumnTotalMark = examTemplateBasicCalculativeItemList.Sum(d => d.ExamTemplateBasicItemMark);
                    //headerColumnTotalMark = headerColumnTotalMark - examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault();


                    decimal incourseMark = 100 - final;
                    if (final == 0)
                        //table.Columns.Add("Total \n ", typeof(string));
                        if (examTemplateMasterObj != null)
                        {
                            table.Columns.Add("Total \n (" + examTemplateMasterObj.ExamTemplateMasterTotalMark.ToString("#") + ")", typeof(string));
                        }
                        else { }
                        else
                            table.Columns.Add("Total (" + final.ToString("#") + "+" + incourseMark.ToString("#") + ")%", typeof(string));
                    table.Columns.Add("Mark in \n100%", typeof(string));
                    table.Columns.Add("Grade", typeof(string));
                    table.Columns.Add("Grade \nPoint", typeof(string));

                    //table.Columns.Add("Student Roll", typeof(string));
                    //table.Columns.Add("Incourse " + incourseMark.ToString("#") + "%", typeof(string));
                    //table.Columns.Add("Final Mark", typeof(string));

                    List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(1);

                    for (int i = 0; i < studentIdList.Count; i++)
                    {
                        int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                        bool isDiscollegiate = StudentCourseHistoryManager.IsDiscollegiateByStudentCourseHistoryId(studentCourseHistoryId);
                        bool isAbsent = StudentCourseHistoryManager.IsAbsentInExamByStudentCourseHistoryId(studentCourseHistoryId);
                        int studentId = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                        ////List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                        DataRow newRow;
                        //if (studentObj != null)
                        //{
                        //    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(Convert.ToInt32(studentObj.GradeMasterId));
                        //}

                        object[] rowArray = new object[examTemplateBasicCalculativeItemList.Count + 6];
                        int newRowCounter = 0;
                        rowArray[0] = studentObj.Name;
                        rowArray[1] = studentObj.Roll;
                        newRowCounter = 1;
                        decimal totalMark = 0;
                        decimal inCourseExamMark = 0;
                        decimal finalWithoutConvertMark = 0;

                        for (int j = 0; j < examSequenceList.Count; j++)
                        {
                            decimal examMarks = 0;
                            string stringExamMark = null;
                            int sequenceNo = Convert.ToInt32(examSequenceList[j]);
                            ExamTemplateBasicCalculativeItemDTO examItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                            if (examItemObj != null)
                            {
                                if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Basic) //(int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                                {
                                    if (examItemObj.ExamMetaTypeId != 3)
                                    {

                                        decimal studentExamMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ConvertedMark).FirstOrDefault());
                                        examMarks = studentExamMark;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);
                                        if (examItemObj.ExamMetaTypeId == 5)
                                        {
                                            decimal scutinizerMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ScrutinizerMark).FirstOrDefault());
                                            if (scutinizerMark > 0)
                                            {
                                                stringExamMark = Convert.ToString(examMarks) + "(" + Convert.ToString(scutinizerMark) + ")";
                                                examMarks = scutinizerMark;
                                            }
                                            else
                                            {
                                                stringExamMark = Convert.ToString(examMarks);
                                            }
                                        }

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType != 0).FirstOrDefault();
                                        if (examCalculativeItemObj == null)
                                        {
                                            totalMark = totalMark + examMarks;
                                            if (examItemObj.ExamMetaTypeId != 8 || !examItemObj.ExamTemplateBasicItemName.Contains("Final"))
                                            {
                                                inCourseExamMark = inCourseExamMark + examMarks;
                                            }
                                            else
                                            {
                                                finalWithoutConvertMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                                else if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Calculative)
                                {
                                    if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Average)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark));
                                        int itemCount = Convert.ToInt32(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 0).ToList().Count);
                                        examMarks = marks / itemCount;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 1).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Max(d => d.ConvertedMark));

                                        examMarks = marks;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();

                                        //examMarks = (maxNumber + secondMax) / 2;
                                        examMarks = (maxNumber + secondMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 40);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax);
                                        
                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 60);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();
                                        var fourthMax = maxArrayObj.OrderByDescending(z => z).Skip(3).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax + fourthMax);
                                        
                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 80);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Sum)
                                    {
                                        decimal mark = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark);

                                        examMarks = mark;
                                        examMarks = Math.Round(examMarks, 2);

                                        stringExamMark = Convert.ToString(examMarks);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                }
                            }
                            rowArray[newRowCounter + 1] = stringExamMark;//examMarks;
                            newRowCounter = newRowCounter + 1;

                        }
                        totalMark = Math.Round(totalMark, 2); //Math.Ceiling(totalMark);
                        rowArray[newRowCounter + 1] = totalMark;
                        newRowCounter = newRowCounter + 1;

                        decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateMasterObj.ExamTemplateMasterTotalMark, 2), 2, MidpointRounding.AwayFromZero);
                        decimal markAddedTotalMark = 0;

                        markAddedTotalMark = convertedTotalMark + Convert.ToDecimal(0.99);
                        if (markAddedTotalMark > 100)
                        {
                            markAddedTotalMark = convertedTotalMark;
                        }

                        decimal markAddedGradePoint = 0;
                        decimal gradePoint = 0;
                        string gradeLetter = "Grading System Not Assigned";
                        int gradeId = 0;

                        if (gradeDetailList != null && gradeDetailList.Count > 0)
                        {

                            markAddedGradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                            gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                            if (gradePoint != markAddedGradePoint)
                            {
                                //convertedTotalMark = decimal.Round(markAddedTotalMark, 0, MidpointRounding.AwayFromZero);
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradeId;
                            }
                            else
                            {
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradeId;
                            }
                        }
                        if (examMetaTypeTypeObj != null)
                        {
                            ExamMarkColumnWiseDTO examMarkColumnWiseObj = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examMetaTypeTypeObj.ExamMetaTypeId).FirstOrDefault();
                            if (examMarkColumnWiseObj != null)
                            {
                                if (examMarkColumnWiseObj.ExamMarkTypeId == 2)
                                {
                                    gradePoint = Convert.ToDecimal(0.00);
                                    gradeLetter = "Absent";
                                }
                            }
                        }

                        

                        rowArray[newRowCounter + 1] = convertedTotalMark;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradeLetter;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradePoint;
                        newRowCounter = newRowCounter + 1;

                        //rowArray[newRowCounter + 1] = studentObj.Roll;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = inCourseExamMark ;
                        //newRowCounter = newRowCounter + 1;
                        //rowArray[newRowCounter + 1] = finalWithoutConvertMark;
                        //newRowCounter = newRowCounter + 1;

                        int retakeNo = 0;
                        StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        if (studentCourseHistory != null)
                        {
                            retakeNo = Convert.ToInt32(studentCourseHistory.RetakeNo);
                        }

                        if (studentCourseHistory != null)
                        {
                            

                            if (gradeLetter == "I")
                            {
                                studentCourseHistory.CourseStatusID = 3;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.I);
                                studentCourseHistory.ObtainedTotalMarks = null;
                                studentCourseHistory.ObtainedGPA = null;
                                studentCourseHistory.ObtainedGrade = null;
                                studentCourseHistory.GradeId = null;
                            }
                            else if (gradeLetter == "F")
                            {
                                studentCourseHistory.CourseStatusID = 7;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.F);
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = gradePoint;
                                studentCourseHistory.ObtainedGrade = gradeLetter;
                                studentCourseHistory.GradeId = gradeId;
                            }
                            else
                            {
                                studentCourseHistory.CourseStatusID = 5;// Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.Pt);
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = gradePoint;
                                studentCourseHistory.ObtainedGrade = gradeLetter;
                                studentCourseHistory.GradeId = gradeId;
                            }

                            
                            if (isAbsent )
                            {
                                studentCourseHistory.CourseStatusID = 14;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal(0.00);
                                studentCourseHistory.ObtainedGrade = Convert.ToString("AB");
                                studentCourseHistory.GradeId = 14;
                            }

                            if (isDiscollegiate)
                            {
                                studentCourseHistory.CourseStatusID = 15;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal(0.00);
                                studentCourseHistory.ObtainedGrade = Convert.ToString("DC");
                                studentCourseHistory.GradeId = 15;
                            }

                            if ((retakeNo == 10 || retakeNo == 12) && gradePoint > Convert.ToDecimal("3.25"))
                            {
                                studentCourseHistory.CourseStatusID = 5;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal("3.25");
                                studentCourseHistory.ObtainedGrade = Convert.ToString("B+");
                                studentCourseHistory.GradeId = 4;
                            }

                            //need to add ex
                            if (studentCourseHistory.CourseStatusID == 4)
                            {
                                studentCourseHistory.CourseStatusID = 4;
                                studentCourseHistory.ObtainedTotalMarks = Convert.ToDecimal("0.00");
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal("0.00");
                                studentCourseHistory.ObtainedGrade = Convert.ToString("E");
                                studentCourseHistory.GradeId = 16;
                            }
                            
                            studentCourseHistory.ModifiedBy = userId;
                            studentCourseHistory.ModifiedDate = DateTime.Now;

                            bool result = StudentCourseHistoryManager.Update(studentCourseHistory);
                            #region Log Insert
                            //try
                            //{
                            //    LogGeneralManager.Insert(
                            //            DateTime.Now,
                            //            userObj.LogInID,
                            //            "",
                            //            "",
                            //            "Result Publish",
                            //            userObj.LogInID + " attempted to publish result of a student, roll : " + studentObj.Roll + ", session : " + studentObj.AcademicCalender.Year + ", program : " + studentObj.Program.Name + ", group : " + studentObj.Department.ShortName + ", year : " + studentObj.Year.YearName + ", paper : " + Convert.ToString(ucPaper.selectedText),
                            //            "normal",
                            //            _pageId,
                            //            _pageName,
                            //            _pageUrl,
                            //            studentObj.Roll);
                            //}
                            //catch (Exception ex)
                            //{ }
                            #endregion
                        }
                        
                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }

        private static DataTable PublishExamMarkTheoryCourseSessionalIndustialTraining(int courseId, int versionId, int acaCalId, int acaCaSectionId, int userId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {
                AcademicCalenderSection acaCalSectionObj = AcademicCalenderSectionManager.GetById(acaCaSectionId);
                if (acaCalSectionObj != null)
                {
                    ExamTemplateMaster examtemplatemasterobject = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);
                    List<ExamTemplateBasicCalculativeItemDTO> examTemplateBasicCalculativeItemList = ExamTemplateMasterManager.ExamTemplateItemGetByAcaCalSectionId(acaCalSectionObj.AcaCal_SectionID).ToList();

                    List<ExamMarkColumnWiseDTO> examMarkColumnWiseList = ExamTemplateMasterManager.GetStudentExamMarkColumnWise(courseId, versionId, acaCalId, acaCaSectionId);

                    List<int> studentIdList = examMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();

                    List<int> examSequenceList = examTemplateBasicCalculativeItemList.Select(d => d.ColumnSequence).ToList();

                    ExamMetaType examMetaTypeTypeObj = ExamMetaTypeManager.GetAll().Where(d => d.ExamMetaTypeName == "Final Exam").FirstOrDefault();
                    ExamTemplateMaster examTemplateMasterObj = ExamTemplateMasterManager.GetById(acaCalSectionObj.BasicExamTemplateId);

                    table.Columns.Add("Student Name", typeof(string));
                    table.Columns.Add("Roll", typeof(string));
                    table.Columns.Add("Exam Roll", typeof(string));
                    decimal final = 0;

                    #region For Column Sequence

                    //if (examTemplateBasicCalculativeItemList != null && examTemplateBasicCalculativeItemList.Count > 0)
                    //{
                    //    examTemplateBasicCalculativeItemList = examTemplateBasicCalculativeItemList.OrderBy(o => o.ColumnSequence).ToList();
                    //}

                    //if (examSequenceList != null && examSequenceList.Count > 0)
                    //{
                    //    examSequenceList.Sort();
                    //} 

                    #endregion

                    for (int j = 0; j < examTemplateBasicCalculativeItemList.Count; j++)
                    {
                        ExamTemplateBasicCalculativeItemDTO examBasicCalItemDto = examTemplateBasicCalculativeItemList[j];
                        if (examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName == "Final")
                        {
                            ExamTemplateBasicItemDetails examTemplateDetails = ExamTemplateBasicItemDetailsManager.GetById(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemId);
                            table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateDetails.ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                            final = examTemplateDetails.ExamTemplateBasicItemMark;
                        }
                        else
                        {
                            if (examTemplateBasicCalculativeItemList[j].CalculationType == 0)
                            {
                                table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemMark.ToString("#") + ")", typeof(string));
                            }
                            else
                            {
                                decimal ctbestmarkColumn = Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark)) - Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault());
                                if (acaCalId > 41 && examBasicCalItemDto.ExamMetaTypeId == 3)
                                {
                                    ctbestmarkColumn = 20 * acaCalSectionObj.Course.Credits; //Convert.ToDecimal(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Sum(d => d.ExamTemplateBasicItemMark));
                                }
                                table.Columns.Add(examTemplateBasicCalculativeItemList[j].ExamTemplateBasicItemName + "\n (" + ctbestmarkColumn.ToString("#") + ")", typeof(string));
                            }
                        }
                    }

                    decimal headerColumnTotalMark = examTemplateBasicCalculativeItemList.Sum(d => d.ExamTemplateBasicItemMark);
                    headerColumnTotalMark = headerColumnTotalMark - examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == 3).Select(d => d.ExamTemplateBasicItemMark).FirstOrDefault();


                    decimal incourseMark = 100 - final;
                    if (final == 0)
                    {
                        //table.Columns.Add("Total \n ", typeof(string));
                        table.Columns.Add("Total \n (" + headerColumnTotalMark.ToString("#") + ")", typeof(string));
                    }
                    else
                    {
                        table.Columns.Add("Total (" + final.ToString("#") + "+" + incourseMark.ToString("#") + ")%", typeof(string));
                    }
                    table.Columns.Add("Mark in\n 100%", typeof(string));
                    table.Columns.Add("Grade", typeof(string));
                    table.Columns.Add("Grade Point", typeof(string));

                    List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                    gradeDetailList = GradeDetailsManager.GetByGradeMasterId(1);

                    for (int i = 0; i < studentIdList.Count; i++)
                    {
                        int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                        bool isDiscollegiate = StudentCourseHistoryManager.IsDiscollegiateByStudentCourseHistoryId(studentCourseHistoryId);
                        bool isAbsent = StudentCourseHistoryManager.IsAbsentInExamByStudentCourseHistoryId(studentCourseHistoryId);
                        int studentId = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                        //StudentExamRoll studentExamRoll = StudentExamRollManager.GetByStudentIdAcaCalId(studentId, acaCalId);

                        DataRow newRow;
                        //if (studentObj != null)
                        //{

                        //}

                        object[] rowArray = new object[examTemplateBasicCalculativeItemList.Count + 7];
                        int newRowCounter = 0;
                        rowArray[0] = studentObj.Name;
                        rowArray[1] = studentObj.Roll;
                        rowArray[2] = "";
                        //if (studentExamRoll != null)
                        //{
                        //    rowArray[2] = studentExamRoll.ExamRoll;
                        //}
                        //else { rowArray[2] = ""; }
                        newRowCounter = 2;
                        decimal totalMark = 0;
                        decimal inCourseExamMark = 0;
                        decimal finalWithoutConvertMark = 0;

                        for (int j = 0; j < examSequenceList.Count; j++)
                        {
                            decimal examMarks = 0;
                            int sequenceNo = Convert.ToInt32(examSequenceList[j]);
                            ExamTemplateBasicCalculativeItemDTO examItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                            if (examItemObj != null)
                            {
                                if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Basic) //(int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                                {
                                    decimal studentExamMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.ConvertedMark).FirstOrDefault());
                                    examMarks = studentExamMark;
                                    examMarks = Math.Round(examMarks, 2);

                                    ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType != 0).FirstOrDefault();
                                    if (examCalculativeItemObj == null)
                                    {
                                        totalMark = totalMark + examMarks;
                                        if (examItemObj.ExamMetaTypeId != 8 || !examItemObj.ExamTemplateBasicItemName.Contains("Final"))
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                        }
                                        else
                                        {
                                            finalWithoutConvertMark = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateBasicItemId == examItemObj.ExamTemplateBasicItemId && d.ExamTemplateBasicItemId > 0 && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                        }
                                    }
                                }
                                else if (examItemObj.ExamTemplateMasterTypeId == (int)CommonUtility.CommonEnum.ExamTemplateType.Calculative)
                                {
                                    if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Average)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark));
                                        int itemCount = Convert.ToInt32(examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 0).ToList().Count);
                                        examMarks = marks / itemCount;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == 1).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne)
                                    {
                                        decimal marks = Convert.ToDecimal(examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Max(d => d.ConvertedMark));

                                        examMarks = marks;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestOne).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();

                                        //examMarks = (maxNumber + secondMax) / 2;
                                        examMarks = (maxNumber + secondMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 40);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 60);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestThree).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour)
                                    {
                                        decimal[] markArray = new decimal[] { };
                                        markArray = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Select(d => d.ConvertedMark).ToArray();

                                        var maxArrayObj = markArray;
                                        var maxNumber = maxArrayObj.Max(z => z);
                                        var secondMax = maxArrayObj.OrderByDescending(z => z).Skip(1).First();
                                        var thirdMax = maxArrayObj.OrderByDescending(z => z).Skip(2).First();
                                        var fourthMax = maxArrayObj.OrderByDescending(z => z).Skip(3).First();

                                        //examMarks = (maxNumber + secondMax + thirdMax) / 3;
                                        examMarks = (maxNumber + secondMax + thirdMax + fourthMax);

                                        //code for OBE template
                                        decimal convertToMark = 20 * acaCalSectionObj.Course.Credits;

                                        if (acaCalId > 41)
                                        {
                                            examMarks = ((examMarks * convertToMark) / 80);
                                        }
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestFour).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                    else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.Sum)
                                    {
                                        decimal mark = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examItemObj.ExamMetaTypeId).Sum(d => d.ConvertedMark);

                                        examMarks = mark;
                                        examMarks = Math.Round(examMarks, 2);

                                        ExamTemplateBasicCalculativeItemDTO examCalculativeItemObj = examTemplateBasicCalculativeItemList.Where(d => d.ExamMetaTypeId == examItemObj.ExamMetaTypeId && d.CalculationType == (int)CommonUtility.CommonEnum.ExamCalculationType.BestTwo).FirstOrDefault();
                                        if (examCalculativeItemObj != null)
                                        {
                                            inCourseExamMark = inCourseExamMark + examMarks;
                                            totalMark = totalMark + examMarks;
                                        }
                                    }
                                }
                            }
                            rowArray[newRowCounter + 1] = examMarks;
                            newRowCounter = newRowCounter + 1;

                        }
                        totalMark = Math.Round(totalMark, 2); //Math.Ceiling(totalMark);
                        rowArray[newRowCounter + 1] = totalMark;
                        newRowCounter = newRowCounter + 1;

                        decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateMasterObj.ExamTemplateMasterTotalMark, 2), 2, MidpointRounding.AwayFromZero);
                        decimal markAddedTotalMark = 0;

                        markAddedTotalMark = convertedTotalMark + Convert.ToDecimal(0.99);
                        if (markAddedTotalMark > 100)
                        {
                            markAddedTotalMark = convertedTotalMark;
                        }

                        decimal markAddedGradePoint = 0;
                        decimal gradePoint = 0;
                        string gradeLetter = "Grading System Not Assigned";
                        int gradeId = 0;
                        if (gradeDetailList != null && gradeDetailList.Count > 0)
                        {

                            markAddedGradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                            gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                            if (gradePoint != markAddedGradePoint)
                            {
                                //convertedTotalMark = decimal.Round(markAddedTotalMark, 0, MidpointRounding.AwayFromZero);
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradeId;
                            }
                            else
                            {
                                gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().Grade;
                                gradeId = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradeId;
                            }
                        }
                        if (examMetaTypeTypeObj != null)
                        {
                            ExamMarkColumnWiseDTO examMarkColumnWiseObj = examMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamMetaTypeId == examMetaTypeTypeObj.ExamMetaTypeId).FirstOrDefault();
                            if (examMarkColumnWiseObj != null)
                            {
                                if (examMarkColumnWiseObj.ExamMarkTypeId == 2)
                                {
                                    gradePoint = Convert.ToDecimal(0.00);
                                    gradeLetter = "Absent";
                                }
                            }
                        }

                        //if(){}

                        rowArray[newRowCounter + 1] = Math.Round(((totalMark * 100) / examtemplatemasterobject.ExamTemplateMasterTotalMark), 2);
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradeLetter;
                        newRowCounter = newRowCounter + 1;
                        rowArray[newRowCounter + 1] = gradePoint;
                        newRowCounter = newRowCounter + 1;

                        int retakeNo = 0;
                        StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        if (studentCourseHistory != null)
                        {
                            retakeNo = Convert.ToInt32(studentCourseHistory.RetakeNo);
                        }
                        if (studentCourseHistory != null)
                        {
                            if (gradeLetter == "I")
                            {
                                studentCourseHistory.CourseStatusID = 3;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.I);
                                studentCourseHistory.ObtainedTotalMarks = null;
                                studentCourseHistory.ObtainedGPA = null;
                                studentCourseHistory.ObtainedGrade = null;
                                studentCourseHistory.GradeId = null;
                            }
                            else if (gradeLetter == "F")
                            {
                                studentCourseHistory.CourseStatusID = 7;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.F);
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = gradePoint;
                                studentCourseHistory.ObtainedGrade = gradeLetter;
                                studentCourseHistory.GradeId = gradeId;
                            }
                            else
                            {
                                studentCourseHistory.CourseStatusID = 5;// Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.Pt);
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = gradePoint;
                                studentCourseHistory.ObtainedGrade = gradeLetter;
                                studentCourseHistory.GradeId = gradeId;
                            }

                            if (isAbsent)
                            {
                                studentCourseHistory.CourseStatusID = 14;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal(0.00);
                                studentCourseHistory.ObtainedGrade = Convert.ToString("AB");
                                studentCourseHistory.GradeId = 14;
                            }

                            if (isDiscollegiate)
                            {
                                studentCourseHistory.CourseStatusID = 15;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal(0.00);
                                studentCourseHistory.ObtainedGrade = Convert.ToString("DC");
                                studentCourseHistory.GradeId = 15;
                            }

                            if ((retakeNo == 10 || retakeNo == 12) && gradePoint > Convert.ToDecimal("3.25"))
                            {
                                studentCourseHistory.CourseStatusID = 5;
                                studentCourseHistory.ObtainedTotalMarks = totalMark;
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal("3.25");
                                studentCourseHistory.ObtainedGrade = Convert.ToString("B+");
                                studentCourseHistory.GradeId = 4;
                            }

                            if (studentCourseHistory.CourseStatusID == 4)
                            {
                                studentCourseHistory.CourseStatusID = 4;
                                studentCourseHistory.ObtainedTotalMarks = Convert.ToDecimal("0.00");
                                studentCourseHistory.ObtainedGPA = Convert.ToDecimal("0.00");
                                studentCourseHistory.ObtainedGrade = Convert.ToString("E");
                                studentCourseHistory.GradeId = 16;
                            }

                            studentCourseHistory.ModifiedBy = userId;
                            studentCourseHistory.ModifiedDate = DateTime.Now;

                            bool result = StudentCourseHistoryManager.Update(studentCourseHistory);
                            #region Log Insert
                            //try
                            //{
                            //    LogGeneralManager.Insert(
                            //            DateTime.Now,
                            //            userObj.LogInID,
                            //            "",
                            //            "",
                            //            "Result Publish",
                            //            userObj.LogInID + " attempted to publish result of a student, roll : " + studentObj.Roll + ", session : " + studentObj.AcademicCalender.Year + ", program : " + studentObj.Program.Name + ", group : " + studentObj.Department.ShortName + ", year : " + studentObj.Year.YearName + ", paper : " + Convert.ToString(ucPaper.selectedText),
                            //            "normal",
                            //            _pageId,
                            //            _pageName,
                            //            _pageUrl,
                            //            studentObj.Roll);
                            //}
                            //catch (Exception ex)
                            //{ }
                            #endregion
                        }

                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }
        #endregion publish exam mark

        public static List<StudentCourseGradeDTO> GetStudentCourseGradeDTO(int courseId, int versionId, int acaCalId, int acaCalSectionId)
        {
            List<StudentCourseGradeDTO> examResultList = RepositoryManager.ExamTemplateMaster_Repository.GetStudentCourseGradeDTO(courseId, versionId, acaCalId, acaCalSectionId);

            return examResultList;
        }
    }
}

