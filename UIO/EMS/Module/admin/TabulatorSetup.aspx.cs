using CommonUtility;
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

namespace EMS.Module.admin
{
    public partial class TabulatorSetup : BasePage
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
                ucDepartment.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);
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

                int YearNo = Convert.ToInt32(ddlExamYear.SelectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("Select", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    if (YearNo > 0)
                    {
                        string expression = "ExamYear ='" + YearNo + "'";
                        DataTableHeldInList = DataTableMethods.FilterDataTable(DataTableHeldInList, expression);
                    }
                }

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

        #endregion

        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadExamYear();
                LoadHeldInInformation();
                LoadData();
            }
            catch (Exception)
            {
            }
        }

        private void LoadExamYear()
        {
            try
            {

                ddlExamYear.Items.Clear();
                ddlExamYear.AppendDataBoundItems = true;
                ddlExamYear.Items.Add(new ListItem("Select", "0"));

                for (int i = DateTime.Now.Year; i > 1950; i--)
                {
                    ddlExamYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadHeldInInformation();
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

        private void ClearGridView()
        {
            try
            {
                gvScheduleList.DataSource = null;
                gvScheduleList.DataBind();
                btnUpdateInfo.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            LoadHeldInInformation();
            LoadData();
        }

        #endregion


        private void LoadData()
        {
            try
            {
                ClearGridView();


                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                parameters1.Add(new SqlParameter { ParameterName = "@ExamYear", SqlDbType = System.Data.SqlDbType.Int, Value = ExamYear });

                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetAllHeldInRelationWithTabulatorInformation", parameters1);

                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                {
                    gvScheduleList.DataSource = DataTableRoutineList;
                    gvScheduleList.DataBind();
                    Session["TabulatorList"] = DataTableRoutineList;
                    ViewState["sortdrTabulator"] = "Asc";
                    btnUpdateInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
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

        protected void gvScheduleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["TabulatorList"];

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


        #region Modal DropDown Binding

        private void LoadDepartmentDDL()
        {
            try
            {
                ddlTabOneDept.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);
                ddlTabTwoDept.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);
                ddlTabThreeDept.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);

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

                int DeptId = Convert.ToInt32(ddlTabOneDept.selectedValue);



                List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

                Session["EmployeeList"] = empList;

                if (DeptId > 0)
                    empList = empList.Where(a => a.DeptID == DeptId).ToList();

                if (empList != null && empList.Any())
                {
                    empList = empList.OrderBy(a => a.CodeAndName).ToList();

                    BindDDLTabulatorOne(empList);
                    BindDDLTabulatorTwo(empList);
                    BindDDLTabulatorThree(empList);

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindDDLTabulatorOne(List<Employee> empList)
        {
            try
            {
                ddlTabOneName.Items.Clear();
                ddlTabOneName.AppendDataBoundItems = true;
                ddlTabOneName.Items.Add(new ListItem("Select", "0"));
                {
                    ddlTabOneName.DataTextField = "CodeAndName";
                    ddlTabOneName.DataValueField = "EmployeeID";
                    ddlTabOneName.DataSource = empList.OrderBy(x=>x.CodeAndName);
                    ddlTabOneName.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindDDLTabulatorTwo(List<Employee> empList)
        {
            try
            {
                ddlTabTwoName.Items.Clear();
                ddlTabTwoName.AppendDataBoundItems = true;
                ddlTabTwoName.Items.Add(new ListItem("Select", "0"));
                if (empList != null && empList.Any())
                {
                    ddlTabTwoName.DataTextField = "CodeAndName";
                    ddlTabTwoName.DataValueField = "EmployeeID";
                    ddlTabTwoName.DataSource = empList.OrderBy(x => x.CodeAndName); ;
                    ddlTabTwoName.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindDDLTabulatorThree(List<Employee> empList)
        {
            try
            {
                ddlTabThreeName.Items.Clear();
                ddlTabThreeName.AppendDataBoundItems = true;
                ddlTabThreeName.Items.Add(new ListItem("Select", "0"));
                if (empList != null && empList.Any())
                {
                    ddlTabThreeName.DataTextField = "CodeAndName";
                    ddlTabThreeName.DataValueField = "EmployeeID";
                    ddlTabThreeName.DataSource = empList.OrderBy(x => x.CodeAndName); ;
                    ddlTabThreeName.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlTabOneDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int DeptId = Convert.ToInt32(ddlTabOneDept.selectedValue);

                List<Employee> empList = new List<Employee>();
                if (Session["EmployeeList"] != null)
                    empList = (List<Employee>)Session["EmployeeList"];
                else
                    empList = EmployeeManager.GetAllByTypeId(1);

                //if (DeptId > 0) // For All Load
                if (DeptId == 0)
                    empList = null;
                else
                    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                BindDDLTabulatorOne(empList);

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlTabTwoDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int DeptId = Convert.ToInt32(ddlTabTwoDept.selectedValue);

                List<Employee> empList = new List<Employee>();
                if (Session["EmployeeList"] != null)
                    empList = (List<Employee>)Session["EmployeeList"];
                else
                    empList = EmployeeManager.GetAllByTypeId(1);

                //if (DeptId > 0)// For All Load
                if (DeptId == 0)
                    empList = null;
                else
                    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                BindDDLTabulatorTwo(empList);

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlTabThreeDept_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopUpInformation.Show();
                int DeptId = Convert.ToInt32(ddlTabThreeDept.selectedValue);

                List<Employee> empList = new List<Employee>();
                if (Session["EmployeeList"] != null)
                    empList = (List<Employee>)Session["EmployeeList"];
                else
                    empList = EmployeeManager.GetAllByTypeId(1);

                //if (DeptId > 0) // For All Load
                if (DeptId == 0)
                    empList = null;
                else
                    empList = empList.Where(x => x.DeptID == DeptId).ToList();

                BindDDLTabulatorThree(empList);

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

                int ProgramId=Convert.ToInt32(ucDepartment.selectedValue);

                if (Selected > 0)
                {
                    ModalPopUpInformation.Show();
                    hdnIsUpdate.Value = "0";
                    LoadDepartmentDDL();
                    ddlTabOneDept.selectedValue = ProgramId.ToString();
                    ddlTabTwoDept.selectedValue = ProgramId.ToString();
                    ddlTabThreeDept.selectedValue = ProgramId.ToString();
                    LoadAllFacultyDDL();
                }
                else
                {
                    showAlert("Please select minimum one Exam Held In Name");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }


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

                                HiddenField hdnRelationId = (HiddenField)row.FindControl("hdnRelationId");
                                HiddenField hdnSetupId = (HiddenField)row.FindControl("hdnSetupId");


                                int RelationId = Convert.ToInt32(hdnRelationId.Value);
                                int SetupId = Convert.ToInt32(hdnSetupId.Value);

                                if (SetupId > 0) // Update
                                {
                                    UpdateExistingEntry(SetupId);
                                    InsertUpdateCount++;
                                }
                                else // Insert
                                {
                                    int InsertId = InsertNewEntry(RelationId);
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

        private int InsertNewEntry(int RelationId)
        {
            int Id = 0;
            try
            {
                int TabOne = Convert.ToInt32(ddlTabOneName.SelectedValue);
                int TabTwo = Convert.ToInt32(ddlTabTwoName.SelectedValue);
                int TabThree = Convert.ToInt32(ddlTabThreeName.SelectedValue);

                DAL.ExamHeldInRelationWiseTabulatorInformation NewObj = new DAL.ExamHeldInRelationWiseTabulatorInformation();

                var ExistingObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Where(x => x.HeldInProgramRelationId == RelationId).FirstOrDefault();

                if (ExistingObj == null)
                {
                    NewObj.HeldInProgramRelationId = RelationId;
                    NewObj.TabulatorOneId = TabOne;
                    NewObj.TabulatorTwoId = TabTwo;
                    NewObj.TabulatorThreeId = TabThree;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.ExamHeldInRelationWiseTabulatorInformations.Add(NewObj);
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
                int TabOne = Convert.ToInt32(ddlTabOneName.SelectedValue);
                int TabTwo = Convert.ToInt32(ddlTabTwoName.SelectedValue);
                int TabThree = Convert.ToInt32(ddlTabThreeName.SelectedValue);

                var ExistingObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Find(SetupId);

                if (ExistingObj != null)
                {
                    ExistingObj.TabulatorOneId = TabOne;
                    ExistingObj.TabulatorTwoId = TabTwo;
                    ExistingObj.TabulatorThreeId = TabThree;
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
                    var ExistingObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        ucamContext.ExamHeldInRelationWiseTabulatorInformations.Remove(ExistingObj);
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
                    int DepartmentId = Convert.ToInt32(ucDepartment.selectedValue);

                    hdnIsUpdate.Value = SetupId.ToString();
                    ModalPopUpInformation.Show();
                    var ExistingObj = ucamContext.ExamHeldInRelationWiseTabulatorInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        LoadDepartmentDDL();


                        List<Employee> empList = new List<Employee>();
                        if (Session["EmployeeList"] != null)
                            empList = (List<Employee>)Session["EmployeeList"];
                        else
                            empList = EmployeeManager.GetAllByTypeId(1);

                        if (ExistingObj.TabulatorOneId != null && ExistingObj.TabulatorOneId != 0)
                        {
                            var emp = empList.Where(x => x.EmployeeID == ExistingObj.TabulatorOneId).FirstOrDefault();
                            if (emp != null)
                            {
                                ddlTabOneDept.selectedValue=emp.DeptID.ToString();
                                ddlTabOneDept_DepartmentSelectedIndexChanged(null, null);
                                ddlTabOneName.SelectedValue = emp.EmployeeID.ToString();
                            }

                        }
                        else
                        {
                            ddlTabOneDept.selectedValue = DepartmentId.ToString();
                            ddlTabOneDept_DepartmentSelectedIndexChanged(null, null);
                        }

                        if (ExistingObj.TabulatorTwoId != null && ExistingObj.TabulatorTwoId != 0)
                        {
                            var emp = empList.Where(x => x.EmployeeID == ExistingObj.TabulatorTwoId).FirstOrDefault();
                            if (emp != null)
                            {
                                ddlTabTwoDept.selectedValue = emp.DeptID.ToString();
                                ddlTabTwoDept_DepartmentSelectedIndexChanged(null, null);
                                ddlTabTwoName.SelectedValue = emp.EmployeeID.ToString();
                            }
                        }
                        else
                        {
                            ddlTabTwoDept.selectedValue = DepartmentId.ToString();
                            ddlTabTwoDept_DepartmentSelectedIndexChanged(null, null);
                        }

                        if (ExistingObj.TabulatorThreeId != null && ExistingObj.TabulatorThreeId != 0)
                        {
                            var emp = empList.Where(x => x.EmployeeID == ExistingObj.TabulatorThreeId).FirstOrDefault();
                            if (emp != null)
                            {
                                ddlTabThreeDept.selectedValue = emp.DeptID.ToString();
                                ddlTabThreeDept_DepartmentSelectedIndexChanged(null, null);
                                ddlTabThreeName.SelectedValue = emp.EmployeeID.ToString();
                            }

                        }
                        else
                        {
                            ddlTabThreeDept.selectedValue = DepartmentId.ToString();
                            ddlTabThreeDept_DepartmentSelectedIndexChanged(null, null);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

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

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


    }
}