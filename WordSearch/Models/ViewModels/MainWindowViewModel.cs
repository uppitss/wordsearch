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
               
        protected List<string> _notInWordChars { get; set; }

        protected List<string> _inWordChars { get; set; }
        

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
            _notInWordChars = new List<string>();
            _inWordChars = new List<string>();
            AddLetters();
        }
        #region Letters Functions
        protected void AddLetters()
        {
            List<Letter> letters = new List<Letter>();
            letters.Add(AddLetter("А"));
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
            int inWordIndex = _inWordChars.FindIndex(p=>p == e.Letter.ToLower());
            int notInWordIndex = _notInWordChars.FindIndex(p => p == e.Letter.ToLower());

            if (inWordIndex != -1)
            {
                _inWordChars.Remove(e.Letter.ToLower());
            }
            if (notInWordIndex != -1)
            {
                _notInWordChars.Remove(e.Letter.ToLower());
            }

            switch (e.Status)
            {
                case LetterStatus.InWord: _inWordChars.Add(e.Letter.ToLower()); break;
                case LetterStatus.NotInWord: _notInWordChars.Add(e.Letter.ToLower()); break;
            }
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
                WordsList = new ObservableCollection<string>(await new SearchWordsService().SearchWords(_dbService, Word, _inWordChars.ToArray(),_notInWordChars.ToArray()));
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
