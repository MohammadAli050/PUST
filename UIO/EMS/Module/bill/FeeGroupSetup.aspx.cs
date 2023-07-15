using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;

namespace EMS.Module.bill
{
    public partial class FeeGroupSetup : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                LoadProgramDropDownList();
                LoadAdmissionSessionDropDownList();
                //LoadPopAdmissionSessionDropDownList();
                //LoadFundType();
                //LoadTypeDefinitionDropDownList();
                //LoadGrid();
                //ucBatch.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                //ucBatch.LoadDropDownList(0);
                //LoadFeeGroup();
                lblMsg.Text = "";
            }
        }

        public void LoadProgramDropDownList()
        {
            try
            {
                List<Program> programList = ProgramManager.GetAll().OrderBy(x => x.DetailName).ToList();
                programDropDownList.Items.Clear();
                programDropDownList.Items.Add(new ListItem("-All-", "-1"));
                programDropDownList.AppendDataBoundItems = true;


                if (programList.Any())
                {
                    programDropDownList.DataTextField = "ShortName";
                    programDropDownList.DataValueField = "ProgramID";
                    programDropDownList.DataSource = programList;
                    programDropDownList.DataBind();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LoadPopProgramDropDownList()
        {
            try
            {
                List<Program> programList = ProgramManager.GetAll().OrderBy(x => x.DetailName).ToList();
                popProgramDropDownList.Items.Clear();
                popProgramDropDownList.Items.Add(new ListItem("-All-", "-1"));
                popProgramDropDownList.AppendDataBoundItems = true;


                if (programList.Any())
                {
                    popProgramDropDownList.DataTextField = "ShortName";
                    popProgramDropDownList.DataValueField = "ProgramID";
                    popProgramDropDownList.DataSource = programList;
                    popProgramDropDownList.DataBind();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LoadAdmissionSessionDropDownList()
        {
            try
            {
                //int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                //var academicCalenderList = AcademicCalenderManager.GetAll().Where(x => x.CalenderUnitTypeID == 1).OrderByDescending(m => m.AcademicCalenderID).ToList();
                var academicCalenderList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();
                admissionSessionDropDownList.Items.Clear();
                admissionSessionDropDownList.Items.Add(new ListItem("-All-", "-1"));
                admissionSessionDropDownList.AppendDataBoundItems = true;


                if (academicCalenderList.Any())
                {
                    admissionSessionDropDownList.DataTextField = "Code";
                    admissionSessionDropDownList.DataValueField = "AcademicCalenderID";
                    admissionSessionDropDownList.DataSource = academicCalenderList;
                    admissionSessionDropDownList.DataBind();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        public void LoadPopAdmissionSessionDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                //var academicCalenderList = AcademicCalenderManager.GetAll().Where(x => x.CalenderUnitTypeID == 1).OrderByDescending(m => m.AcademicCalenderID).ToList();
                var academicCalenderList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();
                popUpAdmissionSessionDropDownList.Items.Clear();
                popUpAdmissionSessionDropDownList.Items.Add(new ListItem("-All-", "-1"));
                popUpAdmissionSessionDropDownList.AppendDataBoundItems = true;


                if (academicCalenderList.Any())
                {
                    popUpAdmissionSessionDropDownList.DataTextField = "Code";
                    popUpAdmissionSessionDropDownList.DataValueField = "AcademicCalenderID";
                    popUpAdmissionSessionDropDownList.DataSource = academicCalenderList;
                    popUpAdmissionSessionDropDownList.DataBind();
                }
            }
            catch (Exception e)
            {
                lblMsg.Text = "Error Occured.";
            }


        }

        private void LoadFeesGroup()
        {
            try
            {
                ResetFeeGroupGridView();

                int? programId = Convert.ToInt32(programDropDownList.SelectedValue);
                if (programId == -1)
                {
                    programId = null;
                }

                int? studentAdmissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (studentAdmissionAcaCalId == -1)
                {
                    studentAdmissionAcaCalId = null;
                }

                List<FeeGroupMaster> feeGroupList = FeeGroupMasterManager.GetAllFeeGroupMasterByProgramIdAdmissionAcaCalId(programId, studentAdmissionAcaCalId);

                foreach (FeeGroupMaster feeGroupMaster in feeGroupList)
                {
                    feeGroupMaster.ProgramName = "All";
                    feeGroupMaster.BatchNO = "All";
                    if (feeGroupMaster.ProgramId != null)
                    {
                        feeGroupMaster.ProgramName = feeGroupMaster.Program.DetailName;
                    }
                    if (feeGroupMaster.StudentAdmissionAcaCalId != null)
                    {
                        feeGroupMaster.BatchNO = feeGroupMaster.Session.Code;
                    }
                    feeGroupMaster.Attribute1 = "InActive";
                    if (feeGroupMaster.IsActive)
                    {
                        feeGroupMaster.Attribute1 = "Active";
                    }
                }

                feeGroupList = feeGroupList.OrderBy(x => x.ProgramName).ToList();
                if (feeGroupList.Any())
                {
                    GvFeeGroup.DataSource = feeGroupList;
                    GvFeeGroup.DataBind();
                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Error Occured.";
            }
        }

        private void LoadFeeGroup()
        {
            try
            {
                ResetFeeGroupGridView();
                List<FeeGroupMaster> allFeeGroupList = FeeGroupMasterManager.GetAll();
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                var programFeeGroupList = allFeeGroupList.Where(m => m.ProgramId == programId);
                if (programFeeGroupList.Any())
                {

                    GvFeeGroup.DataSource = programFeeGroupList;
                    GvFeeGroup.DataBind();
                }
                else
                {
                    lblMsg.Text = "No fee group found.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadGrid()
        {
            //int? programId = Convert.ToInt32(popProgramDropDownList.SelectedValue);
            //if (programId == -1)
            //{
            //    programId = null;
            //}

            //int? admissionAcaCalId = Convert.ToInt32(popUpAdmissionSessionDropDownList.SelectedValue);
            //if (admissionAcaCalId == -1)
            //{
            //    admissionAcaCalId = null;
            //}
            List<FeeGroupMaster> list = FeeSetupManager.GetAllFeeGroupByProgramIdAcaCalId(null, null).ToList();
            list = list.OrderBy(m => m.Type.Definition).ToList();
            feeSetupGridView.DataSource = list;
            feeSetupGridView.DataBind();
            feeSetupGridView.EmptyDataText = "No data found.";
        }

        private void LoadFundTypeUpdate(int fundTypeId)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
            List<FundType> fundTypeList = FundTypeManager.GetAll();
            ddlFundTypeUpdate.Items.Clear();
            ddlFundTypeUpdate.Items.Add(new ListItem("-Select Fund-", "0"));
            ddlFundTypeUpdate.AppendDataBoundItems = true;
            if (fundTypeList != null && fundTypeList.Count > 0)
            {
                ddlFundTypeUpdate.DataSource = fundTypeList;
                ddlFundTypeUpdate.DataValueField = "FundTypeId";
                ddlFundTypeUpdate.DataTextField = "FundName";
                ddlFundTypeUpdate.DataBind();
                //ddlFundTypeUpdate.SelectedIndex.Equals(fundTypeId);
                ddlFundTypeUpdate.Items.FindByValue(fundTypeId.ToString()).Selected = true;
            }
        }

        private void LoadTypeDefinitionDropDownList()
        {
            //this.ModalShowFeeGroupTypePopupExtender.Show();
            //lblShowFeeGroupTypeMsg.Text = string.Empty;
            //List<LogicLayer.BusinessObjects.TypeDefinition> feeTypeList = TypeDefinitionManager.GetAll().OrderBy(m=>m.Definition).Where(m=>m.Definition == "Fee").ToList();
            //ddlTypeDefinition.Items.Clear();
            //ddlTypeDefinition.Items.Add(new ListItem("-Select Fee Head-", "0"));
            //ddlTypeDefinition.AppendDataBoundItems = true;
            //if (feeTypeList.Any())
            //{
            //    ddlTypeDefinition.DataSource = feeTypeList;
            //    ddlTypeDefinition.DataValueField = "TypeDefinitionID";
            //    ddlTypeDefinition.DataTextField = "Definition";
            //    ddlTypeDefinition.DataBind();
            //}
        }

        #endregion

        #region Event

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                this.ModalFeeGroupPopupExtender.Show();

                CheckBox chk = (CheckBox)sender;

                foreach (GridViewRow row in feeSetupGridView.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("checkBox");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                //ucBatch.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                LoadFeesGroup();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void admissionSessionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFeesGroup();
        }

        protected void popAdmissionSessionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ModalFeeGroupPopupExtender.Show();
                lblMessage.Text = string.Empty;
                LoadGrid();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected void popProgramDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalFeeGroupPopupExtender.Show();
            lblMessage.Text = string.Empty;
            //popUpAdmissionSessionDropDownList.LoadDropDownList(Convert.ToInt32(ucProgramFeeGroup.selectedValue));
        }

        protected void ddlFundTypeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
        }

        protected void btnRemoveFeeGroupType_Click(object sender, EventArgs e)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
            lblShowFeeGroupTypeMsg.Text = null;
            LinkButton btn = (LinkButton)sender;
            int feeGroupDetailId = int.Parse(btn.CommandArgument.ToString());
            if (feeGroupDetailId > 0)
            {
                FeeGroupDetail feeGroupDetailObj = FeeGroupDetailManager.GetById(feeGroupDetailId);
                bool result = FeeGroupDetailManager.Delete(feeGroupDetailId);
                if (result)
                {
                    int feeGroupMasterId = feeGroupDetailObj.FeeGroupMasterId;
                    lblShowFeeGroupTypeMsg.Text = "Amount removed against the fee type sucessfully.";
                    List<FeeGroupDetail> feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(feeGroupMasterId);
                    if (feeGroupDetailList.Any())
                    {
                        GvFeeGroupType.DataSource = feeGroupDetailList;
                        GvFeeGroupType.DataBind();
                    }
                }
                else
                {
                    lblShowFeeGroupTypeMsg.Text = "Amount could not be removed against this fee type, please try again providing all necessary field.";
                }
            }
        }

        protected void GvFeeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Button Click

        protected void btnAddFeeGroup_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
                lblMsg.Text = string.Empty;
                this.ModalFeeGroupPopupExtender.Show();
                LoadPopProgramDropDownList();
                LoadPopAdmissionSessionDropDownList();
                LoadGrid();
                //popUpUcProgram.LoadDropdownWithUserAccess(userObj.Id);
                //    txtFeeGroupName.Text = null;
                //    ddlFundType.Items.Clear();
                //    ddlFundType.Items.Add(new ListItem("-Select Fund-", "0"));
                //    ddlFundType.AppendDataBoundItems = true;
                //    ddlFundTypeUpdate.Items.FindByValue(ucProgram.selectedValue).Selected = true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadFeesGroup();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
                this.ModalFeeGroupPopupExtender.Show();
                lblShowFeeGroupTypeMsg.Text = null;
                //txtFeeGroupTypeAmount.Text = null;
                GvFeeGroupType.DataSource = null;
                GvFeeGroupType.DataBind();
                //LoadFeeTypeDDL();
                LinkButton btn = (LinkButton)sender;
                int feeGroupMasterId = int.Parse(btn.CommandArgument.ToString());
                if (feeGroupMasterId > 0)
                {
                    FeeGroupMaster feeGroupMasterObj = FeeGroupMasterManager.GetById(feeGroupMasterId);
                    if (feeGroupMasterObj != null)
                    {
                        LoadPopProgramDropDownList();
                        LoadPopAdmissionSessionDropDownList();
                        //LoadFundTypeUpdate(feeGroupMasterObj.FundTypeId);
                        //lblFeeGroupName.Text = feeGroupMasterObj.FeeGroupName;
                        if (feeGroupMasterObj.ProgramId != null)
                            popProgramDropDownList.SelectedValue = feeGroupMasterObj.ProgramId.ToString();
                        if (feeGroupMasterObj.StudentAdmissionAcaCalId != null)
                            popUpAdmissionSessionDropDownList.SelectedValue = feeGroupMasterObj.StudentAdmissionAcaCalId.ToString();
                        txtFeeGroupName.Text = feeGroupMasterObj.FeeGroupName;
                        btnSaveFeeGroup.Text = "Update";
                        hiddenFeeGroupMasterID.Value = feeGroupMasterObj.FeeGroupMasterId.ToString();

                        lblFeeGroupMasterId.Text = Convert.ToString(feeGroupMasterObj.FeeGroupMasterId);
                        List<FeeGroupMaster> list = FeeGroupDetailManager.GetAllFeeGroupByFeeGroupMasterId(feeGroupMasterObj.FeeGroupMasterId).ToList();
                        list = list.OrderBy(m => m.Type.Definition).ToList();
                        feeSetupGridView.DataSource = list;
                        feeSetupGridView.DataBind();
                        feeSetupGridView.EmptyDataText = "No data found.";
                        //List<FeeGroupDetail> feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(feeGroupMasterObj.FeeGroupMasterId);
                        //if (feeGroupDetailList.Count > 0)
                        //{
                        //    feeSetupGridView.DataSource = feeGroupDetailList;
                        //    feeSetupGridView.DataBind();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSaveFeeGroup_Click(object sender, EventArgs e)
        {
            this.ModalFeeGroupPopupExtender.Show();
            FeeGroupMaster feeGroupMasterObj = new FeeGroupMaster();

            int programId = Convert.ToInt32(popProgramDropDownList.SelectedValue);
            feeGroupMasterObj.ProgramId = programId;
            if (programId == -1)
            {
                feeGroupMasterObj.ProgramId = null;
            }

            int studentAdmissionAcaCalId = Convert.ToInt32(popUpAdmissionSessionDropDownList.SelectedValue);
            feeGroupMasterObj.StudentAdmissionAcaCalId = studentAdmissionAcaCalId;
            if (studentAdmissionAcaCalId == -1)
            {
                feeGroupMasterObj.StudentAdmissionAcaCalId = null;
            }

            string buttonIsSaveOrUpdate = btnSaveFeeGroup.Text;

            if (buttonIsSaveOrUpdate == "Save")
            {

                string feeGroupName = txtFeeGroupName.Text;
                if (String.IsNullOrEmpty(feeGroupName) || String.IsNullOrWhiteSpace(feeGroupName))
                {
                    lblMessage.Text = "Must needed group name.";
                    return;
                }

                feeGroupName = feeGroupName.Trim();
                var feeGroupMasterList = FeeGroupMasterManager.GetAll();
                var feeGroupMaster = feeGroupMasterList.FirstOrDefault(x => x.ProgramId == feeGroupMasterObj.ProgramId && x.StudentAdmissionAcaCalId == feeGroupMasterObj.StudentAdmissionAcaCalId && x.FeeGroupName == feeGroupName);
                if (feeGroupMaster != null)
                {
                    lblMessage.Text = "Free Group Exist";
                    return;
                }
                feeGroupMasterObj.FeeGroupName = feeGroupName;
                feeGroupMasterObj.IsActive = true;
                feeGroupMasterObj.Remarks = null;
                feeGroupMasterObj.CreatedBy = userObj.Id;
                feeGroupMasterObj.CreatedDate = DateTime.Now;
                int result = FeeGroupMasterManager.Insert(feeGroupMasterObj);
                if (result > 0)
                {
                    lblMessage.Text = "Fee Group has been Saved.";
                    foreach (GridViewRow row in feeSetupGridView.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("checkBox");
                        if (ckBox.Checked)
                        {
                            FeeGroupDetail feeGroupDetailObj = new FeeGroupDetail();
                            HiddenField typeDefinitionIdHiddenField = (HiddenField)row.FindControl("hdnTypeDefID");
                            feeGroupDetailObj.FeeGroupMasterId = result;
                            feeGroupDetailObj.TypeDefinitionId = Convert.ToInt32(typeDefinitionIdHiddenField.Value);
                            feeGroupDetailObj.IsActive = true;
                            feeGroupDetailObj.CreatedBy = userObj.Id;
                            feeGroupDetailObj.CreatedDate = DateTime.Now;

                            int result1 = FeeGroupDetailManager.Insert(feeGroupDetailObj);
                            if (result1 > 0)
                            {
                                lblMessage.Text = "Fee Group has been Saved.";
                            }
                            else
                            {
                                lblShowFeeGroupTypeMsg.Text =
                                    "Fee group type could not added, please try again providing all necessary field.";
                            }

                        }
                    }
                }
            }

            if (buttonIsSaveOrUpdate == "Update")
            {
                int feeGroupMasterId = Convert.ToInt32(hiddenFeeGroupMasterID.Value);
                string feeGroupName = txtFeeGroupName.Text;
                if (String.IsNullOrEmpty(feeGroupName) || String.IsNullOrWhiteSpace(feeGroupName))
                {
                    lblMessage.Text = "Must needed group name.";
                    return;
                }
                foreach (GridViewRow row in feeSetupGridView.Rows)
                {
                    FeeGroupDetail feeGroupDetailObj = new FeeGroupDetail();
                    CheckBox ckBox = (CheckBox)row.FindControl("checkBox");
                    HiddenField typeDefinitionIdHiddenField = (HiddenField)row.FindControl("hdnTypeDefID");
                    HiddenField feeGroupDetailIdHiddenField = (HiddenField)row.FindControl("hdnFeeGroupDetailId");
                   
                    int feeGroupDetailId = Convert.ToInt32(feeGroupDetailIdHiddenField.Value);
                    
                    feeGroupDetailObj.TypeDefinitionId = Convert.ToInt32(typeDefinitionIdHiddenField.Value);
                    feeGroupDetailObj.FeeGroupMasterId = feeGroupMasterId;
                    
                    if (feeGroupDetailId > 0)
                    {
                        
                        feeGroupDetailObj.FeeGroupDetailId = feeGroupDetailId;
                        feeGroupDetailObj.ModifiedBy = userObj.Id;
                        feeGroupDetailObj.ModifiedDate = DateTime.Now;
                        feeGroupDetailObj.IsActive = false;
                        if (ckBox.Checked)
                        {
                            feeGroupDetailObj.IsActive = true;
                        }

                        bool result = FeeGroupDetailManager.Update(feeGroupDetailObj);
                        if (result)
                        {
                            lblMessage.Text = "Successfully Updated.";
                        }
                        else
                        {
                            lblShowFeeGroupTypeMsg.Text =
                                "Fee group type could not added, please try again providing all necessary field.";
                        }

                    }

                    if (!(feeGroupDetailId > 0) && ckBox.Checked)
                    {
                        feeGroupDetailObj.IsActive = true;
                        feeGroupDetailObj.CreatedBy = userObj.Id;
                        feeGroupDetailObj.CreatedDate = DateTime.Now;

                        int result1 = FeeGroupDetailManager.Insert(feeGroupDetailObj);
                        if (result1 > 0)
                        {
                            lblMessage.Text = "Successfully Updated.";
                        }
                    }
                }

            }

        }

        protected void btnFeeGroupTypeCancel_Click(object sender, EventArgs e)
        {
            LoadFeesGroup();
        }

        protected void ImageButton1_OnClick(object sender, ImageClickEventArgs e)
        {
            LoadFeesGroup();
        }

        protected void btnFeeGroupTypeSave_Click(object sender, EventArgs e)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
            lblShowFeeGroupTypeMsg.Text = null;
            FeeGroupDetail feeGroupDetailObj = new FeeGroupDetail();
            //UpdateFundType();
            if (CheckFeeGroupTypeInsertField())
            {

                feeGroupDetailObj.FeeGroupMasterId = Convert.ToInt32(lblFeeGroupMasterId.Text);
                //feeGroupDetailObj.FeeTypeId = Convert.ToInt32(ddlTypeDefinition.SelectedValue);
                //feeGroupDetailObj.Amount = Convert.ToDecimal(txtFeeGroupTypeAmount.Text);
                feeGroupDetailObj.CreatedBy = userObj.Id;
                feeGroupDetailObj.CreatedDate = DateTime.Now;
                feeGroupDetailObj.ModifiedBy = userObj.Id;
                feeGroupDetailObj.ModifiedDate = DateTime.Now;
                List<FeeGroupDetail> feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(Convert.ToInt32(lblFeeGroupMasterId.Text));
                if (feeGroupDetailList.Count > 0 && feeGroupDetailList != null)
                {
                    //FeeGroupDetail feeGroupDetailObj2 = feeGroupDetailList.Where(d => d.FeeTypeId == Convert.ToInt32(ddlTypeDefinition.SelectedValue)).FirstOrDefault();
                    //if (feeGroupDetailObj2 == null)
                    {
                        int result = FeeGroupDetailManager.Insert(feeGroupDetailObj);
                        if (result > 0)
                        {
                            lblShowFeeGroupTypeMsg.Text = "Amount added against this fee type successfully.";
                            feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(Convert.ToInt32(lblFeeGroupMasterId.Text));
                            GvFeeGroupType.DataSource = feeGroupDetailList;
                            GvFeeGroupType.DataBind();
                        }
                        else
                        {
                            lblShowFeeGroupTypeMsg.Text = "Fee group type could not added, please try again providing all necessary field.";
                        }
                    }
                    //else
                    //{
                    //    lblShowFeeGroupTypeMsg.Text = "Amount against this fee type already exist, please try with other fee type.";
                    //}
                }
                else
                {
                    int result = FeeGroupDetailManager.Insert(feeGroupDetailObj);
                    if (result > 0)
                    {
                        lblShowFeeGroupTypeMsg.Text = "Fee group type added successfully.";
                        feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(Convert.ToInt32(lblFeeGroupMasterId.Text));
                        GvFeeGroupType.DataSource = feeGroupDetailList;
                        GvFeeGroupType.DataBind();
                    }
                    else
                    {
                        lblShowFeeGroupTypeMsg.Text = "Fee group type could not added, please try again providing all necessary field.";
                    }
                }
            }
            else
            {
                lblShowFeeGroupTypeMsg.Text = "Please provide all necessary field to add fee amount.";
            }
        }

        protected void btnShowFeeGroupCancel_Click(object sender, EventArgs e)
        {
            LoadFeesGroup();
        }

        #endregion

        //public void LoadFundType()
        //{

        //    ddlFundType.Items.Clear();
        //    ddlFundType.Items.Add(new ListItem("Select", "-1"));
        //    ddlFundType.AppendDataBoundItems = true;
        //    ddlFundType.DataSource = FundTypeManager.GetAll();
        //    ddlFundType.DataTextField = "FundName";
        //    ddlFundType.DataValueField = "FundTypeId";
        //    ddlFundType.DataBind();
        //}

        protected void ucBatchFeeGroup_BatchSelectedIndexChanged(object sender, EventArgs e)
        {
            //this.ModalFeeGroupPopupExtender.Show();
            //lblMessage.Text = string.Empty;
            //List<FundType> fundTypeList = FundTypeManager.GetAll();
            //ddlFundType.Items.Clear();
            //ddlFundType.Items.Add(new ListItem("-Select Fund-", "0"));
            //ddlFundType.AppendDataBoundItems = true;
            //if (fundTypeList != null && fundTypeList.Count > 0)
            //{
            //    ddlFundType.DataSource = fundTypeList;
            //    ddlFundType.DataValueField = "FundTypeId";
            //    ddlFundType.DataTextField = "FundName";
            //    ddlFundType.DataBind();
            //}
        }

        protected void ddlFundType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalFeeGroupPopupExtender.Show();
            lblMessage.Text = string.Empty;
        }

        private bool CheckFeeGroupInsertField()
        {
            if (Convert.ToInt32(popProgramDropDownList.SelectedValue) > 0)
            {
                if (Convert.ToInt32(popUpAdmissionSessionDropDownList.SelectedValue) > 0)
                {
                    {
                        if (!string.IsNullOrEmpty(txtFeeGroupName.Text))
                        {
                            return true;
                        }

                        return false;
                    }
                }

                return false;
            }

            return false;
        }

        private void UpdateFundType()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(lblFeeGroupMasterId.Text)))
            {
                int feeGroupMasterId = Convert.ToInt32(lblFeeGroupMasterId.Text);
                FeeGroupMaster feeGroupMasterObj = FeeGroupMasterManager.GetById(feeGroupMasterId);
                List<FeeGroupDetail> feeGroupDetailList = new List<FeeGroupDetail>();
                if (feeGroupMasterObj != null)
                {
                    int fundId = Convert.ToInt32(ddlFundTypeUpdate.SelectedValue);
                    if (fundId > 0)
                    {

                        feeGroupMasterObj.FundTypeId = fundId;
                        feeGroupMasterObj.FeeGroupName = feeGroupNameTextArea.Value;
                        feeGroupMasterObj.ModifiedBy = userObj.Id;
                        feeGroupMasterObj.ModifiedDate = DateTime.Now;
                        bool result = FeeGroupMasterManager.Update(feeGroupMasterObj);
                        if (result)
                        {
                            lblShowFeeGroupTypeMsg.Text = "Fund type updated.";
                            feeGroupDetailList = FeeGroupDetailManager.GetByFeeGroupMasterId(Convert.ToInt32(lblFeeGroupMasterId.Text));
                            GvFeeGroupType.DataSource = feeGroupDetailList;
                            GvFeeGroupType.DataBind();
                        }
                        else
                        {
                            lblShowFeeGroupTypeMsg.Text = "Fund type could not updated.";
                        }
                    }
                }
            }
        }

        private bool CheckFeeGroupTypeInsertField()
        {
            if (Convert.ToInt32(lblFeeGroupMasterId.Text) > 0)
            {
                //if (Convert.ToInt32(ddlTypeDefinition.SelectedValue) > 0)
                {
                    //if (!string.IsNullOrEmpty(txtFeeGroupTypeAmount.Text) && Convert.ToDecimal(txtFeeGroupTypeAmount.Text) > 0)
                    {
                        return true;
                    }
                    //else { return false; }
                }
                //else { return false; }
            }
            else { return false; }
        }

        protected void ddlTypeDefinition_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
            lblShowFeeGroupTypeMsg.Text = null;
        }

        protected void btnUpdateFundType_Click(object sender, EventArgs e)
        {
            this.ModalShowFeeGroupTypePopupExtender.Show();
            UpdateFundType();
        }

        public void ResetFeeGroupGridView()
        {
            GvFeeGroup.DataSource = null;
            GvFeeGroup.DataBind();
        }

        protected void feeSetupGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Find the DropDownList in the Row
                    CheckBox checkBox = (e.Row.FindControl("checkBox") as CheckBox);
                    HiddenField hiddenIsActive = (e.Row.FindControl("hiddenIsActive") as HiddenField);
                    if (hiddenIsActive.Value == "True")
                    {
                        checkBox.Checked = true;
                    }
                    if (hiddenIsActive.Value == "False")
                    {
                        checkBox.Checked = false;
                    }
                    //fundTypeDropDownList.DataSource = FundTypeManager.GetAll();
                    //fundTypeDropDownList.DataTextField = "FundName";
                    //fundTypeDropDownList.DataValueField = "FundTypeId";
                    //fundTypeDropDownList.DataBind();

                    //Add Default Item in the DropDownList
                    //fundTypeDropDownList.Items.Insert(0, new ListItem("Select"));

                    //Select the fundType of fee in DropDownList
                    //HiddenField fundTypeIdHiddenField = (e.Row.FindControl("fundTypeIdHiddenField") as HiddenField);
                    //HiddenField fundTypeIdInFeeSetUpTableHiddenField = (e.Row.FindControl("fundTypeIdInFeeSetUpTableHiddenField") as HiddenField);
                    //if (fundTypeIdHiddenField != null)
                    //{
                    //    if (Convert.ToInt32(fundTypeIdInFeeSetUpTableHiddenField.Value) == 0)
                    //    {
                    //        fundTypeDropDownList.SelectedValue = fundTypeIdHiddenField.Value;
                    //    }

                    //    else
                    //    {
                    //        fundTypeDropDownList.SelectedValue = fundTypeIdInFeeSetUpTableHiddenField.Value;
                    //    }
                    //}
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void saveButton_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ClearAll()
        {
            lblMessage.Text = String.Empty;
            lblMsg.Text = string.Empty;
            btnSaveFeeGroup.Text = "Save";
            txtFeeGroupName.Text = String.Empty;
            hiddenFeeGroupMasterID.Value = null;
            feeSetupGridView.DataSource = null;
            feeSetupGridView.DataBind();
        }
    }
}