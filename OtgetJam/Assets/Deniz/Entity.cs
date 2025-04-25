using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] float drag;


    private Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag; //Sürtünme ayarlanýr
    }

    
    public virtual void moveVectorized(Vector2 dir,int speed)
    {
        rb.AddForce(dir.normalized * speed);
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
}
