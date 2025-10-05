using UnityEngine;

public class Tank : EnemyBehaviour
{
    protected override void Start()
    {
        base.Start();

        m_distanceFactor = 1.0f;
    }

    protected override void Attack()
    {
        base.Attack();

        if (Target.Rb != null 
            && Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest() * 2.0f)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            direction.y = 0.75f;
            direction.Normalize();
            Target.Rb.AddForce(direction * 12.0f, ForceMode.Impulse);
            Target.AdaptForTankAttack();
        }
    }
}