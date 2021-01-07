using System;

namespace BusinessLayer
{
    using Controllers; // todo elo z tym potem
    using Facade;
    using System.Collections.Generic;

    class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SystemFactory().System;

            var (_, token) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "haslo1234",
            });

            var(_, message) = facade.AssignRegistrationDate(new Dictionary<string, string>
            {
                ["dateStart"] = "02/02/2020 10:00:00",
                ["dateEnd"] = "02/05/2020 10:00:00",
            }, token);

            Console.WriteLine(message);
        }
    }
}