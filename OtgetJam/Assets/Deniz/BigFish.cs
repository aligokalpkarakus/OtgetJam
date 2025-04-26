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
        int maxTries = 10; // Çok nadiren hata olmasýn diye sýnýr koyuyoruz
        for (int i = 0; i < maxTries; i++)
        {
            // Rastgele bir nokta seç (viewRadius yarýçapýnda)
            Vector2 randomOffset = Random.insideUnitCircle * viewRadius;
            Vector2 potentialTarget = (Vector2)transform.position + randomOffset;

            // Bu noktada Water layer var mý kontrol et
            RaycastHit2D hit = Physics2D.Raycast(potentialTarget, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                patrolTarget = potentialTarget;
                return;
            }
        }

        // Eðer maxTries kadar denedi ve bulamadýysa: Þu anki pozisyonu hedefle
        patrolTarget = transform.position;
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
