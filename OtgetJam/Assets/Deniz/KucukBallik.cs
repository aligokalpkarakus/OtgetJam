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

        bool isRunning = CanSeePlayer(5f);

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
}
