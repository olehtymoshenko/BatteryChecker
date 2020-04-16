using System.Windows;
using BatteryChecker.ViewModel;

namespace BatteryChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Field with viewmodel for this window
        /// </summary>
        private MainWindowViewModel ViewModel { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.propertiesDG.ItemsSource = ViewModel.properties; // bind observable collection with UI
        }

        /// <summary>
        /// Handler for creating PDF reports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateReportPDF_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateReport(DefaultDialogs.TargetFileType.PDF);
        }

        /// <summary>
        /// Handler for creating DOC_DOCX reports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateReportDOC_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateReport(DefaultDialogs.TargetFileType.DOC_DOCX);
        }

        /// <summary>
        /// Handler for inserting battery information into template (DOC)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InsertTableInTemplate_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.InsertTableInTemplateDOC();
        }

        /// <summary>
        /// Handler for showing brief information about programm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowInfAboutApp_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowInfAboutApp();
        }

        /// <summary>
        /// Handler for shutdown application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
