using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApiBusinessLayer;
using StudentApiDataAccessLayer;

namespace StudentApiServerSide.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("GetAllStudent",Name ="GetAllStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudent()
        {
            List<StudentDTO> studentList= Student.GetAllStudent();
            if (studentList.Count == 0)
            {
                return NotFound("Not Found Data");
            }
            return Ok(studentList);
        }


        [HttpGet("StudentPassed",Name = "StudentPassed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudentPassed()
        {
            var StudentPassed = Student.GetAllPassedStudent();
            if (StudentPassed==null)
            {
                return NotFound("NO Student Passed");
            }
            
                return Ok(StudentPassed);
            
        }


        [HttpGet("StudentAverage", Name = "StudentAverage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGradeForStudent()
        {
            double avgGrade = Student.GetStudentAverage(); 
            if(avgGrade < 0)
            {
                return NotFound("Not Found Average");
            }
            return Ok(avgGrade);
        }


        [HttpGet("FindStudentById",Name = "FindStudentById")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id<0)
            {
                return BadRequest($"No accepted ID : {id}");
            }
            Student student = Student.Find(id);
            if (student==null)
            {
                return NotFound($"Not Found this Id = {id}");
            }
            StudentDTO SDTO = student.SDTO;
            return Ok(SDTO);
        }


        [HttpPost("AddStudent",Name ="AddStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<StudentDTO> AddStudent (StudentDTO studentDTO)
        {
            if (studentDTO == null || String.IsNullOrEmpty(studentDTO.Name) || studentDTO.Age<18 || studentDTO.Grade<10)
            {
                return BadRequest("Invalid Student data.");
            }
            Student newStudent = new Student(studentDTO);
            newStudent.Save();

            studentDTO.Id= newStudent.Id;

            return CreatedAtRoute("FindStudentById", studentDTO);
        }
         

        [HttpPut("UpdateStudent",Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> UpdateStudent(int id,StudentDTO studentDTO)
        {
            if (id<0)
            {
                return BadRequest("Invalid ID");
            }
            var oldStudent = Student.Find(id);
            if(oldStudent==null)
            {
                return NotFound("Not Found with this Id");
            }
            oldStudent.Name = studentDTO.Name;
            oldStudent.Age = studentDTO.Age;
            oldStudent.Grade = studentDTO.Grade;
            oldStudent.Save();
            
            return Ok(oldStudent.SDTO);

        }


        [HttpDelete("DeleteStudent",Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id<0)
            {
                return BadRequest("Invalid ID");
            }
            if (Student.DeleteStudent(id))
            {
                return Ok($"Student with ID {id} has been deleted.");
            }
            else
            {
                return NotFound($"Student with ID {id} not found. no rows deleted!");

            }
        }



    }
}
