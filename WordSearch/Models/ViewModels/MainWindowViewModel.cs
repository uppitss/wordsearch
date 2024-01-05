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
        }
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

        public void AddNewWordExecute()
        {
            OnAddNewWord?.Invoke(this, new OnAddNewWordEventArgs(_dbService));
        }
        public ICommand AddNewWord
        {
            get
            {
                return new RelayCommand(AddNewWordExecute);
            }
        }
        public event EventHandler<OnAddNewWordEventArgs> OnAddNewWord;
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
    public class OnAddNewWordEventArgs : EventArgs
    {
        public IDB DbService { get; set; }
        public OnAddNewWordEventArgs(IDB dbService)
        {
            DbService = dbService;
        }
    }
}
