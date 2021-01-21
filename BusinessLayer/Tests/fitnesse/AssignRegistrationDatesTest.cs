using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Facade;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Tests.fitnesse
{
    public class AssignRegistrationDatesTest : fit.ColumnFixture
    {
        public string DateStart;
        public string DateEnd;
        public string Comment;

        public bool AssignRegistrationDates()
        {
            var facade = new SystemFactory().System;
            var (_, token) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "Pass1",
            });

            facade.AssignRegistrationDate(new Dictionary<string, string>
            {
                ["dateStart"] = DateStart,
                ["dateEnd"] = DateEnd,
            }, token);

            facade.LogOut(token);
            int counter = 0;
            DateTime.TryParse(DateStart, out DateTime dateStart);
            DateTime.TryParse(DateEnd, out DateTime dateEnd);
            var studentDatas = new RepositoryFactory().GetRepository<StudentData>().GetOverview();
            foreach (var student in studentDatas)
            {
                var registrationDate = student.RegistrationDate;
                if (dateStart <= registrationDate && registrationDate <= dateEnd)
                {
                    counter++;
                }
            }

            return counter == studentDatas.ToList().Count;
        }
    }
}