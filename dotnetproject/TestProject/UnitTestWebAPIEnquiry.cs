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
    public class EnquiryControllerTests
    {
        private EnquiryController _EnquiryController;
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
            _context.Enquires.AddRange(new List<Enquiry>
            {
new Enquiry { EnquiryID = 1, EnquiryDate = DateTime.Parse("2023-02-12"),KidName="Kid1",ParentsName="Parent1", ContactNumber = "9876543210", EmailID = "mymail1@gmail.com",CourseName="Course1" },
new Enquiry { EnquiryID = 2, EnquiryDate = DateTime.Parse("2023-04-30"),KidName="Kid2",ParentsName="Parent2", ContactNumber = "9876543217", EmailID = "mymail2@gmail.com",CourseName="Course1" },
new Enquiry { EnquiryID = 3, EnquiryDate = DateTime.Parse("2023-06-30"),KidName="Kid3",ParentsName="Parent3", ContactNumber = "9876543216", EmailID = "mymail3@gmail.com",CourseName="Course4" }
            });
            _context.SaveChanges();

            _EnquiryController = new EnquiryController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void EnquiryClassExists()
        {
            // Arrange
            Type EnquiryType = typeof(Enquiry);

            // Act & Assert
            Assert.IsNotNull(EnquiryType, "Enquiry class not found.");
        }
        [Test]
        public void Enquiry_Properties_CourseName_ReturnExpectedDataTypes()
        {
            // Arrange
            Enquiry enquiry = new Enquiry();
            PropertyInfo propertyInfo = enquiry.GetType().GetProperty("CourseName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CourseName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CourseName property type is not string.");
        }
[Test]
        public void Enquiry_Properties_EnquiryDate_ReturnExpectedDataTypes()
        {
            // Arrange
            Enquiry enquiry = new Enquiry();
            PropertyInfo propertyInfo = enquiry.GetType().GetProperty("EnquiryDate");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "EnquiryDate property not found.");
            Assert.AreEqual(typeof(DateTime), propertyInfo.PropertyType, "EnquiryDate property type is not string.");
        }

        [Test]
        public async Task GetAllEnquirys_ReturnsOkResult()
        {
            // Act
            var result = await _EnquiryController.GetAllEnquiries();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllEnquirys_ReturnsAllEnquirys()
        {
            // Act
            var result = await _EnquiryController.GetAllEnquiries();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Enquiry>>(okResult.Value);
            var enquirys = okResult.Value as IEnumerable<Enquiry>;

            var EnquiryCount = enquirys.Count();
            Assert.AreEqual(3, EnquiryCount); // Assuming you have 3 Enquirys in the seeded data
        }


        [Test]
        public async Task AddEnquiry_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newEnquiry = new Enquiry
            {
EnquiryDate = DateTime.Parse("2023-08-30"),KidName="Kid New",ParentsName="Parent new", ContactNumber = "9876543216", EmailID = "mymail3@gmail.com",CourseName="Course New"          };

            // Act
            var result = await _EnquiryController.AddEnquiry(newEnquiry);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteEnquiry_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new EnquirysController(context);

                // Act
                var result = await _EnquiryController.DeleteEnquiry(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteEnquiry_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _EnquiryController.DeleteEnquiry(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Enquiry id", result.Value);
        }
    }
}
