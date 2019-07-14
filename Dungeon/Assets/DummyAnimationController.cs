using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimationController : MonoBehaviour
{
    private Velocity3D _Velocity;
    private Animator _Animator;
    private bool _wasRunning;
    private bool _isRolling;
    private GameObject _Player;
    private AnimationClip[] _Clips;
    
    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        _wasRunning = false;
        _isRolling = false;
        _Player = GameObject.Find("Player");
        _Clips = _Animator.runtimeAnimatorController.animationClips;
        
    }

    public void ShareVelocity(Velocity3D vel)
    {
        _Velocity = vel;
    }

    // Update is called once per frame
    void Update()
    {
        var v = _Velocity.GetVelocity();
        _Animator.SetFloat("Velocity X", v.x);
        _Animator.SetFloat("Velocity Z", v.z);
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && _Animator.GetFloat("Velocity Z") > 10f)
        {
            RunningRotate();
            _wasRunning = true;
        } else if (_wasRunning && !_isRolling)
        {
            ResetRotation();
        }
    }

    public void PlayRoll()
    {
        _isRolling = true;
        Vector3 velocity = _Velocity.GetVelocity();
        velocity.y = 0f;
        velocity = transform.TransformVector(velocity);
        Vector3 dir = Vector3.RotateTowards(this.transform.forward, velocity, 360f, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
        //_Animator.SetTrigger("Roll trigger");
        _Animator.CrossFade("Forward Roll", 0.3f);
    }

    public void PlayAttack(string name)
    {
        //_Animator.Play(name, 0);
        _Animator.CrossFadeInFixedTime(name, 0.2f);
    }

    public float GetAnimationLength()
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public float GetAnimationClipLength(string name)
    {
        foreach(AnimationClip c in _Clips)
        {
            if (c.name.Equals(name))
                return c.length;
        }

        return 0f;
    }

    public bool GetAnimationName()
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }

    public void RunningRotate()
    {
        Vector3 velocity = _Velocity.GetVelocity();
        velocity.y = 0f;
        velocity = _Player.transform.TransformVector(velocity);
        Vector3 dir = Vector3.RotateTowards(transform.forward, velocity, 360f, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    

    public void ResetRotation()
    {
        Vector3 dir = Vector3.RotateTowards(transform.forward, transform.parent.transform.forward, 360f, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
        _wasRunning = false;
        _isRolling = false;
    }
}
