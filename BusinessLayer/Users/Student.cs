using DataAccess.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Users
{
    using Encryption;

    class Student : IUser
    {
        public User User { get; set; }
        public StudentData StudentData { get; set; }

        public bool checkPassword(string password)
        {
            return new HasherFactory().GetHasher().Check(User.Password, password);
        }
    }
}
