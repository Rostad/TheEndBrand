using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("MoveMetaCollection")]
public class MoveMetaList {


    [XmlArray("Metas")]
    [XmlArrayItem("Meta")]
    public List<AttackMetaInformation> metaInformation = new List<AttackMetaInformation>();


    public static MoveMetaList BuildMetaList(string metapath)
    {
        TextAsset XmlMeta = Resources.Load<TextAsset>(metapath);

        XmlSerializer serializer = new XmlSerializer(typeof(MoveMetaList));

        StringReader reader = new StringReader(XmlMeta.text);

        MoveMetaList moveMeta = serializer.Deserialize(reader) as MoveMetaList;

        return moveMeta;
    }

    public AttackMetaInformation getMetaInformation(string name)
    {
        foreach (AttackMetaInformation m in metaInformation)
        {
            if (name == m.name)
            {
                return m;
            }
        }

        return null;
    }
} 
