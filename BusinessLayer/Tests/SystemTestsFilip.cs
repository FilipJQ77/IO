using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLayer.Facade;
using BusinessLayer.Users;
using DataAccess.BusinessObjects;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLayer.Tests
{
    // testowanie z mockowaniem: https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
    [TestClass]
    public class SystemTestsFilip
    {
        public Mock<DbSet<T>> CreateNewMockSetWithData<T>(List<T> dataSource = null) where T : class
        {
            var data = dataSource ?? new List<T>();
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(e => e.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(e => e.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(e => e.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(e => e.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            //Mocking the insertion of entities
            mockSet.Setup(_ => _.Add(It.IsAny<T>())).Returns((T arg) =>
            {
                data.Add(arg);
                return arg;
            });
            return mockSet;
        }

        [TestMethod]
        public void AddAccount_AddAdmin()
        {
            var facade = new SystemFactory().System;

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Login = "Admin",
                    Password = new Encryption.HasherFactory().GetHasher().Hash("Admin"),
                    Rank = Rank.Administrator
                }
            };

            var mockSet = CreateNewMockSetWithData(users);
            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockSet.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "Admin",
            };

            var (loggedInAdmin, tokenAdmin) = facade.LogIn(loginDict);

            var newUserDict = new Dictionary<string, string>
            {
                ["rank"] = "Administrator",
                ["login"] = "admin123456",
            };

            facade.AddAccount(newUserDict, tokenAdmin);
            facade.LogOut(tokenAdmin);
            mockSet.Verify(
                m => m.Add(
                    It.Is<User>(u => u.Login == "admin123456" && u.Rank == Rank.Administrator)
                )
            );

            RepositoryFactory.SetDbContext(oldDb);
        }

        [TestMethod]
        public void AssignRegistrationDates_AreDatesCorrect()
        {
            var facade = new SystemFactory().System;
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Login = "Admin",
                    Password = new Encryption.HasherFactory().GetHasher().Hash("Admin"),
                    Rank = Rank.Administrator
                }
            };

            var mockUserSet = CreateNewMockSetWithData(users);

            var studentDatas = new List<StudentData>(20);
            for (int i = 0; i < 20; i++)
            {
                studentDatas.Add(new StudentData());
            }

            var mockStudentDataSet = CreateNewMockSetWithData(studentDatas);

            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockUserSet.Object);
            mockContext.Setup(m => m.Set<StudentData>()).Returns(mockStudentDataSet.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "Admin",
            };

            var (loggedInAdmin, tokenAdmin) = facade.LogIn(loginDict);

            var registrationDict = new Dictionary<string, string>
            {
                ["dateStart"] = "2020-01-08 10:00",
                ["dateEnd"] = "2020-01-08 11:00",
            };

            DateTime.TryParse(registrationDict["dateStart"], out DateTime dateStart);
            DateTime.TryParse(registrationDict["dateEnd"], out DateTime dateEnd);

            facade.AssignRegistrationDate(registrationDict, tokenAdmin);
            studentDatas = mockStudentDataSet.Object.ToList();
            foreach (var registrationDate in studentDatas.Select(studentData => studentData.RegistrationDate))
            {
                Assert.IsTrue(dateStart <= registrationDate && registrationDate <= dateEnd);
            }

            RepositoryFactory.SetDbContext(oldDb);
        }

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
    }
}