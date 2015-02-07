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
        //public ObservableCollection<Ticker> Tickers { get; set; }

        //public ObservableCollection<Source> Sources { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Ticker> MervalTickers { get; set; }

        public List<Ticker> AdrTickers { get; set; }

        public bool AdrsFolderReady { get; set; }

        public bool MervalFolderReady { get; set; }

        public string AdrsFolder
        {
            get
            {
                return _adrsFolder;
            }
            set
            {
                if (value == null || _adrsFolder == value) return;

                _adrsFolder = value;
                CheckAdrsFolder();
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
                if (value == null || _mervalFolder == value) return;

                _mervalFolder = value;
                CheckMervalFolder();
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

            if (!MervalFolderReady) return;

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

            if (!AdrsFolderReady) return;

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
