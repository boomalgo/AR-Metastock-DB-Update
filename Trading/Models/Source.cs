namespace Trading.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;
    using Annotations;
    using TradingTools;

    [Serializable]
    public abstract class Source : INotifyPropertyChanged
    {
        #region Fields
        private int _id;
        private string _name;
        private Dictionary<string, string> _parameters;
        #endregion

        #region Properties
        [XmlAttribute]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == null || _name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        [XmlArray]
        public Dictionary<string, string> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                if (value == null || _parameters == value) return;
                _parameters = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public abstract List<PriceRecord> GetPriceRecords(DateTime lastDateTime);
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
