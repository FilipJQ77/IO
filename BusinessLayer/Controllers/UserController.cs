using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Controllers
{
    using Users;
    using Encryption;
    class UserController
    {
        LoggedUsers LoggedUsers = LoggedUsers.GetInstance();

        public (bool, string) LogIn(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("login"))
                return (false, "Należy podać login");
            if (!data.ContainsKey("password"))
                return (false, "Należy podać hasło");

            string login = data["login"];
            string password = data["password"];

            var userRep = new RepositoryFactory().GetRepository<User>();
            var user = userRep.GetDetail(p => p.Login == login);
            if (user == null)
                return (false, "Podany użytkownik nie istnieje");

            var hasher = new HasherFactory().GetHasher();
            if (!hasher.Check(user.Password, password))
                return (false, "Nieprawidłowe hasło");

            IUser loginUser;
            if(user.Rank == Rank.Student)
            {
                var sdRep = new RepositoryFactory().GetRepository<StudentData>();
                var sd = sdRep.GetDetail(p => p.UserId == user.Id);
                loginUser = new UserFactory().CreateStudent(user, sd);
            }
            else
            {
                loginUser = new UserFactory().CreateAdmin(user);
            }

            var info = LoggedUsers.LogInUser(loginUser);

            if (info.Item1)
                return (true, info.Item2);

            return (false, "Użytkownik jest już zalogowany");

        }
        public void LogOut(string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AddAccount(Dictionary<string, string> data, string token)
        {
            return (false, "");
        }

        public (bool, string) AssignRegistrationDate(Dictionary<string, string> data, string token)
        {

            return (false, "");
        }

        public Rank CheckRank(string token)
        {
            var user = LoggedUsers.GetUser(token);
            if (user == null)
            {
                return Rank.None;
            }
            return user.User.Rank;
        }

        public (bool, string) ValidateUser(Dictionary<string, string> data) {

            return (false, "");
        }
    }
}
