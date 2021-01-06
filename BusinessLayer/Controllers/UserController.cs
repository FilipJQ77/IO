﻿using System;
using System.Collections.Generic;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Controllers
{
    using Users;
    using Encryption;

    class UserController
    {
        readonly LoggedUsers LoggedUsers = LoggedUsers.GetInstance();

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
            if (user.Rank == Rank.Student)
            {
                var sdRep = new RepositoryFactory().GetRepository<StudentData>();
                var sd = sdRep.GetDetail(p => p.UserId == user.Id);
                loginUser = new UserFactory().CreateStudent(user, sd);
            }
            else
            {
                loginUser = new UserFactory().CreateAdmin(user);
            }

            var (loggedIn, token) = LoggedUsers.LogInUser(loginUser);

            return loggedIn ? (true, token) : (false, "Użytkownik jest już zalogowany");
        }

        public void LogOut(string token)
        {
            LoggedUsers
                .GetInstance()
                .LogOutUser(token);
        }

        public (bool, string) AddAccount(Dictionary<string, string> data, string token)
        {
            var loggedUser = LoggedUsers
                .GetInstance()
                .GetUser(token);

            if (loggedUser.User.Rank != Rank.Administrator)
            {
                return (false, "Tylko administrator może dodać nowe konto");
            }

            if (!Enum.TryParse<Rank>(data["rank"], out var newUserRank)) // parsowanie stringa na enuma
                return (false, "Niepoprawna ranga nowego użytkownika");

            var newLogin = data["login"];
            var newUser = new User
            {
                Login = newLogin,
                Password = new Hasher().Hash("haslo1234")
            };
            var userRepo = new RepositoryFactory().GetRepository<User>();
            userRepo.Add(newUser);
            if (newUserRank == Rank.Student)
            {
                var firstName = data["firstName"];
                var lastName = data["lastName"];
                int index, fieldId, semester;
                try
                {
                    index = int.Parse(data["index"]);
                    fieldId = int.Parse(data["fieldId"]);
                    semester = int.Parse(data["semester"]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return (false, e.Message);
                }
                var studentData = new StudentData
                {
                    FieldId = fieldId, //todo test potem
                    FirstName = firstName,
                    LastName = lastName,
                    Index = index,
                    Semester = semester,
                    User = newUser
                };
                var dataRepo = new RepositoryFactory().GetRepository<StudentData>();
                dataRepo.Add(studentData);
            }

            return (true, $"Stworzono nowego użytkownika o randze {newUserRank}");
        }

        public (bool, string) AssignRegistrationDate(Dictionary<string, string> data, string token)
        {
            return (false, "");
        }

        public Rank CheckRank(string token)
        {
            var user = LoggedUsers.GetUser(token);
            return user != null ? user.User.Rank : Rank.None;
        }

        public (bool, string) ValidateUser(Dictionary<string, string> data)
        {
            return (false, "");
        }
    }
}