using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetmvcapp.Controllers;
using dotnetmvcapp.Models;
using dotnetmvcapp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace dotnetmvcapp.Tests
{
    [TestFixture]
    public class CourseControllerTests
    {
        private Mock<ICourseService> mockCourseService;
        private CourseController controller;
        [SetUp]
        public void Setup()
        {
            mockCourseService = new Mock<ICourseService>();
            controller = new CourseController(mockCourseService.Object);
        }

        [Test]
        public void AddCourse_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.AddCourse(It.IsAny<Course>())).Returns(true);
            var controller = new CourseController(mockCourseService.Object);
            var course = new Course(); // Provide valid Course data

            // Act
            var result = controller.AddCourse(course) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddCourse_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            var controller = new CourseController(mockCourseService.Object);
            Course invalidCourse = null; // Invalid Course data

            // Act
            var result = controller.AddCourse(invalidCourse) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Course data", result.Value);
        }
        [Test]
        public void AddCourse_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.AddCourse(It.IsAny<Course>())).Returns(false);
            var controller = new CourseController(mockCourseService.Object);
            var course = new Course(); // Provide valid Course data

            // Act
            var result = controller.AddCourse(course) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Course. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddCourse_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.AddCourse(It.IsAny<Course>())).Returns(true);
            var controller = new CourseController(mockCourseService.Object);
            var course = new Course();

            // Act
            var result = controller.AddCourse(course) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddCourse_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            var controller = new CourseController(mockCourseService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var course = new Course();

            // Act
            var result = controller.AddCourse(course) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(course, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.GetAllCourses()).Returns(new List<Course>());
            var controller = new CourseController(mockCourseService.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void Delete_ValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.DeleteCourse(1)).Returns(true);
            var controller = new CourseController(mockCourseService.Object);

            // Act
            var result = controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Delete_InvalidId_ReturnsViewResult()
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();
            mockCourseService.Setup(service => service.DeleteCourse(1)).Returns(false);
            var controller = new CourseController(mockCourseService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
