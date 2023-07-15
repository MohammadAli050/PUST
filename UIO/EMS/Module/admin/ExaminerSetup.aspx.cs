using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessObjects.DTO;
namespace EMS.Module.admin
{
    public partial class ExaminerSetup : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        int userId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            userId = userObj.Id;
            pnlMessage.Visible = false;
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownListWithUserAccess(userObj.Id, userObj.RoleID);
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL(programId);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0_0_0"));
                ddlCourse.AppendDataBoundItems = true;
                ddlExam.Items.Clear();
                ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
                ddlExam.AppendDataBoundItems = true;
                //LoadCurrentRegSessions();
                //LoadlevelTermDDL(Convert.ToInt32(ucProgram.selectedValue));
                //LoadTermDropDown(0);

            }

        }
        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void DepartmentUserControl1_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();

            int DeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
            LoadFacultyOneByDeptId(DeptId);
        }

        private void LoadFacultyOneByDeptId(int DeptId)
        {
            ddlEx1.Items.Clear();
            ddlEx1.Items.Add(new ListItem("-Select-", "0"));

            List<Employee> employeeList = EmployeeManager.GetAll();
            if (DeptId != 0)
                employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            if (employeeList.Count > 0)
            {
                employeeList = employeeList.OrderBy(o => o.Code).ToList();

                foreach (Employee employee in employeeList)
                {
                    ddlEx1.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                }
            }
        }
        protected void DepartmentUserControl2_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            int DeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
            LoadFacultyTwoByDeptId(DeptId);
        }
        private void LoadFacultyTwoByDeptId(int DeptId)
        {
            ddlEx2.Items.Clear();
            ddlEx2.Items.Add(new ListItem("-Select-", "0"));

            List<Employee> employeeList = EmployeeManager.GetAll();
            if (DeptId != 0)
                employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            if (employeeList.Count > 0)
            {
                employeeList = employeeList.OrderBy(o => o.Code).ToList();

                foreach (Employee employee in employeeList)
                {
                    ddlEx2.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                }
            }
        }
        protected void DepartmentUserControl3_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();

            int DeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
            LoadFacultyThreeByDeptId(DeptId);
        }

        private void LoadFacultyThreeByDeptId(int DeptId)
        {
            ddlEx3.Items.Clear();
            ddlEx3.Items.Add(new ListItem("-Select-", "0"));

            List<Employee> employeeList = EmployeeManager.GetAll();
            if (DeptId != 0)
                employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            if (employeeList.Count > 0)
            {
                employeeList = employeeList.OrderBy(o => o.Code).ToList();

                foreach (Employee employee in employeeList)
                {
                    ddlEx3.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                }
            }
        }
        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            //ucBatch.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
            try
            {
               // ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                int programId = Convert.ToInt32(ucProgram.selectedValue);
               // int sessionId = Convert.ToInt32(ucSession.selectedValue);
              //  LoadCourse(programId);
            }
            catch (Exception ex)
            { }
        }
        
        private void LoadYearNoDDL()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYearNo.Items.Clear();
            ddlYearNo.AppendDataBoundItems = true;
            ddlYearNo.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYearNo.DataTextField = "YearNoName";
                ddlYearNo.DataValueField = "YearNo";

                ddlYearNo.DataSource = yearList;
                ddlYearNo.DataBind();
            }
        }

        private void LoadSemesterNoDDL()
        {
            List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
            semesterList = SemesterManager.GetAllDistinct();
            semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();


            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.AppendDataBoundItems = true;
            ddlSemesterNo.Items.Add(new ListItem("-Select-", "-1"));
            if (semesterList != null && semesterList.Count > 0)
            {
                ddlSemesterNo.DataTextField = "SemesterNoName";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }
        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
           // lblMsg.Text = string.Empty;
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                loadExamDropdown(programId, yearNo, semesterNo);
            }
        }

        protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
           // lblMsg.Text = string.Empty;

            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                loadExamDropdown(programId, yearNo, semesterNo);
            }
            //else 
            //{
            //    lblMsg.Text = "Please select program, year no and semester no.";
            //}
        }
        private void loadExamDropdown(int programId, int yearNo, int semesterNo)
        {
            ddlExam.Items.Clear();
            ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
            ddlExam.AppendDataBoundItems = true;
            List<ExamSetupDetailDTO> exam = ExamSetupManager.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            if (exam != null)
            {
                foreach (ExamSetupDetailDTO examlist in exam)
                {
                    ddlExam.Items.Add(new ListItem(examlist.ExamName, examlist.ExamSetupDetailId.ToString()));

                }
            }
        }
        protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearno = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterno = Convert.ToInt32(ddlSemesterNo.SelectedValue);
            int examid = Convert.ToInt32(ddlExam.SelectedValue);
            LoadCourse(programId, yearno, semesterno, examid);
            PersonManager.ExaminerSetupGetAllByAcaCalProgramDataInsert(programId, yearno, semesterno, examid);
            List<ExaminorSetupsDTO> exam = ExaminorSetupsManager.ExaminerSetupGetAllByAcaCalProgram(programId, yearno, semesterno, examid);
            gvStudentList.DataSource = exam;
            gvStudentList.DataBind();
        }
        
        protected void LoadCourse(int programId, int yearno, int semesterno, int examid)
        {
            try
            {
               
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0_0_0"));
                ddlCourse.AppendDataBoundItems = true;

                BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetByProgramIdYearNoSemesterNoExamId(programId, yearno, semesterno, examid);
                User user = UserManager.GetByLogInId(userObj.LogInID);
                Role userRole = RoleManager.GetById(user.RoleID);
               

                if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                {
                    // acaCalSectionList = acaCalSectionList.OrderBy(x => x.CourseID).ToList();
                    foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                    {
                        ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + ":" + acaCalSec.Course.FormalCode + " " + acaCalSec.Course.Credits.ToString() + " (" + acaCalSec.SectionName + ")", acaCalSec.Course.CourseID + "_" + acaCalSec.Course.VersionID + "_" + acaCalSec.AcaCal_SectionID));
                    }
                }



            }
            catch { }
        }
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadgrid();
           
            //LoadAcaCalSection(courseId, versionId, acaCalId);
        }
        
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void loadgrid()
        {
            int yearno = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterno = Convert.ToInt32(ddlSemesterNo.SelectedValue);
            int examid = Convert.ToInt32(ddlExam.SelectedValue);
            string course = ddlCourse.SelectedValue;
            string[] courseVersion = course.Split('_');

            int courseId = Convert.ToInt32(courseVersion[0]);
            int versionId = Convert.ToInt32(courseVersion[1]);
           // int acaCalId = Convert.ToInt32(ucSession.selectedValue);
            int acaCalSection = Convert.ToInt32(courseVersion[2]);
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            //int sessionId = Convert.ToInt32(ucSession.selectedValue);
            if (acaCalSection != 0)
            {
                List<ExaminorSetupsDTO> exam = ExaminorSetupsManager.ExaminerSetupGetAllByAcaCalProgram(programId, yearno, semesterno, examid).Where(x => x.AcaCalSectionId == acaCalSection).ToList();
                gvStudentList.DataSource = exam;
                gvStudentList.DataBind();
            }
            else
            {
                List<ExaminorSetupsDTO> exam = ExaminorSetupsManager.ExaminerSetupGetAllByAcaCalProgram(programId, yearno, semesterno, examid);
                gvStudentList.DataSource = exam;
                gvStudentList.DataBind();
            }
            
            
        }


        protected void btnExaminer_Click(object sender, EventArgs e)
        {         
            Button btl = (Button)sender;
            int Id = Convert.ToInt32(btl.CommandArgument);
            lblID.Text = Id.ToString();
            DepartmentUserControl1.SelectedValue(0);
            DepartmentUserControl2.SelectedValue(0);
            DepartmentUserControl3.SelectedValue(0);
            DepartmentUserControl1.Enabled(false);
            ExaminorSetups obj = ExaminorSetupsManager.GetById(Id);
            if (obj != null)
            {
                AcademicCalenderSection acacalsection = AcademicCalenderSectionManager.GetById(Convert.ToInt32( obj.AcaCalSectionId));


                if (acacalsection != null)
                {
                   
                    TxtSection.Text =acacalsection.Course.Title + "_" +acacalsection.SectionName;
                    loadTeacherDropdown();

                    if (ddlEx1.Items.FindByValue(obj.FirstExaminer.ToString()) != null)
                    {
                        ddlEx1.SelectedValue = obj.FirstExaminer.ToString();
                    }
                    if (ddlEx2.Items.FindByValue(obj.SecondExaminor.ToString()) != null)
                    {
                        ddlEx2.SelectedValue = obj.SecondExaminor.ToString();
                    }
                    if (ddlEx3.Items.FindByValue(obj.ThirdExaminor.ToString()) != null)
                    {
                        ddlEx3.SelectedValue = obj.ThirdExaminor.ToString();
                    }
                    #region Department

                    Employee emp = EmployeeManager.GetById(Convert.ToInt32(ddlEx1.SelectedValue));
                    if (emp != null)
                        DepartmentUserControl1.SelectedValue(emp.DeptID);

                    Employee emp2 = EmployeeManager.GetById(Convert.ToInt32(ddlEx2.SelectedValue));
                    if (emp2 != null)
                        DepartmentUserControl2.SelectedValue(emp2.DeptID);

                    Employee emp3 = EmployeeManager.GetById(Convert.ToInt32(ddlEx3.SelectedValue));
                    if (emp3 != null)
                        DepartmentUserControl3.SelectedValue(emp3.DeptID);
                    #endregion
                    
                }
            }
            ModalPopupExtender1.Show();

        }

        private void loadTeacherDropdown()
        {

            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            List<Employee> emp = EmployeeManager.GetAllByTypeId(1).Where(x => x.DeptID == departmentId).ToList();
            ddlEx1.Items.Clear();
            ddlEx1.Items.Add(new ListItem("-Select Examiner-", "0"));
            ddlEx1.AppendDataBoundItems = true;
            ddlEx2.Items.Clear();
            ddlEx2.Items.Add(new ListItem("-Select Examiner-", "0"));
            ddlEx2.AppendDataBoundItems = true;

            ddlEx3.Items.Clear();
            ddlEx3.Items.Add(new ListItem("-Select Examiner-", "0"));
            ddlEx3.AppendDataBoundItems = true;

            if (emp != null)
            {
                foreach (Employee employee in emp)
                {
                    ddlEx1.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                    ddlEx2.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                    ddlEx3.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
        }

        protected void btnUpdte_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblID.Text);
            int Examiner1 = Convert.ToInt32(ddlEx1.SelectedValue);
            int Examiner2 = Convert.ToInt32(ddlEx2.SelectedValue);
            int Examiner3 = Convert.ToInt32(ddlEx3.SelectedValue);
            ExaminorSetups obj = ExaminorSetupsManager.GetById(id);
            if (obj != null)
            {
                try
                {

                   // obj.FirstExaminer = Examiner1;
                    obj.SecondExaminor = Examiner2;
                    obj.ThirdExaminor = Examiner3;

                    obj.ModifiedBy = userObj.Id;
                    obj.ModifiedDate = DateTime.Now;
                    bool update = ExaminorSetupsManager.Update(obj);
                    if (update == true)
                    {
                        
                        ShowAlertMessage ("Assigned Examiner update  successfully.");
                        
                        //#region Log Insert
                        //try
                        //{
                        //    LogGeneralManager.Insert(
                        //       DateTime.Now,
                        //       BaseAcaCalCurrent.CalendarUnitType_Code + " " + BaseAcaCalCurrent.Year.ToString(),
                        //       BaseAcaCalCurrent.FullCode,
                        //       BaseCurrentUserObj.LogInID,
                        //       "",
                        //       "",
                        //       "Publish date update",
                        //       BaseCurrentUserObj.LogInID + " Publish date update manually for " + Convert.ToString(ucProgram.selectedText),
                        //       "normal",
                        //       _pageId,
                        //       _pageName,
                        //       _pageUrl,
                        //       Convert.ToString(ucProgram.selectedText));
                        //}
                        //catch (Exception ex) { }

                        //#endregion
                    }
                    btnLoad_Click(null, null);
                }
                catch
                {
                }

            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }
       
        //protected void btnExamcancel_Click(object sender, EventArgs e)
        //{

        //}

        //protected void btnExamAssin_Click(object sender, EventArgs e)
        //{


        //    int Examid = Convert.ToInt32(ddlExam.SelectedValue);
        
        //    //int IsInsert = 1;
        //    //string CourseNotInsert = "";
        //    int j = Convert.ToInt32(gvStudentList.Rows.Count);
        //    for (int i = 0; i < gvStudentList.Rows.Count; i++)
        //    {
        //        GridViewRow row = gvStudentList.Rows[i];
        //        Label lblExammID = (Label)row.FindControl("lblExammID");

        //        CheckBox courseCheckd = (CheckBox)row.FindControl("ChkActive");
        //        if (courseCheckd.Checked == true)
        //        {

        //            int ExaminerId = Convert.ToInt32(lblExammID.Text);

        //            ExaminorSetups obj = ExaminorSetupsManager.GetById(ExaminerId);
        //            if (obj != null)
        //            {
        //                try
        //                {

        //                    obj.ExamId = Examid;


        //                    obj.ModifiedBy = userObj.Id;
        //                    obj.ModifiedDate = DateTime.Now;
        //                    bool update = ExaminorSetupsManager.Update(obj);
        //                    if (update == true)
        //                    {

        //                        ShowAlertMessage("Assigned Exam update  successfully.");

        //                        //#region Log Insert
        //                        //try
        //                        //{
        //                        //    LogGeneralManager.Insert(
        //                        //       DateTime.Now,
        //                        //       BaseAcaCalCurrent.CalendarUnitType_Code + " " + BaseAcaCalCurrent.Year.ToString(),
        //                        //       BaseAcaCalCurrent.FullCode,
        //                        //       BaseCurrentUserObj.LogInID,
        //                        //       "",
        //                        //       "",
        //                        //       "Publish date update",
        //                        //       BaseCurrentUserObj.LogInID + " Publish date update manually for " + Convert.ToString(ucProgram.selectedText),
        //                        //       "normal",
        //                        //       _pageId,
        //                        //       _pageName,
        //                        //       _pageUrl,
        //                        //       Convert.ToString(ucProgram.selectedText));
        //                        //}
        //                        //catch (Exception ex) { }

        //                        //#endregion
        //                    }
                            

        //                }
        //                catch
        //                {
        //                }

        //            }
        //        }


        //    }
        //    btnLoad_Click(null, null);
        //}

      
      
    }
}