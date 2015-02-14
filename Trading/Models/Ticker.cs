using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Trading.Annotations;

namespace Trading.Models
{
    [Serializable]
    public class Ticker : INotifyPropertyChanged
    {
        private string _name;
        private string _symbol;
        private bool _bond;
        private string _market;
        private bool _mervalIndex;
        private string _lastUpdate;
        private int _selectedSource;
        private ObservableCollection<Source> _sources;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
