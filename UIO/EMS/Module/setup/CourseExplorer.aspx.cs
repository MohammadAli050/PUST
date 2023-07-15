using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data.SqlClient;
using System.Drawing;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessLogic;
using CommonUtility;
using System.IO;
using OfficeOpenXml;

namespace EMS.SyllabusMan
{
    public partial class CourseExplorer : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.CourseExplorer);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.CourseExplorer));

        BussinessObject.UIUMSUser userObj = null;

        private const string SESSION_OLDCOURSE = "SESSION_OLDCOURSE";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.CheckPage_Load();
                userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            
                if (!IsPostBack)
                {
                    btnSaveOrUpdate.Enabled = false;
                    ucDepartment.LoadDropDownList();
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string txtFormalCode = searchFormalCode.Text.Trim();
                string txtTitle = searchTitle.Text.Trim();
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                if (programId > 0)
                {
                    List<Course> _courseList = CourseManager.GetAllByProgram(programId);

                    List<Course> _courseList2 = new List<Course>();
                    foreach (Course c in _courseList)
                    {
                        if (string.IsNullOrEmpty(c.Title))
                        {
                            c.Title = ".";
                        }
                        _courseList2.Add(c);
                    }

                    if (txtFormalCode != "")
                    {
                        _courseList2 = _courseList2.Where(s => s.FormalCode.Contains(txtFormalCode)).ToList();
                    }
                    if (txtTitle != "")
                    {
                        _courseList2 = _courseList2.Where(s => s.Title.Contains(txtTitle)).ToList();
                    }

                    if (_courseList2 != null && _courseList2.Count > 0)
                    {
                        ShowMessage("", Color.Red);

                        gvCourselists.DataSource = _courseList2;
                        gvCourselists.DataBind();
                    }
                    else
                    {
                        ShowMessage("No Data Found or Error in Data Loading", Color.Red);
                    }
                }
                else
                {
                    ShowMessage("Please select a program", Color.Red);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Color.Red);
            }
        }

        protected void btnAddNewCourse_Click(object sender, EventArgs e)
        {
            ClearAll();
            btnSaveOrUpdate.Text = "Save";
            List<TypeDefinition> tdlist = TypeDefinitionManager.GetAll().Where(d => d.Type == "Course").ToList();

            FillTypeComboBox((int)0);
            FillProgramDropDown();
            //txtCourseID.ReadOnly = false;
            //txtVersionID.ReadOnly = false;

            ddlIsActive.SelectedValue = "1";
            ddlMultiple.SelectedValue = "0";
            ModalPopupExtender1.Show();
        }

        private void FillProgramDropDown()
        {
            List<Program> programlist = ProgramManager.GetAll();
            ddlProgram.Items.Clear();
            ddlProgram.Items.Add(new ListItem("-Select-", "0"));
            ddlProgram.AppendDataBoundItems = true;

            if (programlist != null)
            {
                ddlProgram.DataSource = programlist.OrderBy(d => d.ProgramID).ToList();
                ddlProgram.DataTextField = "ShortName";
                ddlProgram.DataValueField = "ProgramID";
                ddlProgram.DataBind();
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            ClearAll();
            FillProgramDropDown();
            btnSaveOrUpdate.Text = "Update";
            btnSaveOrUpdate.Enabled = true;
            LinkButton btn = (LinkButton)sender;
            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int CourseID = Convert.ToInt32(commandArgs[0]);
            int VersionID = Convert.ToInt32(commandArgs[1]);

            Course _course = CourseManager.GetByCourseIdVersionId(CourseID, VersionID);
            var courseExtend = CourseExtendOneManager.GetByCourseIdVersionId(CourseID, VersionID);

            SessionManager.SaveObjToSession<string>(_course.FormalCode, SESSION_OLDCOURSE);

            hdnCourseId.Value = _course.CourseID.ToString();
            hdnVersionId.Value = _course.VersionID.ToString();
            ddlProgram.SelectedValue = _course.ProgramID.ToString();
            txtVersionCode.Text = _course.VersionCode.ToString();
            txtFormalCode.Text = _course.FormalCode.ToString();
            txtTranscriptCode.Text = string.IsNullOrEmpty(_course.TranscriptCode) ? "" : _course.TranscriptCode.ToString();
            txtCourseGroup.Text = string.IsNullOrEmpty(_course.CourseGroup) ? "" : _course.CourseGroup.ToString();
            txtCredits.Text = _course.Credits.ToString();
            txtTitle.Text = _course.Title.ToString();
            txtCourseContent.Text = _course.CourseContent == null ? "" : _course.CourseContent.ToString();
            txtMarks.Text = courseExtend != null ? courseExtend.Marks.ToString() : string.Empty;
            //txtCourseID.ReadOnly = true;
            //txtVersionID.ReadOnly = true;

            FillTypeComboBox((int)_course.TypeDefinitionID);

            ddlMultiple.SelectedValue = _course.HasMultipleACUSpan == true ? "1" : "0";
            ddlIsActive.SelectedValue = _course.IsActive == true ? "1" : "0";
            ModalPopupExtender1.Show();


        }

        private void FillTypeComboBox(int id)
        {
            try
            {
                List<TypeDefinition> list = TypeDefinitionManager.GetAll("Course");
                

                if (list != null)
                {
                    ddlCourseType.DataSource = list.OrderBy(d => d.TypeDefinitionID).ToList();
                    ddlCourseType.DataTextField = "Definition";
                    ddlCourseType.DataValueField = "TypeDefinitionID";
                    ddlCourseType.DataBind();
                }

                if (id != 0)
                {
                    //ddlCourseType.SelectedValue = id + "";
                }
                else
                {
                    //ddlCourseType.SelectedValue = "5";
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Color.Red);
            }
            finally { }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int CourseID = Convert.ToInt32(commandArgs[0]);
            int VersionID = Convert.ToInt32(commandArgs[1]);

            btnLoad_Click(null, null);
        }

        protected void btnSaveOrUpdate_Click(object sender, EventArgs e)
        {
            int pass = 0;
            try
            {

                //int CourseId = Convert.ToInt32(txtCourseID.Text);
                //int VersionID = Convert.ToInt32(txtVersionID.Text);
                string FormalCode = txtFormalCode.Text.Trim();
                string TranscriptCode = txtTranscriptCode.Text.Trim();
                string CourseGroup = txtCourseGroup.Text.Trim();
                string VersionCode = txtVersionCode.Text.Trim();
                decimal credit = Convert.ToDecimal(txtCredits.Text.Trim());
                string Title = txtTitle.Text.Trim();
                string CourseContent = txtCourseContent.Text.Trim();

                pass = 1;

                Course _course;
                if (btnSaveOrUpdate.Text == "Save")
                {
                    _course = new Course();
                }
                else
                {
                    _course = CourseManager.GetByCourseIdVersionId(Convert.ToInt32(hdnCourseId.Value), Convert.ToInt32(hdnVersionId.Value));
                }

                //_course.CourseID = 0;
                //_course.VersionID = 0;
                _course.FormalCode = FormalCode;
                _course.TranscriptCode = TranscriptCode;
                _course.CourseGroup = CourseGroup;
                _course.VersionCode = VersionCode;
                _course.ProgramID = Convert.ToInt32(ddlProgram.SelectedValue);
                _course.Credits = credit;
                _course.Title = Title;
                _course.CourseContent = CourseContent;
                _course.IsActive = ddlIsActive.SelectedItem.Value == "1" ? true : false;
                _course.HasMultipleACUSpan = ddlMultiple.SelectedItem.Value == "1" ? true : false;
                _course.TypeDefinitionID = Convert.ToInt32(ddlCourseType.SelectedItem.Value);
                //_course.BillTypeDefinitionID = 10;

                if (btnSaveOrUpdate.Text == "Save")
                {
                    _course.CreatedBy = BaseCurrentUserObj.Id;
                    _course.CreatedDate = DateTime.Now;
                    //_course.ModifiedBy = BaseCurrentUserObj.Id;
                    //_course.ModifiedDate = DateTime.Now;
                    try
                    {
                        //  Course cours = CourseManager.GetByCourseIdVersionId(_course.CourseID, _course.VersionID);

                        List<Course> coursList = CourseManager.GetAllByVersionCode(_course.VersionCode);

                        //BussinessObject.Course.IsExist(_course.FormalCode);

                        if (coursList.Count() == 0)
                        {
                            int id = CourseManager.Insert(_course);
                            if (id != 0)
                            {
                                decimal marks = 0;
                                CourseExtendInsertUpdate(id, 1, out marks);

                                ShowMessage("Successfully Saved", Color.Green);

                                #region Log Insert 
                                LogGeneralManager.Insert(
                                     DateTime.Now,
                                     "",
                                     BaseAcaCalCurrent.FullCode,
                                     BaseCurrentUserObj.LogInID,
                                     _course.FormalCode,
                                     _course.VersionCode,
                                     "Insert Course",
                                     BaseCurrentUserObj.LogInID + " is insert course Title : " + _course.Title + "; Credits : " + _course.Credits.ToString() + "; Transcript Code : " + _course.TranscriptCode + "; Course Mark : " + marks.ToString(),
                                     "normal",
                                      ((int)CommonEnum.PageName.CourseExplorer).ToString(),
                                     CommonEnum.PageName.CourseExplorer.ToString(),
                                     _pageUrl,
                                     "");
                                #endregion
                            }
                            else
                            {
                                ShowMessage("Error in Saving !", Color.Red);
                            }
                        }
                        else
                        {
                            ModalPopupExtender1.Show();
                            ShowPopUpMessage("Version Code is already Exist !", Color.Red);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, Color.Red);
                    }
                }
                else
                {
                    try
                    {
                        _course.ModifiedBy = BaseCurrentUserObj.Id;
                        _course.ModifiedDate = DateTime.Now;

                        List<Course> coursList = new List<Course>();
                        if (_course.FormalCode != SessionManager.GetObjFromSession<string>(SESSION_OLDCOURSE))
                        {
                            coursList = CourseManager.GetAllByVersionCode(_course.VersionCode);
                        }

                        if (coursList.Count() == 0)
                        {
                            bool result = CourseManager.Update(_course);
                            if (result)
                            {
                                decimal marks = 0;
                                CourseExtendInsertUpdate(_course.CourseID, _course.VersionID, out marks);

                                ShowMessage("Successfully Updated", Color.Green);

                                #region Log Insert
                                LogGeneralManager.Insert(
                                    DateTime.Now,
                                    "",
                                    BaseAcaCalCurrent.FullCode,
                                    BaseCurrentUserObj.LogInID,
                                    _course.FormalCode,
                                    _course.VersionCode,
                                    "Update Course",
                                    BaseCurrentUserObj.LogInID + " is updated course Title : " + _course.Title + "; Credits : " + _course.Credits.ToString() + "; Transcript Code : " + _course.TranscriptCode + " ; Course Mark : " + marks.ToString(),
                                    "normal",
                                     ((int)CommonEnum.PageName.CourseExplorer).ToString(),
                                    CommonEnum.PageName.CourseExplorer.ToString(),
                                    _pageUrl,
                                    "");
                                #endregion
                            }
                            else
                            {
                                ShowMessage("Error in Updating !", Color.Red);
                            }
                        }
                        else
                        {                            
                            ModalPopupExtender1.Show();
                            ShowPopUpMessage("Version Code is already Exist !", Color.Red);
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, Color.Red);
                    }
                }


                searchFormalCode.Text = FormalCode;
                btnLoad_Click(null, null);

            }
            catch (Exception ex)
            {
                if (pass == 0)
                {
                    ShowPopUpMessage("Invalid Entry . Please Enter Info Properly", Color.Red);
                }
                else
                {
                    ShowPopUpMessage(ex.Message, Color.Red);
                }

                ModalPopupExtender1.Show();
            }
        }

        private void CourseExtendInsertUpdate(int courseId, int versionId, out decimal courseMarks)
        {
            var courseExtend = CourseExtendOneManager.GetByCourseIdVersionId(courseId, versionId);

            courseMarks = 0;
            decimal mark = 0;
            bool isSuccess = decimal.TryParse(txtMarks.Text, out mark);

            if (courseExtend != null)
            {
                courseExtend.Marks = isSuccess ? mark : 0;
                courseExtend.ModifiedBy = BaseCurrentUserObj.Id;
                courseExtend.ModifiedDate = DateTime.Now;

                bool isUpdate = CourseExtendOneManager.Update(courseExtend);
                courseMarks = isUpdate ? mark : 0;
            }
            else
            {
                courseExtend = new CourseExtendOne();

                courseExtend.CourseId = courseId;
                courseExtend.VersionId = versionId;
                courseExtend.Marks = isSuccess ? mark : 0;
                courseExtend.CreatedBy = BaseCurrentUserObj.Id;
                courseExtend.CreatedDate = DateTime.Now;

                int isInsert = CourseExtendOneManager.Insert(courseExtend);
                courseMarks = isInsert > 0 ? mark : 0;
            }                       
        }

        protected void gvStudentBillView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string txtFormalCode = searchFormalCode.Text.Trim();
                string txtTitle = searchTitle.Text.Trim();

                List<Course> _courseList = CourseManager.GetAll();

                List<Course> _courseList2 = new List<Course>();
                foreach (Course c in _courseList)
                {
                    if (string.IsNullOrEmpty(c.Title))
                    {
                        c.Title = ".";
                    }
                    _courseList2.Add(c);
                }

                if (txtFormalCode != "")
                {
                    _courseList2 = _courseList2.Where(s => s.FormalCode.Contains(txtFormalCode)).ToList();
                }
                if (txtTitle != "")
                {
                    _courseList2 = _courseList2.Where(s => s.Title.Contains(txtTitle)).ToList();
                }

                string sortdirection = string.Empty;
                if (Session["direction"] != null)
                {
                    if (Session["direction"].ToString() == "ASC")
                    {
                        sortdirection = "DESC";
                    }
                    else
                    {
                        sortdirection = "ASC";
                    }
                }
                else
                {
                    sortdirection = "DESC";
                }
                Session["direction"] = sortdirection;
                Sort(_courseList2, e.SortExpression.ToString(), sortdirection);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void Sort(List<Course> list, String sortBy, String sortDirection)
        {
            if (sortDirection == "ASC")
            {
                list.Sort(new GenericComparer<Course>(sortBy, (int)SortDirection.Ascending));
            }
            else
            {
                list.Sort(new GenericComparer<Course>(sortBy, (int)SortDirection.Descending));
            }
            gvCourselists.DataSource = list;
            gvCourselists.DataBind();
        }

        #endregion

        #region Methods

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        private void ShowMessage(string message, Color color)
        {
            lblMessage.Text = string.Empty;
            lblMessage.Text = message;
            lblMessage.ForeColor = color;
        }

        private void ShowPopUpMessage(string message, Color color)
        {
            lblPopUpMassege.Text = string.Empty;
            lblPopUpMassege.Text = message;
            lblPopUpMassege.ForeColor = color;
        }

        private void ClearAll()
        {
            //txtCourseID.Text = "";
            //txtVersionID.Text = "";
            txtFormalCode.Text = "";
            txtVersionCode.Text = "";
            txtCourseContent.Text = "";
            txtTranscriptCode.Text = "";
            txtCourseGroup.Text = "";
            txtCredits.Text = "";
            txtTitle.Text = "";
            ddlIsActive.SelectedValue = "0";
            ddlMultiple.SelectedValue = "0";
            lblMessage.Text = "";
            lblPopUpMassege.Text = "";


        }

        #endregion

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string txtFormalCode = searchFormalCode.Text.Trim();
                string txtTitle = searchTitle.Text.Trim();

                List<Course> _courseList = CourseManager.GetAll();

                List<Course> _courseList2 = new List<Course>();
                foreach (Course c in _courseList)
                {
                    if (string.IsNullOrEmpty(c.Title))
                    {
                        c.Title = ".";
                    }
                    _courseList2.Add(c);
                }

                if (txtFormalCode != "")
                {
                    _courseList2 = _courseList2.Where(s => s.FormalCode.Contains(txtFormalCode)).ToList();
                }
                if (txtTitle != "")
                {
                    _courseList2 = _courseList2.Where(s => s.Title.Contains(txtTitle)).ToList();
                }

                if (_courseList2 != null && _courseList2.Count > 0)
                {

                    string filepath = Server.MapPath(@"~\Upload");
                    HttpFileCollection uploadedFiles = Request.Files;
                    string fileName = "CourseExplorer";
                    fileName += DateTime.Now.ToString("dd-MM-yy") + ".xlsx";

                    filepath = filepath + "\\" + fileName;
                    FileInfo newFile = new FileInfo(filepath);

                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(fileName);

                        int k = 2;
                        worksheet.Cell(1, 1).Value = "Formal Code";
                        worksheet.Cell(1, 2).Value = "Version Code";
                        worksheet.Cell(1, 3).Value = "Title";
                        worksheet.Cell(1, 4).Value = "Credits";
                        worksheet.Cell(1, 5).Value = "Transcript Code";
                        worksheet.Cell(1, 6).Value = "Course Group";
                        worksheet.Cell(1, 7).Value = "Thesis/Project";
                        worksheet.Cell(1, 8).Value = "Course Type";

                        for (int i = 0; i < _courseList2.Count; i++)
                        {
                            worksheet.Cell(k, 1).Value = Convert.ToString(_courseList2[i].FormalCode) + " ";
                            worksheet.Cell(k, 2).Value = Convert.ToString(_courseList2[i].VersionCode) + " ";
                            worksheet.Cell(k, 3).Value = Convert.ToString(_courseList2[i].Title) + " ";
                            worksheet.Cell(k, 4).Value = Convert.ToString(_courseList2[i].Credits) + " ";
                            worksheet.Cell(k, 5).Value = Convert.ToString(_courseList2[i].TranscriptCode) + " ";
                            worksheet.Cell(k, 6).Value = Convert.ToString(_courseList2[i].CourseGroup) + " ";
                            worksheet.Cell(k, 7).Value = Convert.ToString(_courseList2[i].HasMultipleACUSpan.ToString()) + " ";
                            worksheet.Cell(k, 8).Value = Convert.ToString(_courseList2[i].CourseType) + " ";
                            k++;
                        }

                        xlPackage.Save();
                        xlPackage.Dispose();
                        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                        response.ClearContent();
                        response.Clear();
                        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
                        response.TransmitFile(filepath);
                        response.Flush();
                        response.Close();
                        //response.End();
                        File.Delete(Server.MapPath("~/Upload/" + fileName));

                    }
                }
                else
                {
                    ShowMessage("No Data Found or Error in Data Loading", Color.Red);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Color.Red);
            }
        }
    }
}