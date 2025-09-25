using AutoMapper;
using common.data.Enums;
using data.models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using web.apis.Controllers;
using web.apis.Models;

namespace web.apis.tests.TestControllers
{
    [TestFixture]
    public class Tests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<AccountsController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<IEmailRepository> _emailRepositoryMock;
        private AccountsController _controller;


        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = MockUserManager<ApplicationUser>();
            _roleManagerMock = MockRoleManager();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<AccountsController>>();
            _mediatorMock = new Mock<IMediator>();
            _emailRepositoryMock = new Mock<IEmailRepository>();

            _controller = new AccountsController(
                _mapperMock.Object,
                _userManagerMock.Object,
                _configurationMock.Object,
                _roleManagerMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object,
                _emailRepositoryMock.Object
                );
        }

        [Test]
        public async Task RegisterUser_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new UserRegistrationBindingModel
            {
                EmailAddress = "test@example.com",
                Password = "Test@1234",
                FirstName = "John",
                LastName = "Doe",
                UserRoleEnum = UserRoleEnum.User.ToString()
            };

            var appUser = new ApplicationUser { Email = model.EmailAddress };

            _mapperMock.Setup(m => m.Map<ApplicationUser>(model)).Returns(appUser);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
           
            // Act
            var result = await _controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseModel;
            Assert.IsFalse(response.HasError);
            StringAssert.Contains("User account was successfully created", response.Status);
        }

        [Test]
        public async Task RegisterUser_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRegistrationBindingModel();
            _controller.ModelState.AddModelError("EmailAddress", "The Email field is required.");

            // Act
            var result = await _controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var response = badRequestResult.Value as ResponseModel;
            Assert.IsTrue(response.HasError);
            Assert.AreEqual("Some fields are not valid", response.Status);
        }

        [Test]
        public async Task RegisterUser_CreateUserFailed_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRegistrationBindingModel
            {
                EmailAddress = "test@example.com",
                Password = "Test@1234",
                ConfirmPassword = "Test@1234",
                FirstName = "John",
                LastName = "Doe",
                UserRoleEnum = UserRoleEnum.User.ToString(),
            };

            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Code = "DuplicateUserName", Description = "User name already exists." }
            };
            _mapperMock.Setup(m => m.Map<ApplicationUser>(model)).Returns(new ApplicationUser());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            
            // Act
            var result = await _controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var response = badRequestResult.Value as ResponseModel;
            Assert.IsTrue(response.HasError);
            Assert.AreEqual("DuplicateUserName, User name already exists.", response.Status);
        }

        // Helper methods to mock UserManager and RoleManager
        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }
    }
}