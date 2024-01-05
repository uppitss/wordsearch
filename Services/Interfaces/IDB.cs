using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDB
    {
        public bool Add(string word);
        public string[] Search(string wordMask);
    }
}
