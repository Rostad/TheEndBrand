using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable, ITargetable
{

    public string moveListPath;
    public string moveMetaListPath;
    public Spell[] spells;

    private Controller3D _Controller;
    private InputBuffer _Buffer;
    private IActionState _ActionState;
    private DummyAnimationController _DummyAnim;
    private Movelist _MoveList;
    private Stats _Stats;
    private Health _Health;
    private Mana _Mana;
    private Hitbox _Weapon;
    private List<StatusEffect> _StatusEffects;
    private ParticleDriver particleDriver;

    // Start is called before the first frame update
    void Start()
    {
        _DummyAnim.DisableRootMotion();
        _StatusEffects = new List<StatusEffect>();
        particleDriver = GetComponent<ParticleDriver>();
    }

    // Update is called once per frame
    void Update()
    {

        AddInput();
        _ActionState.Update();
        TryAttack();
        TrySpell();
        UpdateStatusEffects();
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

    private void TrySpell()
    {
        if (!(_ActionState is NormalState))
            return;

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

    private void UpdateStatusEffects()
    {
        foreach (StatusEffect e in _StatusEffects)
        {
            e.Update();
        }
    }

    IEnumerator ChangeToAttackState(Attack a)
    {
        yield return new WaitForEndOfFrame();
        SwitchActionState(new AttackState(this, a, _DummyAnim.GetAnimationClipLength(a.name)));
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

    public void DisableRootMotion()
    {
        _DummyAnim.DisableRootMotion();
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

    public bool ApplyStatus(StatusEffect effect)
    {
        effect.OnApply();
        _StatusEffects.Add(effect);
        return true;
    }

    public float DoDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public float DoHeal(int amount)
    {
        throw new System.NotImplementedException();
    }

    public float DoManaDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public float DoManaRestore(int amount)
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
