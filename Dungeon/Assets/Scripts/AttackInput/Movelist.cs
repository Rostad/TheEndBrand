using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("MoveCollection")]
public class Movelist{

    [XmlArray("Moves")]
    [XmlArrayItem("Move")]
    public List<Attack> attacks = new List<Attack>();

    protected Dictionary<string, Dictionary<AttackInput, Attack>> moveLists = new Dictionary<string, Dictionary<AttackInput, Attack>>();

    private List<Attack> followUpAttacks = new List<Attack>();
    private bool stateSpecificMoves;

    public Attack GetAttack(AttackInput input, string state)
    {
        return moveLists[state][input];
    }

    public Attack GetAttack(AttackInput input)
    {
        moveLists["Default"].TryGetValue(input, out Attack a);
        if(a == null)
        {
            input.Motion = InputDirection.Neutral;
            moveLists["Default"].TryGetValue(input, out a);
            return a;
        }

        return a;
        
    }

    public static Movelist BuildMoveList(string movepath, string metapath)
    {
        Movelist movelist = BuildMoveList(movepath);
        MoveMetaList metalist = MoveMetaList.BuildMetaList(metapath);
        movelist.StructureAttacksList(metalist);
        movelist.TrimMoveList(metalist);
        movelist.FillDictionaries(metalist);
        return movelist;
    }

    private static Movelist BuildMoveList(string movepath)
    {
        TextAsset xmlMoves = Resources.Load<TextAsset>(movepath);

        XmlSerializer serializer = new XmlSerializer(typeof(Movelist));

        StringReader reader = new StringReader(xmlMoves.text);

        Movelist moves = serializer.Deserialize(reader) as Movelist;

        reader.Close();
        return moves;
    }

    private void StructureAttacksList(MoveMetaList metaList)
    {
        foreach(Attack attack in attacks)
        {
            var info = metaList.getMetaInformation(attack.name);
            if(info == null)
            {
                throw new System.Exception("Meta information about move " + attack.name + " does not exist in the currently used meta information file");
            }
            if (info.followupAttacks.Count != 0)
            {
                foreach (Attack followup in attacks)
                {
                    if (info.followupAttacks.Contains(followup.name))
                    {
                        attack.addFollowUpAttack(followup);
                        followUpAttacks.Add(followup);
                    }
                }
            }

        }
    }

    private void TrimMoveList(MoveMetaList metaList)
    {
        foreach(Attack a in followUpAttacks)
        {
            attacks.Remove(a);
        }
    }

    private void FillDictionaries(MoveMetaList metaList)
    {
        if(attacks[0].state == null)
        {
            stateSpecificMoves = false;
            FillNonStateSpecificDictionary(metaList);
        }
        else
        {
            stateSpecificMoves = true;
            FillStateSpecificDictionaries(metaList);
        }
    }

    private void FillStateSpecificDictionaries(MoveMetaList metaList)
    {
        foreach(Attack a in attacks)
        {
            if (!moveLists.ContainsKey(a.state))
            {
                var d = new Dictionary<AttackInput, Attack>();
                moveLists.Add(a.state, d);
                d.Add(a.GetInput(), a);
            } else
            {
                moveLists.TryGetValue(a.state, out Dictionary<AttackInput, Attack> d);
                d.Add(a.GetInput(), a);
            }
        }
    }

    private void FillNonStateSpecificDictionary(MoveMetaList metaList)
    {
        var d = new Dictionary<AttackInput, Attack>();
        moveLists.Add("Default", d);

        foreach(Attack a in attacks)
        {
            d.Add(a.GetInput(), a);
        }
    }

    public void Initialize()
    {

    }
}
