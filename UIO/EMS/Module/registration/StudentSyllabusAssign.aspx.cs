using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.registration
{
    public partial class StudentSyllabusAssign : BasePage
    {
        int userId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string loginID = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
            User user = UserManager.GetByLogInId(loginID);
            if (user != null)
                userId = user.User_ID;
            base.CheckPage_Load();

            if (!IsPostBack && !IsCallback)
            {
                ucDepartment.LoadDropDownList();
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                ddlCourseTree.Items.Add(new ListItem("-Select-", "0"));
                ddlLinkedCalendars.Items.Add(new ListItem("-Select-", "0"));
            }
        }

        private void FillCourseTreeCombo(int programID)
        {
            ddlCourseTree.Items.Clear();
            List<TreeMaster> treeMasterList = TreeMasterManager.GetAllProgramID(programID);
            ddlCourseTree.AppendDataBoundItems = true;

            if (treeMasterList != null && treeMasterList.Count > 0)
            {
                ddlCourseTree.DataSource = treeMasterList.OrderBy(d => d.TreeMasterID).ToList();
                ddlCourseTree.DataBind();

                FillLinkedCalendars(treeMasterList[0].TreeMasterID);
            }
        }

        private void FillLinkedCalendars(int treeMasterID)
        {
            ddlLinkedCalendars.Items.Clear();
            List<TreeCalendarMaster> treeCalendarMasterList = TreeCalendarMasterManager.GetAllByTreeMasterID(treeMasterID);
            ddlLinkedCalendars.AppendDataBoundItems = true;

            if (treeCalendarMasterList != null)
            {
                ddlLinkedCalendars.DataSource = treeCalendarMasterList.OrderBy(d => d.TreeCalendarMasterID).ToList();
                ddlLinkedCalendars.DataBind();
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

            int programID = Convert.ToInt32(ucProgram.selectedValue);
            int acacalId = Convert.ToInt32(ucSession.selectedValue);

            FillCourseTreeCombo(programID);

            List<Student> studentList = StudentManager.GetAllByProgramIdSessionId(programID, acacalId);
            if (studentList != null && studentList.Count > 0)
            {
                gvwCollection.Visible = true;

                gvwCollection.DataSource = studentList.OrderBy(x => x.Roll);
                gvwCollection.DataBind();
                gvwCollection.Columns[2].Visible = false;
            }
            else
            {
                gvwCollection.Visible = true;

                gvwCollection.DataSource = null;
                gvwCollection.DataBind();
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            ucProgram.LoadDropdownByDepartmentId(departmentId);
        }
        protected void ucSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            gvwCollection.Visible = false;
            ddlCourseTree.Items.Clear();
            ddlCourseTree.Items.Add(new ListItem("-Select-", "0"));
            ddlLinkedCalendars.Items.Clear();
            ddlLinkedCalendars.Items.Add(new ListItem("-Select-", "0"));

            //ucBatch.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
        }

        protected void OnBatchSelectedIndexChanged(object sender, EventArgs e)
        {
            gvwCollection.Visible = false;
            ddlCourseTree.Items.Clear();
            ddlCourseTree.Items.Add(new ListItem("-Select-", "0"));
            ddlLinkedCalendars.Items.Clear();
            ddlLinkedCalendars.Items.Add(new ListItem("-Select-", "0"));
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ch = (CheckBox)sender;
            GridViewRow row = (GridViewRow)ch.NamingContainer;
            ch.Checked = ((CheckBox)sender).Checked;

            HiddenField hfStudent = (HiddenField)(row.Cells[0].FindControl("hfStudentID"));
            HiddenField hfTreeMaster = (HiddenField)(row.Cells[0].FindControl("hfTreeMasterID"));
            HiddenField hfTreeCalendarMaster = (HiddenField)(row.Cells[0].FindControl("hfTreeCalendarMasterID"));

            if (ch.Checked)
            {
                hfTreeMaster.Value = ddlCourseTree.SelectedItem.Value.ToString();
                hfTreeCalendarMaster.Value = ddlLinkedCalendars.SelectedItem.Value.ToString();

                row.BackColor = System.Drawing.Color.Lime;
                row.Cells[5].Text = ddlCourseTree.SelectedItem.Value.ToString();
                row.Cells[6].Text = ddlLinkedCalendars.SelectedItem.Value.ToString();
                row.Cells[8].Text = ddlCourseTree.SelectedItem.Text + " »» " + ddlLinkedCalendars.SelectedItem.Text;
            }
            else
            {
                hfTreeMaster.Value = string.Empty;
                hfTreeCalendarMaster.Value = string.Empty;

                row.Cells[5].Text = string.Empty;
                row.Cells[6].Text = string.Empty;
                row.Cells[8].Text = string.Empty;
                row.BackColor = System.Drawing.Color.Empty;
            }

        }

        protected void CourseTree_SelectedIndexChanged(object serder, EventArgs e)
        {
            FillLinkedCalendars(Convert.ToInt32(ddlCourseTree.SelectedValue));
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (chk.Checked)
            {
                chk.Text = "Unselect All";
            }
            else
            {
                chk.Text = "Select All";
            }

            foreach (GridViewRow rowItem in gvwCollection.Rows)
            {
                CheckBox ckBox = (CheckBox)rowItem.FindControl("chk");
                ckBox.Checked = chk.Checked;

                HiddenField hfStudent = (HiddenField)(rowItem.Cells[0].FindControl("hfStudentID"));
                HiddenField hfTreeMaster = (HiddenField)(rowItem.Cells[0].FindControl("hfTreeMasterID"));
                HiddenField hfTreeCalendarMaster = (HiddenField)(rowItem.Cells[0].FindControl("hfTreeCalendarMasterID"));
                if (chk.Checked)
                {
                    hfTreeMaster.Value = ddlCourseTree.SelectedItem.Value.ToString();
                    hfTreeCalendarMaster.Value = ddlLinkedCalendars.SelectedItem.Value.ToString();

                    rowItem.Cells[5].Text = ddlCourseTree.SelectedItem.Value.ToString();
                    rowItem.Cells[6].Text = ddlLinkedCalendars.SelectedItem.Value.ToString();
                    rowItem.Cells[8].Text = ddlCourseTree.SelectedItem.Text + " »» " + ddlLinkedCalendars.SelectedItem.Text;
                    rowItem.BackColor = System.Drawing.Color.Lime;
                }
                else
                {
                    hfTreeMaster.Value = string.Empty;
                    hfTreeCalendarMaster.Value = string.Empty;

                    rowItem.Cells[5].Text = string.Empty;
                    rowItem.Cells[6].Text = string.Empty;
                    rowItem.Cells[8].Text = string.Empty;
                    rowItem.BackColor = System.Drawing.Color.Empty;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvwCollection.Rows.Count; i++)
            {
                Student student = new Student();
                int id = 0;
                CheckBox chk = (CheckBox)gvwCollection.Rows[i].Cells[0].FindControl("chk");
                if (chk.Checked)
                {
                    HiddenField hfStudent = (HiddenField)gvwCollection.Rows[i].FindControl("hfStudentID");
                    HiddenField hfTreeMaster = (HiddenField)gvwCollection.Rows[i].FindControl("hfTreeMasterID");
                    HiddenField hfTreeCalendarMaster = (HiddenField)gvwCollection.Rows[i].FindControl("hfTreeCalendarMasterID");

                    id = Int32.Parse(hfStudent.Value);
                    student = StudentManager.GetById(id);
                    student.TreeMasterID = Int32.Parse(hfTreeMaster.Value);
                    student.TreeCalendarMasterID = Int32.Parse(hfTreeCalendarMaster.Value);
                    student.ModifiedBy = 0;
                    student.ModifiedDate = DateTime.Now;

                    bool result = StudentManager.Update(student);
                }
            }
            showAlert("Course Tree Assign Complete.");
            btnLoad_Click(null, null);
        }

        protected void ddlAcaCalBatch_Change(object sender, EventArgs e)
        {
            gvwCollection.DataSource = null;
            gvwCollection.DataBind();

            ddlCourseTree.Items.Clear();
            ddlLinkedCalendars.Items.Clear();
        }

        protected void ddlProgram_Change(object sender, EventArgs e)
        {
            gvwCollection.DataSource = null;
            gvwCollection.DataBind();

            ddlCourseTree.Items.Clear();
            ddlLinkedCalendars.Items.Clear();
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }




    }
}