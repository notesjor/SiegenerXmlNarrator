using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SiegenerXmlNarrator.Model
{
  [XmlRoot]
  [Serializable]
  public class npc
  {
    [XmlAttribute]
    public string color { get; set; }

    [XmlText]
    public string turn { get; set; }

    [XmlAttribute]
    public bool isLeft { get; set; }

    [XmlAttribute]
    public string name { get; set; }
  }
}
