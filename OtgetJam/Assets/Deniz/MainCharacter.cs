using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCharacter : Entity
{
    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] float dash_speed;
    [SerializeField] float ruzgar_speed;
    [SerializeField] float coolDownPeriod;

    [Header("Trash Escape System")]
    [SerializeField] KeyCode escapeKey = KeyCode.Space;
    [SerializeField] int requiredPresses = 10;
    [SerializeField] float escapeTimeLimit = 5f;

    [Header("Dash Visuals (UI)")]
    [SerializeField] private Image dashIcon;
    [SerializeField] private float cooldownFadeAlpha = 0.3f;

    [Header("Trash Escape UI")]
    [SerializeField] private GameObject trashEscapePanel; // Panel GameObject
    [SerializeField] private TextMeshProUGUI countdownText;          // Geri sayým text

    public static Vector2 currentMainCharacterPosition;
    private float nextAvaliableTimeDash = 0f;

    private string currentLayer = "";

    private bool isTrapped = false;
    private int currentPresses = 0;
    private float trapTimer = 0f;
    private GameObject trappingTrash;

    private Color originalDashIconColor;

    protected override void Start()
    {
        base.Start();

        if (dashIcon != null)
            originalDashIconColor = dashIcon.color;

        if (trashEscapePanel != null)
            trashEscapePanel.SetActive(false); // Baþta kapalý
    }

    protected override void Update()
    {
        base.Update();

        currentMainCharacterPosition = base.rb.position;
        currentLayer = base.checkCurrentLayer();
        base.updateGravity();

        UpdateDashVisuals();

        if (isTrapped)
        {
            HandleTrashEscape();
            return;
        }

        Vector2 move_v2 = Vector2.zero;

        if (currentLayer == "Water")
        {
            if (Input.GetKey(KeyCode.W)) move_v2 += Vector2.up;
            if (Input.GetKey(KeyCode.S)) move_v2 += Vector2.down;
            if (Input.GetKey(KeyCode.D)) move_v2 += Vector2.right;
            if (Input.GetKey(KeyCode.A)) move_v2 += Vector2.left;

            if (move_v2 != Vector2.zero)
                move_v2 = move_v2.normalized;

            if (Input.GetKey(KeyCode.LeftShift) && Time.time >= nextAvaliableTimeDash)
                dash(move_v2);
            else
                base.moveVectorized(move_v2, this.speed * Time.deltaTime);
        }
        else if (currentLayer == "Ruzgar")
        {
            SoundManager.Instance.PlaySound(5);
            base.moveImpulse(Vector2.right, this.ruzgar_speed * Time.deltaTime);
        }
    }

    private void dash(Vector2 dir)
    {
        SoundManager.Instance.PlaySound(1);
        base.moveImpulse(dir, this.dash_speed);
        nextAvaliableTimeDash = Time.time + coolDownPeriod;
        Debug.Log("Dashed!");
    }

    private void UpdateDashVisuals()
    {
        if (dashIcon != null)
        {
            if (Time.time < nextAvaliableTimeDash)
            {
                Color fadedColor = originalDashIconColor;
                fadedColor.a = cooldownFadeAlpha;
                dashIcon.color = fadedColor;
            }
            else
            {
                dashIcon.color = originalDashIconColor;
            }
        }
    }

    private void HandleTrashEscape()
    {
        trapTimer -= Time.deltaTime;

        UpdateTrashEscapeUI();

        if (trapTimer <= 0)
        {
            Debug.Log("Karakter çöpten kurtulamadý!");
            CloseTrashEscapeUI();
            Die();
            return;
        }

        if (Input.GetKeyDown(escapeKey))
        {
            currentPresses++;
            SoundManager.Instance.PlaySound(9);
            Debug.Log("Tuþa basýldý! " + currentPresses + "/" + requiredPresses);

            if (currentPresses >= requiredPresses)
            {
                EscapeFromTrash();
            }
        }
    }

    private void UpdateTrashEscapeUI()
    {
        if (trashEscapePanel != null)
        {
            if (!trashEscapePanel.activeSelf)
                trashEscapePanel.SetActive(true);

            if (countdownText != null)
                countdownText.text = Mathf.Ceil(trapTimer).ToString("0");

        }
    }

    private void CloseTrashEscapeUI()
    {
        if (trashEscapePanel != null)
            trashEscapePanel.SetActive(false);
    }

    private void EscapeFromTrash()
    {
        isTrapped = false;
        if (trappingTrash != null)
        {
            Destroy(trappingTrash);
        }
        Debug.Log("Karakter çöpten kurtuldu!");
        CloseTrashEscapeUI();
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash") && !isTrapped)
        {
            isTrapped = true;
            trappingTrash = other.gameObject;
            trapTimer = escapeTimeLimit;
            currentPresses = 0;
            Debug.Log("Karakter çöpe yakalandý! " + escapeKey + " tuþuna hýzlýca " + requiredPresses + " kez bas!");
        }
    }
}
