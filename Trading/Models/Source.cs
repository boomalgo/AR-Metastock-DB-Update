using System;
using System.Xml.Serialization;

namespace Trading.Models
{
    [Serializable]
    public class Source
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string SymbolId { get; set; }
    }
}
