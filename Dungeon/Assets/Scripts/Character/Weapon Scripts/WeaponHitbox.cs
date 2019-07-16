using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{

    private bool _Enabled;
    private AttackData _AttackData;
    private List<IDamagable> _TargetsHit;
    private Player _Player;


    private void Awake()
    {
        _Enabled = false;
        _TargetsHit = new List<IDamagable>();
        _Player = GetComponentInParent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackData(int damage, AttackEffect effect, AttackEffect counterEffect)
    {
        _AttackData = new AttackData(damage, effect, counterEffect);
        _Enabled = true;
    }

    public void DisableHitbox()
    {
        _Enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_Enabled)
            return;
        if (_AttackData == null)
            throw new MissingReferenceException("Weapon hit a target without any attack data");

        if (other.CompareTag("Damagable"))
        {
            IDamagable target = other.GetComponent<IDamagable>();
            if (target == null)
                throw new MissingReferenceException("Target hit is marked as damagable but lacks an object implementing IDamagable");
            if (!_TargetsHit.Contains(target)) { 
                HitData hit = target.DoDamage(_AttackData);
                _TargetsHit.Add(target);
                _Player.OnHit(hit);
            }
        }
    }
}
