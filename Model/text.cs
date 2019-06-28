using System.Xml.Serialization;

namespace SiegenerXmlNarrator.Model
{
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [XmlType(AnonymousType = true)]
  [XmlRoot(Namespace = "", IsNullable = false)]
  public partial class text : gametask
  {
    private string jumpField;

    /// <remarks/>
    [XmlAttribute(DataType = "NMTOKEN")]
    public string jump
    {
      get { return this.jumpField; }
      set { this.jumpField = value; }
    }
  }
}