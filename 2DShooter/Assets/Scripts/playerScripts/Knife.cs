using UnityEngine;

public class Knife : MonoBehaviour
{   
    public Player player;
    public Transform knifePoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int damage = 100;
    public float attackRate = 2f;
    float attackTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= attackTimer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                attackTimer = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Play an attack animation
        player.animator.SetTrigger("KnifeAttack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(knifePoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (knifePoint == null)
            return;

        Gizmos.DrawWireSphere(knifePoint.position, attackRange);
    }
}
