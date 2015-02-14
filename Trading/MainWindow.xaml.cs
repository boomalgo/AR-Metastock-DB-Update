using System.Windows.Input;
using Trading.Commands;

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
    }
}
