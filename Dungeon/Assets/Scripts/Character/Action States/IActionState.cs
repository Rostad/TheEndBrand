using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionState
{
    
    void Enter();
    void Update();
    void Exit();
}
