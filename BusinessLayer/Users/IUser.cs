using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Users
{
    interface IUser
    {
        User User { get; set; }

        bool checkPassword(string password);
    }
}
