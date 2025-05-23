using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer; // Bal�k sprite'�
    [SerializeField] private float maxRotationAngle = 75f; // Yukar�/a�a�� maksimum d�n�� derecesi

    public  Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();//S�rt�nme ayarlan�r
    }

    protected virtual void Update()
    {
        RotateToVelocity();
    }

    
    public virtual void moveVectorized(Vector2 dir,float force)
    {
        rb.AddForce(dir.normalized * force);
    }

    public virtual void moveImpulse(Vector2 dir, float force)
    {
        rb.AddForce(dir.normalized * force,ForceMode2D.Impulse);
    }

    public virtual void setGravity(float g)
    {
        rb.gravityScale = g;
    }

    public virtual void setDrag(float d)
    {
        rb.linearDamping = d;
    }

    public virtual string checkCurrentLayer()
    {
        int mask = LayerMask.GetMask("Water", "Air","Ruzgar");
        // rb.position'dan direkt a�a��ya do�ru �ok k�sa bir ray at�yoruz
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 0.1f, mask);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            return LayerMask.LayerToName(hit.collider.gameObject.layer);
        }
        else
        {
            return "Air"; // Hi�bir �eye �arpmad�ysa havaday�z
        }
    }



    public virtual void ApplyForceToTarget(Vector2 targetPosition, float forceAmount)
    {

        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.AddForce(direction * forceAmount);
    }

    private string previousLayer = "";

    public virtual void updateGravity()
    {
        string layerName = checkCurrentLayer();

        if (layerName != previousLayer)
        {
            previousLayer = layerName;

            if (layerName == "Water")
            {
                
                setGravity(0);
                setDrag(2);
            }
            else if (layerName == "Air" )
            {
                
                setGravity(1);
                setDrag(1);
            }
            else if (layerName == "Ruzgar")
            {
                
                setGravity(0.5f);
                setDrag(1);
            }
        }
    }


    public virtual void RotateToVelocity()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(velocity.y, Mathf.Abs(velocity.x)) * Mathf.Rad2Deg;
        float clampedAngle = Mathf.Clamp(angle, -maxRotationAngle, maxRotationAngle);

        // Y�n� belirle ve simetri uygula
        if (velocity.x >= 0)
        {
            spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0f, 0f, clampedAngle);
        }
        else
        {
            spriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0f, 0f, -clampedAngle);
        }
    }

}
