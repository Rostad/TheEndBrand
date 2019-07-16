using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodlust : MonoBehaviour, IResource
{

    private int _CurrentBloodlust;
    private int _MaxBloodlust;
    private int _BloodlustDecay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UseBloodlust(int cost)
    {
        if(cost < _CurrentBloodlust)
        {
            _CurrentBloodlust -= cost;
            return true;
        }

        return false;
    }

    public void AddBloodLust(int damage)
    {
        _CurrentBloodlust += CalculateBloodlustGain(damage);
        if(_CurrentBloodlust > _MaxBloodlust)
        {
            _CurrentBloodlust = _MaxBloodlust;
        }
    }

    private int CalculateBloodlustGain(int damage)
    {
        return damage / 10;
    }

    public bool UseResource(int amount)
    {
        return UseBloodlust(amount);
    }

    public void AddResource(int amount)
    {
        AddBloodLust(amount);
    }
}
