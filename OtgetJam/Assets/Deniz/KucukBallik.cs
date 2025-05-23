using UnityEngine;

public class KucukBallik : Entity
{
    [SerializeField] private int speed = 5;
    [SerializeField] private float patrolWaitTime = 2f; // Rastgele hedef se�tikten sonra bekleme s�resi
    [SerializeField] private float viewRadius = 5f; // G�r�� yar��ap�
    [SerializeField] private float viewAngle = 90f; // G�r�� a��s� (derece)

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

        bool isRunning = CanSeePlayer(3f);

        if (isRunning)
        {
            // Sald�r modunda: oyuncuya git
            Vector2 v2 = new Vector2(base.rb.position.x - MainCharacter.currentMainCharacterPosition.x, base.rb.position.y - MainCharacter.currentMainCharacterPosition.y);
            base.ApplyForceToTarget(v2, speed * Time.deltaTime);
        }
        else
        {
            // Devriye modu: rastgele se�ti�i noktaya git
            Patrol();
        }
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

    private void Patrol()
    {
        if (!isWaiting)
        {
            base.ApplyForceToTarget(patrolTarget, speed * Time.deltaTime);

            if (Vector2.Distance(rb.position, patrolTarget) < 1f)
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

        Debug.LogWarning("Water alan� bulunamad�!");
        return new Bounds(Vector3.zero, Vector3.one * 10f); // fallback
    }



    private void PickNewPatrolTarget()
    {
        int maxTries = 10;
        float safeMargin = 0.5f; // Su kenar�ndan biraz uzak dur

        for (int i = 0; i < maxTries; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * viewRadius;
            Vector2 potentialTarget = (Vector2)transform.position + randomOffset;

            if (IsPointSafe(potentialTarget, safeMargin))
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

}
