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
    public partial class StudentBillHistory : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentBillHistory);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentBillHistory));

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                string studentRoll = Convert.ToString(txtStudentRoll.Text);
                Student studentObj = StudentManager.GetByRoll(studentRoll);
                if (studentObj != null)
                {
                    lblStudentName.Text = studentObj.BasicInfo.FullName;
                    lblStudentProgram.Text = studentObj.Program.DetailName;
                    lblBatch.Text = studentObj.StudentAdmissionAcaCalId.ToString();
                    lblEmailAddress.Text = studentObj.BasicInfo.Email;
                    List<BillPaymentHistoryMasterDTO> billPaymentHistoryMasterList = BillHistoryManager.GetBillPaymentHistoryMasterByStudentId(studentObj.StudentID);
                    //List<BillPaymentHistoryDTO> billPaymentHistoryList = BillHistoryManager.GetBillPaymentHistoryByStudentId(studentObj.StudentID);
                    if (billPaymentHistoryMasterList != null && billPaymentHistoryMasterList.Count > 0)
                    {
                        lblTotalBill.Text = Convert.ToString(billPaymentHistoryMasterList.Sum(d => d.BillAmount));
                        lblTotalPayment.Text = Convert.ToString(billPaymentHistoryMasterList.Sum(d => d.PaymentAmount));

                        GvBillMaster.Columns[2].FooterText = "Total";
                        GvBillMaster.Columns[4].FooterText = billPaymentHistoryMasterList.AsEnumerable().Select(x => x.BillAmount).Sum().ToString();
                        GvBillMaster.Columns[6].FooterText = billPaymentHistoryMasterList.AsEnumerable().Select(x => x.PaymentAmount).Sum().ToString();

                        GvBillMaster.DataSource = billPaymentHistoryMasterList;
                        GvBillMaster.DataBind();
                    }
                    else
                    {
                        lblTotalBill.Text = Convert.ToString(billPaymentHistoryMasterList.Sum(d => d.BillAmount));
                        lblTotalPayment.Text = Convert.ToString(billPaymentHistoryMasterList.Sum(d => d.PaymentAmount));
                        GvBillMaster.DataSource = billPaymentHistoryMasterList;
                        GvBillMaster.DataBind();
                        GvBillMaster.EmptyDataText = "No bill history has been found.";
                    }
                }
                else
                {
                    lblMsg.Text = "Student not found.";
                    lblStudentName.Text = "";
                    lblStudentProgram.Text = "";
                    lblEmailAddress.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblStudentName.Text = "";
                lblStudentProgram.Text = "";
                lblEmailAddress.Text = "";
            }
        }

        protected void GvBillMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewBill")
                {
                    lblPopUpMsg.Text = "";
                    decimal totalBill = 0;
                    this.ModalShowBillHistoryDetailsPopupExtender.Show();
                    int billHistoryMasterId = Convert.ToInt32(e.CommandArgument);

                    if (billHistoryMasterId > 0)
                    {

                        BillHistoryMaster billHistoryMasterObj = BillHistoryMasterManager.GetById(billHistoryMasterId);
                        if (billHistoryMasterObj != null)
                        {

                            LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(billHistoryMasterObj.StudentId);

                            List<BillPaymentHistoryDTO> billPaymentHistoryList = BillHistoryManager.GetBillPaymentHistoryByBillHistoryMasterId(billHistoryMasterObj.BillHistoryMasterId).ToList();
                            foreach (BillPaymentHistoryDTO billPaymentHistoryDto in billPaymentHistoryList)
                            {
                                if (billPaymentHistoryDto.FormalCode != null)
                                {
                                    billPaymentHistoryDto.FeesName = billPaymentHistoryDto.FeesName + '(' +
                                                                     billPaymentHistoryDto.FormalCode +
                                                                     ", Cr" + billPaymentHistoryDto.Credits + ')';
                                }
                            }
                            if (billPaymentHistoryList.Any())
                            {
                                billPaymentHistoryList = billPaymentHistoryList.OrderBy(m => m.FeesName).ToList();

                                GvStudentBillPaymentHistory.Columns[5].FooterText = "Total";
                                GvStudentBillPaymentHistory.Columns[6].FooterText = billPaymentHistoryList.AsEnumerable().Select(x => x.BillAmount).Sum().ToString();
                                GvStudentBillPaymentHistory.Columns[7].FooterText = billPaymentHistoryList.AsEnumerable().Select(x => x.PaymentAmount).Sum().ToString();

                                GvStudentBillPaymentHistory.DataSource = billPaymentHistoryList;
                                GvStudentBillPaymentHistory.DataBind();

                                totalBill = Convert.ToDecimal(billPaymentHistoryList.Sum(s => s.BillAmount));
                            }
                        }
                    }

                    lblpayableBill.Text = totalBill.ToString();
                    //lblBillMasterId.Text = billHistoryMasterId.ToString();
                }
                else if (e.CommandName == "OnlinePayment")
                {
                    decimal totalBill = 0;
                    string billHistoryIds = "";
                    int billHistoryMasterId = Convert.ToInt32(e.CommandArgument);

                    if (billHistoryMasterId > 0)
                    {

                        BillHistoryMaster billHistoryMasterObj = BillHistoryMasterManager.GetById(billHistoryMasterId);
                        if (billHistoryMasterObj != null)
                        {
                            List<BillPaymentHistoryDTO> billPaymentHistoryList = BillHistoryManager.GetBillPaymentHistoryByBillHistoryMasterId(billHistoryMasterObj.BillHistoryMasterId).ToList();
                            if (billPaymentHistoryList != null && billPaymentHistoryList.Count > 0)
                            {
                                billHistoryIds = String.Join(",", billPaymentHistoryList.Select(s => s.BillHistoryId).ToArray());
                                totalBill = Convert.ToDecimal(billPaymentHistoryList.Sum(s => s.BillAmount));
                            }
                        }
                    }

                    lblpayableBill.Text = totalBill.ToString();
                    //lblBillMasterId.Text = billHistoryMasterId.ToString();

                    //if (!string.IsNullOrEmpty(billHistoryIds))
                    //{
                    //    BillHistoryOrder bho = BillHistoryMasterManager.GetOrderIdByBillHistoryIdList(billHistoryIds, BaseCurrentUserObj.Id);

                    //    if (bho != null)
                    //    {
                    //        if (bho.StatusId == 0 && bho.OrderId != 0)
                    //        {
                    //            PayOnline(totalBill.ToString(), bho.OrderId.ToString());
                    //        }
                    //        else
                    //        {
                    //            lblMsg.Text = "Please Contact System Admin!";

                    //        }

                    //    }
                    //}


                }
            }
            catch (Exception ex)
            { }
        }

        protected void chkAllStudentPayHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalBill = 0;

                CheckBox chkHeader = (CheckBox)GvStudentBillPaymentHistory.HeaderRow.FindControl("chkAllStudentPayHeader");
                if (chkHeader.Checked)
                {
                    for (int i = 0; i < GvStudentBillPaymentHistory.Rows.Count; i++)
                    {
                        GridViewRow row = GvStudentBillPaymentHistory.Rows[i];
                        Label PaymentAmount = (Label)row.FindControl("lblFee");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("chkBill");
                        totalBill = totalBill + Convert.ToDecimal(PaymentAmount.Text);

                        studentCheckd.Checked = true;
                    }
                }
                if (!chkHeader.Checked)
                {
                    for (int i = 0; i < GvStudentBillPaymentHistory.Rows.Count; i++)
                    {
                        GridViewRow row = GvStudentBillPaymentHistory.Rows[i];
                        CheckBox studentCheckd = (CheckBox)row.FindControl("chkBill");
                        studentCheckd.Checked = false;
                    }
                }

                lblpayableBill.Text = totalBill.ToString();
                this.ModalShowBillHistoryDetailsPopupExtender.Show();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void chkStudentPayHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalBill = Convert.ToDecimal(lblpayableBill.Text);

                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                int index = row.RowIndex;
                CheckBox chkBill = (CheckBox)GvStudentBillPaymentHistory.Rows[index].FindControl("chkBill");
                Label BillAmount = (Label)GvStudentBillPaymentHistory.Rows[index].FindControl("lblFee");

                if (chkBill.Checked == true)
                {
                    totalBill = totalBill + Convert.ToDecimal(BillAmount.Text);
                }
                else if (chkBill.Checked == false)
                {
                    totalBill = totalBill - Convert.ToDecimal(BillAmount.Text);
                }


                lblpayableBill.Text = totalBill.ToString();
                this.ModalShowBillHistoryDetailsPopupExtender.Show();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        //protected void btnPayOnline_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string billHistoryIds = "";
        //        List<string> idList = new List<string>();

        //        for (int i = 0; i < GvStudentBillPaymentHistory.Rows.Count; i++)
        //        {
        //            GridViewRow row = GvStudentBillPaymentHistory.Rows[i];
        //            Label lblBillHistoryId = (Label)row.FindControl("lblBillHistoryId");
        //            CheckBox chkBill = (CheckBox)row.FindControl("chkBill");
        //            if (chkBill.Checked)
        //            {
        //                idList.Add(lblBillHistoryId.Text);
        //            }
        //        }

        //        if (idList != null && idList.Count() > 0)
        //        {
        //            billHistoryIds = String.Join(",", idList.ToArray());
        //        }


        //        if (!string.IsNullOrEmpty(billHistoryIds))
        //        {
        //            BillHistoryOrder bho = BillHistoryMasterManager.GetOrderIdByBillHistoryIdList(billHistoryIds, BaseCurrentUserObj.Id);

        //            if (bho != null)
        //            {
        //                if (bho.StatusId == 0 && bho.OrderId != 0)
        //                {
        //                    PayOnline(lblpayableBill.Text, bho.OrderId.ToString());
        //                }
        //                else
        //                {
        //                    lblPopUpMsg.Text = "Please Contact System Admin!";
        //                    this.ModalShowBillHistoryDetailsPopupExtender.Show();
        //                }

        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    { }
        //}

        private void PayOnline(string totalBill, string OrderId)
        {
            try
            {
                string EmailAddress = lblEmailAddress.Text;

                string parameters = "StudentId=" + txtStudentRoll.Text + "&Name=" + lblStudentName.Text + "&OrderId=" + OrderId + "&Amount=" + totalBill + "&EmailAddress=" + EmailAddress;

                string url = string.Format("~/Module/bill/PayOnline.aspx?" + parameters);

                Response.Redirect(url, false);

                #region Log Insert

                LogGeneralManager.Insert(
                                                     DateTime.Now,
                                                     BaseAcaCalCurrent.Code,
                                                     BaseAcaCalCurrent.FullCode,
                                                     BaseCurrentUserObj.LogInID,
                                                     "",
                                                     "",
                                                     "Online Payment",
                                                     BaseCurrentUserObj.LogInID + " opening payment getway with amount : " + lblpayableBill.Text + "" + txtStudentRoll.Text,
                                                     "normal",
                                                      ((int)CommonEnum.PageName.StudentBillHistory).ToString(),
                                                     CommonEnum.PageName.StudentBillHistory.ToString(),
                                                     _pageUrl,
                                                     txtStudentRoll.Text);
                #endregion

                lblBillMasterId.Text = "";

            }
            catch (Exception ex)
            { }
        }

        protected void closeImageButton_OnClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                GvStudentBillPaymentHistory.DataSource = null;
                GvStudentBillPaymentHistory.DataBind();
                lblpayableBill.Text = "0";
                this.ModalShowBillHistoryDetailsPopupExtender.Hide();
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}