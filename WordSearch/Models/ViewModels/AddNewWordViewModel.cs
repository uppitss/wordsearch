using Services.Interfaces;
using Spectre.MicroMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WordSearch.Models.MicroMVVM;
using WordSearch.Services;

namespace WordSearch.Models.ViewModels
{
    public class AddNewWordViewModel : ObservableObject, IDataErrorInfo
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
        private string _newWord;
        public string NewWord
        {
            get { return _newWord; }
            set
            {
                _newWord = value;
                RaisePropertyChanged("NewWord");
            }
        }

        
        public AddNewWordViewModel(IDB dbService)
        {
            IsEnabled = true;
            _dbService = dbService;
        }
        #region Commands
        public bool CanAddWordExecute()
        {
            return IsEnabled;
        }
        public void AddWordExecute()
        {
            ForceValidate();
            if (!IsError)
            {
                IsEnabled = false;
                _dbService.Add(NewWord);                
                OnAddNewWord?.Invoke(this, new EventArgs());
                IsEnabled = true;
            }
        }
        
        public ICommand AddWord
        {
            get
            {
                return new RelayCommand(AddWordExecute,CanAddWordExecute);
            }
        }
        public event EventHandler OnAddNewWord;
        #endregion

        #region IDataErrorInfo
        public bool UseImmediateValidation = false;
        public bool IsError = false;
        protected void ForceValidate()
        {
            IsError = false;
            UseImmediateValidation = true;
            RaisePropertyChanged("NewWord");
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
                        case "NewWord":
                            {

                                if (!string.IsNullOrEmpty(NewWord))
                                {
                                    if (!Regex.IsMatch(NewWord.ToLower(), "^[а-я]*$"))
                                    {
                                        msg = "Слово должно состоять только из русских букв";
                                    }
                                }
                                else { 
                                    msg = "Поле обязательно для заполнения";
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
