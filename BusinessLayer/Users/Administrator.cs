using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Users
{
    using Encryption;

    class Administrator : IUser
    {
        public User User { get; set; }

        public bool CheckPassword(string password)
        {
            return new HasherFactory()
                .GetHasher()
                .Check(User.Password, password);
        }
    }
}