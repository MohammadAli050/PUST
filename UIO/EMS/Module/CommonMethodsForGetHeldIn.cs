using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class CommonMethodsForGetHeldIn
    {
        public static DataTable GetExamHeldInInformation(int ProgramId, int AcacalId, int YearNo, int SemesterNo)
        {
            DataTable dt = new DataTable();

            try
            {
                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters2.Add(new SqlParameter { ParameterName = "@AcacalId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalId });
                parameters2.Add(new SqlParameter { ParameterName = "@YearNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@SemesterNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });

                DataTable DataTableHeldInList = DataTableManager.GetDataFromQuery("GetAllHeldInInformation", parameters2);

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                    dt = DataTableHeldInList;

            }
            catch (Exception ex)
            {
            }

            return dt;
        }
    }
}