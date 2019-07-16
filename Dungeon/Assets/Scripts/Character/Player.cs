using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{

    public string moveListPath;
    public string moveMetaListPath;

    private Controller3D _Controller;
    private InputBuffer _Buffer;
    private IActionState _ActionState;
    private DummyAnimationController _DummyAnim;
    private Movelist _MoveList;
    private Stats _Stats;
    private IResource _Resource;
    private Hitbox _Weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddInput();
        _ActionState.Update();
        TryAttack();
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

    private void DoAttack(Attack attack)
    {
        _DummyAnim.PlayAttack(attack.name);
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
        var v = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        _Buffer.TryAdd(v);
    }

    public Stats GetStats()
    {
        return _Stats;
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
        _Weapon.DisableHitbox();
    }

    public void SetWeaponAttack(Attack attack)
    {
        _Weapon.SetAttackData(attack.damage, attack.attackEffect, attack.counterEffect);
    }

    public HitData DoDamage(AttackData attack)
    {
        throw new System.NotImplementedException();
    }

    public void OnHit(HitData hitData)
    {
        Debug.Log(hitData.targetHit + " " + hitData.damage);
    }
}
