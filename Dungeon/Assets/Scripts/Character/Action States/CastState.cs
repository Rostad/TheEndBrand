using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : IActionState
{

    private Player _Player;

    public void Enter()
    {
        _Player.CanMove(false);
    }

    public void Exit()
    {
        _Player.CanMove(true);
    }

    public CastState(Player player)
    {
        _Player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IActionState.Update()
    {
        throw new System.NotImplementedException();
    }
}
