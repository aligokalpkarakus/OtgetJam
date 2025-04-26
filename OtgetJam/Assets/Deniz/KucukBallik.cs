using UnityEngine;

public class KucukBallik : Entity
{
    [SerializeField] private int speed = 5;
    [SerializeField] private float patrolWaitTime = 2f; // Rastgele hedef seçtikten sonra bekleme süresi
    [SerializeField] private float viewRadius = 5f; // Görüþ yarýçapý
    [SerializeField] private float viewAngle = 90f; // Görüþ açýsý (derece)

    private Vector2 patrolTarget;
    private bool isWaiting = false;


    protected override void Start()
    {
        base.Start();
        PickNewPatrolTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        base.updateGravity();

        bool isRunning = CanSeePlayer(5f);

        if (isRunning)
        {
            // Saldýr modunda: oyuncuya git
            Vector2 v2 = new Vector2(base.rb.position.x - MainCharacter.currentMainCharacterPosition.x, base.rb.position.y - MainCharacter.currentMainCharacterPosition.y);
            base.ApplyForceToTarget(v2, speed * Time.deltaTime);
        }
        else
        {
            // Devriye modu: rastgele seçtiði noktaya git
            Patrol();
        }
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

    private Bounds GetWaterBounds()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Water"))
            {
                Collider2D collider = obj.GetComponent<Collider2D>();
                if (collider != null)
                    return collider.bounds;
            }
        }

        Debug.LogWarning("Water alaný bulunamadý!");
        return new Bounds(Vector3.zero, Vector3.one * 100f); // fallback
    }



    private void PickNewPatrolTarget()
    {
        int maxTries = 10;
        float safeMargin = 1f; // Kenardan uzak durma mesafesi

        Bounds waterBounds = GetWaterBounds();

        for (int i = 0; i < maxTries; i++)
        {
            float randomX = Random.Range(waterBounds.min.x + safeMargin, waterBounds.max.x - safeMargin);
            float randomY = Random.Range(waterBounds.min.y + safeMargin, waterBounds.max.y - safeMargin);
            Vector2 potentialTarget = new Vector2(randomX, randomY);

            if (IsPointSafe(potentialTarget, 0.1f)) // çok küçük offset yeter
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

}
