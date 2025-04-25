using UnityEngine;

public class Entity : MonoBehaviour
{

    public  Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Sürtünme ayarlanýr
    }

    
    public virtual void moveVectorized(Vector2 dir,int force)
    {
        rb.AddForce(dir.normalized * force);
    }

    public virtual void moveImpulse(Vector2 dir, int force)
    {
        rb.AddForce(dir.normalized * force,ForceMode2D.Impulse);
    }

    public virtual void setGravity(int g)
    {
        rb.gravityScale = g;
    }

    public virtual void setDrag(int d)
    {
        rb.linearDamping = d;
    }

    public virtual string checkCurrentLayer()
    {
        //O an olunan noktadaki layerýn ismini döndürür.
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.zero);

        string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

        return layerName;
    }

    public virtual void ApplyForceToTarget(Vector2 targetPosition, float forceAmount)
    {

        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.AddForce(direction * forceAmount);
    }

    public virtual void updateGravity()
    {
        string layerName = checkCurrentLayer();

        if (layerName == "Water")//Water ise gravity 0 olcak 
        {
            setGravity(0);
            setDrag(2);
        }

        else if (layerName == "Air")
        {
            setGravity(1);
            setDrag(1);
        }
    }

}
