using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApiDataAccessLayer
{   
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
    public class StudentData
    {
        static string _connectionString = "Data Source=.;Initial Catalog=StudentsDB;Integrated Security=True;Trust Server Certificate=True";

        public static List<StudentDTO> GetAllStudent()
        {
            var StudentsList = new List<StudentDTO>();
            using (SqlConnection con = new SqlConnection(_connectionString) )
            {
                using(SqlCommand cmd = new SqlCommand("SP_GetAllStudents", con))
                { 
                    cmd.CommandType=CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader()) 
                    { 
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))

                                ));
                        }
                    }
                }
            }
            return StudentsList;
        }
        public static List<StudentDTO> GetPassedStudent()
        {
            var StudentPassedList = new List<StudentDTO>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            StudentPassedList.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }

                }
            }
            return StudentPassedList;
        }

        public static double GetAverageGrade()
        {
            double averageGrade = 0;
            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", con))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result!=DBNull.Value)
                    {
                        averageGrade = Double.Parse(result.ToString());

                    }
                    else
                    {
                        averageGrade = 0;
                    }
                }
            }
            return averageGrade;
        }

        public static StudentDTO GetStudentById(int id )
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", id);
                    con.Open();

                    using (var reader=cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static int AddStudent(StudentDTO studentDTO)
        {
            using (SqlConnection con = new SqlConnection(_connectionString) )
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", studentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", studentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                    var outputId = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    cmd.Parameters.Add(outputId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                    return (int)outputId.Value;
                }
            }
        }

        public static bool UpdateStudent(StudentDTO studentDTO)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_UpdateStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentDTO.Id);
                    cmd.Parameters.AddWithValue("@Name", studentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", studentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", studentDTO.Grade);
                    
                    con.Open() ;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteStudent(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString) )
            {
                using (SqlCommand cmd = new SqlCommand("SP_DeleteStudent", con))
                {   cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", id);
                    con.Open();

                     int rowAffected =(int)cmd.ExecuteScalar();
                     return rowAffected>0?true:false;   

                }
            }    
        }
    }
}
