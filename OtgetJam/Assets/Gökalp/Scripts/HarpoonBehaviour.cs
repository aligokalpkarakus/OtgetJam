using UnityEngine;

public class HarpoonBehaviour : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 5f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;  // Ucunu yönlendir
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);  // Uç kýsmý ileri gider

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
