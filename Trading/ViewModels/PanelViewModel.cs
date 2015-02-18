using System;
using System.Windows;

namespace Trading.ViewModels
{
    #region Using
    using Annotations;
    using Commands;
    using Helpers;
    using Models;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Xml.Serialization;
    #endregion

    class PanelViewModel : INotifyPropertyChanged
    {
        #region Properties fields
        private readonly string _panel;
        private string _metastockFolder;
        private bool _metastockFolderReady;
        private ObservableCollection<Ticker> _tickers;
        private int _backgroundWorkerProgress;
        private string _backgroundWorkerProgressText;
        private string _mainButtonAction;
        private RelayCommand _updateDatabaseCommand;
        private bool _bwBusy;

        #endregion

        #region Commands
        public ICommand SelectFolderCommand
        {
            get
            {
                return new RelayCommand(SelectFolder);
            }
        }

        public ICommand UpdateDatabaseCommand
        {
            get
            {
                return _updateDatabaseCommand ?? (_updateDatabaseCommand = new RelayCommand(UpdateDatabase));
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<Ticker> Tickers
        {
            get
            {
                return _tickers;
            }
            set
            {
                if (value == null) return;
                _tickers = value;
                OnPropertyChanged();
            }
        }

        public bool MetastockFolderReady
        {
            get
            {
                return _metastockFolderReady;
            }
            set
            {
                _metastockFolderReady = value;
                OnPropertyChanged();
            }
        }
        
        public string MetastockFolder
        {
            get
            {
                return _metastockFolder;
            }
            set
            {
                if (value == null) return;
                _metastockFolder = value;
                CheckMetastockFolder();
                OnPropertyChanged();
            }
        }

        public string MainButtonAction
        {
            get
            {
                return _mainButtonAction;
            }
            set
            {
                if (value == null) return;
                _mainButtonAction = value;
                OnPropertyChanged();
            }
        }

        public int BackgroundWorkerProgress
        {
            get
            {
                return _backgroundWorkerProgress;
            }
            set
            {
                _backgroundWorkerProgress = value;
                OnPropertyChanged();
            }
        }

        public string BackgroundWorkerProgressText
        {
            get
            {
                return _backgroundWorkerProgressText;
            }
            set
            {
                _backgroundWorkerProgressText = value;
                OnPropertyChanged();
            }
        }

        public bool BackgroundWorkerNotBusy
        {
            get
            {
                return _bwBusy;
            }
            set
            {
                _bwBusy = value;
                OnPropertyChanged();
            }
        }

        public System.ComponentModel.BackgroundWorker BackgroundWorker { get; set; }
        #endregion

        #region Constructors
        public PanelViewModel(string panel, string market)
        {
            _panel = panel;
            MainButtonAction = "Update Database";
            BackgroundWorkerNotBusy = true;

            if (Tickers == null) Tickers = new ObservableCollection<Ticker>();
            const string path = "Resources/Tickers.xml";

            var serializer = new XmlSerializer(typeof(Tickers));

            var reader = new StreamReader(path);
            var tickers = ((Tickers)serializer.Deserialize(reader)).Ticker;
            reader.Close();

            foreach (var ticker in tickers.Where(x => x.Market == market && x.Sources.Count > 0))
            {
                ticker.LastUpdate = "Not Available";
                Tickers.Add(ticker);
            }

            if (!String.IsNullOrEmpty(Properties.Settings.Default[_panel + "Path"].ToString()))
            {
                MetastockFolder = Properties.Settings.Default[_panel + "Path"].ToString();
            }
        }
        #endregion

        #region Public Methods
        public void SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.SelectedPath))
            {
                MetastockFolder = dialog.SelectedPath;
            }
        }

        public void CheckMetastockFolder()
        {
            if (!Metastock.IsValidMetastockFolder(MetastockFolder))
            {
                _metastockFolder = null;
                Properties.Settings.Default[_panel + "Path"] = null;
                Properties.Settings.Default.Save();
                MetastockFolderReady = false;
                MessageBox.Show(@"Selected folder is not a valid folder or does not contain Metastock information",
                    @"Error in folder");
                return;
            }

            BackgroundWorker = Helpers.BackgroundWorker.Launch(BwGetTickersLastDate, BwProgressChanged);

            Properties.Settings.Default[_panel + "Path"] = MetastockFolder;
            Properties.Settings.Default.Save();
        }

        private void BwGetTickersLastDate(object sender, DoWorkEventArgs e)
        {
            MetastockFolderReady = false;
            BackgroundWorkerNotBusy = false;
            MainButtonAction = "Loading...";

            var worker = sender as System.ComponentModel.BackgroundWorker;

            if (worker != null)
                worker.ReportProgress(0);

            for (var i = 0; i < Tickers.Count; i++)
            {
                if (worker != null && worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                
                var ticker = Tickers[i];
                ticker.LastUpdate = Metastock.GetLastDateStatus(MetastockFolder, ticker.Symbol);

                if (worker != null)
                    worker.ReportProgress(((i + 1) * 100) / Tickers.Count);
            }

            if (worker != null && !e.Cancel)
                worker.ReportProgress(100);
            else if (worker != null)
                worker.ReportProgress(0);

            MainButtonAction = "Update Database";
            MetastockFolderReady = true;
            BackgroundWorkerNotBusy = true;
        }

        public void UpdateDatabase()
        {
            BackgroundWorker = Helpers.BackgroundWorker.Launch(BwUpdateDatabase, BwProgressChanged);
        }

        public void BwUpdateDatabase(object sender, DoWorkEventArgs e)
        {
            //http://www.codeproject.com/Articles/299436/WPF-Localization-for-Dummies
            MainButtonAction = "Cancel";
            _updateDatabaseCommand.UpdateCommand(BwCancel);
            BackgroundWorkerNotBusy = false;

            var worker = sender as System.ComponentModel.BackgroundWorker;

            if (worker != null)
                worker.ReportProgress(0);

            for (var i = 0; i < Tickers.Count; i++)
            {
                if (worker != null && worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                var ticker = Tickers[i];
                ticker.Update(MetastockFolder);
                if (worker != null)
                    worker.ReportProgress(((i + 1) * 100) / Tickers.Count);
            }

            if (worker != null && !e.Cancel)
                worker.ReportProgress(100);
            else if (worker != null)
                worker.ReportProgress(0);

            MainButtonAction = "Update Database";
            _updateDatabaseCommand.UpdateCommand(UpdateDatabase);
            BackgroundWorkerNotBusy = true;
        }

        public void BwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorkerProgress = e.ProgressPercentage;
            BackgroundWorkerProgressText = (e.ProgressPercentage + "%");
            if (e.ProgressPercentage == 100)
            {
                BackgroundWorkerProgressText = "Done";
            }
        }

        private void BwCancel()
        {
            if (BackgroundWorker.WorkerSupportsCancellation)
            {
                BackgroundWorker.CancelAsync();
            }
        }

        #endregion

        #region Notification
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
