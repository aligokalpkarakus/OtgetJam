using UnityEngine;

public class SwarmFish : Entity
{
    [SerializeField] private float moveForce = 3f;
    [SerializeField] private float dashForce = 10f;

    private Transform target;
    private bool isPreparingToDash = false;
    private bool isDashing = false;

    private float preparationTime = 0.5f; // Ünlem gösterme süresi
    private float dashStartTime;

    [SerializeField] private GameObject exclamationMarkPrefab;
    private GameObject exclamationInstance;

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();
        base.updateGravity();

        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        if (isPreparingToDash)
        {
            if (Time.time >= dashStartTime)
            {
                Dash(direction);
            }
        }
        else if (isDashing)
        {
            // Zaten impulse verildi, normal yüzmeye devam eder
        }
        else
        {
            moveVectorized(direction, moveForce * Time.deltaTime);
        }
    }

    public void PrepareToDash()
    {
        if (isPreparingToDash || isDashing) return;

        isPreparingToDash = true;
        dashStartTime = Time.time + preparationTime;

        if (exclamationMarkPrefab != null)
        {
            exclamationInstance = Instantiate(exclamationMarkPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
        }
    }

    private void Dash(Vector2 direction)
    {
        isPreparingToDash = false;
        isDashing = true;

        moveImpulse(direction, dashForce);

        if (exclamationInstance != null)
        {
            Destroy(exclamationInstance);
        }

        Invoke(nameof(EndDash), 0.5f); // 0.5 saniye sonra Dash modu bitsin
    }

    private void EndDash()
    {
        isDashing = false;
    }

}
