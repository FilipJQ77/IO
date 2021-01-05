using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Encryption
{
    interface IHasher
    {
        string Hash(string password);
        bool Check(string hash, string password);
    }
}
