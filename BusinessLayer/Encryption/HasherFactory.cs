using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Encryption
{
    class HasherFactory
    {
        public IHasher GetHasher()
        {
            return new Hasher();
        }
    }
}
