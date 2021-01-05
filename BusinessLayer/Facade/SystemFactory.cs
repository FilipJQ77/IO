using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Facade
{
    public class SystemFactory
    {
        private ISystem _system;
        public ISystem System { get
            {
                if (_system == null)
                {
                    _system = new System();
                }
                return _system;
            }
        }
    }
}
