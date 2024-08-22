using StudentApiDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApiBusinessLayer
{
    public class Student
    {   
        public enum enMode {AddNew=0,Update=1};
        public  enMode Mode =enMode.AddNew;
        public StudentDTO SDTO { get { return new StudentDTO(this.Id,this.Name,this.Age,this.Grade); } }
        public Student( StudentDTO SDTO, enMode mode = enMode.AddNew)
        {
            Mode = mode;

            Id = SDTO.Id;
            Name = SDTO.Name;
            Age = SDTO.Age;
            Grade = SDTO.Grade;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age{ get; set; }
        public int  Grade{ get; set; }

        public static List<StudentDTO> GetAllStudent()
        {
            return StudentData.GetAllStudent();
        }
        public static List<StudentDTO> GetAllPassedStudent()
        {
            return StudentData.GetPassedStudent();
        }
        public static double GetStudentAverage()
        {
            return StudentData.GetAverageGrade();
        }

        public static Student Find(int id)
        {
            StudentDTO SDTO = StudentData.GetStudentById(id);
            if (SDTO!=null)
            {
                return new Student(SDTO,enMode.Update);
            }
            else 
            {
                return null;
            }
        }

        private bool _AddNewStudent()
        {
            this.Id = StudentData.AddStudent(SDTO);
            return (this.Id != -1);
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                case enMode.Update:
                    if (_UpdateStudent())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }

        public static bool DeleteStudent(int id )
        {
            return StudentData.DeleteStudent(id);
        }
    }
}
