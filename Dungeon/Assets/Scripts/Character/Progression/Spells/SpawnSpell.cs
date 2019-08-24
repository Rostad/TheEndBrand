using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/SpawnSpell")]
public class SpawnSpell : Spell
{
    public GameObject spellPrefab;

    public override void Initialize(GameObject obj)
    {
        source = obj;
        effect.power = basePower;
    }

    public override void TriggerAbility()
    {
        Debug.Log("Trigger");
        var spellobj = Instantiate(spellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        spellobj.GetComponent<ISpawnableSpell>().SetUp(source, effect);
    }
}
