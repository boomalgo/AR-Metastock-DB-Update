using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Trading.Annotations;

namespace Trading.Models
{
    [Serializable]
    public class Source : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _symbol;

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

        [XmlAttribute]
        public string SymbolId
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (value == null || _symbol == value) return;
                _symbol = value;
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
