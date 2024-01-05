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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected MainWindowViewModel viewModel;
        public MainWindow()
        {            
         
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            viewModel.OnAddNewWord += ViewModel_OnAddNewWord;
            DataContext = viewModel;
            
        }

        private void ViewModel_OnAddNewWord(object? sender, OnAddNewWordEventArgs e)
        {         
            Window wnd = new Window();
            wnd.Owner = this;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.SizeToContent = SizeToContent.WidthAndHeight;

            wnd.Title = "Добавить новое слово";
            wnd.Closed += Wnd_Closed;
            var control = new AddNewWord(wnd,e.DbService);            
            wnd.Content = control;
            wnd.ShowDialog();
        }

        private void Wnd_Closed(object? sender, EventArgs e)
        {
           
        }
    }
}
