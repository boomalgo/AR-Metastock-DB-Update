using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Trading.ViewModels;

namespace Trading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainViewModel = new MainViewModel();
            
            DataContext = mainViewModel;
        }

        private void MervalFolderSelectBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            ((MainViewModel) DataContext).MervalFolder = dialog.SelectedPath;
            MervalUpdateGrid.ItemsSource = null;
            MervalUpdateGrid.ItemsSource = ((MainViewModel) DataContext).MervalTickers;
        }
    }
}
