using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    private int _MaxHealth;
    private int _CurrentHealth;
    private int _StartingHealth = 300;
    private Player _Player;
    public Text healthDisplay;
   

    // Start is called before the first frame update
    void Start()
    {
        _Player = GetComponent<Player>();
        _MaxHealth = 200;
        _CurrentHealth = _MaxHealth;
        UpdateHealthDisplay();
    }


    public void LoadHealth(int health)
    {
        _CurrentHealth = health;
        UpdateHealthDisplay();
    }

    public void Respawn()
    {
        _CurrentHealth = _MaxHealth;
        UpdateHealthDisplay();
    }


    public void DoDamage(int damage)
    {
        _CurrentHealth -= damage;
        if(_CurrentHealth < 0)
        {
            _CurrentHealth = 0;
        }
        UpdateHealthDisplay();
    }

    public void HealDamage(int heal)
    {
        _CurrentHealth += heal;
        if(_CurrentHealth > _MaxHealth)
        {
            _CurrentHealth = _MaxHealth;
        }
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        //healthDisplay.text = _CurrentHealth + " / " + _MaxHealth;
    }
    
}
