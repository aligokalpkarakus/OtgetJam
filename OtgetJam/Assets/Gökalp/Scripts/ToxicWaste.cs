using UnityEngine;

public class ToxicWaste : MonoBehaviour
{
    // Zehirli atýk nesnesine eklenecek kod

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("içerde");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Dýþarda");
        }
    }
}