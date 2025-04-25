using UnityEngine;

public class NetBehaviour : MonoBehaviour
{
    public float fallSpeed = 1f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
}
