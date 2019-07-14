using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class AttackMetaInformation {

    [XmlAttribute("name")]
    public string name;

    [XmlElement("FollowupAttack")]
    public List<string> followupAttacks = new List<string>();

    public List<string> getFollowUpAttacks()
    {
        return followupAttacks;
    }

}
