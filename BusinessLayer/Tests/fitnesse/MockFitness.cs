using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Data.Entity;
using DataAccess.BusinessObjects.Entities;
using DataAccess.BusinessObjects;
using DataAccess.Repositories;
using Newtonsoft.Json;

namespace BusinessLayer.Tests.fitnesse
{
    public class MockFitness : fit.Fixture
    {
        Mock<DatabaseContext> mockContext = new Mock<DatabaseContext>();

        Mock<DbSet<T>> CreateNewMockSetWithData<T>(List<T> dataSource = null) where T : class
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

        public void SetMock()
        {
            var oldDb = RepositoryFactory.SetDbContext(mockContext.Object);
        }

        public void MockUser(string data)
        {
            var users = JsonConvert.DeserializeObject < List < User >> (data);

            foreach(var user in users){
                user.Password = new Encryption.HasherFactory().GetHasher().Hash(user.Password);
            }

            var mockSetUsers = CreateNewMockSetWithData(users);
            mockContext.Setup(m => m.Set<User>()).Returns(mockSetUsers.Object);
        }

        public void MockStudentData(string data)
        {
            var users = JsonConvert.DeserializeObject<List<StudentData>>(data);

            var mockSetUsers = CreateNewMockSetWithData(users);
            mockContext.Setup(m => m.Set<StudentData>()).Returns(mockSetUsers.Object);
        }
    }
}
