using System.Collections.Generic;
using BusinessLayer.Facade;
using BusinessLayer.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class SystemTests
    {
        [TestMethod]
        public void AdminLogin_ReturnsTrue()
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
        public void StudentLogin_ReturnsTrue()
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
        public void NonExistingUserLogin_ReturnsFalse()
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
        public void UserLogOut()
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
    }
}