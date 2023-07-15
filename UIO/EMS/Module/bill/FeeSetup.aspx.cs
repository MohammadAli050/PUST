using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using Microsoft.Reporting.WebForms;

namespace EMS.Module.bill
{
    public partial class FeeSetup : BasePage
    {
        User user = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonEnum.PageName.FeeSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonEnum.PageName.FeeSetup));
        //private List<AcademicCalender> _trimesterInfos = null;
        List<TypeDefinition> _typeDefs = null;

        #region Session Consts
        private const string SESSIONTYPEDEF = "FEETYPEDEF";
        private const string SESSIONTABLE = "FEEDATATABLE";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.CheckPage_Load();
                string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                user = UserManager.GetByLogInId(loginId);
                if (!IsPostBack && !IsCallback)
                {
                    ucDepartment.LoadDropDownList();
                    ucDepartment_ProgramSelectedIndexChanged(null,null);
                    //int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    //ucProgram.LoadDropDownList();
                    //ucProgram.LoadDropdownByDepartmentId(departmentId);
                    //LoadProgramDropDownList();
                    LoadProgramForwardDropDownList();
                    ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
                    _scriptMan.AsyncPostBackTimeout = 36000;
                    //FillBatchCombo();
                    //FillProgramCombo();
                    //FillFeeType();
                    //LoadForwardBatchDropDown();

                }
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        public void LoadProgramDropDownList()
        {
            #region N/A
            //var programList = ProgramManager.GetAll();
            //programDropDownList.Items.Add(new ListItem("Select", "-1"));
            //programDropDownList.AppendDataBoundItems = true;
            //if (programList != null && programList.Any())
            //{
            //    programDropDownList.DataTextField = "ShortName";
            //    programDropDownList.DataValueField = "ProgramID";
            //    programDropDownList.DataSource = programList;
            //    programDropDownList.DataBind();
            //} 
            #endregion

            ucProgram.LoadDropDownList();
            ucProgram.SelectedValue(0);
        }

        public void LoadProgramForwardDropDownList()
        {
            #region N/A
            var programList = ProgramManager.GetAll();
            ddlProgramForward.Items.Add(new ListItem("Select", "-1"));
            ddlProgramForward.AppendDataBoundItems = true;
            if (programList != null && programList.Any())
            {
                ddlProgramForward.DataTextField = "ShortName";
                ddlProgramForward.DataValueField = "ProgramID";
                ddlProgramForward.DataSource = programList;
                ddlProgramForward.DataBind();
            }
            #endregion
        }

        public void LoadAdmissionSessionDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //var academicCalenderList = AcademicCalenderManager.GetAll().Where(x => x.CalenderUnitTypeID == 1).OrderByDescending(m => m.AcademicCalenderID).ToList();
                var academicCalenderList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();
                admissionSessionDropDownList.Items.Clear();
                admissionSessionDropDownList.Items.Add(new ListItem("Select", "-1"));
                admissionSessionDropDownList.AppendDataBoundItems = true;
                if (programId == -1)
                {
                    lblMsg.Text = "select program.";
                    return;
                }

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

        public void LoadForwardAdmissionSessionDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(ddlProgramForward.SelectedValue);
                var academicCalenderList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();
                forwardAdmissionSessionDropDownList.Items.Clear();
                forwardAdmissionSessionDropDownList.Items.Add(new ListItem("Select", "-1"));
                forwardAdmissionSessionDropDownList.AppendDataBoundItems = true;
                if (programId == -1)
                {
                    lblMsg.Text = "select program.";
                    return;
                }

                if (academicCalenderList.Any())
                {
                    forwardAdmissionSessionDropDownList.DataTextField = "Code";
                    forwardAdmissionSessionDropDownList.DataValueField = "AcademicCalenderID";
                    forwardAdmissionSessionDropDownList.DataSource = academicCalenderList;
                    forwardAdmissionSessionDropDownList.DataBind();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        protected void loadButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ClearReportViewer();
                string message = null;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //int scholarshipStatusId = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
                //int govNonGovId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "Please select a program.";
                    return;
                }
                int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionAcaCalId == -1)
                {
                    lblMsg.Text = "Please select a  admission session.";
                    return;
                }
                //List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdBatchIdScholarshipStatusAndGovNonGov(programId, batchId, scholarshipStatusId, govNonGovId).Where(d => d.Amount != 0).ToList();
                List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).Where(d => d.Amount != 0).ToList();
                list = list.OrderBy(m => m.Type.Definition).ToList();
                feeSetupGridView.DataSource = list;
                feeSetupGridView.DataBind();
                feeSetupGridView.EmptyDataText = "No data found.";

            }
            catch (Exception ex)
            {
                Utilities.ShowMassage(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void saveButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;

                int programId = Convert.ToInt32(ucProgram.selectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "Please select a program.";
                    return;
                }

                int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionAcaCalId == -1)
                {
                    lblMsg.Text = "Please select a  admission session.";
                    return;
                }

                //int parentsJobStatusId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
                //if (parentsJobStatusId == -1)
                //{
                //    lblMsg.Text = "Please select a parent job status";
                //    return;
                //}
                //int scholarshipStatus = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
                foreach (GridViewRow row in feeSetupGridView.Rows)
                {
                    LogicLayer.BusinessObjects.FeeSetup feeSetup = new LogicLayer.BusinessObjects.FeeSetup();

                    HiddenField hiddenId = (HiddenField)row.FindControl("hdnFeeSetupID");
                    int feesSetupId = Convert.ToInt32(hiddenId.Value);

                    HiddenField typeDefinitionIdHiddenField = (HiddenField)row.FindControl("hdnTypeDefID");
                    TextBox amount = (TextBox)row.FindControl("txtAmount");
                    DropDownList fundTypeDropDownList = (DropDownList)row.FindControl("fundTypeDropDownList");
                    int fundTypeId = Convert.ToInt32(fundTypeDropDownList.SelectedValue);
                    feeSetup.ProgramId = programId;
                    feeSetup.AcademicCalendarId = admissionAcaCalId;
                    feeSetup.BatchId = null;
                    feeSetup.TypeDefinitionId = Convert.ToInt32(typeDefinitionIdHiddenField.Value);
                    feeSetup.FundTypeId = fundTypeId;
                    feeSetup.Amount = Convert.ToDecimal(amount.Text);
                    feeSetup.ScholarshipStatusType = 0;
                    //feeSetup.GovNonGovType = parentsJobStatusId;
                    feeSetup.CreatedBy = BaseCurrentUserObj.Id;
                    feeSetup.CreatedDate = DateTime.Now;
                    feeSetup.ModifiedBy = BaseCurrentUserObj.Id;
                    feeSetup.ModifiedDate = DateTime.Now;
                    var batchFeeSetupList = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).Where(d => d.Amount != 0).ToList();
                    var isFeeExist = batchFeeSetupList.FirstOrDefault(m => m.ProgramId == feeSetup.ProgramId && m.TypeDefinitionId == feeSetup.TypeDefinitionId && m.GovNonGovType == feeSetup.GovNonGovType); // && m.Amount == feeSetup.Amount
                    if (feesSetupId == 0 && feeSetup.Amount > 0)
                    {
                        if (isFeeExist != null)
                        {
                            PopupMessage("Fee already exist. Set up manually for this program & batch.");
                            return;
                        }
                        int insertResult = FeeSetupManager.Insert(feeSetup);
                        if (insertResult > 0)
                        {
                            lblMsg.Text = "Fees inserted successfully.";

                            #region Log Insert
                            LogGeneralManager.Insert(
                                DateTime.Now,
                                "",
                                "",
                                user.LogInID,
                                "",
                                "",
                                "Fee set up insert",
                                user.LogInID + " has inserted fee for ProgramId,BatchId,ScholarshipStatusType, " + feeSetup.ProgramId + ", " + feeSetup.BatchId + ", " + feeSetup.ScholarshipStatusType + ", " + feeSetup.GovNonGovType + ", ",
                                user.LogInID + "Fee set up insert",
                                ((int)CommonEnum.PageName.FeeSetup).ToString(),
                                CommonEnum.PageName.FeeSetup.ToString(),
                                _pageUrl,
                                "");
                            #endregion
                        }
                        else
                        {
                            lblMsg.Text = "Fees could not inserted successfully.";
                        }
                    }
                    if (feesSetupId > 0)
                    {
                        bool feeExist = false;
                        var feeSetupObject = FeeSetupManager.GetById(feesSetupId);
                        if (feeSetupObject.ProgramId != programId && feeSetupObject.AcademicCalendarId != admissionAcaCalId)
                        {
                            if (isFeeExist != null)
                            {
                                var typeDefinition = TypeDefinitionManager.GetById(feeSetupObject.TypeDefinitionId);
                                feeExist = true;
                                PopupMessage(typeDefinition.Definition + " is already exist. Set up manually the fee for this program & batch.");
                            }

                            feeSetupObject.ProgramId = programId;
                            feeSetupObject.BatchId = null;
                            feeSetupObject.CreatedBy = BaseCurrentUserObj.Id;
                            feeSetup.CreatedDate = DateTime.Now;
                            if (!feeExist)
                            {
                                int insertResult = FeeSetupManager.Insert(feeSetupObject);

                                if (insertResult > 0)
                                {
                                    lblMsg.Text = "Fees inserted successfully.";

                                    #region Log Insert

                                    LogGeneralManager.Insert(
                                        DateTime.Now,
                                        "",
                                        "",
                                        user.LogInID,
                                        "",
                                        "",
                                        "Fee set up insert",
                                        user.LogInID + " has inserted fee for ProgramId,BatchId,ScholarshipStatusType, " +
                                        feeSetup.ProgramId + ", " + feeSetup.BatchId + ", " +
                                        feeSetup.ScholarshipStatusType + ", " + feeSetup.GovNonGovType + ", ",
                                        user.LogInID + "Fee set up insert",
                                        ((int)CommonEnum.PageName.SectionChangeAfterReg).ToString(),
                                        CommonEnum.PageName.SectionChangeAfterReg.ToString(),
                                        _pageUrl,
                                        "");

                                    #endregion
                                }
                                else
                                {
                                    lblMsg.Text = "Fees could not inserted successfully.";
                                }
                            }

                        }
                        else
                        {
                            feeSetupObject.Amount = Convert.ToDecimal(amount.Text);
                            feeSetupObject.FundTypeId = Convert.ToInt32(fundTypeDropDownList.SelectedValue);
                            feeSetupObject.ModifiedBy = BaseCurrentUserObj.Id;
                            feeSetupObject.ModifiedDate = DateTime.Now;
                            bool updateResult = FeeSetupManager.Update(feeSetupObject);
                            if (updateResult)
                            {
                                lblMsg.Text = "Fees updated successfully.";
                                #region Log Update
                                LogGeneralManager.Insert(
                                    DateTime.Now,
                                    "",
                                    "",
                                    user.LogInID,
                                    "",
                                    "",
                                    "Fee set up Update",
                                    user.LogInID + " has inserted fee for ProgramId,BatchId,ScholarshipStatusType, " + feeSetup.ProgramId + ", " + feeSetup.BatchId + ", " + feeSetup.ScholarshipStatusType + ", " + feeSetup.GovNonGovType + ", ",
                                    user.LogInID + "Fee set up Update",
                                    ((int)CommonEnum.PageName.SectionChangeAfterReg).ToString(),
                                    CommonEnum.PageName.SectionChangeAfterReg.ToString(),
                                    _pageUrl,
                                    "");
                                #endregion
                            }
                            else
                            {
                                lblMsg.Text = "Fees could not updated successfully.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
            finally
            {
                LoadGrid();
            }

        }

        private void LoadGrid()
        {
            //ClearReportViewer();

            int programId = Convert.ToInt32(ucProgram.selectedValue);
            //int scholarshipStatusId = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
            //int govNonGovId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
            if (programId == -1)
            {
                lblMsg.Text = "Please select a program.";
                return;
            }

            int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
            if (admissionAcaCalId == -1)
            {
                lblMsg.Text = "Please select a  admission session.";
                return;
            }
            //if (govNonGovId == -1)
            //{
            //    lblMsg.Text = "Please select a Gov / non Gov.";
            //    return;
            //}
            //string feeType = "Fee";
            //List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdBatchIdScholarshipStatusAndGovNonGov(programId, batchId, scholarshipStatusId, govNonGovId).Where(d => d.Amount != 0).ToList();
            List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).Where(d => d.Amount != 0).ToList();
            list = list.OrderBy(m => m.Type.Definition).ToList();
            feeSetupGridView.DataSource = list;
            feeSetupGridView.DataBind();
            feeSetupGridView.EmptyDataText = "No data found.";
        }

        protected void btnLoadAllFee_Click(object sender, EventArgs e)
        {
            try
            {
                //ClearReportViewer();
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //int scholarshipStatusId = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
                //int govNonGovId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "Please select a program.";
                    return;
                }
                int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionAcaCalId == -1)
                {
                    lblMsg.Text = "Please select a  admission session.";
                    return;
                }
                //if (scholarshipStatusId == -1)
                //{
                //    lblMsg.Text = "Please select scholarship status.";
                //    return;
                //}
                //if (govNonGovId == -1)
                //{
                //    lblMsg.Text = "Please select a Gov / non Gov.";
                //    return;
                //}
                //string feeType = "Fee";
                //List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdBatchIdScholarshipStatusAndGovNonGov(programId, batchId, scholarshipStatusId, govNonGovId).ToList();
                List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).ToList();
                list = list.OrderBy(m => m.Type.Definition).ToList();
                feeSetupGridView.DataSource = list;
                feeSetupGridView.DataBind();
                feeSetupGridView.EmptyDataText = "No data found.";
            }
            catch (Exception ex)
            {
                Utilities.ShowMassage(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = String.Empty;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //int scholarshipStatusId = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
                //int govNonGovId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "Please select a program.";
                    return;
                }
                int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionAcaCalId == -1)
                {
                    lblMsg.Text = "Please select a  admission session.";
                    return;
                }
                //if (scholarshipStatusId == -1)
                //{
                //    lblMsg.Text = "Please select scholarship status.";
                //    return;
                //}
                //if (govNonGovId == -1)
                //{
                //    lblMsg.Text = "Please select a parent job status";
                //    return;
                //}
                int forwardProgramId = Convert.ToInt32(ddlProgramForward.SelectedValue);
                if (forwardProgramId == -1)
                {
                    lblMsg.Text = "Please select a forwarding program.";
                    return;
                }
                int frowardBatchId = Convert.ToInt32(forwardAdmissionSessionDropDownList.SelectedValue);
                if (frowardBatchId == 0)
                {
                    lblMsg.Text = "Please select a forward batch.";
                    return;
                }

                //List<LogicLayer.BusinessObjects.FeeSetup> feeList = FeeSetupManager.GetAllByProgramIdBatchIdScholarshipStatusAndGovNonGov(programId, batchId, scholarshipStatusId, govNonGovId).Where(d => d.Amount != 0).ToList();
                List<LogicLayer.BusinessObjects.FeeSetup> feeList = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).Where(d => d.Amount != 0).ToList();
                var forwardBatchFeeSetupList = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(forwardProgramId, frowardBatchId, 0, 0).Where(d => d.Amount != 0).ToList();
                foreach (LogicLayer.BusinessObjects.FeeSetup feeSetup in feeList)
                {
                    var isFeeExist = forwardBatchFeeSetupList.FirstOrDefault(m => m.ProgramId == forwardProgramId && m.TypeDefinitionId == feeSetup.TypeDefinitionId
                                                                                                                   && m.GovNonGovType == feeSetup.GovNonGovType); // && m.Amount == feeSetup.Amount
                    if (isFeeExist == null)
                    {
                        feeSetup.ProgramId = forwardProgramId;
                        feeSetup.BatchId = frowardBatchId;
                        feeSetup.CreatedBy = BaseCurrentUserObj.Id;
                        feeSetup.CreatedDate = DateTime.Now;
                        int insertResult = FeeSetupManager.Insert(feeSetup);
                        if (insertResult > 0)
                        {
                            lblMsg.Text = "Fees inserted successfully.";

                            #region Log Insert
                            LogGeneralManager.Insert(
                                DateTime.Now,
                                "",
                                "",
                                user.LogInID,
                                "",
                                "",
                                "Fee set up insert",
                                user.LogInID + " has inserted fee for ProgramId,BatchId,ScholarshipStatusType, " + feeSetup.ProgramId + ", " + feeSetup.BatchId + ", " + feeSetup.ScholarshipStatusType + ", " + feeSetup.GovNonGovType + ", ",
                                user.LogInID + "Fee set up insert",
                                ((int)CommonEnum.PageName.SectionChangeAfterReg).ToString(),
                                CommonEnum.PageName.SectionChangeAfterReg.ToString(),
                                _pageUrl,
                                "");
                            #endregion
                        }
                        else
                        {
                            lblMsg.Text = "Fees could not inserted successfully.";
                        }
                    }
                    else if (isFeeExist.Amount == 0)
                    {
                        feeSetup.ProgramId = forwardProgramId;
                        feeSetup.BatchId = frowardBatchId;
                        feeSetup.ModifiedBy = BaseCurrentUserObj.Id;
                        feeSetup.ModifiedDate = DateTime.Now;
                        bool updateResult = FeeSetupManager.Update(feeSetup);
                        if (updateResult)
                        {
                            lblMsg.Text = "Fees updated successfully.";
                            #region Log Update
                            LogGeneralManager.Insert(
                                DateTime.Now,
                                "",
                                "",
                                user.LogInID,
                                "",
                                "",
                                "Fee set up Update",
                                user.LogInID + " has updated fee for ProgramId,BatchId,ScholarshipStatusType, " + feeSetup.ProgramId + ", " + feeSetup.BatchId + ", " + feeSetup.ScholarshipStatusType + ", " + feeSetup.GovNonGovType + ", ",
                                user.LogInID + "Fee set up Update",
                                ((int)CommonEnum.PageName.SectionChangeAfterReg).ToString(),
                                CommonEnum.PageName.SectionChangeAfterReg.ToString(),
                                _pageUrl,
                                "");
                            #endregion
                        }
                        else
                        {
                            lblMsg.Text = "Fees could not updated successfully.";
                        }
                    }
                }
                if (feeList.Any())
                {
                    //List<LogicLayer.BusinessObjects.FeeSetup> forwardFeeList = FeeSetupManager.GetForwardedFee(programId, batchId, scholarshipStatusId, govNonGovId, frowardBatchId);
                    //List<LogicLayer.BusinessObjects.FeeSetup> forwardFeeList = FeeSetupManager.GetForwardedFee(programId, batchId, 0, govNonGovId, frowardBatchId);
                    //if (forwardFeeList != null)
                    //{
                    //    lblMsg.Text = "Fee setup forwarded successfully.";
                    //    //LoadGrid();
                    //}
                    //else
                    //{
                    //    lblMsg.Text = "Fee setup could not forwarded successfully.";
                    //}
                }
                else
                {
                    lblMsg.Text = "Fee setup already exist in selected program and selected forward batch.";
                }

            }
            catch (Exception ex) { }
        }

        protected void admissionSessionDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string message = null;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //int scholarshipStatusId = Convert.ToInt32(scholarShipDropDownList.SelectedValue);
                //int govNonGovId = Convert.ToInt32(govNonGovDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "Please select a program.";
                    return;
                }
                int admissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionAcaCalId == -1)
                {
                    lblMsg.Text = "Please select a  admission session.";
                    return;
                }
                List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, admissionAcaCalId, 0, 0).Where(d => d.Amount != 0).ToList();
                list = list.OrderBy(m => m.Type.Definition).ToList();
                feeSetupGridView.DataSource = list;
                feeSetupGridView.DataBind();

            }
            catch (Exception ex)
            {
                Utilities.ShowMassage(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void programDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                lblMsg.Text = string.Empty;
                if (IsSessionVariableExists(SESSIONTYPEDEF))
                {
                    RemoveFromSession(SESSIONTYPEDEF);
                }
                if (IsSessionVariableExists(SESSIONTABLE))
                {
                    RemoveFromSession(SESSIONTABLE);
                }
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                if (programId == -1)
                {
                    return;
                }
                LoadAdmissionSessionDropDownList();
                //ucBatchForward.LoadDropDownList(programId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ddlProgramForward_OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                if (IsSessionVariableExists(SESSIONTYPEDEF))
                {
                    RemoveFromSession(SESSIONTYPEDEF);
                }
                if (IsSessionVariableExists(SESSIONTABLE))
                {
                    RemoveFromSession(SESSIONTABLE);
                }
                int programId = Convert.ToInt32(ddlProgramForward.SelectedValue);
                if (programId == -1)
                {
                    lblMsg.Text = "select forward program.";
                    return;
                }
                LoadForwardAdmissionSessionDropDownList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void feeSetupGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Find the DropDownList in the Row
                    DropDownList fundTypeDropDownList = (e.Row.FindControl("fundTypeDropDownList") as DropDownList);
                    fundTypeDropDownList.DataSource = FundTypeManager.GetAll();
                    fundTypeDropDownList.DataTextField = "FundName";
                    fundTypeDropDownList.DataValueField = "FundTypeId";
                    fundTypeDropDownList.DataBind();

                    //Add Default Item in the DropDownList
                    fundTypeDropDownList.Items.Insert(0, new ListItem("Select"));

                    //Select the fundType of fee in DropDownList
                    HiddenField fundTypeIdHiddenField = (e.Row.FindControl("fundTypeIdHiddenField") as HiddenField);
                    HiddenField fundTypeIdInFeeSetUpTableHiddenField = (e.Row.FindControl("fundTypeIdInFeeSetUpTableHiddenField") as HiddenField);
                    if (fundTypeIdHiddenField != null)
                    {
                        if (Convert.ToInt32(fundTypeIdInFeeSetUpTableHiddenField.Value) == 0)
                        {
                            fundTypeDropDownList.SelectedValue = fundTypeIdHiddenField.Value;
                        }

                        else
                        {
                            fundTypeDropDownList.SelectedValue = fundTypeIdInFeeSetUpTableHiddenField.Value;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void PopupMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('" + message + "');", true);
        }

        private void ClearGridView()
        {
            feeSetupGridView.DataSource = null;
            feeSetupGridView.DataBind();
            feeSetupGridView.EmptyDataText = String.Empty;
        }

        private void ClearReportViewer()
        {
            ReportDataSource rds = new ReportDataSource(null);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.Visible = false;
        }

        protected void downLoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                //ClearGridView();
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                var programList = ProgramManager.GetAll();
                if (programId != 0)
                {
                    programList = programList.Where(x => x.ProgramID == programId).ToList();
                }

                List<LogicLayer.BusinessObjects.FeeSetup>
                    feeSetupList = new List<LogicLayer.BusinessObjects.FeeSetup>();
                foreach (Program program in programList)
                {
                    List<LogicLayer.BusinessObjects.FeeSetup> list = FeeSetupManager
                        .GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(program.ProgramID, null, 0, 0)
                        .Where(d => d.Amount != 0).ToList();
                    if (list.Any())
                    {
                        feeSetupList = feeSetupList.Concat(list).ToList();
                    }
                }

                if (feeSetupList.Any())
                {
                    //List<ReportParameter> parameter = new List<ReportParameter>();
                    //parameter.Add(new ReportParameter("Date", date));
                    //parameter.Add(new ReportParameter("Time", time));
                    //parameter.Add(new ReportParameter("Semester", semester));
                    //parameter.Add(new ReportParameter("Room", room));
                    //parameter.Add(new ReportParameter("CourseCode", courseCode));
                    //parameter.Add(new ReportParameter("Section", courseSection));
                    //parameter.Add(new ReportParameter("CourseTitle", courseTitle));
                    //parameter.Add(new ReportParameter("CourseTeacher", courseTeacher));
                    //parameter.Add(new ReportParameter("FirstId", firstId));
                    //parameter.Add(new ReportParameter("LastId", lastId));
                    //parameter.Add(new ReportParameter("Total", total));
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.Reset();

                    ReportViewer1.LocalReport.ReportPath =
                        Server.MapPath("~/Module/bill/Report/rdlc/FeeSetupDetailsReport.rdlc");

                    //this.ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportDataSource rds = new ReportDataSource("FeeSetup", feeSetupList);
                    //ReportDataSource reserveDataSource = new ReportDataSource("DS_ReserveInvigilator", reserveInvigilatorList);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);

                    #region  download PDF

                    //Warning[] warnings;
                    //string[] streamids;
                    //string mimeType;
                    //string encoding;
                    //string filenameExtension;

                    //if (File.Exists(Server.MapPath("~/Upload/ReportPDF/" + "topSheet" + ".pdf")))
                    //{
                    //    System.IO.File.Delete(Server.MapPath("~/Upload/ReportPDF/" + "topSheet" + ".pdf"));
                    //}


                    //byte[] bytes = invigilatorReportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    //using (FileStream fs = new FileStream(Server.MapPath("~/Upload/ReportPDF/" + "topSheet" + ".pdf"), FileMode.Create))
                    //{
                    //    fs.Write(bytes, 0, bytes.Length);
                    //}

                    ////Response.Redirect(url, false);
                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                    //Response.ClearHeaders();
                    //Response.ClearContent();
                    //Response.Buffer = true;
                    //Response.Clear();
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "topSheet.pdf");
                    //Response.TransmitFile(Server.MapPath("~/Upload/ReportPDF/" + "topSheet.pdf"));
                    //Response.Flush();
                    //Response.Close();

                    #endregion
                }
            }
            catch (Exception exception)
            {

            }
        }

        protected void ucDepartment_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            ucProgram.LoadDropdownByDepartmentId(departmentId);
            LoadAdmissionSessionDropDownList();
        }

    }
}