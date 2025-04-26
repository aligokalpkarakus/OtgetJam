using UnityEngine;

public class CameraBoundsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D boundaryBox; // S�n�rlar� tan�mlayan Box Collider

    private Camera cam;
    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (boundaryBox == null)
        {
            Debug.LogError("L�tfen kamera s�n�rlar� i�in bir BoxCollider2D atay�n!");
            return;
        }

        // Kamera boyutlar�n� hesapla
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.orthographicSize * cam.aspect;
    }

    private void LateUpdate()
    {
        if (boundaryBox == null) return;

        // Kamera merkezini s�n�rlar i�inde tut
        float minX = boundaryBox.bounds.min.x + camHalfWidth;
        float maxX = boundaryBox.bounds.max.x - camHalfWidth;
        float minY = boundaryBox.bounds.min.y + camHalfHeight;
        float maxY = boundaryBox.bounds.max.y - camHalfHeight;

        // Mevcut kamera pozisyonunu al
        Vector3 pos = transform.position;

        // Kameraya s�n�rlamalar� uygula
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Kamera pozisyonunu g�ncelle
        transform.position = pos;
    }
}