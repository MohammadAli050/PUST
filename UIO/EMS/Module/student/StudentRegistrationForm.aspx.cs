using LogicLayer.BusinessLogic;
using CommonUtility;
using LogicLayer.BusinessObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace EMS.Module.student
{
    public partial class StudentRegistrationForm : BasePage
    {

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {


                ucDepartment.LoadDropDownList();
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHallInfo();
                LoadHeldInInformation();

                if (UserObj.RoleID == 4) // Student
                {
                    //txtStudentId.Text = UserObj.LogInID.ToString();
                    btnLoad_Click(null, null);
                }
                else
                {
                }
            }
        }

        //protected void btn_Click(object sender, EventArgs e)
        //{

        //}

        //protected void btn_Download(object sender, EventArgs e)
        //{

        //}

        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                ClearGridView();
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
                ClearGridView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                //LoadStudentInformation();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHallInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
            }
            catch (Exception ex)
            {
            }

        }

        #endregion

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

        private void LoadHallInfo()
        {
            try
            {
                var HallList = ucamContext.HallInformations.Where(x => x.ActiveStatus == 1).ToList();

                ddlHallInfo.Items.Clear();
                ddlHallInfo.AppendDataBoundItems = true;
                ddlHallInfo.Items.Add(new ListItem("Select", "0"));

                if (HallList != null && HallList.Any())
                {
                    ddlHallInfo.DataTextField = "HallName";
                    ddlHallInfo.DataValueField = "Id";
                    ddlHallInfo.DataSource = HallList.OrderBy(x => x.HallName);
                    ddlHallInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }



        #endregion

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int StudentId = 0;
                int HallInfoId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                if (!string.IsNullOrEmpty(txtStudentId.Text))
                {
                    string Roll = txtStudentId.Text.Trim();

                    var StudentObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();

                    if (StudentObj != null)
                    {
                        StudentId = StudentObj.StudentID;
                    }
                }

                if (HeldInRelationId == 0)
                {
                    showAlert("Please Choose Semester & Held In");
                    return;
                }
                if (ProgramId == 0 && StudentId == 0)
                {
                    showAlert("Please Choose a program or enter a student Id");
                    return;
                }

                LoadStudentInformation();

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadStudentInformation()
        {
            try
            {
                ClearGridView();
                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInRelationId > 0)
                {
                    int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                    int HallId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });
                    parameters1.Add(new SqlParameter { ParameterName = "@HallId", SqlDbType = System.Data.SqlDbType.Int, Value = HallId });

                    DataTable DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllRegisteredStudentListByHeldInId", parameters1);

                    DataTable distinctStudents = DataTableStudentList.DefaultView.ToTable(true, "ShortName", "AdmissionSession", "Roll", "FullName", "HallCode");

                    if (distinctStudents != null && distinctStudents.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtStudentId.Text.Trim()))
                        {
                            string Roll = txtStudentId.Text.Trim();
                            string Expression = "Roll ='" + Roll + "'";
                            distinctStudents = DataTableMethods.FilterDataTable(distinctStudents, Expression);
                        }
                    }

                    if (distinctStudents != null && distinctStudents.Rows.Count > 0)
                    {
                        gvStudentList.DataSource = distinctStudents;
                        gvStudentList.DataBind();
                    }
                    else
                    {
                        showAlert("No Student Found");
                        return;
                    }

                }
                else
                {
                    showAlert("Please choose a held in");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearGridView()
        {
            try
            {
                gvStudentList.DataSource = null;
                gvStudentList.DataBind();
            }
            catch (Exception ex)
            {
            }
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

                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [WebMethod]
        public static string GetStudentInformationForRegistrationCard(string RollList)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string[] rollList = RollList.Split(','); 

                DataTable dt = new DataTable();

                if (rollList != null && rollList.Any())
                {
                    foreach (var item in rollList)
                    {
                        List<SqlParameter> parameters1 = new List<SqlParameter>();
                        parameters1.Add(new SqlParameter { ParameterName = "@StudentRoll", SqlDbType = System.Data.SqlDbType.NVarChar, Value = item });

                        DataTable dtNew = DataTableManager.GetDataFromQuery("GetStudentInformationForRegistrationCard", parameters1);

                        if (dtNew != null && dtNew.Rows.Count > 0)
                        {
                            dt.Merge(dtNew);
                        }
                    }
                }


                if (dt != null && dt.Rows.Count > 0)
                {

                    responseAPI.ResponseCode = 200;
                    responseAPI.ResponseStatus = "Success";
                    responseAPI.ResponseMessage = "";
                    responseAPI.ResponseData = dt;
                }
                else
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "No Data Found!";
                    responseAPI.ResponseData = "";
                }
            }
            catch (Exception ex)
            {
                responseAPI.ResponseCode = 400;
                responseAPI.ResponseStatus = "Failed";
                responseAPI.ResponseMessage = "Exception: " + ex.Message.ToString();
                responseAPI.ResponseData = "";
            }

            string jsonString = JsonConvert.SerializeObject(responseAPI);

            return jsonString;
        }




        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


    }
}