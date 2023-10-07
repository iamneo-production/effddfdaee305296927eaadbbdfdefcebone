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
    public class EnquiryControllerTests
    {
        private Mock<IEnquiryService> mockEnquiryService;
        private EnquiryController controller;
        [SetUp]
        public void Setup()
        {
            mockEnquiryService = new Mock<IEnquiryService>();
            controller = new EnquiryController(mockEnquiryService.Object);
        }

        [Test]
        public void AddEnquiry_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.AddEnquiry(It.IsAny<Enquiry>())).Returns(true);
            var controller = new EnquiryController(mockEnquiryService.Object);
            var enquiry = new Enquiry(); // Provide valid Enquiry data

            // Act
            var result = controller.AddEnquiry(enquiry) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddEnquiry_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            var controller = new EnquiryController(mockEnquiryService.Object);
            Enquiry invalidEnquiry = null; // Invalid Enquiry data

            // Act
            var result = controller.AddEnquiry(invalidEnquiry) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Enquiry data", result.Value);
        }
        [Test]
        public void AddEnquiry_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.AddEnquiry(It.IsAny<Enquiry>())).Returns(false);
            var controller = new EnquiryController(mockEnquiryService.Object);
            var enquiry = new Enquiry(); // Provide valid Enquiry data

            // Act
            var result = controller.AddEnquiry(enquiry) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Enquiry. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddEnquiry_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.AddEnquiry(It.IsAny<Enquiry>())).Returns(true);
            var controller = new EnquiryController(mockEnquiryService.Object);
            var enquiry = new Enquiry();

            // Act
            var result = controller.AddEnquiry(enquiry) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddEnquiry_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            var controller = new EnquiryController(mockEnquiryService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var enquiry = new Enquiry();

            // Act
            var result = controller.AddEnquiry(enquiry) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(enquiry, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.GetAllEnquirys()).Returns(new List<Enquiry>());
            var controller = new EnquiryController(mockEnquiryService.Object);

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
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.DeleteEnquiry(1)).Returns(true);
            var controller = new EnquiryController(mockEnquiryService.Object);

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
            var mockEnquiryService = new Mock<IEnquiryService>();
            mockEnquiryService.Setup(service => service.DeleteEnquiry(1)).Returns(false);
            var controller = new EnquiryController(mockEnquiryService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
