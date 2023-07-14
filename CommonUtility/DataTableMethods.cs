using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class DataTableMethods
    {

        #region Filter a datatable
        //string expression = "ShortName NOT ='" + "NULL" + "' AND ShortName NOT ='" + "0" + "' AND ShortName NOT ='" + "" + "'";

        public static DataTable FilterDataTable(DataTable dt, string expression)
        {
            DataTable dt1 = new DataTable();

            if (dt.Rows.Count > 0)
            {

                DataRow[] filteredrows = dt.Select(expression);
                if (filteredrows.Length > 0)
                    dt1 = filteredrows.CopyToDataTable();
                else
                    dt1.Clear();
            }

            return dt1;
        }

        #endregion

        #region List To DataTable
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion

        #region DataTable To List
        //Calling it and converting it to List. 
        //List< Student > studentDetails = new List< Student >();  
        //studentDetails = ConvertDataTable< Student >(dt);

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion

        #region Data Table To JSON

        public static string DataTableToJsonConvert(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }

        #endregion

        #region DataLength Coloumn Add For Sorting
        public static DataTable AddNewColoumnForLengthSorting(DataTable dt, string LengthField)
        {
            try
            {
                /// Add Column
                DataColumn dc = new DataColumn();
                dc.ColumnName = "Length";
                dc.DataType = typeof(int);
                dt.Columns.Add(dc);

                //Add Row Value in new column 
                foreach (DataRow row in dt.Rows)
                {
                    int length = Convert.ToInt32(row[LengthField].ToString().Length);

                    row["Length"] = length;
                }


                return dt;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
