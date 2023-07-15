using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.result
{
    public partial class ExamTemplateItemManage : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                LoadThirdExaminerDropDownList();

                LoadExamTemplateDDL();
                txtExamMark.Text = null;
                if (rdbBasicColumn.Checked)
                {
                    txtExamMark.Enabled = true;
                    txtExamPassMark.Enabled = true;
                }
                if (rdbCalculativeColumn.Checked)
                {
                    txtExamMark.Enabled = false;
                    txtExamPassMark.Enabled = true;
                }
            }
            lblMsg.Text = null;
        }

        private void LoadExamTemplateDDL()
        {
            ddlExamTemplateName.DataSource = ExamTemplateManager.GetAll();
            ddlExamTemplateName.DataTextField = "ExamTemplateName";
            ddlExamTemplateName.DataValueField = "ExamTemplateId";
            ddlExamTemplateName.DataBind();
            ListItem item = new ListItem("-Select Exam Template-", "0");
            ddlExamTemplateName.Items.Insert(0, item);
        }

        private void LoadGvExamTemplateItem(int examTemplateId)
        {
            GvExamTemplateItem.DataSource = ExamTemplateItemManager.GetAll().Where(d => d.ExamTemplateId == examTemplateId).ToList();
            GvExamTemplateItem.DataBind();
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if (CheckAllNecessaryField())
                {
                    if (rdbBasicColumn.Checked)
                    {
                        InsertBasicExamTemplateItem();
                    }
                    else if (rdbCalculativeColumn.Checked)
                    {
                        InsertCalculativeExamTemplateItem();
                    }
                    else if (rdbGradeColumn.Checked)
                    {
                        InsertGradeExamTemplateItem();
                    }
                    else
                    {

                    }
                }
                else
                {
                }
            }
            catch (Exception ex) { }
        }

        private bool CheckAllNecessaryField()
        {
            bool flug = false;
            if (Convert.ToInt32(ddlExamTemplateName.SelectedValue) == 0)
            {
                lblMsg.Text = "Please select a template.";
                return flug;
            }
            else if (txtExamName.Text == null || txtExamName.Text == string.Empty)
            {
                lblMsg.Text = "Please provide a exam/column name.";
                return flug;
            }
            else if (txtSequence.Text == null || txtSequence.Text == string.Empty)
            {
                lblMsg.Text = "Please provide a column sequence.";
                return flug;
            }
            else { return flug = true; }
        }

        private void InsertBasicExamTemplateItem()
        {
            try
            {
                if (Convert.ToDecimal(txtExamMark.Text) != 0)
                {
                    ExamTemplateItem examTemplateItem = new ExamTemplateItem();
                    if (multipleExamDropDownList.SelectedItem.Value != "-1")
                    {
                        examTemplateItem.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                    }
                    examTemplateItem.ExamTemplateId = Convert.ToInt16(ddlExamTemplateName.SelectedValue);
                    examTemplateItem.ExamName = txtExamName.Text;
                    examTemplateItem.ExamMark = Convert.ToDecimal(txtExamMark.Text);
                    if (!string.IsNullOrEmpty(txtExamPassMark.Text))
                    {
                        examTemplateItem.PassMark = Convert.ToDecimal(txtExamPassMark.Text);
                    }
                    examTemplateItem.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                    if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                    {
                        examTemplateItem.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                    }
                    examTemplateItem.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic;
                    examTemplateItem.CreatedBy = userObj.Id;
                    examTemplateItem.CreatedDate = DateTime.Now;
                    examTemplateItem.ModifiedBy = userObj.Id;
                    examTemplateItem.ModifiedDate = DateTime.Now;
                    if (rdbTrue.Checked)
                    {
                        examTemplateItem.ShowInTabulation = true;
                        examTemplateItem.TabulationTitle = txtTabulationTitle.Text;
                    }
                    else
                    {
                        examTemplateItem.ShowInTabulation = false;
                        examTemplateItem.TabulationTitle = null;
                    }
                    if (rdbContinousAssesmentTrue.Checked)
                    {
                        examTemplateItem.Attribute1 = "Continuous";
                    }
                    if (rdbVivaTrue.Checked)
                    {
                        examTemplateItem.Attribute2 = "Viva";
                    }
                    if (rdbIsFinalExamTrue.Checked)
                    {
                        examTemplateItem.IsFinalExam = true;
                    }

                    if (rdbIsSingleQuestionTrue.Checked)
                    {
                        examTemplateItem.SingleQuestionAnswer = true;
                    }

                    if (rdbShowAllContinuousInSubTotalTrue.Checked)
                    {
                        examTemplateItem.ShowAllContinuousInSubTotal = true;
                    }

                    if (rdbShowAllInGrandTotalTrue.Checked)
                    {
                        examTemplateItem.ShowAllInGrandTotal = true;
                    }
                    if (ddlNoOfExaminer.SelectedValue != "-1")
                    {
                        examTemplateItem.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                    }

                    if (ExamTemplateItemManager.GetByExamNameColumnSequence(examTemplateItem.ExamTemplateId, examTemplateItem.ExamMark, examTemplateItem.ColumnSequence))
                    {
                        int result = ExamTemplateItemManager.Insert(examTemplateItem);

                        if (result > 0)
                        {
                            LoadGvExamTemplateItem(examTemplateItem.ExamTemplateId);
                            lblMsg.Text = "Exam template item  added successful.";
                            ClearAllTextBox();
                        }
                        else
                        {
                            lblMsg.Text = "Exam template item could not added successfully.";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Exam template item already exists.";
                    }
                }
                else
                {
                    lblMsg.Text = "Please provide a valid exam mark.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertCalculativeExamTemplateItem()
        {
            try
            {
                ExamTemplateItem examTemplateItem = new ExamTemplateItem();
                if (multipleExamDropDownList.SelectedItem.Value != "-1")
                {
                    examTemplateItem.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                }
                examTemplateItem.ExamTemplateId = Convert.ToInt16(ddlExamTemplateName.SelectedValue);
                examTemplateItem.ExamName = txtExamName.Text;
                if (!string.IsNullOrEmpty(txtExamPassMark.Text))
                {
                    examTemplateItem.PassMark = Convert.ToDecimal(txtExamPassMark.Text);
                }
                examTemplateItem.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                {
                    examTemplateItem.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                }
                examTemplateItem.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Calculative;
                if (rdbPercentage.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Percentage;
                    examTemplateItem.MultiplyBy = Convert.ToDecimal(txtMultiplyBy.Text);
                    examTemplateItem.DivideBy = Convert.ToDecimal(txtDivideBy.Text);
                }
                if (rdbAverage.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Average;
                }
                if (rdbSum.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Sum;
                }
                if (rdbBestOne.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_One;
                }
                if (rdbBestTwo.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Two;
                }
                if (rdbBestThree.Checked)
                {
                    examTemplateItem.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Three;
                }

                examTemplateItem.CreatedBy = userObj.Id;
                examTemplateItem.CreatedDate = DateTime.Now;
                examTemplateItem.ModifiedBy = userObj.Id;
                examTemplateItem.ModifiedDate = DateTime.Now;
                if (rdbTrue.Checked)
                {
                    examTemplateItem.ShowInTabulation = true;
                    examTemplateItem.TabulationTitle = txtTabulationTitle.Text;
                }
                else
                {
                    examTemplateItem.ShowInTabulation = false;
                    examTemplateItem.TabulationTitle = null;
                }

                if (rdbIsSingleQuestionTrue.Checked)
                {
                    examTemplateItem.SingleQuestionAnswer = true;
                }

                if (rdbShowAllContinuousInSubTotalTrue.Checked)
                {
                    examTemplateItem.ShowAllContinuousInSubTotal = true;
                }

                if (rdbShowAllInGrandTotalTrue.Checked)
                {
                    examTemplateItem.ShowAllInGrandTotal = true;
                }
                if (ddlNoOfExaminer.SelectedValue != "-1")
                {
                    examTemplateItem.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                }

                if (ExamTemplateItemManager.GetByExamNameColumnSequence(examTemplateItem.ExamTemplateId, examTemplateItem.ExamMark, examTemplateItem.ColumnSequence))
                {
                    int result = ExamTemplateItemManager.Insert(examTemplateItem);

                    if (result > 0)
                    {
                        InsertExamMarkColumnOrder(result);
                        LoadGvExamTemplateItem(examTemplateItem.ExamTemplateId);
                        lblMsg.Text = "Exam template item added successfully";
                        ClearAllTextBox();
                    }
                    else
                    {
                        lblMsg.Text = "Exam template item could not added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Exam template item already exists.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertGradeExamTemplateItem()
        {
            try
            {
                ExamTemplateItem examTemplateItem = new ExamTemplateItem();
                if (multipleExamDropDownList.SelectedItem.Value != "-1")
                {
                    examTemplateItem.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                }
                examTemplateItem.ExamTemplateId = Convert.ToInt16(ddlExamTemplateName.SelectedValue);
                examTemplateItem.ExamName = txtExamName.Text;
                examTemplateItem.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                {
                    examTemplateItem.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                }
                examTemplateItem.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Grade;
                examTemplateItem.CreatedBy = userObj.Id;
                examTemplateItem.CreatedDate = DateTime.Now;
                examTemplateItem.ModifiedBy = userObj.Id;
                examTemplateItem.ModifiedDate = DateTime.Now;
                if (rdbTrue.Checked)
                {
                    examTemplateItem.ShowInTabulation = true;
                    examTemplateItem.TabulationTitle = txtTabulationTitle.Text;
                }
                else
                {
                    examTemplateItem.ShowInTabulation = false;
                    examTemplateItem.TabulationTitle = null;
                }


                if (rdbIsSingleQuestionTrue.Checked)
                {
                    examTemplateItem.SingleQuestionAnswer = true;
                }

                if (rdbShowAllContinuousInSubTotalTrue.Checked)
                {
                    examTemplateItem.ShowAllContinuousInSubTotal = true;
                }

                if (rdbShowAllInGrandTotalTrue.Checked)
                {
                    examTemplateItem.ShowAllInGrandTotal = true;
                }
                if (ddlNoOfExaminer.SelectedValue != "-1")
                {
                    examTemplateItem.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                }

                if (ExamTemplateItemManager.GetByExamNameColumnSequence(examTemplateItem.ExamTemplateId, examTemplateItem.ExamMark, examTemplateItem.ColumnSequence))
                {
                    int result = ExamTemplateItemManager.Insert(examTemplateItem);

                    if (result > 0)
                    {
                        InsertExamMarkColumnOrder(result);
                        LoadGvExamTemplateItem(examTemplateItem.ExamTemplateId);
                        lblMsg.Text = "Exam template item added successfully";
                        ClearAllTextBox();
                    }
                    else
                    {
                        lblMsg.Text = "Exam template item could not added successfully";
                    }
                }
                else
                {
                    lblMsg.Text = "Exam template item already exists.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertExamMarkColumnOrder(int result)
        {
            if (result > 0)
            {
                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(result);
                for (int i = 1; i <= 9; i++)
                {
                    ExamMarkEquationColumnOrder examEquationColumnOrder = new ExamMarkEquationColumnOrder();
                    examEquationColumnOrder.ColumnSequence = examTemplateItemObj.ColumnSequence;
                    examEquationColumnOrder.TemplateItemId = result;
                    examEquationColumnOrder.CreatedBy = userObj.Id;
                    examEquationColumnOrder.CreatedDate = DateTime.Now;
                    examEquationColumnOrder.ModifiedBy = userObj.Id;
                    examEquationColumnOrder.ModifiedDate = DateTime.Now;
                    if (i == 1 && txtColumnNo1.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo1.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 2 && txtColumnNo2.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo2.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }

                    else if (i == 3 && txtColumnNo3.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo3.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 4 && txtColumnNo4.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo4.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 5 && txtColumnNo5.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo5.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 6 && txtColumnNo6.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo6.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 7 && txtColumnNo7.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo7.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 8 && txtColumnNo8.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo8.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                    else if (i == 9 && txtColumnNo9.Text != string.Empty)
                    {
                        examEquationColumnOrder.SumColumnNo = Convert.ToInt32(txtColumnNo9.Text);
                        int result2 = ExamMarkEquationColumnOrderManager.Insert(examEquationColumnOrder);
                    }
                }
            }
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if (CheckAllNecessaryField())
                {
                    if (rdbBasicColumn.Checked)
                    {
                        UpdateBasicExamTemplateItem();
                    }
                    else if (rdbCalculativeColumn.Checked)
                    {
                        UpdateCalculativeExamTemplateItem();
                    }
                    else if (rdbGradeColumn.Checked)
                    {
                        UpdateGradeExamTemplateItem();
                    }
                    else
                    {

                    }
                    ResetRadioButtonValue();
                }
                else
                {
                }

            }
            catch (Exception ex) { }
        }

        private void UpdateGradeExamTemplateItem()
        {
            try
            {
                int examTemplateItemId = Convert.ToInt32(hdnExamTemplateItemId.Value);
                var examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                examTemplateItemObj.ExamName = txtExamName.Text;
                examTemplateItemObj.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                if (multipleExamDropDownList.SelectedItem.Value != "-1")
                {
                    examTemplateItemObj.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                }
                if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                {
                    examTemplateItemObj.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                }
                examTemplateItemObj.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Grade;
                examTemplateItemObj.ModifiedBy = userObj.Id;
                examTemplateItemObj.ModifiedDate = DateTime.Now;
                if (rdbTrue.Checked)
                {
                    examTemplateItemObj.ShowInTabulation = true;
                    examTemplateItemObj.TabulationTitle = txtTabulationTitle.Text;
                }
                else
                {
                    examTemplateItemObj.ShowInTabulation = false;
                    examTemplateItemObj.TabulationTitle = null;
                }


                if (rdbIsFinalExamTrue.Checked)
                {
                    examTemplateItemObj.IsFinalExam = true;
                }
                else
                    examTemplateItemObj.IsFinalExam = false;

                if (rdbIsSingleQuestionTrue.Checked)
                {
                    examTemplateItemObj.SingleQuestionAnswer = true;
                }
                else
                    examTemplateItemObj.SingleQuestionAnswer = false;
                if (rdbShowAllContinuousInSubTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllContinuousInSubTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllContinuousInSubTotal = false;
                if (rdbShowAllInGrandTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllInGrandTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllInGrandTotal = false;

                if (ddlNoOfExaminer.SelectedValue != "-1")
                {
                    examTemplateItemObj.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                }
                if (rdbContinousAssesmentTrue.Checked)
                {
                    examTemplateItemObj.Attribute1 = "Continuous";
                }
                else
                {
                    examTemplateItemObj.Attribute1 = null;
                }

                if (rdbVivaTrue.Checked)
                {
                    examTemplateItemObj.Attribute2 = "Viva";
                }
                else
                {
                    examTemplateItemObj.Attribute2 = null;
                }

                bool result = ExamTemplateItemManager.Update(examTemplateItemObj);
                if (result)
                {
                    LoadGvExamTemplateItem(examTemplateItemObj.ExamTemplateId);
                    lblMsg.Text = "Exam template item edited successfully.";
                    ClearAllTextBox();
                }
                else
                {
                    lblMsg.Text = "Exam template item could not edited successfully.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateBasicExamTemplateItem()
        {
            try
            {
                int examTemplateItemId = Convert.ToInt32(hdnExamTemplateItemId.Value);
                var examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                examTemplateItemObj.ExamName = txtExamName.Text;
                examTemplateItemObj.ExamMark = Convert.ToDecimal(txtExamMark.Text);
                if (multipleExamDropDownList.SelectedItem.Value != "-1")
                {
                    examTemplateItemObj.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                }
                if (!string.IsNullOrEmpty(txtExamPassMark.Text))
                {
                    examTemplateItemObj.PassMark = Convert.ToDecimal(txtExamPassMark.Text);
                }
                examTemplateItemObj.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                {
                    examTemplateItemObj.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                }
                examTemplateItemObj.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                examTemplateItemObj.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Basic;
                examTemplateItemObj.ModifiedBy = userObj.Id;
                examTemplateItemObj.ModifiedDate = DateTime.Now;
                if (rdbTrue.Checked)
                {
                    examTemplateItemObj.ShowInTabulation = true;
                    examTemplateItemObj.TabulationTitle = txtTabulationTitle.Text;
                }
                else
                {
                    examTemplateItemObj.ShowInTabulation = false;
                    examTemplateItemObj.TabulationTitle = null;
                }
                if (rdbContinousAssesmentTrue.Checked)
                {
                    examTemplateItemObj.Attribute1 = "Continuous";
                }
                else
                {
                    examTemplateItemObj.Attribute1 = null;
                }

                if (rdbVivaTrue.Checked)
                {
                    examTemplateItemObj.Attribute2 = "Viva";
                }
                else
                {
                    examTemplateItemObj.Attribute2 = null;
                }

                if (rdbIsFinalExamTrue.Checked)
                {
                    examTemplateItemObj.IsFinalExam = true;
                }
                else
                    examTemplateItemObj.IsFinalExam = false;

                if (rdbIsSingleQuestionTrue.Checked)
                {
                    examTemplateItemObj.SingleQuestionAnswer = true;
                }
                else
                    examTemplateItemObj.SingleQuestionAnswer = false;
                if (rdbShowAllContinuousInSubTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllContinuousInSubTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllContinuousInSubTotal = false;
                if (rdbShowAllInGrandTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllInGrandTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllInGrandTotal = false;

                if (ddlNoOfExaminer.SelectedValue != "-1")
                {
                    examTemplateItemObj.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                }

                bool result = ExamTemplateItemManager.Update(examTemplateItemObj);
                if (result)
                {
                    LoadGvExamTemplateItem(examTemplateItemObj.ExamTemplateId);
                    lblMsg.Text = "Exam template item edited successfully.";
                    ClearAllTextBox();
                }
                else
                {
                    lblMsg.Text = "Exam template item could not edited successfully.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateCalculativeExamTemplateItem()
        {
            try
            {
                int examTemplateItemId = Convert.ToInt32(hdnExamTemplateItemId.Value);
                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                examTemplateItemObj.ExamTemplateId = Convert.ToInt16(ddlExamTemplateName.SelectedValue);
                examTemplateItemObj.ExamName = txtExamName.Text;
                if (multipleExamDropDownList.SelectedItem.Value != "-1")
                {
                    examTemplateItemObj.MultipleExaminer = Convert.ToInt32(multipleExamDropDownList.SelectedValue);
                }
                if (!string.IsNullOrEmpty(txtExamPassMark.Text))
                {
                    examTemplateItemObj.PassMark = Convert.ToDecimal(txtExamPassMark.Text);
                }
                examTemplateItemObj.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                if (!string.IsNullOrEmpty(txtPrintSequence.Text))
                {
                    examTemplateItemObj.PrintColumnSequence = Convert.ToInt32(txtPrintSequence.Text);
                }
                examTemplateItemObj.ColumnSequence = Convert.ToInt32(txtSequence.Text);
                examTemplateItemObj.ColumnType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnType.Calculative;
                if (rdbPercentage.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Percentage;
                    examTemplateItemObj.MultiplyBy = Convert.ToDecimal(txtMultiplyBy.Text);
                    examTemplateItemObj.DivideBy = Convert.ToDecimal(txtDivideBy.Text);
                }
                if (rdbAverage.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Average;
                }
                if (rdbSum.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Sum;
                }
                if (rdbBestOne.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_One;
                }
                if (rdbBestTwo.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Two;
                }
                if (rdbBestThree.Checked)
                {
                    examTemplateItemObj.CalculationType = (int)CommonUtility.CommonEnum.ExamTemplateItemColumnCalculationType.Best_Three;
                }
                if (rdbContinousAssesmentTrue.Checked)
                {
                    examTemplateItemObj.Attribute1 = "Continuous";
                }
                else
                {
                    examTemplateItemObj.Attribute1 = null;
                }

                if (rdbVivaTrue.Checked)
                {
                    examTemplateItemObj.Attribute2 = "Viva";
                }
                else
                {
                    examTemplateItemObj.Attribute2 = null;
                }

                examTemplateItemObj.ModifiedBy = userObj.Id;
                examTemplateItemObj.ModifiedDate = DateTime.Now;
                if (rdbTrue.Checked)
                {
                    examTemplateItemObj.ShowInTabulation = true;
                    examTemplateItemObj.TabulationTitle = txtTabulationTitle.Text;
                }
                else
                {
                    examTemplateItemObj.ShowInTabulation = false;
                    examTemplateItemObj.TabulationTitle = null;
                }


                if (rdbIsFinalExamTrue.Checked)
                {
                    examTemplateItemObj.IsFinalExam = true;
                }
                else
                    examTemplateItemObj.IsFinalExam = false;

                if (rdbIsSingleQuestionTrue.Checked)
                {
                    examTemplateItemObj.SingleQuestionAnswer = true;
                }
                else
                    examTemplateItemObj.SingleQuestionAnswer = false;
                if (rdbShowAllContinuousInSubTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllContinuousInSubTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllContinuousInSubTotal = false;
                if (rdbShowAllInGrandTotalTrue.Checked)
                {
                    examTemplateItemObj.ShowAllInGrandTotal = true;
                }
                else
                    examTemplateItemObj.ShowAllInGrandTotal = false;

                if (ddlNoOfExaminer.SelectedValue != "-1")
                {
                    examTemplateItemObj.NumberOfExaminer = Convert.ToInt32(ddlNoOfExaminer.SelectedValue);
                }




                bool result = ExamTemplateItemManager.Update(examTemplateItemObj);

                if (result)
                {
                    bool result2 = ExamMarkEquationColumnOrderManager.DeleteByTemplateId(examTemplateItemObj.ExamTemplateItemId);
                    InsertExamMarkColumnOrder(examTemplateItemObj.ExamTemplateItemId);
                    LoadGvExamTemplateItem(examTemplateItemObj.ExamTemplateId);
                    lblMsg.Text = "Exam template item added successfully";
                    ClearAllTextBox();
                }
                else
                {
                    lblMsg.Text = "Exam template item could not added successfully";
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetRadioButtonValue();
            ddlExamTemplateName.Enabled = true;
            ClearAllTextBox();
            AddButton.Visible = true;
            UpdateButton.Visible = false;
            CancelButton.Visible = false;
        }

        private void ClearAllTextBox()
        {
            rdbBasicColumn.Checked = true;
            rdbCalculativeColumn.Checked = false;
            rdbGradeColumn.Checked = false;
            txtExamName.Text = string.Empty;
            txtExamMark.Text = string.Empty;
            txtExamPassMark.Text = string.Empty;
            txtSequence.Text = string.Empty;
            txtPrintSequence.Text = string.Empty;
            txtMultiplyBy.Text = string.Empty;
            txtDivideBy.Text = string.Empty;
            txtColumnNo1.Text = string.Empty;
            txtColumnNo2.Text = string.Empty;
            txtColumnNo3.Text = string.Empty;
            txtColumnNo4.Text = string.Empty;
            txtColumnNo5.Text = string.Empty;
            txtColumnNo6.Text = string.Empty;
            txtColumnNo7.Text = string.Empty;
            txtColumnNo8.Text = string.Empty;
            txtColumnNo9.Text = string.Empty;
            pnlColumnType.Visible = false;
            pnlPercentage.Visible = false;
            pnlRelationalColumn.Visible = false;
            pnlTabulationShow.Visible = false;
            rdbFalse.Checked = true;
            rdbTrue.Checked = false;
        }

        protected void rdbpercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPercentage.Checked)
            {
                pnlPercentage.Visible = true;
                //pnlBestExam.Visible = false;
            }
        }

        protected void rdbAverage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAverage.Checked)
            {
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
            }
        }

        protected void rdbSum_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSum.Checked)
            {
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
            }
        }

        protected void rdbBasicColumn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBasicColumn.Checked)
            {
                pnlColumnType.Visible = false;
                pnlPercentage.Visible = false;
                pnlRelationalColumn.Visible = false;
                txtExamMark.Enabled = true;
                txtExamPassMark.Enabled = true;
            }
        }

        protected void rdbCalculativeColumn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCalculativeColumn.Checked)
            {
                pnlColumnType.Visible = true;
                txtExamMark.Text = null;
                txtExamMark.Enabled = false;
                txtExamPassMark.Enabled = true;
                pnlRelationalColumn.Visible = true;
                if (rdbPercentage.Checked)
                {
                    pnlPercentage.Visible = true;
                }
            }
        }

        protected void rdbGradeColumn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGradeColumn.Checked)
            {
                pnlColumnType.Visible = false;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
                txtExamMark.Text = null;
                txtExamMark.Enabled = false;
                txtExamPassMark.Text = null;
                txtExamPassMark.Enabled = false;
            }
        }

        protected void ddlTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int examTemplateId = Convert.ToInt32(ddlExamTemplateName.SelectedValue);
                ExamTemplate examTemplateObj = ExamTemplateManager.GetById(examTemplateId);
                if (examTemplateObj != null)
                {
                    txtTemplateMark.Text = Convert.ToString(examTemplateObj.ExamTemplateMarks);
                    LoadGvExamTemplateItem(examTemplateId);
                    ResetRadioButtonValue();
                }
                else
                {
                    Reset();
                }

            }
            catch (Exception)
            {
                throw;
            }


        }

        private void ResetRadioButtonValue()
        {
            rdbVivaFalse.Checked = false;
            rdbContinousAssesmentFalse.Checked = true;
            rdbIsFinalExamTrue.Checked = false;
            rdbIsFinalExamFalse.Checked = false;
            rdbIsSingleQuestionTrue.Checked = false;
            rdbIsSingleQuestionFalse.Checked = false;
            rdbShowAllContinuousInSubTotalTrue.Checked = false;
            rdbShowAllContinuousInSubTotalFalse.Checked = false;
            rdbShowAllInGrandTotalTrue.Checked = false;
            rdbShowAllInGrandTotalFalse.Checked = false;
            multipleExamDropDownList.SelectedValue = "-1";
            ddlNoOfExaminer.SelectedValue = "-1";
        }

        public void Reset()
        {
            GvExamTemplateItem.DataSource = null;
            GvExamTemplateItem.DataBind();
            ClearAllTextBox();
            AddButton.Visible = true;
            UpdateButton.Visible = false;
            CancelButton.Visible = false;
        }

        protected void GvExamTemplateItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int examTemplateItemId = Convert.ToInt16(e.CommandArgument);
            ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
            examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
            hdnExamTemplateItemId.Value = Convert.ToString(examTemplateItemId);

            if (e.CommandName == "ExamTemplateItemEdit")
            {
                ResetRadioButtonValue();
                ddlExamTemplateName.Enabled = false;
                LoadExamTemplateItem(examTemplateItemObj);
                UpdateButton.Visible = true;
                CancelButton.Visible = true;
                AddButton.Visible = false;
            }
            if (e.CommandName == "ExamTemplateItemDelete")
            {
                ExamTemplateItemManager.Delete(examTemplateItemId);
                int examTemplateId = Convert.ToInt32(ddlExamTemplateName.SelectedValue);
                LoadGvExamTemplateItem(examTemplateId);
            }
        }

        private void LoadExamTemplateItem(ExamTemplateItem examTemplateItemObj)
        {
            ddlExamTemplateName.SelectedValue = Convert.ToString(examTemplateItemObj.ExamTemplateId);
            ExamTemplate examTemplateObj = ExamTemplateManager.GetById(examTemplateItemObj.ExamTemplateId);
            txtTemplateMark.Text = Convert.ToString(examTemplateObj.ExamTemplateMarks);
            txtExamName.Text = Convert.ToString(examTemplateItemObj.ExamName);
            txtExamMark.Text = Convert.ToString(examTemplateItemObj.ExamMark);
            txtSequence.Text = Convert.ToString(examTemplateItemObj.ColumnSequence);
            txtPrintSequence.Text = Convert.ToString(examTemplateItemObj.PrintColumnSequence);
            txtExamPassMark.Text = Convert.ToString(examTemplateItemObj.PassMark);
            int columnStatus = Convert.ToInt32(examTemplateItemObj.ColumnType);
            int calculationStatus = Convert.ToInt32(examTemplateItemObj.CalculationType);
            bool showInTabulation = Convert.ToBoolean(examTemplateItemObj.ShowInTabulation);


            if (examTemplateItemObj.Attribute1=="Continuous")
            {
                rdbContinousAssesmentTrue.Checked = true;
                rdbContinousAssesmentFalse.Checked = false;
            }
            else
            {
                rdbContinousAssesmentFalse.Checked = true;
                rdbContinousAssesmentTrue.Checked = false;
            }

            if (examTemplateItemObj.Attribute2 == "Viva")
            {
                rdbVivaTrue.Checked = true;
                rdbVivaFalse.Checked = false;
            }
            else
            {
                rdbVivaFalse.Checked = true;
                rdbVivaTrue.Checked = false;
            }

            if (examTemplateItemObj.IsFinalExam == true)
            {
                rdbIsFinalExamTrue.Checked = true;
            }
            else
            {
                rdbIsFinalExamFalse.Checked = true;
            }

            if (examTemplateItemObj.SingleQuestionAnswer == true)
            {
                rdbIsSingleQuestionTrue.Checked = true;
            }
            //else
            //{
            //    rdbIsSingleQuestionFalse.Checked = true;
            //}
            if (examTemplateItemObj.ShowAllContinuousInSubTotal == true)
            {
                rdbShowAllContinuousInSubTotalTrue.Checked = true;
            }
            //else
            //{
            //    rdbShowAllContinuousInSubTotalFalse.Checked = true;
            //}
            if (examTemplateItemObj.ShowAllInGrandTotal == true)
            {
                rdbShowAllInGrandTotalTrue.Checked = true;
            }
            //else
            //{
            //    rdbShowAllInGrandTotalFalse.Checked = true;
            //}
            if (examTemplateItemObj.NumberOfExaminer > 0)
            {
                ddlNoOfExaminer.SelectedValue = examTemplateItemObj.NumberOfExaminer.ToString();
            }
            else
            {
                ddlNoOfExaminer.SelectedValue = "-1";
            }

            if (examTemplateItemObj.MultipleExaminer > 0)
            {
                multipleExamDropDownList.SelectedValue = examTemplateItemObj.MultipleExaminer.ToString();
            }
            else
            {
                multipleExamDropDownList.SelectedValue = "-1";
            }
            if (showInTabulation == true)
            {
                rdbTrue.Checked = true;
                rdbFalse.Checked = false;
                pnlTabulationShow.Visible = true;
                txtTabulationTitle.Text = Convert.ToString(examTemplateItemObj.TabulationTitle);
            }
            else
            {
                rdbFalse.Checked = true;
                rdbTrue.Checked = false;
                pnlTabulationShow.Visible = false;
                txtTabulationTitle.Text = string.Empty;
            }

            if (columnStatus == 1)
            {
                pnlRelationalColumn.Visible = false;
                pnlColumnType.Visible = false;
                pnlPercentage.Visible = false;
                rdbBasicColumn.Checked = true;
                rdbCalculativeColumn.Checked = false;
                rdbGradeColumn.Checked = false;
                txtExamMark.Enabled = true;
            }
            else if (columnStatus == 2)
            {
                rdbBasicColumn.Checked = false;
                rdbCalculativeColumn.Checked = true;
                rdbGradeColumn.Checked = false;
                txtExamMark.Text = null;
                txtExamMark.Enabled = false;
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                LoadCalCulativeColumnDetail(examTemplateItemObj);
                if (calculationStatus == 1)
                {
                    rdbPercentage.Checked = true;
                    rdbAverage.Checked = false;
                    rdbSum.Checked = false;
                    rdbBestOne.Checked = false;
                    rdbBestTwo.Checked = false;
                    rdbBestThree.Checked = false;
                    pnlPercentage.Visible = true;
                    txtMultiplyBy.Text = Convert.ToString(examTemplateItemObj.MultiplyBy);
                    txtDivideBy.Text = Convert.ToString(examTemplateItemObj.DivideBy);
                }
                if (calculationStatus == 2)
                {
                    rdbPercentage.Checked = false;
                    rdbAverage.Checked = true;
                    rdbSum.Checked = false;
                    rdbBestOne.Checked = false;
                    rdbBestTwo.Checked = false;
                    rdbBestThree.Checked = false;
                }
                if (calculationStatus == 3)
                {
                    rdbPercentage.Checked = false;
                    rdbAverage.Checked = false;
                    rdbSum.Checked = true;
                    rdbBestOne.Checked = false;
                    rdbBestTwo.Checked = false;
                    rdbBestThree.Checked = false;
                }
                if (calculationStatus == 4)
                {
                    rdbPercentage.Checked = false;
                    rdbAverage.Checked = false;
                    rdbSum.Checked = false;
                    rdbBestOne.Checked = true;
                    rdbBestTwo.Checked = false;
                    rdbBestThree.Checked = false;
                }
                if (calculationStatus == 5)
                {
                    rdbPercentage.Checked = false;
                    rdbAverage.Checked = false;
                    rdbSum.Checked = false;
                    rdbBestOne.Checked = false;
                    rdbBestTwo.Checked = true;
                    rdbBestThree.Checked = false;
                }
                if (calculationStatus == 6)
                {
                    rdbPercentage.Checked = false;
                    rdbAverage.Checked = false;
                    rdbSum.Checked = false;
                    rdbBestOne.Checked = false;
                    rdbBestTwo.Checked = false;
                    rdbBestThree.Checked = true;
                }
            }
            else
            {
                pnlRelationalColumn.Visible = true;
                pnlColumnType.Visible = false;
                pnlPercentage.Visible = false;
                rdbBasicColumn.Checked = false;
                rdbCalculativeColumn.Checked = false;
                rdbGradeColumn.Checked = true;
                txtExamMark.Text = null;
                txtExamMark.Enabled = false;
            }
        }

        private void LoadCalCulativeColumnDetail(ExamTemplateItem examTemplateItemObj)
        {
            List<ExamMarkEquationColumnOrder> examMarkEquationColumnOrderList = ExamMarkEquationColumnOrderManager.GetByTemplateItemId(examTemplateItemObj.ExamTemplateItemId);
            for (int i = 0; i < examMarkEquationColumnOrderList.Count; i++)
            {
                ExamMarkEquationColumnOrder examEquationColumnOrder = new ExamMarkEquationColumnOrder();
                examEquationColumnOrder = examMarkEquationColumnOrderList[i];

                if (i == 0)
                {
                    txtColumnNo1.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 1)
                {
                    txtColumnNo2.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }

                else if (i == 2)
                {
                    txtColumnNo3.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 3)
                {
                    txtColumnNo4.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 4)
                {
                    txtColumnNo5.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 5)
                {
                    txtColumnNo6.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 6)
                {
                    txtColumnNo7.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 7)
                {
                    txtColumnNo8.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
                else if (i == 8)
                {
                    txtColumnNo9.Text = Convert.ToString(examEquationColumnOrder.SumColumnNo);
                }
            }
        }

        protected void rdbTrue_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTrue.Checked)
            {
                pnlTabulationShow.Visible = true;
                //pnlBestExam.Visible = false;
            }
        }

        protected void rdbFalse_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFalse.Checked)
            {
                pnlTabulationShow.Visible = false;
                //pnlBestExam.Visible = false;
            }
        }

        protected void rdbBestOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBestOne.Checked)
            {
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
            }
        }

        protected void rdbBestTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBestTwo.Checked)
            {
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
            }
        }

        protected void rdbBestThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBestThree.Checked)
            {
                pnlColumnType.Visible = true;
                pnlRelationalColumn.Visible = true;
                pnlPercentage.Visible = false;
            }
        }

        public void LoadThirdExaminerDropDownList()
        {
            foreach (int value in Enum.GetValues(typeof(CommonEnum.ThirdExaminarExamination)))
            {
                multipleExamDropDownList.Items.Add(new ListItem(Enum.GetName(typeof(CommonEnum.ThirdExaminarExamination), value), value.ToString()));
            }

            //int calculationType = (int)CommonUtility.CommonEnum.ThirdExaminarExamination.Final;


            //Array itemValues = System.Enum.GetValues(typeof(CommonEnum.ThirdExaminarExamination));
            //Array itemNames = System.Enum.GetNames(typeof(CommonEnum.ThirdExaminarExamination));

            //for (int i = 0; i <= itemNames.Length - 1; i++)
            //{
            //    ListItem item = new ListItem(itemNames[i], itemValues[i]);
            //    thirdExamDropDownList.Items.Add(item);
            //}
            //thirdExamDropDownList.DataSource = 
        }

    }
}