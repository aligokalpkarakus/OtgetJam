using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer; // Balýk sprite'ý
    [SerializeField] private float maxRotationAngle = 75f; // Yukarý/aþaðý maksimum dönüþ derecesi

    public  Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Sürtünme ayarlanýr
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
        // rb.position'dan direkt aþaðýya doðru çok kýsa bir ray atýyoruz
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 0.1f);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            return LayerMask.LayerToName(hit.collider.gameObject.layer);
        }
        else
        {
            return "Air"; // Hiçbir þeye çarpmadýysa havadayýz
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

        // Yönü belirle ve simetri uygula
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
