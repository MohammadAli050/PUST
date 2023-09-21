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
    public partial class RptAcademicGradeSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string Loadacademicgradesheet(string roll)
        {
            try
            {
                var studentInfo = "";
                var academicInfo = "";

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                List<SqlParameter> parameters2 = new List<SqlParameter>();

                parameters1.Add(new SqlParameter { ParameterName = "@roll", SqlDbType = System.Data.SqlDbType.NVarChar, Value = roll });
                parameters2.Add(new SqlParameter { ParameterName = "@roll", SqlDbType = System.Data.SqlDbType.NVarChar, Value = roll });

                DataTable dt = DataTableManager.GetDataFromQuery("GetAcademicGradeSheetByRoll", parameters1);
                DataTable dt1 = DataTableManager.GetDataFromQuery("GetStudentInfoForTabulation", parameters2);

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    studentInfo = DataTableMethods.DataTableToJsonConvert(dt1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    academicInfo = DataTableMethods.DataTableToJsonConvert(dt);
                }
                else
                {
                    return "";
                }

                var response = new Dictionary<string, object>
                {
                    {"studentInfo", dt1},
                    {"academicInfo",dt },
                };
                return JsonConvert.SerializeObject(response);
            }
            catch
            {
                return null;
            }
        }
    }
}