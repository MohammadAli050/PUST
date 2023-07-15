using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using Microsoft.Reporting.WebForms;
using CommonUtility;
using LogicLayer.DataEntity;
using System.IO;

namespace EMS.Module.student
{
    public partial class RptStudentApplicationForm : System.Web.UI.Page
    {
        StudentApplicationFormModel dbObj = new StudentApplicationFormModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FinalMsg.Visible = false;
                    if (Request.QueryString["Id"] != null)
                    {
                        int officialId = Convert.ToInt32(Request.QueryString["Id"].ToString());

                        var officialInfo = dbObj.StudentApplicationOfficialInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialId);

                        if (officialInfo != null && officialInfo.IsFinalSubmit == false)
                        {
                            LoadReport(officialId, true);   
                        }
                        else
                        {
                            FinalMsg.Visible = true;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadReport(int officialId, bool isPageLoad)
        {
            var studentOfficialInfo = StudentApplicationFormManager.GetStudentApplicationDetailsByOfficialId(officialId);
            var studentEducationInfo = StudentApplicationFormManager.GetStudentApplicationEducationDetailsByOfficialId(officialId);
            var studentPrevousSemesterInfo = StudentApplicationFormManager.GetStudentApplicationPreviousSemesterDetailsByOfficialId(officialId);
            var studentAppliedCourseInfo = StudentApplicationFormManager.GetStudentApplicationAppliedCourseDetailsByOfficialId(officialId);
            
            if (studentOfficialInfo != null)
            {
                var year = studentOfficialInfo.FirstOrDefault().AppliedYear.ToString();
                var semester = studentOfficialInfo.FirstOrDefault().AppliedSemester;
                var session = studentOfficialInfo.FirstOrDefault().AppliedSession;
                var program = studentOfficialInfo.FirstOrDefault().AppliedProgram;

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportParameter p1 = new ReportParameter("PhotoPath", new Uri(Server.MapPath(studentOfficialInfo.FirstOrDefault().PhotoPath)).AbsoluteUri);
                ReportParameter p2 = new ReportParameter("SignaturePath", new Uri(Server.MapPath(studentOfficialInfo.FirstOrDefault().SignaturePath)).AbsoluteUri);
                ReportParameter p3 = new ReportParameter("AppliedYear", year);
                ReportParameter p4 = new ReportParameter("AppliedSemester", semester);
                ReportParameter p5 = new ReportParameter("AppliedSession", session);
                ReportParameter p6 = new ReportParameter("AppliedProgram", program);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/student/Report/RptStudentApplicationForm.rdlc");
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6 });

                ReportDataSource rds1 = new ReportDataSource("DataSet1", studentOfficialInfo);
                ReportDataSource rds2 = new ReportDataSource("EduDataSet", studentEducationInfo);
                ReportDataSource rds3 = new ReportDataSource("PreviousSemesterDataSet", studentPrevousSemesterInfo);
                ReportDataSource rds4 = new ReportDataSource("AppliedCourseDataSet", studentAppliedCourseInfo);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.LocalReport.DataSources.Add(rds2);
                ReportViewer1.LocalReport.DataSources.Add(rds3);
                ReportViewer1.LocalReport.DataSources.Add(rds4);

                if (!isPageLoad)
                {
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = mimeType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ApplicationFrom_" + studentOfficialInfo.FirstOrDefault().StudentIDNo.ToString() + "." + filenameExtension);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();                    
                }                
            }
        }

        protected void btnRetunPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                int officialInfoId = Request.QueryString["Id"] != null ? Convert.ToInt32(Request.QueryString["Id"].ToString()) : 0;
                Session["OfcId"] = officialInfoId.ToString();
                Response.Redirect("~/Module/student/StudentApplicationForm.aspx?mmi=" + UtilityManager.Encrypt("-1"), false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnFinalSubmit_Click(object sender, EventArgs e)
        {
            try
            {                
                int officialInfoId = Request.QueryString["Id"] != null ? Convert.ToInt32(Request.QueryString["Id"].ToString()) : 0;
                if (officialInfoId > 0)
                {
                    var officialInfo = dbObj.StudentApplicationOfficialInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                    if (officialInfo != null)
                    {                        
                        officialInfo.IsFinalSubmit = true;
                        officialInfo.ModifiedBy = -1;
                        officialInfo.ModifiedDate = DateTime.Now;

                        dbObj.SaveChanges();

                        FinalMsg.Visible = true;

                        LoadReport(officialInfoId, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Response.Redirect(Request.RawUrl);
            }
            finally
            {
                //Session["final"] = "1";
                var url = "../../Module/student/StudentApplicationForm.aspx?mmi=" + UtilityManager.Encrypt("-1");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "window.location.href = " + url + ";", true);
                //Response.Redirect("~/Module/student/StudentApplicationForm.aspx?mmi=" + UtilityManager.Encrypt("-1"));
            }
        }

        protected void btnAdmitCardDownload_Click(object sender, EventArgs e)
        {
            try
            {
                 if (Request.QueryString["Id"] != null)
                 {
                     var officialId = Request.QueryString["Id"].ToString();
   
                     
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}