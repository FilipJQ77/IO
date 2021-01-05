using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Users
{
    class LoggedUsers
    {
        Dictionary<string, IUser> Users;

        private LoggedUsers() {
            Users = new Dictionary<string, IUser>();
        }
        
        private static LoggedUsers _instance;
        public static LoggedUsers GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoggedUsers();
            }
            return _instance;
        }

        public (bool, string) LogInUser(IUser user)
        {
            foreach(var loggedUser in Users)
            {
                if(user.User.Id == loggedUser.Value.User.Id)
                {
                    return (false, "");
                }
            }

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Users.Add(token, user);
            return (true, token);
        }

        public bool LogOutUser(string token)
        {
            if (Users.ContainsKey(token))
            {
                Users.Remove(token);
                return true;
            }
            return false;
        }

        public IUser GetUser(string token)
        {
            if (Users.ContainsKey(token))
            {
                return Users[token];
            }
            return null;
        }
    }
}
