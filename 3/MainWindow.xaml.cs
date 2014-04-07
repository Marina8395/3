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

namespace _3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TrainsViewModel viewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new TrainsViewModel();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Load();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }
    }
}
