using System;

namespace BusinessLayer
{
    using Facade;
    using System.Collections.Generic;

    class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SystemFactory().System;

            // var (loggedInAdmin, tokenAdmin) = facade.LogIn(new Dictionary<string, string>
            // {
            //     ["login"] = "Admin",
            //     ["password"] = "haslo1234",
            // });

            // facade.AssignRegistrationDate(new Dictionary<string, string>
            // {
            //     ["dateStart"] = "2020-01-08 10:00",
            //     ["dateEnd"] = "2020-01-08 11:00",
            // }, tokenAdmin);

            var (loggedInStudent, tokenStudent) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            });

            var (signedIn, message) = facade.SignToLesson(new Dictionary<string, string>
            {
                ["lessonId"] = "4",

            }, tokenStudent);

            var (edited, err4) = facade.EditCoursesGroup(new Dictionary<string, string>
            {
                ["id"]="1",
                ["ects"]="29",
                ["semester"]="6",
                ["fieldId"]="2",
                ["code"]="INEK000420",
                ["name"]="Analiza matematyczna 4",
            }, tokenAdmin);

            var (field, message) = facade.ShowField(new Dictionary<string, string>
            {
                ["id"] = "1",
            }, tokenAdmin);
        }
    }
}
