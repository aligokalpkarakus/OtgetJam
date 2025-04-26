using UnityEngine;
using UnityEngine.SceneManagement; // Eðer ölümde sahneyi yeniden baþlatmak için

public class MainCharacter : Entity
{
    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] float dash_speed;
    [SerializeField] float ruzgar_speed;
    [SerializeField] float coolDownPeriod;

    [Header("Trash Escape System")]
    [SerializeField] KeyCode escapeKey = KeyCode.Space; // Kurtulmak için basýlacak tuþ
    [SerializeField] int requiredPresses = 10; // Kurtulmak için gerekli basma sayýsý
    [SerializeField] float escapeTimeLimit = 5f; // Kurtulma süresi

    public static Vector2 currentMainCharacterPosition;
    private float nextAvaliableTimeDash = 0f;

    private string currentLayer = "";

    // Çöpe yakalanma deðiþkenleri
    private bool isTrapped = false; // Karakter çöpe yakalandý mý?
    private int currentPresses = 0; // Mevcut basma sayýsý
    private float trapTimer = 0f; // Yakalanma zamanlayýcýsý
    private GameObject trappingTrash; // Yakalayan çöp nesnesiz

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        currentMainCharacterPosition = base.rb.position;

        // Eðer çöpe yakalandýysa, kaçýþ sistemini çalýþtýr
        

        Vector2 move_v2 = Vector2.zero;
        currentLayer = base.checkCurrentLayer(); // Þu an bulunan layer
        base.updateGravity();//suda ve havada olma durumuna göre gravity scale'ýný günceller.

        if (isTrapped)
        {
            HandleTrashEscape();
            return; // Hareket etmesini engelle
        }
        else
        {
            if (currentLayer == "Water")
            {
                if (Input.GetKey(KeyCode.W))
                {
                    move_v2 += Vector2.up;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    move_v2 += Vector2.down;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    move_v2 += Vector2.right;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    move_v2 += Vector2.left;
                }

                // normalize ediyoz ki çaprazda hýzlý gitmesin
                if (move_v2 != Vector2.zero)
                    move_v2 = move_v2.normalized;

                if (Input.GetKey(KeyCode.LeftShift) && Time.time >= nextAvaliableTimeDash)
                    dash(move_v2);
                else
                    base.moveVectorized(move_v2, this.speed * Time.deltaTime);
            }
            else if (currentLayer == "Ruzgar")
            {
                Debug.Log("RUZGAR");
                base.moveImpulse(Vector2.right, this.ruzgar_speed * Time.deltaTime);
            }
        }

        
    }

    private void dash(Vector2 dir)
    {
        base.moveImpulse(dir, this.dash_speed);
        nextAvaliableTimeDash = Time.time + coolDownPeriod;
        Debug.Log("dashed");
    }

    // Çöpten kurtulma yönetimi
    private void HandleTrashEscape()
    {
        trapTimer -= Time.deltaTime;

        // Süre doldu mu kontrol et
        if (trapTimer <= 0)
        {
            // Karakter kurtulamadý - Oyuncu öldü
            Debug.Log("Karakter çöpten kurtulamadý!");
            Die();
            return;
        }

        // Tuþa basma kontrolü
        if (Input.GetKeyDown(escapeKey))
        {
            currentPresses++;
            Debug.Log("Tuþa basýldý! " + currentPresses + "/" + requiredPresses);

            // Yeterli sayýda bastý mý?
            if (currentPresses >= requiredPresses)
            {
                // Karakter kurtuldu
                EscapeFromTrash();
            }
        }
    }

    private void EscapeFromTrash()
    {
        isTrapped = false;
        if (trappingTrash != null)
        {
            // Ýsterseniz çöp nesnesini yok edebilir veya baþka bir animasyon/etki tetikleyebilirsiniz
            Destroy(trappingTrash);
        }
        Debug.Log("Karakter çöpten kurtuldu!");
    }

    // Ölüm fonksiyonu - istenirse HealthSystem'den alýnabilir
    private void Die()
    {
        // Sahneyi yeniden baþlat
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Veya: istenirse ölüm animasyonu gösterip sonra yeniden baþlatabilirsiniz
        // StartCoroutine(RestartAfterDelay(1.5f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash") && !isTrapped)
        {
            // Çöpe yakalandý
            isTrapped = true;
            trappingTrash = other.gameObject;
            trapTimer = escapeTimeLimit;
            currentPresses = 0;
            Debug.Log("Karakter çöpe yakalandý! Kurtulmak için " + escapeKey + " tuþuna hýzlýca " + requiredPresses + " kez bas!");
        }

        // Diðer trigger fonksiyonlarý buraya ekleyebilirsiniz
        // Örneðin: Harpoon, Net vs.
    }
}