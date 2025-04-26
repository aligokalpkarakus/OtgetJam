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
        isChasing = CanSeePlayer(5f);

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
        int maxTries = 10;
        float offset = 5f; // Sprite'�n yar��ap� kadar kayd�rma (g�venlik pay�)

        for (int i = 0; i < maxTries; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * viewRadius;
            Vector2 potentialTarget = (Vector2)transform.position + randomOffset;

            if (IsPointSafe(potentialTarget, offset))
            {
                patrolTarget = potentialTarget;
                return;
            }
        }

        patrolTarget = transform.position;
    }

    private bool IsPointSafe(Vector2 center, float offset)
    {
        Vector2[] offsets = new Vector2[]
        {
        Vector2.zero,
        new Vector2(offset, 0),
        new Vector2(-offset, 0),
        new Vector2(0, offset),
        new Vector2(0, -offset)
        };

        foreach (Vector2 off in offsets)
        {
            RaycastHit2D hit = Physics2D.Raycast(center + off, Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Water"))
            {
                return false; // E�er bir tanesi bile Water de�ilse bu pozisyon g�venli de�il
            }
        }

        return true; // Hepsi Water -> g�venli
    }


    private bool CanSeePlayer(float range)
    {
        // �evremizdeki t�m "Player" tagli objeleri bul
        Vector2 bigFishPos = base.rb.position;
        float dist = Vector2.Distance(bigFishPos, MainCharacter.currentMainCharacterPosition);

        if (dist <= range)
            return true;
        else
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
