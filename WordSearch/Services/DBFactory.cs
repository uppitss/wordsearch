using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch.Services
{
    internal class DBFactory
    {
        public static IDB GetDBInstance(bool isMock)
        {
            if (isMock)
            {
                return new MockDBService();
            }
            else
            {
                return new FromFileDBService();
            }
        }
    }
}
