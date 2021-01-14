using System.Collections.Generic;
using System.Data.Entity;
using BusinessLayer.Facade;
using BusinessLayer.Users;
using DataAccess.BusinessObjects;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class SystemTestsFilip
    {
        [TestMethod]
        public void LoginAdmin_ReturnsTrue()
        {
            var facade = new SystemFactory().System;
            var dataDict = new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "haslo1234",
            };

            var (loggedInAdmin, tokenAdmin) = facade.LogIn(dataDict);

            Assert.IsTrue(loggedInAdmin);
        }

        [TestMethod]
        public void LoginStudent_ReturnsTrue()
        {
            var facade = new SystemFactory().System;
            var dataDict = new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            };

            var (loggedInStudent, tokenStudent) = facade.LogIn(dataDict);

            Assert.IsTrue(loggedInStudent);
        }

        [TestMethod]
        public void LoginNonExistingUser_ReturnsFalse()
        {
            var facade = new SystemFactory().System;
            var dataDict = new Dictionary<string, string>
            {
                ["login"] = "",
                ["password"] = "",
            };

            var (loggedIn, token) = facade.LogIn(dataDict);

            Assert.IsFalse(loggedIn);
        }

        [TestMethod]
        public void LogoutUser_UserNotInLoggedUsers()
        {
            var facade = new SystemFactory().System;
            var dataDict = new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            };

            var (loggedInStudent, tokenStudent) = facade.LogIn(dataDict);
            facade.LogOut(tokenStudent);
            var user = LoggedUsers.GetInstance().GetUser(tokenStudent);

            Assert.IsTrue(user == null);
        }

        // testowanie z mockowaniem: https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking

        [TestMethod]
        public void AddAccount_AddAdmin()
        {
            var facade = new SystemFactory().System;

            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockSet.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "haslo1234",
            };

            var (loggedInAdmin, tokenAdmin) = facade.LogIn(loginDict);

            RepositoryFactory.SetDbContext(mockContext.Object); 

            var newUserDict = new Dictionary<string, string>
            {
                ["rank"] = "Administrator",
                ["login"] = "admin123456",
            };

            facade.AddAccount(newUserDict, tokenAdmin);
            mockSet.Verify(
                m => m.Add(
                    It.Is<User>(u => u.Login == "admin123456" && u.Rank == Rank.Administrator)
                )
            );
        }

        [TestMethod]
        public void SignToLesson()
        {
            var facade = new SystemFactory().System;

            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockSet.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            };

            var (loggedInStudent, tokenStudent) = facade.LogIn(loginDict);

            RepositoryFactory.SetDbContext(mockContext.Object);

            var signInDict = new Dictionary<string, string>
            {
                ["rank"] = "Administrator",
                ["login"] = "admin123456",
            };

            facade.SignToLesson(signInDict, tokenStudent);
            //todo
            mockSet.Verify(
                m => m.Add(
                    It.Is<User>(u => u.Login == "admin123456" && u.Rank == Rank.Administrator)
                )
            );
        }
    }
}