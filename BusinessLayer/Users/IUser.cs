using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Users
{
    interface IUser
    {
        User User { get; set; }

        bool CheckPassword(string password);
    }
}
