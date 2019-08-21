using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
