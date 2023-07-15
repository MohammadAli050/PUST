using ClosedXML.Excel;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.student
{
    public partial class StudentInformationMigration : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentInformationMigration);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentInformationMigration));

        BussinessObject.UIUMSUser BaseCurrentUserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                DivViewStudent.Visible = false;
                DivExcelUpload.Visible = false;
                ucDepartment.LoadDropDownList();
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                ClearExcelGrid();
                ClearGrid();
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGrid();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
            }
            catch (Exception)
            {

            }
        }


        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        protected void ucAdmissionSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGrid();
                ClearExcelGrid();

                DivViewStudent.Visible = false;
                DivExcelUpload.Visible = false;

                Session["DataTableExcelUpload"] = null;

                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int SessionId = Convert.ToInt32(ucAdmissionSession.selectedValue);

                if (ProgramId == 0)
                {
                    showAlert("Please Choose a program");
                    return;
                }
                if (SessionId == 0)
                {
                    showAlert("Please Choose a Admission Session");
                    return;
                }
                else
                {

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                    parameters1.Add(new SqlParameter { ParameterName = "@SessionId", SqlDbType = System.Data.SqlDbType.Int, Value = SessionId });


                    DataTable dt = DataTableManager.GetDataFromQuery("GetStudentListByProgramAndAdmissionSession", parameters1);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DivViewStudent.Visible = true;
                        DivExcelUpload.Visible = false;
                        gvStudentList.DataSource = dt;
                        gvStudentList.DataBind();
                    }
                    else
                        ClearGrid();

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnMigration_Click(object sender, EventArgs e)
        {
            try
            {
                Session["DataTableExcelUpload"] = null;
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int SessionId = Convert.ToInt32(ucAdmissionSession.selectedValue);
                if (ProgramId == 0)
                {
                    showAlert("Please Choose a program");
                    return;
                }
                if (SessionId == 0)
                {
                    showAlert("Please Choose a Admission Session");
                    return;
                }
                else
                {

                    DivExcelUpload.Visible = true;
                    DivViewStudent.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkExcelUpload_Click(object sender, EventArgs e)
        {
            try
            {
                ClearExcelGrid();
                Session["DataTableExcelUpload"] = null;
                Session["NotMigratedStudentList"] = null;
                if (ExcelUpload.HasFile)
                {
                    string saveFolder = "~/Upload/";
                    string filename = ExcelUpload.FileName;
                    string filePath = Path.Combine(saveFolder, ExcelUpload.FileName);
                    string excelpath = Server.MapPath(filePath);

                    string Extension = Path.GetExtension(filename);

                    if (Extension.ToLower() != ".xlsx" && Extension.ToLower() != ".xls")
                    {
                        showAlert("Please upload the excel file and try again");
                        return;
                    }

                    if (File.Exists(excelpath))
                    {
                        System.IO.File.Delete(excelpath);
                        ExcelUpload.SaveAs(excelpath);
                    }
                    else
                    {
                        ExcelUpload.SaveAs(excelpath);
                    }

                    try
                    {
                        System.Data.OleDb.OleDbConnection MyConnection;
                        System.Data.DataTable DtTable;
                        System.Data.OleDb.OleDbDataAdapter MyCommand; ;
                        MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelpath + ";Extended Properties=Excel 12.0 xml;");
                        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                        MyCommand.TableMappings.Add("Table", "TestTable");
                        DtTable = new System.Data.DataTable();
                        MyCommand.Fill(DtTable);
                        Session["DataTableExcelUpload"] = DtTable;

                        MyConnection.Close();
                        if (DtTable.Rows.Count > 0)
                        {
                            GVTotalStudentList.DataSource = DtTable;
                            GVTotalStudentList.DataBind();

                            lblTotalStudent.Text = "Total Students List : " + DtTable.Rows.Count;

                            DivTotalStudent.Visible = true;
                        }

                        if (File.Exists(excelpath))
                        {
                            System.IO.File.Delete(excelpath);
                        }

                    }
                    catch (Exception ex)
                    {
                        showAlert("Excel file format is not correct.You can download sample excel file for help.");
                        return;
                    }
                }
                else
                {
                    showAlert("Please select a excel file");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkSampleExcel_Click(object sender, EventArgs e)
        {
            try
            {

                string fileName = "SampleStudentMigrationFile.xlsx";

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(Server.MapPath("~/Upload/SampleExcel/" + fileName));

                Response.Flush();
                Response.SuppressContent = true;
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkStudentMigrateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Session["NotMigratedStudentList"] = null;
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int SessionId = Convert.ToInt32(ucAdmissionSession.selectedValue);

                int Total = 0;


                if (ProgramId > 0 && SessionId > 0)
                {
                    DataTable StudentList = (DataTable)Session["DataTableExcelUpload"];

                    if (StudentList != null && StudentList.Rows.Count > 0)
                    {

                        List<StudentMigrationObj> MigrationStudentList = new List<StudentMigrationObj>();

                        Total = StudentList.Rows.Count;

                        string Roll = "", Name = "", DOB = "", FName = "", MName = "", AdmissionRoll = "", Hall = "";

                        #region Data Migration Process

                        foreach (DataRow row in StudentList.Rows)
                        {
                            try
                            {
                                string SL = "";

                                if (!string.IsNullOrEmpty(row[0].ToString()))
                                    SL = row[0].ToString().Trim();

                                if (SL != "")
                                {
                                    if (!string.IsNullOrEmpty(row[3].ToString()))
                                        AdmissionRoll = row[2].ToString().Trim();

                                    if (!string.IsNullOrEmpty(row[3].ToString()))
                                        Roll = row[3].ToString().Trim();

                                    if (!string.IsNullOrEmpty(row[4].ToString()))
                                        Name = row[4].ToString().Trim();
                                    if (!string.IsNullOrEmpty(row[7].ToString()))
                                        DOB = row[7].ToString().Trim();
                                    if (!string.IsNullOrEmpty(row[5].ToString()))
                                        FName = row[5].ToString().Trim();
                                    if (!string.IsNullOrEmpty(row[6].ToString()))
                                        MName = row[6].ToString().Trim();
                                    if (!string.IsNullOrEmpty(row[11].ToString()))
                                        Hall = row[11].ToString().Trim();


                                    StudentMigrationObj MigratedObj = CommonFunctionForStudentMigration.MigrateStudent(AdmissionRoll, ProgramId, SessionId, Roll, Name, DOB, FName, MName, Hall, BaseCurrentUserObj.Id, BaseCurrentUserObj.LogInID, _pageId, _pageName, _pageUrl);

                                    if (MigratedObj != null)
                                    {
                                        MigrationStudentList.Add(MigratedObj);
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                StudentMigrationObj ExceptionObj = new StudentMigrationObj();

                                ExceptionObj.Status = 0;
                                ExceptionObj.StudentID = Roll;
                                ExceptionObj.Name = Name;
                                ExceptionObj.Reason = "Exception occurred";

                                MigrationStudentList.Add(ExceptionObj);

                            }

                        }

                        #endregion

                        Session["DataTableExcelUpload"] = null;

                        ClearExcelGrid();

                        #region Bind Grid View

                        HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                        cookie.Value = "Flag";
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.AppendCookie(cookie);



                        if (MigrationStudentList != null && MigrationStudentList.Any())
                        {
                            string msg = "";

                            if (MigrationStudentList.Where(x => x.Status == 1).Any())
                            {
                                var MigratedList = MigrationStudentList.Where(x => x.Status == 1).ToList();

                                lblTotalStudent.Text = "Total Updated Student List : " + MigratedList.Count();

                                GVTotalStudentList.DataSource = MigratedList;
                                GVTotalStudentList.DataBind();

                                DivTotalStudent.Visible = true;

                                msg = "Students Information Updated Successfully";

                            }
                            if (MigrationStudentList.Where(x => x.Status == 0).Any())
                            {
                                var NotMigratedList = MigrationStudentList.Where(x => x.Status == 0).ToList();

                                lblNotMigratedStudent.Text = "Total Not Updated Student List : " + NotMigratedList.Count();

                                GVNotUploadedStudentList.DataSource = NotMigratedList;
                                GVNotUploadedStudentList.DataBind();

                                DivNotUploadedStudent.Visible = true;

                                Session["NotMigratedStudentList"] = NotMigratedList;

                                if (msg == "")
                                    msg = "Some Students Information is Not Updated";
                                else
                                    msg = msg + " And Some Students Information is Not Updated";

                            }

                            showAlert(msg);
                            return;

                        }
                        else
                        {
                            showAlert("Something went wrong while uploding the excel file");
                            return;
                        }
                        #endregion


                    }
                    else
                    {
                        showAlert("No Data Found To Upload");
                        Session["DataTableExcelUpload"] = null;
                        HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                        cookie.Value = "Flag";
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.AppendCookie(cookie);
                        return;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                ClearExcelGrid();
                showAlert("Something went wrong");
                return;
            }
        }

        protected void lnkDownloadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<StudentMigrationObj> StudentList = (List<StudentMigrationObj>)Session["NotMigratedStudentList"];


                if (StudentList != null && StudentList.Count > 0)
                {
                    DataTable stdlist = CommonUtility.DataTableMethods.ListToDataTable(StudentList);

                    if (stdlist != null && stdlist.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {

                            IXLWorksheet sheet2;
                            sheet2 = wb.AddWorksheet(stdlist, "Sheet");

                            sheet2.Table("Table1").ShowAutoFilter = false;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=" + "NotUploadedStudentList.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);

                                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                                cookie.Value = "Flag";
                                cookie.Expires = DateTime.Now.AddDays(1);
                                Response.AppendCookie(cookie);

                                Response.Flush();
                                Response.SuppressContent = true;
                                Response.End();
                            }
                        };
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void ClearGrid()
        {
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
        }

        private void ClearExcelGrid()
        {
            DivTotalStudent.Visible = false;
            DivNotUploadedStudent.Visible = false;
            GVTotalStudentList.DataSource = null;
            GVTotalStudentList.DataBind();
            GVNotUploadedStudentList.DataSource = null;
            GVNotUploadedStudentList.DataBind();
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

    }
}