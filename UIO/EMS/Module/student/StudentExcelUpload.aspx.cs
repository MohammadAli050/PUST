using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using LogicLayer.BusinessLogic;
using System.Data;
using LogicLayer.BusinessObjects;

namespace EMS.Module.student
{
    public partial class StudentExcelUpload : BasePage
    {
        FileConversion aFileConverterObj = new FileConversion();
        BussinessObject.UIUMSUser userObj = null;               

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                //ucProgram.LoadDropDownList();
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYearDDL(programId);
                lblMsg.Text = string.Empty;
                //AlertSuccess.Visible = false;
                //AlertError.Visible = false;
            }
        }

        protected void ucDepartment_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //lblMsg.Text = string.Empty;
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //ucBatch.LoadDropDownList(programId);
                LoadYearDDL(programId);
                LoadYearSemesterDDL(0);
                //LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ClearGridView();
                //lblMsg.Text = string.Empty;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //ucBatch.LoadDropDownList(programId);
                LoadYearDDL(programId);
                if (programId == -1)
                {
                    return;
                }
                //LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadYearDDL(int programId)
        {
            List<Year> yearList = new List<Year>();
            yearList = YearManager.GetByProgramId(programId);

            ddlYear.Items.Clear();
            ddlYear.AppendDataBoundItems = true;

            if (yearList != null)
            {
                ddlYear.Items.Add(new ListItem("-Select-", "0"));
                ddlYear.DataTextField = "YearName";
                ddlYear.DataValueField = "YearId";
                if (yearList != null)
                {
                    ddlYear.DataSource = yearList.OrderBy(b => b.YearId).ToList();
                    ddlYear.DataBind();
                }
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            LoadYearSemesterDDL(yearId);
        }

        private void LoadYearSemesterDDL(int yearId)
        {
            List<Semester> semesterList = new List<Semester>();
            semesterList = SemesterManager.GetByYearId(yearId);

            ddlSemester.Items.Clear();
            ddlSemester.AppendDataBoundItems = true;

            if (semesterList != null)
            {
                ddlSemester.Items.Add(new ListItem("-Select-", "0"));
                ddlSemester.DataTextField = "SemesterName";
                ddlSemester.DataValueField = "SemesterId";
                if (semesterList != null)
                {
                    ddlSemester.DataSource = semesterList.OrderBy(b => b.SemesterId).ToList();
                    ddlSemester.DataBind();
                }
            }
        }

        protected void LoadSheet_Click(object sender, EventArgs e)
        {
            try
            {
                if (UploadPanel.HasFile)
                {
                    try
                    {
                        string FileName = Path.GetFileName(UploadPanel.FileName);

                        if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                        {
                            string UploadFileLocation = string.Empty;
                            UploadFileLocation = Server.MapPath("~/Upload/");
                            try
                            {
                                string customFileName = GetCustomFileName();
                                string savePath = UploadFileLocation + customFileName;
                                UploadPanel.SaveAs(savePath);
                                FileNameHiddenField.Value = customFileName;
                                FilePathHiddenField.Value = savePath;
                                LoadSheetNames();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {

                        }

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            lblFileName.Text = Convert.ToString(FileName);
                        }
                        //UploadPanel.Enabled = FileName;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetCustomFileName()
        {
            string examName = "StudentPayment";
            string date = DateTime.Now.Day.ToString() + "_" +
                          DateTime.Now.Year.ToString() + "_" +
                          DateTime.Now.Month.ToString();
            return examName + "_" + date + ".xlsx";
        }

        private void LoadSheetNames()
        {
            try
            {
                string newPath = Convert.ToString(FilePathHiddenField.Value);
                string fileName = Convert.ToString(FileNameHiddenField.Value);
                //List<SheetName> sheetList = aFileConverterObj.ListSheetInExcel(newPath);
                List<LogicLayer.BusinessLogic.SheetName> sheetList = aFileConverterObj.PassFileName(newPath);
                ddlSheetName.DataSource = sheetList;
                ddlSheetName.DataTextField = "Name";
                ddlSheetName.DataValueField = "Id";
                ddlSheetName.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlSheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string newPath = Convert.ToString(FilePathHiddenField.Value);
                string fileName = Convert.ToString(FileNameHiddenField.Value);
                string sheetId = Convert.ToString(ddlSheetName.SelectedValue);

                decimal grandTotalAmount = 0;
                DataTable dt = aFileConverterObj.ReadExcelFileDOM(newPath, fileName, sheetId);
                DataRow row = dt.Rows[0];

                var arrayNames = (from DataColumn x in dt.Columns
                                  select x.ColumnName).ToArray();
                int coulmnCounter = arrayNames.Count();

                for (int i = 1; i <= coulmnCounter; i++)
                {
                    dt.Columns[i - 1].ColumnName = Convert.ToString(dt.Rows[0][i - 1]);
                }

                dt.Rows.Remove(row);
                gvStudentInfo.DataSource = dt;
                gvStudentInfo.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong, please contact with administrator";
            }
        }

        protected void btnSaveToServer_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int acacalId = Convert.ToInt32(ucAdmissionSession.selectedValue);
                int batchId = 0; // Convert.ToInt32(ucBatch.selectedValue);
                int activeAcacalId = Convert.ToInt32(ucActiveSession.selectedValue);
                if (programId > 0 && acacalId > 0 )
                {
                    Year yearObj = YearManager.GetById(Convert.ToInt32(ddlYear.SelectedValue));
                    Semester semesterObj = SemesterManager.GetById(Convert.ToInt32(ddlSemester.SelectedValue));

                    for (int i = 0; i < gvStudentInfo.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentInfo.Rows[i];
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                        if (ckBox.Checked)
                        {
                            string studentRoll = Convert.ToString(row.Cells[1].Text.Trim());
                            string name = Convert.ToString(row.Cells[2].Text.Trim());
                            string gender = Convert.ToString(row.Cells[3].Text.Trim());
                            string sessionName = Convert.ToString(row.Cells[4].Text.Trim());
                            string mobileno = Convert.ToString(row.Cells[5].Text.Trim());
                            string batchNo = Convert.ToString(row.Cells[6].Text.Trim());
                            string regNo = Convert.ToString(row.Cells[7].Text.Trim());
                            string hallName = Convert.ToString(row.Cells[8].Text.Trim());
                            string semester = Convert.ToString(row.Cells[9].Text.Trim());

                            LogicLayer.BusinessObjects.Student studentUpdateObj = StudentManager.GetByRoll(studentRoll.Trim());

                            
                            
                            if (studentUpdateObj == null)
                            {
                                int personId = personInsert(gender, mobileno, name);

                                if (personId > 0)
                                {
                                    int studentId = studentInsert(studentRoll, personId, programId, batchId, acacalId, activeAcacalId);

                                    if (studentId > 0)
                                    {

                                        int studentAdditionalInfoId = studentAdditionalInfoInsert(studentId, yearObj.YearId, yearObj.YearNo, semesterObj.SemesterId, semesterObj.SemesterNo, hallName, regNo);
                                    }
                                }
                            }
                            else
                            {
                                bool resultPersonUpdate = personUpdate(studentUpdateObj.PersonID, gender, mobileno, name);

                                if (resultPersonUpdate)
                                {
                                    if (studentUpdateObj == null)
                                    {
                                        int studentId = studentInsert(studentRoll, studentUpdateObj.PersonID, programId, batchId, acacalId, activeAcacalId);
                                        if (studentId > 0)
                                        {

                                            int studentAdditionalInfoId = studentAdditionalInfoInsert(studentId, yearObj.YearId, yearObj.YearNo, semesterObj.SemesterId, semesterObj.SemesterNo, hallName, regNo);
                                        }
                                    }
                                    else 
                                    {
                                        bool resultStudentUpdate = studentUpdate(studentUpdateObj.StudentID, studentRoll, studentUpdateObj.PersonID, programId, batchId, acacalId, activeAcacalId);
                                        if (resultStudentUpdate)
                                        {
                                            LogicLayer.BusinessObjects.StudentAdditionalInfo studentAddiotionalInfoUpdateObj = StudentAdditionalInfoManager.GetByStudentId(studentUpdateObj.StudentID);
                                            if(studentAddiotionalInfoUpdateObj == null)
                                            {
                                                int studentAdditionalInfoId = studentAdditionalInfoInsert(studentUpdateObj.StudentID, yearObj.YearId, yearObj.YearNo, semesterObj.SemesterId, semesterObj.SemesterNo, hallName, regNo);
                                            }
                                            else
                                            {
                                                bool studentAdditionalInfoUpdateObj = studentAdditionalInfoUpdate(studentAddiotionalInfoUpdateObj.InfoId, studentUpdateObj.StudentID, yearObj.YearId, yearObj.YearNo, semesterObj.SemesterId, semesterObj.SemesterNo, hallName, regNo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else 
                {
                    lblMsg.Text = "Please select program, session properly, and try again.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong, please contact with administrator";
            }
        }

        private int personInsert(string gender, string mobileno, string name) 
        {
            LogicLayer.BusinessObjects.Person personObj = new LogicLayer.BusinessObjects.Person();
            if (gender == "Male")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Male);
            }
            else if (gender == "Female")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Female);
            }
            personObj.Phone = mobileno;
            personObj.FullName = Convert.ToString(name);
            personObj.CreatedBy = userObj.Id;
            personObj.CreatedDate = DateTime.Now;
            personObj.ModifiedBy = userObj.Id;
            personObj.ModifiedDate = DateTime.Now;

            int personId = PersonManager.Insert(personObj);

            return personId;
        }

        private bool personUpdate(int personId, string gender, string mobileno, string name)
        {
            LogicLayer.BusinessObjects.Person personObj = PersonManager.GetById(personId);
            personObj.PersonID = personId;
            if (gender == "Male")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Male);
            }
            else if (gender == "Female")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Female);
            }
            personObj.Phone = mobileno;
            personObj.FullName = Convert.ToString(name);
            personObj.CreatedBy = userObj.Id;
            personObj.CreatedDate = DateTime.Now;
            personObj.ModifiedBy = userObj.Id;
            personObj.ModifiedDate = DateTime.Now;

            bool result = PersonManager.Update(personObj);

            return result;
        }

        private int studentInsert(string studentRoll, int personId, int programId, int batchId, int acacalId, int activeAcacalId) 
        {
            LogicLayer.BusinessObjects.Student studentObj = new LogicLayer.BusinessObjects.Student();

            studentObj.Roll = Convert.ToString(studentRoll);
            studentObj.PersonID = personId;
            studentObj.ProgramID = programId;
            studentObj.BatchId = batchId;
            studentObj.StudentAdmissionAcaCalId = acacalId;
            studentObj.ActiveSession = activeAcacalId;
            studentObj.CreatedBy = userObj.Id;
            studentObj.CreatedDate = DateTime.Now;
            studentObj.ModifiedBy = userObj.Id;
            studentObj.ModifiedDate = DateTime.Now;
            studentObj.IsActive = true;
            int studentId = StudentManager.Insert(studentObj);

            return studentId;
        }

        private bool studentUpdate(int studentId, string studentRoll, int personId, int programId, int batchId, int acacalId, int activeAcacalId) 
        {
            LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);

            studentObj.StudentID = studentId;
            studentObj.Roll = Convert.ToString(studentRoll);
            studentObj.PersonID = personId;
            studentObj.ProgramID = programId;
            studentObj.BatchId = batchId;
            studentObj.StudentAdmissionAcaCalId = acacalId;
            studentObj.ActiveSession = activeAcacalId;
            studentObj.CreatedBy = userObj.Id;
            studentObj.CreatedDate = DateTime.Now;
            studentObj.ModifiedBy = userObj.Id;
            studentObj.ModifiedDate = DateTime.Now;
            studentObj.IsActive = true;
            bool studentUpdate = StudentManager.Update(studentObj);

            return studentUpdate;
        }

        private int studentAdditionalInfoInsert(int studentId, int yearId, int yearNo, int semesterId, int semesterNo, string hallName, string regNo) 
        {
            
            LogicLayer.BusinessObjects.StudentAdditionalInfo studentAddiotionalInfoObj = new LogicLayer.BusinessObjects.StudentAdditionalInfo();

            studentAddiotionalInfoObj.StudentId = studentId;
            studentAddiotionalInfoObj.YearId = yearId;
            studentAddiotionalInfoObj.SemesterId = semesterId;
            studentAddiotionalInfoObj.YearNo = yearNo;
            studentAddiotionalInfoObj.SemesterNo = semesterNo;
            studentAddiotionalInfoObj.RegistrationNo = regNo;
            studentAddiotionalInfoObj.Attribute1 = hallName;
            studentAddiotionalInfoObj.CreatedBy = userObj.Id;
            studentAddiotionalInfoObj.CreatedDate = DateTime.Now;
            studentAddiotionalInfoObj.ModifiedBy = userObj.Id;
            studentAddiotionalInfoObj.ModifiedDate = DateTime.Now;

            int studentAdditionalInfoId = StudentAdditionalInfoManager.Insert(studentAddiotionalInfoObj);

            return studentAdditionalInfoId;
        }

        private bool studentAdditionalInfoUpdate(int infoId, int studentId, int yearId, int yearNo, int semesterId, int semesterNo, string hallName, string regNo)
        {
            LogicLayer.BusinessObjects.StudentAdditionalInfo studentAddiotionalInfoObj = StudentAdditionalInfoManager.GetById(infoId);

            studentAddiotionalInfoObj.StudentId = studentId;
            studentAddiotionalInfoObj.YearId = yearId;
            studentAddiotionalInfoObj.SemesterId = semesterId;
            studentAddiotionalInfoObj.RegistrationNo = regNo;
            studentAddiotionalInfoObj.Attribute1 = hallName;
            studentAddiotionalInfoObj.ModifiedBy = userObj.Id;
            studentAddiotionalInfoObj.ModifiedDate = DateTime.Now;

            bool studentAdditionalInfoUpdate = StudentAdditionalInfoManager.Update(studentAddiotionalInfoObj);

            return studentAdditionalInfoUpdate;
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
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

                foreach (GridViewRow row in gvStudentInfo.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong, please contact with administrator";
            }
        }
    }
}