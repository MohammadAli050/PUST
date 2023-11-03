using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.result
{
    public partial class ExamMarkEntryNewVersion : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.ExamMarkEntryNewVersion);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.ExamMarkEntryNewVersion));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                lblSectionStatus.Text = "";
                btnLoadReport.Visible = false;
                btnFinalMarkReport.Visible = false;
                btnFinalMarkReport.Visible = false;
                btnFinalSubmitAll.Visible = false;
                divMark.Visible = false;
                Label3.Visible = false;
                Label2.Visible = false;
                EntryTimeDiv.Visible = false;
                ucDepartment.LoadDropDownByUserIdWithClassTakenAccess(UserObj.Id);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();


            }
        }

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("Select", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblUserType.Text = "";
                lblSectionStatus.Text = "";
                lblDeadLine.Text = "";
                EntryTimeDiv.Visible = false;
                btnLoadReport.Visible = false;
                btnFinalMarkReport.Visible = false;
                btnFinalSubmitAll.Visible = false;
                divMark.Visible = false;
                pnlExamMark.Visible = false;
                txtMark.Text = "";
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                ClearGridView();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblUserType.Text = "";
                lblSectionStatus.Text = "";
                lblDeadLine.Text = "";
                EntryTimeDiv.Visible = false;
                btnLoadReport.Visible = false;
                btnFinalMarkReport.Visible = false;
                btnFinalSubmitAll.Visible = false;
                divMark.Visible = false;
                pnlExamMark.Visible = false;
                txtMark.Text = "";
                LoadCourse();
                LoadHeldInInformation();
                ClearGridView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDeadLine.Text = "";
            lblUserType.Text = "";
            lblSectionStatus.Text = "";
            EntryTimeDiv.Visible = false;
            btnLoadReport.Visible = false;
            btnFinalMarkReport.Visible = false;
            btnFinalSubmitAll.Visible = false;
            divMark.Visible = false;
            pnlExamMark.Visible = false;
            txtMark.Text = "";
            LoadCourse();
            ClearGridView();
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDeadLine.Text = "";
            lblUserType.Text = "";
            lblSectionStatus.Text = "";
            EntryTimeDiv.Visible = false;
            btnLoadReport.Visible = false;
            btnFinalMarkReport.Visible = false;
            btnFinalSubmitAll.Visible = false;
            divMark.Visible = false;
            pnlExamMark.Visible = false;
            txtMark.Text = "";
            ClearGridView();
            CheckUserIsCourseTeacherORScriptExaminer();
            LoadExamTemplateItem();


            string course = ddlCourse.SelectedValue;
            string[] courseVersion = course.Split('_');
            int acaCalSectionId = Convert.ToInt32(courseVersion[2]);

            var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();


            #region View Section Status
            string msg = "";
            try
            {
                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)
                    msg = "এই Course টি Course শিক্ষক এখন ও submit করেন নি।";
                if (SectionStatusObj.StatusId == 1)
                    msg = "এই Course টি Course শিক্ষক submit করেছেন।";
                if (SectionStatusObj.StatusId == 2)
                    msg = "এই Course টি অভ্যন্তরীণ পরীক্ষক submit করেছেন।";
                if (SectionStatusObj.StatusId == 3)
                    msg = "এই Course টি Tabulator গণ submit করেছেন।";
                if (SectionStatusObj.StatusId == 4)
                    msg = "এই Course টি ৩য় পরীক্ষক submit করেছেন।";
            }
            catch (Exception ex)
            {
            }


            lblSectionStatus.Text = msg;

            #endregion

        }

        private void CheckUserIsCourseTeacherORScriptExaminer()
        {
            try
            {
                string course = ddlCourse.SelectedValue;

                int EmployeeId = 0;

                EmployeeId = GetEmployeeId();


                if (course != "0" && EmployeeId > 0)
                {
                    string[] courseVersion = course.Split('_');

                    int courseId = Convert.ToInt32(courseVersion[0]);
                    int versionId = Convert.ToInt32(courseVersion[1]);
                    int acaCalSection = Convert.ToInt32(courseVersion[2]);

                    var SectionObj = ucamContext.AcademicCalenderSections.Find(acaCalSection);

                    if (SectionObj != null && (SectionObj.TeacherOneID == EmployeeId || SectionObj.TeacherTwoID == EmployeeId || SectionObj.TeacherThreeID == EmployeeId))
                    {
                        EntryTimeDiv.Visible = true;
                        return;
                    }

                    var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();
                    if (ExaminerObj != null && ExaminerObj.InternalScriptExaminerId == EmployeeId)
                    {
                        EntryTimeDiv.Visible = true;
                    }

                    // added Later
                    if (ExaminerObj != null && ExaminerObj.ThirdExaminerId == EmployeeId)
                    {
                        EntryTimeDiv.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private int GetEmployeeId()
        {
            int EmployeeId = 0;

            try
            {
                #region Get Employee Id
                try
                {
                    User user = UserManager.GetByLogInId(UserObj.LogInID);
                    if (user != null)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);
                        if (empObj != null)
                        {
                            EmployeeId = empObj.EmployeeID;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion
            }
            catch (Exception ex)
            {
            }

            return EmployeeId;
        }

        private void LoadExamTemplateItem()
        {
            try
            {
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSection = Convert.ToInt32(courseVersion[2]);

                LoadExamItem(acaCalSection);
            }
            catch (Exception ex)
            {
            }
        }

        protected void LoadExamItem(int acaCalSection)
        {
            try
            {
                ddlContinousExam.Items.Clear();
                ddlContinousExam.Items.Add(new ListItem("-Select Exam-", "0"));
                ddlContinousExam.AppendDataBoundItems = true;
                AcademicCalenderSection acacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                if (acacalSectionObj != null)
                {
                    List<ExamTemplateItem> examList = ExamTemplateItemManager.GetByExamTemplateId(acacalSectionObj.BasicExamTemplateId).ToList();

                    int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                    int EmployeeId = GetEmployeeId();
                    bool CT = false, Ex = false, Tab = false;

                    #region Check Employee Is Course Teacher for This Course

                    if (acacalSectionObj.TeacherOneID == EmployeeId || acacalSectionObj.TeacherTwoID == EmployeeId || acacalSectionObj.TeacherThreeID == EmployeeId)
                    {
                        CT = true;
                    }
                    #endregion

                    #region Check Employee Is Examiner For This Course
                    var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();
                    if (ExaminerObj != null && ExaminerObj.InternalScriptExaminerId == EmployeeId)
                    {
                        Ex = true;
                    }
                    #endregion

                    #region Check Employee Is Tabulator
                    var TabulatorObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Where(x => x.HeldInProgramRelationId == HeldInId).FirstOrDefault();
                    if (TabulatorObj != null && (TabulatorObj.TabulatorOneId == EmployeeId || TabulatorObj.TabulatorTwoId == EmployeeId || TabulatorObj.TabulatorThreeId == EmployeeId))
                    {
                        Tab = true;
                    }
                    #endregion

                    var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acacalSectionObj.AcaCal_SectionID).FirstOrDefault();

                    int statusId = 1;
                    if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)
                        statusId = 0;


                    if (CT == true && statusId == 0)// Only Course Assessment For Course Teacher
                    {
                        examList = examList.Where(x => x.IsFinalExam == false).ToList();
                    }
                    else // Only Final Exam Mark Entry is available for Examiner And Tabulator
                    {
                        examList = examList.Where(x => x.IsFinalExam == true).ToList();
                    }

                    ddlContinousExam.DataSource = examList;
                    ddlContinousExam.DataValueField = "ExamTemplateItemId";
                    ddlContinousExam.DataTextField = "ExamName";
                    ddlContinousExam.DataBind();

                    if (CT == true)
                    {
                        btnLoadReport.Visible = true;
                    }

                    if (Ex == true || Tab == true)
                    {
                        btnFinalMarkReport.Visible = true;
                    }
                    else
                        btnFinalMarkReport.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }

        }
        protected void ddlContinousExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            pnlExamMark.Visible = false;
            //btnLoadReport.Visible = false;
            txtMark.Text = "";
            int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);
            ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
            if (examTemplateItemObj != null)
            {
                //if (examTemplateItemObj.IsFinalExam == false)
                //btnLoadReport.Visible = true;

                lblExamMark.Text = examTemplateItemObj.ExamName + " Total Mark : " + Convert.ToString(examTemplateItemObj.ExamMark);
            }
        }

        protected void LoadCourse()
        {
            try
            {
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0"));
                ddlCourse.AppendDataBoundItems = true;


                int HeldInRelationId = 0;
                HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInRelationId > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetAllByHeldInRelationId(HeldInRelationId);
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Role userRole = RoleManager.GetById(user.RoleID);

                    if (user.Person != null)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                        if (empObj != null && (userRole.RoleName != "IT Admin" || userRole.RoleName != "ESCL"))
                        {

                            // Get All Course By EmployeeId From Section Table(Course Teacher), Examiner Table(Internal Examiner), Tabulator Table(Tabulator 1,2,3)
                            List<SqlParameter> parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                            parameters1.Add(new SqlParameter { ParameterName = "@EmployeeId", SqlDbType = System.Data.SqlDbType.Int, Value = empObj.EmployeeID });

                            DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetAllCourseListByEmployeeIdAndHeldInId", parameters1);
                            if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                            {
                                ddlCourse.DataTextField = "CourseInfo";
                                ddlCourse.DataValueField = "CourseSectionId";
                                ddlCourse.DataSource = DataTableCourseList;
                                ddlCourse.DataBind();
                            }
                        }
                    }
                    else
                    {
                        if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                        {
                            acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                            foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                            {
                                ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.Course.CourseID + "_" + acaCalSec.Course.VersionID + "_" + acaCalSec.AcaCal_SectionID));
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch { }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //btnFinalSubmitAll.Visible = true;
                //ResultEntryGrid.Visible = true;
                //ReportViewer1.Visible = false;


                int DeptId = Convert.ToInt32(ucDepartment.selectedValue);

                if (DeptId == 0)
                {
                    showAlert("Please Choose a Department");
                    return;
                }
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                if (ProgramId == 0)
                {
                    showAlert("Please Choose a Program");
                    return;
                }

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);



                if (HeldInRelationId == 0)
                {
                    showAlert("Please Choose Semester & Held in");
                    return;
                }


                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');


                if (course == "0")
                {
                    showAlert("Please Choose Course");
                    return;
                }
                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersion[2]);
                int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);

                if (examTemplateItemId == 0)
                {
                    showAlert("Choose Mark Distribution");
                    return;
                }

                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                if (examTemplateItemObj != null)
                {
                    lblExamMark.Text = examTemplateItemObj.ExamName + " Total Mark : " + Convert.ToString(examTemplateItemObj.ExamMark);
                    lblExamTemplateItemId.Text = Convert.ToString(examTemplateItemObj.ExamTemplateItemId);

                    ExamMarkMaster examMarkMasterObj = ExamMarkMasterManager.GetByAcaCalSectionIdExamTemplateItemId(acaCalSectionId, examTemplateItemId);


                    if (examMarkMasterObj != null)
                    {
                        if (examMarkMasterObj.IsFinalSubmit == false)
                        {

                            LoadResultGrid(acaCalSectionId, examTemplateItemId, 0);
                        }
                        else
                        {
                            showAlert("Marks পরীক্ষা কমিটির কাছে submit করা হয়েছে");
                        }
                    }
                    else
                    {
                        LoadResultGrid(acaCalSectionId, examTemplateItemId, 0);

                    }
                }
                else
                {
                    ClearGridView();
                    showAlert("কোন data পাওয়া যায় নি");
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message.ToString());
            }
        }


        private void LoadResultGrid(int acaCalSectionId, int examTemplateItemId, int SingleSave)
        {
            try
            {
                int CourseType = 0;
                lblDeadLine.Text = "";



                LogicLayer.BusinessObjects.AcademicCalenderSection AcacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                if (AcacalSectionObj != null && AcacalSectionObj.Course != null && AcacalSectionObj.Course.TypeDefinitionID != null)
                    CourseType = Convert.ToInt32(AcacalSectionObj.Course.TypeDefinitionID);

                lblUserType.Text = "";
                lblUserType.Text = "";
                lblUserNo.Text = "0";

                string UserType = "You are entering marks as ";
                string UserNo = "0";

                int EmployeeId = GetEmployeeId();
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });
                parameters1.Add(new SqlParameter { ParameterName = "@ExamTemplateItemId", SqlDbType = System.Data.SqlDbType.Int, Value = examTemplateItemId });


                DataTable DataTableStudentList = new DataTable();

                var DeadLineObj = ucamContext.ExamMarkDateRangeSetups.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();

                string DeadLineMsg = "";

                bool IsDeadLineValid = false;
                int NotStartYet = 0, NotDeclareYet = 0;

                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();

                #region View Section Status
                string msg = "";
                try
                {
                    if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)
                        msg = "এই Course টি Course শিক্ষক এখন ও submit করেন নি।";
                    if (SectionStatusObj.StatusId == 1)
                        msg = "এই Course টি Course শিক্ষক submit করেছেন।";
                    if (SectionStatusObj.StatusId == 2)
                        msg = "এই Course টি অভ্যন্তরীণ পরীক্ষক submit করেছেন।";
                    if (SectionStatusObj.StatusId == 3)
                        msg = "এই Course টি Tabulator গণ submit করেছেন।";
                    if (SectionStatusObj.StatusId == 4)
                        msg = "এই Course টি ৩য় পরীক্ষক submit করেছেন।";
                }
                catch (Exception ex)
                {
                }


                lblSectionStatus.Text = msg;

                #endregion

                if (CourseType == 1) // Theory
                {
                    #region Check Section For Theory Course

                    if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // Enable For Course Teacher Entry
                    {
                        var acacalSectionObj = ucamContext.AcademicCalenderSections.Find(acaCalSectionId);

                        UserType = UserType + "Course Teacher";


                        #region Dead Line Checking

                        if (DeadLineObj != null)
                        {
                            if (DeadLineObj.CAStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.CAStartDate).Add(Convert.ToDateTime(DeadLineObj.CAStartTime).TimeOfDay))
                            {
                                NotStartYet = 1;
                            }
                            else
                                DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.CAStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.CAStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                    + Convert.ToDateTime(DeadLineObj.CAEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.CAEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                            bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.CAStartDate), Convert.ToDateTime(DeadLineObj.CAStartTime),
                                Convert.ToDateTime(DeadLineObj.CAEndDate), Convert.ToDateTime(DeadLineObj.CAEndTime));

                            if (IsValid)
                                IsDeadLineValid = true;
                            else
                                IsDeadLineValid = false;
                        }
                        else
                        {
                            NotDeclareYet = 1;
                        }


                        #endregion


                        if (acacalSectionObj != null && (acacalSectionObj.TeacherOneID == EmployeeId || acacalSectionObj.TeacherTwoID == EmployeeId || acacalSectionObj.TeacherThreeID == EmployeeId))
                        {

                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }

                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;
                                int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                                if (EntryNo == 1) // Get Data From First Time Entry Table 
                                {
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromCourseTeacherFirstTable", parameters1);
                                }
                                else
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromCourseTeacherSecondTable", parameters1);
                            }
                        }
                        else
                        {
                            var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();
                            if (ExaminerObj != null && ExaminerObj.InternalScriptExaminerId == EmployeeId)
                            {
                                showAlert("Course teacher এখনও submit করেন নি");
                                return;
                            }
                            else
                            {
                                showAlert("আপনি এই course এর course teacher নন");
                                return;
                            }
                        }

                    }

                    else if (SectionStatusObj.StatusId == 1) // Enable For Internal Examiner Mark Entry 
                    {
                        UserType = UserType + "Internal Examiner";
                        UserNo = "1";

                        var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();

                        if (ExaminerObj != null && ExaminerObj.InternalScriptExaminerId == EmployeeId)
                        {


                            #region Dead Line Checking

                            if (DeadLineObj != null)
                            {

                                if (DeadLineObj.FinalStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.FinalStartDate).Add(Convert.ToDateTime(DeadLineObj.FinalStartTime).TimeOfDay))
                                    NotStartYet = 1;
                                else
                                    DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.FinalStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                        + Convert.ToDateTime(DeadLineObj.FinalEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                                bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.FinalStartDate), Convert.ToDateTime(DeadLineObj.FinalStartTime),
                                    Convert.ToDateTime(DeadLineObj.FinalEndDate), Convert.ToDateTime(DeadLineObj.FinalEndTime));

                                if (IsValid)
                                    IsDeadLineValid = true;
                                else
                                    IsDeadLineValid = false;
                            }
                            else
                            {
                                NotDeclareYet = 1;
                            }

                            #endregion

                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }
                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Final Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;

                                int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                                #region Exam Attendance Checking

                                List<SqlParameter> parametersN = new List<SqlParameter>();
                                parametersN.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                                DataTable dtAttendanceObj = DataTableManager.GetDataFromQuery("GetExamAttendanceCount", parametersN);

                                if (dtAttendanceObj != null)
                                {
                                    int Result = 0;

                                    Result=Convert.ToInt32(dtAttendanceObj.Rows[0]["Result"]);

                                    // 2 = Attendance Not Given, 3 = Partially Given

                                    if(Result==2)
                                    {
                                        showAlert("এই course এর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন");
                                    }
                                    else if(Result==3)
                                    {
                                        showAlert("এই course এর কিছু শিক্ষার্থীর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন ");
                                    }
                                }

                                #endregion


                                if (EntryNo == 1) // Get Data From First Time Entry Table 
                                {
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromFirstExaminerFirstTable", parameters1);
                                }
                                else
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromFirstExaminerSecondTable", parameters1);
                            }
                        }
                        else
                        {
                            showAlert("আপনি এই course এর অভ্যন্তরীণ পরীক্ষক নন");
                            return;
                        }
                    }

                    else if (SectionStatusObj.StatusId == 2) // Enable For External Examiner (Tabulator 1, 2 and 3) Mark Entry 
                    {
                        int TabNo = 0;

                        #region Check Employee Is Tabulator

                        TabNo = GetTabulatorNo(HeldInId, EmployeeId);

                        if (TabNo > 0)
                        {
                            #region Dead Line Checking

                            if (DeadLineObj != null)
                            {
                                if (DeadLineObj.FinalStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.FinalStartDate).Add(Convert.ToDateTime(DeadLineObj.FinalStartTime).TimeOfDay))
                                    NotStartYet = 1;
                                else
                                    DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.FinalStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                        + Convert.ToDateTime(DeadLineObj.FinalEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                                bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.FinalStartDate), Convert.ToDateTime(DeadLineObj.FinalStartTime),
                                    Convert.ToDateTime(DeadLineObj.FinalEndDate), Convert.ToDateTime(DeadLineObj.FinalEndTime));

                                if (IsValid)
                                    IsDeadLineValid = true;
                                else
                                    IsDeadLineValid = false;
                            }
                            else
                            {
                                NotDeclareYet = 1;
                            }

                            #endregion

                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }
                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Final Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;

                                if (TabNo == 1)
                                {
                                    UserType = UserType + "Tabulator One (External Examiner)";
                                    UserNo = "2";
                                }
                                else if (TabNo == 2)
                                {
                                    UserType = UserType + "Tabulator Two (External Examiner)";
                                    UserNo = "3";
                                }
                                else if (TabNo == 3)
                                {
                                    UserType = UserType + "Tabulator Three (External Examiner)";
                                    UserNo = "4";
                                }
                                else
                                    UserType = UserType + "Tabulator";

                                parameters1.Add(new SqlParameter { ParameterName = "@TabulatorNo", SqlDbType = System.Data.SqlDbType.Int, Value = TabNo });
                                DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromSecondExaminerTable", parameters1);
                            }
                        }
                        else
                        {
                            showAlert("আপনি এই course এর Tabulator নন");
                            return;
                        }

                        #endregion

                    }

                    else if (SectionStatusObj.StatusId == 3) // Enable For Third Examiner Mark Entry 
                    {
                        int TabNo = 0;

                        var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();
                        if (ExaminerObj != null && ExaminerObj.ThirdExaminerId != null && ExaminerObj.ThirdExaminerId > 0)
                        {
                            #region Dead Line Checking

                            if (DeadLineObj != null)
                            {
                                if (DeadLineObj.FinalStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.FinalStartDate).Add(Convert.ToDateTime(DeadLineObj.FinalStartTime).TimeOfDay))
                                    NotStartYet = 1;
                                else
                                    DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.FinalStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                        + Convert.ToDateTime(DeadLineObj.FinalEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                                bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.FinalStartDate), Convert.ToDateTime(DeadLineObj.FinalStartTime),
                                    Convert.ToDateTime(DeadLineObj.FinalEndDate), Convert.ToDateTime(DeadLineObj.FinalEndTime));

                                if (IsValid)
                                    IsDeadLineValid = true;
                                else
                                    IsDeadLineValid = false;
                            }
                            else
                            {
                                NotDeclareYet = 1;
                            }

                            #endregion

                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }
                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Final Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;

                                // Third Examiner Condition

                                // Theory Course
                                // If Third Examiner is internal member then he/she enter marks
                                // If Third Examiner is external member then tabulator will enter marks

                                // Steps
                                // 1) Need to check third examiner is internal/external
                                // 2) If internal then need to save third examiner table
                                // 3) If External then need to save tabulator table

                                if (ExaminerObj.Attribute1 == 0) // Third Examiner Is Internal Member
                                {

                                    #region Check Employee Is Third Examiner

                                    if (ExaminerObj.ThirdExaminerId == EmployeeId)
                                    {

                                        UserType = UserType + "Third Examiner";
                                        UserNo = "5";
                                        int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                                        if (EntryNo == 1) // Get Data From First Time Entry Table 
                                        {
                                            DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromThirdExaminerFirstTable", parameters1);
                                        }
                                        else
                                            DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromThirdExaminerSecondTable", parameters1);
                                    }
                                    else
                                    {
                                        showAlert("আপনি এই course এর ৩য় পরীক্ষক নন");
                                        return;
                                    }

                                    #endregion
                                }
                                else// Third Examiner Is External Member
                                {
                                    #region Check Employee Is Tabulator
                                    TabNo = GetTabulatorNo(HeldInId, EmployeeId);


                                    if (TabNo > 0)
                                    {
                                        if (TabNo == 1)
                                        {
                                            UserType = UserType + "Tabulator One (Third Examiner)";
                                            UserNo = "6";
                                        }
                                        else if (TabNo == 2)
                                        {
                                            UserType = UserType + "Tabulator Two (Third Examiner)";
                                            UserNo = "7";
                                        }
                                        else if (TabNo == 3)
                                        {
                                            UserType = UserType + "Tabulator Three (Third Examiner)";
                                            UserNo = "8";
                                        }
                                        else
                                        {
                                            UserType = UserType + "Tabulator";
                                        }

                                        parameters1.Add(new SqlParameter { ParameterName = "@TabulatorNo", SqlDbType = System.Data.SqlDbType.Int, Value = TabNo });
                                        DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromThirdExaminerTable", parameters1);
                                    }

                                    else
                                    {
                                        showAlert("আপনি এই course এর Tabulator নন");
                                        return;
                                    }
                                    #endregion

                                }
                            }


                        }
                        else
                        {
                            showAlert("৩য় পরীক্ষক দেয়া হয় নি");
                            return;
                        }
                    }


                    #endregion
                }
                else if (CourseType == 2) // Lab
                {
                    #region Check Section For Lab Course

                    if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // Enable For Course Teacher Entry
                    {
                        var acacalSectionObj = ucamContext.AcademicCalenderSections.Find(acaCalSectionId);

                        UserType = UserType + "Course Teacher";


                        #region Dead Line Checking

                        if (DeadLineObj != null)
                        {
                            if (DeadLineObj.CAStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.CAStartDate).Add(Convert.ToDateTime(DeadLineObj.CAStartTime).TimeOfDay))
                                NotStartYet = 1;
                            else
                                DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.CAStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.CAStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                    + Convert.ToDateTime(DeadLineObj.CAEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.CAEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                            bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.CAStartDate), Convert.ToDateTime(DeadLineObj.CAStartTime),
                                Convert.ToDateTime(DeadLineObj.CAEndDate), Convert.ToDateTime(DeadLineObj.CAEndTime));

                            if (IsValid)
                                IsDeadLineValid = true;
                            else
                                IsDeadLineValid = false;
                        }
                        else
                        {
                            NotDeclareYet = 1;
                        }

                        #endregion



                        if (acacalSectionObj != null && (acacalSectionObj.TeacherOneID == EmployeeId || acacalSectionObj.TeacherTwoID == EmployeeId || acacalSectionObj.TeacherThreeID == EmployeeId))
                        {
                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }

                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Continuous Assessment Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;
                                int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                                if (EntryNo == 1) // Get Data From First Time Entry Table 
                                {
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromCourseTeacherFirstTable", parameters1);
                                }
                                else
                                    DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromCourseTeacherSecondTable", parameters1);
                            }
                        }
                        else
                        {
                            var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSectionId).FirstOrDefault();
                            if (ExaminerObj != null && ExaminerObj.InternalScriptExaminerId == EmployeeId)
                            {
                                showAlert("Course teacher এখনও submit করেন নি");
                                return;
                            }
                            else
                            {
                                showAlert("আপনি এই course এর course teacher নন");
                                return;
                            }
                        }

                    }
                    else if (SectionStatusObj.StatusId == 1) // Enable For (Tabulator 1, 2 and 3) Mark Entry 
                    {
                        int TabNo = 0;

                        #region Check Employee Is Tabulator

                        TabNo = GetTabulatorNo(HeldInId, EmployeeId);

                        if (TabNo > 0)
                        {
                            #region Dead Line Checking

                            if (DeadLineObj != null)
                            {
                                if (DeadLineObj.FinalStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.FinalStartDate).Add(Convert.ToDateTime(DeadLineObj.FinalStartTime).TimeOfDay))
                                    NotStartYet = 1;
                                else
                                    DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.FinalStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                        + DeadLineObj.FinalEndDate == null ? "" : Convert.ToDateTime(DeadLineObj.FinalEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                                bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.FinalStartDate), Convert.ToDateTime(DeadLineObj.FinalStartTime),
                                    Convert.ToDateTime(DeadLineObj.FinalEndDate), Convert.ToDateTime(DeadLineObj.FinalEndTime));

                                if (IsValid)
                                    IsDeadLineValid = true;
                                else
                                    IsDeadLineValid = false;
                            }
                            else
                            {
                                NotDeclareYet = 1;
                            }

                            #endregion

                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Final Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }
                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Final Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;
                                if (TabNo == 1)
                                {
                                    UserType = UserType + "Tabulator One";
                                    UserNo = "2";
                                }
                                else if (TabNo == 2)
                                {
                                    UserType = UserType + "Tabulator Two";
                                    UserNo = "3";
                                }
                                else if (TabNo == 3)
                                {
                                    UserType = UserType + "Tabulator Three";
                                    UserNo = "4";

                                }
                                else
                                    UserType = UserType + "Tabulator";


                                #region Exam Attendance Checking

                                List<SqlParameter> parametersN = new List<SqlParameter>();
                                parametersN.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                                DataTable dtAttendanceObj = DataTableManager.GetDataFromQuery("GetExamAttendanceCount", parametersN);

                                if (dtAttendanceObj != null)
                                {
                                    int Result = 0;

                                    Result = Convert.ToInt32(dtAttendanceObj.Rows[0]["Result"]);

                                    // 2 = Attendance Not Given, 3 = Partially Given

                                    if (Result == 2)
                                    {
                                        showAlert("এই course এর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন");
                                    }
                                    else if (Result == 3)
                                    {
                                        showAlert("এই course এর কিছু শিক্ষার্থীর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন ");
                                    }
                                }

                                #endregion


                                parameters1.Add(new SqlParameter { ParameterName = "@TabulatorNo", SqlDbType = System.Data.SqlDbType.Int, Value = TabNo });
                                DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromSecondExaminerTable", parameters1);
                            }
                        }
                        else
                        {
                            showAlert("আপনি এই course এর Tabulator নন");
                            return;
                        }

                        #endregion

                    }

                    #endregion
                }
                else if (CourseType == 3) // Viva
                {
                    #region Check Section For Viva Course

                    if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // Enable For (Tabulator 1, 2 and 3) Mark Entry 
                    {
                        int TabNo = 0;
                        #region Dead Line Checking

                        if (DeadLineObj != null)
                        {

                            if (DeadLineObj.FinalStartDate == null || Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.FinalStartDate).Add(Convert.ToDateTime(DeadLineObj.FinalStartTime).TimeOfDay))
                                NotStartYet = 1;
                            else
                                DeadLineMsg = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.FinalStartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalStartTime).ToString("hh:mm:ss tt") + " থেকে "
                                + Convert.ToDateTime(DeadLineObj.FinalEndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.FinalEndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                            bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.FinalStartDate), Convert.ToDateTime(DeadLineObj.FinalStartTime),
                                Convert.ToDateTime(DeadLineObj.FinalEndDate), Convert.ToDateTime(DeadLineObj.FinalEndTime));

                            if (IsValid)
                                IsDeadLineValid = true;
                            else
                                IsDeadLineValid = false;
                        }
                        else
                        {
                            NotDeclareYet = 1;
                        }

                        #endregion


                        #region Check Employee Is Tabulator

                        TabNo = GetTabulatorNo(HeldInId, EmployeeId);

                        if (TabNo > 0)
                        {
                            if (!IsDeadLineValid)
                            {
                                Label2.Visible = false;
                                Label3.Visible = false;
                                lblDeadLine.ForeColor = System.Drawing.Color.Red;
                                if (NotDeclareYet == 1)
                                    lblDeadLine.Text = "Viva Mark প্রবেশ এর সময়সীমা এখনও ঘোষণা করা হয় নি।";
                                else if (NotStartYet == 1)
                                    lblDeadLine.Text = "Viva Mark প্রবেশ এর সময়সীমা এখনও শুরু হয় নি। " + DeadLineMsg;
                                else
                                    lblDeadLine.Text = "Viva Mark প্রবেশ এর সময়সীমা শেষ। " + DeadLineMsg;
                            }
                            else
                            {
                                Label2.Visible = true;
                                Label3.Visible = true;
                                lblDeadLine.Text = "Final Mark প্রবেশ এর " + DeadLineMsg;
                                lblDeadLine.ForeColor = System.Drawing.Color.Blue;
                                if (TabNo == 1)
                                {
                                    UserType = UserType + "Tabulator One";
                                    UserNo = "2";
                                }
                                else if (TabNo == 2)
                                {
                                    UserType = UserType + "Tabulator Two";
                                    UserNo = "3";
                                }
                                else if (TabNo == 3)
                                {
                                    UserType = UserType + "Tabulator Three";
                                    UserNo = "4";
                                }
                                else
                                    UserType = UserType + "Tabulator";


                                #region Exam Attendance Checking

                                List<SqlParameter> parametersN = new List<SqlParameter>();
                                parametersN.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                                DataTable dtAttendanceObj = DataTableManager.GetDataFromQuery("GetExamAttendanceCount", parametersN);

                                if (dtAttendanceObj != null)
                                {
                                    int Result = 0;

                                    Result = Convert.ToInt32(dtAttendanceObj.Rows[0]["Result"]);

                                    // 2 = Attendance Not Given, 3 = Partially Given

                                    if (Result == 2)
                                    {
                                        showAlert("এই course এর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন");
                                    }
                                    else if (Result == 3)
                                    {
                                        showAlert("এই course এর কিছু শিক্ষার্থীর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক পরীক্ষা কমিটির সভাপতির সাথে যোগাযোগ করুন ");
                                    }
                                }

                                #endregion

                                parameters1.Add(new SqlParameter { ParameterName = "@TabulatorNo", SqlDbType = System.Data.SqlDbType.Int, Value = TabNo });
                                DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentListForMarkEntryFromSecondExaminerTable", parameters1);
                            }
                        }
                        else
                        {
                            showAlert("আপনি এই course এর Tabulator নন");
                            return;
                        }

                        #endregion

                    }

                    #endregion
                }

                lblUserType.Text = UserType;
                lblUserNo.Text = UserNo;

                if (DataTableStudentList != null && DataTableStudentList.Rows.Count > 0)
                {
                    Label3.Visible = true;
                    Label2.Visible = true;
                    ResultEntryGrid.DataSource = DataTableStudentList;
                    ResultEntryGrid.DataBind();
                    ResultEntryGrid.Visible = true;
                    //pnlTotalMark.Visible = true;
                    btnFinalSubmitAll.Visible = true;
                    divMark.Visible = true;
                }
                if (SingleSave == 0)
                    GridRebind(0, "");




            }
            catch (Exception ex)
            {
            }

        }

        private int GetTabulatorNo(int HeldInId, int EmployeeId)
        {
            int TabNo = 0;
            try
            {
                var TabulatorObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Where(x => x.HeldInProgramRelationId == HeldInId).FirstOrDefault();
                if (TabulatorObj != null)
                {
                    if (TabulatorObj.TabulatorOneId == EmployeeId)
                    {
                        TabNo = 1;
                    }
                    else if (TabulatorObj.TabulatorTwoId == EmployeeId)
                    {
                        TabNo = 2;
                    }
                    else if (TabulatorObj.TabulatorThreeId == EmployeeId)
                    {
                        TabNo = 3;
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return TabNo;
        }

        private void ClearGridView()
        {
            Label3.Visible = false;
            Label2.Visible = false;
            ResultEntryGrid.DataSource = null;
            ResultEntryGrid.DataBind();
            //pnlTotalMark.Visible = false;
            btnFinalSubmitAll.Visible = false;
            divMark.Visible = false;
            lblExamMark.Text = "";
            lblDeadLine.Text = "";
            lblUserNo.Text = "0";
        }
        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }



        private void GridRebind(int RebindAfterSave, string Roll)
        {
            if (ResultEntryGrid.Rows.Count > 0)
            {
                Label3.Visible = true;
                Label2.Visible = true;
            }

            int DiffStudentCount = 0;

            int dr = 0;

            for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
            {
                GridViewRow row = ResultEntryGrid.Rows[i];
                Label lblExamStatus = (Label)row.FindControl("lblExamStatus");

                Label lblDiff = (Label)row.FindControl("lblDiff");

                Label lblError = (Label)row.FindControl("lblError");

                Label lbldiffRoll = (Label)row.FindControl("lblStudentRoll");



                int diff = Convert.ToInt32(lblDiff.Text);

                TextBox mark = (TextBox)row.FindControl("txtMark");
                if (Convert.ToString(lblExamStatus.Text) == "2")
                {
                    mark.Text = "0";
                }
                if (diff == 1)
                {
                    //Label3.Visible = true;
                    ResultEntryGrid.Rows[i].BackColor = System.Drawing.Color.Yellow;
                    lblError.Text = "১ম ও ২য় এন্ট্রি অনুরূপ নয়";
                    lblError.Visible = true;

                    if (lbldiffRoll.Text == Roll)
                        dr = 1;

                    DiffStudentCount++;
                }
            }

            if (DiffStudentCount > 0)
            {
                string bengali_text1 = string.Concat(DiffStudentCount.ToString().Select(c => (char)('\u09E6' + c - '0'))); // "১২৩৪৫৬৭৮৯০"

                if (dr == 1 && DiffStudentCount == 1)
                    showAlert("এই শিক্ষার্থীর marks এর ১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করে আবার save করুন");
                else if (dr == 1 && DiffStudentCount > 1)
                {
                    bengali_text1 = string.Concat((DiffStudentCount - 1).ToString().Select(c => (char)('\u09E6' + c - '0'))); // "১২৩৪৫৬৭৮৯০"
                    showAlert("এই শিক্ষার্থী সহ আরও " + bengali_text1 + " জন শিক্ষার্থীর marks এর ১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করে আবার save করুন");
                }
                else
                    showAlert(bengali_text1 + " জন শিক্ষার্থীর marks এর ১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করে আবার save করুন");
                return;
            }

            if (RebindAfterSave == 1)
            {
                if (Roll == "")
                    showAlert("সকল শিক্ষার্থীর marks সফলভাবে save করা হয়েছে");
                else
                    showAlert("এই শিক্ষার্থীর marks সফলভাবে save করা হয়েছে");
                return;
            }


        }

        protected void btnFinalSubmitAll_Clicked(object sender, EventArgs e)
        {

            modalPopupFinalSubmit.Show();

            //lblMsg.Text = string.Empty;

        }

        private void TheoryCourseFinalSubmit(int acaCalSection)
        {
            try
            {

                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();

                var SubmissionDateObj = ucamContext.ExamMarkDateRangeSetups.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();

                #region Course Teacher Final Submission

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // This Section Is Submitted For Course Teacher
                {
                    //bool IsEntryOk = CheckFirstExaminerAllItemDoubleEntryGiven(acaCalSection);
                    bool IsEntryOk = CheckCourseTeacherAllItemDoubleEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("অনুগ্রহপূর্বক Marks submit করার জন্য সব ধরনের Continuous assessment marks ২ বার করে save করে নিন");
                        return;
                    }

                    //bool IsMatch = CheckFirstExaminerBothMarksMatched(acaCalSection);
                    bool IsMatch = CheckCourseTeacherBothMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {
                            SingleTempItem.IsFinalSubmit = true;
                            SingleTempItem.FinalSubmissionDate = DateTime.Now;
                            SingleTempItem.ModifiedBy = UserObj.Id;
                            SingleTempItem.ModifiedDate = DateTime.Now;

                            ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                            var DetailsList = ucamContext.CourseTeacherMarkDetailsFirstTimes.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {
                                    // Insert Mark Details Table

                                    #region Process


                                    var ExistingObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ExamMarkDetail NewObj = new DAL.ExamMarkDetail();

                                        NewObj.ExamMarkMasterId = MarkItem.ExamMarkMasterId;
                                        NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        NewObj.ExamStatus = MarkItem.ExamStatus;
                                        NewObj.Marks = MarkItem.Marks;
                                        NewObj.ConvertedMark = MarkItem.ConvertedMark;
                                        NewObj.IsFinalSubmit = true;
                                        NewObj.ExamMarkTypeId = 0;
                                        NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        NewObj.CreatedBy = UserObj.Id;
                                        NewObj.CreatedDate = DateTime.Now;
                                        NewObj.ModifiedBy = UserObj.Id;
                                        NewObj.ModifiedDate = DateTime.Now;

                                        ucamContext.ExamMarkDetails.Add(NewObj);
                                        ucamContext.SaveChanges();

                                        if (NewObj.ExamMarkDetailId > 0)
                                            Update++;
                                    }
                                    else
                                    {

                                        ExistingObj.ExamStatus = MarkItem.ExamStatus;
                                        ExistingObj.Marks = MarkItem.Marks;
                                        ExistingObj.ConvertedMark = MarkItem.ConvertedMark;
                                        ExistingObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        ExistingObj.ExamMarkTypeId = 0;
                                        ExistingObj.IsFinalSubmit = true;
                                        ExistingObj.ModifiedBy = UserObj.Id;
                                        ExistingObj.ModifiedDate = DateTime.Now;

                                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;
                                    }
                                    #endregion

                                }
                            }

                        }
                    }

                    if (Update > 0)
                    {
                        if (SectionStatusObj != null)
                        {
                            SectionStatusObj.StatusId = 1;// First Examiner Submitted
                            SectionStatusObj.ModifiedBy = UserObj.Id;
                            SectionStatusObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                        }
                        else
                        {
                            DAL.SectionWiseResultSubmissionStatu sectionStat = new DAL.SectionWiseResultSubmissionStatu();

                            sectionStat.AcacalSectionId = acaCalSection;
                            sectionStat.StatusId = 1; // First Examiner Submitted
                            sectionStat.CreatedBy = UserObj.Id;
                            sectionStat.CreatedDate = DateTime.Now;

                            ucamContext.SectionWiseResultSubmissionStatus.Add(sectionStat);
                            ucamContext.SaveChanges();

                        }

                        if (SubmissionDateObj == null)
                        {
                            DAL.ExamMarkDateRangeSetup NewObj = new DAL.ExamMarkDateRangeSetup();

                            NewObj.AcacalSectionId = acaCalSection;
                            NewObj.CourseTeacherSubmissionDate = DateTime.Now;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            ucamContext.ExamMarkDateRangeSetups.Add(NewObj);
                            ucamContext.SaveChanges();
                        }
                        else
                        {
                            SubmissionDateObj.CourseTeacherSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitCourseTeacherAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion
                }
                #endregion

                #region Internal (First) Examiner Final Submission

                else if (SectionStatusObj.StatusId == 1)// Course Teacher Already Submitted.This Section Is Submitted For First Examiner
                {
                    bool IsEntryOk = CheckFirstExaminerAllItemDoubleEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("অনুগ্রহপূর্বক Marks submit করার জন্য Final marks ২ বার করে save করে নিন");
                        return;
                    }

                    bool IsMatch = CheckFirstExaminerBothMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {

                            var DetailsList = ucamContext.FirstExaminerMarkDetailsFirstTimes.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {
                                    // Insert First, Second, Third Examiner Mark Details Table

                                    #region Process


                                    var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ExamMarkFirstSecondThirdExaminer NewObj = new DAL.ExamMarkFirstSecondThirdExaminer();

                                        NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        NewObj.FirstExaminerMark = MarkItem.Marks;
                                        NewObj.FirstExaminerMarkSubmissionStatus = 1;
                                        NewObj.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;
                                        NewObj.CreatedBy = UserObj.Id;
                                        NewObj.CreatedDate = DateTime.Now;
                                        NewObj.ModifiedBy = UserObj.Id;
                                        NewObj.ModifiedDate = DateTime.Now;

                                        ucamContext.ExamMarkFirstSecondThirdExaminers.Add(NewObj);
                                        ucamContext.SaveChanges();

                                        if (NewObj.ID > 0)
                                            Update++;
                                    }
                                    else
                                    {

                                        ExistingObj.FirstExaminerMark = MarkItem.Marks;
                                        ExistingObj.FirstExaminerMarkSubmissionStatus = 1;
                                        ExistingObj.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;
                                        ExistingObj.ModifiedBy = UserObj.Id;
                                        ExistingObj.ModifiedDate = DateTime.Now;


                                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;
                                    }
                                    #endregion

                                }
                            }

                        }
                    }

                    if (Update > 0)
                    {
                        if (SectionStatusObj != null && SectionStatusObj.StatusId == 1)
                        {
                            SectionStatusObj.StatusId = 2;// First Examiner Submitted
                            SectionStatusObj.ModifiedBy = UserObj.Id;
                            SectionStatusObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                        }
                        if (SubmissionDateObj != null)
                        {
                            SubmissionDateObj.InternalExaminerSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitFirstExaminerAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion

                }
                #endregion

                #region External (Second) Examiner Final Submission

                else if (SectionStatusObj.StatusId == 2)// First Examiner Already Submitted.This Section Is Submitted For Second Examiner (Tabulator One, Tabulator Two, Tabulator Three)
                {
                    bool IsEntryOk = CheckSecondExaminerAllTabulatorEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("আপনার marks গুলো সফলভাবে save করা হয়েছে। অন্য Tabulator গন submit করার পরে আপনার marks submit হবে ");
                        return;
                    }

                    bool IsMatch = CheckSecondExaminerAllTabulatorMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("দুঃখিত ! সব Tabulator গনের marks অনুরূপ হয় নি। অনুগ্রহপূর্বক সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0, StudentHasDiffGreaterThan20 = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {
                            bool IsFinalSubmit = false;
                            var DetailsList = ucamContext.SecondExaminerMarkDetails.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && x.TabulatorNo == 1 && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {
                                    // Insert First, Second, Third Examiner Mark Details Table

                                    #region Process

                                    var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ExamMarkFirstSecondThirdExaminer NewObj = new DAL.ExamMarkFirstSecondThirdExaminer();

                                        NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        NewObj.SecondExaminerMark = MarkItem.Marks;
                                        NewObj.SecondExaminerMarkSubmissionStatus = 1;
                                        NewObj.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;
                                        NewObj.CreatedBy = UserObj.Id;
                                        NewObj.CreatedDate = DateTime.Now;
                                        NewObj.ModifiedBy = UserObj.Id;
                                        NewObj.ModifiedDate = DateTime.Now;

                                        ucamContext.ExamMarkFirstSecondThirdExaminers.Add(NewObj);
                                        ucamContext.SaveChanges();

                                        if (NewObj.ID > 0)
                                            Update++;
                                    }
                                    else
                                    {

                                        ExistingObj.SecondExaminerMark = MarkItem.Marks;
                                        ExistingObj.SecondExaminerMarkSubmissionStatus = 1;
                                        ExistingObj.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;
                                        ExistingObj.ModifiedBy = UserObj.Id;
                                        ExistingObj.ModifiedDate = DateTime.Now;

                                        #region Mark Difference Calculation

                                        decimal MarkDiff = 0, TotalMark = Convert.ToDecimal(SingleTempItem.ExamMark);
                                        try
                                        {
                                            if (ExistingObj.FirstExaminerMark > 0 && ExistingObj.SecondExaminerMark > 0)
                                            {
                                                MarkDiff = Math.Abs(Convert.ToDecimal(ExistingObj.FirstExaminerMark) - Convert.ToDecimal(ExistingObj.SecondExaminerMark));
                                                if (MarkDiff != 0)
                                                {
                                                    MarkDiff = (MarkDiff / TotalMark) * 100;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        #endregion


                                        ExistingObj.MarkDifference = MarkDiff;

                                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;

                                        if (MarkDiff > 20)
                                            StudentHasDiffGreaterThan20++;
                                        else
                                        {
                                            #region Mark Saved In Exam Mark Details Table

                                            decimal Mark = Convert.ToDecimal(ExistingObj.FirstExaminerMark) + Convert.ToDecimal(ExistingObj.SecondExaminerMark);

                                            if (Mark != 0)
                                            {
                                                Mark = Mark / 2;
                                            }

                                            var MarkDetailsObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();
                                            if (MarkDetailsObj == null)
                                            {
                                                DAL.ExamMarkDetail DetailNewObj = new DAL.ExamMarkDetail();

                                                DetailNewObj.ExamMarkMasterId = SingleTempItem.ExamMarkMasterId;
                                                DetailNewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                                DetailNewObj.Marks = Mark;
                                                DetailNewObj.ConvertedMark = Mark;
                                                DetailNewObj.IsFinalSubmit = true;
                                                DetailNewObj.ExamMarkTypeId = 0;
                                                DetailNewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                                DetailNewObj.ExamStatus = MarkItem.ExamStatus;
                                                DetailNewObj.CreatedBy = UserObj.Id;
                                                DetailNewObj.CreatedDate = DateTime.Now;

                                                ucamContext.ExamMarkDetails.Add(DetailNewObj);
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }
                                            else
                                            {
                                                MarkDetailsObj.Marks = Mark;
                                                MarkDetailsObj.ConvertedMark = Mark;
                                                MarkDetailsObj.IsFinalSubmit = true;
                                                MarkDetailsObj.ModifiedBy = UserObj.Id;
                                                MarkDetailsObj.ModifiedDate = DateTime.Now;

                                                ucamContext.Entry(MarkDetailsObj).State = EntityState.Modified;
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }

                                            #endregion
                                        }
                                    }
                                    #endregion

                                }

                                #region Check If Third Examiner Is Required Then No need to do Final Submit. Else Need To Transfer Average Mark Into Exammark Details Table and do Final Submit
                                if (StudentHasDiffGreaterThan20 >= 0) // This section will go for third examiner. So do not need to do anything
                                {
                                    IsFinalSubmit = false;
                                }
                                else // This Section Is not eligible for third examiner.
                                {
                                    IsFinalSubmit = true;
                                }
                                #endregion
                            }

                            if (IsFinalSubmit)
                            {
                                SingleTempItem.IsFinalSubmit = true;
                                SingleTempItem.FinalSubmissionDate = DateTime.Now;
                                SingleTempItem.ModifiedBy = UserObj.Id;
                                SingleTempItem.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }

                        }
                    }

                    if (Update > 0)
                    {
                        #region Update Status Table
                        if (SectionStatusObj != null && SectionStatusObj.StatusId == 2)
                        {
                            if (StudentHasDiffGreaterThan20 > 0)
                            {
                                SectionStatusObj.StatusId = 3;// Second Examiner Submitted
                                SectionStatusObj.ModifiedBy = UserObj.Id;
                                SectionStatusObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }
                            else
                            {
                                SectionStatusObj.StatusId = 5;// Final Submitted
                                SectionStatusObj.ModifiedBy = UserObj.Id;
                                SectionStatusObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }

                        }
                        #endregion

                        if (SubmissionDateObj != null)
                        {
                            SubmissionDateObj.TabulatorSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }


                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitSecondExaminerAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion

                }
                #endregion

                #region Third Examiner Final Submission

                else if (SectionStatusObj.StatusId == 3)// External (Second) Examiner Already Submitted.This Section Is Submitted For Third Examiner (Tabulator One, Tabulator Two, Tabulator Three)
                {

                    int ThirdExaminerIsExternal = 0;
                    try
                    {
                        var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();
                        if (ExaminerObj != null && ExaminerObj.ThirdExaminerId != null && ExaminerObj.ThirdExaminerId > 0)
                        {
                            if (ExaminerObj.Attribute1 != null)
                                ThirdExaminerIsExternal = Convert.ToInt32(ExaminerObj.Attribute1);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (ThirdExaminerIsExternal == 0)
                    {

                        #region If Third Examiner Enter Marks

                        bool IsEntryOk = CheckThirdExaminerAllItemDoubleEntryGiven(acaCalSection);

                        if (!IsEntryOk)
                        {
                            showAlert("অনুগ্রহপূর্বক Marks submit করার জন্য Final marks ২ বার করে save করে নিন");
                            return;
                        }

                        bool IsMatch = CheckThirdExaminerBothMarksMatched(acaCalSection);

                        if (!IsMatch)
                        {

                            showAlert("১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করুন");
                            return;
                        }

                        #endregion

                        #region Final Submit Process

                        int Update = 0;

                        var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                        if (CAMarkList != null && CAMarkList.Any())
                        {
                            foreach (var SingleTempItem in CAMarkList)
                            {
                                bool IsFinalSubmit = false;

                                //if tabulator enter for third examiner
                                //var DetailsList = ucamContext.ThirdExaminerMarkDetails.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && x.TabulatorNo == 1 && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                                // if third examiner enter marks
                                var DetailsList = ucamContext.ThirdExaminerMarkDetailsFirstTimes.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();


                                if (DetailsList != null && DetailsList.Any())
                                {
                                    foreach (var MarkItem in DetailsList)
                                    {
                                        // Insert First, Second, Third Examiner Mark Details Table

                                        #region Process

                                        var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                        if (ExistingObj == null)
                                        {
                                            DAL.ExamMarkFirstSecondThirdExaminer NewObj = new DAL.ExamMarkFirstSecondThirdExaminer();

                                            NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                            NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                            NewObj.ThirdExaminerMark = MarkItem.Marks;
                                            NewObj.ThirdExaminerMarkSubmissionStatus = 1;
                                            NewObj.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            NewObj.CreatedBy = UserObj.Id;
                                            NewObj.CreatedDate = DateTime.Now;
                                            NewObj.ModifiedBy = UserObj.Id;
                                            NewObj.ModifiedDate = DateTime.Now;

                                            ucamContext.ExamMarkFirstSecondThirdExaminers.Add(NewObj);
                                            ucamContext.SaveChanges();

                                            if (NewObj.ID > 0)
                                                Update++;
                                        }
                                        else
                                        {

                                            ExistingObj.ThirdExaminerMark = MarkItem.Marks;
                                            ExistingObj.ThirdExaminerMarkSubmissionStatus = 1;
                                            ExistingObj.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            ExistingObj.ModifiedBy = UserObj.Id;
                                            ExistingObj.ModifiedDate = DateTime.Now;

                                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                            ucamContext.SaveChanges();

                                            Update++;
                                        }
                                        #endregion

                                    }

                                    #region Save Marks In Exam Mark Details.
                                    /// Logic 
                                    /// Get Difference of (First, Second),(Second, Third),(First, Third)
                                    /// Get Two marks in which minimum difference has exists
                                    /// If Difference Is Same then take maximum two marks
                                    /// Finally Average two marks and get actual final marks

                                    foreach (var TempItem in DetailsList)
                                    {
                                        #region Process

                                        var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == TempItem.CourseHistoryId && x.ExamTemplateItemId == TempItem.ExamTemplateItemId).FirstOrDefault();

                                        if (ExistingObj != null)
                                        {

                                            decimal Mark = getFinalMarkApplyingCondition(ExistingObj);// Convert.ToDecimal(ExistingObj.FirstExaminerMark) + Convert.ToDecimal(ExistingObj.SecondExaminerMark);

                                            if (Mark != 0)
                                            {
                                                Mark = Mark / 2;
                                            }

                                            var MarkDetailsObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == TempItem.CourseHistoryId && x.ExamTemplateItemId == TempItem.ExamTemplateItemId).FirstOrDefault();
                                            if (MarkDetailsObj == null)
                                            {
                                                DAL.ExamMarkDetail DetailNewObj = new DAL.ExamMarkDetail();

                                                DetailNewObj.ExamMarkMasterId = SingleTempItem.ExamMarkMasterId;
                                                DetailNewObj.CourseHistoryId = TempItem.CourseHistoryId;
                                                DetailNewObj.Marks = Mark;
                                                DetailNewObj.ConvertedMark = Mark;
                                                DetailNewObj.IsFinalSubmit = true;
                                                DetailNewObj.ExamMarkTypeId = 0;
                                                DetailNewObj.ExamTemplateItemId = TempItem.ExamTemplateItemId;
                                                DetailNewObj.ExamStatus = TempItem.ExamStatus;
                                                DetailNewObj.CreatedBy = UserObj.Id;
                                                DetailNewObj.CreatedDate = DateTime.Now;

                                                ucamContext.ExamMarkDetails.Add(DetailNewObj);
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }
                                            else
                                            {
                                                MarkDetailsObj.Marks = Mark;
                                                MarkDetailsObj.ConvertedMark = Mark;
                                                MarkDetailsObj.IsFinalSubmit = true;
                                                MarkDetailsObj.ModifiedBy = UserObj.Id;
                                                MarkDetailsObj.ModifiedDate = DateTime.Now;

                                                ucamContext.Entry(MarkDetailsObj).State = EntityState.Modified;
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }


                                        }
                                        #endregion

                                    }
                                    #endregion
                                }

                                if (IsFinalSubmit)
                                {
                                    SingleTempItem.IsFinalSubmit = true;
                                    SingleTempItem.FinalSubmissionDate = DateTime.Now;
                                    SingleTempItem.ModifiedBy = UserObj.Id;
                                    SingleTempItem.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                                    ucamContext.SaveChanges();
                                }

                            }
                        }

                        if (Update > 0)
                        {
                            #region Update Status Table
                            if (SectionStatusObj != null && SectionStatusObj.StatusId == 3)
                            {
                                SectionStatusObj.StatusId = 5;// Final Submitted
                                SectionStatusObj.ModifiedBy = UserObj.Id;
                                SectionStatusObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }
                            #endregion

                            if (SubmissionDateObj != null)
                            {
                                SubmissionDateObj.ThirdExaminerSubmissionDate = DateTime.Now;
                                SubmissionDateObj.ModifiedBy = UserObj.Id;
                                SubmissionDateObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }

                            List<SqlParameter> parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                            parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                            DataTableManager.GetDataFromQuery("FinalSubmitThirdExaminerAllDetailsMark", parameters1);

                            showAlert("Marks সফলভাবে submit করা হয়েছে");
                            ClearGridView();
                        }


                        #endregion

                    }
                    else
                    {
                        #region If Tabulator will Enter Marks For Third Examiner


                        bool IsEntryOk = CheckThirdExaminerAllTabulatorEntryGiven(acaCalSection);

                        if (!IsEntryOk)
                        {
                            showAlert("আপনার marks গুলো সফলভাবে save করা হয়েছে। অন্য Tabulator গন submit করার পরে আপনার marks submit হবে");
                            return;
                        }

                        bool IsMatch = CheckThirdExaminerAllTabulatorMarksMatched(acaCalSection);

                        if (!IsMatch)
                        {

                            showAlert("দুঃখিত ! সব Tabulator গনের marks অনুরূপ হয় নি। অনুগ্রহপূর্বক সঠিক করুন");
                            return;
                        }


                        #endregion

                        #region Final Submit Process

                        int Update = 0;

                        var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                        if (CAMarkList != null && CAMarkList.Any())
                        {
                            foreach (var SingleTempItem in CAMarkList)
                            {
                                bool IsFinalSubmit = false;

                                //if tabulator enter for third examiner
                                var DetailsList = ucamContext.ThirdExaminerMarkDetails.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && x.TabulatorNo == 1 && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                                // if third examiner enter marks
                                //var DetailsList = ucamContext.ThirdExaminerMarkDetailsFirstTimes.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();


                                if (DetailsList != null && DetailsList.Any())
                                {
                                    foreach (var MarkItem in DetailsList)
                                    {
                                        // Insert First, Second, Third Examiner Mark Details Table

                                        #region Process

                                        var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                        if (ExistingObj == null)
                                        {
                                            DAL.ExamMarkFirstSecondThirdExaminer NewObj = new DAL.ExamMarkFirstSecondThirdExaminer();

                                            NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                            NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                            NewObj.ThirdExaminerMark = MarkItem.Marks;
                                            NewObj.ThirdExaminerMarkSubmissionStatus = 1;
                                            NewObj.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            NewObj.CreatedBy = UserObj.Id;
                                            NewObj.CreatedDate = DateTime.Now;
                                            NewObj.ModifiedBy = UserObj.Id;
                                            NewObj.ModifiedDate = DateTime.Now;

                                            ucamContext.ExamMarkFirstSecondThirdExaminers.Add(NewObj);
                                            ucamContext.SaveChanges();

                                            if (NewObj.ID > 0)
                                                Update++;
                                        }
                                        else
                                        {

                                            ExistingObj.ThirdExaminerMark = MarkItem.Marks;
                                            ExistingObj.ThirdExaminerMarkSubmissionStatus = 1;
                                            ExistingObj.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            ExistingObj.ModifiedBy = UserObj.Id;
                                            ExistingObj.ModifiedDate = DateTime.Now;

                                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                            ucamContext.SaveChanges();

                                            Update++;
                                        }
                                        #endregion

                                    }

                                    #region Save Marks In Exam Mark Details.
                                    /// Logic 
                                    /// Get Difference of (First, Second),(Second, Third),(First, Third)
                                    /// Get Two marks in which minimum difference has exists
                                    /// If Difference Is Same then take maximum two marks
                                    /// Finally Average two marks and get actual final marks

                                    foreach (var TempItem in DetailsList)
                                    {
                                        #region Process

                                        var ExistingObj = ucamContext.ExamMarkFirstSecondThirdExaminers.Where(x => x.CourseHistoryId == TempItem.CourseHistoryId && x.ExamTemplateItemId == TempItem.ExamTemplateItemId).FirstOrDefault();

                                        if (ExistingObj != null)
                                        {

                                            decimal Mark = getFinalMarkApplyingCondition(ExistingObj);// Convert.ToDecimal(ExistingObj.FirstExaminerMark) + Convert.ToDecimal(ExistingObj.SecondExaminerMark);

                                            if (Mark != 0)
                                            {
                                                Mark = Mark / 2;
                                            }

                                            var MarkDetailsObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == TempItem.CourseHistoryId && x.ExamTemplateItemId == TempItem.ExamTemplateItemId).FirstOrDefault();
                                            if (MarkDetailsObj == null)
                                            {
                                                DAL.ExamMarkDetail DetailNewObj = new DAL.ExamMarkDetail();

                                                DetailNewObj.ExamMarkMasterId = SingleTempItem.ExamMarkMasterId;
                                                DetailNewObj.CourseHistoryId = TempItem.CourseHistoryId;
                                                DetailNewObj.Marks = Mark;
                                                DetailNewObj.ConvertedMark = Mark;
                                                DetailNewObj.IsFinalSubmit = true;
                                                DetailNewObj.ExamMarkTypeId = 0;
                                                DetailNewObj.ExamTemplateItemId = TempItem.ExamTemplateItemId;
                                                DetailNewObj.ExamStatus = TempItem.ExamStatus;
                                                DetailNewObj.CreatedBy = UserObj.Id;
                                                DetailNewObj.CreatedDate = DateTime.Now;

                                                ucamContext.ExamMarkDetails.Add(DetailNewObj);
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }
                                            else
                                            {
                                                MarkDetailsObj.Marks = Mark;
                                                MarkDetailsObj.ConvertedMark = Mark;
                                                MarkDetailsObj.IsFinalSubmit = true;
                                                MarkDetailsObj.ModifiedBy = UserObj.Id;
                                                MarkDetailsObj.ModifiedDate = DateTime.Now;

                                                ucamContext.Entry(MarkDetailsObj).State = EntityState.Modified;
                                                ucamContext.SaveChanges();

                                                IsFinalSubmit = true;
                                            }


                                        }
                                        #endregion

                                    }
                                    #endregion
                                }

                                if (IsFinalSubmit)
                                {
                                    SingleTempItem.IsFinalSubmit = true;
                                    SingleTempItem.FinalSubmissionDate = DateTime.Now;
                                    SingleTempItem.ModifiedBy = UserObj.Id;
                                    SingleTempItem.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                                    ucamContext.SaveChanges();
                                }

                            }
                        }

                        if (Update > 0)
                        {
                            #region Update Status Table
                            if (SectionStatusObj != null && SectionStatusObj.StatusId == 3)
                            {
                                SectionStatusObj.StatusId = 5;// Final Submitted
                                SectionStatusObj.ModifiedBy = UserObj.Id;
                                SectionStatusObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }
                            #endregion

                            if (SubmissionDateObj != null)
                            {
                                SubmissionDateObj.ThirdExaminerSubmissionDate = DateTime.Now;
                                SubmissionDateObj.ModifiedBy = UserObj.Id;
                                SubmissionDateObj.ModifiedDate = DateTime.Now;
                                ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }

                            List<SqlParameter> parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                            parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                            DataTableManager.GetDataFromQuery("FinalSubmitThirdExaminerAllDetailsMark", parameters1);

                            showAlert("Marks সফলভাবে submit করা হয়েছে");
                            ClearGridView();
                        }


                        #endregion

                    }


                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        private void LabCourseFinalSubmit(int acaCalSection)
        {
            try
            {
                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();

                var SubmissionDateObj = ucamContext.ExamMarkDateRangeSetups.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();


                #region Course Teacher Final Submission

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // This Section Is Submitted For Course Teacher
                {
                    bool IsEntryOk = CheckCourseTeacherAllItemDoubleEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("অনুগ্রহপূর্বক Marks submit করার জন্য সব ধরনের Continuous assessment marks ২ বার করে save করে নিন");
                        return;
                    }

                    //bool IsMatch = CheckFirstExaminerBothMarksMatched(acaCalSection);
                    bool IsMatch = CheckCourseTeacherBothMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("১ম ও ২য় এন্ট্রি অনুরূপ নয়। অনুগ্রহপূর্বক ১ম ও ২য় এন্ট্রিকৃত marks সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {
                            SingleTempItem.IsFinalSubmit = true;
                            SingleTempItem.FinalSubmissionDate = DateTime.Now;
                            SingleTempItem.ModifiedBy = UserObj.Id;
                            SingleTempItem.ModifiedDate = DateTime.Now;

                            ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                            var DetailsList = ucamContext.CourseTeacherMarkDetailsFirstTimes.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {
                                    // Insert Mark Details Table

                                    #region Process


                                    var ExistingObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ExamMarkDetail NewObj = new DAL.ExamMarkDetail();

                                        NewObj.ExamMarkMasterId = MarkItem.ExamMarkMasterId;
                                        NewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        NewObj.ExamStatus = MarkItem.ExamStatus;
                                        NewObj.Marks = MarkItem.Marks;
                                        NewObj.ConvertedMark = MarkItem.ConvertedMark;
                                        NewObj.IsFinalSubmit = true;
                                        NewObj.ExamMarkTypeId = 0;
                                        NewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        NewObj.CreatedBy = UserObj.Id;
                                        NewObj.CreatedDate = DateTime.Now;
                                        NewObj.ModifiedBy = UserObj.Id;
                                        NewObj.ModifiedDate = DateTime.Now;

                                        ucamContext.ExamMarkDetails.Add(NewObj);
                                        ucamContext.SaveChanges();

                                        if (NewObj.ExamMarkDetailId > 0)
                                            Update++;
                                    }
                                    else
                                    {

                                        ExistingObj.ExamStatus = MarkItem.ExamStatus;
                                        ExistingObj.Marks = MarkItem.Marks;
                                        ExistingObj.ConvertedMark = MarkItem.ConvertedMark;
                                        ExistingObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        ExistingObj.ExamMarkTypeId = 0;
                                        ExistingObj.IsFinalSubmit = true;
                                        ExistingObj.ModifiedBy = UserObj.Id;
                                        ExistingObj.ModifiedDate = DateTime.Now;

                                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;
                                    }
                                    #endregion

                                }
                            }

                        }
                    }

                    if (Update > 0)
                    {
                        if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)
                        {
                            DAL.SectionWiseResultSubmissionStatu sectionStat = new DAL.SectionWiseResultSubmissionStatu();

                            sectionStat.AcacalSectionId = acaCalSection;
                            sectionStat.StatusId = 1; // First Examiner Submitted
                            sectionStat.CreatedBy = UserObj.Id;
                            sectionStat.CreatedDate = DateTime.Now;

                            ucamContext.SectionWiseResultSubmissionStatus.Add(sectionStat);
                            ucamContext.SaveChanges();

                        }
                        else
                        {
                            SectionStatusObj.StatusId = 1;// Course Teacher Submitted
                            SectionStatusObj.ModifiedBy = UserObj.Id;
                            SectionStatusObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        if (SubmissionDateObj == null)
                        {
                            DAL.ExamMarkDateRangeSetup NewObj = new DAL.ExamMarkDateRangeSetup();

                            NewObj.AcacalSectionId = acaCalSection;
                            NewObj.CourseTeacherSubmissionDate = DateTime.Now;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            ucamContext.ExamMarkDateRangeSetups.Add(NewObj);
                            ucamContext.SaveChanges();
                        }
                        else
                        {
                            SubmissionDateObj.CourseTeacherSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitCourseTeacherAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion
                }
                #endregion

                #region Tabulator Final Submission

                else if (SectionStatusObj.StatusId == 1)// Course Teacher Already Submitted.This Section Is Submitted For Tabulator One, Tabulator Two, Tabulator Three
                {
                    bool IsEntryOk = CheckSecondExaminerAllTabulatorEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("আপনার marks গুলো সফলভাবে save করা হয়েছে। অন্য Tabulator গন submit করার পরে আপনার marks submit হবে ");
                        return;
                    }

                    bool IsMatch = CheckSecondExaminerAllTabulatorMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("দুঃখিত ! সব Tabulator গনের marks অনুরূপ হয় নি। অনুগ্রহপূর্বক সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {
                            var DetailsList = ucamContext.SecondExaminerMarkDetails.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && x.TabulatorNo == 1 && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {

                                    var MarkDetailsObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();
                                    if (MarkDetailsObj == null)
                                    {
                                        DAL.ExamMarkDetail DetailNewObj = new DAL.ExamMarkDetail();

                                        DetailNewObj.ExamMarkMasterId = SingleTempItem.ExamMarkMasterId;
                                        DetailNewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        DetailNewObj.Marks = MarkItem.Marks;
                                        DetailNewObj.ConvertedMark = MarkItem.ConvertedMark;
                                        DetailNewObj.IsFinalSubmit = true;
                                        DetailNewObj.ExamMarkTypeId = 0;
                                        DetailNewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        DetailNewObj.ExamStatus = MarkItem.ExamStatus;
                                        DetailNewObj.CreatedBy = UserObj.Id;
                                        DetailNewObj.CreatedDate = DateTime.Now;

                                        ucamContext.ExamMarkDetails.Add(DetailNewObj);
                                        ucamContext.SaveChanges();

                                        Update++;

                                    }
                                    else
                                    {
                                        MarkDetailsObj.Marks = MarkItem.Marks;
                                        MarkDetailsObj.ConvertedMark = MarkItem.ConvertedMark;
                                        MarkDetailsObj.IsFinalSubmit = true;
                                        MarkDetailsObj.ModifiedBy = UserObj.Id;
                                        MarkDetailsObj.ModifiedDate = DateTime.Now;

                                        ucamContext.Entry(MarkDetailsObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;

                                    }

                                }

                            }

                            SingleTempItem.IsFinalSubmit = true;
                            SingleTempItem.FinalSubmissionDate = DateTime.Now;
                            SingleTempItem.ModifiedBy = UserObj.Id;
                            SingleTempItem.ModifiedDate = DateTime.Now;

                            ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                        }
                    }

                    if (Update > 0)
                    {
                        #region Update Status Table
                        if (SectionStatusObj != null && SectionStatusObj.StatusId == 1)
                        {
                            SectionStatusObj.StatusId = 5;// Final Submitted
                            SectionStatusObj.ModifiedBy = UserObj.Id;
                            SectionStatusObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                        }
                        #endregion

                        if (SubmissionDateObj != null)
                        {
                            SubmissionDateObj.TabulatorSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitSecondExaminerAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion

                }
                #endregion

            }
            catch (Exception ex)
            {
            }
        }

        private void VivaCourseFinalSubmit(int acaCalSection)
        {
            try
            {
                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();

                var SubmissionDateObj = ucamContext.ExamMarkDateRangeSetups.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();


                #region Tabulator Final Submission

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)// This Section Is Submitted For Tabulator One, Tabulator Two, Tabulator Three
                {
                    bool IsEntryOk = CheckSecondExaminerAllTabulatorEntryGiven(acaCalSection);

                    if (!IsEntryOk)
                    {
                        showAlert("আপনার marks গুলো সফলভাবে save করা হয়েছে। অন্য Tabulator গন submit করার পরে আপনার marks submit হবে ");
                        return;
                    }

                    bool IsMatch = CheckSecondExaminerAllTabulatorMarksMatched(acaCalSection);

                    if (!IsMatch)
                    {

                        showAlert("দুঃখিত ! সব Tabulator গনের marks অনুরূপ হয় নি। অনুগ্রহপূর্বক সঠিক করুন");
                        return;
                    }

                    #region Final Submit Process

                    int Update = 0;

                    var CAMarkList = ucamContext.ExamMarkMasters.Where(x => x.AcaCalSectionId == acaCalSection && (x.IsFinalSubmit == null || x.IsFinalSubmit == false)).ToList();

                    if (CAMarkList != null && CAMarkList.Any())
                    {
                        foreach (var SingleTempItem in CAMarkList)
                        {
                            var DetailsList = ucamContext.SecondExaminerMarkDetails.Where(x => x.ExamMarkMasterId == SingleTempItem.ExamMarkMasterId && x.TabulatorNo == 1 && ((x.IsFinalSubmit == null || x.IsFinalSubmit == false))).ToList();

                            if (DetailsList != null && DetailsList.Any())
                            {
                                foreach (var MarkItem in DetailsList)
                                {

                                    var MarkDetailsObj = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == MarkItem.CourseHistoryId && x.ExamTemplateItemId == MarkItem.ExamTemplateItemId).FirstOrDefault();
                                    if (MarkDetailsObj == null)
                                    {
                                        DAL.ExamMarkDetail DetailNewObj = new DAL.ExamMarkDetail();

                                        DetailNewObj.ExamMarkMasterId = SingleTempItem.ExamMarkMasterId;
                                        DetailNewObj.CourseHistoryId = MarkItem.CourseHistoryId;
                                        DetailNewObj.Marks = MarkItem.Marks;
                                        DetailNewObj.ConvertedMark = MarkItem.ConvertedMark;
                                        DetailNewObj.IsFinalSubmit = true;
                                        DetailNewObj.ExamMarkTypeId = 0;
                                        DetailNewObj.ExamTemplateItemId = MarkItem.ExamTemplateItemId;
                                        DetailNewObj.ExamStatus = MarkItem.ExamStatus;
                                        DetailNewObj.CreatedBy = UserObj.Id;
                                        DetailNewObj.CreatedDate = DateTime.Now;

                                        ucamContext.ExamMarkDetails.Add(DetailNewObj);
                                        ucamContext.SaveChanges();

                                        Update++;

                                    }
                                    else
                                    {
                                        MarkDetailsObj.Marks = MarkItem.Marks;
                                        MarkDetailsObj.ConvertedMark = MarkItem.ConvertedMark;
                                        MarkDetailsObj.IsFinalSubmit = true;
                                        MarkDetailsObj.ModifiedBy = UserObj.Id;
                                        MarkDetailsObj.ModifiedDate = DateTime.Now;

                                        ucamContext.Entry(MarkDetailsObj).State = EntityState.Modified;
                                        ucamContext.SaveChanges();

                                        Update++;

                                    }

                                }

                            }

                            SingleTempItem.IsFinalSubmit = true;
                            SingleTempItem.FinalSubmissionDate = DateTime.Now;
                            SingleTempItem.ModifiedBy = UserObj.Id;
                            SingleTempItem.ModifiedDate = DateTime.Now;

                            ucamContext.Entry(SingleTempItem).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                        }
                    }

                    if (Update > 0)
                    {
                        #region Update Status Table
                        if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)
                        {
                            DAL.SectionWiseResultSubmissionStatu sectionStat = new DAL.SectionWiseResultSubmissionStatu();

                            sectionStat.AcacalSectionId = acaCalSection;
                            sectionStat.StatusId = 5; // Tabulator Submitted
                            sectionStat.CreatedBy = UserObj.Id;
                            sectionStat.CreatedDate = DateTime.Now;

                            ucamContext.SectionWiseResultSubmissionStatus.Add(sectionStat);
                            ucamContext.SaveChanges();

                        }
                        else
                        {
                            SectionStatusObj.StatusId = 5;// Tabulator Submitted
                            SectionStatusObj.ModifiedBy = UserObj.Id;
                            SectionStatusObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SectionStatusObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }
                        #endregion


                        if (SubmissionDateObj == null)
                        {
                            DAL.ExamMarkDateRangeSetup NewObj = new DAL.ExamMarkDateRangeSetup();

                            NewObj.AcacalSectionId = acaCalSection;
                            NewObj.TabulatorSubmissionDate = DateTime.Now;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            ucamContext.ExamMarkDateRangeSetups.Add(NewObj);
                            ucamContext.SaveChanges();
                        }
                        else
                        {
                            SubmissionDateObj.TabulatorSubmissionDate = DateTime.Now;
                            SubmissionDateObj.ModifiedBy = UserObj.Id;
                            SubmissionDateObj.ModifiedDate = DateTime.Now;
                            ucamContext.Entry(SubmissionDateObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();
                        }

                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });
                        parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.Id });

                        DataTableManager.GetDataFromQuery("FinalSubmitSecondExaminerAllDetailsMark", parameters1);

                        showAlert("Marks সফলভাবে submit করা হয়েছে");
                        ClearGridView();
                    }


                    #endregion

                }
                #endregion

            }
            catch (Exception ex)
            {
            }
        }





        private decimal getFinalMarkApplyingCondition(DAL.ExamMarkFirstSecondThirdExaminer ExistingObj)
        {
            decimal Mark = 0;
            try
            {
                /// Logic 
                /// Get Difference of (First, Second),(Second, Third),(First, Third)
                /// Get Two marks in which minimum difference has exists
                /// If Difference Is Same then take maximum two marks
                /// Finally Average two marks and get actual final marks

                decimal Diff1_2 = 0, Diff2_3 = 0, Diff1_3 = 0;

                if (ExistingObj != null)
                {
                    Diff1_2 = Math.Abs(Convert.ToDecimal(ExistingObj.FirstExaminerMark) - Convert.ToDecimal(ExistingObj.SecondExaminerMark));
                    Diff2_3 = Math.Abs(Convert.ToDecimal(ExistingObj.SecondExaminerMark) - Convert.ToDecimal(ExistingObj.ThirdExaminerMark));
                    Diff1_3 = Math.Abs(Convert.ToDecimal(ExistingObj.FirstExaminerMark) - Convert.ToDecimal(ExistingObj.ThirdExaminerMark));

                    List<Difference> diffList = new List<Difference>();

                    Difference NewObj = new Difference();
                    NewObj.Diff = Diff1_2;
                    NewObj.FirstMark = Convert.ToDecimal(ExistingObj.FirstExaminerMark);
                    NewObj.SecondMark = Convert.ToDecimal(ExistingObj.SecondExaminerMark);
                    diffList.Add(NewObj);

                    Difference NewObj2 = new Difference();
                    NewObj2.Diff = Diff2_3;
                    NewObj2.FirstMark = Convert.ToDecimal(ExistingObj.SecondExaminerMark);
                    NewObj2.SecondMark = Convert.ToDecimal(ExistingObj.ThirdExaminerMark);
                    diffList.Add(NewObj2);

                    Difference NewObj3 = new Difference();
                    NewObj3.Diff = Diff1_3;
                    NewObj3.FirstMark = Convert.ToDecimal(ExistingObj.FirstExaminerMark);
                    NewObj3.SecondMark = Convert.ToDecimal(ExistingObj.ThirdExaminerMark);
                    diffList.Add(NewObj3);


                    decimal diff = Convert.ToDecimal((diffList.OrderBy(x => x.Diff).FirstOrDefault().Diff));

                    var sameDiffMoreThanOne = diffList.Where(x => x.Diff == diff).ToList();
                    if (sameDiffMoreThanOne != null && sameDiffMoreThanOne.Count > 1)
                    {
                        decimal FirstTotal = sameDiffMoreThanOne[0].FirstMark + sameDiffMoreThanOne[0].SecondMark;
                        decimal SecondTotal = sameDiffMoreThanOne[1].FirstMark + sameDiffMoreThanOne[1].SecondMark;
                        if (FirstTotal > SecondTotal)
                            Mark = FirstTotal;
                        else
                            Mark = SecondTotal;
                    }
                    else
                    {
                        var SingleObj = diffList.OrderBy(x => x.Diff).FirstOrDefault();
                        if (SingleObj != null)
                        {
                            Mark = Convert.ToDecimal(SingleObj.FirstMark) + Convert.ToDecimal(SingleObj.SecondMark);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }

            return Mark;
        }

        public class Difference
        {
            public decimal FirstMark { get; set; }
            public decimal SecondMark { get; set; }
            public decimal Diff { get; set; }
        }

        #region Mark Given And Same Mark Checking


        private bool CheckCourseTeacherAllItemDoubleEntryGiven(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                int ReturnValue = 0;

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckCourseTeacherAllItemDoubleEntryGiven", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    ReturnValue = Convert.ToInt32(DTReturnValue.Rows[0][0]);
                }
                if (ReturnValue > 0)
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckCourseTeacherBothMarksMatched(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckCourseTeacherBothMarksMatched", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    IsOk = false;
                }
                else
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckFirstExaminerAllItemDoubleEntryGiven(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                int ReturnValue = 0;

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckFirstExaminerAllItemDoubleEntryGiven", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    ReturnValue = Convert.ToInt32(DTReturnValue.Rows[0][0]);
                }
                if (ReturnValue > 0)
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckFirstExaminerBothMarksMatched(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckFirstExaminerBothMarksMatched", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    IsOk = false;
                }
                else
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckSecondExaminerAllTabulatorEntryGiven(int acaCalSection)
        {
            bool IsOk = false;
            try
            {
                int ReturnValue = 0;

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckSecondExaminerAllTabulatorEntryGiven", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    ReturnValue = Convert.ToInt32(DTReturnValue.Rows[0][0]);
                }
                if (ReturnValue > 0)
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckSecondExaminerAllTabulatorMarksMatched(int acaCalSection)
        {
            bool IsOk = false;
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckSecondExaminerAllTabulatorMarksMatched", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    IsOk = false;
                }
                else
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckThirdExaminerAllTabulatorEntryGiven(int acaCalSection)
        {
            bool IsOk = false;
            try
            {
                int ReturnValue = 0;

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckThirdExaminerAllTabulatorEntryGiven", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    ReturnValue = Convert.ToInt32(DTReturnValue.Rows[0][0]);
                }
                if (ReturnValue > 0)
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckThirdExaminerAllTabulatorMarksMatched(int acaCalSection)
        {
            bool IsOk = false;
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSection });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckThirdExaminerAllTabulatorMarksMatched", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    IsOk = false;
                }
                else
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckThirdExaminerAllItemDoubleEntryGiven(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                int ReturnValue = 0;

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckThirdExaminerAllItemDoubleEntryGiven", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    ReturnValue = Convert.ToInt32(DTReturnValue.Rows[0][0]);
                }
                if (ReturnValue > 0)
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        private bool CheckThirdExaminerBothMarksMatched(int acaCalSectionId)
        {
            bool IsOk = false;
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@acaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = acaCalSectionId });

                DataTable DTReturnValue = DataTableManager.GetDataFromQuery("CheckThirdExaminerBothMarksMatched", parameters1);

                if (DTReturnValue != null && DTReturnValue.Rows.Count > 0)
                {
                    IsOk = false;
                }
                else
                    IsOk = true;

            }
            catch (Exception ex)
            {
            }
            return IsOk;
        }

        #endregion


        protected void btnSubmitAllMark_Click(object sender, EventArgs e)
        {
            try
            {
                //lblMsg.Text = string.Empty;
                int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);
                ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                if (examTemplateItemId > 0)
                {
                    examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                }

                if (examTemplateItemObj != null)
                {
                    string course = ddlCourse.SelectedValue;
                    string[] courseVersion = course.Split('_');

                    int acaCalSection = Convert.ToInt32(courseVersion[2]);

                    int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examTemplateItemObj, acaCalSection, UserObj.Id);
                    if (examMarkMasterId > 0)
                    {
                        if (CheckAllMark(examTemplateItemObj.ExamMark))
                        {
                            SubmitAllExamMark(examMarkMasterId, examTemplateItemObj.ExamTemplateItemId, examTemplateItemObj.ExamMark, acaCalSection);
                            btnLoad_Click(null, null);
                            GridRebind(1, "");
                        }
                    }
                    else
                    {
                        showAlert("কোন data পাওয়া যায় নি ");
                    }
                }
                else
                {
                    showAlert("কোন data পাওয়া যায় নি ");
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message;
            }
        }

        //private int InsertExamMarkMaster(ExamTemplateItem examTemplateItemObj, int acaCalSection)
        //{
        //    int examMarkMasterId = 0;
        //    try
        //    {                
        //        ExamMarkMaster examMarkMasterNewObj = ExamMarkMasterManager.GetByAcaCalSectionIdExamTemplateItemId(acaCalSection, examTemplateItemObj.ExamTemplateItemId);
        //        if (examMarkMasterNewObj == null)
        //        {
        //            if (examTemplateItemObj != null && acaCalSection > 0)
        //            {
        //                ExamMarkMaster examMarkMasterObj = new ExamMarkMaster();
        //                examMarkMasterObj.ExamMarkEntryDate = DateTime.Now;
        //                examMarkMasterObj.ExamMark = examTemplateItemObj.ExamMark;
        //                //if (string.IsNullOrEmpty(txtExamDate.Text))
        //                //    examMarkMasterObj.FinalSubmissionDate = DateTime.Now;
        //                //else
        //                //    examMarkMasterObj.FinalSubmissionDate = DateTime.ParseExact(txtExamDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
        //                examMarkMasterObj.IsFinalSubmit = false;
        //                examMarkMasterObj.ExamTemplateItemId = examTemplateItemObj.ExamTemplateItemId;
        //                examMarkMasterObj.AcaCalSectionId = acaCalSection;
        //                examMarkMasterObj.CreatedBy = userObj.Id;
        //                examMarkMasterObj.CreatedDate = DateTime.Now;
        //                examMarkMasterObj.ModifiedBy = userObj.Id;
        //                examMarkMasterObj.ModifiedDate = DateTime.Now;


        //                int result = ExamMarkMasterManager.Insert(examMarkMasterObj);
        //                if (result > 0)
        //                {
        //                    examMarkMasterId = result;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            examMarkMasterId = examMarkMasterNewObj.ExamMarkMasterId;
        //        }
        //        return examMarkMasterId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return examMarkMasterId;
        //    }
        //}

        private void SubmitAllExamMark(int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, int AcacalSectionId)
        {
            try
            {
                int ThirdExaminerIsExternal = 0;
                try
                {
                    var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == AcacalSectionId).FirstOrDefault();
                    if (ExaminerObj != null && ExaminerObj.ThirdExaminerId != null && ExaminerObj.ThirdExaminerId > 0)
                    {
                        if (ExaminerObj.Attribute1 != null)
                            ThirdExaminerIsExternal = Convert.ToInt32(ExaminerObj.Attribute1);
                    }
                }
                catch (Exception ex)
                {
                }

                #region Get Course Type

                int CourseType = 0;

                LogicLayer.BusinessObjects.AcademicCalenderSection AcacalSectionObj = AcademicCalenderSectionManager.GetById(AcacalSectionId);
                if (AcacalSectionObj != null && AcacalSectionObj.Course != null && AcacalSectionObj.Course.TypeDefinitionID != null)
                    CourseType = Convert.ToInt32(AcacalSectionObj.Course.TypeDefinitionID);

                #endregion


                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");

                    Nullable<decimal> studentMark = 0;
                    int courseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    bool isAbsent = chkStatus.Checked;
                    if (courseHistoryId > 0)
                    {
                        if (Convert.ToString(mark.Text) == "Ab" || string.IsNullOrEmpty(mark.Text))
                        {
                            studentMark = 0;
                        }
                        else
                        {
                            studentMark = Convert.ToDecimal(mark.Text);
                        }

                        if (CourseType == 1)
                            InsertEditExamMarkDetails(courseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateExamMark, studentMark, AcacalSectionId, ThirdExaminerIsExternal);
                        else if (CourseType == 2)
                            InsertEditExamMarkDetailsLab(courseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateExamMark, studentMark, AcacalSectionId);
                        else if (CourseType == 3)
                            InsertEditExamMarkDetailsViva(courseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateExamMark, studentMark, AcacalSectionId);

                    }
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message;
            }
        }

        private bool InsertEditExamMarkDetails(int studentCourseHistoryId, bool isAbsent, int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, Nullable<decimal> studentMark, int AcacalSectionId, int ThirdExaminerIsExternal)
        {
            bool IsUpdate = false;
            int EmployeeId = GetEmployeeId();
            Nullable<decimal> mark = studentMark; // GetStudentMark(studentCourseHistoryId);
            try
            {
                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == AcacalSectionId).FirstOrDefault();

                #region Course Teacher Mark Entry

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // Course Teacher Needs To Enter Mark
                {
                    int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                    if (EntryNo == 1) // First Time Entry
                    {
                        var FirstTimeObj = ucamContext.CourseTeacherMarkDetailsFirstTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region Course Teacher First Time Mark Entry / Update

                        if (FirstTimeObj == null) // New Entry
                        {
                            DAL.CourseTeacherMarkDetailsFirstTime NewObj = new DAL.CourseTeacherMarkDetailsFirstTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 0;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.CourseTeacherMarkDetailsFirstTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (FirstTimeObj.IsFinalSubmit == null || FirstTimeObj.IsFinalSubmit == false)
                            {
                                FirstTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    FirstTimeObj.Marks = Convert.ToDecimal(mark);
                                    FirstTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    FirstTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    FirstTimeObj.ExamStatus = 1;
                                    FirstTimeObj.Marks = 0;
                                    FirstTimeObj.ConvertedMark = 0;
                                }
                                FirstTimeObj.ExamTemplateItemId = examTemplateItemId;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.IsFinalSubmit = false;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.ModifiedBy = UserObj.Id;
                                FirstTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(FirstTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                    else // Second Time Entry
                    {
                        var SecondTimeObj = ucamContext.CourseTeacherMarkDetailsSecondTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region Course Teacher Second Time Mark Entry / Update

                        if (SecondTimeObj == null) // New Entry
                        {
                            DAL.CourseTeacherMarkDetailsSecondTime NewObj = new DAL.CourseTeacherMarkDetailsSecondTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.CourseTeacherMarkDetailsSecondTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (SecondTimeObj.IsFinalSubmit == null || SecondTimeObj.IsFinalSubmit == false)
                            {
                                SecondTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    SecondTimeObj.Marks = Convert.ToDecimal(mark);
                                    SecondTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    SecondTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    SecondTimeObj.ExamStatus = 1;
                                    SecondTimeObj.Marks = 0;
                                    SecondTimeObj.ConvertedMark = 0;
                                }
                                SecondTimeObj.ExamTemplateItemId = examTemplateItemId;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.IsFinalSubmit = false;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.ModifiedBy = UserObj.Id;
                                SecondTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(SecondTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                #region Internal Examiner Mark Entry

                else if (SectionStatusObj.StatusId == 1) // First Examiner Needs To Enter Mark
                {
                    int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                    if (EntryNo == 1) // First Time Entry
                    {
                        var FirstTimeObj = ucamContext.FirstExaminerMarkDetailsFirstTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region First Examiner First Time Mark Entry / Update

                        if (FirstTimeObj == null) // New Entry
                        {
                            DAL.FirstExaminerMarkDetailsFirstTime NewObj = new DAL.FirstExaminerMarkDetailsFirstTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.FirstExaminerMarkDetailsFirstTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (FirstTimeObj.IsFinalSubmit == null || FirstTimeObj.IsFinalSubmit == false)
                            {
                                FirstTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    FirstTimeObj.Marks = Convert.ToDecimal(mark);
                                    FirstTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    FirstTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    FirstTimeObj.ExamStatus = 1;
                                    FirstTimeObj.Marks = null;
                                    FirstTimeObj.ConvertedMark = null;
                                }
                                FirstTimeObj.ExamTemplateItemId = examTemplateItemId;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.IsFinalSubmit = false;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.ModifiedBy = UserObj.Id;
                                FirstTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(FirstTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                    else // Second Time Entry
                    {
                        var SecondTimeObj = ucamContext.FirstExaminerMarkDetailsSecondTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region First Examiner Second Time Mark Entry / Update

                        if (SecondTimeObj == null) // New Entry
                        {
                            DAL.FirstExaminerMarkDetailsSecondTime NewObj = new DAL.FirstExaminerMarkDetailsSecondTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.FirstExaminerMarkDetailsSecondTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (SecondTimeObj.IsFinalSubmit == null || SecondTimeObj.IsFinalSubmit == false)
                            {
                                SecondTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    SecondTimeObj.Marks = Convert.ToDecimal(mark);
                                    SecondTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    SecondTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    SecondTimeObj.ExamStatus = 1;
                                    SecondTimeObj.Marks = 0;
                                    SecondTimeObj.ConvertedMark = 0;
                                }
                                SecondTimeObj.ExamTemplateItemId = examTemplateItemId;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.IsFinalSubmit = false;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.ModifiedBy = UserObj.Id;
                                SecondTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(SecondTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region External Examiner Mark Entry

                else if (SectionStatusObj.StatusId == 2) // External Examiner (Tabulator1, Tabulator2, Tabulator3) Need To Enter Mark
                {
                    int TabNo = GetTabulatorNo(Convert.ToInt32(ddlHeldIn.SelectedValue), EmployeeId);

                    if (TabNo == 1) // User Logged In as Tabulator One
                    {
                        var TabOneObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabOneObj == null) // New Entry For Tabulator One
                        {
                            #region Tabulator One Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator One Update

                            if (TabOneObj.IsFinalSubmit == null || TabOneObj.IsFinalSubmit == false)
                            {
                                TabOneObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabOneObj.Marks = Convert.ToDecimal(mark);
                                    TabOneObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabOneObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabOneObj.ExamStatus = 1;
                                    TabOneObj.Marks = 0;
                                    TabOneObj.ConvertedMark = 0;
                                }
                                TabOneObj.ExamTemplateItemId = examTemplateItemId;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.IsFinalSubmit = false;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.ModifiedBy = UserObj.Id;
                                TabOneObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabOneObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 2) // User Logged In as Tabulator Two
                    {
                        var TabTwoObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabTwoObj == null) // New Entry For Tabulator Two
                        {
                            #region Tabulator Two Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Two Update

                            if (TabTwoObj.IsFinalSubmit == null || TabTwoObj.IsFinalSubmit == false)
                            {
                                TabTwoObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabTwoObj.Marks = Convert.ToDecimal(mark);
                                    TabTwoObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabTwoObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabTwoObj.ExamStatus = 1;
                                    TabTwoObj.Marks = 0;
                                    TabTwoObj.ConvertedMark = 0;
                                }
                                TabTwoObj.ExamTemplateItemId = examTemplateItemId;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.IsFinalSubmit = false;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.ModifiedBy = UserObj.Id;
                                TabTwoObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabTwoObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 3) // User Logged In as Tabulator Three
                    {
                        var TabThreeObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabThreeObj == null) // New Entry For Tabulator Three
                        {
                            #region Tabulator Three Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Three Update

                            if (TabThreeObj.IsFinalSubmit == null || TabThreeObj.IsFinalSubmit == false)
                            {
                                TabThreeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabThreeObj.Marks = Convert.ToDecimal(mark);
                                    TabThreeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabThreeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabThreeObj.ExamStatus = 1;
                                    TabThreeObj.Marks = 0;
                                    TabThreeObj.ConvertedMark = 0;
                                }
                                TabThreeObj.ExamTemplateItemId = examTemplateItemId;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.IsFinalSubmit = false;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.ModifiedBy = UserObj.Id;
                                TabThreeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabThreeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }
                }

                #endregion

                #region Third Examiner Mark Entry

                else if (SectionStatusObj.StatusId == 3) // Third Examiner Need To Enter Mark
                {

                    if (ThirdExaminerIsExternal == 0) // Third Examiner is an Internal Member.
                    {
                        #region If Third Examiner Enter Marks


                        int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                        if (EntryNo == 1) // First Time Entry
                        {
                            var FirstTimeObj = ucamContext.ThirdExaminerMarkDetailsFirstTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                                && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                            #region Third Examiner First Time Mark Entry / Update

                            if (FirstTimeObj == null) // New Entry
                            {
                                DAL.ThirdExaminerMarkDetailsFirstTime NewObj = new DAL.ThirdExaminerMarkDetailsFirstTime();
                                NewObj.ExamMarkMasterId = examMarkMasterId;
                                NewObj.CourseHistoryId = studentCourseHistoryId;
                                if (!isAbsent && mark != null)
                                {
                                    NewObj.Marks = Convert.ToDecimal(mark);
                                    NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                    NewObj.ExamStatus = 1;
                                }
                                else
                                {
                                    NewObj.ExamStatus = 1;
                                    NewObj.Marks = 0;
                                    NewObj.ConvertedMark = 0;
                                }
                                NewObj.IsFinalSubmit = false;
                                NewObj.ExamMarkTypeId = 0;
                                NewObj.ExamTemplateItemId = examTemplateItemId;
                                NewObj.CreatedBy = UserObj.Id;
                                NewObj.CreatedDate = DateTime.Now;
                                NewObj.ModifiedBy = UserObj.Id;
                                NewObj.ModifiedDate = DateTime.Now;

                                ucamContext.ThirdExaminerMarkDetailsFirstTimes.Add(NewObj);
                                ucamContext.SaveChanges();

                                if (NewObj.Id > 0)
                                {
                                    IsUpdate = true;
                                }
                            }
                            else // Update Existing Entry
                            {
                                if (FirstTimeObj.IsFinalSubmit == null || FirstTimeObj.IsFinalSubmit == false)
                                {
                                    FirstTimeObj.ExamMarkMasterId = examMarkMasterId;
                                    if (!isAbsent)
                                    {
                                        FirstTimeObj.Marks = Convert.ToDecimal(mark);
                                        FirstTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                        FirstTimeObj.ExamStatus = 1;
                                    }
                                    else
                                    {
                                        FirstTimeObj.ExamStatus = 1;
                                        FirstTimeObj.Marks = 0;
                                        FirstTimeObj.ConvertedMark = 0;
                                    }
                                    FirstTimeObj.ExamTemplateItemId = examTemplateItemId;
                                    FirstTimeObj.ExamMarkTypeId = 0;
                                    FirstTimeObj.IsFinalSubmit = false;
                                    FirstTimeObj.ExamMarkTypeId = 0;
                                    FirstTimeObj.ModifiedBy = UserObj.Id;
                                    FirstTimeObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(FirstTimeObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    IsUpdate = true;
                                }
                            }
                            #endregion
                        }
                        else // Second Time Entry
                        {
                            var SecondTimeObj = ucamContext.ThirdExaminerMarkDetailsSecondTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                                && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                            #region Third Examiner Second Time Mark Entry / Update

                            if (SecondTimeObj == null) // New Entry
                            {
                                DAL.ThirdExaminerMarkDetailsSecondTime NewObj = new DAL.ThirdExaminerMarkDetailsSecondTime();
                                NewObj.ExamMarkMasterId = examMarkMasterId;
                                NewObj.CourseHistoryId = studentCourseHistoryId;
                                if (!isAbsent && mark != null)
                                {
                                    NewObj.Marks = Convert.ToDecimal(mark);
                                    NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                    NewObj.ExamStatus = 1;
                                }
                                else
                                {
                                    NewObj.ExamStatus = 1;
                                    NewObj.Marks = 0;
                                    NewObj.ConvertedMark = 0;
                                }
                                NewObj.IsFinalSubmit = false;
                                NewObj.ExamMarkTypeId = 0;
                                NewObj.ExamTemplateItemId = examTemplateItemId;
                                NewObj.CreatedBy = UserObj.Id;
                                NewObj.CreatedDate = DateTime.Now;
                                NewObj.ModifiedBy = UserObj.Id;
                                NewObj.ModifiedDate = DateTime.Now;

                                ucamContext.ThirdExaminerMarkDetailsSecondTimes.Add(NewObj);
                                ucamContext.SaveChanges();

                                if (NewObj.Id > 0)
                                {
                                    IsUpdate = true;
                                }
                            }
                            else // Update Existing Entry
                            {
                                if (SecondTimeObj.IsFinalSubmit == null || SecondTimeObj.IsFinalSubmit == false)
                                {
                                    SecondTimeObj.ExamMarkMasterId = examMarkMasterId;
                                    if (!isAbsent)
                                    {
                                        SecondTimeObj.Marks = Convert.ToDecimal(mark);
                                        SecondTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                        SecondTimeObj.ExamStatus = 1;
                                    }
                                    else
                                    {
                                        SecondTimeObj.ExamStatus = 1;
                                        SecondTimeObj.Marks = 0;
                                        SecondTimeObj.ConvertedMark = 0;
                                    }
                                    SecondTimeObj.ExamTemplateItemId = examTemplateItemId;
                                    SecondTimeObj.ExamMarkTypeId = 0;
                                    SecondTimeObj.IsFinalSubmit = false;
                                    SecondTimeObj.ExamMarkTypeId = 0;
                                    SecondTimeObj.ModifiedBy = UserObj.Id;
                                    SecondTimeObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(SecondTimeObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    IsUpdate = true;
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                    else// Third Examiner is an External Member.Tabulator will enter marks
                    {
                        #region If Tabulator will Enter Marks For Third Examiner

                        int TabNo = GetTabulatorNo(Convert.ToInt32(ddlHeldIn.SelectedValue), EmployeeId);

                        if (TabNo == 1) // User Logged In as Tabulator One
                        {
                            var TabOneObj = ucamContext.ThirdExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                                && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                            if (TabOneObj == null) // New Entry For Tabulator One
                            {
                                #region Tabulator One Insert

                                DAL.ThirdExaminerMarkDetail NewObj = new DAL.ThirdExaminerMarkDetail();

                                NewObj.ExamMarkMasterId = examMarkMasterId;
                                NewObj.CourseHistoryId = studentCourseHistoryId;
                                NewObj.TabulatorNo = TabNo;
                                if (!isAbsent && mark != null)
                                {
                                    NewObj.Marks = Convert.ToDecimal(mark);
                                    NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                    NewObj.ExamStatus = 1;
                                }
                                else
                                {
                                    NewObj.ExamStatus = 1;
                                    NewObj.Marks = 0;
                                    NewObj.ConvertedMark = 0;
                                }
                                NewObj.IsFinalSubmit = false;
                                NewObj.ExamMarkTypeId = 0;
                                NewObj.ExamTemplateItemId = examTemplateItemId;
                                NewObj.CreatedBy = UserObj.Id;
                                NewObj.CreatedDate = DateTime.Now;
                                NewObj.ModifiedBy = UserObj.Id;
                                NewObj.ModifiedDate = DateTime.Now;

                                ucamContext.ThirdExaminerMarkDetails.Add(NewObj);
                                ucamContext.SaveChanges();

                                if (NewObj.Id > 0)
                                {
                                    IsUpdate = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Tabulator One Update

                                if (TabOneObj.IsFinalSubmit == null || TabOneObj.IsFinalSubmit == false)
                                {
                                    TabOneObj.ExamMarkMasterId = examMarkMasterId;
                                    if (!isAbsent)
                                    {
                                        TabOneObj.Marks = Convert.ToDecimal(mark);
                                        TabOneObj.ConvertedMark = Convert.ToDecimal(mark);
                                        TabOneObj.ExamStatus = 1;
                                    }
                                    else
                                    {
                                        TabOneObj.ExamStatus = 1;
                                        TabOneObj.Marks = 0;
                                        TabOneObj.ConvertedMark = 0;
                                    }
                                    TabOneObj.ExamTemplateItemId = examTemplateItemId;
                                    TabOneObj.ExamMarkTypeId = 0;
                                    TabOneObj.IsFinalSubmit = false;
                                    TabOneObj.ExamMarkTypeId = 0;
                                    TabOneObj.ModifiedBy = UserObj.Id;
                                    TabOneObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(TabOneObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    IsUpdate = true;
                                }
                                #endregion
                            }
                        }

                        if (TabNo == 2) // User Logged In as Tabulator Two
                        {
                            var TabTwoObj = ucamContext.ThirdExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                                && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                            if (TabTwoObj == null) // New Entry For Tabulator Two
                            {
                                #region Tabulator Two Insert

                                DAL.ThirdExaminerMarkDetail NewObj = new DAL.ThirdExaminerMarkDetail();

                                NewObj.ExamMarkMasterId = examMarkMasterId;
                                NewObj.CourseHistoryId = studentCourseHistoryId;
                                NewObj.TabulatorNo = TabNo;
                                if (!isAbsent && mark != null)
                                {
                                    NewObj.Marks = Convert.ToDecimal(mark);
                                    NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                    NewObj.ExamStatus = 1;
                                }
                                else
                                {
                                    NewObj.ExamStatus = 1;
                                    NewObj.Marks = 0;
                                    NewObj.ConvertedMark = 0;
                                }
                                NewObj.IsFinalSubmit = false;
                                NewObj.ExamMarkTypeId = 0;
                                NewObj.ExamTemplateItemId = examTemplateItemId;
                                NewObj.CreatedBy = UserObj.Id;
                                NewObj.CreatedDate = DateTime.Now;
                                NewObj.ModifiedBy = UserObj.Id;
                                NewObj.ModifiedDate = DateTime.Now;

                                ucamContext.ThirdExaminerMarkDetails.Add(NewObj);
                                ucamContext.SaveChanges();

                                if (NewObj.Id > 0)
                                {
                                    IsUpdate = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Tabulator Two Update

                                if (TabTwoObj.IsFinalSubmit == null || TabTwoObj.IsFinalSubmit == false)
                                {
                                    TabTwoObj.ExamMarkMasterId = examMarkMasterId;
                                    if (!isAbsent)
                                    {
                                        TabTwoObj.Marks = Convert.ToDecimal(mark);
                                        TabTwoObj.ConvertedMark = Convert.ToDecimal(mark);
                                        TabTwoObj.ExamStatus = 1;
                                    }
                                    else
                                    {
                                        TabTwoObj.ExamStatus = 1;
                                        TabTwoObj.Marks = 0;
                                        TabTwoObj.ConvertedMark = 0;
                                    }
                                    TabTwoObj.ExamTemplateItemId = examTemplateItemId;
                                    TabTwoObj.ExamMarkTypeId = 0;
                                    TabTwoObj.IsFinalSubmit = false;
                                    TabTwoObj.ExamMarkTypeId = 0;
                                    TabTwoObj.ModifiedBy = UserObj.Id;
                                    TabTwoObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(TabTwoObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    IsUpdate = true;
                                }
                                #endregion
                            }
                        }

                        if (TabNo == 3) // User Logged In as Tabulator Three
                        {
                            var TabThreeObj = ucamContext.ThirdExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                                && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                            if (TabThreeObj == null) // New Entry For Tabulator Three
                            {
                                #region Tabulator Three Insert

                                DAL.ThirdExaminerMarkDetail NewObj = new DAL.ThirdExaminerMarkDetail();

                                NewObj.ExamMarkMasterId = examMarkMasterId;
                                NewObj.CourseHistoryId = studentCourseHistoryId;
                                NewObj.TabulatorNo = TabNo;
                                if (!isAbsent && mark != null)
                                {
                                    NewObj.Marks = Convert.ToDecimal(mark);
                                    NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                    NewObj.ExamStatus = 1;
                                }
                                else
                                {
                                    NewObj.ExamStatus = 1;
                                    NewObj.Marks = 0;
                                    NewObj.ConvertedMark = 0;
                                }
                                NewObj.IsFinalSubmit = false;
                                NewObj.ExamMarkTypeId = 0;
                                NewObj.ExamTemplateItemId = examTemplateItemId;
                                NewObj.CreatedBy = UserObj.Id;
                                NewObj.CreatedDate = DateTime.Now;
                                NewObj.ModifiedBy = UserObj.Id;
                                NewObj.ModifiedDate = DateTime.Now;

                                ucamContext.ThirdExaminerMarkDetails.Add(NewObj);
                                ucamContext.SaveChanges();

                                if (NewObj.Id > 0)
                                {
                                    IsUpdate = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Tabulator Three Update

                                if (TabThreeObj.IsFinalSubmit == null || TabThreeObj.IsFinalSubmit == false)
                                {
                                    TabThreeObj.ExamMarkMasterId = examMarkMasterId;
                                    if (!isAbsent)
                                    {
                                        TabThreeObj.Marks = Convert.ToDecimal(mark);
                                        TabThreeObj.ConvertedMark = Convert.ToDecimal(mark);
                                        TabThreeObj.ExamStatus = 1;
                                    }
                                    else
                                    {
                                        TabThreeObj.ExamStatus = 1;
                                        TabThreeObj.Marks = 0;
                                        TabThreeObj.ConvertedMark = 0;
                                    }
                                    TabThreeObj.ExamTemplateItemId = examTemplateItemId;
                                    TabThreeObj.ExamMarkTypeId = 0;
                                    TabThreeObj.IsFinalSubmit = false;
                                    TabThreeObj.ExamMarkTypeId = 0;
                                    TabThreeObj.ModifiedBy = UserObj.Id;
                                    TabThreeObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(TabThreeObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    IsUpdate = true;
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {

            }


            return IsUpdate;

        }

        private bool InsertEditExamMarkDetailsLab(int studentCourseHistoryId, bool isAbsent, int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, Nullable<decimal> studentMark, int AcacalSectionId)
        {
            bool IsUpdate = false;
            int EmployeeId = GetEmployeeId();
            Nullable<decimal> mark = studentMark; // GetStudentMark(studentCourseHistoryId);
            try
            {
                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == AcacalSectionId).FirstOrDefault();

                #region Course Teacher Mark Entry

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0) // Course Teacher Needs To Enter Mark
                {
                    int EntryNo = Convert.ToInt32(RadioButtonList1.SelectedValue);

                    if (EntryNo == 1) // First Time Entry
                    {
                        var FirstTimeObj = ucamContext.CourseTeacherMarkDetailsFirstTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region Course Teacher First Time Mark Entry / Update

                        if (FirstTimeObj == null) // New Entry
                        {
                            DAL.CourseTeacherMarkDetailsFirstTime NewObj = new DAL.CourseTeacherMarkDetailsFirstTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.CourseTeacherMarkDetailsFirstTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (FirstTimeObj.IsFinalSubmit == null || FirstTimeObj.IsFinalSubmit == false)
                            {
                                FirstTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    FirstTimeObj.Marks = Convert.ToDecimal(mark);
                                    FirstTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    FirstTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    FirstTimeObj.ExamStatus = 1;
                                    FirstTimeObj.Marks = 0;
                                    FirstTimeObj.ConvertedMark = 0;
                                }
                                FirstTimeObj.ExamTemplateItemId = examTemplateItemId;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.IsFinalSubmit = false;
                                FirstTimeObj.ExamMarkTypeId = 0;
                                FirstTimeObj.ModifiedBy = UserObj.Id;
                                FirstTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(FirstTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                    else // Second Time Entry
                    {
                        var SecondTimeObj = ucamContext.CourseTeacherMarkDetailsSecondTimes.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId).FirstOrDefault();

                        #region Course Teacher Second Time Mark Entry / Update

                        if (SecondTimeObj == null) // New Entry
                        {
                            DAL.CourseTeacherMarkDetailsSecondTime NewObj = new DAL.CourseTeacherMarkDetailsSecondTime();
                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.CourseTeacherMarkDetailsSecondTimes.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                        }
                        else // Update Existing Entry
                        {
                            if (SecondTimeObj.IsFinalSubmit == null || SecondTimeObj.IsFinalSubmit == false)
                            {
                                SecondTimeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    SecondTimeObj.Marks = Convert.ToDecimal(mark);
                                    SecondTimeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    SecondTimeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    SecondTimeObj.ExamStatus = 1;
                                    SecondTimeObj.Marks = 0;
                                    SecondTimeObj.ConvertedMark = 0;
                                }
                                SecondTimeObj.ExamTemplateItemId = examTemplateItemId;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.IsFinalSubmit = false;
                                SecondTimeObj.ExamMarkTypeId = 0;
                                SecondTimeObj.ModifiedBy = UserObj.Id;
                                SecondTimeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(SecondTimeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                        }
                        #endregion
                    }
                }

                #endregion


                #region Tabulator Mark Entry

                else if (SectionStatusObj.StatusId == 1) // Tabulator1, Tabulator2, Tabulator3 Need To Enter Mark
                {
                    int TabNo = GetTabulatorNo(Convert.ToInt32(ddlHeldIn.SelectedValue), EmployeeId);

                    if (TabNo == 1) // User Logged In as Tabulator One
                    {
                        var TabOneObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabOneObj == null) // New Entry For Tabulator One
                        {
                            #region Tabulator One Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator One Update

                            if (TabOneObj.IsFinalSubmit == null || TabOneObj.IsFinalSubmit == false)
                            {
                                TabOneObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabOneObj.Marks = Convert.ToDecimal(mark);
                                    TabOneObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabOneObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabOneObj.ExamStatus = 1;
                                    TabOneObj.Marks = 0;
                                    TabOneObj.ConvertedMark = 0;
                                }
                                TabOneObj.ExamTemplateItemId = examTemplateItemId;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.IsFinalSubmit = false;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.ModifiedBy = UserObj.Id;
                                TabOneObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabOneObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 2) // User Logged In as Tabulator Two
                    {
                        var TabTwoObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabTwoObj == null) // New Entry For Tabulator Two
                        {
                            #region Tabulator Two Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Two Update

                            if (TabTwoObj.IsFinalSubmit == null || TabTwoObj.IsFinalSubmit == false)
                            {
                                TabTwoObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabTwoObj.Marks = Convert.ToDecimal(mark);
                                    TabTwoObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabTwoObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabTwoObj.ExamStatus = 1;
                                    TabTwoObj.Marks = 0;
                                    TabTwoObj.ConvertedMark = 0;
                                }
                                TabTwoObj.ExamTemplateItemId = examTemplateItemId;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.IsFinalSubmit = false;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.ModifiedBy = UserObj.Id;
                                TabTwoObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabTwoObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 3) // User Logged In as Tabulator Three
                    {
                        var TabThreeObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabThreeObj == null) // New Entry For Tabulator Three
                        {
                            #region Tabulator Three Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Three Update

                            if (TabThreeObj.IsFinalSubmit == null || TabThreeObj.IsFinalSubmit == false)
                            {
                                TabThreeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabThreeObj.Marks = Convert.ToDecimal(mark);
                                    TabThreeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabThreeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabThreeObj.ExamStatus = 1;
                                    TabThreeObj.Marks = 0;
                                    TabThreeObj.ConvertedMark = 0;
                                }
                                TabThreeObj.ExamTemplateItemId = examTemplateItemId;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.IsFinalSubmit = false;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.ModifiedBy = UserObj.Id;
                                TabThreeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabThreeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {

            }


            return IsUpdate;

        }

        private bool InsertEditExamMarkDetailsViva(int studentCourseHistoryId, bool isAbsent, int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, Nullable<decimal> studentMark, int AcacalSectionId)
        {
            bool IsUpdate = false;
            int EmployeeId = GetEmployeeId();
            Nullable<decimal> mark = studentMark; // GetStudentMark(studentCourseHistoryId);
            try
            {
                var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == AcacalSectionId).FirstOrDefault();

                #region Tabulator Mark Entry

                if (SectionStatusObj == null || SectionStatusObj.StatusId == 0)  // Tabulator1, Tabulator2, Tabulator3 Need To Enter Mark
                {
                    int TabNo = GetTabulatorNo(Convert.ToInt32(ddlHeldIn.SelectedValue), EmployeeId);

                    if (TabNo == 1) // User Logged In as Tabulator One
                    {
                        var TabOneObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabOneObj == null) // New Entry For Tabulator One
                        {
                            #region Tabulator One Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator One Update

                            if (TabOneObj.IsFinalSubmit == null || TabOneObj.IsFinalSubmit == false)
                            {
                                TabOneObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabOneObj.Marks = Convert.ToDecimal(mark);
                                    TabOneObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabOneObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabOneObj.ExamStatus = 1;
                                    TabOneObj.Marks = 0;
                                    TabOneObj.ConvertedMark = 0;
                                }
                                TabOneObj.ExamTemplateItemId = examTemplateItemId;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.IsFinalSubmit = false;
                                TabOneObj.ExamMarkTypeId = 0;
                                TabOneObj.ModifiedBy = UserObj.Id;
                                TabOneObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabOneObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 2) // User Logged In as Tabulator Two
                    {
                        var TabTwoObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabTwoObj == null) // New Entry For Tabulator Two
                        {
                            #region Tabulator Two Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Two Update

                            if (TabTwoObj.IsFinalSubmit == null || TabTwoObj.IsFinalSubmit == false)
                            {
                                TabTwoObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabTwoObj.Marks = Convert.ToDecimal(mark);
                                    TabTwoObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabTwoObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabTwoObj.ExamStatus = 1;
                                    TabTwoObj.Marks = 0;
                                    TabTwoObj.ConvertedMark = 0;
                                }
                                TabTwoObj.ExamTemplateItemId = examTemplateItemId;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.IsFinalSubmit = false;
                                TabTwoObj.ExamMarkTypeId = 0;
                                TabTwoObj.ModifiedBy = UserObj.Id;
                                TabTwoObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabTwoObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }

                    if (TabNo == 3) // User Logged In as Tabulator Three
                    {
                        var TabThreeObj = ucamContext.SecondExaminerMarkDetails.Where(x => x.CourseHistoryId == studentCourseHistoryId
                            && x.ExamTemplateItemId == examTemplateItemId && x.TabulatorNo == TabNo).FirstOrDefault();

                        if (TabThreeObj == null) // New Entry For Tabulator Three
                        {
                            #region Tabulator Three Insert

                            DAL.SecondExaminerMarkDetail NewObj = new DAL.SecondExaminerMarkDetail();

                            NewObj.ExamMarkMasterId = examMarkMasterId;
                            NewObj.CourseHistoryId = studentCourseHistoryId;
                            NewObj.TabulatorNo = TabNo;
                            if (!isAbsent && mark != null)
                            {
                                NewObj.Marks = Convert.ToDecimal(mark);
                                NewObj.ConvertedMark = Convert.ToDecimal(mark);
                                NewObj.ExamStatus = 1;
                            }
                            else
                            {
                                NewObj.ExamStatus = 1;
                                NewObj.Marks = 0;
                                NewObj.ConvertedMark = 0;
                            }
                            NewObj.IsFinalSubmit = false;
                            NewObj.ExamMarkTypeId = 0;
                            NewObj.ExamTemplateItemId = examTemplateItemId;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;
                            NewObj.ModifiedBy = UserObj.Id;
                            NewObj.ModifiedDate = DateTime.Now;

                            ucamContext.SecondExaminerMarkDetails.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                            {
                                IsUpdate = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Tabulator Three Update

                            if (TabThreeObj.IsFinalSubmit == null || TabThreeObj.IsFinalSubmit == false)
                            {
                                TabThreeObj.ExamMarkMasterId = examMarkMasterId;
                                if (!isAbsent)
                                {
                                    TabThreeObj.Marks = Convert.ToDecimal(mark);
                                    TabThreeObj.ConvertedMark = Convert.ToDecimal(mark);
                                    TabThreeObj.ExamStatus = 1;
                                }
                                else
                                {
                                    TabThreeObj.ExamStatus = 1;
                                    TabThreeObj.Marks = 0;
                                    TabThreeObj.ConvertedMark = 0;
                                }
                                TabThreeObj.ExamTemplateItemId = examTemplateItemId;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.IsFinalSubmit = false;
                                TabThreeObj.ExamMarkTypeId = 0;
                                TabThreeObj.ModifiedBy = UserObj.Id;
                                TabThreeObj.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(TabThreeObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                IsUpdate = true;
                            }
                            #endregion
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {

            }


            return IsUpdate;

        }



        protected void ResultEntryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ResultSubmit")
                {
                    int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");
                    Label lblRoll = (Label)row.FindControl("lblStudentRoll");

                    int studentCourseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    bool isAbsent = chkStatus.Checked;

                    ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                    if (examTemplateItemId > 0)
                    {
                        examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                    }
                    if (examTemplateItemObj != null)
                    {
                        string course = ddlCourse.SelectedValue;
                        string[] courseVersion = course.Split('_');
                        Nullable<decimal> studentExamMark = mark.Text == "" ? 0 : Convert.ToDecimal(mark.Text);
                        int acaCalSection = Convert.ToInt32(courseVersion[2]);
                        int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examTemplateItemObj, acaCalSection, UserObj.Id);
                        if (examMarkMasterId > 0)
                        {
                            if (CheckMark(studentExamMark, examTemplateItemObj.ExamMark))
                            {
                                int ThirdExaminerIsExternal = 0;
                                try
                                {
                                    var ExaminerObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == acaCalSection).FirstOrDefault();
                                    if (ExaminerObj != null && ExaminerObj.ThirdExaminerId != null && ExaminerObj.ThirdExaminerId > 0)
                                    {
                                        if (ExaminerObj.Attribute1 != null)
                                            ThirdExaminerIsExternal = Convert.ToInt32(ExaminerObj.Attribute1);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                #region Get Course Type

                                int CourseType = 0;

                                LogicLayer.BusinessObjects.AcademicCalenderSection AcacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                                if (AcacalSectionObj != null && AcacalSectionObj.Course != null && AcacalSectionObj.Course.TypeDefinitionID != null)
                                    CourseType = Convert.ToInt32(AcacalSectionObj.Course.TypeDefinitionID);

                                #endregion

                                if (CourseType == 1)
                                    InsertEditExamMarkDetails(studentCourseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateItemObj.ExamMark, studentExamMark, acaCalSection, ThirdExaminerIsExternal);
                                else if (CourseType == 2)
                                    InsertEditExamMarkDetailsLab(studentCourseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateItemObj.ExamMark, studentExamMark, acaCalSection);
                                else if (CourseType == 3)
                                    InsertEditExamMarkDetailsViva(studentCourseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateItemObj.ExamMark, studentExamMark, acaCalSection);

                                LoadResultGrid(acaCalSection, examTemplateItemId, 1);

                                GridRebind(1, lblRoll.Text);


                            }
                            else
                            {
                                showAlert("আপনার দেওয়া mark পরীক্ষার mark থেকে বেশি হতে পারবে না ");
                            }
                        }
                        else
                        {
                            showAlert("কোন data পাওয়া যায় নি ");
                        }
                    }
                    else
                    {
                        showAlert("কোন data পাওয়া যায় নি ");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ResultEntryGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                int index = row.RowIndex;
                TextBox txtMark = (TextBox)ResultEntryGrid.Rows[index].FindControl("txtMark");
                CheckBox cb1 = (CheckBox)ResultEntryGrid.Rows[index].FindControl("chkStatus");

                if (cb1.Checked)
                {
                    txtMark.Text = "Ab";
                    txtMark.Enabled = false;
                }
                else
                {
                    txtMark.Text = string.Empty;
                    txtMark.Enabled = true;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        private bool CheckMark(Nullable<decimal> studentMark, decimal examMark)
        {
            if (studentMark == null)
            {
                return true;
            }
            else
            {
                double stuMark = Convert.ToDouble(studentMark);
                double exmMark = Convert.ToDouble(examMark);

                if (stuMark > exmMark)
                {
                    return false;
                }
                else if (stuMark >= 0 && stuMark <= exmMark)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool CheckAllMark(decimal examMark)
        {
            try
            {
                bool result = true;
                string msgText = null;
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label courseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    Label studentRoll = (Label)row.FindControl("lblStudentRoll");
                    TextBox mark = (TextBox)row.FindControl("txtMark");

                    Nullable<decimal> studentMark = 0;
                    if (Convert.ToString(mark.Text) == "Ab" || string.IsNullOrEmpty(mark.Text))
                    {
                        studentMark = 0;
                    }
                    else
                    {
                        studentMark = Convert.ToDecimal(mark.Text);//GetStudentMark(Convert.ToInt32(courseHistoryId.Text));

                        bool chkResult = CheckMark(studentMark, examMark);
                        if (chkResult == false)
                        {
                            result = false;
                            msgText = msgText + Convert.ToString(studentRoll.Text) + ", ";
                        }

                    }
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            try
            {
                string courseSection = ddlCourse.SelectedValue.ToString();
                string[] courseVersionSection = courseSection.Split('_');
                int courseId = Convert.ToInt32(courseVersionSection[0]);
                int versionId = Convert.ToInt32(courseVersionSection[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);
                if (acaCalSectionId > 0)
                {
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                    string url = string.Format("../../Module/result/Report/RptContinuousAssessment.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}&a={3}", departmentId, programId, heldInRelationId, acaCalSectionId);
                    string script = string.Format("window.open('{0}');", url);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);

                    //ExamTemplateManager.ProcessContinuousAssessmentMark(acaCalSectionId, UserObj.Id);

                    //AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    //List<rContinuousAssessmentMark> list = ExamTemplateManager.GetContinuousMarkBySectionID(acaCalSectionId);

                    //string Mark = "";
                    //bool IsViva = false;

                    //int programId = Convert.ToInt32(ucProgram.selectedValue), yearNo = Convert.ToInt32(ddlYearNo.SelectedValue), semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue)
                    //   , sessionId = Convert.ToInt32(ucFilterCurrentSession.selectedValue);


                    //string Exam = ddlExam.SelectedItem.ToString();
                    //string[] ExamNameSplit = Exam.Split(' ');
                    //string ExamName = "", ExamYear = "";
                    //if (ExamNameSplit[4] != "")
                    //    ExamName = ExamNameSplit[4] + " Examination";
                    //if (ExamNameSplit[5] != "")
                    //    ExamYear = ExamNameSplit[5];

                    //List<ExamTemplateItem> tmpList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId).Where(x => x.Attribute1 == "Continuous").ToList();

                    //List<rExamCommitteePersonInfo> list2 = new List<rExamCommitteePersonInfo>();

                    //if (tmpList != null && tmpList.Count > 0)
                    //{
                    //    Mark = tmpList.Sum(x => x.ExamMark).ToString();
                    //    if (tmpList.Where(x => x.Attribute2 == "Viva").Count() > 0)
                    //    {
                    //        IsViva = true;

                    //        list2 = ExamSetupManager.GetAllExamCommitteePersonInfoByParameter(programId, yearNo, semesterNo, Convert.ToInt32(ExamYear), sessionId);
                    //    }

                    //}

                    //Course crs = CourseManager.GetByCourseIdVersionId(courseId, versionId);
                    //CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = ExamMarkMasterManager.GetTeacherInfoAPIBySectionId(acaCalSectionId);

                    //string teacherName = "", teacherDept = "";

                    //if (countinousMarkTeacherInfoObj != null)
                    //{
                    //    teacherName = countinousMarkTeacherInfoObj.TeacherName == null ? "" : countinousMarkTeacherInfoObj.TeacherName;
                    //    teacherDept = countinousMarkTeacherInfoObj.DepartmentName == null ? "" : countinousMarkTeacherInfoObj.DepartmentName;
                    //}


                    //if (list.Count > 0)
                    //{
                    //    ResultEntryGrid.Visible = false;
                    //    ResultEntryGrid.DataSource = null;
                    //    ResultEntryGrid.DataBind();
                    //    btnFinalSubmitAll.Visible = false;
                    //    lblExamMark.Text = string.Empty;


                    //    //ReportParameter p1 = new ReportParameter("Year", ddlYearNo.SelectedItem.Text.ToString());
                    //    //ReportParameter p2 = new ReportParameter("Semester", ddlSemesterNo.SelectedItem.Text.ToString());
                    //    //ReportParameter p3 = new ReportParameter("Department", ucDepartment.selectedText.ToString());
                    //    //ReportParameter p4 = new ReportParameter("Course", crs.FormalCode.ToString());
                    //    //ReportParameter p5 = new ReportParameter("TeacherName", teacherName.ToString());
                    //    //ReportParameter p6 = new ReportParameter("DepartmentName", teacherDept.ToString());
                    //    //ReportParameter p7 = new ReportParameter("SessionName", countinousMarkTeacherInfoObj.SessionName.ToString());
                    //    //ReportParameter p8 = new ReportParameter("ExamMark", Mark);
                    //    //ReportParameter p9 = new ReportParameter("ExamName", ExamName);
                    //    //ReportParameter p10 = new ReportParameter("ExamYear", ExamYear);
                    //    //ReportParameter p11 = new ReportParameter("CourseTitle", crs.Title.ToString());

                    //    //if (IsViva)
                    //    //{

                    //    //    ReportParameter p12 = new ReportParameter("ChairmanName", list2[0].ChairmanName.ToString());
                    //    //    ReportParameter p13 = new ReportParameter("ChairmanDept", list2[0].ChairmanDept.ToString());

                    //    //    ReportParameter p14 = new ReportParameter("ExaminerOne", list2[0].MemberOneName.ToString());
                    //    //    ReportParameter p15 = new ReportParameter("ExaminerOneDept", list2[0].MemberOneDept.ToString());

                    //    //    ReportParameter p16 = new ReportParameter("ExaminerTwo", list2[0].MemberTwoName.ToString());
                    //    //    ReportParameter p17 = new ReportParameter("ExaminerTwoDept", list2[0].MemberTwoDept.ToString());

                    //    //    ReportParameter p18 = new ReportParameter("ExternalName", list2[0].ExternalMemberName.ToString());
                    //    //    ReportParameter p19 = new ReportParameter("ExternalDept", list2[0].ExternalMemberDept.ToString());

                    //    //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptContinuousAssessmentMarkViva.rdlc");
                    //    //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19 });

                    //    //}
                    //    //else
                    //    //{
                    //    //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptContinuousAssessmentMark.rdlc");
                    //    //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
                    //    //}

                    //    //ReportDataSource rds = new ReportDataSource("DataSet", list);

                    //    //ReportViewer1.LocalReport.DataSources.Clear();
                    //    //ReportViewer1.LocalReport.DataSources.Add(rds);





                    //    //ReportViewer1.Visible = true;





                    //}
                    //else
                    //{
                    //    ReportViewer1.LocalReport.DataSources.Clear();
                    //    ReportViewer1.Visible = false;
                    //    return;
                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            divMark.Visible = false;
            ClearGridView();
        }

        protected void lnkTestMark_Click(object sender, EventArgs e)
        {
            pnlExamMark.Visible = !pnlExamMark.Visible;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);
                decimal TotalMark = 0;
                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                if (examTemplateItemObj != null)
                {
                    TotalMark = Convert.ToDecimal(examTemplateItemObj.ExamMark);
                }

                string Mark = txtMark.Text;
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    try
                    {
                        GridViewRow row = ResultEntryGrid.Rows[i];

                        TextBox mark = (TextBox)row.FindControl("txtMark");
                        if (Mark != "")
                        {
                            Label lblError = (Label)row.FindControl("lblError");

                            mark.Text = Mark;
                            lblError.Text = "";

                            if (examTemplateItemObj.ExamName.ToLower().Contains("attendance"))
                            {
                                if (Convert.ToDecimal(Mark) > 0 && Convert.ToDecimal(Mark) < 4)
                                {
                                    lblError.Text = "*Class Attendance ০ কিংবা ৪ থেকে ১০ এন্ট্রি করুন";
                                    lblError.Visible = true;
                                }
                            }

                            if (Convert.ToDecimal(Mark) > TotalMark)
                            {
                                lblError.Text = "* " + TotalMark + " এর মধ্যে Mark এন্ট্রি করুন";
                                lblError.Visible = true;
                            }



                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnRequestConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                divMark.Visible = false;
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int acaCalSection = Convert.ToInt32(courseVersion[2]);


                int CourseType = 0;

                LogicLayer.BusinessObjects.AcademicCalenderSection AcacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                if (AcacalSectionObj != null && AcacalSectionObj.Course != null && AcacalSectionObj.Course.TypeDefinitionID != null)
                    CourseType = Convert.ToInt32(AcacalSectionObj.Course.TypeDefinitionID);


                #region Final Submit Process

                if (CourseType == 1) // Theory Course Final Submit
                {
                    TheoryCourseFinalSubmit(acaCalSection);
                }
                else if (CourseType == 2)// Lab Course Final Submit
                {
                    LabCourseFinalSubmit(acaCalSection);
                }
                else if (CourseType == 3)// Viva Course Final Submit
                {
                    VivaCourseFinalSubmit(acaCalSection);
                }


                #endregion

            }
            catch (Exception ex) { }

            modalPopupFinalSubmit.Hide();
        }

        protected void btnFinalMarkReport_Click(object sender, EventArgs e)
        {
            try
            {
                string courseSection = ddlCourse.SelectedValue.ToString();
                string[] courseVersionSection = courseSection.Split('_');
                int courseId = Convert.ToInt32(courseVersionSection[0]);
                int versionId = Convert.ToInt32(courseVersionSection[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);
                if (acaCalSectionId > 0)
                {
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                    int examinerNo = Convert.ToInt32(lblUserNo.Text);

                    string url = string.Format("../../Module/result/Report/RptFinalMark.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}&a={3}&e={4}", departmentId, programId, heldInRelationId, acaCalSectionId, examinerNo);
                    string script = string.Format("window.open('{0}');", url);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}