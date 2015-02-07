using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

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
            }
        }

        [XmlArray]
        public List<Source> Sources { get; set; }

        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
