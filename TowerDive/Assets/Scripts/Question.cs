using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class Question {

    [XmlElement("ID")]
    public string ID = "";
    [XmlElement("QuestionText")]
    public string QuestionText = "";
    [XmlElement("Answer")]
    public List<string> Answers = new List<string>();
    [XmlElement("CorrectIndex")]
    public int CorrectIndex = 0;
    [XmlElement("Explanation")]
    public string Explanation = "";
    [XmlElement("ADCLevel")]
    public int ADCLevel = -1;
    [XmlElement("SupportLevel")]
    public int SupportLevel = -1;
    [XmlElement("MidLevel")]
    public int MidLevel = -1;
    [XmlElement("TopLevel")]
    public int TopLevel = -1;
    [XmlElement("JungleLevel")]
    public int JungleLevel = -1;
    [XmlElement("AwarenessLevel")]
    public int AwarenessLevel = -1;
    [XmlElement("CounterLevel")]
    public int CounterLevel = -1;
    [XmlElement("Version")]
    public string Version = "";

    public Question() { }
    public Question(int wrongAnswers)
    {
        for (int i = 0; i < wrongAnswers; i++)
        {
            Answers.Add("");
        }
    }
    public Question(string text, string category)
    {
        QuestionText = text;
    }
    public string ToXml()
    {
        XmlSerializer xmls = new XmlSerializer(typeof(Question));
        StringWriter writer = new StringWriter();
        xmls.Serialize(writer, this);
        writer.Close();
        return writer.ToString();       
    }
}
