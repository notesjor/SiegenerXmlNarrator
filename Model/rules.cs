namespace SiegenerXmlNarrator.Model
{
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class rules
  {

    private rule[] ruleField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("rule")]
    public rule[] rule
    {
      get { return this.ruleField; }
      set { this.ruleField = value; }
    }
  }
}