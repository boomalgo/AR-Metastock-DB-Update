using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Trading.Annotations;
using Trading.Models;
using TradingTools;

namespace Trading.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _adrsFolder;
        private string _mervalFolder;
        private bool _mervalFolderReady;
        private bool _adrsFolderReady;
        private List<Ticker> _mervalTickers;
        private List<Ticker> _adrTickers;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Ticker> MervalTickers
        {
            get
            {
                return _mervalTickers;
            }
            set
            {
                if (value == null) return;
                _mervalTickers = value;
                OnPropertyChanged();
            }
        }

        public List<Ticker> AdrTickers
        {
            get
            {
                return _adrTickers;
            }
            set
            {
                if (value == null) return;
                _adrTickers = value;
                OnPropertyChanged();
            }
        }

        public bool AdrsFolderReady
        {
            get
            {
                return _adrsFolderReady;
            }
            set
            {
                _adrsFolderReady = value;
                OnPropertyChanged();
            }
        }

        public bool MervalFolderReady
        {
            get
            {
                return _mervalFolderReady;
            }
            set
            {
                _mervalFolderReady = value;
                OnPropertyChanged();
            }
        }

        public string AdrsFolder
        {
            get
            {
                return _adrsFolder;
            }
            set
            {
                if (value == null) return;
                _adrsFolder = value;
                CheckAdrsFolder();
                OnPropertyChanged();
            }
        }

        public string MervalFolder
        {
            get
            {
                return _mervalFolder;
            }
            set
            {
                if (value == null) return;
                _mervalFolder = value;
                CheckMervalFolder();
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            if (MervalTickers == null) MervalTickers = new List<Ticker>();
            if (AdrTickers == null) AdrTickers = new List<Ticker>();
            const string path = "Resources/Tickers.xml";

            var serializer = new XmlSerializer(typeof(Tickers));

            var reader = new StreamReader(path);
            var tickers = ((Tickers)serializer.Deserialize(reader)).Ticker;
            reader.Close();

            foreach (var ticker in tickers.Where(x => x.Market == "BCBA"))
            {
                ticker.LastUpdate = "Not Available";
                MervalTickers.Add(ticker);
            }

            foreach (var ticker in tickers.Where(x => x.Market == "NYSE"))
            {
                ticker.LastUpdate = "Not Available";
                AdrTickers.Add(ticker);
            }

            CheckMervalFolder();
            CheckAdrsFolder();
        }

        private void CheckMervalFolder()
        {
            var meta = new MetaLib();

            var mervalPath = MervalFolder;

            MervalFolderReady = !String.IsNullOrEmpty(mervalPath) && Directory.Exists(mervalPath) &&
                                       meta.IsMetaStockDirectory(mervalPath);

            if (!MervalFolderReady)
            {
                _mervalFolder = null;
                Properties.Settings.Default.MervalPath = null;
                Properties.Settings.Default.Save();
            }

            foreach (var mervalTicker in MervalTickers)
            {
                mervalTicker.LastUpdate = GetLastDateStatus(MervalFolder, mervalTicker.Symbol);
            }

            Properties.Settings.Default.MervalPath = mervalPath;
            Properties.Settings.Default.Save();
        }

        private void CheckAdrsFolder()
        {
            var meta = new MetaLib();

            var adrsPath = AdrsFolder;

            AdrsFolderReady = !String.IsNullOrEmpty(adrsPath) && Directory.Exists(adrsPath) &&
                                       meta.IsMetaStockDirectory(adrsPath);

            if (!AdrsFolderReady)
            {
                _adrsFolder = null;
                Properties.Settings.Default.ADRsPath = null;
                Properties.Settings.Default.Save();
            }

            foreach (var adrTicker in AdrTickers)
            {
                adrTicker.LastUpdate = GetLastDateStatus(MervalFolder, adrTicker.Symbol);
            }

            Properties.Settings.Default.ADRsPath = adrsPath;
            Properties.Settings.Default.Save();
        }

        private static string GetLastDateStatus(string metastockFolder, string symbol)
        {
            var meta = new MetaLib();
            meta.OpenDirectory(metastockFolder, FileAccess.ReadWrite);
            var found = meta.OpenSecuritySymbol(symbol);
            if (!found) return "Symbol not found";
            meta.SeekForLastRecord();
            return meta.ReadPriceRecord().Date.Date.ToShortDateString();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
