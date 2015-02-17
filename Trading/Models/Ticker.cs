using System.Linq;
using Trading.Helpers;

namespace Trading.Models
{
    #region Using
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;
    using Annotations;
    #endregion

    [Serializable]
    public class Ticker : INotifyPropertyChanged
    {
        #region Fields
        private string _name;
        private string _symbol;
        private bool _bond;
        private string _market;
        private bool _mervalIndex;
        private string _lastUpdate;
        private int _selectedSource;
        private ObservableCollection<Source> _sources; 
        #endregion

        #region Properties

        [XmlAttribute]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null && _name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [XmlAttribute]
        public int SelectedSource
        {
            get
            {
                return _selectedSource;
            }
            set
            {
                _selectedSource = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (value != null && _symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged();
                }
            }
        }

        [XmlAttribute]
        public bool Bond
        {
            get
            {
                return _bond;
            }
            set
            {
                _bond = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public string Market
        {
            get
            {
                return _market;
            }
            set
            {
                if (value != null && _market != value)
                {
                    _market = value;
                    OnPropertyChanged();
                }
            }
        }

        [XmlAttribute]
        public bool MervalIndex
        {
            get
            {
                return _mervalIndex;
            }
            set
            {
                _mervalIndex = value;
                OnPropertyChanged();
            }
        }

        public string LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
            set
            {
                _lastUpdate = value;
                OnPropertyChanged();
            }
        }

        [XmlArray]
        public ObservableCollection<Source> Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                OnPropertyChanged();
            }
        } 
        #endregion

        #region Public Methods
        public void Update(string metastockFolder)
        {
            DateTime lastDate;
            try
            {
                lastDate = DateTime.Parse(LastUpdate);
            }
            catch (Exception)
            {
                return;
            }

            var source = Sources.FirstOrDefault(x => x.Id == SelectedSource);
            if (source == null) return;

            var missingPriceRecords = source.GetPriceRecords(lastDate);

            Metastock.UpdateSymbol(metastockFolder, Symbol, missingPriceRecords);
            LastUpdate = Metastock.GetLastDateStatus(metastockFolder, Symbol);
        } 
        #endregion

        #region Notifications
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
