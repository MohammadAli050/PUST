using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System.Web.Services;
using Newtonsoft.Json;

namespace EMS.Module.admin
{
    public partial class ExamSetups : BasePage
    {
        User user = null;

        int GeneralMessage = 1;
        int ModalMessage = 2;
        int ExamCommitteeMessage = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //base.CheckPage_Load();
                string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                user = UserManager.GetByLogInId(loginId);
                if (!IsPostBack && !IsCallback)
                {
                    hfExamSetupId.Value = "0";
                    hfExamSetupWithExamCommitteesId.Value = "0";

                    ucDepartment.LoadDropDownListWithUserAccess(user.User_ID, user.RoleID);
                    LoadDDL();
                    //LoadGridView();
                }
            }
            catch (Exception) { }
        }

        protected void MessageView(string msg, string status, int msgType)
        {

            if (status == "success")
            {
                if (msgType == 1)
                {
                    lblMsg.Text = string.Empty;
                    lblMsg.Text = msg.ToString();
                    lblMsg.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMsg.Attributes.CssStyle.Add("color", "green");
                }
                else if (msgType == 2)
                {
                    lblMessage.Text = string.Empty;
                    lblMessage.Text = msg.ToString();
                    lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMessage.Attributes.CssStyle.Add("color", "green");
                }
                else if (msgType == 3)
                {
                    lblMessageExamCommittee.Text = string.Empty;
                    lblMessageExamCommittee.Text = msg.ToString();
                    lblMessageExamCommittee.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMessageExamCommittee.Attributes.CssStyle.Add("color", "green");
                }
            }
            else if (status == "fail")
            {
                if (msgType == 1)
                {
                    lblMsg.Text = string.Empty;
                    lblMsg.Text = msg.ToString();
                    lblMsg.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMsg.Attributes.CssStyle.Add("color", "crimson");
                }
                else if (msgType == 2)
                {
                    lblMessage.Text = string.Empty;
                    lblMessage.Text = msg.ToString();
                    lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMessage.Attributes.CssStyle.Add("color", "crimson");
                }
                else if (msgType == 3)
                {
                    lblMessageExamCommittee.Text = string.Empty;
                    lblMessageExamCommittee.Text = msg.ToString();
                    lblMessageExamCommittee.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMessageExamCommittee.Attributes.CssStyle.Add("color", "crimson");
                }

            }
            else if (status == "clear")
            {
                if (msgType == 1)
                {
                    lblMsg.Text = string.Empty;
                }
                else if (msgType == 2)
                {
                    lblMessage.Text = string.Empty;
                }
                else if (msgType == 3)
                {
                    lblMessageExamCommittee.Text = string.Empty;
                }
            }

        }

        private void LoadDDL()
        {
            #region Load Department Modal
            ucDepartmentModal.LoadDropDownList();
            #endregion


            #region Load Program
            ucProgram.LoadDropdownWithUserAccess(user.User_ID);
            #endregion

            #region Load Program Modal
            ucProgramModal.LoadDropDownListWithoutSelectionProgramAll();
            #endregion


            #region Load Year
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYear.Items.Clear();
            ddlYear.AppendDataBoundItems = true;
            ddlYear.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYear.DataTextField = "YearNoName";
                ddlYear.DataValueField = "YearNo";

                ddlYear.DataSource = yearList;
                ddlYear.DataBind();

            }
            #endregion

            #region Load Year Modal
            ddlYearModal.Items.Clear();
            ddlYearModal.AppendDataBoundItems = true;
            ddlYearModal.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYearModal.DataTextField = "YearNoName";
                ddlYearModal.DataValueField = "YearNo";

                ddlYearModal.DataSource = yearList;
                ddlYearModal.DataBind();

            }
            #endregion


            #region Load SemesterNo
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
            #endregion

            #region Load Semester Modal
            ddlSemesterModal.Items.Clear();
            ddlSemesterModal.AppendDataBoundItems = true;
            ddlSemesterModal.Items.Add(new ListItem("-Select-", "-1"));
            if (semesterList != null && semesterList.Count > 0)
            {
                ddlSemesterModal.DataTextField = "SemesterNoName";
                ddlSemesterModal.DataValueField = "SemesterNo";

                ddlSemesterModal.DataSource = semesterList;
                ddlSemesterModal.DataBind();

            }
            #endregion



            #region Load Shal
            ddlShal.Items.Clear();
            ddlShal.Items.Add(new ListItem("-Select Exam Year-", "-1"));
            ddlShal.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlShal.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            #endregion

            #region Load Shal Modal
            ddlShalModal.Items.Clear();
            ddlShalModal.Items.Add(new ListItem("-Select Exam Year-", "-1"));
            ddlShalModal.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlShalModal.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            #endregion


            //DepartmentUserControl1.LoadDropDownListWithoutSelectionFirstDept();
            //DepartmentUserControl2.LoadDropDownListWithoutSelectionFirstDept();
            //DepartmentUserControl3.LoadDropDownListWithoutSelectionFirstDept();


        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnDepartmentSelectedIndexChangedModal(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgramModal.LoadDropdownByDepartmentId(departmentId);

                this.ModalPopupExtender2.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadGridView()
        {
            try
            {
                #region Paramiters
                int? programId = null;
                try
                {
                    if (Convert.ToInt32(ucProgram.selectedValue) > 0)
                    {
                        programId = Convert.ToInt32(ucProgram.selectedValue);
                    }
                }
                catch (Exception ex)
                {

                }

                int? yearNo = null;
                try
                {
                    if (Convert.ToInt32(ddlYear.SelectedValue) > 0)
                    {
                        yearNo = Convert.ToInt32(ddlYear.SelectedValue);
                    }
                }
                catch (Exception ex)
                {

                }

                int? semesterNo = null;
                try
                {
                    if (Convert.ToInt32(ddlSemesterNo.SelectedValue) > 0)
                    {
                        semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                    }
                }
                catch (Exception ex)
                {

                }

                int? shal = null;
                try
                {
                    if (Convert.ToInt32(ddlShal.SelectedValue) > 0)
                    {
                        shal = Convert.ToInt32(ddlShal.SelectedValue);
                    }
                }
                catch (Exception ex)
                {

                }

                int? sessionId = null;
                try
                {
                    if (Convert.ToInt32(ucAdmissionSession.selectedValue) > 0)
                    {
                        sessionId = Convert.ToInt32(ucAdmissionSession.selectedValue);
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion


                List<ExamSetupDTO> list = ExamSetupManager.GetAllExamSetupDTO(programId, yearNo, semesterNo, shal, sessionId);

                if (list != null && list.Count > 0)
                {
                    gvExamSetup.DataSource = list;
                    gvExamSetup.DataBind();

                    btnAddCommittee.Visible = true;
                }
                else
                {
                    gvExamSetup.DataSource = null;
                    gvExamSetup.DataBind();

                    btnAddCommittee.Visible = false;
                }

            }
            catch (Exception ex)
            {

            }

        }

        protected void loadButton_Click(object sender, EventArgs e)
        {
            MessageView("", "clear", GeneralMessage);
            MessageView("", "clear", ModalMessage);

            LoadGridView();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });

                int examSetupId = int.Parse(commandArgs[0]);
                int examSetupDetailId = int.Parse(commandArgs[1]);
                int semesterNo = int.Parse(commandArgs[2]);

                if (examSetupId > 0 && examSetupDetailId > 0 && semesterNo > 0)
                {
                    List<ExamSetupDetail> esdList = ExamSetupDetailManager.GetAllByExamSetupId(examSetupId);

                    if (esdList.Count == 1)
                    {
                        ExamSetupDetail model = esdList.Where(x => x.ExamSetupDetailId == examSetupDetailId).FirstOrDefault();
                        if (model != null)
                        {
                            // Delete ExamSetupDetail
                            bool resultExamSetupDetail = ExamSetupDetailManager.Delete(model.ExamSetupDetailId);

                            if (resultExamSetupDetail == true)
                            {
                                ExamSetupWithExamCommittees examSetupWithExamCommittees = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examSetupId);
                                if (examSetupWithExamCommittees != null)
                                {
                                    // Delete ExamSetupWithExamCommittees
                                    bool resultExamSetupWithExamCommittees = ExamSetupWithExamCommitteesManager.Delete(examSetupWithExamCommittees.ID);
                                }


                                // Delete ExamSetup
                                bool resultExamSetup = ExamSetupManager.Delete(examSetupId);

                                if (resultExamSetup == true)
                                {
                                    LoadGridView();

                                    MessageView("Data Delete Successfully", "success", GeneralMessage);
                                }
                                else
                                {
                                    MessageView("Failed to Delete Data !!", "fail", GeneralMessage);
                                }
                            }
                            else
                            {
                                MessageView("Failed to Delete Data !!", "fail", GeneralMessage);
                            }
                        }

                    }
                    else if (esdList.Count > 1)
                    {
                        ExamSetupDetail model = esdList.Where(x => x.ExamSetupDetailId == examSetupDetailId).FirstOrDefault();
                        if (model != null)
                        {
                            bool result = ExamSetupDetailManager.Delete(model.ExamSetupDetailId);

                            if (result == true)
                            {
                                LoadGridView();

                                MessageView("Data Delete Successfully", "success", GeneralMessage);
                            }
                            else
                            {
                                MessageView("Failed to Delete Data !!", "fail", GeneralMessage);
                            }
                        }
                    }
                    else
                    {
                        MessageView("No Data Found for Delete !!", "fail", GeneralMessage);
                    }


                    //ExamSetup model = ExamSetupManager.GetById(examSetupId);
                    //if (model != null)
                    //{

                    //    bool result = ExamSetupManager.Delete(model.ID);

                    //    if (result == true)
                    //    {
                    //        LoadGridView();

                    //        MessageView("Data Delete Successfully", "success", GeneralMessage);
                    //    }
                    //    else
                    //    {
                    //        MessageView("Failed to Delete Data !!", "fail", GeneralMessage);
                    //    }

                    //}
                    //else
                    //{
                    //    MessageView("No Data Found !!", "fail", GeneralMessage);
                    //}
                }
                else
                {
                    MessageView("Failed to Delete Data !!", "fail", GeneralMessage);
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something Went Wrong Delete; Error: " + ex.Message.ToString(), "fail", GeneralMessage);
            }
        }

        private void ClearField()
        {
            #region Exam Modal
            ucDepartmentModal.LoadDropDownList();
            ucProgramModal.LoadDropDownListWithoutSelectionProgram();
            ddlYearModal.SelectedValue = "-1";
            ddlSemesterModal.SelectedValue = "-1";
            ucAdmissionSession.SelectedValue(0);
            ddlShalModal.SelectedValue = "-1";
            //txtExamNameModal.Text = string.Empty;
            ddlExamName.SelectedValue = "1";
            txtExamStartDateModal.Text = string.Empty;
            txtExamEndDateModal.Text = string.Empty;
            ucAdmissionSessionModal.SelectedValue(0);
            cbIsActive.Checked = false;
            #endregion


            #region Enable(True) Exam Modal Field
            ucDepartmentModal.Enabled(true);
            ucProgramModal.Enable(true);
            ddlYearModal.Enabled = true;
            ddlSemesterModal.Enabled = true;
            ddlShalModal.Enabled = true;
            #endregion


            #region Exam Committee
            ddlExamCommitteeChairman.SelectedValue = "0";
            ddlExamCommitteeMemberOne.SelectedValue = "0";
            ddlExamCommitteeMemberTwo.SelectedValue = "0";
            #endregion


            btnSave.Text = "Save";
            btnSaveExamCommittee.Text = "Save";

            hfExamSetupId.Value = "0";
            hfExamSetupWithExamCommitteesId.Value = "0";

        }


        #region Modal Section

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MessageView("", "clear", ModalMessage);

            ClearField();

            this.ModalPopupExtender2.Show();

            btnSave.Text = "Save";

            hfExamSetupId.Value = "0";

            if (Convert.ToInt32(ucDepartment.selectedValue) > 0)
            {
                ucDepartmentModal.SelectedValue(Convert.ToInt32(ucDepartment.selectedValue));
            }
            if (Convert.ToInt32(ucProgram.selectedValue) > 0)
            {
                ucProgramModal.SelectedValue(Convert.ToInt32(ucProgram.selectedValue));
            }
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                ddlYearModal.SelectedValue = ddlYear.SelectedValue;
            }
            if (Convert.ToInt32(ddlSemesterNo.SelectedValue) > 0)
            {
                ddlSemesterModal.SelectedValue = ddlSemesterNo.SelectedValue;
            }
            if (Convert.ToInt32(ddlShal.SelectedValue) > 0)
            {
                ddlShalModal.SelectedValue = ddlShal.SelectedValue;
            }

        }

        protected void ucProgramModal_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
        }

        protected void ucAdmissionSessionModal_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ModalPopupExtender2.Show();

                MessageView("", "clear", GeneralMessage);
                MessageView("", "clear", ModalMessage);



                int programId = Convert.ToInt32(ucProgramModal.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearModal.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterModal.SelectedValue);
                if (programId == 0 && yearNo > 0 && semesterNo > 0)
                {
                    int errorCount = 1;
                    Dictionary<int, string> dictErrorList = new Dictionary<int, string>();

                    // ================ Multiple Program ====================
                    int saveOrUpdateCount = 0;
                    List<Program> programList = new List<Program>();
                    programList = ProgramManager.GetAll();
                    foreach (var tData in programList)
                    {
                        YearSemesterDTO yearSemesterDTO = ExamSetupManager.GetYearSemesterByProgramIdYearNoSemesterNo(tData.ProgramID, yearNo, semesterNo);
                        if (yearSemesterDTO != null)
                        {
                            MessageModelDTO dataSaveOrUpdate = ExamSetupSaveOrUpdate(tData.ProgramID, yearSemesterDTO.YearNo, yearSemesterDTO.SemesterNo);

                            if (dataSaveOrUpdate.MessageCode == 2)
                            {
                                //saveOrUpdateCount++;

                                dictErrorList.Add(errorCount++, dataSaveOrUpdate.MessageBody);
                            }
                        }

                    }

                    if (dictErrorList != null && dictErrorList.Count > 0)
                    {
                        string massageError = "";
                        foreach (var tData in dictErrorList)
                        {
                            massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                        }


                        MessageView(massageError, "fail", ModalMessage);
                        MessageView(massageError, "fail", GeneralMessage);
                    }
                    else
                    {
                        ClearField();

                        LoadGridView();

                        this.ModalPopupExtender2.Hide();

                        MessageView("Exam Setup Updated Successfully", "success", ModalMessage);
                        MessageView("Exam Setup Updated Successfully", "success", GeneralMessage);
                    }


                    //if (saveOrUpdateCount > 0)
                    //{

                    //}
                    //else
                    //{
                    //    MessageView("Data Update Failed !!", "fail", ModalMessage);
                    //    MessageView("Data Update Failed !!", "fail", GeneralMessage);
                    //}

                }
                else if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    // ================ Single Program ====================
                    YearSemesterDTO yearSemesterDTO = ExamSetupManager.GetYearSemesterByProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);

                    MessageModelDTO dataSaveOrUpdate = ExamSetupSaveOrUpdate(programId, yearSemesterDTO.YearNo, yearSemesterDTO.SemesterNo);

                    if (dataSaveOrUpdate.MessageCode == 1)
                    {
                        ClearField();

                        LoadGridView();

                        this.ModalPopupExtender2.Hide();

                        MessageView(dataSaveOrUpdate.MessageBody, "success", ModalMessage);
                        MessageView(dataSaveOrUpdate.MessageBody, "success", GeneralMessage);
                    }
                    else
                    {
                        MessageView(dataSaveOrUpdate.MessageBody, "fail", ModalMessage);
                        MessageView(dataSaveOrUpdate.MessageBody, "fail", GeneralMessage);
                    }

                }
                else
                {
                    MessageView("Please Provide All Field Input to Save/Update Data !!", "fail", ModalMessage);
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something Went Wrong Save/Update; Error: " + ex.Message.ToString(), "fail", ModalMessage);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            MessageView("", "clear", GeneralMessage);
            MessageView("", "clear", ModalMessage);



            btnSave.Text = "Update";
            hfExamSetupId.Value = "0";

            LinkButton btn = (LinkButton)sender;

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });

            int examSetupId = int.Parse(commandArgs[0]);
            int examSetupDetailId = int.Parse(commandArgs[1]);
            int semesterNo = int.Parse(commandArgs[2]);

            if (examSetupId > 0 && examSetupDetailId > 0 && semesterNo > 0)
            {
                hfExamSetupId.Value = examSetupId.ToString();
                hfExamSetupDetailId.Value = examSetupDetailId.ToString();
                hfSemesterNo.Value = semesterNo.ToString();

                ExamSetup obj = ExamSetupManager.GetById(examSetupId);
                if (obj != null)
                {

                    ExamSetupDetail esdModel = ExamSetupDetailManager.GetByExamSetupIdSemesterNo(examSetupId, semesterNo);

                    if (esdModel != null)
                    {

                        this.ModalPopupExtender2.Show();

                        ucProgramModal.LoadDropDownListWithoutSelectionProgram();
                        if (!string.IsNullOrEmpty(obj.ProgramId.ToString()))
                        {
                            ucProgramModal.SelectedValue(Convert.ToInt32(obj.ProgramId));

                            Program program = ProgramManager.GetById(Convert.ToInt32(obj.ProgramId));
                            ucDepartmentModal.SelectedValue(program.DeptID);
                        }




                        ddlShalModal.SelectedValue = obj.Shal.ToString();
                        ddlExamName.SelectedValue = esdModel.ExamName == "Final" ? "1" : "2";

                        try
                        {
                            if (!string.IsNullOrEmpty(esdModel.ExamStartDate.ToString()))
                            {
                                txtExamStartDateModal.Text = Convert.ToDateTime(esdModel.ExamStartDate).ToString("dd/MM/yyyy");
                            }
                            if (!string.IsNullOrEmpty(esdModel.ExamEndDate.ToString()))
                            {
                                txtExamEndDateModal.Text = Convert.ToDateTime(esdModel.ExamEndDate).ToString("dd/MM/yyyy");
                            }
                        }
                        catch (Exception ex)
                        {
                        }


                        //ucProgramModal_ProgramSelectedIndexChanged(null, null);
                        ddlYearModal.SelectedValue = obj.YearNo.ToString();

                        //ddlYearModal_SelectedIndexChanged(null, null);
                        ddlSemesterModal.SelectedValue = esdModel.SemesterNo.ToString();


                        ucAdmissionSessionModal.LoadDropDownList();
                        if (!string.IsNullOrEmpty(obj.AcaCalId.ToString()))
                        {
                            ucAdmissionSessionModal.SelectedValue(Convert.ToInt32(obj.AcaCalId));
                        }


                        cbIsActive.Checked = Convert.ToBoolean(obj.IsActive);


                        #region Enable(False) Modal Field
                        ucDepartmentModal.Enabled(false);
                        ucProgramModal.Enable(false);
                        ddlYearModal.Enabled = false;
                        ddlSemesterModal.Enabled = false;
                        ddlShalModal.Enabled = false;
                        #endregion

                    }
                    else
                    {
                        MessageView("No Exam Setup Details Data Found !!", "fail", GeneralMessage);
                    }

                }
                else
                {
                    MessageView("No Exam Setup Data Found !!", "fail", GeneralMessage);
                }
            }
            else
            {
                MessageView("No Exam Setup Details Data Found !!", "fail", GeneralMessage);
            }
        }

        #endregion


        private MessageModelDTO ExamSetupSaveOrUpdate(int programId, int yearNo, int semesterNo)
        {
            MessageModelDTO mm = new MessageModelDTO();

            try
            {
                bool dataSaveOrUpdate = false;

                int dublicateCount = 0;

                int departmentId = Convert.ToInt32(ucDepartmentModal.selectedValue);
                int sessionId = Convert.ToInt32(ucAdmissionSessionModal.selectedValue);
                int shalId = Convert.ToInt32(ddlShalModal.SelectedValue);
                int examNameId = Convert.ToInt32(ddlExamName.SelectedValue);
                //int examNameString = Convert.ToInt32(ddlExamName.SelectedItem.Text);
                DateTime? examStartDate = null;
                DateTime? examEndDate = null;
                //&& !string.IsNullOrEmpty(txtExamNameModal.Text)
                //&& !string.IsNullOrEmpty(txtExamStartDateModal.Text)
                //&& !string.IsNullOrEmpty(txtExamEndDateModal.Text)
                if (departmentId > 0 && programId > 0 && yearNo > 0 && semesterNo > 0 && sessionId > 0 && shalId > 0
                    && examNameId > 0)
                {
                    string examName = ddlExamName.SelectedItem.Text;
                    string programName = ucProgramModal.selectedText;
                    string yearNoName = ddlYearModal.SelectedItem.Text;
                    string semesterNoName = ddlSemesterModal.SelectedItem.Text;
                    string shalName = ddlShalModal.SelectedItem.Text;

                    try
                    {
                        if (!string.IsNullOrEmpty(txtExamStartDateModal.Text) && !string.IsNullOrEmpty(txtExamEndDateModal.Text))
                        {
                            examStartDate = DateTime.ParseExact(txtExamStartDateModal.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            examEndDate = DateTime.ParseExact(txtExamEndDateModal.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        }
                    }
                    catch (Exception ex)
                    {
                        examStartDate = null;
                        examEndDate = null;
                    }

                    ExamSetup obj = new ExamSetup();
                    obj.ProgramId = programId;
                    obj.YearNo = yearNo;
                    //obj.SemesterNo = semesterNo;
                    obj.AcaCalId = sessionId;
                    obj.Year = shalId;
                    obj.Shal = shalId;
                    //obj.ExamName = examName;
                    //obj.ExamStartDate = examStartDate;
                    //obj.ExamEndDate = examEndDate;
                    obj.IsActive = cbIsActive.Checked;


                    int examsetupid = Convert.ToInt32(hfExamSetupId.Value);

                    //// For Save
                    //if (btnSave.Text == "Save" && hfExamSetupId.Value == "0")
                    //{


                    if (examsetupid == 0)
                    {
                        // Insert

                        ExamSetup examSetupModel = ExamSetupManager.GetByProgramIdYearNoShal(programId, yearNo, shalId);

                        if (examSetupModel != null)
                        {
                            #region N/A
                            //examSetupModel.AcaCalId = sessionId;
                            //examSetupModel.ModifiedBy = user.User_ID;
                            //examSetupModel.ModifiedDate = DateTime.Now;

                            //bool resultExamSetup = ExamSetupManager.Update(examSetupModel);

                            ////if (resultExamSetup == true)
                            ////{
                            ////    dataSaveOrUpdate = true;
                            ////} 
                            #endregion

                            // Insert/Update ExamSetupDetails
                            ExamSetupDetail examSetupDetailModel = ExamSetupDetailManager.GetByExamSetupIdSemesterNo(examSetupModel.ID, semesterNo);

                            if (examSetupDetailModel != null)
                            {
                                mm.MessageCode = 2;
                                mm.MessageBoolean = false;
                                mm.MessageBody = "Exam Setup is already Exist with [Program: " + programName + ", Year: " + yearNoName + ", Semester: " + semesterNoName + ", Exam Year: " + shalName + "]";

                                #region N/A
                                //// Update ExamSetupDetails

                                //examSetupDetailModel.SemesterNo = semesterNo;
                                //examSetupDetailModel.ExamName = examName;
                                //examSetupDetailModel.ExamStartDate = examStartDate;
                                //examSetupDetailModel.ExamEndDate = examEndDate;
                                //examSetupDetailModel.IsActive = cbIsActive.Checked;
                                //examSetupDetailModel.ModifiedBy = user.User_ID;
                                //examSetupDetailModel.ModifiedDate = DateTime.Now;

                                //bool resultExamSetupDetail = ExamSetupDetailManager.Update(examSetupDetailModel);

                                //if (resultExamSetupDetail == true)
                                //{
                                //    dataSaveOrUpdate = true;
                                //} 
                                #endregion
                            }
                            else
                            {
                                // Insert ExamSetupDetails

                                ExamSetupDetail esdModel = new ExamSetupDetail();
                                esdModel.ExamSetupId = examSetupModel.ID;
                                esdModel.SemesterNo = semesterNo;
                                esdModel.ExamName = examName;
                                esdModel.ExamStartDate = examStartDate;
                                esdModel.ExamEndDate = examEndDate;
                                esdModel.IsActive = cbIsActive.Checked;
                                esdModel.CreatedBy = user.User_ID;
                                esdModel.CreatedDate = DateTime.Now;

                                int examSetupDetailId = ExamSetupDetailManager.Insert(esdModel);

                                if (examSetupDetailId > 0)
                                {
                                    //dataSaveOrUpdate = true;

                                    mm.MessageCode = 1;
                                    mm.MessageBoolean = true;
                                    mm.MessageBody = "Exam Setup Save Successful.";

                                }
                            }

                        }
                        else
                        {
                            // Insert ExamSetup and ExamSetupDetails
                            #region Save ExamSetup and ExamSetupDetails

                            obj.CreatedBy = user.User_ID;
                            obj.CreatedDate = DateTime.Now;

                            int examSetupId = ExamSetupManager.Insert(obj);


                            if (examSetupId > 0)
                            {
                                ExamSetupDetail esdModel = new ExamSetupDetail();
                                esdModel.ExamSetupId = examSetupId;
                                esdModel.SemesterNo = semesterNo;
                                esdModel.ExamName = examName;
                                esdModel.ExamStartDate = examStartDate;
                                esdModel.ExamEndDate = examEndDate;
                                esdModel.IsActive = cbIsActive.Checked;
                                esdModel.CreatedBy = user.User_ID;
                                esdModel.CreatedDate = DateTime.Now;

                                int examSetupDetailId = ExamSetupDetailManager.Insert(esdModel);

                                if (examSetupDetailId > 0)
                                {
                                    //dataSaveOrUpdate = true;
                                    mm.MessageCode = 1;
                                    mm.MessageBoolean = true;
                                    mm.MessageBody = "Exam Setup Save Successful.";
                                }
                            }

                            #endregion

                        }
                        #region N/A
                        //}
                        //else
                        //{
                        //    // For Update

                        //}


                        //List<ExamSetupDTO> checkModel = ExamSetupManager.GetAllExamSetupDTO(programId, yearNo, semesterNo, sessionId);
                        //checkModel = checkModel.Where(x => x.ExamSetupID != examsetupid).ToList();

                        //if (checkModel != null && checkModel.Count > 0)
                        //{
                        //    MessageView("Exam Setup is Already Exist !!", "fail", ModalMessage);
                        //}
                        //else
                        //{
                        //    if (btnSave.Text == "Save" && hfExamSetupId.Value == "0")
                        //    {
                        //        #region Save

                        //        obj.CreatedBy = user.User_ID;
                        //        obj.CreatedDate = DateTime.Now;

                        //        int examSetupId = ExamSetupManager.Insert(obj);


                        //        if (examSetupId > 0)
                        //        {
                        //            //LoadGridView();

                        //            //MessageView("Data Saved Successfully", "success", ModalMessage);

                        //            dataSaveOrUpdate = true;
                        //        }
                        //        //else
                        //        //{
                        //        //    MessageView("Failed to Save Data !!", "fail", ModalMessage);
                        //        //}
                        //        #endregion
                        //    }
                        //    else
                        //    {
                        //        try
                        //        {

                        //            ExamSetup model = ExamSetupManager.GetById(examsetupid);
                        //            if (model != null)
                        //            {
                        //                #region Update
                        //                obj.ID = model.ID;
                        //                obj.CreatedBy = model.CreatedBy;
                        //                obj.CreatedDate = model.CreatedDate;
                        //                obj.ModifiedBy = user.User_ID;
                        //                obj.ModifiedDate = DateTime.Now;

                        //                bool result = ExamSetupManager.Update(obj);


                        //                if (result == true)
                        //                {
                        //                    //LoadGridView();

                        //                    //MessageView("Data Updated Successfully", "success", ModalMessage);

                        //                    dataSaveOrUpdate = true;
                        //                }
                        //                //else
                        //                //{
                        //                //    MessageView("Failed to Update Data !!", "fail", ModalMessage);
                        //                //}
                        //                #endregion
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            MessageView("Exception: Something Went Wrong Update; Error: " + ex.Message.ToString(), "fail", ModalMessage);
                        //            return false;
                        //        }

                        //    }
                        //} 
                        #endregion


                    }
                    else
                    {
                        // Update

                        ExamSetup examSetupModel = ExamSetupManager.GetById(examsetupid); //GetByProgramIdYearNoShal(programId, yearNo, shalId);

                        if (examSetupModel != null)
                        {
                            #region Update Exam Setup
                            examSetupModel.AcaCalId = sessionId;
                            examSetupModel.ExamName = examName;
                            examSetupModel.ModifiedBy = user.User_ID;
                            examSetupModel.ModifiedDate = DateTime.Now;

                            bool resultExamSetup = ExamSetupManager.Update(examSetupModel);

                            //if (resultExamSetup == true)
                            //{
                            //    dataSaveOrUpdate = true;
                            //} 
                            #endregion

                            if (resultExamSetup == true)
                            {

                                // Insert/Update ExamSetupDetails
                                ExamSetupDetail examSetupDetailModel = ExamSetupDetailManager.GetByExamSetupIdSemesterNo(examSetupModel.ID, semesterNo);

                                if (examSetupDetailModel != null)
                                {

                                    #region Update ExamSetupDetails
                                    //// Update ExamSetupDetails

                                    //examSetupDetailModel.SemesterNo = semesterNo;
                                    examSetupDetailModel.ExamName = examName;
                                    examSetupDetailModel.ExamStartDate = examStartDate;
                                    examSetupDetailModel.ExamEndDate = examEndDate;
                                    examSetupDetailModel.IsActive = cbIsActive.Checked;
                                    examSetupDetailModel.ModifiedBy = user.User_ID;
                                    examSetupDetailModel.ModifiedDate = DateTime.Now;

                                    bool resultExamSetupDetail = ExamSetupDetailManager.Update(examSetupDetailModel);

                                    if (resultExamSetupDetail == true)
                                    {
                                        //dataSaveOrUpdate = true;

                                        mm.MessageCode = 1;
                                        mm.MessageBoolean = true;
                                        mm.MessageBody = "Exam Setup Update Successful.";
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                }
                else
                {
                    //MessageView("Please Provide All Field Input to Save/Update Data !!", "fail", ModalMessage);

                    mm.MessageCode = 2;
                    mm.MessageBoolean = false;
                    mm.MessageBody = "Please Provide All Mandatory Field with sign (*) for Save/Update Data !!";

                }


                return mm;
            }
            catch (Exception ex)
            {
                //MessageView("Exception: Something Went Wrong Save/Update; Error: " + ex.Message.ToString(), "fail", ModalMessage);

                mm.MessageCode = 2;
                mm.MessageBoolean = false;
                mm.MessageBody = "Exception: Something Went Wrong Save/Update; Error: " + ex.Message.ToString();

                return mm;
            }
        }




        #region Modal Exam Setup Committee
        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                foreach (GridViewRow row in gvExamSetup.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("cbSelect");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddCommittee_Click(object sender, EventArgs e)
        {

            MessageView("", "clear", ExamCommitteeMessage);



            int countCheck = 0;

            try
            {
                foreach (GridViewRow row in gvExamSetup.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("cbSelect");

                    if (ckBox.Checked)
                    {
                        countCheck++;
                    }
                }

                if (countCheck > 0)
                {
                    this.ModalPopupExtenderExamCommittee.Show();

                    btnSaveExamCommittee.Text = "Save";

                    hfExamSetupWithExamCommitteesId.Value = "0";

                    #region Load Department
                    DepartmentUserControl1.LoadDropDownList();
                    DepartmentUserControl1_DepartmentSelectedIndexChanged(null, null);
                    DepartmentUserControl2.LoadDropDownList();
                    DepartmentUserControl2_DepartmentSelectedIndexChanged(null, null);
                    DepartmentUserControl3.LoadDropDownList();
                    DepartmentUserControl3_DepartmentSelectedIndexChanged(null, null);
                    DepartmentUserControl4.LoadDropDownList();
                    DepartmentUserControl4_DepartmentSelectedIndexChanged(null, null);
                    #endregion
                }
                else
                {
                    MessageView("For (Add/Edit) Committee, you need to select an Exam Setup", "fail", GeneralMessage);
                }
            }
            catch (Exception ex)
            {
                MessageView("Exception: Something Went Wrong Exam Committee Add/Edit; Error: " + ex.Message.ToString(), "fail", GeneralMessage);
            }
        }

        protected void btnSaveExamCommittee_Click(object sender, EventArgs e)
        {
            MessageView("", "clear", GeneralMessage);
            MessageView("", "clear", ExamCommitteeMessage);
            this.ModalPopupExtenderExamCommittee.Show();

            try
            {


                int examSetupWithExamCommitteesId = Convert.ToInt32(hfExamSetupWithExamCommitteesId.Value);

                if (examSetupWithExamCommitteesId > 0)
                {
                    // Update
                    #region Single Edit
                    ExamSetupWithExamCommittees model = ExamSetupWithExamCommitteesManager.GetById(examSetupWithExamCommitteesId);
                    if (model != null)
                    {
                        int chairmanDeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
                        int memberOneDeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
                        int memberTwoDeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
                        int externalMemberDeptId = Convert.ToInt32(DepartmentUserControl4.selectedValue);

                        int chairmanId = Convert.ToInt32(ddlExamCommitteeChairman.SelectedValue);
                        int memberOneId = Convert.ToInt32(ddlExamCommitteeMemberOne.SelectedValue);
                        int memberTwoId = Convert.ToInt32(ddlExamCommitteeMemberTwo.SelectedValue);
                        int externalMemberId = Convert.ToInt32(ddlExamCommitteeExternalMember.SelectedValue);

                        if (chairmanDeptId > 0 && memberOneDeptId > 0 && memberTwoDeptId > 0 && externalMemberDeptId > 0 &&
                            chairmanId > 0 && memberOneId > 0 && memberTwoId > 0 && externalMemberId > 0)
                        {

                            model.ExamCommitteeChairmanDeptId = chairmanDeptId;
                            model.ExamCommitteeMemberOneDeptId = memberOneDeptId;
                            model.ExamCommitteeMemberTwoDeptId = memberTwoDeptId;
                            model.ExamCommitteeExternalMemberDeptId = externalMemberDeptId;

                            model.ExamCommitteeChairmanId = chairmanId;
                            model.ExamCommitteeMemberOneId = memberOneId;
                            model.ExamCommitteeMemberTwoId = memberTwoId;
                            model.ExamCommitteeExternalMemberId = externalMemberId;

                            bool resultExamSetupWithExamCommittees = ExamSetupWithExamCommitteesManager.Update(model);

                            if (resultExamSetupWithExamCommittees == true)
                            {
                                ClearField();

                                LoadGridView();

                                this.ModalPopupExtenderExamCommittee.Hide();

                                MessageView("Data Updated Successfully", "success", ExamCommitteeMessage);
                                MessageView("Data Updated Successfully", "success", GeneralMessage);
                            }
                            else
                            {
                                MessageView("Failed to Update Data", "fail", ExamCommitteeMessage);
                                MessageView("Failed to Update Data", "fail", GeneralMessage);
                            }
                        }
                        else
                        {
                            MessageView("Please Select Department and Chairmen, Member One and Member Two for Update !!", "fail", ExamCommitteeMessage);
                        }

                    }
                    else
                    {
                        MessageView("No Committee is forund", "fail", ExamCommitteeMessage);
                    }
                    #endregion
                }
                else
                {
                    // Bulk (Insert/Update)   
                    #region (Single/Multiple) Insert/Update

                    int countNoCheckBox = 0;
                    int insertOrUpdateCount = 0;
                    List<ExamSetupDTO> distinctExamsetupList = new List<ExamSetupDTO>();
                    List<ExamSetupDTO> examsetupList = new List<ExamSetupDTO>();

                    #region Get Data from GridView
                    foreach (GridViewRow row in gvExamSetup.Rows)
                    {
                        Label lblExmaSetupId = (Label)row.FindControl("lblExmaSetupId");
                        CheckBox ckBox = (CheckBox)row.FindControl("cbSelect");

                        int exmaSetupId = 0;
                        try
                        {
                            exmaSetupId = Convert.ToInt32(lblExmaSetupId.Text);

                        }
                        catch (Exception ex)
                        {
                        }

                        if (ckBox.Checked && exmaSetupId > 0)
                        {

                            ExamSetupDTO model = new ExamSetupDTO();
                            model.ExamSetupID = exmaSetupId;

                            examsetupList.Add(model);
                        }
                        else
                        {
                            countNoCheckBox = countNoCheckBox + 1;
                        }

                    } // END: Loop 
                    #endregion

                    if (countNoCheckBox == gvExamSetup.Rows.Count)
                    {
                        MessageView("No Exam Setup is Selected for Assigning Exam Committee !!", "fail", ExamCommitteeMessage);
                        return;
                    }

                    #region Get Distinct ExamSetupId from List
                    if (examsetupList != null && examsetupList.Count > 0)
                    {
                        var ListOfExamSetup = examsetupList.GroupBy(x => x.ExamSetupID)
                                                                      .Select(g => g.First())
                                                                      .ToList();

                        if (ListOfExamSetup != null && ListOfExamSetup.Count > 0)
                        {
                            foreach (var tData in ListOfExamSetup)
                            {
                                distinctExamsetupList.Add(tData);
                            }
                        }
                    }
                    #endregion

                    if (distinctExamsetupList != null && distinctExamsetupList.Count > 0)
                    {

                        foreach (var tData in distinctExamsetupList)
                        {

                            ExamSetupWithExamCommittees model = ExamSetupWithExamCommitteesManager.GetByExamSetupId(tData.ExamSetupID);
                            if (model != null)
                            {
                                //// Dublicate Check
                                //// If there is a data with same ExanSetupID
                                //// Do Nothing

                                //MessageView("Committee is Already Exist", "fail", ExamCommitteeMessage);
                                //MessageView("Committee is Already Exist", "fail", GeneralMessage);

                                //return;

                                int chairmanDeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
                                int memberOneDeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
                                int memberTwoDeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
                                int externalMemberDeptId = Convert.ToInt32(DepartmentUserControl4.selectedValue);

                                if (chairmanDeptId > 0)
                                {
                                    model.ExamCommitteeChairmanDeptId = chairmanDeptId;
                                }
                                if (memberOneDeptId > 0)
                                {
                                    model.ExamCommitteeMemberOneDeptId = memberOneDeptId;
                                }
                                if (memberTwoDeptId > 0)
                                {
                                    model.ExamCommitteeMemberTwoDeptId = memberTwoDeptId;
                                }
                                if (externalMemberDeptId > 0)
                                {
                                    model.ExamCommitteeExternalMemberDeptId = externalMemberDeptId;
                                }

                                int chairmanId = Convert.ToInt32(ddlExamCommitteeChairman.SelectedValue);
                                int memberOneId = Convert.ToInt32(ddlExamCommitteeMemberOne.SelectedValue);
                                int memberTwoId = Convert.ToInt32(ddlExamCommitteeMemberTwo.SelectedValue);
                                int externalMemberId = Convert.ToInt32(ddlExamCommitteeExternalMember.SelectedValue);

                                if (chairmanId > 0)
                                {
                                    model.ExamCommitteeChairmanId = chairmanId;
                                }
                                if (memberOneId > 0)
                                {
                                    model.ExamCommitteeMemberOneId = memberOneId;
                                }
                                if (memberTwoId > 0)
                                {
                                    model.ExamCommitteeMemberTwoId = memberTwoId;
                                }
                                if (externalMemberId > 0)
                                {
                                    model.ExamCommitteeExternalMemberId = externalMemberId;
                                }
                                model.ModifiedBy = user.User_ID;
                                model.ModifiedDate = DateTime.Now;

                                bool resultExamSetupWithExamCommittees = ExamSetupWithExamCommitteesManager.Update(model);

                                if (resultExamSetupWithExamCommittees == true)
                                {
                                    insertOrUpdateCount++;
                                }

                            }
                            else
                            {
                                int chairmanDeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
                                int memberOneDeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
                                int memberTwoDeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
                                int externalMemberDeptId = Convert.ToInt32(DepartmentUserControl4.selectedValue);

                                int chairmanId = Convert.ToInt32(ddlExamCommitteeChairman.SelectedValue);
                                int memberOneId = Convert.ToInt32(ddlExamCommitteeMemberOne.SelectedValue);
                                int memberTwoId = Convert.ToInt32(ddlExamCommitteeMemberTwo.SelectedValue);
                                int externalMemberId = Convert.ToInt32(ddlExamCommitteeExternalMember.SelectedValue);

                                //if (chairmanDeptId > 0 && memberOneDeptId > 0 && memberTwoDeptId > 0 && externalMemberDeptId > 0 &&
                                //    chairmanId > 0 && memberOneId > 0 && memberTwoId > 0 && externalMemberId > 0)
                                if (chairmanDeptId > 0 && memberOneDeptId > 0 && memberTwoDeptId > 0 &&
                                    chairmanId > 0 && memberOneId > 0 && memberTwoId > 0)
                                {
                                    ExamSetupWithExamCommittees obj = new ExamSetupWithExamCommittees();
                                    obj.HeldInProgramRelationId = tData.ExamSetupID;
                                    obj.ExamCommitteeChairmanDeptId = chairmanDeptId;
                                    obj.ExamCommitteeChairmanId = chairmanId;
                                    obj.ExamCommitteeMemberOneDeptId = memberOneDeptId;
                                    obj.ExamCommitteeMemberOneId = memberOneId;
                                    obj.ExamCommitteeMemberTwoDeptId = memberTwoDeptId;
                                    obj.ExamCommitteeMemberTwoId = memberTwoId;
                                    obj.ExamCommitteeExternalMemberDeptId = externalMemberDeptId;
                                    obj.ExamCommitteeExternalMemberId = externalMemberId;
                                    obj.CreatedBy = user.User_ID;
                                    obj.CreatedDate = DateTime.Now;

                                    int rExamSetupWithExamCommitteesId = ExamSetupWithExamCommitteesManager.Insert(obj);

                                    if (rExamSetupWithExamCommitteesId > 0)
                                    {
                                        insertOrUpdateCount++;
                                    }
                                }
                                else
                                {
                                    MessageView("Please Select Department and Chairmen, Member One and Member Two for Insert !!", "fail", ExamCommitteeMessage);
                                }
                            }

                        } // END: Loop


                        if (insertOrUpdateCount > 0)
                        {
                            ClearField();

                            LoadGridView();

                            this.ModalPopupExtenderExamCommittee.Hide();

                            MessageView("Data Update Successfully", "success", ExamCommitteeMessage);
                            MessageView("Data Update Successfully", "success", GeneralMessage);
                        }
                        else
                        {
                            MessageView("Failed to Update Data", "fail", ExamCommitteeMessage);
                            MessageView("Failed to Update Data", "fail", GeneralMessage);
                        }


                    }
                    else
                    {
                        MessageView("No Exam Setup is Found", "fail", ExamCommitteeMessage);
                    }
                    #endregion
                }



            }
            catch (Exception ex)
            {
                MessageView("Exception: Something Went Wrong Exam Committee Save/Update; Error: " + ex.Message.ToString(), "fail", ExamCommitteeMessage);
            }
        }

        protected void btnEditCommittee_Click(object sender, EventArgs e)
        {
            MessageView("", "clear", GeneralMessage);
            MessageView("", "clear", ExamCommitteeMessage);


            try
            {
                btnSaveExamCommittee.Text = "Update";
                hfExamSetupWithExamCommitteesId.Value = "0";


                LinkButton btn = (LinkButton)sender;

                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });

                int examSetupId = int.Parse(commandArgs[0]);
                int examSetupDetailId = int.Parse(commandArgs[1]);
                int semesterNo = int.Parse(commandArgs[2]);
                int examSetupWithExamCommitteesId = int.Parse(commandArgs[3]);

                if (examSetupId > 0 && examSetupDetailId > 0 && semesterNo > 0 && examSetupWithExamCommitteesId > 0)
                {
                    ExamSetupWithExamCommittees model = ExamSetupWithExamCommitteesManager.GetById(examSetupWithExamCommitteesId);

                    if (model != null)
                    {
                        this.ModalPopupExtenderExamCommittee.Show();

                        hfExamSetupWithExamCommitteesId.Value = model.ID.ToString();

                        int chairmanDeptIdT = model.ExamCommitteeChairmanDeptId > 0 ? Convert.ToInt32(model.ExamCommitteeChairmanDeptId) : 0;
                        int memberOneDeptIdT = model.ExamCommitteeMemberOneDeptId > 0 ? Convert.ToInt32(model.ExamCommitteeMemberOneDeptId) : 0;
                        int memberTwoDeptIdT = model.ExamCommitteeMemberTwoDeptId > 0 ? Convert.ToInt32(model.ExamCommitteeMemberTwoDeptId) : 0;
                        int externalMemberDeptIdT = model.ExamCommitteeExternalMemberDeptId > 0 ? Convert.ToInt32(model.ExamCommitteeExternalMemberDeptId) : 0;

                        DepartmentUserControl1.SelectedValue(chairmanDeptIdT);
                        DepartmentUserControl2.SelectedValue(memberOneDeptIdT);
                        DepartmentUserControl3.SelectedValue(memberTwoDeptIdT);
                        DepartmentUserControl4.SelectedValue(externalMemberDeptIdT);

                        DepartmentUserControl1_DepartmentSelectedIndexChanged(null, null);
                        ddlExamCommitteeChairman.SelectedValue = model.ExamCommitteeChairmanId > 0 ? model.ExamCommitteeChairmanId.ToString() : "0";

                        DepartmentUserControl2_DepartmentSelectedIndexChanged(null, null);
                        ddlExamCommitteeMemberOne.SelectedValue = model.ExamCommitteeMemberOneId > 0 ? model.ExamCommitteeMemberOneId.ToString() : "0";

                        DepartmentUserControl3_DepartmentSelectedIndexChanged(null, null);
                        ddlExamCommitteeMemberTwo.SelectedValue = model.ExamCommitteeMemberTwoId > 0 ? model.ExamCommitteeMemberTwoId.ToString() : "0";

                        DepartmentUserControl4_DepartmentSelectedIndexChanged(null, null);
                        ddlExamCommitteeExternalMember.SelectedValue = model.ExamCommitteeExternalMemberId > 0 ? model.ExamCommitteeExternalMemberId.ToString() : "0";
                    }
                    else
                    {
                        MessageView("No Exam Committees has been Found !!", "fail", GeneralMessage);
                    }
                }
                else
                {
                    MessageView("No Exam Committees has been Created Yet !!", "fail", GeneralMessage);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion




        protected void DepartmentUserControl1_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtenderExamCommittee.Show();

            int DeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
            LoadFacultyOneByDeptId(DeptId);
        }

        private void LoadFacultyOneByDeptId(int DeptId)
        {
            #region N/A
            //ddlFaculty1.Items.Clear();
            //ddlFaculty1.Items.Add(new ListItem("-Select-", "0"));

            //List<Employee> employeeList = EmployeeManager.GetAll();
            //if (DeptId != 0)
            //    employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            //if (employeeList.Count > 0)
            //{
            //    employeeList = employeeList.OrderBy(o => o.Code).ToList();

            //    foreach (Employee employee in employeeList)
            //    {
            //        ddlFaculty1.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            //    }
            //} 
            #endregion


            #region Load Chairman
            List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

            if (DeptId != 0)
            {
                empList = empList.Where(x => x.DeptID == DeptId).ToList();
            }

            ddlExamCommitteeChairman.Items.Clear();
            ddlExamCommitteeChairman.Items.Add(new ListItem("-Select Chairman-", "0"));
            ddlExamCommitteeChairman.AppendDataBoundItems = true;

            if (empList != null && empList.Count > 0)
            {
                foreach (Employee employee in empList)
                {
                    ddlExamCommitteeChairman.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
            #endregion


        }



        protected void DepartmentUserControl2_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtenderExamCommittee.Show();
            int DeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
            LoadFacultyTwoByDeptId(DeptId);
        }

        private void LoadFacultyTwoByDeptId(int DeptId)
        {
            #region N/A
            //ddlFaculty2.Items.Clear();
            //ddlFaculty2.Items.Add(new ListItem("-Select-", "0"));

            //List<Employee> employeeList = EmployeeManager.GetAll();
            //if (DeptId != 0)
            //    employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            //if (employeeList.Count > 0)
            //{
            //    employeeList = employeeList.OrderBy(o => o.Code).ToList();

            //    foreach (Employee employee in employeeList)
            //    {
            //        ddlFaculty2.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            //    }
            //} 
            #endregion


            #region Load Member One
            List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

            if (DeptId != 0)
            {
                empList = empList.Where(x => x.DeptID == DeptId).ToList();
            }

            ddlExamCommitteeMemberOne.Items.Clear();
            ddlExamCommitteeMemberOne.Items.Add(new ListItem("-Select Member One-", "0"));
            ddlExamCommitteeMemberOne.AppendDataBoundItems = true;

            if (empList != null && empList.Count > 0)
            {
                foreach (Employee employee in empList)
                {
                    ddlExamCommitteeMemberOne.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
            #endregion
        }



        protected void DepartmentUserControl3_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtenderExamCommittee.Show();

            int DeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
            LoadFacultyThreeByDeptId(DeptId);
        }

        private void LoadFacultyThreeByDeptId(int DeptId)
        {
            #region N/A
            //ddlFaculty3.Items.Clear();
            //ddlFaculty3.Items.Add(new ListItem("-Select-", "0"));

            //List<Employee> employeeList = EmployeeManager.GetAll();
            //if (DeptId != 0)
            //    employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            //if (employeeList.Count > 0)
            //{
            //    employeeList = employeeList.OrderBy(o => o.Code).ToList();

            //    foreach (Employee employee in employeeList)
            //    {
            //        ddlFaculty3.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            //    }
            //} 
            #endregion

            #region Load Member Two
            List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

            if (DeptId != 0)
            {
                empList = empList.Where(x => x.DeptID == DeptId).ToList();
            }

            ddlExamCommitteeMemberTwo.Items.Clear();
            ddlExamCommitteeMemberTwo.Items.Add(new ListItem("-Select Member Two-", "0"));
            ddlExamCommitteeMemberTwo.AppendDataBoundItems = true;

            if (empList != null && empList.Count > 0)
            {
                foreach (Employee employee in empList)
                {
                    ddlExamCommitteeMemberTwo.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
            #endregion


        }



        protected void DepartmentUserControl4_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtenderExamCommittee.Show();

            int DeptId = Convert.ToInt32(DepartmentUserControl4.selectedValue);
            LoadFacultyExternalByDeptId(DeptId);
        }

        private void LoadFacultyExternalByDeptId(int DeptId)
        {
            #region N/A
            //ddlFaculty3.Items.Clear();
            //ddlFaculty3.Items.Add(new ListItem("-Select-", "0"));

            //List<Employee> employeeList = EmployeeManager.GetAll();
            //if (DeptId != 0)
            //    employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
            //if (employeeList.Count > 0)
            //{
            //    employeeList = employeeList.OrderBy(o => o.Code).ToList();

            //    foreach (Employee employee in employeeList)
            //    {
            //        ddlFaculty3.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            //    }
            //} 
            #endregion

            #region Load Member Two
            List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

            empList = empList.Where(x => x.TeacherTypeId == 31).ToList(); // 31 = External Member

            if (DeptId != 0)
            {
                empList = empList.Where(x => x.DeptID == DeptId).ToList();
            }



            ddlExamCommitteeExternalMember.Items.Clear();
            ddlExamCommitteeExternalMember.Items.Add(new ListItem("-Select External Member-", "0"));
            ddlExamCommitteeExternalMember.AppendDataBoundItems = true;

            if (empList != null && empList.Count > 0)
            {
                foreach (Employee employee in empList)
                {
                    ddlExamCommitteeExternalMember.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
            #endregion


        }

    }
}