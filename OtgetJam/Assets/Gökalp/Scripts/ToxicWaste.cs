using UnityEngine;

public class ToxicWaste : MonoBehaviour
{
    // Zehirli at�k nesnesine eklenecek kod

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("i�erde");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("D��arda");
        }
    }
}