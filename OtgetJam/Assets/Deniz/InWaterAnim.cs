using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float floatStrength = 0.5f; // Ne kadar yukarý aþaðý oynayacak
    [SerializeField] private float floatSpeed = 1f;      // Ne kadar hýzlý oynayacak

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatStrength;
    }
}
