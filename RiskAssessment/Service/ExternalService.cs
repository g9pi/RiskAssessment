using System.Data.SqlClient;
using System;
using RiskAssessment.Repositorys.IRepositorys;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RiskAssessment.Service.IService;
using RiskAssessment.Models;
using RiskAssessment.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskAssessment.Service
{
    public class ExternalService : IExternalService
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly string _connectionString;

   
        public ExternalService(IUserAccountRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _connectionString = configuration.GetConnectionString("MILDB");
        }
        public List<SelectListItem> getSections()
        {
            try
            {
                List<SelectListItem> ddl = new List<SelectListItem>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string queryString = @"SELECT DISTINCT SECTION_CODE FROM TDP_EMPLOYEE ORDER BY SECTION_CODE";
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = queryString;
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ddl.Add(new SelectListItem
                        {
                            Text = reader["SECTION_CODE"] != DBNull.Value ? (string)reader["SECTION_CODE"] : null,
                            Value = reader["SECTION_CODE"] != DBNull.Value ? (string)reader["SECTION_CODE"] : null,
                        });
                    }
                    con.Close();
                }
                return ddl; 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeModel> getEmployee()
        {
            try
            {
                List<EmployeeModel> list = new List<EmployeeModel>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string queryString = @"SELECT DISTINCT * FROM (SELECT CAST(EMPLOYEE_ID as INTEGER) as EMPLOYEE_ID, Employee_no, Employee_name, EMPLOYEE_LOCAL_NAME, DIVISION_CODE, DEPARTMENT_CODE, SECTION_CODE, GRADE_CODE, POSITION_DESCRIPTION, EMPLOYEE_STATUS, DATE_JOINED FROM TDP_Employee 
  union all select null, EMPNUM, OLD_EMPNAM, EMPNAM, LEFT(seccde, 1) + '000', left(seccde, 2) + '00', seccde, levcde, POSNAM, empstt, JOIDTE from TDP_EMPLOYEE_MASTER WHERE EMPSTT ='A') AS TDP_Employee 
WHERE Employee_STATUS = 'A'";
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = queryString;
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add (new EmployeeModel
                        {
                            EmployeeId = reader["Employee_id"] != DBNull.Value ? (int?)reader["Employee_id"] : null,
                            EmployeeNo = reader["Employee_no"] != DBNull.Value ? reader["Employee_no"].ToString() : null,
                            EmployeeName = reader["Employee_name"] != DBNull.Value ? (string)reader["Employee_name"] : null,
                            EmployeeNameTH = reader["Employee_local_name"] != DBNull.Value ? (string)reader["Employee_local_name"] : null,
                            DivisionNo = reader["Division_code"] != DBNull.Value ? (string)reader["Division_code"] : null,
                            DepartmentNo = reader["Department_code"] != DBNull.Value ? (string)reader["Department_code"] : null,
                            SectionNo = reader["Section_code"] != DBNull.Value ? (string)reader["Section_code"] : null,
                            Level = reader["Grade_code"] != DBNull.Value ? (string)reader["Grade_code"] : null,
                            Position = reader["Position_description"] != DBNull.Value ? (string)reader["Position_description"] : null,
                            JoinedDate = reader["DATE_JOINED"] != DBNull.Value ? (DateTime?)reader["DATE_JOINED"] : null,
                            
                        });
                        list.Last().setDivision();
                    }
                    reader.Close();
                    con.Close();
                }
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateUserStatusFromEmployeeNo()
        {
            try
            {
                
                var users = _userRepository.getUsers().Where(m => m.Isactive).ToList();
                users.RemoveAll(x => x.EmployeeNo == null);
                List<string> resign_users = new List<string>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string queryString = null;
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    queryString = "SELECT DISTINCT EMPLOYEE_NO,EMPLOYEE_STATUS FROM TDP_EMPLOYEE WHERE EMPLOYEE_STATUS <> 'A' AND EMPLOYEE_NO IN (";

                    for (int i = 0; i < users.Count(); i++)
                    {
                        queryString += "@ACC"+i;
                        if (i != users.Count() - 1)
                        {
                            queryString += ",";
                        }
                        command.Parameters.AddWithValue("@ACC" + i, users[i].EmployeeNo);
                    }
                    queryString += ")";
                    command.CommandText = queryString;
                    //for(int i = 0; i < users.Count(); i++)
                    //{
                    //    command.Parameters.AddWithValue("@ACC"+i, users[i].EmployeeNo);
                    //}
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        resign_users.Add(reader["EMPLOYEE_NO"] != DBNull.Value ? (string)reader["EMPLOYEE_NO"] : null);
                    }
                    con.Close();
                    if(resign_users.Count>0)
                        updateResignUser(resign_users);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void updateResignUser(List<string> emp_no)
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand();
                command.Connection = con;

                string query = "UPDATE RA_USER_ACCOUNT SET MODDTE = GETDATE(), USEMOD = 'System',ISACTIVE = 0 WHERE EMP_NO IN (";
                for (int i = 0; i < emp_no.Count(); i++)
                {

                    query += "@EMP_NO" + i;
                    command.Parameters.AddWithValue("@EMP_NO" + i, (object)emp_no[i] ?? DBNull.Value);
                    if (i != emp_no.Count() - 1)
                    {
                        query += ",";
                    }
                    else
                    {
                        query += ")";
                    }
                }
                command.CommandText = query;
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
