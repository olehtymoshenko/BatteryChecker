using System.Windows;
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
            ViewModel.CreateReport(DefaultDialogs.TargetFileType.PDF);
        }

        public void CreateReportDOC_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateReport(DefaultDialogs.TargetFileType.DOC_DOCX);
        }

        public void InsertTableInTemplate_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.InsertTableInTemplateDOC();
        }

        public void ShowInfAboutApp_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowInfAboutApp();
        }

        public void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
