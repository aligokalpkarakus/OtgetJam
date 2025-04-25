using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] float drag;


    private Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag; //S�rt�nme ayarlan�r
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
        //O an olunan noktadaki layer�n ismini d�nd�r�r.
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.zero);

        string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

        return layerName;
    }
}
