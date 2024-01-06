using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class FromFileDBService : IDB
    {
        public string[] Search(string wordMask)
        {
            List<string> allWords = new List<string>();
            using (FileStream fs = File.OpenRead("russian_nouns.txt"))
            {
                using (StreamReader sr = new StreamReader(fs)) { 
                    while(!sr.EndOfStream)
                    {
                        string word = sr.ReadLine();
                        if (!String.IsNullOrEmpty(word))
                        {
                            allWords.Add(word.ToLower());
                        }                        
                    }
                }
            }
            string mask = "^" + wordMask.Replace("*", "[а-я]") + "$";
            return allWords.Where(p => Regex.IsMatch(p, mask)).ToArray();
        }
    }
}
