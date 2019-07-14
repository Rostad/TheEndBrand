using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Attack {

    [XmlAttribute("name")]
    public string name;

    [XmlElement("Button")]
    public AttackButton attackButton;

    [XmlElement("Input")]
    public InputDirection inputDir;

    [XmlElement("Damage")]
    public int damage;

    [XmlElement("State")]
    public string state;

    private List<Attack> followUpAttacks;
    //Additional enums with on hit effect knockdown, crumple, knockup, knockaway

    public void addFollowUpAttack(Attack a)
    {
        if (followUpAttacks == null)
        {
            followUpAttacks = new List<Attack>();
            followUpAttacks.Add(a);
        }
        else
        {
            followUpAttacks.Add(a);
        }
    }

    public AttackInput GetInput()
    {
        return new AttackInput(inputDir, attackButton);
    }

    public Attack GetFollowup(AttackInput input)
    {
        if (followUpAttacks == null)
            return null;
        if (followUpAttacks.Count == 0)
            return null;

        foreach(Attack a in followUpAttacks)
        {
            var followupinput = a.GetInput();
            if(followUpAttacks.Count == 1)
            {
                if(followupinput.Button == input.Button)
                {
                    return followUpAttacks[0];
                }
            }
            if(followupinput.Button == input.Button && followupinput.Motion == input.Motion)
            {
                return a;
            }
        }
        return null;
    }


}
