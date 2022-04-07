using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCruiter.Models;
using UniCruiter.ViewModels;

namespace UniCruiter.Repository
{
    public interface IStudentRepository : IDisposable
    {
        Task<IList<Student>> GetStudents(StudentViewModel studentViewModel);
        Task<Student> GetStudentByID(int studentID);
        Task<Student> InsertStudent(StudentViewModel studentViewModel);
        Task DeleteStudent(int studentID);
        Task<Student> UpdateStudent(StudentViewModel studentViewModel);
    }
}
