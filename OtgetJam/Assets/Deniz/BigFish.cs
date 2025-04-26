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
        isChasing = CanSeePlayer(5f);

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
        int maxTries = 10;
        float offset = 5f; // Sprite'ýn yarýçapý kadar kaydýrma (güvenlik payý)

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
                return false; // Eðer bir tanesi bile Water deðilse bu pozisyon güvenli deðil
            }
        }

        return true; // Hepsi Water -> güvenli
    }


    private bool CanSeePlayer(float range)
    {
        // Çevremizdeki tüm "Player" tagli objeleri bul
        Vector2 bigFishPos = base.rb.position;
        float dist = Vector2.Distance(bigFishPos, MainCharacter.currentMainCharacterPosition);

        if (dist <= range)
            return true;
        else
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
