using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace EMS.Module.bill
{
    public partial class BillGenerationV2 : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        int userId = 0;
        User user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
            user = UserManager.GetByLogInId(loginId);
            if (user != null)
                userId = user.User_ID;
            if (!IsPostBack)
            {
                LoadProgramDropdownList();
            }
        }

        #region Load Method

        public void LoadProgramDropdownList()
        {
            try
            {
                var programList = ProgramManager.GetAll();
                programDropDownList.Items.Add(new ListItem("Select", "-1"));
                programDropDownList.AppendDataBoundItems = true;
                if (programList.Any())
                {
                    programDropDownList.DataSource = programList;
                    programDropDownList.DataTextField = "ShortName";
                    programDropDownList.DataValueField = "ProgramID";
                    programDropDownList.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadSessionDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMessage.Text = "Select program";
                    return;
                }
                Program program = ProgramManager.GetById(programId);
                //AcademicCalender acacal = AcademicCalenderManager.GetIsCurrentAcademicCalenderByProgramId(programId);

                List<AcademicCalender> sessionList = new List<AcademicCalender>();
                if (program != null)
                    sessionList = AcademicCalenderManager.GetAll();

                sessionDropDownList.Items.Clear();
                sessionDropDownList.AppendDataBoundItems = true;

                if (sessionList != null && sessionList.Any())
                {
                    //sessionList = sessionList.Where(b => b.ProgramId == programId).ToList();

                    sessionDropDownList.Items.Add(new ListItem("Select", "-1"));
                    sessionDropDownList.DataTextField = "FullCode";
                    sessionDropDownList.DataValueField = "AcademicCalenderID";

                    sessionDropDownList.DataSource = sessionList.OrderByDescending(a => a.AcademicCalenderID);
                    sessionDropDownList.DataBind();

                    //if (acacal != null)
                    //{
                    //    sessionDropDownList.SelectedValue = acacal.AcademicCalenderID.ToString();
                    //    sessionDropDownList.SelectedValue = acacal.AcademicCalenderID.ToString();
                    //    //sessionDropDownList.SelectedText = acacal.FullCode;
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadAdmissionSessionDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                //var academicCalenderList = AcademicCalenderManager.GetAll().Where(x => x.CalenderUnitTypeID == 1).OrderByDescending(m => m.AcademicCalenderID).ToList();
                var academicCalenderList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();
                admissionSessionDropDownList.Items.Clear();
                admissionSessionDropDownList.Items.Add(new ListItem("All", "-1"));
                admissionSessionDropDownList.AppendDataBoundItems = true;
                if (programId == -1)
                {
                    lblMessage.Text = "select program.";
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

        private void LoadStudentListGridView()
        {
            try
            {
                lblMessage.Text = string.Empty;
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                if (programId == -1)
                {
                    lblMessage.Text = "Select program";
                    return;
                }
                int admissionSessionId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionSessionId == -1)
                {
                    admissionSessionId = 0;
                }
                List<Student> studentList = StudentManager.GetAllByProgramIdSessionId(Convert.ToInt32(programId), admissionSessionId);

                //gvStudentList.DataSource = studentList;
                //gvStudentList.DataBind();

                //if (studentList != null && studentList.Count > 0)
                //{
                //    SessionManager.SaveListToSession<Student>(studentList, _StudentList);
                //}

                //var studentList = StudentManager.GetAllRegisteredStudentByProgramSessionBatchId(programId, sessionId, null);

                if (studentList != null && studentList.Any())
                {
                    totalStudentLabel.Visible = true;
                    lblCount.Text = studentList.Count().ToString();
                    StudentListGridView.DataSource = studentList.OrderBy(s => s.Roll).ToList();
                    StudentListGridView.DataBind();
                }
                else
                {
                    totalStudentLabel.Visible = false;
                    lblCount.Visible = false;
                    lblCount.Text = String.Empty;
                    StudentListGridView.DataSource = null;
                    StudentListGridView.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void LoadFeeGroupDropDownList()
        {
            try
            {
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                int studentAdmissionAcaCalId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                var feeGroupMasterList = FeeGroupMasterManager.GetAll();
                var feeGroupMasterList1 = feeGroupMasterList.Where(x => x.ProgramId == null && x.StudentAdmissionAcaCalId == null).ToList();
                var feeGroupMasterList2 = feeGroupMasterList.Where(x => x.ProgramId == programId && x.StudentAdmissionAcaCalId == null).ToList();
                List<FeeGroupMaster> finalFeeGroupMasterList = new List<FeeGroupMaster>();
                if (feeGroupMasterList1.Any())
                {
                    finalFeeGroupMasterList = feeGroupMasterList1;
                }
                if (feeGroupMasterList2.Any())
                {
                    if (finalFeeGroupMasterList.Any())
                        finalFeeGroupMasterList = finalFeeGroupMasterList.Concat(feeGroupMasterList2).ToList();
                    else
                        finalFeeGroupMasterList = feeGroupMasterList2;
                }

                if (studentAdmissionAcaCalId != -1)
                {
                    var feeGroupMasterList3 = feeGroupMasterList
                        .Where(x => x.ProgramId == programId && x.StudentAdmissionAcaCalId == studentAdmissionAcaCalId).ToList();

                    if (feeGroupMasterList3.Any())
                    {
                        finalFeeGroupMasterList = finalFeeGroupMasterList.Any()
                            ? finalFeeGroupMasterList.Concat(feeGroupMasterList3).ToList()
                            : feeGroupMasterList3;
                    }
                }

                foreach (FeeGroupMaster feeGroupMaster in finalFeeGroupMasterList)
                {
                    if (feeGroupMaster.StudentAdmissionAcaCalId == null && feeGroupMaster.ProgramId == null)
                    {
                        feeGroupMaster.FeeGroupName = feeGroupMaster.FeeGroupName + "_" + "All" + "_" + "All";
                    }

                    if (feeGroupMaster.ProgramId != null && feeGroupMaster.StudentAdmissionAcaCalId == null)
                    {
                        feeGroupMaster.FeeGroupName = feeGroupMaster.FeeGroupName + "_" + feeGroupMaster.Program.Code + "_" + "All";
                    }
                    if (feeGroupMaster.ProgramId != null && feeGroupMaster.StudentAdmissionAcaCalId != null)
                    {
                        feeGroupMaster.FeeGroupName = feeGroupMaster.FeeGroupName + "_" + feeGroupMaster.Program.Code + "_" + feeGroupMaster.Session.Code;
                    }
                }
                feeGroupDropDownList.Items.Clear();
                feeGroupDropDownList.Items.Add(new ListItem("Select", "-1"));
                feeGroupDropDownList.AppendDataBoundItems = true;
                if (finalFeeGroupMasterList.Any())
                {
                    feeGroupDropDownList.DataSource = finalFeeGroupMasterList;
                    feeGroupDropDownList.DataTextField = "FeeGroupName";
                    feeGroupDropDownList.DataValueField = "FeeGroupMasterId";
                    feeGroupDropDownList.DataBind();
                }

            }
            catch (Exception e)
            {
                lblMessage.Text = "Error Occured";
            }
        }

        #endregion

        #region Button click

        protected void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoadStudentListGridView();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void createBillButton_OnClick(object sender, EventArgs e)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    lblMessage.Text = String.Empty;
                    int sessionId = Convert.ToInt32(sessionDropDownList.SelectedValue);
                    if (sessionId == -1)
                    {
                        lblMessage.Text = "Select billing session";
                        return;
                    }

                    int feeGroupMasterId = Convert.ToInt32(feeGroupDropDownList.SelectedValue);
                    if (feeGroupMasterId == -1)
                    {
                        lblMessage.Text = "Select fee group.";
                        return;
                    }

                    foreach (GridViewRow row in StudentListGridView.Rows)
                    {

                        CheckBox chkBoxRows = (CheckBox)row.FindControl("itemCheckBox");
                        if (chkBoxRows.Checked)
                        {
                            HiddenField studentIdHiddenField = (HiddenField)row.FindControl("studentIdHiddenField");
                            //HiddenField programIdHiddenField = (HiddenField)row.FindControl("programIdHiddenField");
                            //HiddenField studentAdmissionAcaCalIdHiddenField = (HiddenField)row.FindControl("studentAdmissionAcaCalIdHiddenField");
                            int studentId = Convert.ToInt32(studentIdHiddenField.Value);
                            Student student = StudentManager.GetById(studentId);
                            //AcademicCalender academicCalender = AcademicCalenderManager.GetById(sessionId);
                            InsertBillMaster(student, sessionId, feeGroupMasterId);
                        }
                        else
                        {
                            chkBoxRows.Checked = false;
                        }
                    }
                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    scope.Complete();
                }

            }
        }

        #endregion

        #region Selected Index Change Event

        protected void programDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAdmissionSessionDropDownList();
                LoadFeeGroupDropDownList();
                LoadSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void sessionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //LoadAdmissionSessionDropDownList();
                LoadStudentListGridView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void selectAllCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox selectAllCheckBox = (CheckBox)StudentListGridView.HeaderRow.FindControl("selectAllCheckBox");
                foreach (GridViewRow row in StudentListGridView.Rows)
                {
                    CheckBox chkBoxRows = (CheckBox)row.FindControl("itemCheckBox");
                    if (selectAllCheckBox.Checked)
                    {
                        chkBoxRows.Checked = true;
                        row.BackColor = System.Drawing.Color.BlueViolet;
                        row.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        chkBoxRows.Checked = false;
                        row.BackColor = System.Drawing.Color.Empty;
                        row.ForeColor = System.Drawing.Color.Empty;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void itemCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (cb.Checked)
                {
                    row.BackColor = System.Drawing.Color.BlueViolet;
                    row.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    row.BackColor = System.Drawing.Color.Empty;
                    row.ForeColor = System.Drawing.Color.Empty;
                }
            }
            catch (Exception exception)
            {
                lblMessage.Text = exception.Message;
            }
        }

        //protected void batchDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LoadStudentListGridView();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        #region Insert Method

        private void InsertBillMaster(Student student, int billingSessionId, int feeGroupMasterId)
        {
            try
            {
                List<StudentBillDetailsDTO> studentBillDetailList = BillHistoryManager.GetStudentBillingDetailsByGroup(student.StudentID, student.ProgramID, student.StudentAdmissionAcaCalId, billingSessionId, feeGroupMasterId); //acaCal.AcademicCalenderID

                //if (student.StudentAdmissionAcaCalId == billingSessionId)
                //{
                //    List<StudentBillDetailsDTO> studentAnnualBillDetailList = BillHistoryManager.GenerateStudentAnnualBillingDetails(student.StudentID, student.ProgramID, student.StudentAdmissionAcaCalId, billingSessionId);

                //    if (studentAnnualBillDetailList != null && studentAnnualBillDetailList.Any())
                //    {
                //        studentBillDetailList = studentBillDetailList.Concat(studentAnnualBillDetailList).ToList();
                //    }
                //}

                decimal billMasterAmount = studentBillDetailList.Sum(totalAmount => totalAmount.Amount);
                if (billMasterAmount > 0)
                {
                    BillHistoryMaster billHistoryMaster = new BillHistoryMaster();
                    billHistoryMaster.StudentId = student.StudentID;
                    billHistoryMaster.Amount = billMasterAmount;
                    billHistoryMaster.BillingDate = DateTime.Now;
                    billHistoryMaster.ReferenceNo = Convert.ToString(BillHistoryMasterManager.GetBillMasterMaxReferenceNo(billHistoryMaster.BillingDate));

                    billHistoryMaster.IsDeleted = false;
                    billHistoryMaster.IsDue = false;
                    billHistoryMaster.ParentBillHistroyMasterId = 0;
                    billHistoryMaster.AcaCalId = billingSessionId;

                    billHistoryMaster.CreatedBy = user.User_ID;
                    billHistoryMaster.CreatedDate = DateTime.Now;
                    billHistoryMaster.ModifiedBy = user.User_ID;
                    billHistoryMaster.ModifiedDate = DateTime.Now;

                    int billHistoryMasterId = BillHistoryMasterManager.Insert(billHistoryMaster);
                    if (billHistoryMasterId > 0)
                    {
                        //#region Log Insert
                        //LogGeneralManager.Insert(
                        //    DateTime.Now,
                        //    BaseAcaCalCurrent.Code,
                        //    BaseAcaCalCurrent.FullCode,
                        //    BaseCurrentUserObj.LogInID,
                        //    "",
                        //    "",
                        //    "Bill History Master Entry",
                        //    BaseCurrentUserObj.LogInID + " posted bill history Master for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + billHistoryMaster.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                        //    "normal",
                        //    ((int)CommonEnum.PageName.BillGeneration).ToString(),
                        //    CommonEnum.PageName.BillGeneration.ToString(),
                        //    _pageUrl,
                        //    student.Roll);
                        //#endregion
                        InsertBillHistory(student, billHistoryMasterId, billingSessionId, studentBillDetailList);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void InsertBillHistory(Student student, int billHistoryMasterId, int sessionId, List<StudentBillDetailsDTO> studentBillDeatilList)
        {
            try
            {
                int noOfFees = studentBillDeatilList.Count;
                int billInsertCounter = 0;

                foreach (StudentBillDetailsDTO studentBillDetails in studentBillDeatilList)
                {
                    decimal feeAmount = studentBillDetails.Amount;
                    if (feeAmount > 0)
                    {
                        BillHistory billHistroyObj = new BillHistory();
                        billHistroyObj.StudentId = studentBillDetails.StudentID;
                        billHistroyObj.StudentCourseHistoryId = studentBillDetails.StudentCourseHistoryId;
                        billHistroyObj.FeeSetupId = studentBillDetails.FeeSetupId;
                        billHistroyObj.TypeDefinitionId = studentBillDetails.TypeDefinitionID;
                        billHistroyObj.AcaCalId = sessionId;
                        billHistroyObj.Fees = feeAmount;
                        billHistroyObj.BillingDate = DateTime.Now;
                        billHistroyObj.IsDeleted = false;
                        billHistroyObj.BillHistoryMasterId = Convert.ToInt32(billHistoryMasterId);
                        billHistroyObj.CreatedBy = user.User_ID;
                        billHistroyObj.CreatedDate = DateTime.Now;
                        billHistroyObj.ModifiedBy = user.User_ID;
                        billHistroyObj.ModifiedDate = DateTime.Now;

                        int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                        if (billInsertId > 0)
                        {
                            lblMessage.Text = "Bill inserted successfully.";
                            billInsertCounter = billInsertCounter + 1;
                            if (feeAmount > 0)
                            {
                                //#region Log Insert
                                //LogGeneralManager.Insert(
                                //     DateTime.Now,
                                //     BaseAcaCalCurrent.Code,
                                //     BaseAcaCalCurrent.FullCode,
                                //     BaseCurrentUserObj.LogInID,
                                //     "",
                                //     "",
                                //     "Bill History Entry",
                                //     BaseCurrentUserObj.LogInID + " posted bill for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + feeAmount.ToString() + ", Session : " + sessionDropDownList.SelectedItem.Text,
                                //     "normal",
                                //     ((int)CommonEnum.PageName.BillGeneration).ToString(),
                                //     CommonEnum.PageName.BillGeneration.ToString(),
                                //     _pageUrl,
                                //     student.Roll);
                                //#endregion
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Bill could not inserted successfully.";

                            #region Log Insert
                            LogGeneralManager.Insert(
                                 DateTime.Now,
                                 BaseAcaCalCurrent.Code,
                                 BaseAcaCalCurrent.FullCode,
                                 BaseCurrentUserObj.LogInID,
                                 "",
                                 "",
                                 "Unsuccessful Bill History Entry",
                                 BaseCurrentUserObj.LogInID + " unsuccessful posted bill for Roll : " + student.Roll + ", Fee Type : " + studentBillDetails.TypeDefinitionID + ", Amount: " + studentBillDetails.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                                 "normal",
                                 ((int)CommonEnum.PageName.BillPosting).ToString(),
                                 CommonEnum.PageName.BillPosting.ToString(),
                                 _pageUrl,
                                 student.Roll);
                            #endregion
                        }
                    }

                }
                //if (noOfFees == billInsertCounter)
                //{
                //    lblMessage.Text = "Bill inserted successfully.";
                //}
                //else
                //{
                //    lblMessage.Text = "Bill could not inserted successfully.";
                //}

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Insert Short / Supplementary Bill

        private void InsertShort_SupplementaryBillMaster(Student student, int sessionId, int calenderUnitTypeId)
        {
            try
            {
                List<StudentBillDetailsDTO> studentBillDetailList = BillHistoryManager.GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(student.StudentID, sessionId, calenderUnitTypeId); //acaCal.AcademicCalenderID

                decimal billMasterAmount = studentBillDetailList.Sum(totalAmount => totalAmount.Amount);
                if (billMasterAmount > 0)
                {
                    BillHistoryMaster billHistoryMaster = new BillHistoryMaster();
                    billHistoryMaster.StudentId = student.StudentID;
                    billHistoryMaster.Amount = billMasterAmount;
                    billHistoryMaster.BillingDate = DateTime.Now;
                    billHistoryMaster.ReferenceNo = Convert.ToString(BillHistoryMasterManager.GetBillMasterMaxReferenceNo(billHistoryMaster.BillingDate));

                    billHistoryMaster.IsDeleted = false;
                    billHistoryMaster.IsDue = false;
                    billHistoryMaster.ParentBillHistroyMasterId = 0;
                    billHistoryMaster.AcaCalId = sessionId;

                    billHistoryMaster.CreatedBy = user.User_ID;
                    billHistoryMaster.CreatedDate = DateTime.Now;
                    billHistoryMaster.ModifiedBy = user.User_ID;
                    billHistoryMaster.ModifiedDate = DateTime.Now;

                    int billHistoryMasterId = BillHistoryMasterManager.Insert(billHistoryMaster);
                    if (billHistoryMasterId > 0)
                    {
                        #region Log Insert Bill Master Short Supplementary
                        LogGeneralManager.Insert(
                            DateTime.Now,
                            BaseAcaCalCurrent.Code,
                            BaseAcaCalCurrent.FullCode,
                            BaseCurrentUserObj.LogInID,
                            "",
                            "",
                            "Bill History Master for Short Supplementary Entry",
                            BaseCurrentUserObj.LogInID + " posted Short Supplementary bill history Master for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + billHistoryMaster.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                            "normal",
                            ((int)CommonEnum.PageName.BillGeneration).ToString(),
                            CommonEnum.PageName.BillGeneration.ToString(),
                            _pageUrl,
                            student.Roll);
                        #endregion
                        InsertShort_SupplementaryBillHistory(student, billHistoryMasterId, sessionId, studentBillDetailList);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void InsertShort_SupplementaryBillHistory(Student student, int billHistoryMasterId, int sessionId, List<StudentBillDetailsDTO> studentBillDetailList)
        {
            try
            {
                int noOfFees = studentBillDetailList.Count;
                int billInsertCounter = 0;

                foreach (StudentBillDetailsDTO studentBillDetails in studentBillDetailList)
                {
                    decimal feeAmount = studentBillDetails.Amount;
                    if (feeAmount > 0)
                    {
                        BillHistory billHistroyObj = new BillHistory();
                        billHistroyObj.StudentId = studentBillDetails.StudentID;
                        billHistroyObj.StudentCourseHistoryId = studentBillDetails.StudentCourseHistoryId;
                        billHistroyObj.FeeSetupId = studentBillDetails.FeeSetupId;
                        billHistroyObj.TypeDefinitionId = studentBillDetails.TypeDefinitionID;
                        billHistroyObj.AcaCalId = sessionId;
                        billHistroyObj.Fees = feeAmount;
                        billHistroyObj.BillingDate = DateTime.Now;
                        billHistroyObj.IsDeleted = false;
                        billHistroyObj.BillHistoryMasterId = Convert.ToInt32(billHistoryMasterId);
                        billHistroyObj.CreatedBy = user.User_ID;
                        billHistroyObj.CreatedDate = DateTime.Now;
                        billHistroyObj.ModifiedBy = user.User_ID;
                        billHistroyObj.ModifiedDate = DateTime.Now;

                        int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                        if (billInsertId > 0)
                        {
                            lblMessage.Text = "Bill inserted successfully.";
                            billInsertCounter = billInsertCounter + 1;
                            if (feeAmount > 0)
                            {
                                #region Log Insert
                                LogGeneralManager.Insert(
                                     DateTime.Now,
                                     BaseAcaCalCurrent.Code,
                                     BaseAcaCalCurrent.FullCode,
                                     BaseCurrentUserObj.LogInID,
                                     "",
                                     "",
                                     "Bill History for Short Supplementary Entry",
                                     BaseCurrentUserObj.LogInID + " posted bill for Roll : " + student.Roll + ", Fee Type : " + lblMessage.Text + ", Amount: " + feeAmount.ToString() + ", Session : " + sessionDropDownList.SelectedItem.Text,
                                     "normal",
                                     ((int)CommonEnum.PageName.BillGeneration).ToString(),
                                     CommonEnum.PageName.BillGeneration.ToString(),
                                     _pageUrl,
                                     student.Roll);
                                #endregion
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Bill could not inserted successfully.";

                            #region Log Insert
                            LogGeneralManager.Insert(
                                 DateTime.Now,
                                 BaseAcaCalCurrent.Code,
                                 BaseAcaCalCurrent.FullCode,
                                 BaseCurrentUserObj.LogInID,
                                 "",
                                 "",
                                 "Unsuccessful Bill History for Short Supplementary Entry",
                                 BaseCurrentUserObj.LogInID + " unsuccessful posted bill for Roll : " + student.Roll + ", Fee Type : " + studentBillDetails.TypeDefinitionID + ", Amount: " + studentBillDetails.Amount + ", Session : " + sessionDropDownList.SelectedItem.Text,
                                 "normal",
                                 ((int)CommonEnum.PageName.BillPosting).ToString(),
                                 CommonEnum.PageName.BillPosting.ToString(),
                                 _pageUrl,
                                 student.Roll);
                            #endregion
                        }
                    }

                }
                //if (noOfFees == billInsertCounter)
                //{
                //    lblMessage.Text = "Bill inserted successfully.";
                //}
                //else
                //{
                //    lblMessage.Text = "Bill could not inserted successfully.";
                //}

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void admissionSessionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFeeGroupDropDownList();
            LoadStudentListGridView();
        }

        //protected void testButton_Click(object sender, EventArgs e)
        //{
        //    BillHistoryMasterManager.GenerateNonCollegiateBill(1, 1, 44, 3215, 1);
        //}
    }
}