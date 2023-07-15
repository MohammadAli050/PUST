using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class StudentReAdmission : BasePage
    {

        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentReAdmission);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentReAdmission));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int StudentId = 0;

                if (string.IsNullOrEmpty(txtStudentId.Text))
                {
                    showAlert("Please enter a StudentId");
                    return;
                }

                string Roll = "";

                Roll = txtStudentId.Text.Trim();

                var stdObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();
                if (stdObj != null)
                    StudentId = stdObj.StudentID;

                if (StudentId > 0)
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });

                    DataTable dt = DataTableManager.GetDataFromQuery("GetStudentReAdmissionInfo", parameters1);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lblName.Text = dr["FullName"].ToString();
                            lblAdmissionSession.Text = dr["AdmissionSession"].ToString();
                            lblReAdmissionSession.Text = dr["ReadmissionSession"].ToString();
                            lblProgram.Text = dr["DetailName"].ToString();
                        }
                    }

                }
                else
                {
                    showAlert("Enter valid studentId");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnReAdmission_Click(object sender, EventArgs e)
        {
            try
            {
                int StudentId = 0;

                int ReAdSessionId = Convert.ToInt32(admissionSession.selectedValue);

                if (ReAdSessionId == 0)
                {
                    showAlert("Please choose a Re-Admission Session");
                    return;
                }

                if (string.IsNullOrEmpty(txtStudentId.Text))
                {
                    showAlert("Please enter a student Id");
                    return;
                }

                string Roll = txtStudentId.Text.Trim();

                var StudentObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();

                if (StudentObj != null)
                {
                    StudentId = StudentObj.StudentID;

                    var ExistingObj = ucamContext.StudentReAdmissionInformations.Where(x => x.StudentId == StudentId && x.ReAdmissionSessionId == ReAdSessionId).FirstOrDefault();

                    if (ExistingObj == null) // New Entry
                    {
                        #region Update All Prevoius Entry

                        var prevList = ucamContext.StudentReAdmissionInformations.Where(x => x.StudentId == StudentId && x.ActiveStatus == 1).ToList();

                        if (prevList != null && prevList.Any())
                        {
                            foreach(var item in prevList)
                            {
                                item.ActiveStatus = 0;
                                item.ModifiedBy = UserObj.Id;
                                item.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(item).State = EntityState.Modified;
                                ucamContext.SaveChanges();
                            }
                        }
                        #endregion

                        #region New Entry

                        DAL.StudentReAdmissionInformation NewObj = new DAL.StudentReAdmissionInformation();

                        NewObj.StudentId = StudentId;
                        NewObj.ReAdmissionSessionId = ReAdSessionId;
                        NewObj.ActiveStatus = 1;
                        NewObj.CreatedBy = UserObj.Id;
                        NewObj.CreatedDate = DateTime.Now;

                        ucamContext.StudentReAdmissionInformations.Add(NewObj);
                        ucamContext.SaveChanges();

                        #endregion

                        if(NewObj.Id>0)
                        {
                            showAlert("Student Re-Admitted Successfully");
                            btnLoad_Click(null, null);
                            return;
                        }
                        else
                        {
                            showAlert("Re-Admission Failed");
                            return;
                        }
                    }
                    else
                    {
                        showAlert("Already Re-Admitted in choosen session");
                        return;
                    }

                }
                else
                {
                    showAlert("Please provide a valid student Id");
                    return;

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


    }
}