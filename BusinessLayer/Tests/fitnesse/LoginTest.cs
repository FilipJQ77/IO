using BusinessLayer.Facade;
using System.Collections.Generic;

namespace BusinessLayer.Tests.fitnesse
{
    public class LoginTest : fit.ColumnFixture
    {
        public string Login;
        public string Password;
        public string Comment;

        public bool LoginUser()
        {
            var system = new SystemFactory().System;

            var (passed, token) = system.LogIn(new Dictionary<string, string>
            {
                ["login"] = Login,
                ["password"] = Password,
            });

            return passed;
        }
    }
}
