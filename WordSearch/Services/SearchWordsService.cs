using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WordSearch.Services
{
    internal class SearchWordsService
    {
        public async Task<string[]> SearchWords(IDB dbService, string wordMask, string[] InWordChars, string[] NotInWordChars)
        {
            //Сначала мы ищем в БД слово по маске dbService.Search(wordMask)
            //Далее в блоке Where пробегаемся по каждому слову из наденных и проверяем два условия
            //В массиве букв, которые вообще не должны быть в слове пробегаемся по каждой букве и проверяем, что в найденных словах нет ни одного такого слова, где эта буква есть NotInWordChars.Where(s=>p.Contains(s)).Count() == 0
            //В массиве букв, которые должны быть - пробегаемся по каждой букве и размеры исходной коллекции букв и найденных совпадений совпадают
            return dbService.Search(wordMask).Where(p=>NotInWordChars.Where(s=>p.Contains(s)).Count() == 0 && InWordChars.Where(s=>p.Contains(s)).Count() == InWordChars.Count()).ToArray();            
            
        }
    }
}
