using UnityEngine;

public class FishControllerGk : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Çöp yakalanma sistemi değişkenleri
    public KeyCode escapeKey = KeyCode.Space; // Kurtulmak için basılacak tuş
    public int requiredPresses = 10; // Kurtulmak için gerekli basma sayısı
    public float escapeTimeLimit = 5f; // Kurtulma süresi

    private bool isTrapped = false; // Balık çöpe yakalandı mı?
    private int currentPresses = 0; // Mevcut basma sayısı
    private float trapTimer = 0f; // Yakalanma zamanlayıcısı
    private GameObject trappingTrash; // Yakalayan çöp nesnesi

    void Update()
    {
        if (!isTrapped)
        {
            // Normal hareket kodu
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical);
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Çöpe yakalandığında kaçma mekanizması
            trapTimer -= Time.deltaTime;

            // Süre doldu mu kontrol et
            if (trapTimer <= 0)
            {
                // Balık kurtulamadı
                Destroy(gameObject);
                Debug.Log("Balık çöpten kurtulamadı!");
                return;
            }

            // Tuşa basma kontrolü
            if (Input.GetKeyDown(escapeKey))
            {
                currentPresses++;
                Debug.Log("Tuşa basıldı! " + currentPresses + "/" + requiredPresses);

                // Yeterli sayıda bastı mı?
                if (currentPresses >= requiredPresses)
                {
                    // Balık kurtuldu
                    isTrapped = false;
                    if (trappingTrash != null)
                        Destroy(trappingTrash);
                    Debug.Log("Balık çöpten kurtuldu!");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Harpoon") || other.CompareTag("Net"))
        {
            // Orijinal yakalanma kodu
            Destroy(gameObject);
            Destroy(other.gameObject);
            Debug.Log("Balık yakalandı!");
            // Örn: can azalt, oyunu bitir, vs.
        }
        else if (other.CompareTag("Trash") && !isTrapped)
        {
            // Çöpe yakalandı
            isTrapped = true;
            trappingTrash = other.gameObject;
            trapTimer = escapeTimeLimit;
            currentPresses = 0;
            Debug.Log("Balık çöpe yakalandı! Kurtulmak için " + escapeKey + " tuşuna hızlıca " + requiredPresses + " kez bas!");
        }
    }
}