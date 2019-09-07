using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : IActionState
{
    private bool hasCast;
    private Player _Player;
    private Spell _CurrentSpell;
    private float duration = 1f;
    private float time = 0f;

    public void Enter()
    {
        _Player.GetComponentInChildren<DummyAnimationController>().PlayCast();
        _Player.CanMove(false);
        _Player.PlayParticles(_CurrentSpell.channelParticles);
    }

    public void Exit()
    {
        _Player.CanMove(true);
        _Player.StopParticles(_CurrentSpell.channelParticles);
        _Player.PlayParticles(_CurrentSpell.castParticles);
    }

    public CastState(Player player, Spell spell)
    {
        hasCast = false;
        _Player = player;
        _CurrentSpell = spell;
        _CurrentSpell.Initialize(_Player.gameObject);
    }

    void IActionState.Update()
    {
        if(time >= 0.8f && !hasCast)
        {
            _CurrentSpell.TriggerAbility();
            hasCast = true;
        }
        if (time >= duration)
        {
            _Player.SwitchActionState(new NormalState());
        }

        time += Time.deltaTime;
    }
}
