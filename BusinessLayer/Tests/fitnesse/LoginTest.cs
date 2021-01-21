using BusinessLayer.Facade;
using System.Collections.Generic;

namespace BusinessLayer.Tests.fitnesse
{
    public class LoginTest : fit.ColumnFixture
    {
        public string Login;
        public string Password;
        public string Comment;

        private string lastMessage = "";

        public bool LoginUser()
        {
            var system = new SystemFactory().System;

            var (passed, ms) = system.LogIn(new Dictionary<string, string>
            {
                ["login"] = Login,
                ["password"] = Password,
            });

            lastMessage = ms;
            return passed;
        }

        public string getLastMessage()
        {
            return lastMessage;
        }
    }
}
