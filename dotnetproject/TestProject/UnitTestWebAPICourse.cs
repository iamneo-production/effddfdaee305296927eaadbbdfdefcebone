using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetapiapp.Controllers;
using dotnetapiapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetapiapp.Tests
{
   [TestFixture]
    public class CourseControllerTests
    {
        private CourseController _CourseController;
        private CourseEnquiryDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<CourseEnquiryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CourseEnquiryDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Courses.AddRange(new List<Course>
            {
                new Course { CourseID = 1, CourseName = "Course 1", Description = "Course Description1", Duration = "70",Cost=12000 },
                new Course { CourseID = 2, CourseName = "Course 2", Description = "Course Description2", Duration = "80",Cost=24000 },
                new Course { CourseID = 3, CourseName = "Course 3", Description = "Course Description3", Duration = "90",Cost=32000 }
            });
            _context.SaveChanges();

            _CourseController = new CourseController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void CourseClassExists()
        {
            // Arrange
            Type CourseType = typeof(Course);

            // Act & Assert
            Assert.IsNotNull(CourseType, "Course class not found.");
        }
        [Test]
        public void Course_Properties_CourseName_ReturnExpectedDataTypes()
        {
            // Arrange
            Course Course = new Course();
            PropertyInfo propertyInfo = Course.GetType().GetProperty("CourseName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CourseName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CourseName property type is not string.");
        }
[Test]
        public void Course_Properties_Description_ReturnExpectedDataTypes()
        {
            // Arrange
            Course Course = new Course();
            PropertyInfo propertyInfo = Course.GetType().GetProperty("Description");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Description property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Description property type is not string.");
        }

        [Test]
        public async Task GetAllCourses_ReturnsOkResult()
        {
            // Act
            var result = await _CourseController.GetAllCourses();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllCourses_ReturnsAllCourses()
        {
            // Act
            var result = await _CourseController.GetAllCourses();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Course>>(okResult.Value);
            var courses = okResult.Value as IEnumerable<Course>;

            var CourseCount = courses.Count();
            Assert.AreEqual(3, CourseCount); // Assuming you have 3 Courses in the seeded data
        }


        [Test]
        public async Task AddCourse_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newCourse = new Course
            {
CourseName = "Course New", Description = "New Course Description", Duration = "150",Cost=50000
            };

            // Act
            var result = await _CourseController.AddCourse(newCourse);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteCourse_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new CoursesController(context);

                // Act
                var result = await _CourseController.DeleteCourse(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteCourse_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _CourseController.DeleteCourse(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Course id", result.Value);
        }
    }
}
