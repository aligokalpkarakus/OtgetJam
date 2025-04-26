using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float smoothTime = 0.3f; // Gecikme süresi
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Kamera z'si genelde -10 olur

    private Vector3 velocity = Vector3.zero;

    void Update()
    {


        // Hedef pozisyon
        Vector3 targetPosition = new Vector3(
            MainCharacter.currentMainCharacterPosition.x + offset.x,
            MainCharacter.currentMainCharacterPosition.y + offset.y,
            -10f
        );

        // Kamera anýnda pozisyona sýçrar
        transform.position = targetPosition;


    }




}
