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
            var data = new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            };
            var (_, token) = facade.LogIn(data);
            var rank = new UserController().CheckRank(token);
            var rank2 = new UserController().CheckRank("test");
            Console.WriteLine($"{rank}, {rank2}");
        }
    }
}