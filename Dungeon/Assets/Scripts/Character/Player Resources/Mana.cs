using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour, IResource
{
    private int _CurrentMana;
    private int _MaxMana;


    public bool UseMana(int cost)
    {
        if (cost < _CurrentMana)
        {
            _CurrentMana -= cost;
            return true;
        }

        return false;
    }

    public void AddMana(int amount)
    {
        _CurrentMana += amount;
        if (_CurrentMana > _MaxMana)
            _CurrentMana = _MaxMana;
    }

    public void RestoreMana()
    {
        AddMana(_MaxMana);
    }

    public void AddResource(int amount)
    {
        throw new System.NotImplementedException();
    }

    public bool UseResource(int amount)
    {
        throw new System.NotImplementedException();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
