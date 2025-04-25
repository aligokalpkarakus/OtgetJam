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
}
