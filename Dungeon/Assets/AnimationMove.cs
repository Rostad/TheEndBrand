using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMove : MonoBehaviour
{

    private CharacterController _Controller;
    private Animator _Anim;

    // Start is called before the first frame update
    void Start()
    {
        _Controller = GetComponentInParent<CharacterController>();
        _Anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        /*if(!_Anim.GetCurrentAnimatorStateInfo(0).IsName("Forward roll"))
            _Controller.Move(_Anim.deltaPosition);*/
    }
}
