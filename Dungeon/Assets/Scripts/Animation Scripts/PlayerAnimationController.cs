using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
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

    public void SetRunning(bool b)
    {
        _Animator.SetBool("Running", b);
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
            return;
        } else if (_wasRunning && !_isRolling)
        {
            //ResetRotation();
        }

        RunToGroundRotation();
        /*if (_Animator.GetBool("Running"))
        {
            RunningRotate();
        }*/
    }

    public void PlayRoll()
    {

        _isRolling = true;
        Vector3 velocity = _Velocity.GetVelocity();
        velocity.y = 0f;
        velocity = transform.TransformVector(velocity);
        Vector3 dir = Vector3.RotateTowards(transform.InverseTransformDirection(this.transform.forward), velocity, 360f, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
        //_Animator.SetTrigger("Roll trigger");
        _Animator.Play("Forward Roll", 0);
    }

    public void PlayAttack(string name)
    {
        //_Animator.Play(name, 0);
        _Animator.CrossFadeInFixedTime(name, 0.2f);
    }

    //Called in the IEnumerator ChangeToAttackState when switching to an attack state. The float returned specifies the length of the attack animation and determines how long the attack state lasts.
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
    //Called when the player is running, rotates the player in the direction he is currently moving.
    public void RunningRotate()
    {
        Vector3 velocity = _Velocity.GetVelocity();
        velocity.y = 0f;
        velocity = _Player.transform.TransformVector(velocity);
        Vector3 dir = Vector3.RotateTowards(transform.forward, velocity, 5 * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    
    //Called when the player exits the DodgeState3D or has stopped running, resets the player objects rotation to face forward.
    public void ResetRotation()
    {
        Vector3 dir = Vector3.RotateTowards(transform.InverseTransformDirection(transform.forward), transform.parent.transform.forward, 360f, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
        _wasRunning = false;
        _isRolling = false;
    }

    public void RunToGroundRotation()
    {
        if (!_isRolling)
        {
            Vector3 dir = Vector3.RotateTowards(transform.forward, transform.parent.transform.forward, 5 * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void DisableRootMotion()
    {
        _Animator.applyRootMotion = false;
    }

    public void EnableRootMotion()
    {
        _Animator.applyRootMotion = true;
    }

    public void PlayStateSwitchAnimation(string switchFrom, string switchTo)
    {
        Debug.Log("Playing exit anim for " + switchFrom + " playing enter anim for switchTo");

        if (switchFrom.Equals("RunState3D"))
            SetRunning(false);
        if (switchFrom.Equals("DodgeState3D"))
            ResetRotation();

        if (switchTo.Equals("DodgeState3D"))
            PlayRoll();
        if (switchTo.Equals("RunState3D"))
            SetRunning(true);
    }
}
