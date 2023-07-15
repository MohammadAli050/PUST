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
    public partial class ExamTemplateManage : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                LoadExamTemplateGird();
                lblMsg.Text = string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.mp1.Show();
                ExamTemplate examTemplate = new ExamTemplate();
                examTemplate.ExamTemplateName = txtExamTemplateName.Text;
                examTemplate.ExamTemplateMarks = Convert.ToInt32(txtExamTemplateMark.Text);
                examTemplate.CreatedBy = userObj.Id;
                examTemplate.CreatedDate = DateTime.Now;
                examTemplate.ModifiedBy = userObj.Id;
                examTemplate.ModifiedDate = DateTime.Now;

                if (ExamTemplateManager.GetExamTemplateByName(examTemplate.ExamTemplateName))
                {
                    int result = ExamTemplateManager.Insert(examTemplate);

                    if (result > 0)
                    {
                        lblMsg.Text = "Exam template created successfully.";
                        LoadExamTemplateGird();
                    }
                    else
                    {
                        lblMsg.Text = "Exam template could not created.";
                    }
                }
                else
                {
                    lblMsg.Text = "Exam template already exist.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.mp1.Show();
                int examTemplateId = Convert.ToInt32(btnUpdate.CommandArgument);
                ExamTemplate examTemplate = new ExamTemplate();
                examTemplate = ExamTemplateManager.GetById(examTemplateId);
                examTemplate.ExamTemplateName = txtExamTemplateName.Text;
                examTemplate.ExamTemplateMarks = Convert.ToInt32(txtExamTemplateMark.Text);
                examTemplate.ModifiedBy = userObj.Id;
                examTemplate.ModifiedDate = DateTime.Now;

                ExamTemplate newExamTemplateObj = new ExamTemplate();
                newExamTemplateObj = ExamTemplateManager.GetById(examTemplateId);
                if (examTemplate.ExamTemplateName == newExamTemplateObj.ExamTemplateName)
                {
                    if (ExamTemplateManager.Update(examTemplate))
                    {
                        lblMsg.Text = "Course exam template edited successfully.";
                        LoadExamTemplateGird();
                    }
                    else
                    {
                        lblMsg.Text = "Course exam template could not edited.";
                    }
                }
                else
                {
                    if (ExamTemplateManager.GetExamTemplateByName(examTemplate.ExamTemplateName))
                    {
                        if (ExamTemplateManager.Update(examTemplate))
                        {
                            lblMsg.Text = "Course exam template edited successfully.";
                            LoadExamTemplateGird();
                        }
                        else
                        {
                            lblMsg.Text = "Course exam template could not edited.";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Course exam template already exist.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.mp1.Hide();
            btnUpdate.Visible = false;
            btnClose.Visible = true;
            btnSave.Visible = true;
        }

        protected void GvExamTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int examTemplateId = Convert.ToInt32(e.CommandArgument);
            ExamTemplate examTemplateObj = new ExamTemplate();
            examTemplateObj = ExamTemplateManager.GetById(examTemplateId);
            HiddenExamTemplateId.Value = Convert.ToString(examTemplateId);

            if (e.CommandName == "ExamTemplateEdit")
            {
                this.mp1.Show();
                LoadAllTextBox(examTemplateObj);
                btnUpdate.Visible = true;
                btnUpdate.CommandArgument = Convert.ToString(examTemplateId);
                btnClose.Visible = true;
                btnSave.Visible = false;
            }
            if (e.CommandName == "ExamTemplateDelete")
            {
                ExamTemplateManager.Delete(examTemplateId);
                LoadExamTemplateGird();
            }
        }

        public void LoadExamTemplateGird()
        {
            GvExamTemplate.DataSource = ExamTemplateManager.GetAll();
            GvExamTemplate.DataBind();
        }

        private void LoadAllTextBox(ExamTemplate examTemplateObj)
        {
            txtExamTemplateName.Text = examTemplateObj.ExamTemplateName;
            txtExamTemplateMark.Text = Convert.ToString(examTemplateObj.ExamTemplateMarks);
        }

        protected void GvExamTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvExamTemplate.PageIndex = e.NewPageIndex;
            LoadExamTemplateGird();
        }
    }
}