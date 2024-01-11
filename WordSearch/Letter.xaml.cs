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
    /// Interaction logic for Letter.xaml
    /// </summary>
    public partial class Letter : UserControl
    {
        LetterViewModel viewModel;
        public event EventHandler<LetterEventArgs> OnChangeStatus;
        public Letter(string letter)
        {
            viewModel = new LetterViewModel(letter);
            viewModel.OnChangeStatus += ViewModel_OnChangeStatus;
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void ViewModel_OnChangeStatus(object? sender, LetterEventArgs e)
        {
            OnChangeStatus?.Invoke(this, e);
        }
        public void Clear()
        {
            viewModel.Clear();
        }
    }
}
