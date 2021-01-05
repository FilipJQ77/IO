using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Users
{
    class UserFactory
    {
        public IUser CreateStudent(User user, StudentData data)
        {
            var student = new Student
            {
                User = user,
                StudentData = data,
            };

            return student;
        }

        public IUser CreateAdmin(User user)
        {
            var admin = new Administrator
            {
                User = user,
            };

            return admin;
        }
    }
}