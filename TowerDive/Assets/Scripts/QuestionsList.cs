using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
[XmlRoot("Questions")]
public class QuestionsList {
    [XmlElement("Question")]
    public List<Question> Questions = new List<Question>();

    public QuestionsList() { }

}
