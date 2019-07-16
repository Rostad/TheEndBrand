using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpDriver : MonoBehaviour
{

    private Player _Player;

    // Start is called before the first frame update
    void Start()
    {
        _Player = GetComponentInParent<Player>();
    }


    //Called through animation events in attack animations, tells the player object which attack to do next.
    public void DoFollowUp(string name)
    {
        _Player.DoFollowUp(name);
    }
}
