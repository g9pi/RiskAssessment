using Microsoft.Extensions.Configuration;
using RiskAssessment.Models;
using RiskAssessment.Models.Data;
using RiskAssessment.Repositorys.IRepositorys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using static System.Collections.Specialized.BitVector32;

namespace RiskAssessment.Repositorys
{
    public class DivisionControlRepository : IDivisionControlRepository
    {
        private readonly string _connectionString;

        public DivisionControlRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MILDB");
        }
        public List<Divisions> getDivision()
        {
            try
            {
                List<Divisions> divisions = new List<Divisions>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"SELECT        DIV_ID,DIVCDE, ISACTIVE, IP_CRE, USECRE, CREDTE, IP_MOD, USEMOD, MODDTE
FROM            RA_DIV_MST";
                    command.CommandText = query;
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        divisions.Add(new Divisions
                        {
                            DivisionId = reader["DIV_ID"] != DBNull.Value ? (int?)reader["DIV_ID"] :null,
                            DivisionName = reader["DIVCDE"] != DBNull.Value ? (string)reader["DIVCDE"] : null,
                            IsActive = reader["ISACTIVE"] != DBNull.Value ? (bool)reader["ISACTIVE"] : false,
                            CreateUser = reader["USECRE"] != DBNull.Value ? (string)reader["USECRE"] : null,
                            CreateIp = reader["IP_CRE"] != DBNull.Value ? (string)reader["IP_CRE"] : null,
                            CreateDate = reader["CREDTE"] != DBNull.Value ? (DateTime?)reader["CREDTE"] : null,
                            ModifyUser = reader["USEMOD"] != DBNull.Value ? (string)reader["USEMOD"] : null,
                            ModifyIp = reader["IP_MOD"] != DBNull.Value ? (string)reader["IP_MOD"] : null,
                            ModifyDate = reader["MODDTE"] != DBNull.Value ? (DateTime?)reader["MODDTE"] : null
                        });
                    }
                    con.Close();
                }
                return divisions;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void insertDivision(Divisions input)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"INSERT INTO RA_DIV_MST (DIVCDE, ISACTIVE, IP_CRE, USECRE, CREDTE) VALUES (@DIVISION,@ISACTIVE,@IPCRE,@USECRE,GETDATE())";
                    command.CommandText = query;
                    con.Open();
                    command.Parameters.Add(new SqlParameter("@DIVISION", (object)input.DivisionName ?? DBNull.Value));
                
                    command.Parameters.Add(new SqlParameter("@ISACTIVE", (object)input.IsActive ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IPCRE", (object)input.CreateIp ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@USECRE", (object)input.CreateUser ?? DBNull.Value));
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void updateDivision(Divisions input)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"UPDATE RA_DIV_MST SET DIVCDE = @DIVISION,  ISACTIVE = @ISACTIVE, IP_MOD = @IPMOD, USEMOD = @USEMOD , MODDTE = GETDATE() WHERE DIV_ID = @DIVISION_ID";
                    command.CommandText = query;
                    con.Open();
                    command.Parameters.Add(new SqlParameter("@DIVISION", (object)input.DivisionName ?? DBNull.Value));                    
                    command.Parameters.Add(new SqlParameter("@ISACTIVE", (object)input.IsActive ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IPMOD", (object)input.CreateIp ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@USEMOD", (object)input.CreateUser ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@DIVISION_ID", (object)input.DivisionId ?? DBNull.Value));
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SectionsModel> getSection()
        {
            try
            {
                List<SectionsModel> sections = new List<SectionsModel>();
                
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"SELECT        SECTION_ID, DIVCDE, SECCDE, ISACTIVE, IP_CRE, USECRE, CREDTE, IP_MOD, USEMOD, MODDTE
FROM            RA_SECTION ORDER BY DIVCDE,ISACTIVE DESC,SECCDE";
                    command.CommandText = query;
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        SectionsModel section = new SectionsModel();
                        section.SectionId = reader["SECTION_ID"] != DBNull.Value ? (int?)reader["SECTION_ID"] : null;
                        section.Sectionname = reader["SECCDE"] != DBNull.Value ? (string)reader["SECCDE"] : null;
                        section.Division = reader["DIVCDE"] != DBNull.Value ? (string)reader["DIVCDE"] : null;
                        section.IsActive = reader["ISACTIVE"] != DBNull.Value ? (bool)reader["ISACTIVE"] : false;
                        section.CreateUser = reader["USECRE"] != DBNull.Value ? (string)reader["USECRE"] : null;
                        section.CreateIp = reader["IP_CRE"] != DBNull.Value ? (string)reader["IP_CRE"] : null;
                        section.CreateDate = reader["CREDTE"] != DBNull.Value ? (DateTime?)reader["CREDTE"] : null;
                        section.ModifyUser = reader["USEMOD"] != DBNull.Value ? (string)reader["USEMOD"] : null;
                        section.ModifyIp = reader["IP_MOD"] != DBNull.Value ? (string)reader["IP_MOD"] : null;
                        section.ModifyDate = reader["MODDTE"] != DBNull.Value ? (DateTime?)reader["MODDTE"] : null;
                        sections.Add(section);
                    }
                    con.Close();
                }
                return sections;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void insertSection(SectionsModel input)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"INSERT INTO RA_SECTION (DIVCDE, SECCDE, ISACTIVE, IP_CRE, USECRE, CREDTE) VALUES (@DIVISION,@SECTION,@ISACTIVE,@IPCRE,@USECRE,GETDATE())";
                    command.CommandText = query;
                    con.Open();
                    command.Parameters.Add(new SqlParameter("@DIVISION", (object)input.Division ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@SECTION", (object)input.Sectionname ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@ISACTIVE", (object)input.IsActive ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IPCRE", (object)input.CreateIp ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@USECRE", (object)input.CreateUser ?? DBNull.Value));
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void updateSection(SectionsModel input)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"UPDATE RA_SECTION SET DIVCDE = @DIVISION, SECCDE = @SECTION, ISACTIVE = @ISACTIVE, IP_MOD = @IPMOD, USEMOD = @USEMOD , MODDTE = GETDATE() WHERE SECTION_ID = @SECTION_ID";
                    command.CommandText = query;
                    con.Open();
                    command.Parameters.Add(new SqlParameter("@DIVISION", (object)input.Division ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@SECTION", (object)input.Sectionname ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@ISACTIVE", (object)input.IsActive ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IPMOD", (object)input.CreateIp ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@USEMOD", (object)input.CreateUser ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@SECTION_ID", (object)input.SectionId ?? DBNull.Value));
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Reviewer> getReviewer()
        {
            try
            {
                List<Reviewer> reviews = new List<Reviewer>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"SELECT        ROW_ID,REVIEW_TYP, SECTION_ID, USER_ID, IP_CRE, USECRE, CREDTE, LV
FROM            RA_REVIEWER_CTRL";
                    command.CommandText = query;
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        reviews.Add(new Reviewer
                        {
                            RowId = reader["ROW_ID"] != DBNull.Value ? (int?)reader["ROW_ID"] : null,
                            SectionId = reader["SECTION_ID"] != DBNull.Value ? (int?)reader["SECTION_ID"] : null,
                            UserId = reader["USER_ID"] != DBNull.Value ? (int?)reader["USER_ID"] : null,
                            LV = reader["LV"] != DBNull.Value ? (short?)reader["LV"] : null,
                            ReviewType = reader["REVIEW_TYP"] != DBNull.Value ? (string)reader["REVIEW_TYP"] : null,
                            CreateUser = reader["USECRE"] != DBNull.Value ? (string)reader["USECRE"] : null,
                            CreateIp = reader["IP_CRE"] != DBNull.Value ? (string)reader["IP_CRE"] : null,
                            CreateDate = reader["CREDTE"] != DBNull.Value ? (DateTime?)reader["CREDTE"] : null,
                            
                        });
                    }
                    con.Close();
                }
                return reviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReviewer(ReviewerInput input)
        {
            try
            {
               // DeleteReviewerBySectionId(input.SectionId);
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"INSERT INTO RA_REVIEWER_CTRL (SECTION_ID,USER_ID,LV,REVIEW_TYP,USECRE,IP_CRE,CREDTE)
VALUES (@SECTION_ID,@USER_ID,@LV,@REVIEW_TYPE,@USECRE,@IPCRE,GETDATE())";
                    command.CommandText = query;
                    con.Open();
                    foreach(var user in input.UserIds)
                    {
                        command.Parameters.Add(new SqlParameter("@SECTION_ID", (object)input.SectionId ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@USER_ID", (object)user?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@LV", (object)input.LV ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@REVIEW_TYPE", (object)input.ReviewType ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@USECRE", (object)input.CreateUser ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@IPCRE", (object)input.CreateIp ?? DBNull.Value));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }                                      
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteReviewerBySectionId(int? SectionId,short? level,string ReviewType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    string query = @"DELETE RA_REVIEWER_CTRL WHERE SECTION_ID = @SECTION_ID AND LV = @LEVEL AND REVIEW_TYP = @REVIEW_TYPE";
                    command.CommandText = query;
                    con.Open();                   
                    command.Parameters.Add(new SqlParameter("@SECTION_ID", (object)SectionId ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@LEVEL", (object)level ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@REVIEW_TYPE", (object)ReviewType ?? DBNull.Value));
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
