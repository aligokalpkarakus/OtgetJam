using UnityEngine;

public class HarpoonBehaviour : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 5f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;  // Ucunu y�nlendir
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);  // U� k�sm� ileri gider

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
