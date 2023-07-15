using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;

namespace EMS.Module.bill
{
    public partial class BillEdit : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                LoadFeeType();
                Session["BillHistoryDetailList"] = null;
                //ucProgram.LoadDropdownWithUserAccess(userObj.Id);
            }
        }

        private void LoadFeeType()
        {
            try
            {
                var feeGroupDetailList = TypeDefinitionManager.GetAll().Where(x=>x.Type == "Fee").OrderBy(x=>x.Definition);
                ddlFeeItem.Items.Clear();
                ddlFeeItem.Items.Add(new ListItem("-Select Fee Item-", "0"));
                ddlFeeItem.AppendDataBoundItems = true;

                if (feeGroupDetailList.Any())
                {
                    ddlFeeItem.DataSource = feeGroupDetailList;
                    ddlFeeItem.DataTextField = "Definition";
                    ddlFeeItem.DataValueField = "TypeDefinitionID";
                    ddlFeeItem.DataBind();
                }
                else
                {
                    lblMsg.Text = "No fee item found.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnLoadStudentBill_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = String.Empty;

                if (String.IsNullOrEmpty( txtStudentRoll.Text))
                {
                    lblMsg.Text = "Provide student roll.";
                    Reset();
                    return;
                }

                if (String.IsNullOrEmpty(txtReferenceNo.Text))
                {
                    lblMsg.Text = "Provide Reference No.";
                    Reset();
                    return;
                }

                string studentRoll = Convert.ToString(txtStudentRoll.Text);
                Student studentObj = StudentManager.GetByRoll(studentRoll);
                if (studentObj != null)
                {
                    string referenceNo = Convert.ToString(txtReferenceNo.Text);
                    lblTxtStudentName.Text = studentObj.BasicInfo.FullName;
                    //batchLabel.Text = studentObj.Batch.BatchName;
                    LoadStudentBill(studentObj.StudentID, referenceNo);
                }
                else
                {
                    lblMsg.Text = "Student not found";
                }
            }
            catch(Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void AddFeeItem()
        {
            try
            {
                int typedefinitionId = Convert.ToInt32(ddlFeeItem.SelectedValue);
                string feeName = Convert.ToString(ddlFeeItem.SelectedItem);

                BillHistory billHistoryObj = new BillHistory();
                //billHistoryObj.FundTypeId = 0;
                billHistoryObj.StudentId = 0;
                billHistoryObj.TypeDefinitionId = typedefinitionId;
                //billHistoryObj.TypeName = feeName;
                billHistoryObj.Fees = 0;
                billHistoryObj.AcaCalId = 0;
                billHistoryObj.Remark = null;
                billHistoryObj.BillHistoryMasterId = 0;

                var billHistorySessionList = Session["BillHistoryDetailList"];
                var billHistoryList = billHistorySessionList as List<BillHistory>;
                billHistoryObj.Attribute1 = billHistoryObj.TypeName;
                if (billHistoryList != null && billHistoryList.Any())
                {
                    bool result = CheckListAndObject(billHistoryList, billHistoryObj);
                    if (result)
                    {
                        billHistoryList.Add(billHistoryObj);
                    }
                    else
                    {
                        lblMsg.Text = "Fee item already added.";
                    }
                    billHistoryList = billHistoryList.OrderBy(m => m.TypeName).ToList();
                    GvFeeAmount.DataSource = billHistoryList;
                    GvFeeAmount.DataBind();
                    Session["BillHistoryDetailList"] = null;
                    Session["BillHistoryDetailList"] = billHistoryList;
                }
                else
                {
                    List<BillHistory> billHistoryList2 = new List<BillHistory>();
                    billHistoryList2.Add(billHistoryObj);
                    GvFeeAmount.DataSource = billHistoryList2;
                    GvFeeAmount.DataBind();
                    Session["BillHistoryDetailList"] = null;
                    Session["BillHistoryDetailList"] = billHistoryList2;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private bool CheckListAndObject(List<BillHistory> billHistoryList, BillHistory billHistoryObj)
        {
            int counter = 0;
            for (int i = 0; i < billHistoryList.Count; i++)
            {
                if (billHistoryList[i].TypeDefinitionId == billHistoryObj.TypeDefinitionId)
                {
                    counter = 1;
                }
            }
            if (counter == 1)
            {
                return false;
            }
            return true;
        }

        private void LoadStudentBill(int studentId, string referenceNo)
        {
            try
            {
                List<BillHistory> billHistoryList = BillHistoryManager.GetBillHistoryByStudentIdReferenceNo(studentId, referenceNo);
                BillHistoryMaster billHistoryMaster = BillHistoryMasterManager.GetByReferenceId(studentId, referenceNo);
                if (billHistoryMaster == null)
                {
                    lblMsg.Text = "No bill has been found.";
                    Reset();
                    return;
                }
                var collectionHistoryList = CollectionHistoryManager.GetByBillHistoryMasterId(billHistoryMaster.BillHistoryMasterId);
                if (collectionHistoryList!=null && collectionHistoryList.Any())
                {
                    Reset();
                    PopupMessage("This bill has already been paid. You can not edit this bill.");
                    return;
                }
                foreach (BillHistory billHistory in billHistoryList)
                {
                    billHistory.Attribute1 = billHistory.TypeName;
                    if (billHistory.StudentCourseHistoryId > 0)
                    {
                        var studentCourse = StudentCourseHistoryManager.GetById(billHistory.StudentCourseHistoryId);
                        string courseDetail = studentCourse.Course.FormalCode + "_Cr." + studentCourse.Course.Credits;
                        billHistory.Attribute1 = billHistory.TypeName + " _" + courseDetail;
                    }
                }
                
                Session["BillHistoryMasterObj"] = null;
                Session["BillHistoryMasterObj"] = billHistoryMaster;

                Session["BillHistoryDetailList"] = null;
                Session["BillHistoryDetailList"] = billHistoryList;

                //added course credit and name with the type name
                
                //List<BillPaymentHistoryDTO> billPaymentHistoryList = BillHistoryManager.GetBillPaymentHistoryByStudentId(studentObj.StudentID);
                if (billHistoryList != null && billHistoryList.Any())
                {
                    billHistoryList = billHistoryList.OrderBy(m => m.TypeName).ToList();
                    GvFeeAmount.DataSource = billHistoryList;
                    GvFeeAmount.DataBind();
                }

                totalBillLabel.Text = billHistoryList.Sum(m => m.Fees).ToString();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnBillPosting_Click(object sender, EventArgs e)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < GvFeeAmount.Rows.Count; i++)
                    {
                        GridViewRow row = GvFeeAmount.Rows[i];
                        Label lblBillHistoryId = (Label) row.FindControl("lblBillHistoryId");
                        //Label lblFundTypeId = (Label)row.FindControl("lblFundTypeId");
                        Label lblTypeDefinitionId = (Label) row.FindControl("lblTypeDefinitionId");
                        Label lblBillHistoryMasterId = (Label) row.FindControl("lblBillHistoryMasterId");
                        Label lblStudentId = (Label) row.FindControl("lblStudentId");
                        TextBox txtComment = (TextBox) row.FindControl("txtComment");
                        TextBox txtNewFeesAmount = (TextBox) row.FindControl("txtNewFeesAmount");
                        //CheckBox studentCheckd = (TextBox)row.FindControl("CheckBox");
                        if (!string.IsNullOrEmpty(txtNewFeesAmount.Text))
                        {
                            if (!string.IsNullOrEmpty(lblBillHistoryId.Text))
                            {
                                BillHistoryMaster billHistoryMasterOb =
                                    (BillHistoryMaster) HttpContext.Current.Session["BillHistoryMasterObj"];
                                //BillHistoryMaster billHistoryMasterOb = (BillHistoryMaster) Session["BillHistoryMasterObj"];
                                int billHistoryId = Convert.ToInt32(lblBillHistoryId.Text);

                                Student student = StudentManager.GetById(billHistoryMasterOb.StudentId);

                                if (billHistoryId > 0)
                                {
                                    BillHistory billHistoryObj = BillHistoryManager.GetById(billHistoryId);
                                    BillHistoryMaster billHistoryMaster =
                                        BillHistoryMasterManager.GetById(billHistoryObj.BillHistoryMasterId);

                                    decimal previousMasterAmount = billHistoryMaster.Amount;
                                    decimal previousAmount = billHistoryObj.Fees;

                                    billHistoryMaster.Amount = billHistoryMaster.Amount - billHistoryObj.Fees;
                                    billHistoryMaster.Amount =
                                        billHistoryMaster.Amount + Convert.ToDecimal(txtNewFeesAmount.Text);

                                    billHistoryObj.Remark = Convert.ToString(txtComment.Text);
                                    billHistoryObj.Fees = Convert.ToDecimal(txtNewFeesAmount.Text);
                                    billHistoryObj.ModifiedBy = userObj.Id;
                                    billHistoryObj.ModifiedDate = DateTime.Now;

                                    bool result = BillHistoryManager.Update(billHistoryObj);
                                    if (result)
                                    {
                                        #region Log Update billHistory

                                        LogGeneralManager.Insert(
                                            DateTime.Now,
                                            BaseAcaCalCurrent.Code,
                                            BaseAcaCalCurrent.FullCode,
                                            BaseCurrentUserObj.LogInID,
                                            "",
                                            "",
                                            "Bill History Edit",
                                            BaseCurrentUserObj.LogInID + " Updated bill history for : " +
                                            "Student Id " + billHistoryObj.StudentId + ", Previous Bill Amount: " + previousAmount + "By New Amount: " + billHistoryObj.Fees,
                                            "Bill History Edit for BillHistoryId: "+ billHistoryId,
                                            ((int) CommonEnum.PageName.BillEdit).ToString(),
                                            CommonEnum.PageName.BillEdit.ToString(),
                                            _pageUrl,
                                             student.Roll);

                                        #endregion

                                        //billHistoryMaster.Amount = billHistoryMaster.Amount - billHistoryObj.Fees;
                                        billHistoryMaster.ModifiedBy = userObj.Id;
                                        billHistoryMaster.ModifiedDate = DateTime.Now;

                                        var isUpdate = BillHistoryMasterManager.Update(billHistoryMaster);
                                        if (isUpdate)
                                        {
                                            #region Log Update Bill History Master

                                            LogGeneralManager.Insert(
                                                DateTime.Now,
                                                BaseAcaCalCurrent.Code,
                                                BaseAcaCalCurrent.FullCode,
                                                BaseCurrentUserObj.LogInID,
                                                "",
                                                "",
                                                "Bill History master edit",
                                                BaseCurrentUserObj.LogInID + " Updated bill master for " +
                                                "Student Id " + billHistoryMaster.StudentId + ", Previous Bill master Amount: " + previousMasterAmount.ToString() + "By New Amount: " + billHistoryMaster.Amount,
                                                "Bill History master edit for BillHistoryMasterId: "+billHistoryMaster.BillHistoryMasterId.ToString(),
                                                ((int) CommonEnum.PageName.BillEdit).ToString(),
                                                CommonEnum.PageName.BillEdit.ToString(),
                                                _pageUrl,
                                                  student.Roll);

                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    //int billHistoryId = Convert.ToInt32(lblBillHistoryId.Text);

                                    BillHistory billHistoryObj = new BillHistory();

                                    int typeDefinitionId = Convert.ToInt32(lblTypeDefinitionId.Text);
                                    Student studentObj = StudentManager.GetByRoll(txtStudentRoll.Text);
                                    //int studentId = Convert.ToInt32(studentObj.StudentID);
                                    int studentId = billHistoryMasterOb.StudentId;
                                    string remark = Convert.ToString(txtComment.Text);
                                    decimal feeAmount = Convert.ToDecimal(txtNewFeesAmount.Text);
                                    //var feeSetup = FeeSetupManager.GetAllByProgramIdBatchIdTypeDefinationIdScholarshipStatusAndGovNonGov(
                                    //        studentObj.ProgramID, studentObj.BatchId, typeDefinitionId, 0,
                                    //        studentObj.StudentAddionalInformation.StudentParentsPrimaryJobTypeEnumId)
                                    //    .FirstOrDefault();
                                    FeeSetup feeSetup = null;
                                    if (feeSetup == null)
                                    {
                                        LogicLayer.BusinessObjects.TypeDefinition typeDefinition =
                                            TypeDefinitionManager.GetById(typeDefinitionId);
                                        string message = "Please set the fee type for this bill head " +
                                                         typeDefinition.Definition + " first and then try again.";
                                        PopupMessage(message);
                                        LoadStudentBill(billHistoryObj.StudentId, billHistoryMasterOb.ReferenceNo);
                                        return;
                                    }

                                    //InsertBillHistory(billHistoryMasterOb, studentId, typeDefinitionId, feeAmount,
                                    //    remark, feeSetup.FeeSetupId);

                                    //BillHistoryMaster billHistoryMaster = BillHistoryMasterManager.GetById(billHistoryMasterOb.BillHistoryMasterId);
                                    decimal previousMasterAmount = billHistoryMasterOb.Amount;

                                    billHistoryMasterOb.Amount = billHistoryMasterOb.Amount + feeAmount;
                                    billHistoryMasterOb.ModifiedBy = userObj.Id;
                                    billHistoryMasterOb.ModifiedDate = DateTime.Now;
                                    var isUpdate = BillHistoryMasterManager.Update(billHistoryMasterOb);
                                    if (isUpdate)
                                    {
                                        #region Log Update Bill History Master

                                        LogGeneralManager.Insert(
                                            DateTime.Now,
                                            BaseAcaCalCurrent.Code,
                                            BaseAcaCalCurrent.FullCode,
                                            BaseCurrentUserObj.LogInID,
                                            "",
                                            "",
                                            "Bill History master edit",
                                            BaseCurrentUserObj.LogInID + " Updated bill master for : " +
                                            "Student Id " + billHistoryMasterOb.StudentId + ", Previous Bill master Amount: " + previousMasterAmount.ToString() + "By New Amount: " + billHistoryMasterOb.Amount,
                                            "Bill History master edit for BillHistoryMasterId: " + billHistoryMasterOb.BillHistoryMasterId,
                                            ((int)CommonEnum.PageName.BillEdit).ToString(),
                                            CommonEnum.PageName.BillEdit.ToString(),
                                            _pageUrl,
                                             student.Roll);

                                        #endregion
                                    }
                                    //LoadStudentBill(studentId, billHistoryMasterOb.ReferenceNo);
                                    //bool result = BillHistoryManager.Update(billHistoryObj);
                                }
                            }
                        }
                    }
                    BillHistoryMaster billHistoryMasterObject =
                        (BillHistoryMaster)HttpContext.Current.Session["BillHistoryMasterObj"];
                    LoadStudentBill(billHistoryMasterObject.StudentId, billHistoryMasterObject.ReferenceNo);

                    scope.Complete();
                }
                catch (Exception exception)
                {
                    scope.Dispose();
                    scope.Complete();
                }
            }
        }

        private void InsertBillHistory(BillHistoryMaster billHistoryMasterOb, int studentId, int typeDefinitionId, decimal feeAmount, string remark, int feeSetupId)
        {
            try
            {
                Student student = StudentManager.GetById(billHistoryMasterOb.StudentId);
                if (feeAmount > 0)
                {
                    BillHistory billHistroyObj = new BillHistory();
                    billHistroyObj.StudentId = studentId;
                    billHistroyObj.Fees = feeAmount;
                    billHistroyObj.BillingDate = billHistoryMasterOb.BillingDate;// DateTime.ParseExact(txtBillingDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                    billHistroyObj.Remark = remark;
                    billHistroyObj.FeeSetupId = feeSetupId;
                    billHistroyObj.TypeDefinitionId = typeDefinitionId;
                    billHistroyObj.BillHistoryMasterId = billHistoryMasterOb.BillHistoryMasterId;
                    billHistroyObj.AcaCalId = billHistoryMasterOb.AcaCalId;
                    billHistroyObj.IsDeleted = false;
                    billHistroyObj.CreatedBy = userObj.Id;
                    billHistroyObj.CreatedDate = DateTime.Now;
                    billHistroyObj.ModifiedBy = userObj.Id;
                    billHistroyObj.ModifiedDate = DateTime.Now;

                    int billInsertId = BillHistoryManager.Insert(billHistroyObj);
                    if (billInsertId > 0)
                    {
                        lblMsg.Text = "Bill inserted successfully.";
                        #region Log Update billHistory
                        LogGeneralManager.Insert(
                            DateTime.Now,
                            BaseAcaCalCurrent.Code,
                            BaseAcaCalCurrent.FullCode,
                            BaseCurrentUserObj.LogInID,
                            "",
                            "",
                            "Bill History Entry",
                            BaseCurrentUserObj.LogInID + " inserted bill history for Student Id " + studentId + ", from bill edit page and " + ", Amount: " + feeAmount.ToString(),
                            "inserted bill history from bill edit page by" + BaseCurrentUserObj.LogInID,
                            ((int)CommonEnum.PageName.BillEdit).ToString(),
                            CommonEnum.PageName.BillEdit.ToString(),
                            _pageUrl,
                             student.Roll);
                        #endregion
                        //billInsertCounter = billInsertCounter + 1;
                    }
                    else
                    {
                        lblMsg.Text = "Bill could not inserted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnLoadFeeItem_Click(object sender, EventArgs e)
        {
            try
            {
                int feeItemId = Convert.ToInt32(ddlFeeItem.SelectedValue);
                AddFeeItem();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        public void Reset()
        {
            try
            {
                //batchLabel.Text = String.Empty;
                totalBillLabel.Text = string.Empty;
                GvFeeAmount.DataSource = null;
                GvFeeAmount.DataBind();
                Session["BillHistoryMasterObj"] = null;
                Session["BillHistoryDetailList"] = null;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void PopupMessage(string message)
        {

            ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('" + message + "');", true);

        }

    }
}