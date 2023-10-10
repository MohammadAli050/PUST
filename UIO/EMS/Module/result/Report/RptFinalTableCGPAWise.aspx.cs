using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using LogicLayer.BusinessLogic;
using CommonUtility;
using Newtonsoft.Json;

namespace EMS.Module.result.Report
{
    public partial class RptFinalTableCGPAWise : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.RptStudentAdmitCard);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.RptStudentAdmitCard));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {


                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                //LoadHallInfo();
                LoadHeldInInformation();

                //if (UserObj.RoleID == 4) // Student
                //{
                //    txtStudentId.Text = UserObj.LogInID.ToString();
                //    btnLoad.Visible = false;
                //    btnLoad_Click(null, null);
                //}
                //else
                //{
                //    btnLoad.Visible = true;
                //}
            }
        }


        #region On Load Methods

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("Select", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        //private void LoadHallInfo()
        //{
        //    try
        //    {
        //        var HallList = ucamContext.HallInformations.Where(x => x.ActiveStatus == 1).ToList();

        //        ddlHallInfo.Items.Clear();
        //        ddlHallInfo.AppendDataBoundItems = true;
        //        ddlHallInfo.Items.Add(new ListItem("Select", "0"));

        //        if (HallList != null && HallList.Any())
        //        {
        //            ddlHallInfo.DataTextField = "HallName";
        //            ddlHallInfo.DataValueField = "Id";
        //            ddlHallInfo.DataSource = HallList.OrderBy(x => x.HallName);
        //            ddlHallInfo.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        #endregion


        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                //ClearReportView();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHeldInInformation();
                //ClearReportView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ClearReportView();
            }
            catch (Exception ex)
            {
            }
        }

        //protected void ddlHallInfo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClearReportView();
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        #endregion

        [WebMethod]
        public static string GeneratefinalTabulationCGPAWise(string heldInId)
        {
            //int programId = Convert.ToInt32(progID);
            //int acalSecId = Convert.ToInt32(acalID);
            int HeldInId = Convert.ToInt32(heldInId);
            int StudentId = 0;

            try
            {
                //List<LogicLayer.BusinessObjects.Student> list = StudentManager.GetAllByProgramIdSessionId(programId, acalSecId);
                List<StudentListTabulation> studentListTabulationList = new List<StudentListTabulation>();

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetHeldInWiseStudentResultSummary", parameters1);
                List<DataList> list = new List<DataList>();
                if (dt.Rows.Count > 0)
                {
                    list = DataTableMethods.ConvertDataTable<DataList>(dt);
                }
                //List<StudentListTabulation> list2 = new List<StudentListTabulation>();

                //if (list.Count > 0)
                //{
                //    foreach (var student in list)
                //    {
                //        StudentListTabulation studentListTabulation = new StudentListTabulation
                //        {
                //            ID = student.StudentID,
                //            Roll = student.Roll,
                //            RegNo = student.RegNo,
                //            StudentName = student.StudentName,
                //            FatherName = student.FatherName
                //        };

                //        // year1 sem1
                //        if (student.YearNo == 1 && student.SemesterNo == 1)
                //        {
                //            studentListTabulation.EarnedCreditPoint11 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit11 = student.TakenCredit;
                //            studentListTabulation.SGPA11 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint11 = 0;
                //            studentListTabulation.TotalCredit11 = 0;
                //            studentListTabulation.SGPA11 = 0;
                //        }

                //        // year1 sem2
                //        if (student.YearNo == 1 && student.SemesterNo == 2)
                //        {
                //            studentListTabulation.EarnedCreditPoint12 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit12 = student.TakenCredit;
                //            studentListTabulation.SGPA12 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint12 = 0;
                //            studentListTabulation.TotalCredit12 = 0;
                //            studentListTabulation.SGPA12 = 0;
                //        }
                //        // year2 sem1
                //        if (student.YearNo == 2 && student.SemesterNo == 1)
                //        {
                //            studentListTabulation.EarnedCreditPoint21 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit21 = student.TakenCredit;
                //            studentListTabulation.SGPA21 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint21 = 0;
                //            studentListTabulation.TotalCredit21 = 0;
                //            studentListTabulation.SGPA21 = 0;
                //        }
                //        // year2 sem2
                //        if (student.YearNo == 2 && student.SemesterNo == 2)
                //        {
                //            studentListTabulation.EarnedCreditPoint22 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit22 = student.TakenCredit;
                //            studentListTabulation.SGPA22 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint22 = 0;
                //            studentListTabulation.TotalCredit22 = 0;
                //            studentListTabulation.SGPA22 = 0;
                //        }
                //        // year3 sem1
                //        if (student.YearNo == 3 && student.SemesterNo == 1)
                //        {
                //            studentListTabulation.EarnedCreditPoint31 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit31 = student.TakenCredit;
                //            studentListTabulation.SGPA31 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint31 = 0;
                //            studentListTabulation.TotalCredit31 = 0;
                //            studentListTabulation.SGPA31 = 0;
                //        }

                //        // year3 sem2
                //        if (student.YearNo == 3 && student.SemesterNo == 2)
                //        {
                //            studentListTabulation.EarnedCreditPoint32 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit32 = student.TakenCredit;
                //            studentListTabulation.SGPA32 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint32 = 0;
                //            studentListTabulation.TotalCredit32 = 0;
                //            studentListTabulation.SGPA32 = 0;
                //        }
                //        // year4 sem1
                //        if (student.YearNo == 4 && student.SemesterNo == 1)
                //        {
                //            studentListTabulation.EarnedCreditPoint41 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit41 = student.TakenCredit;
                //            studentListTabulation.SGPA41 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint41 = 0;
                //            studentListTabulation.TotalCredit41 = 0;
                //            studentListTabulation.SGPA41 = 0;
                //        }
                //        // year4 sem2
                //        if (student.YearNo == 4 && student.SemesterNo == 2)
                //        {
                //            studentListTabulation.EarnedCreditPoint42 = student.EarnedCredit;
                //            studentListTabulation.TotalCredit42 = student.TakenCredit;
                //            studentListTabulation.SGPA42 = student.TranscriptGPA;
                //        }
                //        else
                //        {
                //            studentListTabulation.EarnedCreditPoint42 = 0;
                //            studentListTabulation.TotalCredit42 = 0;
                //            studentListTabulation.SGPA42 = 0;
                //        }
                //    }
                //}
                var returnlist = JsonConvert.SerializeObject(list);
                return returnlist;
                //return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public class StudentListTabulation
        {
            public int ID { get; set; }
            public string Roll { get; set; }
            public string RegNo { get; set; }
            public string StudentName { get; set; }
            public string FatherName { get; set; }
            public decimal EarnedCreditPoint11 { get; set; }
            public decimal EarnedCreditPoint12 { get; set; }
            public decimal EarnedCreditPoint21 { get; set; }
            public decimal EarnedCreditPoint22 { get; set; }
            public decimal EarnedCreditPoint31 { get; set; }
            public decimal EarnedCreditPoint32 { get; set; }
            public decimal EarnedCreditPoint41 { get; set; }
            public decimal EarnedCreditPoint42 { get; set; }
            public decimal TotalCredit11 { get; set; }
            public decimal TotalCredit12 { get; set; }
            public decimal TotalCredit21 { get; set; }
            public decimal TotalCredit22 { get; set; }
            public decimal TotalCredit31 { get; set; }
            public decimal TotalCredit32 { get; set; }
            public decimal TotalCredit41 { get; set; }
            public decimal TotalCredit42 { get; set; }
            public decimal SGPA11 { get; set; }
            public decimal SGPA12 { get; set; }
            public decimal SGPA21 { get; set; }
            public decimal SGPA22 { get; set; }
            public decimal SGPA31 { get; set; }
            public decimal SGPA32 { get; set; }
            public decimal SGPA41 { get; set; }
            public decimal SGPA42 { get; set; }
        }

        public class DataList
        {
            public int YearNo { get; set; }
            public string YearName { get; set; }
            public int SemesterNo { get; set; }
            public string SemesterName { get; set; }
            public decimal TakenCredit { get; set; }
            public decimal EarnedCredit { get; set; }
            public decimal GPA { get; set; }
            public decimal TranscriptGPA { get; set; }
            public int Result { get; set; }
            public string RegNo { get; set; }
            public string Roll { get; set; }
            public string StudentName { get; set; }
            public string FatherName { get; set; }
            public int StudentID { get; set; }
        }
    }
}