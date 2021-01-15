using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLayer.Controllers;
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

            var userController = new UserController();

            var (loggedInAdmin, tokenAdmin) = userController.LogIn(loginDict);

            var newUserDict = new Dictionary<string, string>
            {
                ["rank"] = "Administrator",
                ["login"] = "admin123456",
            };

            userController.AddAccount(newUserDict, tokenAdmin);
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
            var users = new List<User>
            {
                new User
                {
                    Id = 10,
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

            var userController = new UserController();

            var (loggedInAdmin, tokenAdmin) = userController.LogIn(loginDict);

            var registrationDict = new Dictionary<string, string>
            {
                ["dateStart"] = "2020-01-08 10:00",
                ["dateEnd"] = "2020-01-08 11:00",
            };

            DateTime.TryParse(registrationDict["dateStart"], out DateTime dateStart);
            DateTime.TryParse(registrationDict["dateEnd"], out DateTime dateEnd);

            userController.AssignRegistrationDate(registrationDict, tokenAdmin);
            studentDatas = mockStudentDataSet.Object.ToList();
            foreach (var registrationDate in studentDatas.Select(studentData => studentData.RegistrationDate))
            {
                Console.WriteLine(registrationDate);
                Assert.IsTrue(dateStart <= registrationDate && registrationDate <= dateEnd);
            }

            RepositoryFactory.SetDbContext(oldDb);
        }

        [TestMethod]
        public void LoginAdmin_ReturnsTrue()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 2,
                    Login = "Admin1",
                    Password = new Encryption.HasherFactory().GetHasher().Hash("Admin1"),
                    Rank = Rank.Administrator
                }
            };

            var mockSet = CreateNewMockSetWithData(users);
            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockSet.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Admin1",
                ["password"] = "Admin1",
            };

            var (loggedInAdmin, tokenAdmin) = new UserController().LogIn(loginDict);

            Assert.IsTrue(loggedInAdmin);

            RepositoryFactory.SetDbContext(oldDb);
        }

        [TestMethod]
        public void LoginStudent_ReturnsTrue()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 3,
                    Login = "Student1",
                    Password = new Encryption.HasherFactory().GetHasher().Hash("Student1"),
                    Rank = Rank.Student
                }
            };

            var studentDatas = new List<StudentData>
            {
                new StudentData
                {
                    UserId = 1
                }
            };

            var mockUserSet = CreateNewMockSetWithData(users);
            var mockStudentDataSet = CreateNewMockSetWithData(studentDatas);
            var mockContext = new Mock<DatabaseContext>();

            mockContext.Setup(m => m.Set<User>()).Returns(mockUserSet.Object);
            mockContext.Setup(m => m.Set<StudentData>()).Returns(mockStudentDataSet.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            var loginDict = new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "Student1",
            };

            var (loggedInStudent, tokenStudent) = new UserController().LogIn(loginDict);

            Assert.IsTrue(loggedInStudent);

            RepositoryFactory.SetDbContext(oldDb);
        }

        [TestMethod]
        public void LoginNonExistingUser_ReturnsFalse()
        {
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
                ["login"] = "",
                ["password"] = "",
            };

            var (loggedIn, token) = new UserController().LogIn(loginDict);

            Assert.IsFalse(loggedIn);

            RepositoryFactory.SetDbContext(oldDb);
        }

        [TestMethod]
        public void LogoutUser_UserNotInLoggedUsers()
        {
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

            var userController = new UserController();

            var (loggedInAdmin, tokenAdmin) = userController.LogIn(loginDict);

            userController.LogOut(tokenAdmin);

            var user = LoggedUsers.GetInstance().GetUser(tokenAdmin);

            Assert.IsTrue(user == null);

            RepositoryFactory.SetDbContext(oldDb);
        }
    }
}