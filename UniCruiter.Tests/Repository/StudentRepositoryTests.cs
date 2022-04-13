using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCruiter.Tests.Fixtures;
using UniCruiter.Tests.Helpers;
using UniCruiter.Models;
using UniCruiter.Repository;
using UniCruiter.ViewModels;
using Xunit;

namespace UniCruiter.Tests
{
    public class StudentRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly SharedDatabaseFixture _fixture;
        private readonly IStudentRepository _repo;

        public StudentRepositoryTests(SharedDatabaseFixture fixture)
        {
            _fixture = fixture;
            _repo = new StudentRepository(_fixture.CreateContext());
        }

        [Fact]
        public async Task Get_Students()
        {
            // Arrange.
            StudentViewModel svm = new StudentViewModel(new Student());

            // Act.
            IList<Student> students = await _repo.GetStudents(svm);

            // Assert.
            Assert.Equal(4, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_4, s.LastName));
        }

        [Fact]
        public async void Get_Students_With_Comments()
        {
            // Arrange.
            StudentViewModel svm = new StudentViewModel(new Student());

            // Act.
            IList<Student> students = await _repo.GetStudents(svm);

            // Assert.
            Assert.Equal(4, students.Count);

            foreach (var student in students)
            {
                Assert.True(student.Comments.Count == 1);
            }
        }

        [Fact]
        public async Task Get_Students_BySearch_None()
        {
            // Arrange.
            StudentViewModel svm = new StudentViewModel(new Student());

            svm.LastName = "qqqxxx";

            // Act.
            IList<Student> students = await _repo.GetStudents(svm);

            // Assert.
            Assert.Equal(0, students.Count);
        }

        [Fact]
        public async Task Get_Students_BySearch_Many()
        {
            // Arrange.
            StudentViewModel svm = new StudentViewModel(new Student());

            // Act.
            IList<Student> students = await _repo.GetStudents(svm);

            // Assert.
            Assert.Equal(4, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_4, s.LastName));
        }

        [Fact]
        public async Task Get_Students_BySearch_Single()
        {
            // Arrange.
            StudentViewModel svm = new StudentViewModel(new Student());

            svm.LastName = Constants.LAST_NAME_1;

            // Act.
            IList<Student> students = await _repo.GetStudents(svm);

            // Assert.
            Assert.Equal(1, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));
        }

        [Fact]
        public async Task Insert_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = "Test",
                LastName = "Insert"
            };

            // Act.
            Student newStudent = await _repo.InsertStudent(viewModel);
            Student student = await _repo.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);

            // Cleanup.
            await _repo.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Update_Student()
        {
            // Arrange.
            string tempLastName = "Update_Update";

            StudentViewModel viewModel = new()
            {
                FirstName = "Test",
                LastName = "Update"
            };

            // Act.
            Student newStudent = await _repo.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            viewModel.FirstName = newStudent.FirstName;
            viewModel.LastName = tempLastName;

            Student student = await _repo.UpdateStudent(viewModel);

            // Assert.
            Assert.IsAssignableFrom<Student>(student);
            Assert.Equal(student.LastName, tempLastName);

            // Cleanup.
            await _repo.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Delete_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = "Test",
                LastName = "Delete"
            };

            // Act.
            Student newStudent = await _repo.InsertStudent(viewModel);

            int id = newStudent.Id;
            await _repo.DeleteStudent(id);

            Student student = await _repo.GetStudentByID(id);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Delete_Student_With_Comment()
        {
            // Arrange.
            var viewModel = new StudentViewModel();

            viewModel.FirstName = "Test";
            viewModel.LastName = "Delete";
            viewModel.CommentEnteredBy = null;
            viewModel.CommentEnteredOn = DateTime.Now;
            viewModel.CommentText = "This is a test comment";

            // Act.
            Student newStudent = await _repo.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            _ = await _repo.InsertComment(viewModel);

            int id = newStudent.Id;
            await _repo.DeleteStudent(id);

            Student student = await _repo.GetStudentByID(id);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Get_Student_ById()
        {
            // Arrange.
            int studentId = 1;

            // Act.
            Student student = await _repo.GetStudentByID(studentId);

            // Assert.
            Assert.Equal(student.LastName, Constants.LAST_NAME_1);
        }

        [Fact]
        public async Task Get_Student_ById_NotFound()
        {
            // Arrange.
            int studentId = -1;

            // Act.
            Student student = await _repo.GetStudentByID(studentId);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Get_Student_ById_With_Comments()
        {
            // Arrange.
            int studentId = 1;

            // Act.
            Student student = await _repo.GetStudentByID(studentId);

            // Assert.
            Assert.True(student.Comments.Count() > 0);
        }

        [Fact]
        public async Task Get_Student_ById_After_Insert()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = "Test",
                LastName = "GetById"
            };

            // Act.
            Student newStudent = await _repo.InsertStudent(viewModel);
            Student sudent = await _repo.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, sudent);
            Assert.Equal(sudent.LastName, viewModel.LastName);

            // Cleanup.
            await _repo.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Get_Comments()
        {
            // Arrange.

            // Act.
            IList<Comment> comments = await _repo.GetComments();

            // Assert.
            Assert.Equal(4, comments.Count);
        }

        [Fact]
        public async Task Insert_Comment()
        {
            // Arrange.
            int studentId = 1;
            DateTime commentEnteredOn = DateTime.Now;
            string commentText = "Good interview.";

            var viewModel = new StudentViewModel();

            viewModel.Id = studentId;
            viewModel.CommentEnteredOn = commentEnteredOn;
            viewModel.CommentEnteredBy = null;
            viewModel.CommentText = commentText;

            // Act.
            Comment comment = await _repo.InsertComment(viewModel);

            // Assert.
            Assert.Equal(commentEnteredOn, comment.EnteredOn);
            Assert.Equal(commentText, comment.Text);

            // Cleanup.
            await _repo.DeleteComment(comment.Id);
        }
    }
}

