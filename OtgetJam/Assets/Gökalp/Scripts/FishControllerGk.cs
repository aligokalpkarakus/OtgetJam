using UnityEngine;

public class FishControllerGk : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Harpoon") || other.CompareTag("Net"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            Debug.Log("Balýk yakalandý!");
            // Örn: can azalt, oyunu bitir, vs.
        }
    }

}
