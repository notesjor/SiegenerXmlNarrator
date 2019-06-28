using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiegenerXmlNarrator.Model
{
  [XmlRoot]
  [Serializable]
  public class gametask
  {
    [XmlElement]
    public ui ui { get; set; }

    [XmlArrayItem]
    public npc[] npcs { get; set; }

    [XmlText]
    public string question { get; set; }

    [XmlAttribute]
    public string id { get; set; }

    [XmlElement("change")]
    public change[] change { get; set; }
  }
}
