using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace EMS.Module.bill
{
    public partial class BillDelete : BasePage
    {
        private int _deleteBillOrPaymentButtonBit;

        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();

            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                LoadProgramDropdownList();
                LoadSessionDropDownList();
                Application["ButtonClickValue"] = null;
            }
        }

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
                var sessionList = AcademicCalenderManager.GetAll();

                sessionDropDownList.Items.Clear();
                sessionDropDownList.AppendDataBoundItems = true;

                if (sessionList != null && sessionList.Any())
                {
                    sessionDropDownList.DataTextField = "FullCode";
                    sessionDropDownList.DataValueField = "AcademicCalenderID";

                    sessionDropDownList.DataSource = sessionList.OrderByDescending(a => a.AcademicCalenderID);
                    sessionDropDownList.DataBind();

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

        //public void LoadBatchDropDownList()
        //{
        //    try
        //    {
        //        int programId = Convert.ToInt32(programDropDownList.SelectedValue);
        //        if (programId == -1)
        //        {
        //            lblMsg.Text = "Select program";
        //            return;
        //        }
        //        var batchList = BatchManager.GetAll();

        //        batchDropDownList.Items.Clear();
        //        batchDropDownList.Items.Add(new ListItem("Select", "-1"));
        //        batchDropDownList.AppendDataBoundItems = true;

        //        if (batchList != null && batchList.Any())
        //        {
        //            batchList = batchList.Where(b => b.ProgramId == programId).ToList();
        //            batchDropDownList.DataTextField = "BatchName";
        //            batchDropDownList.DataValueField = "BatchId";
        //            batchDropDownList.DataSource = batchList.OrderByDescending(b => b.BatchNO).ToList(); ;
        //            batchDropDownList.DataBind();

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        protected void programDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAdmissionSessionDropDownList();
                LoadSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Application["ButtonClickValue"] = null;
                lblMsg.Text = string.Empty;
                LoadPayment();
                Application["ButtonClickValue"] = 1;
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadPayment()
        {
            try
            {
                infoLabel.Text = "List of non paid students.";
                List<BillDeleteDTO> billPrintList = new List<BillDeleteDTO>();
                string studentRoll = Convert.ToString(txtStudentRoll.Text);
                Student studentObj = null;
                int programId = Convert.ToInt32(programDropDownList.SelectedValue);
                int? admissionSessionId;
                if (!string.IsNullOrEmpty(studentRoll))
                {
                    studentObj = StudentManager.GetByRoll(studentRoll);
                    if (studentObj != null)
                    {
                        programId = studentObj.ProgramID;
                        programDropDownList.SelectedValue = programId.ToString();
                        admissionSessionId = studentObj.StudentAdmissionAcaCalId;
                        LoadAdmissionSessionDropDownList();
                        admissionSessionDropDownList.SelectedValue = admissionSessionId.ToString();
                    }
                }
                if (programId == -1)
                {
                    lblMsg.Text = "Select program";
                    return;
                }
                if (string.IsNullOrEmpty(sessionDropDownList.SelectedValue))
                {
                    lblMsg.Text = "Select session";
                    LoadSessionDropDownList();
                    return;
                }
                int sessionId = Convert.ToInt32(sessionDropDownList.SelectedValue);
                if (sessionId == -1)
                {
                    lblMsg.Text = "Select session";
                    return;
                }
                admissionSessionId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
                if (admissionSessionId == -1)
                {
                    admissionSessionId = null;
                    //lblMsg.Text = "Select batch";
                    //return;
                }
                billPrintList = BillHistoryMasterManager.GetStudentsForBillPrintByProgramIdSessionIdStudentAdmissionSessionId(programId, sessionId, admissionSessionId);
                if (studentObj != null)
                {
                    billPrintList = billPrintList.Where(m => m.StudentId == studentObj.StudentID).ToList();
                }

                billPrintList = billPrintList.OrderBy(m => m.Roll).ToList();
                deleteBillGridView.DataSource = billPrintList;
                deleteBillGridView.DataBind();

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void chkAllStudent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkHeader = (CheckBox)deleteBillGridView.HeaderRow.FindControl("chkAllStudentHeader");
                if (chkHeader.Checked)
                {
                    for (int i = 0; i < deleteBillGridView.Rows.Count; i++)
                    {
                        GridViewRow row = deleteBillGridView.Rows[i];
                        Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = true;
                    }
                }
                if (!chkHeader.Checked)
                {
                    for (int i = 0; i < deleteBillGridView.Rows.Count; i++)
                    {
                        GridViewRow row = deleteBillGridView.Rows[i];
                        Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void deleteButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                int buttonClickFlagValue = (int) Application["ButtonClickValue"];
                lblMsg.Text = String.Empty;
                var isDeleted = false;
                for (int i = 0; i < deleteBillGridView.Rows.Count; i++)
                {
                    GridViewRow row = deleteBillGridView.Rows[i];
                    Label lblBillHistoryMasterId = (Label)row.FindControl("lblBillHistoryMasterId");
                    int billHistoryMasterId = Convert.ToInt32(lblBillHistoryMasterId.Text);

                    var billHistoryMasterObj = BillHistoryMasterManager.GetById(billHistoryMasterId);
                    var student = StudentManager.GetById(billHistoryMasterObj.StudentId);

                    CheckBox studentChecked = (CheckBox)row.FindControl("CheckBox");
                    if (studentChecked.Checked)
                    {
                        isDeleted = BillHistoryMasterManager.Delete(billHistoryMasterId, buttonClickFlagValue);
                        #region Log Insert

                        string logMessage = "Bill";

                        if (buttonClickFlagValue == 2)
                        {
                            logMessage = "Payment";
                        }

                        LogGeneralManager.Insert(
                            DateTime.Now,
                            "",
                            "",
                            userObj.LogInID,
                            "",
                            "",
                            logMessage + " Delete",
                            userObj.LogInID + " has deleted " + logMessage + " for studentId= "  +billHistoryMasterObj.StudentId+", "
                            +"Academic CalendarId= "+billHistoryMasterObj.AcaCalId + ", BillHistoryMasterId= " + billHistoryMasterId + ", "
                            + "Amount= "+billHistoryMasterObj.Amount,
                            userObj.LogInID + " Done " + logMessage + " deleting.",
                            ((int)CommonEnum.PageName.BillDelete).ToString(),
                            CommonEnum.PageName.BillDelete.ToString(),
                            _pageUrl,
                            student.Roll);

                        #endregion

                    }
                }

                if (isDeleted && buttonClickFlagValue == 1)
                {
                    lblMsg.Text = "Deleted successfully.";
                    LoadPayment();
                }
                if (isDeleted && buttonClickFlagValue == 2)
                {
                    lblMsg.Text = "Deleted successfully.";
                    LoadPaidStudentList();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Application["ButtonClickValue"] = null;
                lblMsg.Text = String.Empty;
                LoadPaidStudentList();
                Application["ButtonClickValue"] = 2;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void LoadPaidStudentList()
        {
            infoLabel.Text = String.Empty;
            string studentRoll = Convert.ToString(txtStudentRoll.Text);
            Student studentObj = null;
            int programId = Convert.ToInt32(programDropDownList.SelectedValue);
            int? admissionSessionId;
            if (!string.IsNullOrEmpty(studentRoll))
            {
                studentObj = StudentManager.GetByRoll(studentRoll);
                if (studentObj != null)
                {
                    programId = studentObj.ProgramID;
                    programDropDownList.SelectedValue = programId.ToString();
                    admissionSessionId = studentObj.BatchId;
                    LoadAdmissionSessionDropDownList();
                    admissionSessionDropDownList.SelectedValue = admissionSessionId.ToString();
                }
            }
            if (programId == -1)
            {
                lblMsg.Text = "Select program";
                return;
            }
            if (string.IsNullOrEmpty(sessionDropDownList.SelectedValue))
            {
                lblMsg.Text = "Select session";
                LoadSessionDropDownList();
                return;
            }
            int sessionId = Convert.ToInt32(sessionDropDownList.SelectedValue);
            if (sessionId == -1)
            {
                lblMsg.Text = "Select session";
                return;
            }
            admissionSessionId = Convert.ToInt32(admissionSessionDropDownList.SelectedValue);
            if (admissionSessionId == -1)
            {
                admissionSessionId = null;
                //lblMsg.Text = "Select batch";
                //return;
            }
            var billPrintList = CollectionHistoryManager.GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId(programId, sessionId, admissionSessionId);
            if (studentObj != null)
            {
                billPrintList = billPrintList.Where(m => m.StudentId == studentObj.StudentID).ToList();
            }

            infoLabel.Text = "List of paid students.";
            billPrintList = billPrintList.OrderBy(m => m.Roll).ToList();
            deleteBillGridView.DataSource = billPrintList;
            deleteBillGridView.DataBind();
        }
    }
}