using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.BusinessLogic
{
    public class ExamTemplateManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamTemplateCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamTemplate> GetCacheAsList(string rawKey)
        {
            List<ExamTemplate> list = (List<ExamTemplate>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamTemplate GetCacheItem(string rawKey)
        {
            ExamTemplate item = (ExamTemplate)HttpRuntime.Cache[GetCacheKey(rawKey)];
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

        public static int Insert(ExamTemplate examtemplate)
        {
            int id = RepositoryManager.ExamTemplate_Repository.Insert(examtemplate);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamTemplate examtemplate)
        {
            bool isExecute = RepositoryManager.ExamTemplate_Repository.Update(examtemplate);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamTemplate_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamTemplate GetById(int? id)
        {
            string rawKey = "ExamTemplateByID" + id;
            ExamTemplate examtemplate = GetCacheItem(rawKey);

            if (examtemplate == null)
            {
                examtemplate = RepositoryManager.ExamTemplate_Repository.GetById(id);
                if (examtemplate != null)
                    AddCacheItem(rawKey, examtemplate);
            }

            return examtemplate;
        }

        public static List<ExamTemplate> GetAll()
        {
            List<ExamTemplate> list = RepositoryManager.ExamTemplate_Repository.GetAll();
            return list;
        }

        public static bool GetExamTemplateByName(string examTemplateName)
        {
            ExamTemplate examTemplate = RepositoryManager.ExamTemplate_Repository.GetExamTemplateByName(examTemplateName);
            if (examTemplate == null) { return true; }
            else { return false; }
        }

        public static ExamTemplate GetBySyllabusDetailIdAcaCalId(int syllabusDetailId, int acaCalId)
        {
            ExamTemplate examtemplate = RepositoryManager.ExamTemplate_Repository.GetBySyllabusDetailIdAcaCalId(syllabusDetailId, acaCalId);
            return examtemplate;
        }

        public static DataTable GetExamResultDataTable(int courseId, int versionId, int acaCaSectionId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {

                int acaCalSectionId = acaCaSectionId;
                AcademicCalenderSection acaCalSecObj = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                List<TempStudentExamMarkColumnWise> studentExamMarkColumnWiseList = ExamMarkMasterManager.GetContinuousMarkColumnWise(acaCalSectionId).ToList();
                List<ExamTemplateItem> examTemplateItemList = ExamTemplateItemManager.GetBasicWithOutFinalTemplateItemByExamTemplateId(acaCalSecObj.BasicExamTemplateId).ToList();
                List<ExamTemplateItem> examNameList = examTemplateItemList.Distinct().OrderBy(d => d.ColumnSequence).ToList();
                List<int> studentIdList = studentExamMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();
                List<int> examSequenceList = examTemplateItemList.OrderBy(d => d.ColumnSequence).Select(d => d.ColumnSequence).Distinct().ToList();

                table.Columns.Add("Student Name", typeof(string));
                table.Columns.Add("Roll", typeof(string));



                for (int j = 0; j < examNameList.Count; j++)
                {
                    table.Columns.Add(examNameList[j].ExamName, typeof(string));
                }

                for (int i = 0; i < studentIdList.Count; i++)
                {
                    bool isFail = false;

                    int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                    int studentId = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                    List<Grade> gradeList = new List<Grade>();
                    LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);

                    string studentRoll = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.Roll).FirstOrDefault(); //GetStudentRoll(studentId);
                    DataRow newRow;

                    int tabulationRowCounter = examNameList.ToList().Count;
                    object[] rowArray = new object[tabulationRowCounter + 2];
                    int newRowCounter = 0;
                    rowArray[0] = studentObj.Name; ;
                    rowArray[1] = studentObj.Roll;
                    newRowCounter = 1;
                    for (int k = 0; k < examSequenceList.Count; k++)
                    {
                        int sequenceNo = Convert.ToInt32(examSequenceList[k]);
                        ExamTemplateItem examItemObj = examTemplateItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                        if (examItemObj != null)
                        {

                            if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                            {
                                decimal totalMark = 0;

                                decimal studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                totalMark = studentExamMark;
                                totalMark = Math.Round(totalMark, 2);
                                if (examItemObj.PassMark > 0)
                                {
                                    if (totalMark < examItemObj.PassMark)
                                    {
                                        isFail = true;
                                    }
                                }

                                int examStatus = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.ExamStatus).FirstOrDefault();
                                if (examStatus == 2)
                                {
                                    rowArray[newRowCounter + 1] = "Ab"; //totalMark + " (ab)";
                                }
                                else
                                {
                                    rowArray[newRowCounter + 1] = totalMark;
                                }
                                    //newRowCounter = newRowCounter + 1;
                                
                                //rowArray[newRowCounter + 1] = "Ab";
                                newRowCounter = newRowCounter + 1;
                            }
                            else if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Calculative)
                            {
                                decimal totalMark = 0;

                                List<ExamMarkEquationColumnOrder> sumColumnList = ExamMarkEquationColumnOrderManager.GetByTemplateItemId(examItemObj.ExamTemplateItemId);
                                if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Sum)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = studentExamMark + Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());
                                    }

                                    totalMark = studentExamMark;
                                    //totalMark = Math.Round(totalMark, 0, MidpointRounding.AwayFromZero);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }
                                else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Average)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = studentExamMark + Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());

                                    }
                                    totalMark = studentExamMark / columnCount;
                                    //totalMark = Math.Round(totalMark, 2);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }
                                else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_One)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    decimal bestOneMark = 0;
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());
                                        if (studentExamMark >= bestOneMark)
                                        {
                                            bestOneMark = studentExamMark;
                                        }
                                    }
                                    totalMark = bestOneMark;
                                    //totalMark = Math.Round(totalMark, 2);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Two)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    decimal bestTwoTotal = 0;
                                    List<decimal> markList = new List<decimal>();
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());
                                        markList.Add(studentExamMark);
                                    }
                                    for (int n = 0; n < 2; n++)
                                    {
                                        decimal maxMark = markList.Max();
                                        markList.Remove(maxMark);
                                        bestTwoTotal = bestTwoTotal + maxMark;
                                    }
                                    totalMark = bestTwoTotal / 2;
                                    //totalMark = Math.Round(totalMark, 2);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Three)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    decimal bestThreeTotal = 0;
                                    List<decimal> markList = new List<decimal>();
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());

                                        markList.Add(studentExamMark);
                                    }
                                    for (int n = 0; n < 3; n++)
                                    {
                                        decimal maxMark = markList.Max();
                                        markList.Remove(maxMark);
                                        bestThreeTotal = bestThreeTotal + maxMark;

                                    }
                                    totalMark = bestThreeTotal / 3;
                                    //totalMark = Math.Round(totalMark, 2);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                else if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Percentage)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    decimal sumTotal = 0;
                                    decimal multiplyBy = Convert.ToDecimal(examItemObj.MultiplyBy);
                                    decimal divideBy = Convert.ToDecimal(examItemObj.DivideBy);



                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        List<decimal> studentExamMarkList = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).ToList();//.FirstOrDefault());
                                        studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());

                                        sumTotal = sumTotal + studentExamMark;

                                    }
                                    if (multiplyBy > 0 && divideBy > 0)
                                    {
                                        sumTotal = Math.Round(sumTotal, 2);
                                        totalMark = (sumTotal * multiplyBy) / divideBy;
                                        //totalMark = Math.Round(totalMark, 2);


                                    }
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                decimal gradePoint = 0;
                                decimal markAddedGradePoint = 0;
                                decimal markAddedTotalMark = 0;

                                markAddedTotalMark = totalMark + Convert.ToDecimal(0.50);
                                if (markAddedTotalMark > 100)
                                {
                                    markAddedTotalMark = totalMark;
                                }
                                //markAddedGradePoint = gradeList.Where(d => d.MinMarks <= markAddedTotalMark && d.MaxMarks >= markAddedTotalMark).FirstOrDefault().GradePoint;
                                //gradePoint = gradeList.Where(d => d.MinMarks <= totalMark && d.MaxMarks >= totalMark).FirstOrDefault().GradePoint;
                                //if (gradePoint != markAddedGradePoint)
                                //{
                                //    totalMark = decimal.Round(markAddedTotalMark, 0, MidpointRounding.AwayFromZero);
                                //}

                                TempStudentExamMarkColumnWise studentExamMarkObj = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == sequenceNo).FirstOrDefault();
                                TempStudentExamMarkColumnWise tempStudentExamMarkObj = studentExamMarkObj;
                                tempStudentExamMarkObj.Marks = totalMark;
                                studentExamMarkColumnWiseList.Remove(studentExamMarkObj);
                                studentExamMarkColumnWiseList.Add(tempStudentExamMarkObj);

                                //List<TempStudentExamMarkColumnWise> abc1 = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).ToList();

                                //if (examItemObj.ShowInTabulation == true)
                                //{
                                rowArray[newRowCounter + 1] = totalMark;
                                newRowCounter = newRowCounter + 1;
                                //}
                            }
                        }
                        else { }
                    }
                    newRow = table.NewRow();
                    newRow.ItemArray = rowArray;
                    table.Rows.Add(newRow);
                }
            }
            return table;
        }

        public static List<ExamResultDTO> GetResultFromTable(DataTable dt)
        {
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    ExamResultDTO examResultObj = new ExamResultDTO();
                    examResultObj.StudentName = dt.Rows[i].ItemArray[0].ToString();
                    examResultObj.Roll = dt.Rows[i].ItemArray[1].ToString();
                    examResultObj.ExamName = dt.Columns[j].ColumnName.ToString();
                    examResultObj.ColumnSequence = Convert.ToInt32(j - 1);
                    examResultObj.MarksOrGrade = dt.Rows[i].ItemArray[j].ToString();
                    examResultList.Add(examResultObj);
                }
            }
            return examResultList;
        }

        public static List<ExamResultDTO> GetExamResultDTO(int courseId, int versionId, int acaCaSectionId)
        {
            DataTable dt = GetExamResultDataTable(courseId, versionId, acaCaSectionId);
            List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
            examResultList = GetResultFromTable(dt);
            return examResultList;
        }

        //public static List<ExamResultTabulationDTO> GetExamResultDTO(int courseId, int versionId, int acaCaSectionId)
        //{
        //    DataTable dt = GetExamResultDataTable(courseId, versionId, acaCaSectionId);
        //    List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
        //    examResultList = GetResultFromTable(dt);
        //    return examResultList;
        //}


        //public static List<ExamResultDTO> GetResultFromTable(DataTable dt)
        //{
        //    List<ExamResultDTO> examResultList = new List<ExamResultDTO>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 2; j < dt.Columns.Count; j++)
        //        {
        //            ExamResultDTO examResultObj = new ExamResultDTO();
        //            examResultObj.StudentName = dt.Rows[i].ItemArray[0].ToString();
        //            examResultObj.Roll = dt.Rows[i].ItemArray[1].ToString();
        //            examResultObj.ExamName = dt.Columns[j].ColumnName.ToString();
        //            examResultObj.ColumnSequence = Convert.ToInt32(j - 1);
        //            examResultObj.MarksOrGrade = dt.Rows[i].ItemArray[j].ToString();
        //            examResultList.Add(examResultObj);
        //        }
        //    }
        //    return examResultList;
        //}

        public static DataTable GetExamResultTabulationDataTable(int courseId, int versionId, int acaCaSectionId, int userId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {

                int acaCalSectionId = acaCaSectionId;
                AcademicCalenderSection acaCalSecObj = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                List<TempStudentExamMarkColumnWise> studentExamMarkColumnWiseList = ExamMarkMasterManager.GetAllMarkColumnWise(acaCalSectionId).ToList();
                List<ExamTemplateItem> examTemplateItemList = ExamTemplateItemManager.GetByExamTemplateId(acaCalSecObj.BasicExamTemplateId).ToList();
                ExamTemplateItem finalMarkTemplateItem = examTemplateItemList.Where(d => d.IsFinalExam==true).FirstOrDefault();
                List<ExamTemplateItem> examNameList = examTemplateItemList.Distinct().OrderBy(d => d.ColumnSequence).ToList();
                List<int> studentIdList = studentExamMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();
                List<int> examSequenceList = examTemplateItemList.OrderBy(d => d.ColumnSequence).Select(d => d.ColumnSequence).Distinct().ToList();
                List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                gradeDetailList = GradeDetailsManager.GetAll();

                table.Columns.Add("Student Name", typeof(string));
                table.Columns.Add("Roll", typeof(string));

                for (int j = 0; j < examNameList.Count; j++)
                {
                    table.Columns.Add(examNameList[j].ExamName, typeof(string));
                }

                for (int i = 0; i < studentIdList.Count; i++) //studentIdList.Count
                {
                    bool isFail = false;

                    int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                    int studentId = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                    List<Grade> gradeList = new List<Grade>();
                    LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);

                    string studentRoll = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.Roll).FirstOrDefault(); //GetStudentRoll(studentId);
                    DataRow newRow;

                    int tabulationRowCounter = examNameList.Count;
                    object[] rowArray = new object[tabulationRowCounter + 2];
                    int newRowCounter = 0;
                    rowArray[0] = studentObj.Name; ;
                    rowArray[1] = studentObj.Roll;
                    decimal totalMark = 0;
                    newRowCounter = 1;
                    for (int k = 0; k < examSequenceList.Count; k++)
                    {
                        int sequenceNo = Convert.ToInt32(examSequenceList[k]);
                        ExamTemplateItem examItemObj = examTemplateItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                        if (examItemObj != null)
                        {
                            if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                            {
                                decimal studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                totalMark = studentExamMark;
                                totalMark = Math.Round(totalMark, 2);
                                if (examItemObj.PassMark > 0)
                                {
                                    if (totalMark < examItemObj.PassMark)
                                    {
                                        isFail = true;
                                    }
                                }
                                if (examItemObj.ShowInTabulation == true)
                                {
                                    int examStatus = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.ExamStatus).FirstOrDefault();
                                    if (examStatus == 2)
                                    {
                                        rowArray[newRowCounter + 1] = "Ab"; //totalMark + " (ab)";
                                    }
                                    else
                                    {
                                        rowArray[newRowCounter + 1] = totalMark;
                                    }
                                }
                                newRowCounter = newRowCounter + 1;
                            }
                            else if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Calculative)
                            {
                                //decimal totalMark = 0;

                                List<ExamMarkEquationColumnOrder> sumColumnList = ExamMarkEquationColumnOrderManager.GetByTemplateItemId(examItemObj.ExamTemplateItemId);
                                if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Sum)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = studentExamMark + Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());
                                    }

                                    totalMark = studentExamMark;
                                    //totalMark = Math.Ceiling(totalMark);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examItemObj, acaCaSectionId, userId);

                                ExamMarkDetailsManager.InsertEditExamMarkDetails(studentCourseHistoryId, false, examMarkMasterId, examItemObj.ExamTemplateItemId, examItemObj.ExamMark, totalMark, userId);

                                TempStudentExamMarkColumnWise studentExamMarkObj = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == sequenceNo).FirstOrDefault();
                                TempStudentExamMarkColumnWise tempStudentExamMarkObj = studentExamMarkObj;
                                tempStudentExamMarkObj.Marks = totalMark;
                                studentExamMarkColumnWiseList.Remove(studentExamMarkObj);
                                studentExamMarkColumnWiseList.Add(tempStudentExamMarkObj);

                                rowArray[newRowCounter + 1] = totalMark;
                                newRowCounter = newRowCounter + 1;                                
                            }
                        }
                    }
                    TempStudentExamMarkColumnWise studentFinalExamMarkObj = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == finalMarkTemplateItem.ExamTemplateItemId).FirstOrDefault();

                    InsertStudentCourseHistory(finalMarkTemplateItem.ExamTemplateId, Math.Ceiling(totalMark), gradeDetailList, studentFinalExamMarkObj, studentCourseHistoryId, userId);

                    newRow = table.NewRow();
                    newRow.ItemArray = rowArray;
                    table.Rows.Add(newRow);
                }
            }
            return table;
        }

        private static void InsertStudentCourseHistory(int templateId, decimal totalMark, List<GradeDetails> gradeDetailList, TempStudentExamMarkColumnWise studentFinalExamMarkObj, int studentCourseHistoryId, int userId)
        {
            ExamTemplate examTemplateObj = ExamTemplateManager.GetById(templateId);
            decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateObj.ExamTemplateMarks, 2));
            //decimal convertedTotalMark = decimal.Round(Math.Round((totalMark * 100) / examTemplateObj.ExamTemplateMarks, 2), 2, MidpointRounding.AwayFromZero);

            decimal gradePoint = 0;
            string gradeLetter = "";
            int gradeId = 0;

            if (gradeDetailList != null && gradeDetailList.Count > 0)
            {
                gradePoint = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradePoint;
                gradeLetter = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().Grade;
                gradeId = gradeDetailList.Where(d => d.MinMarks <= convertedTotalMark && d.MaxMarks >= convertedTotalMark).FirstOrDefault().GradeId;
            }
           
            if (studentFinalExamMarkObj != null)
            {
                if (studentFinalExamMarkObj.ExamStatus == 2)
                {
                    gradePoint = Convert.ToDecimal(0.00);
                    gradeLetter = "Ab";
                }
            }

            StudentCourseHistory studentCourseHistory = StudentCourseHistoryManager.GetById(studentCourseHistoryId);
                        
            if (studentCourseHistory != null)
            {
                if (gradeLetter == "Ab")
                {
                    studentCourseHistory.CourseStatusID = 10;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.I);
                    studentCourseHistory.ObtainedTotalMarks = null;
                    studentCourseHistory.ObtainedGPA = Convert.ToDecimal(0.00);
                    studentCourseHistory.ObtainedGrade = "Ab";
                    studentCourseHistory.GradeId = 13;
                }
                else if (gradeLetter == "F")
                {
                    studentCourseHistory.CourseStatusID = 7;//Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.F);
                    studentCourseHistory.ObtainedTotalMarks = convertedTotalMark;
                    studentCourseHistory.ObtainedGPA = gradePoint;
                    studentCourseHistory.ObtainedGrade = gradeLetter;
                    studentCourseHistory.GradeId = gradeId;
                }
                else
                {
                    studentCourseHistory.CourseStatusID = 5;// Convert.ToInt32(CommonUtility.CommonEnum.CourseStatus.Pt);
                    studentCourseHistory.ObtainedTotalMarks = convertedTotalMark;
                    studentCourseHistory.ObtainedGPA = gradePoint;
                    studentCourseHistory.ObtainedGrade = gradeLetter;
                    studentCourseHistory.GradeId = gradeId;
                }

                studentCourseHistory.ModifiedBy = userId;
                studentCourseHistory.ModifiedDate = DateTime.Now;

                bool result = StudentCourseHistoryManager.Update(studentCourseHistory);
            }
        }

        public static void ProcessFirstSecondThirdExaminerMarkToExamMark(int acacalSectionId) 
        {
            RepositoryManager.ExamTemplate_Repository.ProcessFirstSecondThirdExaminerMarkToExamMark(acacalSectionId);
        }


        public static DataTable ProcessContinuousAssessmentMark(int acaCaSectionId, int userId)
        {
            DataTable table = new DataTable();
            if (acaCaSectionId > 0)
            {

                int acaCalSectionId = acaCaSectionId;
                AcademicCalenderSection acaCalSecObj = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                List<TempStudentExamMarkColumnWise> studentExamMarkColumnWiseList = ExamMarkMasterManager.GetAllMarkColumnWise(acaCalSectionId).ToList();
                List<ExamTemplateItem> examTemplateItemList = ExamTemplateItemManager.GetByExamTemplateId(acaCalSecObj.BasicExamTemplateId).ToList();
                ExamTemplateItem finalMarkTemplateItem = examTemplateItemList.Where(d => d.IsFinalExam==true).FirstOrDefault();
                List<ExamTemplateItem> examNameList = examTemplateItemList.Distinct().OrderBy(d => d.ColumnSequence).ToList();
                List<int> studentIdList = studentExamMarkColumnWiseList.Select(d => d.StudentCourseHistoryId).Distinct().ToList();
                List<int> examSequenceList = examTemplateItemList.OrderBy(d => d.ColumnSequence).Select(d => d.ColumnSequence).Distinct().ToList();
                List<GradeDetails> gradeDetailList = new List<GradeDetails>();
                gradeDetailList = GradeDetailsManager.GetAll();

                table.Columns.Add("Student Name", typeof(string));
                table.Columns.Add("Roll", typeof(string));

                for (int j = 0; j < examNameList.Count; j++)
                {
                    table.Columns.Add(examNameList[j].ExamName, typeof(string));
                }

                for (int i = 0; i < studentIdList.Count; i++) //studentIdList.Count
                {
                    bool isFail = false;

                    int studentCourseHistoryId = Convert.ToInt32(studentIdList[i]);
                    int studentId = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.StudentId).FirstOrDefault();
                    List<Grade> gradeList = new List<Grade>();
                    LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);

                    string studentRoll = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId).Select(d => d.Roll).FirstOrDefault(); //GetStudentRoll(studentId);
                    DataRow newRow;

                    int tabulationRowCounter = examNameList.Count;
                    object[] rowArray = new object[tabulationRowCounter + 2];
                    int newRowCounter = 0;
                    rowArray[0] = studentObj.Name; ;
                    rowArray[1] = studentObj.Roll;
                    decimal totalMark = 0;
                    newRowCounter = 1;
                    for (int k = 0; k < examSequenceList.Count; k++)
                    {
                        int sequenceNo = Convert.ToInt32(examSequenceList[k]);
                        ExamTemplateItem examItemObj = examTemplateItemList.Where(d => d.ColumnSequence == sequenceNo).FirstOrDefault();
                        if (examItemObj != null)
                        {
                            if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic)
                            {
                                decimal studentExamMark = Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.Marks).FirstOrDefault());
                                totalMark = studentExamMark;
                                totalMark = Math.Round(totalMark, 2);
                                if (examItemObj.PassMark > 0)
                                {
                                    if (totalMark < examItemObj.PassMark)
                                    {
                                        isFail = true;
                                    }
                                }
                                if (examItemObj.ShowInTabulation == true)
                                {
                                    int examStatus = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == examItemObj.ExamTemplateItemId && d.ColumnSequence == sequenceNo).Select(d => d.ExamStatus).FirstOrDefault();
                                    if (examStatus == 2)
                                    {
                                        rowArray[newRowCounter + 1] = "Ab"; //totalMark + " (ab)";
                                    }
                                    else
                                    {
                                        rowArray[newRowCounter + 1] = totalMark;
                                    }
                                }
                                newRowCounter = newRowCounter + 1;
                            }
                            else if (examItemObj.ColumnType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Calculative)
                            {
                                //decimal totalMark = 0;

                                List<ExamMarkEquationColumnOrder> sumColumnList = ExamMarkEquationColumnOrderManager.GetByTemplateItemId(examItemObj.ExamTemplateItemId);
                                if (examItemObj.CalculationType == (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Sum)
                                {
                                    decimal studentExamMark = 0;
                                    int columnCount = sumColumnList.Count;
                                    for (int m = 0; m < sumColumnList.Count; m++)
                                    {
                                        int columnSequence = Convert.ToInt32(sumColumnList[m].SumColumnNo);
                                        studentExamMark = studentExamMark + Convert.ToDecimal(studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == columnSequence).Select(d => d.Marks).FirstOrDefault());
                                    }

                                    totalMark = studentExamMark;
                                    //totalMark = Math.Ceiling(totalMark);
                                    if (examItemObj.PassMark > 0)
                                    {
                                        if (totalMark < examItemObj.PassMark)
                                        {
                                            isFail = true;
                                        }
                                    }
                                }

                                int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examItemObj, acaCaSectionId, userId);

                                ExamMarkDetailsManager.InsertEditExamMarkDetails(studentCourseHistoryId, false, examMarkMasterId, examItemObj.ExamTemplateItemId, examItemObj.ExamMark, totalMark, userId);

                                TempStudentExamMarkColumnWise studentExamMarkObj = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ColumnSequence == sequenceNo).FirstOrDefault();
                                TempStudentExamMarkColumnWise tempStudentExamMarkObj = studentExamMarkObj;
                                tempStudentExamMarkObj.Marks = totalMark;
                                studentExamMarkColumnWiseList.Remove(studentExamMarkObj);
                                studentExamMarkColumnWiseList.Add(tempStudentExamMarkObj);

                                rowArray[newRowCounter + 1] = totalMark;
                                newRowCounter = newRowCounter + 1;
                            }
                        }
                    }
                    TempStudentExamMarkColumnWise studentFinalExamMarkObj = studentExamMarkColumnWiseList.Where(d => d.StudentCourseHistoryId == studentCourseHistoryId && d.ExamTemplateItemId == finalMarkTemplateItem.ExamTemplateItemId).FirstOrDefault();

                    newRow = table.NewRow();
                    newRow.ItemArray = rowArray;
                    table.Rows.Add(newRow);
                }
            }
            return table;
        }


        public static List<rContinuousAssessmentMark> GetContinuousMarkBySectionID(int SectionId)
        {
            List<rContinuousAssessmentMark> list = RepositoryManager.ExamTemplate_Repository.GetContinuousMarkBySectionID(SectionId);
            return list;
        }
        

    }
}

