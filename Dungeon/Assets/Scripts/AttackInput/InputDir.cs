using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputDir {

    private InputDirection _Direction;
    public float Time;

    
    public InputDirection Motion
    {

        get
        {
            return _Direction;
        }
        private set { }

    }

  

    public InputDir(float Time, InputDirection Direction)
    {
        
        this.Time = Time;
        _Direction = Direction;
        
    }





}
