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
  public partial class options : gametask
  {
    private option[] optionField;

    /// <remarks/>
    [XmlElement("option")]
    public option[] option
    {
      get { return this.optionField; }
      set { this.optionField = value; }
    }
  }
}