using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BusinessLayer.Controllers;
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
    public class SystemTestsMaciek
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

        // pobieranie zalogowanego użytkonika
        [TestMethod]
        public void LoggedUsers_GetUser()
        {
            var loggedUsers = LoggedUsers.GetInstance();
            var user1 = new UserFactory().CreateAdmin(new User
            {
                Id = 21,
                Login = "User1",
                Password = new Encryption.HasherFactory().GetHasher().Hash("Pass"),
                Rank = Rank.Administrator
            });
            var (_, token1) = loggedUsers.LogInUser(user1);
            var user2 = new UserFactory().CreateStudent(new User
            {
                Id = 22,
                Login = "User2",
                Password = new Encryption.HasherFactory().GetHasher().Hash("Pass1"),
                Rank = Rank.Student
            }, new StudentData());
            var (_, token2) = loggedUsers.LogInUser(user2);
            // pobranie User1
            Assert.IsTrue(Object.ReferenceEquals(user1, loggedUsers.GetUser(token1)));
            // pobranie User2
            Assert.IsTrue(Object.ReferenceEquals(user2, loggedUsers.GetUser(token2)));

            loggedUsers.LogOutUser(token1);
            loggedUsers.LogOutUser(token2);
            // pobranie User1
            Assert.IsFalse(Object.ReferenceEquals(user1, loggedUsers.GetUser(token1)));
            // pobranie User2 - niezalogowanyt
            Assert.IsFalse(Object.ReferenceEquals(user2, loggedUsers.GetUser(token2)));
        }

        // sprawdzanie rangi zalogowanego użytkownika
        [TestMethod]
        public void UserController_CheckRank()
        {
            var loggedUsers = LoggedUsers.GetInstance();
            var user1 = new UserFactory().CreateAdmin(new User
            {
                Id = 20,
                Login = "User1",
                Password = new Encryption.HasherFactory().GetHasher().Hash("Pass"),
                Rank = Rank.Administrator
            });
            var (_, token1) = loggedUsers.LogInUser(user1);
            var user2 = new UserFactory().CreateStudent(new User
            {
                Id = 21,
                Login = "User2",
                Password = new Encryption.HasherFactory().GetHasher().Hash("Pass1"),
                Rank = Rank.Student
            }, new StudentData());
            var (_, token2) = loggedUsers.LogInUser(user2);

            var userController = new UserController();
            // pobranie rangi User1
            Assert.IsTrue(userController.CheckRank(token1) == Rank.Administrator);
            // pobranie rangi User2
            Assert.IsTrue(userController.CheckRank(token2) == Rank.Student);

            loggedUsers.LogOutUser(token1);
            loggedUsers.LogOutUser(token2);
            // pobranie rangi wylogowanego użytkownika User1
            Assert.IsTrue(userController.CheckRank(token1) == Rank.None);
            // pobranie rangi wylogowanego użytkownika User2
            Assert.IsTrue(userController.CheckRank(token2) == Rank.None);
        }

        // Sprawdzanie uprawnień do zapisów
        [TestMethod]
        public void CourseGroupController_CheckPermissions()
        {
            // tworzenie studentów
            var password = new Encryption.HasherFactory().GetHasher().Hash("Pass");
            var admin = new User {Id = 21, Login = "Admin", Password = password, Rank = Rank.Administrator};
            var student1 = new User { Id = 22, Login = "Student1", Password = password, Rank = Rank.Student };
            var student2 = new User { Id = 23, Login = "Student2", Password = password,  Rank = Rank.Student };
            var student3 = new User { Id = 24, Login = "Student3", Password = password, Rank = Rank.Student  };
            var student4 = new User { Id = 25, Login = "Student4", Password = password, Rank = Rank.Student };
            var student5 = new User { Id = 26, Login = "Student5", Password = password, Rank = Rank.Student };
            var users = new List<User> {
                admin, student1, student2, student3, student4, student5
            };
            // tworzenie danych dla semestrów
            var field1 = new Field {Id = 1, Name = "Inf"};
            var field2 = new Field { Id = 2, Name = "Eka" };
            var flieds = new List<Field> {field1, field2};
            // tworzenie danych studentów
            var studentData1 = new StudentData
            {
                Field = field1, Id = 1, FieldId = field1.Id, RegistrationDate = DateTime.Now.AddDays(-1), Semester = 1,
                User = student1, UserId = student1.Id
            }; // poprawny
            var studentData2 = new StudentData
            {
                Field = field2, Id = 1, FieldId = field2.Id, RegistrationDate = DateTime.Now.AddDays(-1), Semester = 1,
                User = student2, UserId = student2.Id
            }; // zły kierunek
            var studentData3 = new StudentData
            {
                Field = field1, Id = 1, FieldId = field1.Id, RegistrationDate = DateTime.Now.AddDays(1), Semester = 1,
                User = student3, UserId = student3.Id
            }; // zła data
            var studentData4 = new StudentData
            {
                Field = field1, Id = 1, FieldId = field1.Id, RegistrationDate = DateTime.Now.AddDays(-1), Semester = 2,
                User = student4, UserId = student4.Id
            }; // zły semestr
            var studentData5 = new StudentData
            {
                Field = field1, Id = 1, FieldId = field1.Id, RegistrationDate = null, Semester = 1, User = student5,
                UserId = student5.Id
            }; // nieprzydzielona data
            var studentDatas = new List<StudentData>
                {studentData1, studentData2, studentData3, studentData4, studentData5};

            // tworzenie danych grup kursów
            var courseGroups = new List<CourseGroup> {
                new CourseGroup{ Field = field1, Id = 1, FieldId = field1.Id, Semester = 1}
            };


            var mockContext = new Mock<DatabaseContext>();

            var mockSetUsers = CreateNewMockSetWithData(users);
            mockContext.Setup(m => m.Set<User>()).Returns(mockSetUsers.Object);
            var mockSetFields = CreateNewMockSetWithData(flieds);
            mockContext.Setup(m => m.Set<Field>()).Returns(mockSetFields.Object);
            var mockSetSD = CreateNewMockSetWithData(studentDatas);
            mockContext.Setup(m => m.Set<StudentData>()).Returns(mockSetSD.Object);
            var mockSetCG = CreateNewMockSetWithData(courseGroups);
            mockContext.Setup(m => m.Set<CourseGroup>()).Returns(mockSetCG.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            var cgController = new CourseGroupController();

            // admin
            var (result, _) = cgController.CheckPermissions(new UserFactory().CreateAdmin(admin), 1);
            Assert.IsTrue(result);
            // poprawny student
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student1, studentData1), 1);
            Assert.IsTrue(result);
            // zły kierunek
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student2, studentData2), 1);
            Assert.IsFalse(result);
            // zła data
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student3, studentData3), 1);
            Assert.IsFalse(result);
            // zły semestr
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student4, studentData4), 1);
            Assert.IsFalse(result);
            // nieprzydzielona data
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student5, studentData5), 1);
            Assert.IsFalse(result);
            // przydzielenie aktualnej daty
            studentData5.RegistrationDate = DateTime.Now;
            (result, _) = cgController.CheckPermissions(new UserFactory().CreateStudent(student5, studentData5), 1);
            Assert.IsTrue(result);

            RepositoryFactory.SetDbContext(oldDb);
        }

        // Tworzenie kierunków
        [TestMethod]
        public void Facade_EditCoursesGroup()
        {
            var facade = new SystemFactory().System;
            var student = new User
            {
                Id = 22,
                Login = "Student",
                Password = new Encryption.HasherFactory().GetHasher().Hash("Student"),
                Rank = Rank.Student
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 21,
                    Login = "Admin",
                    Password = new Encryption.HasherFactory().GetHasher().Hash("Admin"),
                    Rank = Rank.Administrator
                }, student
            };
            var studentData = new StudentData {Id = 1, UserId = student.Id, User = student};
            var studentDatas = new List<StudentData> {studentData};
            var field = new Field {Id = 1, Name = "Inf"};
            var fields = new List<Field> {field};
            var courseGroup = new CourseGroup {Id = 1, Field = field, FieldId = field.Id};
            var courseGroups = new List<CourseGroup> { courseGroup };

            var mockContext = new Mock<DatabaseContext>();

            var mockSetUsers = CreateNewMockSetWithData(users);
            mockContext.Setup(m => m.Set<User>()).Returns(mockSetUsers.Object);
            var mockSetDS = CreateNewMockSetWithData(studentDatas);
            mockContext.Setup(m => m.Set<StudentData>()).Returns(mockSetDS.Object);
            var mockSetFields = CreateNewMockSetWithData(fields);
            mockContext.Setup(m => m.Set<Field>()).Returns(mockSetFields.Object);
            var mockSetCG = CreateNewMockSetWithData(courseGroups);
            mockContext.Setup(m => m.Set<CourseGroup>()).Returns(mockSetCG.Object);

            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);

            // logowanie
            var (_, tokenAdmin) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "Admin",
            });
            var (_, tokenStudent) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Student",
                ["password"] = "Student",
            });

            var rightData = new Dictionary<string, string>
            {
                ["id"] = "1",
                ["ects"] = "29",
                ["semester"] = "6",
                ["fieldId"] = "1",
                ["code"] = "INEK000420",
                ["name"] = "Analiza matematyczna 4",
            };

            // nieprawidłowy użytkownik
            var (result, _) = facade.EditCoursesGroup(rightData, tokenStudent);
            Assert.IsFalse(result);
            // niezalogowany użytkownik
            (result, _) = facade.EditCoursesGroup(rightData, "");
            Assert.IsFalse(result);
            // Nieprawidłowe Id grupy kursów
            var badData = new Dictionary<string,string>(rightData);
            badData["id"] = "2";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // Nieprawidłowe ects
            badData = new Dictionary<string, string>(rightData);
            badData["ects"] = "asd";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            badData = new Dictionary<string, string>(rightData);
            badData["ects"] = "-1";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // Nieprawidłowy semestr
            badData = new Dictionary<string, string>(rightData);
            badData["semester"] = "asd";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            badData = new Dictionary<string, string>(rightData);
            badData["semester"] = "-1";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // nieprawidłowy kierunek
            badData = new Dictionary<string, string>(rightData);
            badData["fieldId"] = "asd";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            badData = new Dictionary<string, string>(rightData);
            badData["fieldId"] = "2";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // pusty kod grupy kursów
            badData = new Dictionary<string, string>(rightData);
            badData["code"] = "";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // pusta nazwa grupy kursów
            badData = new Dictionary<string, string>(rightData);
            badData["name"] = "";
            (result, _) = facade.EditCoursesGroup(badData, tokenAdmin);
            Assert.IsFalse(result);
            // prawidłowe
            (result, _) = facade.EditCoursesGroup(rightData, tokenAdmin);
            Assert.IsTrue(result && courseGroup.Id == Int32.Parse(rightData["id"])
                                 && courseGroup.NumberOfEcts == Int32.Parse(rightData["ects"])
                                 && courseGroup.Semester == Int32.Parse(rightData["semester"])
                                 && Object.ReferenceEquals(courseGroup.Field, field)
                                 && courseGroup.Code == rightData["code"]
                                 && courseGroup.Name == rightData["name"]);

            facade.LogOut(tokenStudent);
            facade.LogOut(tokenAdmin);
            RepositoryFactory.SetDbContext(oldDb);
        }
    }
}