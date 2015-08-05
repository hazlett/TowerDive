using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot]
public class User {

    [XmlAttribute]
    public string Name = "";

    [XmlAttribute]
    public string Level = "";

    [XmlAttribute]
    public string Money = "";

    [XmlElement]
    public List<string> Games = new List<string>();

    public User() { }
    public User(string name)
    {
        this.Name = name;
        this.Level = "0";
        this.Money = "0";
    }
    public string ToXml()
    {
        XmlSerializer xmls = new XmlSerializer(typeof(User));
        StringWriter writer = new StringWriter();
        xmls.Serialize(writer, this);
        writer.Close();
        return writer.ToString();
    }
}
