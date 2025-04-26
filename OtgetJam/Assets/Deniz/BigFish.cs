using UnityEngine;

public class BigFish : Entity
{
    [Header("Big Fish Settings")]
    [SerializeField] private int speed = 5;
    [SerializeField] private float patrolWaitTime = 2f; // Rastgele hedef seçtikten sonra bekleme süresi
    [SerializeField] private float viewRadius = 5f; // Görüþ yarýçapý
    [SerializeField] private float viewAngle = 90f; // Görüþ açýsý (derece)

    private Vector2 patrolTarget;
    private bool isWaiting = false;
    private bool isChasing = false;

    protected override void Start()
    {
        base.Start();
        PickNewPatrolTarget();
    }

    protected override void Update()
    {
        base.Update();
        base.updateGravity();

        // Oyuncuyu görüyor mu?
        isChasing = CanSeePlayer();

        if (isChasing)
        {
            // Saldýr modunda: oyuncuya git
            base.ApplyForceToTarget(MainCharacter.currentMainCharacterPosition, speed * Time.deltaTime);
        }
        else
        {
            // Devriye modu: rastgele seçtiði noktaya git
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!isWaiting)
        {
            base.ApplyForceToTarget(patrolTarget, speed * Time.deltaTime);

            if (Vector2.Distance(rb.position, patrolTarget) < 0.5f)
            {
                StartCoroutine(WaitAndPickNewTarget());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndPickNewTarget()
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);
        PickNewPatrolTarget();
        isWaiting = false;
    }

    private void PickNewPatrolTarget()
    {
        // Görüþ alaný içinde rastgele bir yön ve mesafe seç
        float randomAngle = Random.Range(-viewAngle / 2f, viewAngle / 2f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * transform.right;

        patrolTarget = (Vector2)transform.position + direction.normalized * Random.Range(viewRadius * 0.3f, viewRadius);
    }

    private bool CanSeePlayer()
    {
        Vector2 directionToPlayer = MainCharacter.currentMainCharacterPosition - rb.position;
        if (directionToPlayer.magnitude > viewRadius) return false;

        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer.normalized);
        if (angleToPlayer < viewAngle / 2f)
        {
            // Burada raycast ile araya bir obje girdi mi kontrol edebilirsin istersen
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        // Görüþ alanýný editörde göstermek için
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        // Þu anki patrol hedefini de çizelim
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(patrolTarget, 0.2f);
    }
}
