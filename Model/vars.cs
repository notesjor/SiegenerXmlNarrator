namespace SiegenerXmlNarrator.Model
{
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class vars
  {

    private variable[] varField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("var")]
    public variable[] var
    {
      get { return this.varField; }
      set { this.varField = value; }
    }
  }
}