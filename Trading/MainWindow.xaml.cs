namespace Trading
{
    using System.Windows;
    using System.Windows.Forms;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var mervalViewModel = new PanelViewModel(Properties.Settings.Default.Merval, Properties.Settings.Default.MervalMarket);
            MervalTab.DataContext = mervalViewModel;

            var adrsViewModel = new PanelViewModel(Properties.Settings.Default.ADRS, Properties.Settings.Default.ADRSMarket);
            AdrsTab.DataContext = adrsViewModel;
        }

        private void MervalFolderSelectBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            ((PanelViewModel)MervalTab.DataContext).MetastockFolder = dialog.SelectedPath;
            MervalUpdateGrid.ItemsSource = null;
            MervalUpdateGrid.ItemsSource = ((PanelViewModel)MervalTab.DataContext).Tickers;
        }

        private void AdrsFolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            ((PanelViewModel)AdrsTab.DataContext).MetastockFolder = dialog.SelectedPath;
            AdrsUpdateGrid.ItemsSource = null;
            AdrsUpdateGrid.ItemsSource = ((PanelViewModel)MervalTab.DataContext).Tickers;
        }
    }
}
