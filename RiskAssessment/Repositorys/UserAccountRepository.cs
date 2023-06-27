using Microsoft.Extensions.Configuration;
using RiskAssessment.Models;
using RiskAssessment.Repositorys.IRepositorys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RiskAssessment.Repositorys
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly string _connectionString;

        public UserAccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MILDB");
        }
        public void insertUser(UserModel user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandText = @"INSERT INTO RAS_USER_ACCOUNT (USENAM,PSSWRD,EMP_NO,NAME,EMAIL,DIVCDE,SECCDE,USEGRP,ISACTIVE,USECRE,IP_CRE,CREDTE)
                                                   VALUES (@USERNAME,@PASSWORD,@EMP_NO,@NAME,@EMAIL,@DIVISION,@SECTION,@GROUP,1,@USECRE,@IPCRE,GETDATE())";
                command.Parameters.Add(new SqlParameter("@USERNAME", (object)user.Username ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EMP_NO", (object)user.EmployeeNo ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EMAIL", (object)user.Email ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@PASSWORD", (object)user.Password ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@NAME", (object)user.Name ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@GROUP", (object)user.Group ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@DIVISION", (object)user.Division ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SECTION", (object)user.Section ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@USECRE", (object)user.CreateUser ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IPCRE", (object)user.CreateIp ?? DBNull.Value));
                con.Open();
                command.ExecuteNonQuery();                
                con.Close();
            }
        }

        public void updateUser(UserModel user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandText = @"UPDATE RAS_USER_ACCOUNT SET EMP_NO = @EMP_NO,NAME = @NAME,EMAIL = @EMAIL,DIVCDE = @DIVISION,SECCDE = @SECTION,USEGRP = @GROUP,IMG_FILE = @IMG_FILE,
USEMOD = @USEMOD,IP_MOD = @IPMOD,MODDTE = GETDATE() WHERE USER_ID = @USER_ID";
                //command.Parameters.Add(new SqlParameter("@USERNAME", (object)user.Username ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EMP_NO", (object)user.EmployeeNo ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EMAIL", (object)user.Email ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IMG_FILE", (object)user.ImageName ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@NAME", (object)user.Name ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@GROUP", (object)user.Group ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@DIVISION", (object)user.Division ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SECTION", (object)user.Section ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@USEMOD", (object)user.CreateUser ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IPMOD", (object)user.CreateIp ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@USER_ID", (object)user.UserId?? DBNull.Value));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void updateStatusUser(UserModel user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RAS_USER_ACCOUNT SET ISACTIVE = @ISACTIVE,USEMOD = @USEMOD,IP_MOD = @IPMOD,MODDTE = GETDATE() WHERE USER_ID = @USER_ID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@USER_ID", (object)user.UserId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ISACTIVE", (object)user.Isactive ?? DBNull.Value);
                command.Parameters.Add(new SqlParameter("@USEMOD", (object)user.CreateUser ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IPMOD", (object)user.CreateIp ?? DBNull.Value));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdatePassword(UserModel user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RAS_USER_ACCOUNT SET PSSWRD = @PASSWORD,USEMOD = @USEMOD,IP_MOD = @IPMOD,MODDTE = GETDATE() WHERE USER_ID = @USER_ID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@USER_ID", (object)user.UserId ?? DBNull.Value);
                command.Parameters.AddWithValue("@PASSWORD", (object)user.Password ?? DBNull.Value);
                command.Parameters.Add(new SqlParameter("@USEMOD", (object)user.CreateUser ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IPMOD", (object)user.CreateIp ?? DBNull.Value));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<UserModel> getUsers()
        {
            List<UserModel> user = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandText = "SELECT * FROM RAS_USER_ACCOUNT";
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Add(new UserModel
                    {
                        UserId = reader["USER_ID"] != DBNull.Value ? (int?)reader["USER_ID"] : null,
                        Username = reader["USENAM"] != DBNull.Value ? (string)reader["USENAM"] : null,
                        Password = reader["PSSWRD"] != DBNull.Value ? (string)reader["PSSWRD"] : null,
                        EmployeeNo = reader["EMP_NO"] != DBNull.Value ? (string)reader["EMP_NO"] : null,
                        Name = reader["NAME"] != DBNull.Value ? (string)reader["NAME"] : null,
                        Email = reader["EMAIL"] != DBNull.Value ? (string)reader["EMAIL"] : null,
                        Division = reader["DIVCDE"] != DBNull.Value ? (string)reader["DIVCDE"] : null,
                        Section = reader["SECCDE"] != DBNull.Value ? (string)reader["SECCDE"] : null,
                        Group = reader["USEGRP"] != DBNull.Value ? (string)reader["USEGRP"] : null,
                        Isactive = reader["ISACTIVE"] != DBNull.Value ? (bool)reader["ISACTIVE"] : false,
                        ImageName = reader["IMG_FILE"] != DBNull.Value ? (string)reader["IMG_FILE"] : null,
                        CreateUser = reader["USECRE"] != DBNull.Value ? (string)reader["USECRE"] : null,
                        CreateIp = reader["IP_CRE"] != DBNull.Value ? (string)reader["IP_CRE"] : null,
                        CreateDate = reader["CREDTE"] != DBNull.Value ? (DateTime?)reader["CREDTE"] : null,
                        ModifyUser = reader["USEMOD"] != DBNull.Value ? (string)reader["USEMOD"] : null,
                        ModifyIp = reader["IP_MOD"] != DBNull.Value ? (string)reader["IP_MOD"] : null,
                        ModifyDate = reader["MODDTE"] != DBNull.Value ? (DateTime?)reader["MODDTE"] : null
                    }) ;
                }
                con.Close();
            }
            return user;
        }
        
    }
}
