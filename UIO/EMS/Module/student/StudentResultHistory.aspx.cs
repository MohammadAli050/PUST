using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessObjects.DTO;
using System.IO;

namespace EMS.Module.student
{
    public partial class StudentResultHistory : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentResultHistory);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentResultHistory));

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();

            if (!IsPostBack)
            {
                lblRegistered.Visible = false;
                lblWaiver.Visible = false;
                //lblResult.Visible = false;

                //gvResult.Visible = false;
                gvRegisteredCourse.Visible = false;
                gvWaiVeredCourse.Visible = false;

                if (currentUserRoleId == (int)CommonEnum.Role.Student)
                {
                    Person person = PersonManager.GetByUserId(BaseCurrentUserObj.Id);
                    if (person == null)
                    {
                        ShowAlertMessage("User\\'s profile is not uptodate. Make relation with user and person or contact with system admin.");
                        txtStudentId.ReadOnly = true;
                        return;
                    }

                    Student student = StudentManager.GetBypersonID(person.PersonID);
                    if (student == null)
                    {
                        ShowAlertMessage("User\\'s profile is not uptodate. Make relation with user and person or contact with system admin.");
                        txtStudentId.ReadOnly = true;
                        return;
                    }

                    txtStudentId.Text = student.Roll;
                    txtStudentId.ReadOnly = true;

                    if (student != null)
                    {
                        if (AccessAuthentication(BaseCurrentUserObj, student.Roll.Trim()))
                        {
                            btnLoad_Click(null, null);
                        }
                        else
                        {
                            ShowAlertMessage("Access Permission Denied. Please contact with Administrator.");
                        }
                    }
                }

                if (Request.QueryString["Roll"] != null)
                {
                    txtStudentId.Text = Request.QueryString["Roll"];
                    btnLoad_Click(null, null);
                }
            }
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        private void ClearGrid()
        {
            try
            {
                lblRegistered.Visible = false;
                lblWaiver.Visible = false;
                //lblResult.Visible = false;

                //gvResult.DataSource = null;
                //gvResult.DataBind();
                gvRegisteredCourse.DataSource = null;
                gvRegisteredCourse.DataBind();
                gvWaiVeredCourse.DataSource = null;
                gvWaiVeredCourse.DataBind();

                //gvResult.Visible = false;
                gvRegisteredCourse.Visible = false;
                gvWaiVeredCourse.Visible = false;
            }
            catch { }
            finally { }
        }

        private void CleareTxtField()
        {
            txtStudentId.Text = "";
            lblStudentName.Text = "";
            //lblStudentBatch.Text = "";
            lblStudentProgram.Text = "";
            //lblCGPA.Text = "";          
            //lblCompletedCr.Text = "";
            //lblAttemptedCr.Text = "";
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //imgPhoto.Visible = false;
                BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

                string studentId = txtStudentId.Text;
                if (true) //(AccessAuthentication(BaseCurrentUserObj, studentId))
                {

                    Student student = StudentManager.GetByRoll(studentId);
                    if (student != null)
                    {
                        lblActiveStatus.Text = student.IsActive == true ? "Active" : "Inactive";
                        //StudentProbationDTO studentMaxTrimesterProb = StudentManager.GetMaxProbationStatus(student.StudentID);
                        //if (studentMaxTrimesterProb != null && studentMaxTrimesterProb.ProbationCount > 0)
                        //    lblProbation.Text = "Probation no (" + studentMaxTrimesterProb.ProbationCount + ")";
                        //else
                        //    lblProbation.Text = "No probation!";

                        #region Display Photo
                        //try
                        //{
                        //    if (student.BasicInfo.PhotoPath != null)
                        //    {
                        //        if (File.Exists(Server.MapPath("~/Upload/Avatar/Student/" + student.BasicInfo.PhotoPath)))
                        //        {
                        //            imgPhoto.ImageUrl = "~/Upload/Avatar/Student/" + student.BasicInfo.PhotoPath;
                        //            imgPhoto.Visible = true;
                        //        }
                        //        else if (student.BasicInfo.Gender != null)
                        //        {
                        //            imgPhoto.Visible = true;
                        //            if (student.BasicInfo.Gender.ToLower() == "female")
                        //                imgPhoto.ImageUrl = "~/Images/photoGirl.png";
                        //            else
                        //                imgPhoto.ImageUrl = "~/Images/photoBoy.png";
                        //        }

                        //    }
                        //    else
                        //    {
                        //        if (student.BasicInfo.Gender != null)
                        //        {
                        //            imgPhoto.Visible = true;
                        //            if (student.BasicInfo.Gender.ToLower() == "female")
                        //                imgPhoto.ImageUrl = "~/Images/photoGirl.png";
                        //            else
                        //                imgPhoto.ImageUrl = "~/Images/photoBoy.png";
                        //        }
                        //    }
                        //}
                        //catch { }
                        #endregion

                        Person person = PersonManager.GetById(student.PersonID);
                        if (person != null)
                        {
                            lblStudentName.Text = person.FullName;
                            //lblStudentBatch.Text = student.Batch == null ? "" : student.Batch.BatchNO.ToString();
                            lblStudentProgram.Text = student.Program.ShortName;

                            //lblCGPA.Text = student.CurrentSeccionResult == null ? "0" : student.LatestTranscriptCGPA.ToString();
                            //lblMajor.Text = student.Major1NodeName;
                            //lblMajor2.Text = student.Major2NodeName == " -- " ? "" : student.Major2NodeName;

                            //List<DegreeRequirementCredit> rc = RequiredCreditManager.GetAllDegreeRequirementCreditByProgramAndSession(student.ProgramID, student.Batch.AdmissionCalenderID);
                            //lblDegreeReq.Text = (decimal.Round(Convert.ToDecimal(rc[0].RequiredCredit), 2)).ToString();
                        }
                        else
                        {
                            lblStudentName.Text = "-------";
                        }

                        //List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentId(student.StudentID);
                        List<StudentCourseHistoryNewDTO> registeredStudentCourseHistoryList = StudentCourseHistoryManager.GetAllCourseHistoryDetailByStudentId(student.StudentID);
                        List<StudentCourseHistoryNewDTO> waiverStudentCourseHistoryList = null;

                        if (registeredStudentCourseHistoryList.Count > 0 && registeredStudentCourseHistoryList != null)
                        {
                            //List<StudentCourseHistory> completeCr = studentCourseHistoryList.Where(x => x.IsConsiderGPA == true && x.ObtainedGrade != "F").ToList();

                            decimal completeCredit = StudentCourseHistoryManager.GetCompletedCreditByRoll(student.Roll);
                            //lblCompletedCr.Text = completeCredit.ToString();

                            decimal attemptCredit = StudentCourseHistoryManager.GetAttemptedCreditByRoll(student.Roll);
                            //lblAttemptedCr.Text = (decimal.Round(attemptCredit, 2)).ToString();

                            lblRegistered.Visible = true;
                            lblWaiver.Visible = true;
                            //lblResult.Visible = true;

                            //gvResult.Visible = true;
                            gvRegisteredCourse.Visible = true;
                            gvWaiVeredCourse.Visible = true;

                            #region Registered Course
                            //registeredStudentCourseHistoryList = studentCourseHistoryList.Where(x => x.CourseWavTransfrID == null || x.CourseWavTransfrID == 0).ToList();
                            if (registeredStudentCourseHistoryList.Count > 0 && registeredStudentCourseHistoryList != null)
                            {
                                //registeredStudentCourseHistoryList = registeredStudentCourseHistoryList.OrderBy(x => x.AcaCalID).ToList();
                                //foreach (StudentCourseHistory studentCourseHistory in registeredStudentCourseHistoryList)
                                //{
                                //    if (acaCalHash.ContainsKey(studentCourseHistory.AcaCalID))
                                //        studentCourseHistory.Semester = acaCalHash[studentCourseHistory.AcaCalID].ToString();
                                //    if (courseHashCourseCode.ContainsKey(studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID))
                                //        studentCourseHistory.CourseCode = courseHashCourseCode[studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID].ToString();
                                //    if (courseHashCourseName.ContainsKey(studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID))
                                //        studentCourseHistory.CourseName = courseHashCourseName[studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID].ToString();

                                //if (studentCourseHistory.ObtainedGrade == null && courseStatusHash.ContainsKey(studentCourseHistory.CourseStatusID))
                                //{
                                //    //studentCourseHistory.CourseStatus.CourseStatusID.ToString() = courseStatusHash[studentCourseHistory.CourseStatusID].ToString();
                                //    if (studentCourseHistory.CourseStatus.Code == "R")
                                //    {
                                //        studentCourseHistory.CourseStatus = "Running Course";
                                //    }
                                //    else if (studentCourseHistory.CourseStatus == "W")
                                //    {
                                //        studentCourseHistory.CourseStatus = "Withdraw";
                                //        studentCourseHistory.ObtainedGrade = "W";
                                //        studentCourseHistory.ObtainedGPA = 0;
                                //    }
                                //    else if (studentCourseHistory.CourseStatus == "X")
                                //    {
                                //        // studentCourseHistory.CourseStatus = "Withdraw";
                                //        studentCourseHistory.ObtainedGrade = "X";
                                //        studentCourseHistory.ObtainedGPA = 0;
                                //    }
                                //    else if (studentCourseHistory.CourseStatus == "I")
                                //    {
                                //        // studentCourseHistory.CourseStatus = "Withdraw";
                                //        studentCourseHistory.ObtainedGrade = "I";
                                //        studentCourseHistory.ObtainedGPA = 0;
                                //    }
                                //}
                                //}

                                //registeredStudentCourseHistoryList = registeredStudentCourseHistoryList.Where(x => x.CourseStatusID != 19).ToList();

                                gvRegisteredCourse.DataSource = registeredStudentCourseHistoryList.Where(x => x.CourseStatusCode != null && x.CourseStatusCode != "Wv").ToList();
                                gvRegisteredCourse.DataBind();
                            }
                            else
                            {
                                gvRegisteredCourse.DataSource = null;
                                gvRegisteredCourse.DataBind();
                            }
                            #endregion

                            #region Waiverred Course
                            waiverStudentCourseHistoryList = registeredStudentCourseHistoryList.Where(x => x.CourseStatusCode != null && x.CourseStatusCode == "Wv").ToList();

                            //var uiuTransferCourse = studentCourseHistoryList.Where(x => x.CourseStatusID == 19).ToList();

                            //waiverStudentCourseHistoryList.AddRange(uiuTransferCourse);

                            if (waiverStudentCourseHistoryList.Count > 0 && waiverStudentCourseHistoryList != null)
                            {
                                //foreach (StudentCourseHistory studentCourseHistory in waiverStudentCourseHistoryList)
                                //{
                                //    if (courseHashCourseCode.ContainsKey(studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID))
                                //        studentCourseHistory.CourseCode = courseHashCourseCode[studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID].ToString();
                                //    if (courseHashCourseName.ContainsKey(studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID))
                                //        studentCourseHistory.CourseName = courseHashCourseName[studentCourseHistory.CourseID + "_" + studentCourseHistory.VersionID].ToString();
                                //}

                                gvWaiVeredCourse.DataSource = waiverStudentCourseHistoryList;
                                gvWaiVeredCourse.DataBind();
                            }
                            else
                            {
                                gvWaiVeredCourse.DataSource = null;
                                gvWaiVeredCourse.DataBind();
                            }
                            #endregion

                            #region Result
                            //List<StudentACUDetailDTO> studentACUDetailList = StudentACUDetailManager.GetAllByStudentId(student.StudentID);
                            //studentACUDetailList = studentACUDetailList.Where(l => l.Credit != 0).ToList();

                            //if (studentACUDetailList.Count > 0 && studentACUDetailList != null)
                            //{
                            //    studentACUDetailList = studentACUDetailList.OrderBy(x => x.StdAcademicCalenderID).ToList();
                            //    foreach (StudentACUDetailDTO studentACUDetail in studentACUDetailList)
                            //        if (acaCalHash.ContainsKey(studentACUDetail.StdAcademicCalenderID))
                            //            studentACUDetail.Semester = acaCalHash[studentACUDetail.StdAcademicCalenderID].ToString();
                            //    gvResult.DataSource = studentACUDetailList;
                            //    gvResult.DataBind();
                            //}
                            //else
                            //{
                            //    gvResult.DataSource = null;
                            //    gvResult.DataBind();
                            //}
                            #endregion
                        }
                        else
                        {
                            ClearGrid();
                            showAlert("No Course History Found.");
                        }
                        #region Log Insert
                        try
                        {
                            LogGeneralManager.Insert(
                               DateTime.Now,
                               "",
                               BaseAcaCalCurrent.FullCode,
                               BaseCurrentUserObj.LogInID,
                               "",
                               "",
                               "Result History",
                               BaseCurrentUserObj.LogInID + " loaded result history for student " + student.Roll,
                               "normal",
                               _pageId,
                               _pageName,
                               _pageUrl,
                               Convert.ToString(txtStudentId.Text));
                        }
                        catch (Exception ex) { }

                        #endregion
                    }
                    else
                    {
                        CleareTxtField();
                        ClearGrid();
                        showAlert("Student ID Not Found.");
                    }
                }
                else
                {
                    ClearGrid();
                    CleareTxtField();
                    showAlert("Access Permission Denied.");
                }
            }
            catch { }

        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }
    }
}