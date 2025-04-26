using UnityEngine;

public class BigFish : Entity
{
    [Header("Big Fish Settings")]
    [SerializeField] private int speed = 5;
    [SerializeField] private float patrolWaitTime = 2f; // Rastgele hedef se�tikten sonra bekleme s�resi
    [SerializeField] private float viewRadius = 5f; // G�r�� yar��ap�
    [SerializeField] private float viewAngle = 90f; // G�r�� a��s� (derece)

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

        // Oyuncuyu g�r�yor mu?
        isChasing = CanSeePlayer();

        if (isChasing)
        {
            // Sald�r modunda: oyuncuya git
            base.ApplyForceToTarget(MainCharacter.currentMainCharacterPosition, speed * Time.deltaTime);
        }
        else
        {
            // Devriye modu: rastgele se�ti�i noktaya git
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
        int maxTries = 10; // �ok nadiren hata olmas�n diye s�n�r koyuyoruz
        for (int i = 0; i < maxTries; i++)
        {
            // Rastgele bir nokta se� (viewRadius yar��ap�nda)
            Vector2 randomOffset = Random.insideUnitCircle * viewRadius;
            Vector2 potentialTarget = (Vector2)transform.position + randomOffset;

            // Bu noktada Water layer var m� kontrol et
            RaycastHit2D hit = Physics2D.Raycast(potentialTarget, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                patrolTarget = potentialTarget;
                return;
            }
        }

        // E�er maxTries kadar denedi ve bulamad�ysa: �u anki pozisyonu hedefle
        patrolTarget = transform.position;
    }


    private bool CanSeePlayer()
    {
        // �evremizdeki t�m "Player" tagli objeleri bul
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // E�er "Player" tagli bir obje bulduysa
                Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;

                // E�er viewAngle kullanmak istemiyorsan direkt return true diyebilirsin
                float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);
                if (angleToPlayer < viewAngle / 2f)
                {
                    return true;
                }
            }
        }

        return false;
    }


    private void OnDrawGizmosSelected()
    {
        // G�r�� alan�n� edit�rde g�stermek i�in
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        // �u anki patrol hedefini de �izelim
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(patrolTarget, 0.2f);
    }
}
