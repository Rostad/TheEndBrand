using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IActionState
{

    private Player _Player;
    private Attack _CurrentAttack;
    private Attack _NextAttack;
    private int _Damage;
    private float _Duration;
    private float _Timer;
    private float _DurationFraction = 0.8f;
    private List<GameObject> targetsHit;


    public void Enter()
    {
        _Player.SetCurrentAttack(_CurrentAttack);
        _Player.CanMove(false);
        //_Player.SetWeaponAttack(_CurrentAttack);
        //_Player.SetWeaponAttack(_Damage);
    }

    public void Exit()
    {
        _Player.DisableWeaponHitbox();
        if (_NextAttack == null)
        {
            _Player.ClearBuffer();
        }
        _Player.CanMove(true);
        _Player.DisableRootMotion();
    }

    public void Update()
    {
        if (_Timer >= _Duration)
        {
            _Player.SwitchActionState(new NormalState());
        }

        _Timer += Time.deltaTime;
        
    }

    public void PerformCast(GameObject origin)
    {
        Debug.Log("Casted a hitbox from " + origin.name);
        RaycastHit hit;
        //Physics.BoxCast(origin.transform.position, new Vector3(1,1,1), origin.transform.forward, out hit, origin.transform.localRotation, 2.5f);
        Collider[] collisions = Physics.OverlapBox(origin.transform.position, new Vector3(0.5f, 0.5f, 0.5f), origin.transform.localRotation);
        foreach(Collider c in collisions)
        {
            Debug.Log(c.name);
        }
      
    }

    public AttackState(Player p, Attack a, float time)
    {
        _Player = p;
        _CurrentAttack = a;
        _Duration = time * _DurationFraction;
        _Timer = 0f;
        targetsHit = new List<GameObject>();
    }

    public AttackState(Player p, int damage, float time)
    {
        _Player = p;
        _Duration = time * _DurationFraction;
        _Timer = 0f;
        _Damage = damage;
    }

    public void TrySetFollowup(AttackInput input)
    {
        _NextAttack = _CurrentAttack.GetFollowup(input);
    }

    public Attack GetNextAttack()
    {
        if (_NextAttack != null)
            return _NextAttack;

        return null;
    }
}
