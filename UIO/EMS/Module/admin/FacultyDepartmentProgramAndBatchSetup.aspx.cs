using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class FacultyDepartmentProgramAndBatchSetup : BasePage
    {

        //string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        //string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.ExternalCommitteeMemberInformationSetup);
        //string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.ExternalCommitteeMemberInformationSetup));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                DivFaculty.Visible = false;
                DivDepartment.Visible = false;
                DivProgram.Visible = false;
                DivBatch.Visible = false;

                DivProgramAdd.Visible = false;
                DivBatchAdd.Visible = false;


                DivFacultyAdd.Visible = false;
                DivFacultyGrid.Visible = false;
                LoadFaculty();

                DivDeptAdd.Visible = false;
                DivDeptGrid.Visible = false;
                LoadDepartment();



            }
        }



        #region Faculty Setup Information

        private void LoadFaculty()
        {
            try
            {
                ddlFaculty.Items.Clear();
                ddlFaculty.AppendDataBoundItems = true;
                ddlFaculty.Items.Add(new ListItem("All", "0"));

                var FacultyList = CommonMethodForFacultyDepartmentProgramBatch.AllFacultyList();

                if (FacultyList != null && FacultyList.Count>0)
                {
                    ddlFaculty.DataTextField = "FacultyName";
                    ddlFaculty.DataValueField = "Id";
                    ddlFaculty.DataSource = FacultyList;
                    ddlFaculty.DataBind();

                    BindFacultyGrid();


                }

            }
            catch (Exception ex)
            {

            }
        }

        private void BindFacultyGrid()
        {
            try
            {
                int FacultyId = Convert.ToInt32(ddlFaculty.SelectedValue);
                List<DAL.FacultyInformation> FacultyList = CommonMethodForFacultyDepartmentProgramBatch.AllFacultyList();
                if (FacultyId > 0)
                    FacultyList = FacultyList.Where(x => x.Id == FacultyId).ToList();
                if (FacultyList != null && FacultyList.Count>0)
                {
                    DivFacultyGrid.Visible = true;
                    gvFacultyList.DataSource = FacultyList;
                    gvFacultyList.DataBind();
                }
                else
                    ClearFacultyGrid();

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearFacultyGrid()
        {
            DivFacultyGrid.Visible = false;
            gvFacultyList.DataSource = null;
            gvFacultyList.DataBind();
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearFacultyGrid();
            BindFacultyGrid();
        }

        protected void lnkAddNewFaculty_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFacultyField();
                hdnFacultySetupId.Value = "0";
                DivFacultyAdd.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkFacultyCancel_Click(object sender, EventArgs e)
        {
            hdnFacultySetupId.Value = "0";
            DivFacultyAdd.Visible = false;
        }

        protected void lnkFacultySave_Click(object sender, EventArgs e)
        {
            try
            {

                int SetupId = Convert.ToInt32(hdnFacultySetupId.Value);


                string Name = "", Code = "";
                if (string.IsNullOrEmpty(txtFacultyName.Text))
                {
                    showAlert("Please enter faculty name");
                    return;
                }
                if (string.IsNullOrEmpty(txtFacultyCode.Text))
                {
                    showAlert("Please enter faculty code");
                    return;
                }

                Name = txtFacultyName.Text.Trim();
                Code = txtFacultyCode.Text.Trim();

                var ExistingObj = ucamContext.FacultyInformations.Where(x => (x.FacultyName == Name || x.FacultyCode == Code) && x.Id != SetupId).FirstOrDefault();

                if (ExistingObj != null)
                {
                    showAlert("Faculty already exists with the Name or Code");
                    return;
                }

                if (SetupId == 0) // New Entry
                {
                    DAL.FacultyInformation NewObj = new DAL.FacultyInformation();

                    NewObj.FacultyName = Name;
                    NewObj.OfficialFacultyName = Name;
                    NewObj.FacultyCode = Code;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.FacultyInformations.Add(NewObj);
                    ucamContext.SaveChanges();

                    int Id = NewObj.Id;

                    if (Id > 0)
                    {
                        showAlert("Faculty Added Successfully");
                        LoadFaculty();
                    }
                    else
                    {
                        showAlert("Faculty Add Failed");
                    }

                }
                else // Udpate Existing Entry
                {
                    var UpdatingObj = ucamContext.FacultyInformations.Find(SetupId);
                    if (UpdatingObj != null)
                    {
                        hdnFacultySetupId.Value = "0";

                        UpdatingObj.FacultyName = Name;
                        UpdatingObj.OfficialFacultyName = Name;
                        UpdatingObj.FacultyCode = Code;
                        UpdatingObj.ModifiedBy = UserObj.Id;
                        UpdatingObj.ModifiedDate = DateTime.Now;
                        ucamContext.Entry(UpdatingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("Faculty Updated Successfully");
                        LoadFaculty();
                    }
                }
                ClearFacultyField();
                DivFacultyAdd.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void editFaculty_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFacultyField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.FacultyInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        hdnFacultySetupId.Value = SetupId.ToString();
                        txtFacultyName.Text = ExistingObj.FacultyName;
                        txtFacultyCode.Text = ExistingObj.FacultyCode;

                        DivFacultyAdd.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void RemoveFaculty_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.FacultyInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        var IsProgramExists = CommonMethodForFacultyDepartmentProgramBatch.AllProgramListByParameter(SetupId, 0, 0, 0);
                        if (IsProgramExists != null && IsProgramExists.Count>0)
                        {
                            showAlert("Can not remove.Some program exsits under this faculty");
                            return;
                        }
                        else
                        {
                            ucamContext.FacultyInformations.Remove(ExistingObj);
                            ucamContext.SaveChanges();

                            showAlert("Faculty Removed Successfully");
                            LoadFaculty();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearFacultyField()
        {
            try
            {
                txtFacultyName.Text = "";
                txtFacultyCode.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Department Setup Information

        private void LoadDepartment()
        {
            try
            {
                ddlDepartment.Items.Clear();
                ddlDepartment.AppendDataBoundItems = true;
                ddlDepartment.Items.Add(new ListItem("All", "0"));

                var DepartmentList = CommonMethodForFacultyDepartmentProgramBatch.AllDepartmentList();

                if (DepartmentList != null && DepartmentList.Count>0)
                {
                    ddlDepartment.DataTextField = "DetailedName";
                    ddlDepartment.DataValueField = "DeptID";
                    ddlDepartment.DataSource = DepartmentList;
                    ddlDepartment.DataBind();

                    BindDepartmentGrid();


                }

            }
            catch (Exception ex)
            {

            }
        }

        private void BindDepartmentGrid()
        {
            try
            {
                int DeptId = Convert.ToInt32(ddlDepartment.SelectedValue);
                List<DAL.Department> DeptList = CommonMethodForFacultyDepartmentProgramBatch.AllDepartmentList();
                if (DeptId > 0)
                    DeptList = DeptList.Where(x => x.DeptID == DeptId).ToList();
                if (DeptList != null && DeptList.Count>0)
                {
                    DivDeptGrid.Visible = true;
                    gvDeptList.DataSource = DeptList;
                    gvDeptList.DataBind();
                }
                else
                    ClearDeptGrid();

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearDeptGrid()
        {
            DivDeptGrid.Visible = false;
            gvDeptList.DataSource = null;
            gvDeptList.DataBind();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDeptGrid();
            BindDepartmentGrid();
        }

        protected void lnkAddNewDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDepartmentField();
                hdnDepartmentSetupId.Value = "0";
                DivDeptAdd.Visible = true;

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                txtDeptOpeningDate.Text = startDate.ToString("dd/MM/yyyy");
                txtDeptClosingDate.Text = endDate.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkDeptCancel_Click(object sender, EventArgs e)
        {
            hdnDepartmentSetupId.Value = "0";
            DivDeptAdd.Visible = false;
        }

        protected void lnkDeptSave_Click(object sender, EventArgs e)
        {
            try
            {

                int SetupId = Convert.ToInt32(hdnDepartmentSetupId.Value);


                string Name = "", detailName = "", Code = "";
                if (string.IsNullOrEmpty(txtDeptName.Text))
                {
                    showAlert("Please enter department name");
                    return;
                }
                if (string.IsNullOrEmpty(txtDeptCode.Text))
                {
                    showAlert("Please enter department code");
                    return;
                }
                if (string.IsNullOrEmpty(txtDeptDetailName.Text))
                {
                    showAlert("Please enter department detail name");
                    return;
                }

                Name = txtDeptName.Text.Trim();
                Code = txtDeptCode.Text.Trim();
                detailName = txtDeptDetailName.Text.Trim();

                var ExistingObj = ucamContext.Departments.Where(x => (x.Name == Name || x.Code == Code || x.DetailedName == detailName) && x.DeptID != SetupId).FirstOrDefault();

                if (ExistingObj != null)
                {
                    showAlert("Department already exists with the Name or Detail Name or Code");
                    return;
                }

                if (SetupId == 0) // New Entry
                {
                    DAL.Department NewObj = new DAL.Department();

                    NewObj.Name = Name;
                    NewObj.DetailedName = detailName;
                    NewObj.Code = Code;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(txtDeptOpeningDate.Text))
                    {
                        DateTime FromDate = DateTime.ParseExact(txtDeptOpeningDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        NewObj.OpeningDate = FromDate;
                    }
                    else
                        NewObj.OpeningDate = null;

                    if (!string.IsNullOrEmpty(txtDeptClosingDate.Text))
                    {
                        DateTime ToDate = DateTime.ParseExact(txtDeptClosingDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        NewObj.ClosingDate = ToDate;
                    }
                    else
                        NewObj.ClosingDate = null;

                    ucamContext.Departments.Add(NewObj);
                    ucamContext.SaveChanges();

                    int Id = NewObj.DeptID;

                    if (Id > 0)
                    {
                        showAlert("Department Added Successfully");
                        LoadDepartment();
                    }
                    else
                    {
                        showAlert("Department Add Failed");
                    }

                }
                else // Udpate Existing Entry
                {
                    var UpdatingObj = ucamContext.Departments.Find(SetupId);
                    if (UpdatingObj != null)
                    {
                        hdnDepartmentSetupId.Value = "0";

                        UpdatingObj.Name = Name;
                        UpdatingObj.DetailedName = detailName;
                        UpdatingObj.Code = Code;
                        UpdatingObj.ModifiedBy = UserObj.Id;
                        UpdatingObj.ModifiedDate = DateTime.Now;

                        if (!string.IsNullOrEmpty(txtDeptOpeningDate.Text))
                        {
                            DateTime FromDate = DateTime.ParseExact(txtDeptOpeningDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            UpdatingObj.OpeningDate = FromDate;
                        }
                        else
                            UpdatingObj.OpeningDate = null;

                        if (!string.IsNullOrEmpty(txtDeptClosingDate.Text))
                        {
                            DateTime ToDate = DateTime.ParseExact(txtDeptClosingDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            UpdatingObj.ClosingDate = ToDate;
                        }
                        else
                            UpdatingObj.ClosingDate = null;

                        ucamContext.Entry(UpdatingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("Department Updated Successfully");
                        LoadDepartment();
                    }
                }
                ClearDepartmentField();
                DivDeptAdd.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void EditDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDepartmentField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Departments.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        hdnDepartmentSetupId.Value = SetupId.ToString();

                        txtDeptName.Text = ExistingObj.Name;
                        txtDeptDetailName.Text = ExistingObj.DetailedName;
                        txtDeptCode.Text = ExistingObj.Code;

                        if (ExistingObj.OpeningDate != null)
                        {
                            DateTime startDate = (DateTime)ExistingObj.OpeningDate;
                            txtDeptOpeningDate.Text = startDate.ToString("dd/MM/yyyy");
                        }
                        if (ExistingObj.ClosingDate != null)
                        {
                            DateTime endDate = (DateTime)ExistingObj.ClosingDate;
                            txtDeptClosingDate.Text = endDate.ToString("dd/MM/yyyy");
                        }


                        DivDeptAdd.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void RemoveDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Departments.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        var IsProgramExists = CommonMethodForFacultyDepartmentProgramBatch.AllProgramListByParameter(0, SetupId, 0, 0);
                        if (IsProgramExists != null && IsProgramExists.Count>0)
                        {
                            showAlert("Can not remove.Some program exsits under this department");
                            return;
                        }
                        else
                        {
                            ucamContext.Departments.Remove(ExistingObj);
                            ucamContext.SaveChanges();

                            showAlert("Department Removed Successfully");
                            LoadDepartment();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearDepartmentField()
        {
            try
            {
                txtDeptName.Text = "";
                txtDeptDetailName.Text = "";
                txtDeptCode.Text = "";
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                txtDeptOpeningDate.Text = startDate.ToString("dd/MM/yyyy");
                txtDeptClosingDate.Text = endDate.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Program Setup Information

        private void LoadProgramFaculty()
        {
            try
            {
                ddlProgramFaculty.Items.Clear();
                ddlProgramFaculty.AppendDataBoundItems = true;
                ddlProgramFaculty.Items.Add(new ListItem("All", "0"));

                var FacultyList = CommonMethodForFacultyDepartmentProgramBatch.AllFacultyList();

                if (FacultyList != null && FacultyList.Count>0)
                {
                    ddlProgramFaculty.DataTextField = "FacultyName";
                    ddlProgramFaculty.DataValueField = "Id";
                    ddlProgramFaculty.DataSource = FacultyList;
                    ddlProgramFaculty.DataBind();
                }


                ddlAddProgramFaculty.Items.Clear();
                ddlAddProgramFaculty.AppendDataBoundItems = true;
                ddlAddProgramFaculty.Items.Add(new ListItem("Select", "0"));

                if (FacultyList != null && FacultyList.Count>0)
                {
                    ddlAddProgramFaculty.DataTextField = "FacultyName";
                    ddlAddProgramFaculty.DataValueField = "Id";
                    ddlAddProgramFaculty.DataSource = FacultyList;
                    ddlAddProgramFaculty.DataBind();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LoadProgramDepartment()
        {
            try
            {
                ddlProgramDepartment.Items.Clear();
                ddlProgramDepartment.AppendDataBoundItems = true;
                ddlProgramDepartment.Items.Add(new ListItem("All", "0"));

                var DepartmentList = CommonMethodForFacultyDepartmentProgramBatch.AllDepartmentList();

                if (DepartmentList != null && DepartmentList.Count>0)
                {
                    ddlProgramDepartment.DataTextField = "DetailedName";
                    ddlProgramDepartment.DataValueField = "DeptID";
                    ddlProgramDepartment.DataSource = DepartmentList;
                    ddlProgramDepartment.DataBind();
                }

                ddlAddProgramDepartment.Items.Clear();
                ddlAddProgramDepartment.AppendDataBoundItems = true;
                ddlAddProgramDepartment.Items.Add(new ListItem("Select", "0"));

                if (DepartmentList != null && DepartmentList.Count>0)
                {
                    ddlAddProgramDepartment.DataTextField = "DetailedName";
                    ddlAddProgramDepartment.DataValueField = "DeptID";
                    ddlAddProgramDepartment.DataSource = DepartmentList;
                    ddlAddProgramDepartment.DataBind();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LoadProgram()
        {
            try
            {
                ddlProgram.Items.Clear();
                ddlProgram.AppendDataBoundItems = true;
                ddlProgram.Items.Add(new ListItem("All", "0"));

                int FacultyId = Convert.ToInt32(ddlProgramFaculty.SelectedValue);
                int DeptId = Convert.ToInt32(ddlProgramDepartment.SelectedValue);


                var ProgramList = CommonMethodForFacultyDepartmentProgramBatch.AllProgramListByParameter(FacultyId, DeptId, 0, 0);

                if (ProgramList != null && ProgramList.Count>0)
                {
                    ddlProgram.DataTextField = "ShortName";
                    ddlProgram.DataValueField = "ProgramID";
                    ddlProgram.DataSource = ProgramList;
                    ddlProgram.DataBind();

                    BindProgramGrid();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LoadProgramType()
        {
            try
            {
                ddlProgramType.Items.Clear();
                ddlProgramType.AppendDataBoundItems = true;
                ddlProgramType.Items.Add(new ListItem("Select", "0"));

                var programTypeList = ucamContext.ProgramTypes.ToList();

                if (programTypeList != null && programTypeList.Count>0)
                {
                    ddlProgramType.DataTextField = "TypeDescription";
                    ddlProgramType.DataValueField = "ProgramTypeID";
                    ddlProgramType.DataSource = programTypeList;
                    ddlProgramType.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCalender()
        {
            try
            {
                ddlCalenderType.Items.Clear();
                ddlCalenderType.AppendDataBoundItems = true;
                ddlCalenderType.Items.Add(new ListItem("Select", "0"));

                var CalenderList = ucamContext.CalenderUnitMasters.ToList();

                if (CalenderList != null && CalenderList.Count>0)
                {
                    ddlCalenderType.DataTextField = "Name";
                    ddlCalenderType.DataValueField = "CalenderUnitMasterID";
                    ddlCalenderType.DataSource = CalenderList;
                    ddlCalenderType.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindProgramGrid()
        {
            try
            {
                int FacultyId = Convert.ToInt32(ddlProgramFaculty.SelectedValue);
                int DeptId = Convert.ToInt32(ddlProgramDepartment.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlProgram.SelectedValue);

                var ProgramList = CommonMethodForFacultyDepartmentProgramBatch.AllProgramListByParameter(FacultyId, DeptId, 0, 0);

                if (ProgramId > 0 && ProgramList != null && ProgramList.Count>0)
                    ProgramList = ProgramList.Where(x => x.ProgramID == ProgramId).ToList();

                if (ProgramList != null && ProgramList.Count>0)
                {

                    var FacultyList = ucamContext.FacultyInformations.ToList();
                    var DepartmentList = ucamContext.Departments.ToList();


                    foreach (var item in ProgramList)
                    {
                        try
                        {
                            var Fac = FacultyList.Where(x => x.Id == item.FacultyId).FirstOrDefault();
                            var Dpt = DepartmentList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();

                            if (Fac != null)
                                item.Attribute1 = Fac.FacultyName;
                            else
                                item.Attribute1 = "";

                            if (Dpt != null)
                                item.Attribute2 = Dpt.Name;
                            else
                                item.Attribute2 = "";
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    DivProgramGrid.Visible = true;
                    gvProgramList.DataSource = ProgramList;
                    gvProgramList.DataBind();
                }
                else
                    ClearProgramGrid();

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearProgramGrid()
        {
            DivProgramGrid.Visible = false;
            gvProgramList.DataSource = null;
            gvProgramList.DataBind();
        }

        protected void ddlProgramFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgram();
            BindProgramGrid();
        }

        protected void ddlProgramDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgram();
            BindProgramGrid();
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProgramGrid();
        }

        protected void lnkAddNewProgram_Click(object sender, EventArgs e)
        {
            DivProgramAdd.Visible = true;
            hdnProgramId.Value = "0";
        }

        protected void lnkProgramCancel_Click(object sender, EventArgs e)
        {
            DivProgramAdd.Visible = false;
        }

        protected void lnkProgramSave_Click(object sender, EventArgs e)
        {
            try
            {

                int SetupId = Convert.ToInt32(hdnProgramId.Value);

                int FacultyId = Convert.ToInt32(ddlAddProgramFaculty.SelectedValue);
                int DeptId = Convert.ToInt32(ddlAddProgramDepartment.SelectedValue);
                int ProgramType = Convert.ToInt32(ddlProgramType.SelectedValue);
                int CalenderType = Convert.ToInt32(ddlCalenderType.SelectedValue);
                string ProgramName = string.IsNullOrEmpty(txtProgramName.Text) ? "" : txtProgramName.Text.Trim();
                string DetailName = string.IsNullOrEmpty(txtProgramDetailName.Text) ? "" : txtProgramDetailName.Text.Trim();
                string Code = string.IsNullOrEmpty(txtProgramCode.Text) ? "" : txtProgramCode.Text.Trim();
                string ShortName = string.IsNullOrEmpty(txtProgramShortName.Text) ? "" : txtProgramShortName.Text.Trim();

                decimal Credit = string.IsNullOrEmpty(txtTotalCredit.Text) ? 0 : Convert.ToDecimal(txtTotalCredit.Text.Trim());
                int Duration = string.IsNullOrEmpty(txtDuration.Text) ? 0 : Convert.ToInt32(txtDuration.Text.Trim());


                if (FacultyId == 0 || DeptId == 0 || ProgramType == 0 || CalenderType == 0 || ProgramName == "" || DetailName == "" || Code == "" || ShortName == "" || Credit == 0 || Duration == 0)
                {
                    showAlert("Please fill up all mandatory field");
                    return;
                }


                var ExistingObj = ucamContext.Programs.Where(x => x.ShortName == ShortName && x.DeptID == DeptId && x.FacultyId == FacultyId && x.ProgramID != SetupId).FirstOrDefault();

                if (ExistingObj != null)
                {
                    showAlert("Program already exists");
                    return;
                }

                if (SetupId == 0) // New Entry
                {
                    DAL.Program NewObj = new DAL.Program();

                    NewObj.FacultyId = FacultyId;
                    NewObj.DeptID = DeptId;
                    NewObj.DegreeName = ProgramName;
                    NewObj.Code = Code;
                    NewObj.ShortName = ShortName;
                    NewObj.TotalCredit = Credit;
                    NewObj.DeptID = DeptId;
                    NewObj.DetailName = DetailName;
                    NewObj.ProgramTypeID = ProgramType;
                    NewObj.CalenderUnitMasterID = CalenderType;
                    NewObj.Duration = Duration;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.Programs.Add(NewObj);
                    ucamContext.SaveChanges();

                    int Id = NewObj.ProgramID;

                    if (Id > 0)
                    {
                        showAlert("Program Added Successfully");
                        LoadProgram();
                    }
                    else
                    {
                        showAlert("Department Add Failed");
                    }

                }
                else // Udpate Existing Entry
                {
                    var UpdatingObj = ucamContext.Programs.Find(SetupId);
                    if (UpdatingObj != null)
                    {
                        hdnDepartmentSetupId.Value = "0";

                        UpdatingObj.FacultyId = FacultyId;
                        UpdatingObj.DeptID = DeptId;
                        UpdatingObj.DegreeName = ProgramName;
                        UpdatingObj.Code = Code;
                        UpdatingObj.ShortName = ShortName;
                        UpdatingObj.TotalCredit = Credit;
                        UpdatingObj.DeptID = DeptId;
                        UpdatingObj.DetailName = DetailName;
                        UpdatingObj.ProgramTypeID = ProgramType;
                        UpdatingObj.CalenderUnitMasterID = CalenderType;
                        UpdatingObj.Duration = Duration;
                        UpdatingObj.ModifiedBy = UserObj.Id;
                        UpdatingObj.ModifiedDate = DateTime.Now;


                        ucamContext.Entry(UpdatingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("Program Updated Successfully");
                        LoadProgram();
                    }
                }
                ClearProgramField();
                DivProgramAdd.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void EditProgram_Click(object sender, EventArgs e)
        {
            try
            {
                ClearProgramField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Programs.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        hdnProgramId.Value = SetupId.ToString();

                        ddlAddProgramFaculty.SelectedValue = ExistingObj.FacultyId == null ? "0" : ExistingObj.FacultyId.ToString();
                        ddlAddProgramDepartment.SelectedValue = ExistingObj.DeptID.ToString();
                        ddlProgramType.SelectedValue = ExistingObj.ProgramTypeID == null ? "0" : ExistingObj.ProgramTypeID.ToString();
                        ddlCalenderType.SelectedValue = ExistingObj.CalenderUnitMasterID == null ? "0" : ExistingObj.CalenderUnitMasterID.ToString();
                        txtProgramName.Text = ExistingObj.DegreeName == null ? "" : ExistingObj.DegreeName.ToString();
                        txtProgramDetailName.Text = ExistingObj.DetailName == null ? "" : ExistingObj.DetailName.ToString();
                        txtProgramCode.Text = ExistingObj.Code == null ? "" : ExistingObj.Code.ToString();
                        txtProgramShortName.Text = ExistingObj.ShortName == null ? "" : ExistingObj.ShortName.ToString();
                        txtTotalCredit.Text = ExistingObj.TotalCredit == null ? "" : ExistingObj.TotalCredit.ToString();
                        txtDuration.Text = ExistingObj.Duration == null ? "" : ExistingObj.Duration.ToString();


                        DivProgramAdd.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void RemoveProgram_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Programs.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        var IsBatchExists = ucamContext.Batches.Where(x => x.ProgramId == SetupId).ToList();
                        if (IsBatchExists != null && IsBatchExists.Count>0)
                        {
                            showAlert("Can not remove.Some batches exsits under this program");
                            return;
                        }
                        else
                        {
                            ucamContext.Programs.Remove(ExistingObj);
                            ucamContext.SaveChanges();

                            showAlert("Program Removed Successfully");
                            LoadProgram();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearProgramField()
        {
            ddlAddProgramFaculty.SelectedValue = "0";
            ddlAddProgramDepartment.SelectedValue = "0";
            ddlProgramType.SelectedValue = "0";
            ddlCalenderType.SelectedValue = "0";
            txtProgramName.Text = "";
            txtProgramDetailName.Text = "";
            txtProgramCode.Text = "";
            txtProgramShortName.Text = "";
            txtTotalCredit.Text = "";
            txtDuration.Text = "";

        }

        #endregion

        #region Batch Setup Information

        private void LoadSession()
        {
            try
            {
                ddlSession.Items.Clear();
                ddlSession.AppendDataBoundItems = true;
                ddlSession.Items.Add(new ListItem("All", "0"));

                var SessionList = CommonMethodForFacultyDepartmentProgramBatch.AllSession();

                if (SessionList != null && SessionList.Count>0)
                {
                    ddlSession.DataTextField = "Attribute1";
                    ddlSession.DataValueField = "AcademicCalenderID";
                    ddlSession.DataSource = SessionList.OrderByDescending(x => x.AcademicCalenderID);
                    ddlSession.DataBind();
                }


                ddlAddSession.Items.Clear();
                ddlAddSession.AppendDataBoundItems = true;
                ddlAddSession.Items.Add(new ListItem("Select", "0"));

                if (SessionList != null && SessionList.Count>0)
                {
                    ddlAddSession.DataTextField = "Attribute1";
                    ddlAddSession.DataValueField = "AcademicCalenderID";
                    ddlAddSession.DataSource = SessionList.OrderByDescending(x => x.AcademicCalenderID);
                    ddlAddSession.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadBatchProgram()
        {
            try
            {
                ddlBatchProgram.Items.Clear();
                ddlBatchProgram.AppendDataBoundItems = true;
                ddlBatchProgram.Items.Add(new ListItem("All", "0"));

                var ProgramList = CommonMethodForFacultyDepartmentProgramBatch.AllProgramListByParameter(0, 0, 0, 0);

                if (ProgramList != null && ProgramList.Count>0)
                {
                    ddlBatchProgram.DataTextField = "ShortName";
                    ddlBatchProgram.DataValueField = "ProgramID";
                    ddlBatchProgram.DataSource = ProgramList;
                    ddlBatchProgram.DataBind();
                }

                ddlAddProgram.Items.Clear();
                ddlAddProgram.AppendDataBoundItems = true;
                ddlAddProgram.Items.Add(new ListItem("Select", "0"));


                if (ProgramList != null && ProgramList.Count>0)
                {
                    ddlAddProgram.DataTextField = "ShortName";
                    ddlAddProgram.DataValueField = "ProgramID";
                    ddlAddProgram.DataSource = ProgramList;
                    ddlAddProgram.DataBind();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LoadBatchInformation()
        {
            int AcacalId = Convert.ToInt32(ddlSession.SelectedValue);
            int ProgramId = Convert.ToInt32(ddlBatchProgram.SelectedValue);

            var BatchList = CommonMethodForFacultyDepartmentProgramBatch.AllBatchListByProgramId(0);

            if (AcacalId > 0 && BatchList != null && BatchList.Count>0)
                BatchList = BatchList.Where(x => x.AcaCalId == AcacalId).ToList();

            if (ProgramId > 0 && BatchList != null && BatchList.Count>0)
                BatchList = BatchList.Where(x => x.ProgramId == ProgramId).ToList();

            if (BatchList != null && BatchList.Count>0)
            {
                foreach (var item2 in BatchList)
                {
                    try
                    {

                        var ProgramList = ucamContext.Programs.ToList();
                        var SessionList = CommonMethodForFacultyDepartmentProgramBatch.AllSession();

                        var PrgObj = ProgramList.Where(x => x.ProgramID == item2.ProgramId).FirstOrDefault();
                        var SessionObj = SessionList.Where(x => x.AcademicCalenderID == item2.AcaCalId).FirstOrDefault();

                        if (PrgObj != null)
                            item2.Attribute1 = PrgObj.ShortName;
                        if (SessionObj != null)
                            item2.Attribute2 = SessionObj.Attribute1 ;

                    }
                    catch (Exception ex)
                    {
                    }
                }

                DivBatchGrid.Visible = true;
                gvBatchList.DataSource = BatchList;
                gvBatchList.DataBind();
            }
            else
                ClearBatchGrid();

        }

        private void ClearBatchGrid()
        {
            DivBatchGrid.Visible = false;
            gvBatchList.DataSource = null;
            gvBatchList.DataBind();
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBatchInformation();
        }

        protected void ddlBatchProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBatchInformation();
        }

        protected void lnkAddNewBatch_Click(object sender, EventArgs e)
        {
            DivBatchAdd.Visible = true;
            hdnBatchId.Value = "0";
        }

        protected void lnkBatchCancel_Click(object sender, EventArgs e)
        {
            DivBatchAdd.Visible = false;
        }

        protected void lnkBatchSave_Click(object sender, EventArgs e)
        {
            try
            {

                int SetupId = Convert.ToInt32(hdnBatchId.Value);

                int AcacalId = Convert.ToInt32(ddlAddSession.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlAddProgram.SelectedValue);

                string BatchNo = "", Code = "";

                if (AcacalId == 0)
                {
                    showAlert("Please select a session");
                    return;
                }
                if (ProgramId == 0)
                {
                    showAlert("Please select a program");
                    return;
                }

                if (string.IsNullOrEmpty(txtBatchNo.Text))
                {
                    showAlert("Please enter batch no");
                    return;
                }

                BatchNo = txtBatchNo.Text.Trim();

                var ExistingObj = ucamContext.Batches.Where(x => x.AcaCalId == AcacalId && x.ProgramId == ProgramId && x.BatchId != SetupId).FirstOrDefault();

                if (ExistingObj != null)
                {
                    showAlert("Batch already exists with the Session And Program");
                    return;
                }

                if (SetupId == 0) // New Entry
                {
                    DAL.Batch NewObj = new DAL.Batch();

                    NewObj.AcaCalId = AcacalId;
                    NewObj.ProgramId = ProgramId;
                    NewObj.BatchNO = Convert.ToInt32(BatchNo);
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.Batches.Add(NewObj);
                    ucamContext.SaveChanges();

                    int Id = NewObj.BatchId;

                    if (Id > 0)
                    {
                        showAlert("Batch Added Successfully");
                        LoadBatchInformation();
                    }
                    else
                    {
                        showAlert("Batch Add Failed");
                    }

                }
                else // Udpate Existing Entry
                {
                    var UpdatingObj = ucamContext.Batches.Find(SetupId);
                    if (UpdatingObj != null)
                    {
                        hdnBatchId.Value = "0";

                        UpdatingObj.AcaCalId = AcacalId;
                        UpdatingObj.ProgramId = ProgramId;
                        UpdatingObj.BatchNO = Convert.ToInt32(BatchNo);
                        UpdatingObj.ModifiedBy = UserObj.Id;
                        UpdatingObj.ModifiedDate = DateTime.Now;
                        ucamContext.Entry(UpdatingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("Batch Updated Successfully");
                        LoadBatchInformation();
                    }
                }
                ClearBatchField();
                DivBatchAdd.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void editBatch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearBatchField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Batches.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        hdnBatchId.Value = SetupId.ToString();

                        ddlAddSession.SelectedValue = ExistingObj.AcaCalId.ToString();
                        ddlAddProgram.SelectedValue = ExistingObj.ProgramId.ToString();
                        txtBatchNo.Text = ExistingObj.BatchNO.ToString();

                        DivBatchAdd.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void RemoveBatch_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.Batches.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        var IsStudentExists = ucamContext.Students.Where(x => x.BatchId == SetupId).ToList();
                        if (IsStudentExists != null && IsStudentExists.Count>0)
                        {
                            showAlert("Can not remove.Some student exsits under this batch");
                            return;
                        }
                        else
                        {
                            ucamContext.Batches.Remove(ExistingObj);
                            ucamContext.SaveChanges();

                            showAlert("Batch Removed Successfully");
                            LoadBatchInformation();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearBatchField()
        {
            try
            {
                ddlAddSession.SelectedValue = "0";
                ddlAddProgram.SelectedValue = "0";
                txtBatchNo.Text = "0";
            }
            catch (Exception ex)
            {
            }
        }

        #endregion



        protected void lnkFaculty_Click(object sender, EventArgs e)
        {
            DivFaculty.Visible = !DivFaculty.Visible;
            DivDepartment.Visible = false;
            DivProgram.Visible = false;
            DivBatch.Visible = false;
        }

        protected void lnkDepartment_Click(object sender, EventArgs e)
        {
            DivDepartment.Visible = !DivDepartment.Visible;
            DivFaculty.Visible = false;
            DivProgram.Visible = false;
            DivBatch.Visible = false;
        }

        protected void lnkProgram_Click(object sender, EventArgs e)
        {
            DivProgram.Visible = !DivProgram.Visible;
            DivDepartment.Visible = false;
            DivFaculty.Visible = false;
            DivBatch.Visible = false;

            DivProgramAdd.Visible = false;

            LoadProgramFaculty();
            LoadProgramDepartment();
            LoadProgram();
            LoadProgramType();
            LoadCalender();
        }

        protected void lnkBatch_Click(object sender, EventArgs e)
        {
            DivBatch.Visible = !DivBatch.Visible;
            DivProgram.Visible = false;
            DivDepartment.Visible = false;
            DivFaculty.Visible = false;

            LoadSession();
            LoadBatchProgram();
            LoadBatchInformation();

        }






        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


    }
}