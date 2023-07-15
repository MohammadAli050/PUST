using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
//using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace EMS.Module.result.Report
{
    public partial class RPTExamMarkEntry : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        int userId = 0;
        //int employeeId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Page.Form.Enctype = "multipart/form-data";

            //base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            userId = userObj.Id;

            //UserInPerson userInPerson = UserInPersonManager.GetById(userId);
            //if (userInPerson != null)
            //{
            //    Employee emp = EmployeeManager.GetByPersonId(userInPerson.PersonID);
            //    if (emp != null)
            //    {
            //        employeeId = emp.EmployeeID;
            //    }
            //}

            if (!IsPostBack)
            {
                //ClearReportViewer();
                if (Request.QueryString["val"] != null)
                {
                    string[] paramArgs = Request.QueryString["val"].Split(new char[] { '_' });
                    int acaCalSectionId = int.Parse(paramArgs[0]);
                    int examSetupId = int.Parse(paramArgs[1]);
                    int firstSecondThirdTypeId = int.Parse(paramArgs[2]);
                    int examId = int.Parse(paramArgs[3]);
                    int employeeId = int.Parse(paramArgs[4]);
                    if (acaCalSectionId > 0 && examSetupId > 0 && firstSecondThirdTypeId > 0 && examId > 0 && employeeId>0)
                    {
                        LoadRdlcReport(acaCalSectionId, examSetupId, firstSecondThirdTypeId, examId,employeeId);
                    }
                    else
                    {
                        ClearReportViewer();
                    }
                }

            }
        }



        private void ClearReportViewer()
        {
            ReportDataSource rds = new ReportDataSource(null);
            ExamMarkEntryRV.LocalReport.DataSources.Clear();
            ExamMarkEntryRV.LocalReport.DataSources.Add(rds);
            ExamMarkEntryRV.Visible = false;
        }

        public void LoadRdlcReport(int acaCalSectionId, int examSetupId, int firstSecondThirdTypeId, int examId, int employeeId) // changed to public by HS, for use in BillManualEntry, int examTemplateItemId,
        {
            try
            {

                #region N/A
                //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                //{
                //    //MessageView("No student is found agaist this course or section !!", "fail");
                //    //return;
                //}


                //List<ExamMarkGridViewDTO> emgvDTOList = new List<ExamMarkGridViewDTO>();

                //foreach (var tData in stdCourseHistoryList)
                //{
                //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                //    Student s = StudentManager.GetById(tData.StudentID);
                //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(tData.ID, examTemplateItemId);

                //    if (s != null && em != null)
                //    {
                //        model.ExamMarkId = em.ID;
                //        model.Roll = s.Roll;
                //        model.Name = s.BasicInfo.FullName;
                //        try
                //        {
                //            if (!string.IsNullOrEmpty(em.FirstExaminerMark.ToString()))
                //            {
                //                model.Mark = Convert.ToDecimal(em.FirstExaminerMark);
                //            }
                //            else
                //            {
                //                model.Mark = 0.00M;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            model.Mark = 0.00M;
                //        }

                //        model.PresentAbsent = Convert.ToBoolean(em.IsAbsent) == true ? "Absent" : "Present";

                //        emgvDTOList.Add(model);
                //    }
                //} 
                #endregion
                bool excelLoopLimit = false;

                List<ExamMarkFirstSecondThirdDTO> list = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkFirstSecondThirdByAcaCalSectionIdFirstSecondThirdTypeId(acaCalSectionId, firstSecondThirdTypeId, examId, employeeId);

                if (list != null && list.Count > 0)
                {

                    string departmentName = string.Empty;
                    string courseCode = string.Empty;
                    string totalMark = string.Empty;
                    string courseTitle = string.Empty;
                    string teacherName = string.Empty;
                    string teacherDesignation = string.Empty;
                    string examName = string.Empty;
                    string session = string.Empty;
                    string firstSecondThirdName = string.Empty;

                    if (firstSecondThirdTypeId == 1)
                    {
                        firstSecondThirdName = "(First)";
                    }
                    else if (firstSecondThirdTypeId == 2)
                    {
                        firstSecondThirdName = "(Second)";
                    } 
                    else
                    {
                        firstSecondThirdName = "(Third)";
                    }

                    #region Get Header Info
                    ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                    if (emgvsiDTO != null)
                    {
                        departmentName = emgvsiDTO.DepartmentName;
                        courseCode = emgvsiDTO.CourseName; // +" (" + emgvsiDTO.SectionName + ")";
                        if (emgvsiDTO.ExamMark != null)
                        {
                            totalMark = Convert.ToInt32(emgvsiDTO.ExamMark).ToString();
                        }
                        else
                        {
                            totalMark = string.Empty;
                        }
                        courseTitle = emgvsiDTO.CourseTitle;
                        teacherName = emgvsiDTO.TeacherName;
                        teacherDesignation = emgvsiDTO.TeacherDesignation;
                        examName = emgvsiDTO.ExamName;
                        session = emgvsiDTO.Session;
                    }


                    #region Get Examiner Name

                    ExaminorSetups obj = ExaminorSetupsManager.GetByAcaCalSecId(acaCalSectionId);
                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    if (acs != null)
                    {
                        List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);


                        etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                        if (etiList == null || etiList.Count == 0)
                        {
                           // MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                            return;
                        }

                        if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                        {
                            excelLoopLimit = true;
                        }

                    }
                    else
                    {
                       // MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                        return;
                    }
                    int ExaminerId = 0;

                    if (obj != null)
                    {
                        if (firstSecondThirdTypeId == 1 && obj.FirstExaminer != null)
                            ExaminerId = Convert.ToInt32(obj.FirstExaminer);
                        if (firstSecondThirdTypeId == 2 && obj.SecondExaminor != null)
                            ExaminerId = Convert.ToInt32(obj.SecondExaminor);
                        if (firstSecondThirdTypeId == 3 && obj.ThirdExaminor != null)
                            ExaminerId = Convert.ToInt32(obj.ThirdExaminor);
                    }
                    if (ExaminerId > 0)
                    {
                        Employee emp = EmployeeManager.GetById(ExaminerId);
                        if (emp != null && emp.BasicInfo != null)
                        {
                            teacherName = emp.BasicInfo.FullName;
                            if (emp.EmployeeTypeInformation != null)
                                teacherDesignation = emp.EmployeeTypeInformation.EmployeTypeName;
                            if (emp.DepartmentInformation != null)
                                departmentName = emp.DepartmentInformation.Name;
                        }
                    }

                    #endregion


                    //if (emgvsiDTO != null)
                    //{
                    //    //lblFirstExaminerModalViewCourse.Text = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                    //    //lblFirstExaminerModalViewExam.Text = emgvsiDTO.ExamName;
                    //    //lblFirstExaminerModalViewTotalStudent.Text = emgvsiDTO.TotalStudentCount.ToString();
                    //    //lblFirstExaminerModalViewAbsentCount.Text = emgvsiDTO.AbsentCount.ToString();




                    //    List<ReportParameter> parameter = new List<ReportParameter>();
                    //    parameter.Add(new ReportParameter("date", DateTime.Now.ToString("dd-MM-yyyy")));
                    //    parameter.Add(new ReportParameter("ExamName", examName));
                    //    parameter.Add(new ReportParameter("department", departmentName));
                    //    parameter.Add(new ReportParameter("session", session));
                    //    parameter.Add(new ReportParameter("totalmark", totalMark));
                    //    parameter.Add(new ReportParameter("coursetitle", courseTitle));
                    //    parameter.Add(new ReportParameter("examinername", teacherName));
                    //    parameter.Add(new ReportParameter("designation", teacherDesignation));
                    //    this.ExamMarkEntryRV.LocalReport.SetParameters(parameter);
                    //}
                    #endregion

                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    ExamMarkEntryRV.Visible = true;
                    ReportParameter p1 = new ReportParameter("date", date);
                    ReportParameter p2 = new ReportParameter("ExamName", examName);
                    ReportParameter p3 = new ReportParameter("department", departmentName);
                    ReportParameter p4 = new ReportParameter("session", session);
                    ReportParameter p5 = new ReportParameter("totalmark", totalMark);
                    ReportParameter p6 = new ReportParameter("coursetitle", courseTitle);
                    ReportParameter p7 = new ReportParameter("examinername", teacherName);
                    ReportParameter p8 = new ReportParameter("designation", teacherDesignation);
                    ReportParameter p9 = new ReportParameter("coursecode", courseCode);
                    ReportParameter p10 = new ReportParameter("FirstSecondThirdName", firstSecondThirdName);


                    //ExamMarkEntryRV.LocalReport.DataSources.Clear();
                    //ExamMarkEntryRV.Reset();
                    if (excelLoopLimit)
                    {
                        ExamMarkEntryRV.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RPTExamMarkEntrySingle.rdlc");
                    }
                    else
                    {
                        ExamMarkEntryRV.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RPTExamMarkEntry.rdlc");
                    }
                   
                    ExamMarkEntryRV.LocalReport.EnableExternalImages = true;
                    this.ExamMarkEntryRV.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", list);
                    //ExamMarkEntryRV.LocalReport.DisplayName = "Waiver_Statistics_Report_"+DateTime.Now;
                    ExamMarkEntryRV.LocalReport.DataSources.Clear();
                    ExamMarkEntryRV.LocalReport.DataSources.Add(rds);

                    //ReportDataSource rds = new ReportDataSource("DataSet1", list);
                    //ExamMarkEntryRV.LocalReport.DataSources.Clear();
                    //ExamMarkEntryRV.LocalReport.DataSources.Add(rds);
                }
                else
                {
                    ClearReportViewer();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}