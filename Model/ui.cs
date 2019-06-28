using System;
using System.Xml.Serialization;

namespace SiegenerXmlNarrator.Model
{
    [XmlRoot]
    [Serializable]
    public class ui
    {
        [XmlAttribute]
        public string background { get; set; }
        [XmlAttribute]
        public string left { get; set; }
        [XmlAttribute]
        public string right { get; set; }
    }
}
