namespace SiegenerXmlNarrator.Model
{
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class ui
  {

    private string backgroundPathField;

    private string backgroundField;

    private string leftField;

    private string rightField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string BackgroundPath
    {
      get { return this.backgroundPathField; }
      set { this.backgroundPathField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string background
    {
      get { return this.backgroundField; }
      set { this.backgroundField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string left
    {
      get { return this.leftField; }
      set { this.leftField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string right
    {
      get { return this.rightField; }
      set { this.rightField = value; }
    }
  }
}