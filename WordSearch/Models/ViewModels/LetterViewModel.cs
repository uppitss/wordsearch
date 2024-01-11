using Spectre.MicroMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Input;
using WordSearch.Models.MicroMVVM;
using WordSearch.Services;

namespace WordSearch.Models.ViewModels
{
    public enum LetterStatus
    {
        Neutral,
        InWord,
        NotInWord
    }
    internal class LetterViewModel:ObservableObject
    {
        private string _letter;
        public string Letter
        {
            get { return _letter; }
            set
            {
                _letter = value;
                RaisePropertyChanged("Letter");
                RaisePropertyChanged("Name");
            }
        }
        public string Name
        {
            get
            {
                string retVal = Letter;
                switch (Status)
                {
                    case LetterStatus.InWord: retVal += " (+)"; break;
                    case LetterStatus.NotInWord: retVal += " (-)";break;
                }
                return retVal;
            }
            
        }
        private LetterStatus _status;
        public LetterStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
                RaisePropertyChanged("Name");
            }
        }


        public LetterViewModel(string letter)
        {
            Letter = letter;
            Status = LetterStatus.Neutral;
        }

        public event EventHandler<LetterEventArgs> OnChangeStatus;

        #region Commands
        
        public void ChangeStatusExecute()
        {
            switch (Status)
            {
                case LetterStatus.Neutral: Status = LetterStatus.InWord; break;
                case LetterStatus.InWord: Status = LetterStatus.NotInWord; break;
                case LetterStatus.NotInWord: Status = LetterStatus.Neutral; break;
            }

            OnChangeStatus?.Invoke(this, new LetterEventArgs(this.Letter, this.Status));
        }
        public ICommand ChangeStatus
        {
            get
            {
                return new RelayCommand(ChangeStatusExecute);
            }
        }

        #endregion
        public void Clear()
        {
            Status = LetterStatus.Neutral;
        }
    }
    public class LetterEventArgs : EventArgs
    {
        public string Letter;
        public LetterStatus Status;
        public LetterEventArgs(string letter, LetterStatus status)
        {
            this.Letter = letter;
            this.Status = status;
        }
    }
}
