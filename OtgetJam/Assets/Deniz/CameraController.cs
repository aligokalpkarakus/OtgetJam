using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float smoothTime = 0.3f; // Gecikme s�resi
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Kamera z'si genelde -10 olur

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {

        Vector3 targetPosition = new Vector3(
             MainCharacter.currentMainCharacterPosition.x + offset.x,
            MainCharacter.currentMainCharacterPosition.y + offset.y,
            -10f // Kamera z'si sabit kalmal� 2D i�in
        );

        // Yumu�ak takip
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }




}
