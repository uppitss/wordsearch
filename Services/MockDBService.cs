using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Services
{
    public class MockDBService : IDB
    {
        protected List<string> _words;
        public List<string> Words
        {
            get
            {
                return _words;
            }
        }
        public MockDBService()
        {
            _words = new List<string> { "топор", "аванс", "бекас","техас", "вихрь", "грамм", "деталь" };
        }

        public string[] Search(string wordMask)
        {
            string mask = "^"+wordMask.Replace("*", "[а-я]")+"$";
            List<string> retArr = _words.Where(p => Regex.IsMatch(p, mask)).ToList();
            return retArr.ToArray();
        }
    }
}
