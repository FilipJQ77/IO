using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fitlibrary;

namespace BusinessLayer.Tests.fitnesse
{
    public class ColumnFixtureTest : fit.ColumnFixture
    {
        public String firstPart;
        public String secondPart;
        public String XD;
        public String Together
        {
            get
            {
                return firstPart + ", " + secondPart + ", " + XD;
            }
        }
        public int TotalLength()
        {
            return firstPart.Length + secondPart.Length + XD.Length;
        }
    }
}
