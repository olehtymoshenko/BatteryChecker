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
using BatteryChecker.ViewModel;

namespace BatteryChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.propertiesDG.ItemsSource = ViewModel.properties;            
        }

        public void CreateReportPDF_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateReportPDF();
        }

        public void CreateReportDOC_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateReportDOC();
        }

        public void InsertTableInTemplate_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.InsertTableInTemplateDOC();
        }
    }
}
