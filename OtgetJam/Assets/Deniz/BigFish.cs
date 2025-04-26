using UnityEngine;

public class BigFish : Entity
{
    [Header("Big Fish Settings")]
    [SerializeField] private int speed = 5;
    [SerializeField] private Transform[] patrolPoints; // Devriye noktalar�
    [SerializeField] private float patrolWaitTime = 2f; // Noktada bekleme s�resi
    [SerializeField] private float viewRadius = 5f; // G�r�� yar��ap�
    [SerializeField] private float viewAngle = 90f; // G�r�� a��s� (derece)

    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private bool isChasing = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        base.updateGravity();

        // Oyuncuyu g�r�yor mu kontrol et
        isChasing = CanSeePlayer();

        if (isChasing)
        {
            // G�r�yorsa -> Oyuncuya sald�r
            base.ApplyForceToTarget(MainCharacter.currentMainCharacterPosition, speed * Time.deltaTime);
        }
        else
        {
            // G�rm�yorsa -> Devriye at
            Patrol();
        }
    }


    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!isWaiting)
        {
            Vector2 targetPos = patrolPoints[currentPatrolIndex].position;
            base.ApplyForceToTarget(targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(rb.position, targetPos) < 0.5f)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        isWaiting = false;
    }

    private bool CanSeePlayer()
    {
        Vector2 directionToPlayer = MainCharacter.currentMainCharacterPosition - rb.position;
        if (directionToPlayer.magnitude > viewRadius) return false;

        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer.normalized);
        if (angleToPlayer < viewAngle / 2f)
        {
            // Burada istersen raycast at�p arada duvar vs var m� kontrol edebiliriz.
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        // Edit�rde g�r�� alan�n� g�rmek i�in
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }
}
