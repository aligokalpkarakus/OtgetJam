using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float speed = 2f;
    public float moveRange = 5f;
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Hareket
        if (movingRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Sýnýr kontrolü ve yön deðiþtirme
        if (transform.position.x > startPos.x + moveRange)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x < startPos.x - moveRange)
        {
            movingRight = true;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // X yönünü ters çevirir
        transform.localScale = localScale;
    }
}
