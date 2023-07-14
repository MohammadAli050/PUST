﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LogicLayer.DataLogic.SQLRepository
{
    public class SqlDataTableRepository : IDataTableRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString;        

        public virtual DataTable GetDataFromQuery(string SPName)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(SPName))
            {
                return dt;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(SPName, connection);
                command.CommandType = System.Data.CommandType.Text;
                try
                {
                    connection.Open();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        public virtual DataTable GetDataFromQuery(string SPName, List<SqlParameter> parameterList)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(SPName))
            {
                return dt;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(SPName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var item in parameterList)
                {
                    command.Parameters.Add(item);
                }
                try
                {
                    connection.Open();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }
    }
}
