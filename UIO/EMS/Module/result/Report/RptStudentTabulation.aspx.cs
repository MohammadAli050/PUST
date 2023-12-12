using CommonUtility;
using LogicLayer.BusinessLogic;
using Microsoft.Reporting.WebForms;
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

namespace EMS.Module.result.Report
{
    public partial class RptStudentTabulation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static string LoadStudentTabulationData(string stdRoll)
        {
            try
            {
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@Roll", SqlDbType = System.Data.SqlDbType.VarChar, Value = stdRoll });
                parameters2.Add(new SqlParameter { ParameterName = "@Roll", SqlDbType = System.Data.SqlDbType.VarChar, Value = stdRoll });
                DataTable dt1 = DataTableManager.GetDataFromQuery("GetStudentInfoForTabulation", parameters1);
                DataTable dt2 = DataTableManager.GetDataFromQuery("GetStudentAcademicInfoForTabulation", parameters2);

                var student = StudentManager.GetByRoll(stdRoll);

                var program = ProgramManager.GetById(student.ProgramID);


                var studentInfo = "";
                var academicInfo = "";              

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    studentInfo = DataTableMethods.DataTableToJsonConvert(dt1);                    
                }
                if(dt2 != null && dt2.Rows.Count > 0)
                {
                    academicInfo = DataTableMethods.DataTableToJsonConvert(dt2);
                }
                else
                {
                    return "";
                }

                var response = new Dictionary<string, object>
                {
                    {"studentInfo", dt1},
                    {"academicInfo",dt2 },
                    {"department",program.DetailName },
                };

                return JsonConvert.SerializeObject(response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;

            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            showHide.Visible = true;
            UpdatePanel02.Update();
        }
    }
}