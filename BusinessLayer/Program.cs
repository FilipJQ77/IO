namespace BusinessLayer
{
    using Facade;
    using System.Collections.Generic;
    using Controllers; // elo z tym potem
    class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SystemFactory().System;
            var data = new Dictionary<string, string>()
            {
                ["login"]= "Student1",
                ["password"]= "haslo1234",
            };
            var returned = facade.LogIn(data);
            var rank = new UserController().CheckRank(returned.Item2);
            var rank2 = new UserController().CheckRank("test");
        }
    }
}