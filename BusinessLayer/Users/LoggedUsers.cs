using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Users
{
    class LoggedUsers
    {
        readonly Dictionary<string, IUser> Users;

        private LoggedUsers()
        {
            Users = new Dictionary<string, IUser>();
        }

        private static LoggedUsers _instance;

        public static LoggedUsers GetInstance()
        {
            return _instance ?? (_instance = new LoggedUsers());
        }

        public (bool, string) LogInUser(IUser user)
        {
            if (Users.Any(loggedUser => user.User.Id == loggedUser.Value.User.Id))
                return (false, "");

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Users.Add(token, user);
            return (true, token);
        }

        public bool LogOutUser(string token)
        {
            if (!Users.ContainsKey(token)) return false;
            Users.Remove(token);
            return true;
        }

        public IUser GetUser(string token)
        {
            return Users.ContainsKey(token) ? Users[token] : null;
        }
    }
}