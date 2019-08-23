using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public AudioClip audioClip;
    public float coolDown; 
    public string tooltip;
    public int manaCost;
    public int basePower;
    public float scaling;
    public Effect effect;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();

}
