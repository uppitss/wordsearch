using Services.Interfaces;
using Spectre.MicroMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using WordSearch.Models.MicroMVVM;
using WordSearch.Services;

namespace WordSearch.Models.ViewModels
{
    public class MainWindowViewModel : ObservableObject, IDataErrorInfo
    {
        protected IDB _dbService;
        protected bool _isEnabled;
        protected List<Letter> _letters;
        public List<Letter> Letters
        {
            get { return _letters; }
            set
            {
                _letters = value;
                RaisePropertyChanged("Letters");
            }
        }
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }
        private string _word;
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                RaisePropertyChanged("Word");
            }
        }

        private string _notInWordChars;
        public string NotInWordChars
        {
            get
            {
                return _notInWordChars;
            }
            set
            {
                _notInWordChars = value;
                RaisePropertyChanged("NotInWordChars");
            }
        }
        public string[] NotInWordCharsArr
        {
            get
            {
                if (!string.IsNullOrEmpty(NotInWordChars))
                {
                    return NotInWordChars.Replace(" ", "").Split(',').ToArray();
                }
                return new string[] { };
            }
        }
        private string _inWordChars;
        public string InWordChars
        {
            get
            {
                return _inWordChars;
            }
            set
            {
                _inWordChars = value;
                RaisePropertyChanged("InWordChars");
            }
        }
        public string[] InWordCharsArr
        {
            get
            {
                if (!string.IsNullOrEmpty(InWordChars))
                {
                    return InWordChars.Replace(" ", "").Split(',').ToArray();
                }
                return new string[] { };
            }
        }

        protected ObservableCollection<string> _wordsList;
        public ObservableCollection<string> WordsList
        {
            get
            {
                return _wordsList;
            }
            set
            {
                _wordsList = value;
                RaisePropertyChanged("WordsList");
            }
        }
        public MainWindowViewModel()
        {
            IsEnabled = true;
            _dbService = DBFactory.GetDBInstance(false);
            AddLetters();
           
        }
        #region Letters Functions
        protected void AddLetters()
        {
            List<Letter> letters = new List<Letter>();
            letters.Add(AddLetter("A"));
            letters.Add(AddLetter("Б"));
            letters.Add(AddLetter("В"));
            letters.Add(AddLetter("Г"));
            letters.Add(AddLetter("Д"));
            letters.Add(AddLetter("Е"));
            letters.Add(AddLetter("Ё"));
            letters.Add(AddLetter("Ж"));
            letters.Add(AddLetter("З"));
            letters.Add(AddLetter("И"));
            letters.Add(AddLetter("Й"));
            letters.Add(AddLetter("К"));
            letters.Add(AddLetter("Л"));
            letters.Add(AddLetter("М"));
            letters.Add(AddLetter("Н"));
            letters.Add(AddLetter("О"));
            letters.Add(AddLetter("П"));
            letters.Add(AddLetter("Р"));
            letters.Add(AddLetter("С"));
            letters.Add(AddLetter("Т"));
            letters.Add(AddLetter("У"));
            letters.Add(AddLetter("Ф"));
            letters.Add(AddLetter("Х"));
            letters.Add(AddLetter("Ц"));
            letters.Add(AddLetter("Ч"));
            letters.Add(AddLetter("Ш"));
            letters.Add(AddLetter("Щ"));
            letters.Add(AddLetter("Ъ"));
            letters.Add(AddLetter("Ь"));
            letters.Add(AddLetter("Ы"));
            letters.Add(AddLetter("Э"));
            letters.Add(AddLetter("Ю"));
            letters.Add(AddLetter("Я"));
            Letters = letters;
        }
        protected Letter AddLetter(string letter)
        {
            var l = new Letter(letter);
            l.OnChangeStatus += AddLetter_OnChangeStatus;
            return l;
        }

        private void AddLetter_OnChangeStatus(object? sender, LetterEventArgs e)
        {
            int i = 0;
        }
        #endregion Letters Functions
        #region Commands
        public bool CanSearchWordsExecute()
        {
            return IsEnabled;
        }
        public async void SearchWordsExecute()
        {
            ForceValidate();
            if (!IsError)
            {
                IsEnabled = false;
                WordsList = new ObservableCollection<string>(await new SearchWordsService().SearchWords(_dbService, Word, InWordCharsArr, NotInWordCharsArr));
                IsEnabled = true;
            }
        }
        public ICommand SearchWords
        {
            get
            {
                return new RelayCommand(SearchWordsExecute, CanSearchWordsExecute);
            }
        }
        
        #endregion

        #region IDataErrorInfo
        public bool UseImmediateValidation = false;
        public bool IsError = false;
        protected void ForceValidate()
        {
            IsError = false;
            UseImmediateValidation = true;
            RaisePropertyChanged("Word");
            RaisePropertyChanged("NotInWordChars");
            RaisePropertyChanged("InWordChars");
        }
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = string.Empty;
                if (UseImmediateValidation)
                {
                    switch (columnName)
                    {
                        case "Word":
                            {

                                if (string.IsNullOrEmpty(Word))
                                {
                                    msg = "Укажите маску слова";
                                }
                                else
                                {
                                    if (!Regex.IsMatch(Word.ToLower().Trim(), @"^[a-я,*]*$"))
                                    {
                                        msg = "Допустимые символы а-я и знак *";
                                    }
                                }
                            }
                            break;
                        case "NotInWordChars":
                            {
                                if (!string.IsNullOrEmpty(NotInWordChars))
                                {
                                    var arr = NotInWordChars.Replace(" ", "").Split(',');
                                    foreach (var item in NotInWordChars.Replace(" ", "").Split(','))
                                    {
                                        if (!Regex.IsMatch(item.ToLower(), "^[а-я]*$"))
                                        {
                                            msg = "Неверный формат строки";
                                        }
                                    }
                                }
                            }
                            break;
                        case "InWordChars":
                            {
                                if (!string.IsNullOrEmpty(InWordChars))
                                {
                                    var arr = InWordChars.Replace(" ", "").Split(',');
                                    foreach (var item in InWordChars.Replace(" ", "").Split(','))
                                    {
                                        if (!Regex.IsMatch(item.ToLower(), "^[а-я]*$"))
                                        {
                                            msg = "Неверный формат строки";
                                        }
                                    }
                                }
                            }
                            break;

                    }
                    if (IsError == false && !string.IsNullOrEmpty(msg))
                    {
                        IsError = true;
                    }
                }
                return msg;
            }
        }
        #endregion
    }
 
}
