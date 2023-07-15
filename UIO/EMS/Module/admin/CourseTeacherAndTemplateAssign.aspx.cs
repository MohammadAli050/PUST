using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class CourseTeacherAndTemplateAssign : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.CourseTeacherAndTemplateAssign);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.CourseTeacherAndTemplateAssign));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                DivUpdate.Visible = false;
                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id,UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
                LoadCourseType();
                LoadCourseTeacher();
                LoadCourseTemplate();
            }
        }

        #region On Load Methods

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

                //ddlHeldIn.Items.Insert(0, new ListItem("Select", "0"));

                //ddlHeldIn.SelectedValue = "0";
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCourseType()
        {
            try
            {
                List<LogicLayer.BusinessObjects.TypeDefinition> CourseTypeList = LogicLayer.BusinessLogic.TypeDefinitionManager.GetAll()
                    .Where(x => x.Type == "Course").ToList();

                ddlCourseType.Items.Clear();
                ddlCourseType.AppendDataBoundItems = true;
                ddlCourseType.Items.Add(new ListItem("All", "0"));

                if (CourseTypeList != null && CourseTypeList.Any())
                {
                    ddlCourseType.DataTextField = "Definition";
                    ddlCourseType.DataValueField = "TypeDefinitionID";

                    ddlCourseType.DataSource = CourseTypeList.OrderBy(x => x.TypeDefinitionID);
                    ddlCourseType.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCourseTeacher()
        {
            try
            {
                int HeldInRelationId = 0;
                if (ddlHeldIn.SelectedValue != "")
                    HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                ddlTeacher.Items.Clear();
                ddlTeacher.AppendDataBoundItems = true;
                ddlTeacher.Items.Add(new ListItem("All", "0"));

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableTeacherList = DataTableManager.GetDataFromQuery("GetAllTeacherFromAcademicCalenderSectionByHeldInRelationId", parameters1);

                if (DataTableTeacherList != null && DataTableTeacherList.Rows.Count > 0)
                {
                    ddlTeacher.DataTextField = "TeacherInfo";
                    ddlTeacher.DataValueField = "EmployeeID";

                    ddlTeacher.DataSource = DataTableTeacherList;
                    ddlTeacher.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCourseTemplate()
        {
            try
            {
                int HeldInRelationId = 0;
                if (ddlHeldIn.SelectedValue != "")
                    HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                ddlTemplate.Items.Clear();
                ddlTemplate.AppendDataBoundItems = true;
                ddlTemplate.Items.Add(new ListItem("All", "0"));

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableTemplateList = DataTableManager.GetDataFromQuery("GetAllTemplateFromAcademicCalenderSectionByHeldInRelationId", parameters1);

                if (DataTableTemplateList != null && DataTableTemplateList.Rows.Count > 0)
                {
                    ddlTemplate.DataTextField = "ExamTemplateMasterName";
                    ddlTemplate.DataValueField = "ExamTemplateMasterId";

                    ddlTemplate.DataSource = DataTableTemplateList;
                    ddlTemplate.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
                LoadHeldInInformation();
                LoadCourseTeacher();
                LoadCourseTemplate();
                ClearGridView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadCourseTeacher();
                LoadCourseTemplate();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCourseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
        }

        protected void ddlTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
        }

        #endregion

        private void ClearGridView()
        {
            gvScheduleList.DataSource = null;
            gvScheduleList.DataBind();
            DivUpdate.Visible = false;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();

                Session["RoutineInList"] = null;


                int HeldInRelationId = 0, CourseType = 0, TeacherId = 0, TemplateId = 0;
                string Course = "";

                if (ddlHeldIn.SelectedValue != "")
                    HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                if (ddlCourseType.SelectedValue != "0")
                    CourseType = Convert.ToInt32(ddlCourseType.SelectedValue);
                if (ddlTeacher.SelectedValue != "0")
                    TeacherId = Convert.ToInt32(ddlTeacher.SelectedValue);
                if (ddlTemplate.SelectedValue != "0")
                    TemplateId = Convert.ToInt32(ddlTemplate.SelectedValue);
                if (!string.IsNullOrEmpty(txtCourse.Text))
                    Course = txtCourse.Text.Trim();

                if (HeldInRelationId == 0)
                {
                    showAlert("Please select a held In");
                    return;
                }

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                parameters1.Add(new SqlParameter { ParameterName = "@CourseType", SqlDbType = System.Data.SqlDbType.Int, Value = CourseType });
                parameters1.Add(new SqlParameter { ParameterName = "@TeacherId", SqlDbType = System.Data.SqlDbType.Int, Value = TeacherId });
                parameters1.Add(new SqlParameter { ParameterName = "@TemplateId", SqlDbType = System.Data.SqlDbType.Int, Value = TemplateId });
                parameters1.Add(new SqlParameter { ParameterName = "@Course", SqlDbType = System.Data.SqlDbType.NVarChar, Value = Course });

                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetAllClassRoutineInformation", parameters1);

                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                {
                    DivUpdate.Visible = true;
                    gvScheduleList.DataSource = DataTableRoutineList;
                    gvScheduleList.DataBind();
                    Session["RoutineInList"] = DataTableRoutineList;
                    ViewState["sortdrRoutine"] = "Asc";

                    GridRebind();

                    LoadTeacherDDL();
                    LoadTemplateDDL();
                    DepartmentUserControl1.LoadDropDownList();
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void GridRebind()
        {
            try
            {
                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    GridViewRow row = gvScheduleList.Rows[i];
                    HiddenField lblCode = (HiddenField)row.FindControl("hdnTeacherId");
                    HiddenField lblTemplate = (HiddenField)row.FindControl("hdnTemplateId");

                    if (lblCode.Value == "0" || lblTemplate.Value == "0")
                    {
                        gvScheduleList.Rows[i].BackColor = System.Drawing.Color.Aqua;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Update Region

        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeacherDDL();
        }

        protected void DepartmentUserControl1_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeacherDDL();
        }

        private void LoadTeacherDDL()
        {
            try
            {
                ddlCourseTeacher.Items.Clear();
                ddlCourseTeacher.AppendDataBoundItems = true;
                ddlCourseTeacher.Items.Add(new ListItem("Select", "0"));

                int DepartmentId = 0;
                string Designation = "";

                DepartmentId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
                Designation = ddlDesignation.SelectedValue.ToString();

                List<LogicLayer.BusinessObjects.Employee> empList = LogicLayer.BusinessLogic.EmployeeManager.GetAll().Where(x => x.EmployeeTypeId == 1).ToList();

                if (empList != null && empList.Any())
                {
                    if (DepartmentId > 0)
                        empList = empList.Where(x => x.DeptID == DepartmentId).ToList();
                    if (Designation != "All")
                        empList = empList.Where(x => x.Designation == Designation).ToList();
                }


                if (empList != null && empList.Any())
                {
                    ddlCourseTeacher.DataTextField = "CodeAndName";
                    ddlCourseTeacher.DataValueField = "EmployeeID";

                    ddlCourseTeacher.DataSource = empList.OrderBy(x => x.CodeAndName);
                    ddlCourseTeacher.DataBind();
                }



            }
            catch (Exception ex)
            {
            }
        }

        private void LoadTemplateDDL()
        {
            List<LogicLayer.BusinessObjects.ExamTemplate> templateList = LogicLayer.BusinessLogic.ExamTemplateManager.GetAll().ToList();

            ddlCourseTemplate.Items.Clear();
            ddlCourseTemplate.AppendDataBoundItems = true;
            ddlCourseTemplate.Items.Add(new ListItem("Select", "0"));

            if (templateList != null && templateList.Any())
            {
                ddlCourseTemplate.DataTextField = "ExamTemplateName";
                ddlCourseTemplate.DataValueField = "ExamTemplateId";

                ddlCourseTemplate.DataSource = templateList.OrderBy(x => x.ExamTemplateName);
                ddlCourseTemplate.DataBind();
            }

        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.Checked)
                {
                    chk.Text = "Unselect All";
                }
                else
                {
                    chk.Text = "Select All";
                }

                foreach (GridViewRow row in gvScheduleList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void btnUpdateTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                int TeacherId = Convert.ToInt32(ddlCourseTeacher.SelectedValue);

                int SelectedCount = CountGridSelected();

                if (SelectedCount == 0)
                {
                    showAlert("Please check minimum one schedule");
                    return;
                }

                int UpdateCount = 0;

                foreach (GridViewRow row in gvScheduleList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    if (ckBox.Checked)
                    {

                        HiddenField hdnAcaCalSectionID = (HiddenField)row.FindControl("hdnAcaCalSectionID");

                        int SectionId = Convert.ToInt32(hdnAcaCalSectionID.Value);


                        if (SectionId > 0)
                        {
                            var ExistingObj = ucamContext.AcademicCalenderSections.Find(SectionId);

                            if (ExistingObj != null)
                            {
                                ExistingObj.TeacherOneID = TeacherId;
                                ExistingObj.ModifiedBy = UserObj.Id;
                                ExistingObj.ModifiedDate = DateTime.Now;


                                ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();


                                UpdateCount++;
                            }

                        }
                    }
                }


                if (UpdateCount > 0)
                {
                    showAlert("Teacher Information Updated Successfully");
                    LoadCourseTeacher();
                    LoadCourseTemplate();
                    btnLoad_Click(null, null);
                    return;
                }
                else
                {
                    showAlert("Teacher Information Update Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnUpdateTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                int TemplateId = Convert.ToInt32(ddlCourseTemplate.SelectedValue);

                int SelectedCount = CountGridSelected();

                if (SelectedCount == 0)
                {
                    showAlert("Please check minimum one schedule");
                    return;
                }

                int UpdateCount = 0;

                foreach (GridViewRow row in gvScheduleList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    if (ckBox.Checked)
                    {

                        HiddenField hdnAcaCalSectionID = (HiddenField)row.FindControl("hdnAcaCalSectionID");

                        int SectionId = Convert.ToInt32(hdnAcaCalSectionID.Value);


                        if (SectionId > 0)
                        {
                            var ExistingObj = ucamContext.AcademicCalenderSections.Find(SectionId);

                            if (ExistingObj != null)
                            {
                                ExistingObj.BasicExamTemplateId = TemplateId;
                                ExistingObj.ModifiedBy = UserObj.Id;
                                ExistingObj.ModifiedDate = DateTime.Now;


                                ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();


                                UpdateCount++;
                            }

                        }
                    }
                }


                if (UpdateCount > 0)
                {
                    showAlert("Mark Distribution Updated Successfully");
                    LoadCourseTeacher();
                    LoadCourseTemplate();
                    btnLoad_Click(null, null);
                    return;
                }
                else
                {
                    showAlert("Mark Distribution Update Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private int CountGridSelected()
        {
            int Counter = 0;

            try
            {
                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    GridViewRow row = gvScheduleList.Rows[i];
                    CheckBox schCheckd = (CheckBox)row.FindControl("ChkActive");

                    if (schCheckd.Checked == true)
                    {
                        Counter++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Counter;
        }

        #endregion

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void gvScheduleList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                DataTable dtrslt = (DataTable)Session["RoutineInList"];
                if (dtrslt != null && dtrslt.Rows.Count > 0)
                {
                    if (Convert.ToString(ViewState["sortdrRoutine"]) == "Asc")
                    {
                        dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                        ViewState["sortdrRoutine"] = "Desc";
                    }
                    else
                    {
                        dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                        ViewState["sortdrRoutine"] = "Asc";
                    }
                    gvScheduleList.DataSource = dtrslt;
                    gvScheduleList.DataBind();
                }

                GridRebind();
            }

            catch (Exception ex)
            {

            }
        }

        protected void gvScheduleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["RoutineInList"];

                if (list != null)
                {
                    gvScheduleList.PageIndex = e.NewPageIndex;
                    gvScheduleList.DataSource = list;
                    gvScheduleList.DataBind();
                }
                GridRebind();
            }
            catch (Exception ex)
            {
            }
        }

    }
}