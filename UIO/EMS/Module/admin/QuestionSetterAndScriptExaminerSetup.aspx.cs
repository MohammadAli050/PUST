using ClosedXML.Excel;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class QuestionSetterAndScriptExaminerSetup : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.QuestionSetterAndScriptExaminerSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.QuestionSetterAndScriptExaminerSetup));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                btnUpdateInfo.Visible = false;
                hdnIsUpdate.Value = "0";
                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
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
                LoadData();
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
                ClearGridView();
                LoadData();
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
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private void LoadData()
        {
            try
            {
                ClearGridView();

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetAllClassRoutineInformationWithQuestionSetterAndScriptExaminer", parameters1);

                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                {

                    gvScheduleList.DataSource = DataTableRoutineList;
                    gvScheduleList.DataBind();
                    Session["ScheduleList"] = DataTableRoutineList;
                    ViewState["sortdrSchedule"] = "Asc";
                    btnUpdateInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Gridview Methods

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

        protected void gvScheduleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["ScheduleList"];

                if (list != null)
                {
                    gvScheduleList.PageIndex = e.NewPageIndex;
                    gvScheduleList.DataSource = list;
                    gvScheduleList.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        protected void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            try
            {

                int Selected = CalculatedGridSelectedItem();

                if (Selected > 0)
                {
                    ModalPopUpInformation.Show();
                    hdnIsUpdate.Value = "0";
                    LoadDepartmentDDL();
                    LoadAllFacultyDDL();
                }
                else
                {
                    showAlert("Please select minimum one course");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private int CalculatedGridSelectedItem()
        {
            int ItemSelected = 0;
            try
            {
                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    GridViewRow row = gvScheduleList.Rows[i];
                    CheckBox scheduleCheckd = (CheckBox)row.FindControl("ChkActive");

                    if (scheduleCheckd.Checked == true)
                    {
                        ItemSelected = ItemSelected + 1;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ItemSelected;
        }

        #region Modal DropDown Binding

        private void LoadDepartmentDDL()
        {
            try
            {
                int DeptId = Convert.ToInt32(ucDepartment.selectedValue);

                ddlInternalQSetterDept.LoadDropDownList();
                ddlInternalQSetterDept.SelectedValue(DeptId);
                ddlInternalScriptExDept.LoadDropDownList();
                ddlInternalScriptExDept.SelectedValue(DeptId);


                LoadExternalQSDepartment();
                LoadExternalSEDepartment();

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadExternalQSDepartment()
        {
            try
            {
                var ExternalDeptList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Select(x => x.Department).Distinct().ToList();


                ddlExternalQSetterDept.Items.Clear();
                ddlExternalQSetterDept.AppendDataBoundItems = true;
                ddlExternalQSetterDept.Items.Add(new ListItem("All", "0"));

                if (ExternalDeptList != null && ExternalDeptList.Any())
                {
                    ddlExternalQSetterDept.DataSource = ExternalDeptList;
                    ddlExternalQSetterDept.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadExternalSEDepartment()
        {
            try
            {
                var ExternalDeptList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Select(x => x.Department).Distinct().ToList();

                ddlExternalScriptExDept.Items.Clear();
                ddlExternalScriptExDept.AppendDataBoundItems = true;
                ddlExternalScriptExDept.Items.Add(new ListItem("All", "0"));

                if (ExternalDeptList != null && ExternalDeptList.Any())
                {

                    ddlExternalScriptExDept.DataSource = ExternalDeptList;
                    ddlExternalScriptExDept.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadAllFacultyDDL()
        {
            try
            {
                Session["EmployeeList"] = null;

                int DeptId = Convert.ToInt32(ddlInternalQSetterDept.selectedValue);



                List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

                Session["EmployeeList"] = empList;

                if (DeptId > 0)
                    empList = empList.Where(a => a.DeptID == DeptId).ToList();

                if (empList != null && empList.Any())
                {
                    empList = empList.OrderBy(a => a.CodeAndName).ToList();

                    BindDDLInternalSetter(empList);
                    BindDDLInternalScriptEx(empList);




                    LoadExternalQSName();
                    LoadExternalSEName();

                    //BindDDLExternalSetter(empList);
                    //BindDDLExternalScriptEx(empList);

                }
            }
            catch (Exception ex)
            {
            }
        }


        private void BindDDLInternalSetter(List<Employee> empList)
        {
            try
            {
                ddlInternalQSetterName.Items.Clear();
                ddlInternalQSetterName.AppendDataBoundItems = true;
                ddlInternalQSetterName.Items.Add(new ListItem("Select", "0"));
                ddlInternalQSetterName.DataTextField = "CodeAndName";
                ddlInternalQSetterName.DataValueField = "EmployeeID";
                ddlInternalQSetterName.DataSource = empList;
                ddlInternalQSetterName.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        private void BindDDLInternalScriptEx(List<Employee> empList)
        {
            try
            {
                ddlInternalScriptExName.Items.Clear();
                ddlInternalScriptExName.AppendDataBoundItems = true;
                ddlInternalScriptExName.Items.Add(new ListItem("Select", "0"));
                ddlInternalScriptExName.DataTextField = "CodeAndName";
                ddlInternalScriptExName.DataValueField = "EmployeeID";
                ddlInternalScriptExName.DataSource = empList;
                ddlInternalScriptExName.DataBind();
            }
            catch (Exception ex)
            {

            }
        }



        private void LoadExternalQSName()
        {
            try
            {

                string Dept = ddlExternalQSetterDept.SelectedValue.ToString();

                ddlExternalQSetterName.Items.Clear();
                ddlExternalQSetterName.AppendDataBoundItems = true;
                ddlExternalQSetterName.Items.Add(new ListItem("Select", "0"));

                var ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().ToList();
                if (Dept != "0")
                    ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Where(x => x.Department == Dept).ToList();

                if (ExternalEmpList != null && ExternalEmpList.Any())
                {
                    ddlExternalQSetterName.DataTextField = "Name";
                    ddlExternalQSetterName.DataValueField = "ExternalId";
                    ddlExternalQSetterName.DataSource = ExternalEmpList;
                    ddlExternalQSetterName.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadExternalSEName()
        {
            try
            {

                string Dept = ddlExternalScriptExDept.SelectedValue.ToString();

                ddlExternalScriptExName.Items.Clear();
                ddlExternalScriptExName.AppendDataBoundItems = true;
                ddlExternalScriptExName.Items.Add(new ListItem("Select", "0"));

                var ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().ToList();
                if (Dept != "0")
                    ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Where(x => x.Department == Dept).ToList();

                if (ExternalEmpList != null && ExternalEmpList.Any())
                {
                    ddlExternalScriptExName.DataTextField = "Name";
                    ddlExternalScriptExName.DataValueField = "ExternalId";
                    ddlExternalScriptExName.DataSource = ExternalEmpList;
                    ddlExternalScriptExName.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }


        #region Not Used Methods

        //private void BindDDLExternalSetter(List<Employee> empList)
        //{
        //    try
        //    {
        //        ddlExternalQSetterName.Items.Clear();
        //        ddlExternalQSetterName.AppendDataBoundItems = true;
        //        ddlExternalQSetterName.Items.Add(new ListItem("Select", "0"));
        //        ddlExternalQSetterName.DataTextField = "CodeAndName";
        //        ddlExternalQSetterName.DataValueField = "EmployeeID";
        //        ddlExternalQSetterName.DataSource = empList;
        //        ddlExternalQSetterName.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void BindDDLExternalScriptEx(List<Employee> empList)
        //{
        //    try
        //    {
        //        ddlExternalScriptExName.Items.Clear();
        //        ddlExternalScriptExName.AppendDataBoundItems = true;
        //        ddlExternalScriptExName.Items.Add(new ListItem("Select", "0"));
        //        ddlExternalScriptExName.DataTextField = "CodeAndName";
        //        ddlExternalScriptExName.DataValueField = "EmployeeID";
        //        ddlExternalScriptExName.DataSource = empList;
        //        ddlExternalScriptExName.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        #endregion



        protected void ddlInternalQSetterDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();

                int IsUpdate = Convert.ToInt32(hdnIsUpdate.Value);

                int DeptId = Convert.ToInt32(ddlInternalQSetterDept.selectedValue);

                List<Employee> empList = new List<Employee>();
                if (Session["EmployeeList"] != null)
                    empList = (List<Employee>)Session["EmployeeList"];
                else
                    empList = EmployeeManager.GetAllByTypeId(1);

                if (DeptId > 0)
                    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                BindDDLInternalSetter(empList);
                if (IsUpdate == 0)
                {
                    ddlInternalScriptExDept.SelectedValue(DeptId);
                    ddlInternalScriptExDept_DepartmentSelectedIndexChanged(null, null);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlExternalQSetterDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int IsUpdate = Convert.ToInt32(hdnIsUpdate.Value);

                string Dept = ddlExternalQSetterDept.SelectedValue;

                //List<Employee> empList = new List<Employee>();
                //if (Session["EmployeeList"] != null)
                //    empList = (List<Employee>)Session["EmployeeList"];
                //else
                //    empList = EmployeeManager.GetAllByTypeId(1);

                //if (DeptId > 0)
                //    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                LoadExternalQSName();
                if (IsUpdate == 0)
                {
                    ddlExternalScriptExDept.SelectedValue = Dept;
                    ddlExternalScriptExDept_DepartmentSelectedIndexChanged(null, null);
                }


            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlInternalScriptExDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int DeptId = Convert.ToInt32(ddlInternalScriptExDept.selectedValue);

                List<Employee> empList = new List<Employee>();
                if (Session["EmployeeList"] != null)
                    empList = (List<Employee>)Session["EmployeeList"];
                else
                    empList = EmployeeManager.GetAllByTypeId(1);

                if (DeptId > 0)
                    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                BindDDLInternalScriptEx(empList);


            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlExternalScriptExDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                LoadExternalSEName();



                //int DeptId = Convert.ToInt32(ddlExternalScriptExDept.SelectedValue);

                //List<Employee> empList = new List<Employee>();
                //if (Session["EmployeeList"] != null)
                //    empList = (List<Employee>)Session["EmployeeList"];
                //else
                //    empList = EmployeeManager.GetAllByTypeId(1);

                //if (DeptId > 0)
                //    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                //BindDDLExternalScriptEx(empList);

            }
            catch (Exception ex)
            {
            }

        }

        protected void ddlInternalQSetterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int TeacherId = Convert.ToInt32(ddlInternalQSetterName.SelectedValue);
                int IsUpdate = Convert.ToInt32(hdnIsUpdate.Value);

                if (IsUpdate == 0)
                {
                    if (ddlInternalScriptExName.Items.FindByValue(TeacherId.ToString()) != null)
                    {
                        ddlInternalScriptExName.SelectedValue = TeacherId.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlExternalQSetterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int TeacherId = Convert.ToInt32(ddlExternalQSetterName.SelectedValue);
                int IsUpdate = Convert.ToInt32(hdnIsUpdate.Value);

                if (IsUpdate == 0)
                {
                    if (ddlExternalScriptExName.Items.FindByValue(TeacherId.ToString()) != null)
                    {
                        ddlExternalScriptExName.SelectedValue = TeacherId.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Save/Update Button Click Process


        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int InsertUpdateCount = 0;

                int IsUpdate = Convert.ToInt32(hdnIsUpdate.Value);

                if (IsUpdate > 0) // Update a Single Entry
                {
                    UpdateExistingEntry(IsUpdate);
                    InsertUpdateCount++;
                }

                else //Insert Or Update Selected Entry
                {
                    foreach (GridViewRow row in gvScheduleList.Rows)
                    {
                        try
                        {
                            CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                            if (ckBox.Checked)
                            {

                                HiddenField hdnAcaCalSectionID = (HiddenField)row.FindControl("hdnAcaCalSectionID");
                                HiddenField hdnSetupId = (HiddenField)row.FindControl("hdnSetupId");


                                int SectionId = Convert.ToInt32(hdnAcaCalSectionID.Value);
                                int SetupId = Convert.ToInt32(hdnSetupId.Value);

                                if (SetupId > 0) // Update
                                {
                                    UpdateExistingEntry(SetupId);
                                    InsertUpdateCount++;
                                }
                                else // Insert
                                {
                                    int InsertId = InsertNewEntry(SectionId);
                                    if (InsertId > 0)
                                        InsertUpdateCount++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }

                if (InsertUpdateCount > 0)
                {
                    showAlert("Information Updated Successfully");
                    LoadData();
                    return;
                }
                else
                {
                    showAlert("Information Update Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private int InsertNewEntry(int SectionId)
        {
            int Id = 0;
            try
            {
                int InternalQSetterId = Convert.ToInt32(ddlInternalQSetterName.SelectedValue);
                int ExternalQSetterId = Convert.ToInt32(ddlExternalQSetterName.SelectedValue);
                int InternalScriptExId = Convert.ToInt32(ddlInternalScriptExName.SelectedValue);
                int ExternalScriptExId = Convert.ToInt32(ddlExternalScriptExName.SelectedValue);

                DAL.QuestionSetterAndScriptExaminerInformation NewObj = new DAL.QuestionSetterAndScriptExaminerInformation();

                var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Where(x => x.AcacalSectionId == SectionId).FirstOrDefault();

                if (ExistingObj == null)
                {
                    NewObj.AcacalSectionId = SectionId;
                    NewObj.InternalQuestionSetterId = InternalQSetterId;
                    NewObj.ExternalQuestionSetterId = ExternalQSetterId;
                    NewObj.InternalScriptExaminerId = InternalScriptExId;
                    NewObj.ExternalScriptExaminerId = ExternalScriptExId;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.QuestionSetterAndScriptExaminerInformations.Add(NewObj);
                    ucamContext.SaveChanges();

                    Id = NewObj.Id;
                }
            }
            catch (Exception ex)
            {
            }
            return Id;
        }

        private void UpdateExistingEntry(int SetupId)
        {
            try
            {
                int InternalQSetterId = Convert.ToInt32(ddlInternalQSetterName.SelectedValue);
                int ExternalQSetterId = Convert.ToInt32(ddlExternalQSetterName.SelectedValue);
                int InternalScriptExId = Convert.ToInt32(ddlInternalScriptExName.SelectedValue);
                int ExternalScriptExId = Convert.ToInt32(ddlExternalScriptExName.SelectedValue);

                var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Find(SetupId);

                if (ExistingObj != null)
                {
                    ExistingObj.InternalQuestionSetterId = InternalQSetterId;
                    ExistingObj.ExternalQuestionSetterId = ExternalQSetterId;
                    ExistingObj.InternalScriptExaminerId = InternalScriptExId;
                    ExistingObj.ExternalScriptExaminerId = ExternalScriptExId;
                    ExistingObj.ModifiedBy = UserObj.Id;
                    ExistingObj.ModifiedDate = DateTime.Now;

                    ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                    ucamContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
            }
        }


        #endregion


        #region Edit And Remove Button Click Methods


        protected void DeleteMember_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        ucamContext.QuestionSetterAndScriptExaminerInformations.Remove(ExistingObj);
                        ucamContext.SaveChanges();

                        showAlert("Information Removed Successfully");
                        LoadData();
                        return;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void EditMember_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    hdnIsUpdate.Value = SetupId.ToString();
                    ModalPopUpInformation.Show();
                    var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        LoadDepartmentDDL();


                        List<Employee> empList = new List<Employee>();
                        if (Session["EmployeeList"] != null)
                            empList = (List<Employee>)Session["EmployeeList"];
                        else
                            empList = EmployeeManager.GetAllByTypeId(1);

                        var ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().ToList();

                        if (ExistingObj.InternalQuestionSetterId != null && ExistingObj.InternalScriptExaminerId != 0)
                        {
                            var emp = empList.Where(x => x.EmployeeID == ExistingObj.InternalQuestionSetterId).FirstOrDefault();
                            if (emp != null)
                            {
                                ddlInternalQSetterDept.SelectedValue(emp.DeptID);
                                ddlInternalQSetterDept_DepartmentSelectedIndexChanged(null, null);
                                ddlInternalQSetterName.SelectedValue = emp.EmployeeID.ToString();
                            }

                        }
                        else
                            ddlInternalQSetterDept_DepartmentSelectedIndexChanged(null, null);

                        if (ExistingObj.ExternalQuestionSetterId != null && ExistingObj.ExternalQuestionSetterId != 0)
                        {
                            if (ExternalEmpList != null && ExternalEmpList.Any())
                            {
                                var emp = ExternalEmpList.Where(x => x.ExternalId == ExistingObj.ExternalQuestionSetterId).FirstOrDefault();
                                if (emp != null)
                                {
                                    ddlExternalQSetterDept.SelectedValue = emp.Department.ToString();
                                    ddlExternalQSetterDept_DepartmentSelectedIndexChanged(null, null);
                                    ddlExternalQSetterName.SelectedValue = emp.ExternalId.ToString();
                                }
                            }

                        }
                        else
                            ddlExternalQSetterDept_DepartmentSelectedIndexChanged(null, null);

                        if (ExistingObj.InternalScriptExaminerId != null && ExistingObj.InternalScriptExaminerId != 0)
                        {
                            var emp = empList.Where(x => x.EmployeeID == ExistingObj.InternalScriptExaminerId).FirstOrDefault();
                            if (emp != null)
                            {
                                ddlInternalScriptExDept.SelectedValue(emp.DeptID);
                                ddlInternalScriptExDept_DepartmentSelectedIndexChanged(null, null);
                                ddlInternalScriptExName.SelectedValue = emp.EmployeeID.ToString();
                            }

                        }
                        else
                            ddlInternalScriptExDept_DepartmentSelectedIndexChanged(null, null);

                        if (ExistingObj.ExternalScriptExaminerId != null && ExistingObj.ExternalScriptExaminerId != 0)
                        {

                            if (ExternalEmpList != null && ExternalEmpList.Any())
                            {
                                var emp = ExternalEmpList.Where(x => x.ExternalId == ExistingObj.ExternalScriptExaminerId).FirstOrDefault();
                                if (emp != null)
                                {
                                    ddlExternalScriptExDept.SelectedValue = emp.Department.ToString();
                                    ddlExternalScriptExDept_DepartmentSelectedIndexChanged(null, null);
                                    ddlExternalScriptExName.SelectedValue = emp.ExternalId.ToString();
                                }
                            }

                        }
                        else
                            ddlExternalScriptExDept_DepartmentSelectedIndexChanged(null, null);


                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion


        private void ClearGridView()
        {
            gvScheduleList.DataSource = null;
            gvScheduleList.DataBind();
            btnUpdateInfo.Visible = false;
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


        #region Third Examiner Setup Process

        protected void AssignThirdExaminer_Click(object sender, EventArgs e)
        {
            try
            {
                hdnThirdExSetupId.Value = "0";
                ddlExaminerType.SelectedValue = "0";

                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    hdnThirdExSetupId.Value = SetupId.ToString();
                    ModalPopupThirdExaminer.Show();

                    DivInternal.Visible = false;
                    DivExternal.Visible = false;
                    divSave.Visible = false;



                    var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Find(SetupId);
                    if (ExistingObj != null && ExistingObj.ThirdExaminerId != null && ExistingObj.ThirdExaminerId != 0)
                    {
                        divSave.Visible = true;

                        int TeacherId = Convert.ToInt32(ExistingObj.ThirdExaminerId);

                        if (ExistingObj.Attribute1 == null || ExistingObj.Attribute1 == 0)// Third Examiner Is Internal
                        {
                            var EmpObj = ucamContext.Employees.Find(TeacherId);

                            if (EmpObj != null)
                            {
                                ddlExaminerType.SelectedValue = "0";
                                DivInternal.Visible = true;

                                ddlThirdExaminerDept.LoadDropDownList();
                                LoadThirdExaminer();

                                ddlThirdExaminerDept.SelectedValue(Convert.ToInt32(EmpObj.DeptID));
                                ddlThirdExaminerDept_DepartmentSelectedIndexChanged(null, null);

                                if (ddlThirdExaminerName.Items.FindByValue(TeacherId.ToString()) != null)
                                {
                                    ddlThirdExaminerName.SelectedValue = TeacherId.ToString();
                                }
                            }
                        }
                        else// Third Examiner Is External
                        {
                            ddlExaminerType.SelectedValue = "1";
                            DivExternal.Visible = true;

                            LoadExternalThirdExaminerDept();
                            LoadExternalThirdExaminer();

                            var ExternalObj = ucamContext.ExternalCommitteeMemberInformations.Find(TeacherId);
                            if (ExternalObj != null)
                            {
                                if (ddlExternalThirdExaminerDept.Items.FindByValue(ExternalObj.Department.ToString()) != null)
                                {
                                    ddlExternalThirdExaminerDept.SelectedValue = ExternalObj.Department.ToString();
                                }
                            }
                            ddlExternalThirdExaminerDept_SelectedIndexChanged(null, null);

                            if (ddlExternalThirdExaminer.Items.FindByValue(TeacherId.ToString()) != null)
                            {
                                ddlExternalThirdExaminer.SelectedValue = TeacherId.ToString();
                            }

                        }

                    }
                    else
                    {
                        ddlExaminerType_SelectedIndexChanged(null, null);
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadExternalThirdExaminerDept()
        {
            try
            {
                var ExternalDeptList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Select(x => x.Department).Distinct().ToList();


                ddlExternalThirdExaminerDept.Items.Clear();
                ddlExternalThirdExaminerDept.AppendDataBoundItems = true;
                ddlExternalThirdExaminerDept.Items.Add(new ListItem("All", "0"));

                if (ExternalDeptList != null && ExternalDeptList.Any())
                {
                    ddlExternalThirdExaminerDept.DataSource = ExternalDeptList;
                    ddlExternalThirdExaminerDept.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadExternalThirdExaminer()
        {
            try
            {

                string Dept = ddlExternalThirdExaminerDept.SelectedValue.ToString();

                ddlExternalThirdExaminer.Items.Clear();
                ddlExternalThirdExaminer.AppendDataBoundItems = true;
                ddlExternalThirdExaminer.Items.Add(new ListItem("Select", "0"));

                var ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().ToList();
                if (Dept != "0")
                    ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Where(x => x.Department == Dept).ToList();

                if (ExternalEmpList != null && ExternalEmpList.Any())
                {
                    ddlExternalThirdExaminer.DataTextField = "Name";
                    ddlExternalThirdExaminer.DataValueField = "ExternalId";
                    ddlExternalThirdExaminer.DataSource = ExternalEmpList;
                    ddlExternalThirdExaminer.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadThirdExaminer()
        {
            try
            {
                List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

                int DeptId = Convert.ToInt32(ddlThirdExaminerDept.selectedValue);

                ddlThirdExaminerName.Items.Clear();
                ddlThirdExaminerName.AppendDataBoundItems = true;
                ddlThirdExaminerName.Items.Add(new ListItem("Select", "0"));

                if (DeptId > 0)
                    empList = empList.Where(a => a.DeptID == DeptId).ToList();
                if (empList != null && empList.Any())
                {
                    ddlThirdExaminerName.DataTextField = "CodeAndName";
                    ddlThirdExaminerName.DataValueField = "EmployeeID";
                    ddlThirdExaminerName.DataSource = empList;
                    ddlThirdExaminerName.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlThirdExaminerDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupThirdExaminer.Show();
            LoadThirdExaminer();
        }

        protected void btnExaminerUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32(hdnThirdExSetupId.Value);

                if (SetupId > 0)
                {
                    int ExaminerType = Convert.ToInt32(ddlExaminerType.SelectedValue);

                    var ExistingObj = ucamContext.QuestionSetterAndScriptExaminerInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        bool NotEligible = false;
                        string user = "";
                        #region Condition Checking for Eligible Third Examiner

                        /// These Types of user not eligible for third examiner
                        /// 1) Course Teacher of this course
                        /// 2) Any Member of this exam committee
                        /// 3) Any Member of three tabulator

                        try
                        {
                            #region Checking Process

                            int TrdExId = Convert.ToInt32(ddlThirdExaminerName.SelectedValue);
                            if (ExaminerType == 0 && TrdExId > 0)
                            {
                                int HeldInRelationId = 0;
                                var SectionObj = ucamContext.AcademicCalenderSections.Find(Convert.ToInt32(ExistingObj.AcacalSectionId));
                                if (SectionObj != null && (SectionObj.TeacherOneID == TrdExId || SectionObj.TeacherTwoID == TrdExId))
                                {
                                    NotEligible = true;
                                    user = "User is course teacher.";
                                }
                                HeldInRelationId = Convert.ToInt32(SectionObj.HeldInRelationId);
                                if (!NotEligible)
                                {
                                    var ExamCommittee = ucamContext.ExamSetupWithExamCommittees.Where(x => x.HeldInProgramRelationId == HeldInRelationId).FirstOrDefault();

                                    if (ExamCommittee != null && (ExamCommittee.ExamCommitteeChairmanId == TrdExId || ExamCommittee.ExamCommitteeMemberOneId == TrdExId || ExamCommittee.ExamCommitteeMemberTwoId == TrdExId))
                                    {
                                        NotEligible = true;
                                        user = "User is a member of exam committee.";
                                    }
                                }

                                if (!NotEligible)
                                {
                                    var tabulator = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Where(x => x.HeldInProgramRelationId == HeldInRelationId).FirstOrDefault();

                                    if (tabulator != null && (tabulator.TabulatorOneId == TrdExId || tabulator.TabulatorTwoId == TrdExId || tabulator.TabulatorThreeId == TrdExId))
                                    {
                                        NotEligible = true;
                                        user = "User is a tabulator";
                                    }
                                }

                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                        }





                        #endregion

                        if (!NotEligible)
                        {
                            if (ExaminerType == 0)
                                ExistingObj.ThirdExaminerId = Convert.ToInt32(ddlThirdExaminerName.SelectedValue);
                            else
                                ExistingObj.ThirdExaminerId = Convert.ToInt32(ddlExternalThirdExaminer.SelectedValue);

                            ExistingObj.Attribute1 = ExaminerType;
                            ExistingObj.ModifiedBy = UserObj.Id;
                            ExistingObj.ModifiedDate = DateTime.Now;

                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                            showAlert("Third Examiner Updated Successfully");
                            LoadData();
                            return;
                        }
                        else
                        {
                            showAlert("User is not eligible for third examiner." + user);
                            ModalPopupThirdExaminer.Show();
                            return;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        protected void ddlExternalThirdExaminerDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupThirdExaminer.Show();
            LoadExternalThirdExaminer();
        }

        protected void ddlExaminerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopupThirdExaminer.Show();

                DivExternal.Visible = false;
                DivInternal.Visible = false;

                divSave.Visible = true;

                int ExaminerType = Convert.ToInt32(ddlExaminerType.SelectedValue);

                if (ExaminerType == 0) // Inetrnal
                {
                    ddlThirdExaminerDept.LoadDropDownList();
                    LoadThirdExaminer();
                    DivInternal.Visible = true;
                }
                else // External
                {
                    LoadExternalThirdExaminerDept();
                    LoadExternalThirdExaminer();
                    DivExternal.Visible = true;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkEligibleStudent_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SectionId = Convert.ToInt32(btn.CommandArgument);

                if (SectionId > 0)
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = SectionId });



                    DataTable dt = DataTableManager.GetDataFromQuery("GetThirdExaminerStudentListByAcacalSectionId", parameters1);

                    string Course = "";
                    AcademicCalenderSection ac = AcademicCalenderSectionManager.GetById(SectionId);
                    if (ac != null)
                        Course = ac.Course.FormalCode + "_" + ac.SectionName;

                    string fileName = Course + "ThirdExList" + ".xlsx";

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.AddWorksheet(dt, "Sheet");
                            //wb.Worksheets.Add(dt);

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);

                                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                                cookie.Value = "Flag";
                                cookie.Expires = DateTime.Now.AddDays(1);
                                Response.AppendCookie(cookie);

                                Response.Flush();
                                Response.SuppressContent = true;
                                Response.End();
                            }
                        };
                    }
                    else
                    {
                        showAlert("No Student Found.");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}