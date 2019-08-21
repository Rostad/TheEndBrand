using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public AudioClip audioClip;
    public float coolDown;
    public float baseEffect;
    public float scaling;
    public ITargeting targetSystem;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();

}
