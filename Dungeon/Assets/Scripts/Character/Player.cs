using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITargetable
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
    private KeyCode[] abilityKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

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
        Spell s = GetSpell();
        if (s == null)
            return;
        CastSpell(s);



    }

    private Spell GetSpell()
    {
        for(int i = 0; i < abilityKeys.Length; i++)
        {
            if (Input.GetKeyDown(abilityKeys[i]))
            {
                if (!(spells[i] == null))
                    return spells[i];
                
            }
                
        }


        return null;
    }

    private void CastSpell(Spell s)
    {
        SwitchActionState(new CastState(this, s));
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
