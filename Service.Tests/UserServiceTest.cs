using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ToDoList.Service.Concretes;
using ToDoList.Service.Rules;
using ToDoList.Models.Entities;
using ToDoList.Models.Dtos.Users.Requests;
using Core.Exceptions;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Service.Tests
{
    [TestFixture]
    public class UserServiceTest
    {
        private UserService userService;
        private Mock<UserManager<User>> mockUserManager;
        private Mock<UserBusinessRules> mockBusinessRules;

        [SetUp]
        public void SetUp()
        {
            // Setup UserManager mock
            var store = new Mock<IUserStore<User>>();
            mockUserManager = new Mock<UserManager<User>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<User>>(),
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>());

            mockBusinessRules = new Mock<UserBusinessRules>();
            userService = new UserService(mockUserManager.Object, mockBusinessRules.Object);
        }

        [Test]
        public async Task RegisterAsync_WhenValid_ReturnsUser()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            ("Beyza", "Aydoğan", "beyza@gmail.com","Password123!","Beyza123456");

            var user = new User
            {
                Id = "2b",
                FirstName = "Beyza",
                LastName = "Aydoğan",
                Email = "beyza@gmail.com",
                UserName = "Beyza123456"
            };

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<User, string>((u, p) => {
                    u.Id = "new-user-id";
                });

            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()));

            // Act
            var result = await userService.RegisterAsync(registerDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(registerDto.Email, result.Email);
            Assert.AreEqual(registerDto.FirstName, result.FirstName);
            Assert.AreEqual(registerDto.LastName, result.LastName);
            Assert.AreEqual(registerDto.Username, result.UserName);
            Assert.IsFalse(string.IsNullOrEmpty(result.Id));
            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password), Times.Once);
            mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Exactly(2));
        }

        [Test]
        public void RegisterAsync_WhenCreateFails_ThrowsException()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Username = "janesmith",
                Password = "Password123!"
            };

            var identityError = new IdentityError { Description = "User creation failed." };
            var identityResult = IdentityResult.Failed(identityError);

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
                .ReturnsAsync(identityResult);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(identityResult))
                .Throws(new BusinessException(identityError.Description));

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await userService.RegisterAsync(registerDto));
            Assert.AreEqual("User creation failed.", ex.Message);
            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(identityResult), Times.Once);
            mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Never);
        }

        [Test]
        public async Task LoginAsync_WhenValidCredentials_ReturnsUser()
        {
            // Arrange
            var loginDto = new LoginRequestDto
            {
                Email = "john.doe@example.com",
                Password = "Password123!"
            };

            var user = new User
            {
                Id = "user-id",
                Email = "john.doe@example.com",
                UserName = "johndoe"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(true);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            // Act
            var result = await userService.LoginAsync(loginDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.UserName, result.UserName);
            mockUserManager.Verify(x => x.FindByEmailAsync(loginDto.Email), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(user, loginDto.Password), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
        }

        [Test]
        public void LoginAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "Password123!"
            };

            User user = null;

            mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user))
                .Throws(new NotFoundException("User not found."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await userService.LoginAsync(loginDto));
            Assert.AreEqual("User not found.", ex.Message);
            mockUserManager.Verify(x => x.FindByEmailAsync(loginDto.Email), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void LoginAsync_WhenPasswordInvalid_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginRequestDto
            {
                Email = "john.doe@example.com",
                Password = "WrongPassword!"
            };

            var user = new User
            {
                Id = "user-id",
                Email = "john.doe@example.com",
                UserName = "johndoe"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(false);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await userService.LoginAsync(loginDto));
            Assert.AreEqual("Parolanız yanlış.", ex.Message);
            mockUserManager.Verify(x => x.FindByEmailAsync(loginDto.Email), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(user, loginDto.Password), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
        }

        [Test]
        public async Task GetByEmailAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var email = "john.doe@example.com";
            var user = new User
            {
                Id = "user-id",
                Email = email,
                UserName = "johndoe"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            // Act
            var result = await userService.GetByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
        }

        [Test]
        public void GetByEmailAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var email = "nonexistent@example.com";
            User user = null;

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user))
                .Throws(new NotFoundException("User not found."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetByEmailAsync(email));
            Assert.AreEqual("User not found.", ex.Message);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var userId = "user-id";
            var updateDto = new UserUpdateRequestDto
            {
                Username = "newusername",
                FirstName = "John",
                LastName = "Doe"
            };

            var user = new User
            {
                Id = userId,
                UserName = "oldusername",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            mockUserManager.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()));

            // Act
            var result = await userService.UpdateAsync(userId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.Username, result.UserName);
            Assert.AreEqual(updateDto.FirstName, result.FirstName);
            Assert.AreEqual(updateDto.LastName, result.LastName);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Once);
        }

        [Test]
        public void UpdateAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var userId = "nonexistent-id";
            var updateDto = new UserUpdateRequestDto
            {
                Username = "newusername",
                FirstName = "Jane",
                LastName = "Doe"
            };

            User user = null;

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user))
                .Throws(new NotFoundException("User not found."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await userService.UpdateAsync(userId, updateDto));
            Assert.AreEqual("User not found.", ex.Message);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_WhenUserExists_ReturnsSuccessMessage()
        {
            // Arrange
            var userId = "user-id";
            var user = new User
            {
                Id = userId,
                UserName = "johndoe",
                Email = "john.doe@example.com"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            mockUserManager.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()));

            // Act
            var result = await userService.DeleteAsync(userId);

            // Assert
            Assert.AreEqual("Kullanıcı Silindi.", result);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(user), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var userId = "nonexistent-id";
            User user = null;

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user))
                .Throws(new NotFoundException("User not found."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await userService.DeleteAsync(userId));
            Assert.AreEqual("User not found.", ex.Message);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Never);
        }

        [Test]
        public async Task ChangePasswordAsync_WhenValid_ReturnsUser()
        {
            // Arrange
            var userId = "user-id";
            var changePasswordDto = new ChangePasswordRequestDto
            {
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!"
            };

            var user = new User
            {
                Id = userId,
                UserName = "johndoe",
                Email = "john.doe@example.com"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            mockUserManager.Setup(x => x.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()));

            // Act
            var result = await userService.ChangePasswordAsync(userId, changePasswordDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Once);
        }

        [Test]
        public void ChangePasswordAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var userId = "nonexistent-id";
            var changePasswordDto = new ChangePasswordRequestDto
            {
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!"
            };

            User user = null;

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user))
                .Throws(new NotFoundException("User not found."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await userService.ChangePasswordAsync(userId, changePasswordDto));
            Assert.AreEqual("User not found.", ex.Message);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(It.IsAny<IdentityResult>()), Times.Never);
        }

        [Test]
        public void ChangePasswordAsync_WhenChangeFails_ThrowsException()
        {
            // Arrange
            var userId = "user-id";
            var changePasswordDto = new ChangePasswordRequestDto
            {
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!"
            };

            var user = new User
            {
                Id = userId,
                UserName = "johndoe",
                Email = "john.doe@example.com"
            };

            var identityError = new IdentityError { Description = "Password change failed." };
            var identityResult = IdentityResult.Failed(identityError);

            mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            mockBusinessRules.Setup(x => x.UserIsNullCheck(user));

            mockUserManager.Setup(x => x.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword))
                .ReturnsAsync(identityResult);

            mockBusinessRules.Setup(x => x.CheckForIdentityResult(identityResult))
                .Throws(new BusinessException(identityError.Description));

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await userService.ChangePasswordAsync(userId, changePasswordDto));
            Assert.AreEqual("Password change failed.", ex.Message);
            mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            mockBusinessRules.Verify(x => x.UserIsNullCheck(user), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword), Times.Once);
            mockBusinessRules.Verify(x => x.CheckForIdentityResult(identityResult), Times.Once);
        }
    }
}
