using System;
using System.Xml.Serialization;

namespace Trading.Models
{
    [Serializable]
    public class Tickers
    {
        [XmlArray("Tickers")]
        [XmlArrayItem("Ticker", typeof(Ticker))]
        public Ticker[] Ticker { get; set; }
    }
}
