namespace Trading.Models
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Sources; 
    #endregion

    [Serializable]
    public class Tickers : IXmlSerializable
    {
        #region Properties
        [XmlArray("Tickers")]
        [XmlArrayItem("Ticker", typeof(Ticker))]
        public List<Ticker> Ticker { get; set; } 
        #endregion

        #region XMLSerialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var document = XDocument.Load(reader);
            if (Ticker == null) Ticker = new List<Ticker>();
            if (document.Root == null) return;
            foreach (var xNode in document.Root.Elements().Nodes().Where(x => x.NodeType == XmlNodeType.Element))
            {
                var tickerNode = (XElement)xNode;
                if (tickerNode.Name != "Ticker") continue;

                var ticker = new Ticker
                {
                    Name = tickerNode.Attribute("Name").Value,
                    Symbol = tickerNode.Attribute("Symbol").Value,
                    SelectedSource = Int32.Parse(tickerNode.Attribute("SelectedSource").Value),
                    Market = tickerNode.Attribute("Market").Value,
                    Sources = new ObservableCollection<Source>()
                };

                var sources = (XElement)tickerNode.FirstNode;
                if (sources.Name != "Sources") continue;
                foreach (var sourceNode in sources.Elements())
                {
                    Source source;
                    switch (sourceNode.Attribute("Name").Value)
                    {
                        case "Invertir Online":
                            source = new InvertirOnline()
                            {
                                Id = Int32.Parse(sourceNode.Attribute("Id").Value),
                                Name = sourceNode.Attribute("Name").Value,
                                Parameters = new Dictionary<string, string>()
                            };
                            break;
                        case "Rava Online":
                            source = new RavaOnline()
                            {
                                Id = Int32.Parse(sourceNode.Attribute("Id").Value),
                                Name = sourceNode.Attribute("Name").Value,
                                Parameters = new Dictionary<string, string>()
                            };
                            break;
                        default:
                            continue;
                    }

                    var parameters = (XElement)sourceNode.FirstNode;
                    if (parameters.Name != "Parameters") continue;
                    foreach (var parameterNode in parameters.Elements())
                    {
                        source.Parameters.Add(parameterNode.Attribute("Name").Value,
                            parameterNode.Attribute("Value").Value);
                    }

                    ticker.Sources.Add(source);
                }

                Ticker.Add(ticker);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Test", "Value");
        } 
        #endregion
    }
}
