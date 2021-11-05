namespace SiegenerXmlNarrator.Model
{
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class npcs
  {

    private npc[] npcField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("npc")]
    public npc[] npc
    {
      get { return this.npcField; }
      set { this.npcField = value; }
    }
  }
}