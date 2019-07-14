using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public enum InputDirection {


    [XmlEnum(Name = "Forward")]
    Forward,
    [XmlEnum(Name = "Left")]
    Left,
    [XmlEnum(Name = "Right")]
    Right,
    [XmlEnum(Name = "Back")]
    Back,
    [XmlEnum(Name = "Neutral")]
    Neutral,
    [XmlEnum(Name = "None")]
    NO_VALUE
	
}
