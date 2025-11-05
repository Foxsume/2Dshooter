using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private float AttackCooldown = 0.5f;
    [SerializeField] private float damage;
    private float lastAttackTime;

    public override void StartFire()
    {
        TryAttack();
    }

    public override void ContinueFire()
    {
        //Later for if melee has charge attack
    }

    public override void StopFire()
    {
        //Same here - if melee has charge attack
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < AttackCooldown) return;
        lastAttackTime = Time.time;
        Swing();
    }

    private void Swing()
    {
        // Play swing animation, check hit, deal damage
    }
}

