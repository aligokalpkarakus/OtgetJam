using UnityEngine;

public class CameraBoundsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D boundaryBox; // Sýnýrlarý tanýmlayan Box Collider

    private Camera cam;
    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (boundaryBox == null)
        {
            Debug.LogError("Lütfen kamera sýnýrlarý için bir BoxCollider2D atayýn!");
            return;
        }

        // Kamera boyutlarýný hesapla
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.orthographicSize * cam.aspect;
    }

    private void LateUpdate()
    {
        if (boundaryBox == null) return;

        // Kamera merkezini sýnýrlar içinde tut
        float minX = boundaryBox.bounds.min.x + camHalfWidth;
        float maxX = boundaryBox.bounds.max.x - camHalfWidth;
        float minY = boundaryBox.bounds.min.y + camHalfHeight;
        float maxY = boundaryBox.bounds.max.y - camHalfHeight;

        // Mevcut kamera pozisyonunu al
        Vector3 pos = transform.position;

        // Kameraya sýnýrlamalarý uygula
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Kamera pozisyonunu güncelle
        transform.position = pos;
    }
}