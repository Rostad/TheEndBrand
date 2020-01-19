using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string moveListPath;
    public string moveMetaListPath;
    public ComboAttackGenerator attackGenerator;


    private Controller3D _Controller;
    private InputBuffer _Buffer;
    private IActionState _ActionState;
    private DummyAnimationController _DummyAnim;
    private Movelist _MoveList;
    private Health _Health;
    private Hitbox _Weapon;
    private ParticleDriver particleDriver;
    private float _TimeOfTriedAttack = -99;
    private float _AttackBufferThreshhold = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        _DummyAnim.DisableRootMotion();
        particleDriver = GetComponent<ParticleDriver>();
    }

    // Update is called once per frame
    void Update()
    {

        AddInput();
        _ActionState.Update();
        TryAttack();
        //TryComboAttack();
        CheckControllerStatus();
    }

    private void TryAttack()
    {
        if (_ActionState is AttackState)
        {
            if (!_Buffer.IsEmpty())
            {
                var attackstate = (AttackState)_ActionState;
                attackstate.TrySetFollowup(_Buffer.Dequeue());
            }
        }
        else if (_ActionState is NormalState && _Controller.CanAttack)
        {
            if (!_Buffer.IsEmpty())
            {
                AttackInput input = _Buffer.Dequeue();
                Attack attack = _MoveList.GetAttack(input);
                DoAttack(attack);
            }

        }
    }

    private void TryComboAttack()
    {
        if((_ActionState is AttackState || _ActionState is NormalState) && _Controller.CanAttack)
        {
            if (Input.GetMouseButtonDown(0) || Time.time <= _TimeOfTriedAttack + _AttackBufferThreshhold)
            {
                var cattack = attackGenerator.GetAttack();
                DoAttack(cattack);
            }
        } else if (Input.GetMouseButtonDown(0))
        {
            _TimeOfTriedAttack = Time.time;
        }
    }

    private void CheckControllerStatus()
    {
        if(_ActionState is AttackState && !_Controller.CanAttack)
        {
            SwitchActionState(new NormalState());
        }
    }

    private void DoAttack(Attack attack)
    {
        _DummyAnim.PlayAttack(attack.name);
        StartCoroutine(ChangeToAttackState(attack));
    }

    private void DoAttack(ComboAttack attack)
    {
        _DummyAnim.PlayAttack(attack.clip);
        StartCoroutine(ChangeToAttackState(attack));
    }

    public void DoFollowUp(string name)
    {
        var attackstate = (AttackState)_ActionState;
        var nextattack = attackstate.GetNextAttack();
        if (nextattack != null)
        {
            if (nextattack.name == name)
            {
                DoAttack(nextattack);
            }
        }
    }

    IEnumerator ChangeToAttackState(Attack a)
    {
        yield return new WaitForEndOfFrame();
        SwitchActionState(new AttackState(this, a, _DummyAnim.GetAnimationClipLength(a.name)));
        _DummyAnim.EnableRootMotion();
    }

    IEnumerator ChangeToAttackState(ComboAttack a)
    {
        yield return new WaitForEndOfFrame();
        SwitchActionState(new AttackState(this, a.damage, _DummyAnim.GetAnimationClipLength(a.clip)));
        _DummyAnim.EnableRootMotion();
    }


    private void Awake()
    {
        CacheComponents();
        SetInitialActionState();
    }

    private void CacheComponents()
    {
        _Controller = GetComponent<Controller3D>();
        _Buffer = new InputBuffer();
        _Buffer.Initialize();
        _DummyAnim = GetComponentInChildren<DummyAnimationController>();
        _MoveList = Movelist.BuildMoveList(moveListPath, moveMetaListPath);
        _Weapon = GetComponentInChildren<Hitbox>();
    }

    public void SwitchActionState(IActionState state)
    {
        _ActionState.Exit();
        _ActionState = state;
        _ActionState.Enter();
    }

    public void SetInitialActionState()
    {
        _ActionState = new NormalState();
    }

    private void AddInput()
    {
        var v = new Vector3(Input.GetAxisRaw("RightStickX"), 0.0f, Input.GetAxisRaw("RightStickY"));
        _Buffer.TryAdd(v);
    }

    public void ClearBuffer()
    {
        _Buffer.Clear();
    }

    public void CanMove(bool canmove)
    {
        _Controller.canMove = canmove;
    }

    public void DisableWeaponHitbox()
    {
        //_Weapon.DisableHitbox();
    }

    public void DisableRootMotion()
    {
        _DummyAnim.DisableRootMotion();
    }

    public void SetWeaponAttack(Attack attack)
    {
        //_Weapon.SetAttackData(attack.damage, attack.attackEffect, attack.counterEffect);
    }

    public void SetWeaponAttack(int damage)
    {
        _Weapon.SetAttackData(damage);
    }

    public void OnHit(HitData hitData)
    {
        Debug.Log(hitData.targetHit + " " + hitData.damage);
    }

    public void DoDamage(int amount)
    {
        Debug.Log("Something did " + amount + " damage");
    }

    public void DoHeal(int amount)
    {
        Debug.Log("Something restored " + amount + " health");
    }

    public void DoManaDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void DoManaRestore(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void PlayParticles(string name)
    {
        particleDriver.PlayParticles(name);
    }

    public void StopParticles(string name)
    {
        particleDriver.StopParticles(name);
    }
}
