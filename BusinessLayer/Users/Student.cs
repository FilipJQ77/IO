using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Users
{
    using Encryption;

    class Student : IUser
    {
        public User User { get; set; }
        public StudentData StudentData { get; set; }

        public bool CheckPassword(string password)
        {
            return new HasherFactory()
                .GetHasher()
                .Check(User.Password, password);
        }
    }
}