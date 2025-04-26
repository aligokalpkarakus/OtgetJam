using UnityEngine;
using UnityEngine.SceneManagement; // E�er �l�mde sahneyi yeniden ba�latmak i�in

public class MainCharacter : Entity
{
    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] float dash_speed;
    [SerializeField] float ruzgar_speed;
    [SerializeField] float coolDownPeriod;

    [Header("Trash Escape System")]
    [SerializeField] KeyCode escapeKey = KeyCode.Space; // Kurtulmak i�in bas�lacak tu�
    [SerializeField] int requiredPresses = 10; // Kurtulmak i�in gerekli basma say�s�
    [SerializeField] float escapeTimeLimit = 5f; // Kurtulma s�resi

    public static Vector2 currentMainCharacterPosition;
    private float nextAvaliableTimeDash = 0f;

    private string currentLayer = "";

    // ��pe yakalanma de�i�kenleri
    private bool isTrapped = false; // Karakter ��pe yakaland� m�?
    private int currentPresses = 0; // Mevcut basma say�s�
    private float trapTimer = 0f; // Yakalanma zamanlay�c�s�
    private GameObject trappingTrash; // Yakalayan ��p nesnesiz

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        currentMainCharacterPosition = base.rb.position;

        // E�er ��pe yakaland�ysa, ka��� sistemini �al��t�r
        

        Vector2 move_v2 = Vector2.zero;
        currentLayer = base.checkCurrentLayer(); // �u an bulunan layer
        base.updateGravity();//suda ve havada olma durumuna g�re gravity scale'�n� g�nceller.

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

                // normalize ediyoz ki �aprazda h�zl� gitmesin
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

    // ��pten kurtulma y�netimi
    private void HandleTrashEscape()
    {
        trapTimer -= Time.deltaTime;

        // S�re doldu mu kontrol et
        if (trapTimer <= 0)
        {
            // Karakter kurtulamad� - Oyuncu �ld�
            Debug.Log("Karakter ��pten kurtulamad�!");
            Die();
            return;
        }

        // Tu�a basma kontrol�
        if (Input.GetKeyDown(escapeKey))
        {
            currentPresses++;
            Debug.Log("Tu�a bas�ld�! " + currentPresses + "/" + requiredPresses);

            // Yeterli say�da bast� m�?
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
            // �sterseniz ��p nesnesini yok edebilir veya ba�ka bir animasyon/etki tetikleyebilirsiniz
            Destroy(trappingTrash);
        }
        Debug.Log("Karakter ��pten kurtuldu!");
    }

    // �l�m fonksiyonu - istenirse HealthSystem'den al�nabilir
    private void Die()
    {
        // Sahneyi yeniden ba�lat
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Veya: istenirse �l�m animasyonu g�sterip sonra yeniden ba�latabilirsiniz
        // StartCoroutine(RestartAfterDelay(1.5f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash") && !isTrapped)
        {
            // ��pe yakaland�
            isTrapped = true;
            trappingTrash = other.gameObject;
            trapTimer = escapeTimeLimit;
            currentPresses = 0;
            Debug.Log("Karakter ��pe yakaland�! Kurtulmak i�in " + escapeKey + " tu�una h�zl�ca " + requiredPresses + " kez bas!");
        }

        // Di�er trigger fonksiyonlar� buraya ekleyebilirsiniz
        // �rne�in: Harpoon, Net vs.
    }
}