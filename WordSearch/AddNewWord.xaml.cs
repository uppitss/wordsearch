using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordSearch.Models.ViewModels;

namespace WordSearch
{
    /// <summary>
    /// Interaction logic for AddNewWord.xaml
    /// </summary>
    public partial class AddNewWord : UserControl
    {
        protected Window _parentWnd;
        protected AddNewWordViewModel viewModel;
        public AddNewWord(Window parentWnd, IDB dbService)
        {
            InitializeComponent();
            viewModel = new AddNewWordViewModel(dbService);
            viewModel.OnAddNewWord += ViewModel_OnAddNewWord;
            this.DataContext = viewModel;
            _parentWnd = parentWnd;
        }

        private void ViewModel_OnAddNewWord(object? sender, EventArgs e)
        {
            _parentWnd.Close();
        }
    }
}
